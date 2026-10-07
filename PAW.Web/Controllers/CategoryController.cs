using Microsoft.AspNetCore.Mvc;
using PAW.Models.DTO;
using PAW.Web.Models;
using PAW.Web.Services;

namespace PAW.Web.Controllers
{
    public class CategoryController : Controller
    {
        private const int PageSize = 25;

        private readonly ICategoryService _service;
        private readonly IProductService _productService;
        private readonly ILogger<CategoryController> _logger;

        public CategoryController(ICategoryService service, IProductService productService, ILogger<CategoryController> logger)
        {
            _service = service;
            _productService = productService;
            _logger = logger;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            var items = await _service.GetAllAsync();
            var paged = PagedResult<CategoryDTO>.Create(items.OrderBy(x => x.CategoryId), page, PageSize);
            return View(paged);
        }

        // Returns a partial view (no full page) that the Index screen shows inside a modal window.
        public async Task<IActionResult> Details(int id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item is null) return NotFound();

            var products = (await _productService.GetAllAsync())
                .Where(p => p.CategoryId == id)
                .OrderBy(p => p.ProductId)
                .ToList();

            return PartialView("_CategoryDetailsWithProducts", new DetailsWithProductsViewModel<CategoryDTO> { Item = item, Products = products });
        }

        public IActionResult Create()
        {
            return View(new CategoryDTO());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryDTO model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            model.ModifiedBy = "PAW.Web";

            try
            {
                await _service.CreateAsync(model);
                TempData["Success"] = "Category created successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating Category");
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
        public async Task<IActionResult> Edit(int id, CategoryDTO model)
        {
            model.CategoryId = id;

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

                TempData["Success"] = "Category updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating Category {Id}", id);
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
                    TempData["Success"] = "Category deleted successfully.";
                else
                    TempData["Error"] = "The record could not be deleted.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting Category {Id}", id);
                TempData["Error"] = "The record could not be deleted. It may be referenced by other records.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
