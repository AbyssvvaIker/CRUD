using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Moq;
using FluentAssertions;
using FizzWare.NBuilder;
using Warehouse.Core.Interfaces.Repositories;
using Warehouse.Core.Entities;
using Warehouse.Core.Interfaces;
using FluentValidation;
using Warehouse.Core.Logic;
using System.Threading.Tasks;
using Warehouse.Core.UnitTests.Extensions;
using FluentValidation.Results;
using Warehouse.Core.UnitTests.Logic.Categories.Infrastructure;

namespace Warehouse.Core.UnitTests.Logic.Categories
{
    public class AddAsyncTests : BaseTest
    {
        public Category CorrectFlow(Mock<ICategoryRepository> mockCategoryRepository, Mock<IProductRepository> mockProductRepository,
            Mock<IValidator<Category>> mockValidator)
        {
            var category = Builder<Category>
                .CreateNew()
                .Build();

            mockCategoryRepository.Setup(x => x.AddAsync(category)).ReturnsAsync(category);
            mockValidator.SetValidationSuccess();
            return category;
        }

        public override CategoryLogic Create()
        {
            var categoryLogic = base.Create();
            CorrectFlow(mockCategoryRepository, mockProductRepository, mockValidator);


            return categoryLogic;
        }


        [Fact]
        public async Task ShouldReturnAddedCategoryAndSuccess()
        {
            //arrange
            var category = Builder<Category>
                .CreateNew()
                .Build();

            var mockCategoryRepository = new Mock<ICategoryRepository>();
            mockCategoryRepository.Setup(x => x.AddAsync(category)).ReturnsAsync(category);

            var mockProductRepository = new Mock<IProductRepository>();
            var mockValidator = new Mock<IValidator<Category>>();

            var categoryLogic = new CategoryLogic(mockCategoryRepository.Object, mockProductRepository.Object, mockValidator.Object);
            mockValidator.SetValidationSuccess();
            //act
            var result = await categoryLogic.AddAsync(category);
            //assert
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Value.Should().BeSameAs(category);
        }

        [Fact]
        public async Task ShouldReturnErrorListAndNoSuccess()
        {
            //arrange
            var category = Builder<Category>
                .CreateNew()
                .Build();

            var mockCategoryRepository = new Mock<ICategoryRepository>();
            mockCategoryRepository.Setup(x => x.AddAsync(category) ).ReturnsAsync((Category)null);

            var mockProductRepository = new Mock<IProductRepository>();
            var mockValidator = new Mock<IValidator<Category>>();

            var categoryLogic = new CategoryLogic(mockCategoryRepository.Object, mockProductRepository.Object, mockValidator.Object);
            string validatedProperty = "test";
            string errorMessage = "test error message";
            mockValidator.SetValidationFailure(validatedProperty, errorMessage);

            //act
            var result = await categoryLogic.AddAsync(category);
            //assert
            result.Should().NotBeNull();
            result.Success.Should().BeFalse();
            result.Errors.Should().HaveCount(1);
            foreach(var err in result.Errors)
            {
                err.Should().BeEquivalentTo(new ErrorMessage() 
                {
                    PropertyName = validatedProperty,
                    Message = errorMessage,
                });
            }
        }

        [Fact]
        public void ShouldThrowArgumentNullException()
        {
            var mockCategoryRepository = new Mock<ICategoryRepository>();
            var mockProductRepository = new Mock<IProductRepository>();
            var mockValidator = new Mock<IValidator<Category>>();

            var categoryLogic = new CategoryLogic(mockCategoryRepository.Object, mockProductRepository.Object, mockValidator.Object);

            Func<Task> act = async () => await categoryLogic.AddAsync((Category)null);

            act.Should().ThrowAsync<ArgumentNullException>();
        }
    }
}
