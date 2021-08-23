using FizzWare.NBuilder;
using FluentAssertions.AspNetCore.Mvc;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Warehouse.Core.Entities;
using Warehouse.Core.UnitTests.Controllers.Products.Infrastructure;
using Warehouse.Web.Controllers;
using Warehouse.Web.ViewModels.Product;
using Xunit;

namespace Warehouse.Core.UnitTests.Controllers.Products
{
    public class DeleteGetTests : BaseTest
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
            MockMapper.Setup(x => x.Map<ProductViewModel>(It.IsAny<Category>())).Returns(ViewModel);

            return controller;
        }

        [Fact]
        public async Task Should_Be_NotFound_When_GivenId_IsNull()
        {
            //arrange
            var controller = Create();
            //act
            var result =await controller.Delete(null);
            //assert
            result.Should()
                .BeNotFoundResult();

            MockCategoryLogic.Verify(
                x => x.GetByIdAsync(It.IsAny<Guid>()),
                Times.Never);
            MockMapper.Verify(
                x => x.Map<ProductViewModel>(It.IsAny<Category>()),
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
            var result =await controller.Delete(Category.Id);
            //assert
            result.Should()
                .BeNotFoundResult();

            MockCategoryLogic.Verify(
                x => x.GetByIdAsync(Category.Id),
                Times.Once);
            MockMapper.Verify(
                x => x.Map<ProductViewModel>(It.IsAny<Category>()),
                Times.Never);
        }
        [Fact]
        public async Task Should_Return_View_With_ViewModel_When_ResultIs_Ok()
        {
            //arrange
            var controller = Create();
            //act
            var result = await controller.Delete(Category.Id);
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
                x => x.Map<ProductViewModel>(CategoryResult.Value),
                Times.Once);
        }

    }
}
