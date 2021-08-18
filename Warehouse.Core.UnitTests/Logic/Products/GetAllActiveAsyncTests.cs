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
    public class GetAllActiveAsyncTests :BaseTest
    {
        public IEnumerable<Product> Products;
        public void CorrectFlow()
        {
            Products = Builder<Product>
                .CreateListOfSize(3)
                .Build();

            MockProductRepository.Setup(x => x.GetAllActiveAsync()).ReturnsAsync(Products);
            MockValidator.SetValidationSuccess();
        }

        protected override ProductLogic Create()
        {
            var productLogic = base.Create();
            CorrectFlow();

            return productLogic;
        }
        [Fact]
        public async Task Should_Return_ResultOk_With_ListOfProducts()
        {
            //arrange
            var productLogic = Create();
            //act
            var result = await productLogic.GetAllActiveAsync();
            //assert
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Value.Should().BeSameAs(Products);

            MockProductRepository.Verify(
                x => x.GetAllActiveAsync(),
                Times.Once);

        }
        [Fact]
        public async Task Should_Return_ResultOk_With_EmptyList_When_NoActiveProducts()
        {
            //arrange
            var productLogic = Create();
            //act
            var result = await productLogic.GetAllActiveAsync();
            //assert
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Value.Should().BeSameAs(Products);

            MockProductRepository.Verify(
                x => x.GetAllActiveAsync(),
                Times.Once);
        }
    }
}
