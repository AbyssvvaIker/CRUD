using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Warehouse.Core.Entities;
using Warehouse.Core.Interfaces;
using Warehouse.Web.Infrastructure.ExtensionMethods;
using Warehouse.Web.ViewModels;
using Warehouse.Web.ViewModels.Product;

namespace Warehouse.Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductLogic _productLogic;
        private readonly ICategoryLogic _categoryLogic;
        private readonly IMapper _mapper;

        public ProductsController(IProductLogic productLogic, ICategoryLogic categoryLogic, IMapper mapper)
        {
            _productLogic = productLogic;
            _categoryLogic = categoryLogic;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var result = await _productLogic.GetAllActiveAsync();
            if (result.Success == false)
            {
                return NotFound();
            }
            var viewModel = new IndexViewModel()
            {
                Products = _mapper.Map<IList<IndexItemViewModel>>(result.Value)
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
                result.AddErrorToModelState(ModelState);
                return NotFound();
            }
            var productViewModel = _mapper.Map<ProductViewModel>(result.Value);
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
            var product = _mapper.Map<Product>(productViewModel);
            var result = await _productLogic.AddAsync(product);
            if (result.Success == false)
            {
                result.AddErrorToModelState(ModelState);
                await GetCategoriesFromDb(productViewModel);
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
            var productViewModel = _mapper.Map<ProductViewModel>(result.Value);
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

            var getResult = await _productLogic.GetByIdAsync(productViewModel.Id);
            if (getResult.Success == false)
            {
                getResult.AddErrorToModelState(ModelState);
                return View(productViewModel);
            }
            getResult.Value = _mapper.Map(productViewModel, getResult.Value);
            var updateResult = await _productLogic.UpdateAsync(getResult.Value);
            if (updateResult.Success == false)
            {
                updateResult.AddErrorToModelState(ModelState);
                await GetCategoriesFromDb(productViewModel);
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
            var productViewModel = _mapper.Map<ProductViewModel>(result.Value);
            return View(productViewModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var result = await _productLogic.GetByIdAsync((Guid)id);
            if (result.Success == false)
            {

                result.AddErrorToModelState(ModelState);
                return RedirectToAction(nameof(Index));
            }
            await _productLogic.DeleteAsync(result.Value);
            return RedirectToAction(nameof(Index));
        }

        private async Task GetCategoriesFromDb(ProductViewModel viewModel)
        {
            var result = await _categoryLogic.GetAllActiveAsync();
            var categories = _mapper.Map<IList<SelectItemViewModel>>(result.Value);
            viewModel.AvailableCategories = categories;
            return;
        }
    }
}
