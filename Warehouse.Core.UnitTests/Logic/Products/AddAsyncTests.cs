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
using Warehouse.Core.UnitTests.Logic.Products.Infrastructure;

namespace Warehouse.Core.UnitTests.Logic.Products
{
    public class AddAsyncTests : BaseTest
    {
        public Product Product;
        public void CorrectFlow()
        {
            Product = Builder<Product>
                .CreateNew()
                .Build();

            MockProductRepository.Setup(x => x.AddAsync(Product)).ReturnsAsync(Product);
            MockValidator.SetValidationSuccess();
        }

        protected override ProductLogic Create()
        {
            var productLogic = base.Create();
            CorrectFlow();

            return productLogic;
        }
        [Fact]
        public async Task Should_Return_AddedProduct_And_ResultOk()
        {
            //arrange
            var productLogic = Create();
            MockProductRepository.Setup(x => x.AddAsync(Product)).ReturnsAsync(Product);
            //act
            var result = await productLogic.AddAsync(Product);
            //assert
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Value.Should().BeSameAs(Product);

            MockValidator.Verify(
                x => x.Validate(Product),
                Times.Once);
            MockProductRepository.Verify(
                x => x.AddAsync(Product),
                Times.Once);
            MockProductRepository.Verify(
                x => x.SaveChangesAsync(),
                Times.Once);
        }

        [Fact]
        public async Task Should_Return_ResultFailure_When_Validation_Failed()
        {
            //arrange
            var productLogic = Create();
            var validatedProperty = "test";
            var errorMessage = "test error message";
            MockValidator.SetValidationFailure(validatedProperty, errorMessage);
            //act
            var result = await productLogic.AddAsync(Product);
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

            MockValidator.Verify(
                x => x.Validate(Product),
                Times.Once);
            MockProductRepository.Verify(
                x => x.AddAsync(It.IsAny<Product>()),
                Times.Never);
            MockProductRepository.Verify(
                x => x.SaveChangesAsync(),
                Times.Never);
        }

        [Fact]
        public void Should_Throw_ArgumentNullException_When_GivenProduct_Null()
        {
            //arrange
            var productLogic = Create();
            //act
            Func<Task> act = async () => await productLogic.AddAsync(null);
            //assert
            act.Should().ThrowAsync<ArgumentNullException>();

            MockValidator.Verify(
                x => x.Validate(It.IsAny<Product>()),
                Times.Never);
            MockProductRepository.Verify(
                x => x.AddAsync(It.IsAny<Product>()),
                Times.Never);
            MockProductRepository.Verify(
                x => x.SaveChangesAsync(),
                Times.Never);
        }
    }
}
