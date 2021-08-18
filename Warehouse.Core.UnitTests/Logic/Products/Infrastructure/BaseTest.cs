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
        public virtual ProductLogic Create()
        {
            var mockProductRepository = new Mock<IProductRepository>();
            var mockValidator = new Mock<IValidator<Product>>();

            return new ProductLogic(mockProductRepository.Object, mockValidator.Object);
        }
    }
}
