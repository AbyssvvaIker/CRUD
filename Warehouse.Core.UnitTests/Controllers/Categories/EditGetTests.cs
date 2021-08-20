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
            var controller = Create();

            var result = await controller.Edit((Guid?)null);

            result.Should()
                .BeNotFoundResult();
        }
        [Fact]
        public async Task Should_Be_NotFound_When_ResultIs_Failure()
        {
            var controller = Create();
            var errorProperty = "property";
            var errorMessage = "error message";
            CategoryResult = Result.Failure<Category>(errorProperty, errorMessage);

            var result = await controller.Edit(Category.Id);

            result.Should()
                .BeNotFoundResult();
        }

        [Fact]
        public async Task Should_Return_View_With_ViewModel_When_ResultIs_Ok()
        {
            var controller = Create();

            var result = await controller.Edit(Category.Id);

            result.Should()
                .BeViewResult()
                .WithDefaultViewName()
                .Model
                .Should()
                .BeEquivalentTo(ViewModel);
        }

    }
}
