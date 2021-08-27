using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Warehouse.Core.Entities;
using Warehouse.Infrastructure.DataAccess;
using Warehouse.Web.ViewModels.Category;
using Warehouse.Core.Interfaces;
using Warehouse.Web.Infrastructure.ExtensionMethods;
using AutoMapper;
using System.Collections.Generic;

namespace Warehouse.Web.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryLogic _categoryLogic;

        private readonly IMapper _mapper;

        public CategoryController(ICategoryLogic categoryLogic, IMapper mapper)
        {
            _categoryLogic = categoryLogic;
            _mapper = mapper;

        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            int zero = 0;
            int divZero = 10 / zero;
            var result = await _categoryLogic.GetAllActiveAsync();

            var viewModel = new IndexViewModel()
            {
                Categories = _mapper.Map<IList<IndexItemViewModel>>(result.Value)
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
            var categoryViewModel = _mapper.Map<CategoryViewModel>(result.Value);
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
            var category = _mapper.Map<Category>(categoryViewModel);
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
            var categoryViewModel = _mapper.Map<CategoryViewModel>(result.Value);
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
            var getResult = await _categoryLogic.GetByIdAsync(categoryViewModel.Id);
            
            if(getResult.Success == false)
            {
                getResult.AddErrorToModelState(ModelState);
                return View(categoryViewModel);
            }
            getResult.Value = _mapper.Map(categoryViewModel, getResult.Value);

            var updateResult = await _categoryLogic.UpdateAsync(getResult.Value);
            if (updateResult.Success == false)
            {
                updateResult.AddErrorToModelState(ModelState);
                return View(categoryViewModel);
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
            var result = await _categoryLogic.GetByIdAsync((Guid)id);
            if (result.Success == false)
            {
                return NotFound();
            }
            var categoryViewModel = _mapper.Map<CategoryViewModel>(result.Value);
            return View(categoryViewModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var result = await _categoryLogic.GetByIdAsync((Guid)id);
            if (result.Success == false)
            {
                result.AddErrorToModelState(ModelState);
                return RedirectToAction(nameof(Index));
            }
            await _categoryLogic.DeleteAsync(result.Value);

            return RedirectToAction(nameof(Index));
        }

    }
}
