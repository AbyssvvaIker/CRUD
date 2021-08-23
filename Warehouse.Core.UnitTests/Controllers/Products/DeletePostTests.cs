﻿using FizzWare.NBuilder;
using FluentAssertions.AspNetCore.Mvc;
using FluentAssertions;
using Moq;
using System;
using System.Threading.Tasks;
using Warehouse.Core.Entities;
using Warehouse.Core.UnitTests.Controllers.Products.Infrastructure;
using Warehouse.Web.Controllers;
using Warehouse.Web.ViewModels.Product;
using Xunit;
using Warehouse.Core.UnitTests.CustomAssertions;

namespace Warehouse.Core.UnitTests.Controllers.Products
{
    public class DeletePostTests : BaseTest
    {
        protected Category Category { get; set; }
        protected ProductViewModel ViewModel { get; set; }
        protected Result<Category> CategoryResult { get; set; }

        protected override ProductsController Create()
        {
            var controller = base.Create();
            Category = Builder<Category>
                .CreateNew()
                .Build();

            ViewModel = Builder<ProductViewModel>
                .CreateNew()
                .Build();

            CategoryResult = Result.Ok(Category);

            MockCategoryLogic.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(() => CategoryResult);
            //MockMapper.Setup(x => x.Map(It.IsAny<CategoryViewModel>(), It.IsAny<Category>())).Returns(Category);

            return controller;
        }

        [Fact]
        public async Task Should_RedirectToAction_Index_When_GetResultIs_Failure()
        {
            //arrange
            var controller = Create();
            var errorProperty = "property";
            var errorMessage = "error message";
            CategoryResult = Result.Failure<Category>(errorProperty, errorMessage);
            //act
            var result =await controller.DeleteConfirmed(Category.Id);
            //assert
            result.Should()
                .BeRedirectToActionResult()
                .WithActionName(nameof(Index));
            controller.Should()
                .HasError(errorProperty, errorMessage);


            MockCategoryLogic.Verify(
                x => x.GetByIdAsync(Category.Id),
                Times.Once);
            MockCategoryLogic.Verify(
                x => x.DeleteAsync(It.IsAny<Category>()),
                Times.Never);

        }
        [Fact]
        public async Task Should_RedirectToAction_Index_When_GetResultIs_Ok()
        {
            //arrange
            var controller = Create();
            //act
            var result =await controller.DeleteConfirmed(Category.Id);
            //assert
            result.Should()
                .BeRedirectToActionResult()
                .WithActionName(nameof(Index));

            MockCategoryLogic.Verify(
                x => x.GetByIdAsync(Category.Id),
                Times.Once);
            MockCategoryLogic.Verify(
                x => x.DeleteAsync(CategoryResult.Value),
                Times.Once);
        }
    }
}
