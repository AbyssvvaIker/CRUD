using FizzWare.NBuilder;
using FluentAssertions.AspNetCore.Mvc;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Warehouse.Core.Entities;
using Warehouse.Core.UnitTests.Controllers.Categories.Infrastructure;
using Warehouse.Web.Controllers;
using Warehouse.Web.ViewModels.Category;
using Xunit;
namespace Warehouse.Core.UnitTests.Controllers.Categories
{
    public class EditPostTests : BaseTest
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

            MockCategoryLogic.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(() => CategoryResult);
            MockMapper.Setup(x => x.Map(It.IsAny<CategoryViewModel>(), It.IsAny<Category>())).Returns(Category);

            return controller;
        }
        [Fact]
        public async Task Should_return_View_With_ViewModel_When_Validation_Failed()
        {
            var controller = Create();
            var errorProperty = "property";
            var errorMessage = "error message";
            controller.ModelState.AddModelError(errorProperty, errorMessage);

            var result = await controller.Create(ViewModel);

            result.Should()
                .BeViewResult()
                .WithDefaultViewName()
                .Model
                .Should()
                .BeEquivalentTo(ViewModel);
            //errors?
        }


        [Fact]
        public async Task Should_Return_View_With_ViewModel_When_GetResultIs_Failure()
        {
            var controller = Create();
            var errorProperty = "property";
            var errorMessage = "error message";
            CategoryResult = Result.Failure<Category>(errorProperty,errorMessage);

            var result = await controller.Edit(ViewModel);

            result.Should()
                .BeViewResult()
                .WithDefaultViewName()
                .Model
                .Should()
                .BeEquivalentTo(ViewModel);
            //errors?
        }

        [Fact]
        public async Task Should_Return_View_With_ViewModel_When_UpdateResultIs_Failure()
        {
            var controller = Create();
            var errorProperty = "property";
            var errorMessage = "error message";
            CategoryResult = Result.Failure<Category>(errorProperty, errorMessage);
            MockCategoryLogic.Setup(x => x.UpdateAsync(It.IsAny<Category>())).ReturnsAsync(CategoryResult);

            var result = await controller.Edit(ViewModel);

            result.Should()
                .BeViewResult()
                .WithDefaultViewName()
                .Model
                .Should()
                .BeEquivalentTo(ViewModel);
            //errors?
        }

        [Fact]
        public async Task Should_RedirectToAction_When_GetResult_And_UpdateResult_Are_Ok()
        {
            var controller = Create();
            MockCategoryLogic.Setup(x => x.UpdateAsync(It.IsAny<Category>())).ReturnsAsync(CategoryResult);

            var result =await controller.Edit(ViewModel);

            result.Should()
                .BeRedirectToActionResult()
                .WithActionName(nameof(Index));
        }

    }
}
