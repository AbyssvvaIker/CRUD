using FizzWare.NBuilder;
using FluentAssertions.AspNetCore.Mvc;
using FluentAssertions;
using Moq;
using System;
using System.Threading.Tasks;
using Warehouse.Core.Entities;
using Warehouse.Core.UnitTests.Controllers.Categories.Infrastructure;
using Warehouse.Web.Controllers;
using Warehouse.Web.ViewModels.Category;
using Xunit;
using System.Collections.Generic;

namespace Warehouse.Core.UnitTests.Controllers.Categories
{
    public class IndexTests : BaseTest
    {
        protected IList<IndexItemViewModel> Categories { get; set; }
        protected IList<IndexItemViewModel> ViewModel { get; set; }
        protected Result<IEnumerable<Category>> CategoriesResult { get; set; }

        protected override CategoryController Create()
        {
            var controller = base.Create();
            Categories = Builder<IndexItemViewModel>
                .CreateListOfSize(5)
                .Build();

            ViewModel = Builder<IndexItemViewModel>
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
            var controller = Create();

            var result =await controller.Index();

            result.Should()
                .BeViewResult()
                .WithDefaultViewName()
                .Model
                .Should()
                .BeEquivalentTo(ViewModel);
        }
    }
}
