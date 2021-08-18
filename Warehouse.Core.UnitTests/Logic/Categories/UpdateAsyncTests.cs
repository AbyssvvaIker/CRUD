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

            Func<Task> act = async () => await categoryLogic.UpdateAsync((Category)null);

            await act.Should().ThrowAsync<ArgumentNullException>();
            //errors?

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
                .With(x => x.Id = Guid.NewGuid())
                .With(x => x.Name = "testName")
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
