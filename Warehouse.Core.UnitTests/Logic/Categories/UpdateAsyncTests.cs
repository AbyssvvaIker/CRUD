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
using Xunit;

namespace Warehouse.Core.UnitTests.Logic.Categories
{
    public class UpdateAsyncTests
    {
        [Fact]
        public async Task ShouldThrowArgumentNullException()
        {
            var mockCategoryRepository = new Mock<ICategoryRepository>();
            var mockProductRepository = new Mock<IProductRepository>();
            var mockValidator = new Mock<IValidator<Category>>();

            var categoryLogic = new CategoryLogic(mockCategoryRepository.Object, mockProductRepository.Object,
                mockValidator.Object);

            Func<Task> act = async () => await categoryLogic.UpdateAsync(null);

            await act.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task ShouldReturnResultFailure()
        {
            var category = Builder<Category>
                .CreateNew()
                .Build();

            var mockCategoryRepository = new Mock<ICategoryRepository>();
            var mockProductRepository = new Mock<IProductRepository>();
            var mockValidator = new Mock<IValidator<Category>>();
            string validatedProperty = "test";
            string errorMessage = "test error message";
            mockValidator.SetValidationFailure(validatedProperty, errorMessage);

            var categoryLogic = new CategoryLogic(mockCategoryRepository.Object, mockProductRepository.Object,
                mockValidator.Object);

            var result = await categoryLogic.UpdateAsync(category);

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
        }

        [Fact]
        public async Task ShouldReturnResultOk()
        {
            //arrange
            var category = Builder<Category>
                .CreateNew()
                .Build();

            var mockCategoryRepository = new Mock<ICategoryRepository>();
            var mockProductRepository = new Mock<IProductRepository>();
            var mockValidator = new Mock<IValidator<Category>>();

            var categoryLogic = new CategoryLogic(mockCategoryRepository.Object, mockProductRepository.Object, mockValidator.Object);
            mockValidator.SetValidationSuccess();
            //act
            var result = await categoryLogic.UpdateAsync(category);
            //assert
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Value.Should().BeSameAs(category);
        }
    }
}
