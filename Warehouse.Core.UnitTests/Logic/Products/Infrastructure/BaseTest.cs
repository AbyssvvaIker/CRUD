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
        protected Mock<IProductRepository> mockProductRepository;
        protected Mock<IValidator<Product>> mockValidator;
        
        protected virtual ProductLogic Create()
        {
            mockProductRepository = new Mock<IProductRepository>();
            mockValidator = new Mock<IValidator<Product>>();

            return new ProductLogic(mockProductRepository.Object, mockValidator.Object);
        }
    }
}
