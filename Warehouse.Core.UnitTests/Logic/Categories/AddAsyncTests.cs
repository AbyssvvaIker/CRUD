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
    public class AddAsyncTests
    {
        [Fact]
        public void ShouldReturnAddedCategoryAndSuccess()
        {
            //arrange

            //act
            //assert
        }

        [Fact]
        public async Task ShouldReturnErrorListAndNoSuccess()
        {
            //arrange
            var category = new Category()
            {
                Id = Guid.NewGuid(),
                Name = "test",
            };
            var mockCategoryRepository = new Mock<ICategoryRepository>();
            mockCategoryRepository.Setup(x => x.AddAsync(category) ).ReturnsAsync((Category)null);

            var mockProductRepository = new Mock<IProductRepository>();
            var mockValidator = new Mock<IValidator<Category>>();
            //mockValidator.Setup(x => x.Validate(category)).Returns(
            //    new FluentValidation.Results.ValidationResult( )
            //    {
            //        //IsValid = false
            //    });

            var categoryLogic = new CategoryLogic(mockCategoryRepository.Object, mockProductRepository.Object, mockValidator.Object);
            mockValidator.SetValidationFailure("test", "test error message");

            //act
            var result = await categoryLogic.AddAsync(category);
            //assert
            result.Should().NotBeNull();
            result.Success.Should().BeFalse();
            //result.Errors.Should().HaveCount(1).And.Contain(new ErrorMessage()
            //{
            //    Message = "",//?
            //    PropertyName = ""//?
            //});
        }

        [Fact]
        public void ShouldThrowArgumentNullReferenceException()
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
