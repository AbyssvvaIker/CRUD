using FizzWare.NBuilder;
using FluentAssertions;
using FluentAssertions.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Warehouse.Core;
using Warehouse.Core.Entities;
using Warehouse.Web.Controllers;
using Warehouse.Web.UnitTests.CustomAssertions;
using Warehouse.Web.UnitTests.Products.Infrastructure;
using Warehouse.Web.ViewModels;
using Warehouse.Web.ViewModels.Product;
using Xunit;

namespace Warehouse.Web.UnitTests.Products
{
    public class EditPostTests : BaseTest
    {
        protected Product Product { get; set; }
        protected ProductViewModel ViewModel { get; set; }
        protected Result<Product> ProductGetResult { get; set; }
        protected Result<Product> ProductUpdateResult { get; set; }
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

            ProductGetResult = Result.Ok(Product);
            ProductUpdateResult = Result.Ok(ProductGetResult.Value);

            CategoriesResult = Builder<Result<IEnumerable<Category>>>
            .CreateNew()
            .Build();

            MockProductLogic.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(() => ProductGetResult);
            MockProductLogic.Setup(x => x.UpdateAsync(It.IsAny<Product>())).ReturnsAsync(ProductUpdateResult);
            MockMapper.Setup(x => x.Map(It.IsAny<ProductViewModel>(), It.IsAny<Product>())).Returns(Product);

            MockCategoryLogic.Setup(x => x.GetAllActiveAsync()).ReturnsAsync(CategoriesResult);
            MockMapper.Setup(x => x.Map<IList<SelectItemViewModel>>(CategoriesResult.Value));


            return controller;
        }
        [Fact]
        public async Task Should_return_View_With_ViewModel_When_Validation_Failed()
        {
            //arrange
            var controller = Create();
            var errorProperty = "property";
            var errorMessage = "error message";
            controller.ModelState.AddModelError(errorProperty, errorMessage);
            //act
            var result = await controller.Edit(ViewModel);
            //assert
            result.Should()
                .BeViewResult()
                .WithDefaultViewName()
                .Model
                .Should()
                .BeEquivalentTo(ViewModel);
            controller.Should()
                .HasError(errorProperty, errorMessage);

            MockProductLogic.Verify(
                x => x.GetByIdAsync(It.IsAny<Guid>()),
                Times.Never);
            MockMapper.Verify(
                x => x.Map(It.IsAny<ProductViewModel>(), It.IsAny<Product>()),
                Times.Never);
            MockProductLogic.Verify(
                x => x.UpdateAsync(It.IsAny<Product>()),
                Times.Never);

            MockCategoryLogic.Verify(
                x => x.GetAllActiveAsync(),
                Times.Once);
            MockMapper.Verify(
                x => x.Map<IList<SelectItemViewModel>>(CategoriesResult.Value),
                Times.Once);
        }


        [Fact]
        public async Task Should_Return_View_With_ViewModel_When_GetResultIs_Failure()
        {
            //arrange
            var controller = Create();
            var errorProperty = "property";
            var errorMessage = "error message";
            ProductGetResult = Result.Failure<Product>(errorProperty, errorMessage);
            //act
            var result = await controller.Edit(ViewModel);
            //assert
            result.Should()
                .BeViewResult()
                .WithDefaultViewName()
                .Model
                .Should()
                .BeEquivalentTo(ViewModel);
            controller.Should()
                .HasError(errorProperty, errorMessage);

            MockProductLogic.Verify(
                x => x.GetByIdAsync(ViewModel.Id),
                Times.Once);
            MockMapper.Verify(
                x => x.Map(It.IsAny<ProductViewModel>(), It.IsAny<Product>()),
                Times.Never);
            MockProductLogic.Verify(
                x => x.UpdateAsync(It.IsAny<Product>()),
                Times.Never);
            MockCategoryLogic.Verify(
                x => x.GetAllActiveAsync(),
                Times.Never);
            MockMapper.Verify(
                x => x.Map<IList<SelectItemViewModel>>(It.IsAny<Result<IEnumerable<Category>>>()),
                Times.Never);
        }

        [Fact]
        public async Task Should_Return_View_With_ViewModel_When_UpdateResultIs_Failure()
        {
            //arrange
            var controller = Create();
            var errorProperty = "property";
            var errorMessage = "error message";
            ProductUpdateResult = Result.Failure<Product>(errorProperty, errorMessage);
            MockProductLogic.Setup(x => x.UpdateAsync(It.IsAny<Product>())).ReturnsAsync(ProductUpdateResult);
            //act
            var result = await controller.Edit(ViewModel);
            //assert
            result.Should()
                .BeViewResult()
                .WithDefaultViewName()
                .Model
                .Should()
                .BeEquivalentTo(ViewModel);
            controller.Should()
                .HasError(errorProperty, errorMessage);

            MockProductLogic.Verify(
                x => x.GetByIdAsync(ViewModel.Id),
                Times.Once);
            MockMapper.Verify(
                x => x.Map(ViewModel, ProductGetResult.Value),
                Times.Once);
            MockProductLogic.Verify(
                x => x.UpdateAsync(ProductGetResult.Value),
                Times.Once);

            MockCategoryLogic.Verify(
                x => x.GetAllActiveAsync(),
                Times.Once);
            MockMapper.Verify(
                x => x.Map<IList<SelectItemViewModel>>(CategoriesResult.Value),
                Times.Once);
        }

        [Fact]
        public async Task Should_RedirectToAction_When_GetResult_And_UpdateResult_Are_Ok()
        {
            //arrange
            var controller = Create();
            //act
            var result = await controller.Edit(ViewModel);
            //assert
            result.Should()
                .BeRedirectToActionResult()
                .WithActionName(nameof(Index));

            MockProductLogic.Verify(
                x => x.GetByIdAsync(ViewModel.Id),
                Times.Once);
            MockMapper.Verify(
                x => x.Map(ViewModel, ProductGetResult.Value),
                Times.Once);
            MockProductLogic.Verify(
                x => x.UpdateAsync(ProductGetResult.Value),
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
