﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Warehouse.Core.Entities;
using Warehouse.Core.Interfaces;
using Warehouse.Core.Interfaces.Repositories;

namespace Warehouse.Core.Logic
{
    class ProductLogic : IProductLogic
    {
        private readonly IProductRepository _productRepository;

        public ProductLogic(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Result<Product>> AddAsync(Product product)
        {
            if(product == null)
            {
                throw new ArgumentNullException(nameof(product));
            }
            var result = await _productRepository.AddAsync(product);
            await _productRepository.SaveChangesAsync();
            return Result.Ok(result);
        }

        public async Task<Result> DeleteAsync(Product product)
        {
            if(product == null)
            {
                throw new ArgumentNullException(nameof(product));
            }
            _productRepository.Delete(product);
            await _productRepository.SaveChangesAsync();
            return Result.Ok();
        }

        public async Task<Result<IEnumerable<Product>>> GetAllActiveAsync()
        {
            var result = await _productRepository.GetAllActiveAsync();
            return Result.Ok(result);
        }

        public async Task<Result<Product>> GetByIdAsync(Guid id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                return Result.Failure<Product>($"product {id} does not exist");
            }
            return Result.Ok(product);
        }

        public async Task<Result<Product>> UpdateAsync(Product product)
        {
            if(product == null)
            {
                throw new ArgumentNullException(nameof(product));
            }

            await _productRepository.SaveChangesAsync();

            return Result.Ok(product);
        }
    }
}