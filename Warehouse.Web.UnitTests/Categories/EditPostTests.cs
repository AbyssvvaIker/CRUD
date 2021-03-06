using FizzWare.NBuilder;
using FluentAssertions.AspNetCore.Mvc;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Warehouse.Core.Entities;
using Warehouse.Web.UnitTests.Categories.Infrastructure;
using Warehouse.Web.Controllers;
using Warehouse.Web.ViewModels.Category;
using Xunit;
using Warehouse.Web.UnitTests.CustomAssertions;
using Warehouse.Core;

namespace Warehouse.Web.UnitTests.Categories
{
    public class EditPostTests : BaseTest
    {
        protected Category Category { get; set; }
        protected CategoryViewModel ViewModel { get; set; }
        protected Result<Category> CategoryGetResult { get; set; }
        protected Result<Category> CategoryUpdateResult { get; set; }

        protected override CategoryController Create()
        {
            var controller = base.Create();
            Category = Builder<Category>
                .CreateNew()
                .Build();

            ViewModel = Builder<CategoryViewModel>
                .CreateNew()
                .Build();

            CategoryGetResult = Result.Ok(Category);
            CategoryUpdateResult = Result.Ok(CategoryGetResult.Value);

            MockCategoryLogic.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(() => CategoryGetResult);
            MockCategoryLogic.Setup(x => x.UpdateAsync(It.IsAny<Category>())).ReturnsAsync(CategoryUpdateResult);
            MockMapper.Setup(x => x.Map(It.IsAny<CategoryViewModel>(), It.IsAny<Category>())).Returns(Category);

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

            MockCategoryLogic.Verify(
                x => x.GetByIdAsync(It.IsAny<Guid>()),
                Times.Never);
            MockMapper.Verify(
                x => x.Map(It.IsAny<CategoryViewModel>(), It.IsAny<Category>()),
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
                x => x.Map(It.IsAny<CategoryViewModel>(), It.IsAny<Category>()),
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
