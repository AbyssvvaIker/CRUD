using FizzWare.NBuilder;
using FluentAssertions.AspNetCore.Mvc;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Warehouse.Core.Entities;
using Warehouse.Web.UnitTests.Products.Infrastructure;
using Warehouse.Web.Controllers;
using Warehouse.Web.ViewModels.Product;
using Xunit;
using Warehouse.Web.ViewModels;
using Warehouse.Core;

namespace Warehouse.Web.UnitTests.Products
{
    public class CreateGetTests : BaseTest
    {
        protected ProductViewModel ViewModel { get; set; }
        protected Result<IEnumerable<Category>> CategoriesResult { get; set; }

        protected override ProductsController Create()
        {
            var controller = base.Create();;

            CategoriesResult = Builder<Result<IEnumerable<Category>>>
            .CreateNew()
            .Build();

            MockCategoryLogic.Setup(x => x.GetAllActiveAsync()).ReturnsAsync(CategoriesResult);
            MockMapper.Setup(x => x.Map<IList<SelectItemViewModel>>(CategoriesResult.Value));

            return controller;
        }

        [Fact]
        public async Task Should_Return_View_With_Empty_ViewModel()
        {
            //arrange
            var controller = Create();
            //act
            var result =await controller.Create();
            //assert
            result.Should()
                .BeViewResult()
                .WithDefaultViewName()
                .Model
                .Should()
                .BeOfType(typeof(ProductViewModel));

            MockCategoryLogic.Verify(
                x => x.GetAllActiveAsync(),
                Times.Once);
            MockMapper.Verify(
                x => x.Map<IList<SelectItemViewModel>>(CategoriesResult.Value),
                Times.Once);
        }
    }
}
