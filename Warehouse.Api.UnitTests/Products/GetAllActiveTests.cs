using FizzWare.NBuilder;
//using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Warehouse.Api.Controllers;
using Warehouse.Api.DTOs.Product;
using Warehouse.Api.UnitTests.Products.Infrastructure;
using Warehouse.Core;
using Warehouse.Core.Entities;
using Xunit;

namespace Warehouse.Api.UnitTests.Products
{
    public class GetAllActiveTests : BaseTest
    {
        protected IEnumerable<Product> Products { get; set; }
        protected Result<IEnumerable<Product>> ProductsResult { get; set; }
        protected Result<IEnumerable<ProductDto>> DtoResult { get; set; }
        protected IEnumerable<ProductDto> Dto { get; set; }
        protected override ProductsController Create()
        {
            var controller = base.Create();

            Products = Builder<Product>.CreateListOfSize(5)
                .Build();

            Dto = Builder<ProductDto>.CreateListOfSize(5)
                .Build();

            ProductsResult = Result.Ok(Products);
            DtoResult = Result.Ok(Dto);

            MockProductLogic.Setup(x => x.GetAllActiveAsync()).ReturnsAsync(ProductsResult);
            MockMapper.Setup(x => x.Map<IEnumerable<ProductDto>>(It.IsAny<IEnumerable<Product>>()))
                .Returns(Dto);

            return controller;
        }
        [Fact]
        public async Task Should_Be_BadRequest_When_ResultIs_Failure()
        {
            //arrange
            var controller = Create();
            var property = "property";
            var message = "message";
            ProductsResult = Result.Failure<IEnumerable<Product>>(property, message);
            MockProductLogic.Setup(x => x.GetAllActiveAsync()).ReturnsAsync(ProductsResult);
            //act
            var result =await controller.GetAllActive();
            //assert
            result.Should()
                .BeBadRequest<IEnumerable<Product>>(property, message);

            MockProductLogic.Verify(x =>
            x.GetAllActiveAsync(),
            Times.Once);

            MockMapper.Verify(x =>
            x.Map<IEnumerable<ProductDto>>(It.IsAny<IEnumerable<Product>>()),
            Times.Never);
        }
        [Fact]
        public async Task Should_Be_Ok_When_ResultIs_Ok()
        {
            //arrange
            var controller = Create();
            //act
            var result = await controller.GetAllActive();
            //assert
            result.Should()
                .BeOk(Dto);

            MockProductLogic.Verify(x =>
            x.GetAllActiveAsync(),
            Times.Once);

            MockMapper.Verify(x =>
            x.Map<IEnumerable<ProductDto>>(Products),
            Times.Once);
        }
    }
}
