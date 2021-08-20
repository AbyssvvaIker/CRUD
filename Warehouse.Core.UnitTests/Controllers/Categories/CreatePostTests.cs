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
using Warehouse.Core.UnitTests.CustomAssertions;

namespace Warehouse.Core.UnitTests.Controllers.Categories
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
            var result =await controller.Create(ViewModel);
            //assert
            result.Should()
                .BeViewResult()
                .WithDefaultViewName()
                .Model
                .Should()
                .BeEquivalentTo(ViewModel);
            controller.Should()
                .HasError(errorProperty, errorMessage);



        }

        [Fact]
        public async Task Should_Return_View_With_Errors_When_ResultIs_Failure()
        {
            //arrange
            var controller = Create();
            var errorProperty = "property";
            var errorMessage = "message";
            CategoryResult = Result.Failure<Category>(errorProperty, errorMessage);
            MockMapper.Setup(x => x.Map<Category>(ViewModel)).Returns(Category);
            //act
            var result =await controller.Create(ViewModel);

            //assert
            result.Should()
                .BeViewResult()
                .WithDefaultViewName()
                .Model
                .Should()
                .BeEquivalentTo(ViewModel);
            controller.Should()
                .HasError(errorProperty, errorMessage);

        }

        [Fact]
        public async Task Should_Redirect_ToIndex_When_ResultIs_Ok()
        {
            //arrange
            var controller = Create();
            //act
            var result =await controller.Create(ViewModel);
            //assert
            result.Should()
                .BeRedirectToActionResult()
                .WithActionName(nameof(Index));
        }
    }
}
