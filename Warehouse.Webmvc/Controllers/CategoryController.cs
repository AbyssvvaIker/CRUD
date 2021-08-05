using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Warehouse.Core.Entities;
using Warehouse.Infrastructure.DataAccess;
using Warehouse.WebMvc.ViewModels;

namespace Warehouse.WebMvc.Controllers
{
    public class CategoryController : Controller
    {
        private readonly DataContext _context;

        public CategoryController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IndexViewModel viewModel = new IndexViewModel();
            var categoriesContext = await _context.Categories.ToListAsync();
            foreach (var item in categoriesContext)
            {
                viewModel.Categories.Add(
                    new IndexItemViewModel { 
                        Id = item.Id, 
                        Name = item.Name }
                    );
            }
            return View(viewModel.Categories);
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            CategoryViewModel categoryViewModel = new CategoryViewModel { 
                Id = category.Id,
                Name = category.Name 
            };
            return View(categoryViewModel);
        }

        [HttpGet]
        public IActionResult Create()
        {
            CategoryViewModel categoryViewModel = new CategoryViewModel();
            return View(categoryViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryViewModel categoryViewModel)
        {
            if (ModelState.IsValid == false)
            {
                return View(categoryViewModel);
            }
            Category category = new Category
            {
                Name = categoryViewModel.Name, //Should I use constructor in Category class?
            };
            _context.Add(category);
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

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            CategoryViewModel categoryViewModel = new CategoryViewModel { 
                Id = category.Id,
                Name = category.Name 
            };
            return View(categoryViewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CategoryViewModel categoryViewModel)
        {

            if (ModelState.IsValid == false)
            {

                return View(categoryViewModel);

            }

            Category category =await _context.Categories.FindAsync(categoryViewModel.Id);
            category.Name = categoryViewModel.Name;

            _context.Update(category);
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
            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            CategoryViewModel categoryViewModel = new CategoryViewModel { 
                Id = category.Id,
                Name = category.Name 
            };
            return View(categoryViewModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var category = await _context.Categories.FindAsync(id);
            if(category == null)
            {
                return RedirectToAction(nameof(Index));
            }
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
