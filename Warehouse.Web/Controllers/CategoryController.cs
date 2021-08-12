using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Warehouse.Core.Entities;
using Warehouse.Infrastructure.DataAccess;
using Warehouse.Web.ViewModels.Category;
using Warehouse.Core.Interfaces;

namespace Warehouse.Web.Controllers
{
    public class CategoryController : Controller
    {
        //private readonly DataContext _context;
        private readonly ICategoryLogic _categoryLogic; 

        public CategoryController(DataContext context, ICategoryLogic categoryLogic)
        {
            //_context = context;
            _categoryLogic = categoryLogic;

        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {

            //var result = await _context.Categories.ToListAsync();
            var result = await _categoryLogic.GetAllActiveAsync();

            var viewModel = new IndexViewModel()
            {
                Categories = result.Value.Select(cat =>
                   new IndexItemViewModel()
                   {
                       Id = cat.Id,
                       Name = cat.Name,
                   }
                ).ToList()
            };
            return View(viewModel.Categories);
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            //var category = await _context.Categories
            //    .FirstOrDefaultAsync(m => m.Id == id);
            var result = await _categoryLogic.GetByIdAsync((Guid)id);
            if (result.Success == false)
            {
                return NotFound();
            }
            var categoryViewModel = new CategoryViewModel
            {
                Id = result.Value.Id,
                Name = result.Value.Name
            };
            return View(categoryViewModel);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var categoryViewModel = new CategoryViewModel();
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
            var category = new Category
            {
                Name = categoryViewModel.Name, //Should I use constructor in Category class?
            };
            //await _context.AddAsync(category);
            //await _context.SaveChangesAsync();
            var result = await _categoryLogic.AddAsync(category);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var category = await _context.Categories.FindAsync(id);
            var result = await _categoryLogic.GetByIdAsync((Guid)id);
            if (result.Success == false)
            {
                return NotFound();
            }
            var categoryViewModel = new CategoryViewModel
            {
                Id = result.Value.Id,
                Name = result.Value.Name
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

            //Category category = await _context.Categories.FindAsync(categoryViewModel.Id);
            var result = await _categoryLogic.GetByIdAsync(categoryViewModel.Id);
            result.Value.Name = categoryViewModel.Name;

            await _categoryLogic.UpdateAsync(result.Value);

            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            //var category = await _context.Categories
            //    .FirstOrDefaultAsync(m => m.Id == id);
            var result = await _categoryLogic.GetByIdAsync((Guid)id);
            if (result.Success == false)
            {
                return NotFound();
            }
            var categoryViewModel = new CategoryViewModel
            {
                Id = result.Value.Id,
                Name = result.Value.Name,
            };
            return View(categoryViewModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            //var category = await _context.Categories.FindAsync(id);
            var result = await _categoryLogic.DeleteAsync(id);
            if (result.Success == false)
            {
                return RedirectToAction(nameof(Index));
            }
            //_context.Categories.Remove(category);
            //await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
