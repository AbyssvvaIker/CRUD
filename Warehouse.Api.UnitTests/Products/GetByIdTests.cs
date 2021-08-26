using FizzWare.NBuilder;
//using FluentAssertions.AspNetCore.Mvc;
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
    public class GetByIdTests : BaseTest
    {
        protected Product Product { get; set; }
        protected Result<Product> ProductResult { get; set; }
        protected Result<ProductDto> DtoResult { get; set; }
        protected ProductDto Dto { get; set; }
        protected override ProductsController Create()
        {
            var controller = base.Create();

            Product = Builder<Product>.CreateNew()
                .Build();

            Dto = Builder<ProductDto>.CreateNew()
                .Build();

            ProductResult = Result.Ok(Product);
            DtoResult = Result.Ok(Dto);

            MockProductLogic.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(ProductResult);
            MockMapper.Setup(x => x.Map<ProductDto>(It.IsAny<Product>())).Returns(Dto);

            return controller;
        }

        [Fact]
        public async Task Should_Be_NotFound_When_ResultIs_Failure()
        {
            //arrange
            var controller = Create();
            var property = "property";
            var message = "message";
            ProductResult = Result.Failure<Product>(property, message);
            MockProductLogic.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(ProductResult);
            //act
            var result =await controller.GetById(Product.Id);
            //assert
            result.Should()
                .BeNotFound<Product>(property,message);

            MockProductLogic.Verify(x =>
            x.GetByIdAsync(Product.Id),
            Times.Once);

            MockMapper.Verify(x =>
            x.Map<ProductDto>(It.IsAny<Product>()),
            Times.Never);

        }

        [Fact]
        public async Task Should_Return_ResultDto_When_ResultIs_Ok()
        {
            //arrange
            var controller = Create();
            //act
            var result =await controller.GetById(Product.Id);
            //assert
            result.Should()
                .BeOk(Dto);

            MockProductLogic.Verify(x =>
            x.GetByIdAsync(Product.Id),
            Times.Once);

            MockMapper.Verify(x =>
            x.Map<ProductDto>(Product),
            Times.Once);
        }
    }
}
