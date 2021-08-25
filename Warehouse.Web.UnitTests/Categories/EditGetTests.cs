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
using Warehouse.Core;

namespace Warehouse.Web.UnitTests.Categories
{
    public class EditGetTests : BaseTest
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
            MockMapper.Setup(x => x.Map<CategoryViewModel>(It.IsAny<Category>())).Returns(ViewModel);

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
                x => x.Map<CategoryViewModel>(It.IsAny<Category>()),
                Times.Never);
        }
        [Fact]
        public async Task Should_Be_NotFound_When_ResultIs_Failure()
        {
            //arrange
            var controller = Create();
            var errorProperty = "property";
            var errorMessage = "error message";
            CategoryResult = Result.Failure<Category>(errorProperty, errorMessage);
            //act
            var result = await controller.Edit(Category.Id);
            //assert
            result.Should()
                .BeNotFoundResult();

            MockCategoryLogic.Verify(
                x => x.GetByIdAsync(Category.Id),
                Times.Once);
            MockMapper.Verify(
                x => x.Map<CategoryViewModel>(It.IsAny<Category>()),
                Times.Never);
        }

        [Fact]
        public async Task Should_Return_View_With_ViewModel_When_ResultIs_Ok()
        {
            //arrange
            var controller = Create();
            //act
            var result = await controller.Edit(Category.Id);
            //assert
            result.Should()
                .BeViewResult()
                .WithDefaultViewName()
                .Model
                .Should()
                .BeEquivalentTo(ViewModel);

            MockCategoryLogic.Verify(
                x => x.GetByIdAsync(Category.Id),
                Times.Once);
            MockMapper.Verify(
                x => x.Map<CategoryViewModel>(CategoryResult.Value),
                Times.Once);
        }

    }
}
