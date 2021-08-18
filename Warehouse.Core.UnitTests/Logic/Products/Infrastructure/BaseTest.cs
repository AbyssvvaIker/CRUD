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
        protected Mock<IProductRepository> MockProductRepository;
        protected Mock<IValidator<Product>> MockValidator;
        
        protected virtual ProductLogic Create()
        {
            MockProductRepository = new Mock<IProductRepository>();
            MockValidator = new Mock<IValidator<Product>>();

            return new ProductLogic(MockProductRepository.Object, MockValidator.Object);
        }
    }
}
