using Microsoft.AspNetCore.Mvc;
using PAW.Models.DTO;
using PAW.Web.Models;
using PAW.Web.Services;

namespace PAW.Web.Controllers
{
    public class SupplierController : Controller
    {
        private const int PageSize = 25;

        private readonly ISupplierService _service;
        private readonly IProductService _productService;
        private readonly ILogger<SupplierController> _logger;

        public SupplierController(ISupplierService service, IProductService productService, ILogger<SupplierController> logger)
        {
            _service = service;
            _productService = productService;
            _logger = logger;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            var items = await _service.GetAllAsync();
            var paged = PagedResult<SupplierDTO>.Create(items.OrderBy(x => x.SupplierId), page, PageSize);
            return View(paged);
        }

        // Returns a partial view (no full page) that the Index screen shows inside a modal window.
        public async Task<IActionResult> Details(int id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item is null) return NotFound();

            var products = (await _productService.GetProductsAsync())
                .Where(p => p.SupplierId == id)
                .OrderBy(p => p.ProductId)
                .ToList();

            return PartialView("_SupplierDetailsWithProducts", new DetailsWithProductsViewModel<SupplierDTO> { Item = item, Products = products });
        }

        public IActionResult Create()
        {
            return View(new SupplierDTO());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SupplierDTO model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            model.ModifiedBy = "PAW.Web";

            try
            {
                await _service.CreateAsync(model);
                TempData["Success"] = "Supplier created successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating Supplier");
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
        public async Task<IActionResult> Edit(int id, SupplierDTO model)
        {
            model.SupplierId = id;

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

                TempData["Success"] = "Supplier updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating Supplier {Id}", id);
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
                    TempData["Success"] = "Supplier deleted successfully.";
                else
                    TempData["Error"] = "The record could not be deleted.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting Supplier {Id}", id);
                TempData["Error"] = "The record could not be deleted. It may be referenced by other records.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
