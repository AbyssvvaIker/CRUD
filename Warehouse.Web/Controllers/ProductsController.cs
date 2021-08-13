using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Warehouse.Core.Entities;
using Warehouse.Core.Interfaces;
using Warehouse.Infrastructure.DataAccess;
using Warehouse.Web.Infrastructure.ExtensionMethods;
using Warehouse.Web.ViewModels.Product;

namespace Warehouse.Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductLogic _productLogic;
        private readonly ICategoryLogic _categoryLogic;

        public ProductsController(IProductLogic productLogic, ICategoryLogic categoryLogic)
        {
            _productLogic = productLogic;
            _categoryLogic = categoryLogic;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var result = await _productLogic.GetAllActiveAsync();
            if(result.Success == false)
            {
                return NotFound();
            }
            var viewModel = new IndexViewModel()
            {
                Products = result.Value.Select(prod =>
                new IndexItemViewModel()
                {
                    Id = prod.Id,
                    Name = prod.Name,
                    Description = prod.Description,
                    Price = prod.Price,
                }
                ).ToList()
            };
            return View(viewModel.Products);
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var result = await _productLogic.GetByIdAsync((Guid)id);
            if (result.Success == false)
            {
                return NotFound();
            }
            var productViewModel = new ProductViewModel
            {
                Id = result.Value.Id,
                Name = result.Value.Name,
                Price = result.Value.Price,
                Description = result.Value.Description,
                Category = result.Value.CategoryId,
            };
            return View(productViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var productViewModel = new ProductViewModel();
            await GetCategoriesFromDb(productViewModel);
            return View(productViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductViewModel productViewModel)
        {
            if (ModelState.IsValid == false)
            {
                await GetCategoriesFromDb(productViewModel);
                return View(productViewModel);
            }
            var product = new Product()
            {
                Name = productViewModel.Name,
                Price = productViewModel.Price,
                Description = productViewModel.Description,
                CategoryId = productViewModel.Category,
            };
            var result = await _productLogic.AddAsync(product);
            if(result.Success == false)
            {
                result.AddErrorToModelState(ModelState);
                return View(productViewModel);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var result = await _productLogic.GetByIdAsync((Guid)id);
            if (result.Success == false)
            {
                return NotFound();
            }
            var productViewModel = new ProductViewModel
            {
                Id = result.Value.Id,
                Name = result.Value.Name,
                Price = result.Value.Price,
                Description = result.Value.Description,
                Category = result.Value.CategoryId,
            };
            await GetCategoriesFromDb(productViewModel);
            return View(productViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductViewModel productViewModel)
        {
            if (ModelState.IsValid == false)
            {
                await GetCategoriesFromDb(productViewModel);
                return View(productViewModel);
            }

            var result = await _productLogic.GetByIdAsync(productViewModel.Id);
            if(result.Success == false)
            {
                result.AddErrorToModelState(ModelState);
                return View(productViewModel);
            }
            result.Value.Name = productViewModel.Name;
            result.Value.Price = productViewModel.Price;
            result.Value.Description = productViewModel.Description;
            result.Value.CategoryId = productViewModel.Category;

            result = await _productLogic.UpdateAsync(result.Value);
            if (result.Success == false)
            {
                result.AddErrorToModelState(ModelState);
                return View(productViewModel);
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var result = await _productLogic.GetByIdAsync((Guid)id);
            if (result.Success == false)
            {
                return NotFound();
            }
            var productViewModel = new ProductViewModel()
            {
                Name = result.Value.Name,
                Price = result.Value.Price,
                Description = result.Value.Description,
                Category = result.Value.CategoryId,
            };
            return View(productViewModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            //var result = await _productLogic.GetByIdAsync(id);
            var result = await _productLogic.GetByIdAsync((Guid)id);
            if (result.Success == false)
            {
                return RedirectToAction(nameof(Index));
            }
            await _productLogic.DeleteAsync(result.Value);
            return RedirectToAction(nameof(Index));
        }

        private async Task GetCategoriesFromDb(ProductViewModel viewModel)
        {
            var result = await _categoryLogic.GetAllActiveAsync();
            var categories = result.Value.Select(c =>
                new ViewModels.SelectItemViewModel()
                {
                    Display = c.Name,
                    Value = c.Id.ToString(),
                }
                ).ToList();
            viewModel.AvailableCategories = categories;
            return; 
        }
    }
}
