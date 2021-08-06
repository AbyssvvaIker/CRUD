using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Warehouse.Core.Entities;
using Warehouse.Infrastructure.DataAccess;
using Warehouse.Web.ViewModels.Product;
//using Warehouse.Web.ViewModels;

namespace Warehouse.Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly DataContext _context;

        public ProductsController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var result = await _context.Products.ToListAsync();
            var viewModel = new IndexViewModel()
            {
                Products = result.Select(prod =>
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

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            var productViewModel = new ProductViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Description = product.Description,
                Category = product.CategoryId,
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
            await _context.AddAsync(product);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            var productViewModel = new ProductViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Description = product.Description,
                Category = product.CategoryId,
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

            var product = await _context.Products.FindAsync(productViewModel.Id);
            product.Name = productViewModel.Name;
            product.Price = productViewModel.Price;
            product.Description = productViewModel.Description;
            product.CategoryId = productViewModel.Category;
            
            _context.Update(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            var productViewModel = new ProductViewModel()
            {
                Name = product.Name,
                Price = product.Price,
                Description = product.Description,
                Category = product.CategoryId,
            };
            return View(productViewModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return RedirectToAction(nameof(Index));
            }
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private void GetCategoriesFromDb(ProductViewModel viewModel)
        {
            var result = _context.Categories
                .OrderBy(c => c.Name)
                .Select(c => 
                new ViewModels.SelectItemViewModel()
                {
                    Display = c.Name,
                    Value = c.Id.ToString(),
                }
                ).ToList();
            viewModel.AvailableCategories = result;
        }
    }
}
