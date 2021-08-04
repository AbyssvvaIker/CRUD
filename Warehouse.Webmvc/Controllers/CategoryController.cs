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
                viewModel.Categories.Add(new IndexItemViewModel(item.Id, item.Name));
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
            CategoryViewModel categoryViewModel = new CategoryViewModel(category.Id, category.Name);
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
            CategoryViewModel categoryViewModel = new CategoryViewModel(category.Id, category.Name);
            return View(categoryViewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name")] Category category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid == false)
            {

                return View(category);

            }
            _context.Update(category);
            await _context.SaveChangesAsync();


            if (!CategoryExists(category.Id))
            {
                return NotFound();
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

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var category = await _context.Categories.FindAsync(id);
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(Guid id)
        {
            return _context.Categories.Any(e => e.Id == id);
        }

        private bool CategoryNameExists(Category category, bool caseSensitive = false)
        {
            IQueryable<string> categoryNameQuery = from m in _context.Categories
                                                   orderby m.Name
                                                   select m.Name;
            if (caseSensitive)
            {
                foreach (var item in categoryNameQuery)
                {

                    if (item.Equals(category.Name))
                    {
                        return true;
                    }
                }
            }
            else
            {
                foreach (var item in categoryNameQuery)
                {

                    if (item.ToLower().Equals(category.Name.ToLower()))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
