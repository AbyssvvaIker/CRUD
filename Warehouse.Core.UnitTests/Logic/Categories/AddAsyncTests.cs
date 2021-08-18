using FizzWare.NBuilder;
using FluentAssertions;
using FluentValidation;
using Moq;
using System;
using System.Threading.Tasks;
using Warehouse.Core.Entities;
using Warehouse.Core.Interfaces.Repositories;
using Warehouse.Core.Logic;
using Warehouse.Core.UnitTests.Extensions;
using Warehouse.Core.UnitTests.Logic.Categories.Infrastructure;
using Xunit;

namespace Warehouse.Core.UnitTests.Logic.Categories
{
    public class AddAsyncTests : BaseTest
    {
        public Category Category;
        public void CorrectFlow()
        {
            Category = Builder<Category>
                .CreateNew()
                .Build();

            mockCategoryRepository.Setup(x => x.AddAsync(Category)).ReturnsAsync(Category);
            mockValidator.SetValidationSuccess();
        }

        public override CategoryLogic Create()
        {
            var categoryLogic = base.Create();
            CorrectFlow();


            return categoryLogic;
        }


        [Fact]
        public async Task Should_Return_AddedCategory_And_ResultOk()
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

            mockValidator.Verify(
                x => x.Validate(category),
                Times.Once);
            mockCategoryRepository.Verify(
                x => x.AddAsync(It.IsAny<Category>()),
                Times.Once);
            mockCategoryRepository.Verify(
                x => x.SaveChangesAsync(),
                Times.Once);
        }

        [Fact]
        public async Task Should_Return_ResultFailure_When_Validation_Failed()
        {
            //arrange
            var category = Builder<Category>
                .CreateNew()
                .Build();

            var mockCategoryRepository = new Mock<ICategoryRepository>();
            mockCategoryRepository.Setup(x => x.AddAsync(category)).ReturnsAsync((Category)null);

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
            foreach (var err in result.Errors)
            {
                err.Should().BeEquivalentTo(new ErrorMessage()
                {
                    PropertyName = validatedProperty,
                    Message = errorMessage,
                });
            }
            mockValidator.Verify(
                x => x.Validate(category),
                Times.Once);
            mockCategoryRepository.Verify(
                x => x.AddAsync(It.IsAny<Category>()),
                Times.Never);
            mockCategoryRepository.Verify(
                x => x.SaveChangesAsync(),
                Times.Never);

        }

        [Fact]
        public void Should_Throw_ArgumentNullException_When_GivenCategory_Null()
        {
            var mockCategoryRepository = new Mock<ICategoryRepository>();
            var mockProductRepository = new Mock<IProductRepository>();
            var mockValidator = new Mock<IValidator<Category>>();

            var categoryLogic = new CategoryLogic(mockCategoryRepository.Object, mockProductRepository.Object, mockValidator.Object);

            Func<Task> act = async () => await categoryLogic.AddAsync((Category)null);

            act.Should().ThrowAsync<ArgumentNullException>();
            mockValidator.Verify(
                x => x.Validate(Category),
                Times.Never);
            mockCategoryRepository.Verify(
                x => x.AddAsync(It.IsAny<Category>()),
                Times.Never);
            mockCategoryRepository.Verify(
                x => x.SaveChangesAsync(),
                Times.Never);
        }
    }
}
