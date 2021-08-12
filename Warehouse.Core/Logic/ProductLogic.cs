using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Warehouse.Core.Entities;
using Warehouse.Core.Interfaces;

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
            var result = await _productRepository.AddAsync(product);
            if (result == null)
            {
                return Result.Failure<Product>($"Unable to add product");
            }
            await _productRepository.SaveChangesAsync();
            return Result.Ok(result);
        }

        public async Task<Result> DeleteAsync(Product product)
        {
            _productRepository.Delete(product);
            return Result.Ok();
        }
        public async Task<Result> DeleteAsync(Guid id)
        {
            var category = await _productRepository.GetByIdAsync(id);
            if (category == null)
            {
                return Result.Failure<Category>($"failed to find category {id}");
            }
            _productRepository.Delete(category);
            await _productRepository.SaveChangesAsync();
            return Result.Ok();
        }

        public async Task<Result<IEnumerable<Product>>> GetAllActiveAsync()
        {
            var result = await _productRepository.GetAllActiveAsync();
            if (result == null)
            {
                return Result.Failure<IEnumerable<Product>>($"No Active products found");
            }
            return Result.Ok<IEnumerable<Product>>(result);
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
            var result = await _productRepository.GetByIdAsync(product.Id);
            if (result == null)
            {
                return Result.Failure<Product>($"category with id {product.Id} not found");
            }
            result.Name = product.Name;
            result.Description = product.Description;
            result.CategoryId = product.CategoryId;
            result.Category = product.Category;
            result.Price = product.Price;
            await _productRepository.SaveChangesAsync();

            return Result.Ok(result);
        }
    }
}
