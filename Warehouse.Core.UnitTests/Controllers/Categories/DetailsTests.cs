using FizzWare.NBuilder;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Warehouse.Core.Entities;
using Warehouse.Core.UnitTests.Controllers.Categories.Infrastructure;
using Warehouse.Web.Controllers;
using Warehouse.Web.ViewModels.Category;
using Xunit;
using FluentAssertions.AspNetCore.Mvc;

namespace Warehouse.Core.UnitTests.Controllers.Categories
{
    public class DetailsTests : BaseTest
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
            MockMapper.Setup(x => x.Map<CategoryViewModel>(It.IsAny<Category>()));

            return controller;
        }
        [Fact]
        public void Should_RedirectTo_Index_When_ResultIs_Failure()
        {
            var controller = Create();
            CategoryResult = Result.Failure<Category>("Property", "Error");
            var result = controller.Details(ViewModel.Id);
            result.Should()
            .BeRedirectToRouteResult()
            .WithAction("Index");
        }
        [Fact]
        public void Should_Return_View_With_Data_When_ResultIs_Failure()
        {
            var controller = Create();
            var result = controller.Details(ViewModel.Id);
            result.Should()
            .BeViewResult()
            .WithDefaultViewName()
            .Model
            .Should()
            .BeEquivalentTo(ViewModel);
        }
    }
}
