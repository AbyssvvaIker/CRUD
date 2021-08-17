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

namespace Warehouse.Core.UnitTests.Logic.Products
{
    public class AddAsyncTests
    {
        [Fact]
        public async Task ShouldReturnAddedCategoryAndSuccess()
        {
            var product = Builder<Product>
                .CreateNew()
                .With(x => x.Id = Guid.NewGuid())
                .Build();

            var mockProductRepository = new Mock<IProductRepository>();
            mockProductRepository.Setup(x => x.AddAsync(product)).ReturnsAsync(product);
            var mockValidator = new Mock<IValidator<Product>>();

            var categoryLogic = new ProductLogic(mockProductRepository.Object, mockValidator.Object);
            mockValidator.SetValidationSuccess();
            
            var result = await categoryLogic.AddAsync(product);
            
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Value.Should().BeSameAs(product);
        }

        [Fact]
        public async Task ShouldReturnErrorListAndNoSuccess()
        {
            var product = new Product()
            {
                Id = Guid.NewGuid(),
            };
            var mockProductRepository = new Mock<IProductRepository>();
            mockProductRepository.Setup(x => x.AddAsync(product)).ReturnsAsync((Product)null);
            var mockValidator = new Mock<IValidator<Product>>();

            var categoryLogic = new ProductLogic(mockProductRepository.Object, mockValidator.Object);
            mockValidator.SetValidationFailure("test", "test error message");

            var result = await categoryLogic.AddAsync(product);

            result.Should().NotBeNull();
            result.Success.Should().BeFalse();
            //Errors?
        }

        [Fact]
        public void ShouldThrowArgumentNullException()
        {
            var mockProductRepository = new Mock<IProductRepository>();
            var mockValidator = new Mock<IValidator<Product>>();

            var categoryLogic = new ProductLogic(mockProductRepository.Object, mockValidator.Object);

            Func<Task> act = async () => await categoryLogic.AddAsync((Product)null);

            act.Should().ThrowAsync<ArgumentNullException>();
        }
    }
}
