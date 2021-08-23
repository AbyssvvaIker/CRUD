using FizzWare.NBuilder;
using FluentAssertions.AspNetCore.Mvc;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Warehouse.Core.Entities;
using Warehouse.Core.UnitTests.Controllers.Products.Infrastructure;
using Warehouse.Web.Controllers;
using Warehouse.Web.ViewModels.Product;
using Xunit;

namespace Warehouse.Core.UnitTests.Controllers.Products
{
    public class EditGetTests : BaseTest
    {
        protected Product Product { get; set; }
        protected ProductViewModel ViewModel { get; set; }
        protected Result<Product> ProductResult { get; set; }
        protected override ProductsController Create()
        {
            var controller = base.Create();
            Product = Builder<Product>
                .CreateNew()
                .Build();

            ViewModel = Builder<ProductViewModel>
                .CreateNew()
                .Build();

            ProductResult = Result.Ok(Product);

            MockProductLogic.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(() => ProductResult);
            MockMapper.Setup(x => x.Map<ProductViewModel>(It.IsAny<Product>())).Returns(ViewModel);

            return controller;
        }

        [Fact]
        public async Task Should_Be_NotFound_When_GivenIdIs_Null()
        {
            //arrange
            var controller = Create();
            //act
            var result = await controller.Edit((Guid?)null);
            //assert
            result.Should()
                .BeNotFoundResult();


            MockCategoryLogic.Verify(
                x => x.GetByIdAsync(It.IsAny<Guid>()),
                Times.Never);
            MockMapper.Verify(
                x => x.Map<ProductViewModel>(It.IsAny<Product>()),
                Times.Never);
        }
        [Fact]
        public async Task Should_Be_NotFound_When_ResultIs_Failure()
        {
            //arrange
            var controller = Create();
            var errorProperty = "property";
            var errorMessage = "error message";
            ProductResult = Result.Failure<Product>(errorProperty, errorMessage);
            //act
            var result = await controller.Edit(Product.Id);
            //assert
            result.Should()
                .BeNotFoundResult();

            MockProductLogic.Verify(
                x => x.GetByIdAsync(Product.Id),
                Times.Once);
            MockMapper.Verify(
                x => x.Map<ProductViewModel>(It.IsAny<Product>()),
                Times.Never);
        }

        [Fact]
        public async Task Should_Return_View_With_ViewModel_When_ResultIs_Ok()
        {
            //arrange
            var controller = Create();
            //act
            var result = await controller.Edit(Product.Id);
            //assert
            result.Should()
                .BeViewResult()
                .WithDefaultViewName()
                .Model
                .Should()
                .BeEquivalentTo(ViewModel);

            MockProductLogic.Verify(
                x => x.GetByIdAsync(Product.Id),
                Times.Once);
            MockMapper.Verify(
                x => x.Map<ProductViewModel>(ProductResult.Value),
                Times.Once);
        }

    }
}
