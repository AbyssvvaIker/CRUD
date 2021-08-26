using FizzWare.NBuilder;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Warehouse.Api.Controllers;
using Warehouse.Api.DTOs.Category;
using Warehouse.Api.UnitTests.Categories.Infrastructure;
using Warehouse.Core;
using Warehouse.Core.Entities;
using Xunit;

namespace Warehouse.Api.UnitTests.Categories
{
    public class DeleteTests : BaseTest
    {
        protected Category Category { get; set; }
        protected Result<Category> GetResult { get; set; }
        protected Result<Category> DeleteResult { get; set; }
        protected override CategoryController Create()
        {
            var controller = base.Create();

            Category = Builder<Category>.CreateNew()
                .Build();

            GetResult = Result.Ok(Category);
            DeleteResult = Result.Ok(Category);

            MockCategoryLogic.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(GetResult);
            MockCategoryLogic.Setup(x => x.DeleteAsync(It.IsAny<Category>())).ReturnsAsync(DeleteResult);

            return controller;
        }
        [Fact]
        public async Task Should_Be_NotFound_When_GetResultIs_Failure()
        {
            var controller = Create();
            var property = "property";
            var message = "message";
            GetResult = Result.Failure<Category>(property, message);
            MockCategoryLogic.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(GetResult);
            //act
            var result = await controller.Delete(Category.Id);
            //assert
            result.Should()
                .BeNotFound<Result<Category>>(property, message, string.Empty);
            MockCategoryLogic.Verify(x =>
            x.GetByIdAsync(Category.Id),
            Times.Once);
            MockCategoryLogic.Verify(x =>
            x.DeleteAsync(It.IsAny<Category>()),
            Times.Never);
        }

        [Fact]
        public async Task Should_Be_BadRequest_When_DeleteResultIs_Failure()
        {
            //arrange
            var controller = Create();
            var property = "property";
            var message = "message";
            DeleteResult = Result.Failure<Category>(property, message);
            MockCategoryLogic.Setup(x => x.DeleteAsync(It.IsAny<Category>())).ReturnsAsync(DeleteResult);
            //act
            var result = await controller.Delete(Category.Id);
            //assert
            result.Should()
                .BeBadRequest<Result<Category>>(property, message, string.Empty);

            MockCategoryLogic.Verify(x =>
            x.GetByIdAsync(Category.Id),
            Times.Once);
            MockCategoryLogic.Verify(x =>
            x.DeleteAsync(Category),
            Times.Once);
        }
        [Fact]
        public async Task Should_Be_NoContent_When_ResultsAre_Ok()
        {
            //arrange
            var controller = Create();
            //act
            var result =await controller.Delete(Category.Id);
            //assert
            //result.Should()
            //    .BeNoContent();
        }

    }
}
