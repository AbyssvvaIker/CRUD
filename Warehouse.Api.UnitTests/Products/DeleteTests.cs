using FizzWare.NBuilder;
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
    public class DeleteTests : BaseTest
    {
        protected Product Product { get; set; }
        protected Result<Product> GetResult { get; set; }
        protected Result<Product> DeleteResult { get; set; }
        protected override ProductsController Create()
        {
            var controller = base.Create();

            Product = Builder<Product>.CreateNew()
                .Build();

            GetResult = Result.Ok(Product);
            DeleteResult = Result.Ok(Product);

            MockProductLogic.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(GetResult);
            MockProductLogic.Setup(x => x.DeleteAsync(It.IsAny<Product>())).ReturnsAsync(DeleteResult);

            return controller;
        }
        [Fact]
        public async Task Should_Be_NotFound_When_GetResultIs_Failure()
        {
            var controller = Create();
            var property = "property";
            var message = "message";
            GetResult = Result.Failure<Product>(property, message);
            MockProductLogic.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(GetResult);
            //act
            var result = await controller.Delete(Product.Id);
            //assert
            result.Should()
                .BeNotFound<Product>(property, message, string.Empty);

            MockProductLogic.Verify(x =>
            x.GetByIdAsync(Product.Id),
            Times.Once);

            MockProductLogic.Verify(x =>
            x.DeleteAsync(It.IsAny<Product>()),
            Times.Never);
        }

        [Fact]
        public async Task Should_Be_BadRequest_When_DeleteResultIs_Failure()
        {

            var controller = Create();
            var property = "property";
            var message = "message";
            DeleteResult = Result.Failure<Product>(property, message);
            MockProductLogic.Setup(x => x.DeleteAsync(It.IsAny<Product>())).ReturnsAsync(DeleteResult);
            //act
            var result = await controller.Delete(Product.Id);
            //assert
            result.Should()
                .BeBadRequest<Product>(property, message, string.Empty);

            MockProductLogic.Verify(x =>
            x.GetByIdAsync(Product.Id),
            Times.Once);

            MockProductLogic.Verify(x =>
            x.DeleteAsync(Product),
            Times.Once);
        }

    }
}
