﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Warehouse.Api.DTOs.Product;
using Warehouse.Core;
using Warehouse.Core.Entities;
using Warehouse.Core.Interfaces;
using Warehouse.Infrastructure.DataAccess;
using Warehouse.Web.Infrastructure.ExtensionMethods;

namespace Warehouse.Api.Controllers
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
        [ProducesResponseType(200, Type = typeof(Result<ProductDto>))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _productLogic.GetByIdAsync(id);
            if (result.Success == false)
            {
                return NotFound();
            }

            var dto = _mapper.Map<ProductDto>(result.Value);
            var resultDto = Result.Ok(dto);

            return Ok(resultDto);
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(Result<IEnumerable<ProductDto>>))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetAllActive()
        {
            var result = await _productLogic.GetAllActiveAsync();
            if (result.Success == false)
            {
                return NotFound();
            }

            var dto = _mapper.Map<ProductDto>(result.Value);
            var resultDto = Result.Ok(dto);

            return Ok(resultDto);
        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(Result<Product>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Add(ProductDto dto)
        {
            if (dto == null)
            {
                return BadRequest();
            }
            var product = _mapper.Map<Product>(dto);
            var result = await _productLogic.AddAsync(product);
            if (result.Success == false)
            {
                return BadRequest();
            }
            return Ok(Result.Ok(result.Value));
        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(Result<Product>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Update(ProductDto dto)
        {
            if (dto == null)
            {
                return BadRequest();
            }
            var productGetResult = await _productLogic.GetByIdAsync(dto.Id);
            if (productGetResult.Success == false)
            {
                return BadRequest();
            }
            productGetResult.Value = _mapper.Map(dto, productGetResult.Value);
            var productUpdateResult = await _productLogic.UpdateAsync(productGetResult.Value);
            if (productUpdateResult.Success == false)
            {
                return BadRequest();
            }
            return Ok(Result.Ok(productUpdateResult.Value));
        }

        [HttpPost]
        [ProducesResponseType(200, Type = typeof(Result))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var product = _mapper.Map<Product>(id);
            var result = await _productLogic.DeleteAsync(product);
            if (result.Success == false)
            {
                return NotFound();
            }
            return Ok(Result.Ok());
        }

        //private async Task GetCategoriesFromDb(ProductViewModel viewModel)
        //{
        //    var result = await _categoryLogic.GetAllActiveAsync();
        //    var categories = _mapper.Map<IList<SelectItemViewModel>>(result.Value);
        //    viewModel.AvailableCategories = categories;
        //    return; 
        //}
    }
}
