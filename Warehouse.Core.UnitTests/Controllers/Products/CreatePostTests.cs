using FizzWare.NBuilder;
using FluentAssertions;
using FluentAssertions.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Warehouse.Core.Entities;
using Warehouse.Core.UnitTests.Controllers.Products.Infrastructure;
using Warehouse.Core.UnitTests.CustomAssertions;
using Warehouse.Web.Controllers;
using Warehouse.Web.ViewModels;
using Warehouse.Web.ViewModels.Product;
using Xunit;

namespace Warehouse.Core.UnitTests.Controllers.Products
{
    public class CreatePostTests : BaseTest
    {
        protected Product Product { get; set; }
        protected ProductViewModel ViewModel { get; set; }
        protected Result<Product> ProductResult { get; set; }
        protected Result<IEnumerable<Category>> CategoriesResult { get; set; }

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

            CategoriesResult = Builder<Result<IEnumerable<Category>>>
            .CreateNew()
            .Build();

            MockProductLogic.Setup(x => x.AddAsync(It.IsAny<Product>())).ReturnsAsync(() => ProductResult);
            MockMapper.Setup(x => x.Map<Product>(It.IsAny<ProductViewModel>())).Returns(Product);

            MockCategoryLogic.Setup(x => x.GetAllActiveAsync()).ReturnsAsync(CategoriesResult);
            MockMapper.Setup(x => x.Map<IList<SelectItemViewModel>>(CategoriesResult.Value));

            return controller;
        }

        [Fact]
        public async Task Should_Return_View_With_ViewModel_When_ValidationFailed()
        {
            //arrange
            var controller = Create();
            var errorProperty = "property";
            var errorMessage = "error message";
            controller.ModelState.AddModelError(errorProperty, errorMessage);
            //act
            var result = await controller.Create(ViewModel);
            //assert
            result.Should()
                .BeViewResult()
                .WithDefaultViewName()
                .Model
                .Should()
                .BeEquivalentTo(ViewModel);
            controller.Should()
                .HasError(errorProperty, errorMessage);

            MockMapper.Verify(
                x => x.Map<Product>(It.IsAny<ProductViewModel>()),
                Times.Never
                );
            MockProductLogic.Verify(
                x => x.AddAsync(It.IsAny<Product>()),
                Times.Never);

            MockCategoryLogic.Verify(
                x => x.GetAllActiveAsync(),
                Times.Once);
            MockMapper.Verify(
                x => x.Map<IList<SelectItemViewModel>>(CategoriesResult.Value),
                Times.Once);
        }

        [Fact]
        public async Task Should_Return_View_With_Errors_When_ResultIs_Failure()
        {
            //arrange
            var controller = Create();
            var errorProperty = "property";
            var errorMessage = "message";
            ProductResult = Result.Failure<Product>(errorProperty, errorMessage);
            MockMapper.Setup(x => x.Map<Product>(ViewModel)).Returns(Product);
            
            //act
            var result = await controller.Create(ViewModel);

            //assert
            result.Should()
                .BeViewResult()
                .WithDefaultViewName()
                .Model
                .Should()
                .BeEquivalentTo(ViewModel);
            controller.Should()
                .HasError(errorProperty, errorMessage);

            MockMapper.Verify(
                x => x.Map<Product>(ViewModel),
                Times.Once
                );
            MockProductLogic.Verify(
                x => x.AddAsync(Product),
                Times.Once);

            MockCategoryLogic.Verify(
                x => x.GetAllActiveAsync(),
                Times.Once);
            MockMapper.Verify(
                x => x.Map<IList<SelectItemViewModel>>(CategoriesResult.Value),
                Times.Once);
        }

        [Fact]
        public async Task Should_Redirect_ToIndex_When_ResultIs_Ok()
        {
            //arrange
            var controller = Create();
            //act
            var result = await controller.Create(ViewModel);
            //assert
            result.Should()
                .BeRedirectToActionResult()
                .WithActionName(nameof(Index));
            MockMapper.Verify(
                x => x.Map<Product>(ViewModel),
                Times.Once);
            MockProductLogic.Verify(
                x => x.AddAsync(Product),
                Times.Once);


            MockCategoryLogic.Verify(
                x => x.GetAllActiveAsync(),
                Times.Never);
            MockMapper.Verify(
                x => x.Map<IList<SelectItemViewModel>>(It.IsAny<Result<IEnumerable<Category>>>()),
                Times.Never);
        }
    }
}
