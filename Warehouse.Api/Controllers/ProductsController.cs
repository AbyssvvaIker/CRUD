using AutoMapper;
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
using Warehouse.Api.Infrastructure.ExtensionMethods;

namespace Warehouse.Api.Controllers
{
    [Route("api/Product")]
    [ApiController]
    [Produces("application/json")]
    public class ProductsController : Controller
    {
        private readonly IProductLogic _productLogic;
        private readonly IMapper _mapper;

        public ProductsController(IProductLogic productLogic, IMapper mapper)
        {
            _productLogic = productLogic;
            _mapper = mapper;
        }
        /// <summary>
        /// Get Product By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(Result<ProductDto>))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _productLogic.GetByIdAsync(id);
            if (result.Success == false)
            {
                return NotFound(result);
            }

            var dto = _mapper.Map<ProductDto>(result.Value);
            var resultDto = Result.Ok(dto);

            return Ok(resultDto);
        }
        /// <summary>
        /// Get All Active Products
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(Result<IEnumerable<ProductDto>>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetAllActive()
        {
            var result = await _productLogic.GetAllActiveAsync();
            if (result.Success == false)
            {
                return BadRequest(result);
            }

            var dto = _mapper.Map<IEnumerable<ProductDto>>(result.Value);
            var resultDto = Result.Ok(dto);

            return Ok(resultDto);
        }
        /// <summary>
        /// Add new Product
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(Result<ProductDto>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Add(ProductDto dto)
        {
            var product = _mapper.Map<Product>(dto);
            var result = await _productLogic.AddAsync(product);
            if (result.Success == false)
            {
                return BadRequest(result);
            }

            var resultDto = Result.Ok(_mapper.Map<ProductDto>(result.Value));
            return CreatedAtAction(nameof(GetById),
                new { id = resultDto.Value.Id },
                resultDto);
        }
        /// <summary>
        /// Update existing product
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(200, Type = typeof(Result<ProductDto>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Update(Guid id, [FromBody] ProductDto dto)
        {
            var getResult = await _productLogic.GetByIdAsync(id);
            if (getResult.Success == false)
            {
                return NotFound(getResult);
            }
            getResult.Value = _mapper.Map(dto, getResult.Value);
            var updateResult = await _productLogic.UpdateAsync(getResult.Value);
            if (updateResult.Success == false)
            {
                return BadRequest(updateResult);
            }
            var resultDto = _mapper.Map<ProductDto>(updateResult.Value);
            return Ok(Result.Ok(resultDto));
        }
        /// <summary>
        /// delete product
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [ProducesResponseType(200, Type = typeof(Result))]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var productResult = await _productLogic.GetByIdAsync(id);
            if(productResult.Success == false)
            {
                return NotFound(productResult);
            }
            var result = await _productLogic.DeleteAsync(productResult.Value);
            if (result.Success == false)
            {
                return BadRequest(result);
            }
            return NoContent();
        }

    }
}
