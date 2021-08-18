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
        protected Mock<ICategoryRepository> mockCategoryRepository;
        protected Mock<IProductRepository> mockProductRepository;
        protected Mock<IValidator<Category>> mockValidator;
        
        protected virtual CategoryLogic Create()
        {
            mockCategoryRepository = new Mock<ICategoryRepository>();
            mockProductRepository = new Mock<IProductRepository>();
            mockValidator = new Mock<IValidator<Category>>();

            return new CategoryLogic(mockCategoryRepository.Object, mockProductRepository.Object, mockValidator.Object);

        }
    }
}
