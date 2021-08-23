using FizzWare.NBuilder;
using FluentAssertions.AspNetCore.Mvc;
using FluentAssertions;
using Moq;
using System;
using System.Threading.Tasks;
using Warehouse.Core.Entities;
using Warehouse.Core.UnitTests.Controllers.Products.Infrastructure;
using Warehouse.Web.Controllers;
using Warehouse.Web.ViewModels.Product;
using Xunit;
using Warehouse.Core.UnitTests.CustomAssertions;

namespace Warehouse.Core.UnitTests.Controllers.Products
{
    public class DeletePostTests : BaseTest
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
            

            return controller;
        }

        [Fact]
        public async Task Should_RedirectToAction_Index_When_GetResultIs_Failure()
        {
            //arrange
            var controller = Create();
            var errorProperty = "property";
            var errorMessage = "error message";
            ProductResult = Result.Failure<Product>(errorProperty, errorMessage);
            //act
            var result =await controller.DeleteConfirmed(Product.Id);
            //assert
            result.Should()
                .BeRedirectToActionResult()
                .WithActionName(nameof(Index));
            controller.Should()
                .HasError(errorProperty, errorMessage);


            MockProductLogic.Verify(
                x => x.GetByIdAsync(Product.Id),
                Times.Once);
            MockProductLogic.Verify(
                x => x.DeleteAsync(It.IsAny<Product>()),
                Times.Never);

        }
        [Fact]
        public async Task Should_RedirectToAction_Index_When_GetResultIs_Ok()
        {
            //arrange
            var controller = Create();
            //act
            var result =await controller.DeleteConfirmed(Product.Id);
            //assert
            result.Should()
                .BeRedirectToActionResult()
                .WithActionName(nameof(Index));

            MockProductLogic.Verify(
                x => x.GetByIdAsync(Product.Id),
                Times.Once);
            MockProductLogic.Verify(
                x => x.DeleteAsync(ProductResult.Value),
                Times.Once);
        }
    }
}
