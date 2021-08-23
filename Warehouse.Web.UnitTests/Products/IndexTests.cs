using FizzWare.NBuilder;
using FluentAssertions.AspNetCore.Mvc;
using FluentAssertions;
using Moq;
using System;
using System.Threading.Tasks;
using Warehouse.Core.Entities;
using Warehouse.Web.UnitTests.Products.Infrastructure;
using Warehouse.Web.Controllers;
using Warehouse.Web.ViewModels.Product;
using Xunit;
using System.Collections.Generic;
using Warehouse.Core;

namespace Warehouse.Web.UnitTests.Products
{
    public class IndexTests : BaseTest
    {
        protected IList<IndexItemViewModel> Products { get; set; }
        //protected IList<IndexItemViewModel> ViewModel { get; set; }
        protected Result<IEnumerable<Product>> ProductsResult { get; set; }

        protected override ProductsController Create()
        {
            var controller = base.Create();
            Products = Builder<IndexItemViewModel>
                .CreateListOfSize(5)
                .Build();

            //ViewModel = Builder<IndexItemViewModel>
            //    .CreateListOfSize(5)
            //    .Build();

            ProductsResult = Builder<Result<IEnumerable<Product>>>
            .CreateNew()
            .Build();
            ProductsResult.Value = Builder<Product>
                .CreateListOfSize(5)
                .Build();
            ProductsResult.Success = true;

            MockProductLogic.Setup(x => x.GetAllActiveAsync()).ReturnsAsync(ProductsResult);
            MockMapper.Setup(x => x.Map<IList<IndexItemViewModel>>(ProductsResult.Value)).Returns(Products);

            return controller;
        }

        [Fact]
        public async Task Should_Be_NotFound_When_ResultIs_Failure()
        {
            //arrange
            var controller = Create();
            ProductsResult.Success = false;
            //act
            var result = await controller.Index();
            //assert
            result.Should()
                .BeNotFoundResult();

            MockProductLogic.Verify(
                x => x.GetAllActiveAsync(),
                Times.Once);
            MockMapper.Verify(
                x => x.Map<IList<IndexItemViewModel>>(It.IsAny<Result<IEnumerable<Product>>>()),
                Times.Never);
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
                .BeEquivalentTo(Products);

            MockProductLogic.Verify(
                x => x.GetAllActiveAsync(),
                Times.Once);
            MockMapper.Verify(
                x => x.Map<IList<IndexItemViewModel>>(ProductsResult.Value),
                Times.Once);
        }
    }
}
