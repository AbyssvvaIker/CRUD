using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Warehouse.Core.Entities;
using Warehouse.Infrastructure.DataAccess;
using Warehouse.Web.ViewModels.Category;
using Warehouse.Core.Interfaces;
using Warehouse.Web.ExtensionMethods;

namespace Warehouse.Web.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryLogic _categoryLogic; 

        public CategoryController(ICategoryLogic categoryLogic)
        {
            _categoryLogic = categoryLogic;

        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
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
                Name = categoryViewModel.Name,
            };
            var result = await _categoryLogic.AddAsync(category);
            if(result.Success == false)
            {
                result.AddErrorToModelState(ModelState);
                return View(categoryViewModel);
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
            var result = await _categoryLogic.GetByIdAsync(categoryViewModel.Id);
            
            if(result.Success == false)
            {
                result.AddErrorToModelState(ModelState);
                return View(categoryViewModel);
            }
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
            var result = await _categoryLogic.GetByIdAsync((Guid)id);
            if (result.Success == false)
            {
                return RedirectToAction(nameof(Index));
            }
            await _categoryLogic.DeleteAsync(result.Value);

            return RedirectToAction(nameof(Index));
        }

    }
}
