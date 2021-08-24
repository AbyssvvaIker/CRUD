using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Warehouse.Core.Entities;
using Warehouse.Infrastructure.DataAccess;
using Warehouse.Api.DTOs.Category;
using Warehouse.Core.Interfaces;
using Warehouse.Web.Infrastructure.ExtensionMethods;
using AutoMapper;
using System.Collections.Generic;
using Warehouse.Core;

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
        [ProducesResponseType(200, Type = typeof(Result<CategoryDto>))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _categoryLogic.GetByIdAsync(id);
            if(result.Success == false)
            {
                return NotFound();
            }

            var dto = _mapper.Map<CategoryDto>(result.Value);
            var resultDto = Result.Ok(dto);

            return Ok(resultDto);
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(Result<IEnumerable<CategoryDto>>))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetAllActive()
        {
            var result = await _categoryLogic.GetAllActiveAsync();
            if(result.Success== false)
            {
                return NotFound();
            }

            var dto = _mapper.Map<CategoryDto>(result.Value);
            var resultDto = Result.Ok(dto);

            return Ok(resultDto);
        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(Result<Category>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Add(CategoryDto dto)
        {
            if(dto == null)
            {
                return BadRequest();
            }
            var category = _mapper.Map<Category>(dto);
            var result =await _categoryLogic.AddAsync(category);
            if(result.Success == false)
            {
                return BadRequest();
            }
            return Ok(Result.Ok(result.Value));
        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(Result<Category>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Update(CategoryDto dto)
        {
            if(dto == null)
            {
                return BadRequest();
            }
            var categoryGetResult = await _categoryLogic.GetByIdAsync(dto.Id);
            if(categoryGetResult.Success == false)
            {
                return BadRequest();
            }
            categoryGetResult.Value = _mapper.Map(dto, categoryGetResult.Value);
            var categoryUpdateResult =await _categoryLogic.UpdateAsync(categoryGetResult.Value);
            if(categoryUpdateResult.Success == false)
            {
                return BadRequest();
            }
            return Ok(Result.Ok(categoryUpdateResult.Value));
        }

        [HttpPost]
        [ProducesResponseType(200, Type = typeof(Result))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var category = _mapper.Map<Category>(id);
            var result = await _categoryLogic.DeleteAsync(category);
            if(result.Success == false)
            {
                return NotFound();
            }
            return Ok(Result.Ok());
        }

    }
}
