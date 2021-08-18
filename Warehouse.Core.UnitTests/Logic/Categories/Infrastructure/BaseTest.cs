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
        protected Mock<ICategoryRepository> MockCategoryRepository;
        protected Mock<IProductRepository> MockProductRepository;
        protected Mock<IValidator<Category>> MockValidator;
        
        protected virtual CategoryLogic Create()
        {
            MockCategoryRepository = new Mock<ICategoryRepository>();
            MockProductRepository = new Mock<IProductRepository>();
            MockValidator = new Mock<IValidator<Category>>();

            return new CategoryLogic(MockCategoryRepository.Object, MockProductRepository.Object, MockValidator.Object);

        }
    }
}
