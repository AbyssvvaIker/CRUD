using FluentValidation;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Warehouse.Core.Entities;
using Warehouse.Core.Interfaces.Repositories;
using Warehouse.Core.Logic;

namespace Warehouse.Core.UnitTests.Logic.Products.Infrastructure
{
    public class BaseTest
    {
        public Mock<IProductRepository> mockProductRepository;
        public Mock<IValidator<Product>> mockValidator;
        public Product product;
        public IEnumerable<Product> products;
        public virtual ProductLogic Create()
        {
            mockProductRepository = new Mock<IProductRepository>();
            mockValidator = new Mock<IValidator<Product>>();

            return new ProductLogic(mockProductRepository.Object, mockValidator.Object);
        }
    }
}
