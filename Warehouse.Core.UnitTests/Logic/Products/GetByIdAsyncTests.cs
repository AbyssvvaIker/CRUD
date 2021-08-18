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
    public class GetByIdAsyncTests : BaseTest
    {
        public Product Product;
        public void CorrectFlow()
        {
            Product = Builder<Product>
                .CreateNew()
                .Build();

            MockProductRepository.Setup(x => x.GetByIdAsync(Product.Id)).ReturnsAsync(Product);
            MockValidator.SetValidationSuccess();
        }

        protected override ProductLogic Create()
        {
            var productLogic = base.Create();
            CorrectFlow();

            return productLogic;
        }
        [Fact]
        public async Task Should_Return_ResultOk()
        {
            //arrage
            var productLogic = Create();
            //act
            var result = await productLogic.GetByIdAsync(Product.Id);
            //assert
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Value.Should().BeSameAs(Product);

            MockProductRepository.Verify(
                x => x.GetByIdAsync(Product.Id),
                Times.Once);
        }
        [Fact]
        public async Task Should_Return_ResultFailure_When_ProductDoesNotExist()
        {
            //arrange
            var productLogic = Create();
            MockProductRepository.Setup(x => x.GetByIdAsync(Product.Id)).ReturnsAsync((Product)null);
            //act
            var result = await productLogic.GetByIdAsync(Product.Id);
            //assert
            result.Should().NotBeNull();
            result.Success.Should().BeFalse();
            result.Value.Should().BeSameAs(null);

            MockProductRepository.Verify(
                x => x.GetByIdAsync(Product.Id),
                Times.Once);
        }
    }
}
