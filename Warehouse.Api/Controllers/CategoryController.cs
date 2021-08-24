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
    [Route("api/Category")]
    [ApiController]
    [Produces("application/json")]
    public class CategoryController : Controller
    {
        private readonly ICategoryLogic _categoryLogic;

        private readonly IMapper _mapper;

        public CategoryController(ICategoryLogic categoryLogic, IMapper mapper)
        {
            _categoryLogic = categoryLogic;
            _mapper = mapper;

        }
        /// <summary>
        /// Get Category By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(Result<CategoryDto>))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _categoryLogic.GetByIdAsync(id);
            if(result.Success == false)
            {
                return NotFound(result);
            }

            var dto = _mapper.Map<CategoryDto>(result.Value);
            var resultDto = Result.Ok(dto);

            return Ok(resultDto);
        }
        /// <summary>
        /// Get All Active Categories
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(Result<IEnumerable<CategoryDto>>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetAllActive()
        {
            var result = await _categoryLogic.GetAllActiveAsync();
            if(result.Success== false)
            {
                return BadRequest(result);
            }

            var dto = _mapper.Map<CategoryDto>(result.Value);
            var resultDto = Result.Ok(dto);

            return Ok(resultDto);
        }
        /// <summary>
        /// Add new Category
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(Result<Category>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Add(CategoryDto dto)
        {
            var category = _mapper.Map<Category>(dto);
            var result =await _categoryLogic.AddAsync(category);
            if(result.Success == false)
            {
                return BadRequest(result);
            }

            var resultDto = _mapper.Map<CategoryDto>(result.Value);

            return CreatedAtAction(nameof(GetById),
                new { id = resultDto.Id },
                resultDto);
        }

        /// <summary>
        /// Update existing category
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(200, Type = typeof(Result<CategoryDto>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Update(Guid id, [FromBody] CategoryDto dto)
        {
            var categoryGetResult = await _categoryLogic.GetByIdAsync(id);
            if(categoryGetResult.Success == false)
            {
                return NotFound(categoryGetResult);
            }
            categoryGetResult.Value = _mapper.Map(dto, categoryGetResult.Value);
            var categoryUpdateResult =await _categoryLogic.UpdateAsync(categoryGetResult.Value);
            if(categoryUpdateResult.Success == false)
            {
                return BadRequest(categoryUpdateResult);
            }

            var resultDto = _mapper.Map<CategoryDto>(categoryUpdateResult.Value);
            return Ok(Result.Ok(resultDto));
        }
        /// <summary>
        /// Delete category
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [ProducesResponseType(200, Type = typeof(Result))]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var categoryResult =await _categoryLogic.GetByIdAsync(id);
            if(categoryResult.Success == false)
            {
                return NotFound(categoryResult);
            }
            var result = await _categoryLogic.DeleteAsync(categoryResult.Value);
            if(result.Success == false)
            {
                return BadRequest(result);
            }
            return NoContent();
        }

    }
}
