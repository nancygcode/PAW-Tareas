using Microsoft.AspNetCore.Mvc;
using PAW.Models.DTO;
using PAW.Web.Models;
using PAW.Web.Services;

namespace PAW.Web.Controllers
{
    public class PawTaskController : Controller
    {
        private const int PageSize = 25;

        private readonly IPawTaskService _service;
        private readonly ILogger<PawTaskController> _logger;

        public PawTaskController(IPawTaskService service, ILogger<PawTaskController> logger)
        {
            _service = service;
            _logger = logger;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            var items = await _service.GetAllAsync();
            var paged = PagedResult<PawTaskDTO>.Create(items.OrderBy(x => x.Id), page, PageSize);
            return View(paged);
        }

        // Returns a partial view (no full page) that the Index screen shows inside a modal window.
        public async Task<IActionResult> Details(int id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item is null) return NotFound();

            return PartialView("_PawTaskDetails", item);
        }

        public IActionResult Create()
        {
            return View(new PawTaskDTO());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PawTaskDTO model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            model.ModifiedBy = "PAW.Web";

            try
            {
                await _service.CreateAsync(model);
                TempData["Success"] = "Task created successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating Task");
                ModelState.AddModelError(string.Empty, "The record could not be saved. Please verify the data and try again.");
                return View(model);
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item is null) return NotFound();

            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PawTaskDTO model)
        {
            model.Id = id;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            model.ModifiedBy = "PAW.Web";

            try
            {
                var updated = await _service.UpdateAsync(id, model);
                if (!updated)
                {
                    ModelState.AddModelError(string.Empty, "No changes were saved.");
                    return View(model);
                }

                TempData["Success"] = "Task updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating Task {Id}", id);
                ModelState.AddModelError(string.Empty, "The record could not be updated. Please verify the data and try again.");
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
                    TempData["Success"] = "Task deleted successfully.";
                else
                    TempData["Error"] = "The record could not be deleted.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting Task {Id}", id);
                TempData["Error"] = "The record could not be deleted. It may be referenced by other records.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
