using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Warehouse.Core.Entities;
using Warehouse.Core.Interfaces;
using Warehouse.Infrastructure.DataAccess;
using Warehouse.Web.ViewModels.Product;
//using Warehouse.Web.ViewModels;

namespace Warehouse.Web.Controllers
{
    public class ProductsController : Controller
    {
        //private readonly DataContext _context;
        private readonly IProductLogic _productLogic;

        public ProductsController(DataContext context, IProductLogic productLogic)
        {
            //_context = context;
            _productLogic = productLogic;
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

            //var product = await _context.Products
            //    .FirstOrDefaultAsync(m => m.Id == id);
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
        public IActionResult Create()
        {
            var productViewModel = new ProductViewModel();
            GetCategoriesFromDb(productViewModel);
            return View(productViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductViewModel productViewModel)
        {
            if (ModelState.IsValid == false)
            {
                GetCategoriesFromDb(productViewModel);
                return View(productViewModel);
            }
            var product = new Product()
            {
                Name = productViewModel.Name,
                Price = productViewModel.Price,
                Description = productViewModel.Description,
                CategoryId = productViewModel.Category,
            };
            //await _context.AddAsync(product);
            //await _context.SaveChangesAsync();
            await _productLogic.AddAsync(product);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var product = await _context.Products.FindAsync(id);
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
            GetCategoriesFromDb(productViewModel);
            return View(productViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductViewModel productViewModel)
        {
            if (ModelState.IsValid == false)
            {
                GetCategoriesFromDb(productViewModel);
                return View(productViewModel);
            }

            var result = await _productLogic.GetByIdAsync(productViewModel.Id);
            result.Value.Name = productViewModel.Name;
            result.Value.Price = productViewModel.Price;
            result.Value.Description = productViewModel.Description;
            result.Value.CategoryId = productViewModel.Category;

            await _productLogic.UpdateAsync(result.Value);
            //_context.Update(result);
            //await _context.SaveChangesAsync();
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
            var result = await _productLogic.GetByIdAsync(id);
            if (result.Success == false)
            {
                return RedirectToAction(nameof(Index));
            }
            await _productLogic.DeleteAsync(id);
            //_context.Products.Remove(result);
            //await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        //private void GetCategoriesFromDb(ProductViewModel viewModel)
        //{
        //    var result = _context.Categories
        //        .OrderBy(c => c.Name)
        //        .Select(c => 
        //        new ViewModels.SelectItemViewModel()
        //        {
        //            Display = c.Name,
        //            Value = c.Id.ToString(),
        //        }
        //        ).ToList();
        //    viewModel.AvailableCategories = result;
        //}

        private async void GetCategoriesFromDb(ProductViewModel viewModel)
        {
            var result = await _productLogic.GetCategories();
            if (result.Success == false)
            {
                NotFound();
            }
            var categoriesList = result.Value
                .Select(c =>
                new ViewModels.SelectItemViewModel()
                {
                    Display = c,
                    Value = c,
                }
                ).ToList();
            viewModel.AvailableCategories = categoriesList;

        }
    }
}
