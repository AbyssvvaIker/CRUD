using FizzWare.NBuilder;
using FluentAssertions.AspNetCore.Mvc;
using FluentAssertions;
using Moq;
using System;
using System.Threading.Tasks;
using Warehouse.Core.Entities;
using Warehouse.Web.UnitTests.Categories.Infrastructure;
using Warehouse.Web.Controllers;
using Warehouse.Web.ViewModels.Category;
using Xunit;
using System.Collections.Generic;
using Warehouse.Core;

namespace Warehouse.Web.UnitTests.Categories
{
    public class IndexTests : BaseTest
    {
        protected IList<IndexItemViewModel> Categories { get; set; }
        protected Result<IEnumerable<Category>> CategoriesResult { get; set; }

        protected override CategoryController Create()
        {
            var controller = base.Create();
            Categories = Builder<IndexItemViewModel>
                .CreateListOfSize(5)
                .Build();

            CategoriesResult = Builder<Result<IEnumerable<Category>>>
            .CreateNew()
            .Build();

            MockCategoryLogic.Setup(x => x.GetAllActiveAsync()).ReturnsAsync(CategoriesResult);
            MockMapper.Setup(x => x.Map<IList<IndexItemViewModel>>(CategoriesResult.Value)).Returns(Categories);

            return controller;
        }

        [Fact]
        public async Task Should_Return_View_With_ListOf_IndexViewModel()
        {
            //arrange
            var controller = Create();
            //act
            var result =await controller.Index();
            //assert
            result.Should()
                .BeViewResult()
                .WithDefaultViewName()
                .Model
                .Should()
                .BeEquivalentTo(Categories);

            MockCategoryLogic.Verify(
                x => x.GetAllActiveAsync(),
                Times.Once);
            MockMapper.Verify(
                x => x.Map<IList<IndexItemViewModel>>(CategoriesResult.Value),
                Times.Once);
        }
    }
}
