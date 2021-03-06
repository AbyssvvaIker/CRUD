using FizzWare.NBuilder;

using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Warehouse.Core.Entities;
using Warehouse.Web.UnitTests.Products.Infrastructure;
using Warehouse.Web.Controllers;
using Warehouse.Web.ViewModels.Product;
using Xunit;
using FluentAssertions.AspNetCore.Mvc;
using FluentAssertions;
using System.Threading.Tasks;
using Warehouse.Web.UnitTests.CustomAssertions;
using Warehouse.Core;

namespace Warehouse.Web.UnitTests.Products
{
    public class DetailsGetTests : BaseTest
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
        public async Task Should_Be_NotFound_When_Id_IsNull()
        {
            //arrange
            var controller = Create();
            //act
            var result =await controller.Details(null);
            //assert
            result.Should()
                .BeNotFoundResult();

            MockProductLogic.Verify(
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
            var errProperty = "Property";
            var errMessage = "Error";
            ProductResult = Result.Failure<Product>(errProperty,errMessage);
            //act
            var result = await controller.Details(ViewModel.Id);
            //assert
            result.Should()
                .BeNotFoundResult();
            controller.Should()
                .HasError(errProperty,errMessage);

            MockProductLogic.Verify(
                x => x.GetByIdAsync(ViewModel.Id),
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
            MockProductLogic.Setup(x => x.GetByIdAsync(Product.Id)).ReturnsAsync(() => ProductResult);
            //act
            var result = await controller.Details(ViewModel.Id);
            //assert
            result.Should()
                .BeViewResult()
                .WithDefaultViewName()
                .Model
                .Should()
                .BeEquivalentTo(ViewModel);

            MockProductLogic.Verify(
                x => x.GetByIdAsync(ViewModel.Id),
                Times.Once);
            MockMapper.Verify(
                x => x.Map<ProductViewModel>(ProductResult.Value),
                Times.Once);

        }
    }
}
