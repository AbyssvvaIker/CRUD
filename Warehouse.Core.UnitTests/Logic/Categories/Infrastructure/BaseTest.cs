using FluentValidation;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Warehouse.Core.Entities;
using Warehouse.Core.Interfaces.Repositories;
using Warehouse.Core.Logic;

namespace Warehouse.Core.UnitTests.Logic.Categories.Infrastructure
{
    public class BaseTest
    {
        public virtual CategoryLogic Create()
        {
            var mockCategoryRepository = new Mock<ICategoryRepository>();
            var mockProductRepository = new Mock<IProductRepository>();
            var mockValidator = new Mock<IValidator<Category>>();

            return new CategoryLogic(mockCategoryRepository.Object, mockProductRepository.Object, mockValidator.Object);

        }
    }
}
