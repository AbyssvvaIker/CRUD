using FizzWare.NBuilder;
using FluentAssertions;
using FluentAssertions.AspNetCore.Mvc;
using Moq;
using System;
using System.Threading.Tasks;
using Warehouse.Core.Entities;
using Warehouse.Web.UnitTests.Categories.Infrastructure;
using Warehouse.Web.UnitTests.CustomAssertions;
using Warehouse.Web.Controllers;
using Warehouse.Web.ViewModels.Category;
using Xunit;
using Warehouse.Core;

namespace Warehouse.Web.UnitTests.Categories
{
    public class CreatePostTests : BaseTest
    {
        protected Category Category { get; set; }
        protected CategoryViewModel ViewModel { get; set; }
        protected Result<Category> CategoryResult { get; set; }

        protected override CategoryController Create()
        {
            var controller = base.Create();
            Category = Builder<Category>
                .CreateNew()
                .Build();

            ViewModel = Builder<CategoryViewModel>
                .CreateNew()
                .Build();

            CategoryResult = Result.Ok(Category);

            MockCategoryLogic.Setup(x => x.AddAsync(It.IsAny<Category>())).ReturnsAsync(() => CategoryResult);
            MockMapper.Setup(x => x.Map<Category>(It.IsAny<CategoryViewModel>())).Returns(Category);

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
                x => x.Map<Category>(It.IsAny<CategoryViewModel>()),
                Times.Never
                );
            MockCategoryLogic.Verify(
                x => x.AddAsync(It.IsAny<Category>()),
                Times.Never);
        }

        [Fact]
        public async Task Should_Return_View_With_Errors_When_ResultIs_Failure()
        {
            //arrange
            var controller = Create();
            var errorProperty = "property";
            var errorMessage = "message";
            CategoryResult = Result.Failure<Category>(errorProperty, errorMessage);
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
                x => x.Map<Category>(ViewModel),
                Times.Once
                );
            MockCategoryLogic.Verify(
                x => x.AddAsync(Category),
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
                x => x.Map<Category>(ViewModel),
                Times.Once);
            MockCategoryLogic.Verify(
                x => x.AddAsync(Category),
                Times.Once);
        }
    }
}
