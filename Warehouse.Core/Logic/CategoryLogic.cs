using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Warehouse.Core.Entities;
using Warehouse.Core.Interfaces;

namespace Warehouse.Core.Logic
{
    class CategoryLogic : ICategoryLogic
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IProductRepository _productRepository;
        
        public async Task<Result<Category>> AddAsync(Category category)
        {
            var result = await _categoryRepository.AddAsync(category);
            if(result == null)
            {
                return Result.Failure<Category>($"Unable to add category");
            }
            return Result.Ok(result);
        }

        public async Task<Result> DeleteAsync(Category category)
        {
            _categoryRepository.Delete(category);
            return Result.Ok();
        }

        public async Task<Result<IEnumerable<Category>>> GetAllActiveAsync()
        {
            var result = await _categoryRepository.GetAllActiveAsync();
            if(result == null)
            {
                return Result.Failure<IEnumerable<Category>>($"No Active categories found");
            }
            return Result.Ok<IEnumerable<Category>>(result);
        }

        public async Task<Result<Category>> GetByIdAsync(Guid id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if(category == null)
            {
                return Result.Failure<Category>($"Category {id} does not exist");
            }
            return Result.Ok(category);
        }

        public async Task<Result<Category>> UpdateAsync(Category category)
        {
            var result = await _categoryRepository.GetByIdAsync(category.Id);
            if (result == null)
            {
                return Result.Failure<Category>($"category with id {category.Id} not found");
            }
            result.Name = category.Name;
            result.Products = category.Products;

            return Result.Ok(result);
        }
    }
}
