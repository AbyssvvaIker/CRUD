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
using Warehouse.Core.UnitTests.CustomAssertions;

namespace Warehouse.Core.UnitTests.Logic.Products
{
    public class UpdateAsyncTests : BaseTest
    {
        public Product Product;
        public void CorrectFlow()
        {
            Product = Builder<Product>
                .CreateNew()
                .Build();

            MockValidator.SetValidationSuccess();
        }

        protected override ProductLogic Create()
        {
            var productLogic = base.Create();
            CorrectFlow();

            return productLogic;
        }
        [Fact]
        public async Task Should_Throw_ArgumentNullException_When_GivenProduct_Null()
        {
            //arrange
            var productLogic = Create();
            //act
            Func<Task> act = async () => await productLogic.UpdateAsync(null);
            //assert
            await act.Should().ThrowAsync<ArgumentNullException>();

            MockValidator.Verify(
                x => x.Validate(It.IsAny<Product>()),
                Times.Never);
            MockProductRepository.Verify(
                x => x.SaveChangesAsync(),
                Times.Never);
        }

        [Fact]
        public async Task Should_Return_ResultFailure_When_ValidationFailed()
        {
            //arrange
            var productLogic = Create();
            var validatedProperty = "test";
            var errorMessage = "test error message";
            MockValidator.SetValidationFailure(validatedProperty, errorMessage);
            //act
            var result = await productLogic.UpdateAsync(Product);
            //assert
            result.Should().BeFailure(validatedProperty, errorMessage);

            MockValidator.Verify(
                x => x.Validate(Product),
                Times.Once);
            MockProductRepository.Verify(
                x => x.SaveChangesAsync(),
                Times.Never);
        }

        [Fact]
        public async Task ShouldReturnResultOk()
        {
            //arrange
            var productLogic = Create();
            //act
            var result = await productLogic.UpdateAsync(Product);
            //assert
            result.Should().BeSuccess(Product);

            MockValidator.Verify(
                x => x.Validate(Product),
                Times.Once);
            MockProductRepository.Verify(
                x => x.SaveChangesAsync(),
                Times.Once);
        }
    }
}
