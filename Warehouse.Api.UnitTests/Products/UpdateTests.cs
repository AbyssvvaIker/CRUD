using FizzWare.NBuilder;
using Moq;
using System;
using System.Threading.Tasks;
using Warehouse.Api.Controllers;
using Warehouse.Api.DTOs.Product;
using Warehouse.Api.UnitTests.Products.Infrastructure;
using Warehouse.Core;
using Warehouse.Core.Entities;
using Xunit;

namespace Warehouse.Api.UnitTests.Products
{
    public class UpdateTests : BaseTest
    {
        protected Product Product { get; set; }
        protected Result<Product> GetResult { get; set; }
        protected Result<Product> UpdateResult { get; set; }
        protected Result<ProductDto> DtoResult { get; set; }
        protected ProductDto Dto { get; set; }
        protected override ProductsController Create()
        {
            var controller = base.Create();

            Product = Builder<Product>.CreateNew()
                .Build();

            Dto = Builder<ProductDto>.CreateNew()
                .Build();

            GetResult = Result.Ok(Product);
            UpdateResult = Result.Ok(Product);
            DtoResult = Result.Ok(Dto);

            MockProductLogic.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(GetResult);
            MockProductLogic.Setup(x => x.UpdateAsync(It.IsAny<Product>())).ReturnsAsync(UpdateResult);
            MockMapper.Setup(x => x.Map<ProductDto>(It.IsAny<Product>())).Returns(Dto);

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
            var result = await controller.Update(Product.Id, Dto);
            //assert
            result.Should()
                .BeNotFound<Product>(property, message, string.Empty);

            MockProductLogic.Verify(x =>
            x.GetByIdAsync(Product.Id),
            Times.Once);

            MockMapper.Verify(x =>
            x.Map(It.IsAny<ProductDto>(), It.IsAny<Product>()),
            Times.Never);

            MockProductLogic.Verify(x =>
            x.UpdateAsync(It.IsAny<Product>()),
            Times.Never);

            MockMapper.Verify(x =>
            x.Map<ProductDto>(It.IsAny<Product>()),
            Times.Never);
        }
        [Fact]
        public async Task Should_Be_BadRequest_When_UpdateResultIs_Failure()
        {
            var controller = Create();
            var property = "property";
            var message = "message";
            UpdateResult = Result.Failure<Product>(property, message);
            MockProductLogic.Setup(x => x.UpdateAsync(It.IsAny<Product>())).ReturnsAsync(UpdateResult);
            //act
            var result = await controller.Update(Product.Id, Dto);
            //assert
            result.Should()
                .BeBadRequest<Product>(property, message, string.Empty);

            MockProductLogic.Verify(x =>
            x.GetByIdAsync(Product.Id),
            Times.Once);

            MockMapper.Verify(x =>
            x.Map(Dto, Product),
            Times.Once);

            MockProductLogic.Verify(x =>
            x.UpdateAsync(Product),
            Times.Once);

            MockMapper.Verify(x =>
            x.Map<ProductDto>(It.IsAny<Product>()),
            Times.Never);
        }
        [Fact]
        public async Task Should_Be_Ok_When_ResultAre_Ok()
        {

            var controller = Create();
            //act
            var result = await controller.Add(Dto);
            //assert
            result.Should()
                .BeOk(Dto);

            MockProductLogic.Verify(x =>
            x.GetByIdAsync(Product.Id),
            Times.Once);

            MockMapper.Verify(x =>
            x.Map(Dto, Product),
            Times.Once);

            MockProductLogic.Verify(x =>
            x.UpdateAsync(Product),
            Times.Once);

            MockMapper.Verify(x =>
            x.Map<ProductDto>(Product),
            Times.Once);
        }
    }
}
