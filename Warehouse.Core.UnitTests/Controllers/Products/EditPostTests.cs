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
using Warehouse.Core.UnitTests.CustomAssertions;

namespace Warehouse.Core.UnitTests.Controllers.Products
{
    public class EditPostTests : BaseTest
    {
        protected Category Category { get; set; }
        protected ProductViewModel ViewModel { get; set; }
        protected Result<Category> CategoryGetResult { get; set; }
        protected Result<Category> CategoryUpdateResult { get; set; }

        protected override ProductsController Create()
        {
            var controller = base.Create();
            Category = Builder<Category>
                .CreateNew()
                .Build();

            ViewModel = Builder<ProductViewModel>
                .CreateNew()
                .Build();

            CategoryGetResult = Result.Ok(Category);
            CategoryUpdateResult = Result.Ok(CategoryGetResult.Value);

            MockCategoryLogic.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(() => CategoryGetResult);
            MockMapper.Setup(x => x.Map(It.IsAny<ProductViewModel>(), It.IsAny<Category>())).Returns(Category);

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

            MockCategoryLogic.Verify(
                x => x.GetByIdAsync(It.IsAny<Guid>()),
                Times.Never);
            MockMapper.Verify(
                x => x.Map(It.IsAny<ProductViewModel>(), It.IsAny<Category>()),
                Times.Never);
            MockCategoryLogic.Verify(
                x => x.UpdateAsync(It.IsAny<Category>()),
                Times.Never);
        }


        [Fact]
        public async Task Should_Return_View_With_ViewModel_When_GetResultIs_Failure()
        {
            //arrange
            var controller = Create();
            var errorProperty = "property";
            var errorMessage = "error message";
            CategoryGetResult = Result.Failure<Category>(errorProperty,errorMessage);
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
            MockCategoryLogic.Verify(
                x => x.GetByIdAsync(ViewModel.Id),
                Times.Once);
            MockMapper.Verify(
                x => x.Map(It.IsAny<ProductViewModel>(), It.IsAny<Category>()),
                Times.Never);
            MockCategoryLogic.Verify(
                x => x.UpdateAsync(It.IsAny<Category>()),
                Times.Never);
        }

        [Fact]
        public async Task Should_Return_View_With_ViewModel_When_UpdateResultIs_Failure()
        {
            //arrange
            var controller = Create();
            var errorProperty = "property";
            var errorMessage = "error message";
            CategoryUpdateResult = Result.Failure<Category>(errorProperty, errorMessage);
            MockCategoryLogic.Setup(x => x.UpdateAsync(It.IsAny<Category>())).ReturnsAsync(CategoryUpdateResult);
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

            MockCategoryLogic.Verify(
                x => x.GetByIdAsync(ViewModel.Id),
                Times.Once);
            MockMapper.Verify(
                x => x.Map(ViewModel, CategoryGetResult.Value),
                Times.Once);
            MockCategoryLogic.Verify(
                x => x.UpdateAsync(CategoryGetResult.Value),
                Times.Once);
        }

        [Fact]
        public async Task Should_RedirectToAction_When_GetResult_And_UpdateResult_Are_Ok()
        {
            //arrange
            var controller = Create();
            MockCategoryLogic.Setup(x => x.UpdateAsync(It.IsAny<Category>())).ReturnsAsync(CategoryUpdateResult);
            //act
            var result =await controller.Edit(ViewModel);
            //assert
            result.Should()
                .BeRedirectToActionResult()
                .WithActionName(nameof(Index));

            MockCategoryLogic.Verify(
                x => x.GetByIdAsync(ViewModel.Id),
                Times.Once);
            MockMapper.Verify(
                x => x.Map(ViewModel, CategoryGetResult.Value),
                Times.Once);
            MockCategoryLogic.Verify(
                x => x.UpdateAsync(CategoryGetResult.Value),
                Times.Once);
        }

    }
}
