using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PAW.Models.DTO;
using PAW.Web.Models;
using PAW.Web.Services;

namespace PAW.Web.Controllers
{
    public class UserController : Controller
    {
        private const int PageSize = 25;

        private readonly IUserService _service;
        private readonly IRoleService _roleService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService service, IRoleService roleService, ILogger<UserController> logger)
        {
            _service = service;
            _roleService = roleService;
            _logger = logger;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            var items = await _service.GetAllAsync();
            var paged = PagedResult<UserDTO>.Create(items.OrderBy(x => x.UserId), page, PageSize);
            return View(paged);
        }

        // Returns a partial view (no full page) that the Index screen shows inside a modal window.
        public async Task<IActionResult> Details(int id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item is null) return NotFound();

            return PartialView("_UserDetails", item);
        }

        public async Task<IActionResult> Create()
        {
            await LoadLookupsAsync();
            return View(new UserDTO());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserDTO model)
        {
            if (!ModelState.IsValid)
            {
                await LoadLookupsAsync();
                return View(model);
            }

            model.ModifiedBy = "PAW.Web";
            model.LastModifiedBy = "PAW.Web";

            try
            {
                await _service.CreateAsync(model);
                TempData["Success"] = "User created successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating User");
                ModelState.AddModelError(string.Empty, "The record could not be saved. Please verify the data and try again.");
                await LoadLookupsAsync();
                return View(model);
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item is null) return NotFound();

            await LoadLookupsAsync();
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UserDTO model)
        {
            model.UserId = id;

            if (!ModelState.IsValid)
            {
                await LoadLookupsAsync();
                return View(model);
            }

            model.ModifiedBy = "PAW.Web";
            model.LastModifiedBy = "PAW.Web";

            try
            {
                var updated = await _service.UpdateAsync(id, model);
                if (!updated)
                {
                    ModelState.AddModelError(string.Empty, "No changes were saved.");
                    await LoadLookupsAsync();
                    return View(model);
                }

                TempData["Success"] = "User updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating User {Id}", id);
                ModelState.AddModelError(string.Empty, "The record could not be updated. Please verify the data and try again.");
                await LoadLookupsAsync();
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var deleted = await _service.DeleteAsync(id);
                if (deleted)
                    TempData["Success"] = "User deleted successfully.";
                else
                    TempData["Error"] = "The record could not be deleted.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting User {Id}", id);
                TempData["Error"] = "The record could not be deleted. It may be referenced by other records.";
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task LoadLookupsAsync()
        {
            ViewBag.Roles = (await _roleService.GetAllAsync())
                .OrderBy(x => x.RoleId)
                .Select(x => new SelectListItem { Value = x.RoleId.ToString(), Text = x.RoleName ?? $"Role {x.RoleId}" })
                .ToList();
        }
    }
}
