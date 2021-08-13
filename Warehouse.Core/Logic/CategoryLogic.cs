﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Warehouse.Core.Entities;
using Warehouse.Core.Interfaces;
using Warehouse.Core.Interfaces.Repositories;

namespace Warehouse.Core.Logic
{
    class CategoryLogic : ICategoryLogic
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IProductRepository _productRepository;

        public CategoryLogic(ICategoryRepository categoryRepository, IProductRepository productRepository)
        {
            _categoryRepository = categoryRepository;
            _productRepository = productRepository;
        }
        
        public async Task<Result<Category>> AddAsync(Category category)
        {
            if(category == null)
            {
                throw new ArgumentNullException(nameof(category));
            }
            var result = await _categoryRepository.AddAsync(category);
            await _categoryRepository.SaveChangesAsync();
            return Result.Ok(result);
        }

        public async Task<Result> DeleteAsync(Category category)
        {
            if(category == null)
            {
                throw new ArgumentNullException(nameof(category));
            }
            await _productRepository.DeleteByCategoryIdAsync(category.Id);
            await _productRepository.SaveChangesAsync();
            _categoryRepository.Delete(category);
            await _categoryRepository.SaveChangesAsync();
            return Result.Ok();
        }

        public async Task<Result<IEnumerable<Category>>> GetAllActiveAsync()
        {
            var result = await _categoryRepository.GetAllActiveAsync();
            return Result.Ok(result);
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
            if(category == null)
            {
                throw new ArgumentNullException(nameof(category));
            }
            await _categoryRepository.SaveChangesAsync();

            return Result.Ok(category);
        }
    }
}
