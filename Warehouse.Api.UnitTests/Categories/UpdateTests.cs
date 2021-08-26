using FizzWare.NBuilder;
using Moq;
using System;
using System.Threading.Tasks;
using Warehouse.Api.Controllers;
using Warehouse.Api.DTOs.Category;
using Warehouse.Api.UnitTests.Categories.Infrastructure;
using Warehouse.Core;
using Warehouse.Core.Entities;
using Xunit;

namespace Warehouse.Api.UnitTests.Categories
{
    public class UpdateTests : BaseTest
    {
        protected Category Category { get; set; }
        protected Result<Category> GetResult { get; set; }
        protected Result<Category> UpdateResult { get; set; }
        protected Result<CategoryDto> DtoResult { get; set; }
        protected CategoryDto Dto { get; set; }
        protected override CategoryController Create()
        {
            var controller = base.Create();

            Category = Builder<Category>.CreateNew()
                .Build();

            Dto = Builder<CategoryDto>.CreateNew()
                .Build();

            GetResult = Result.Ok(Category);
            UpdateResult = Result.Ok(Category);
            DtoResult = Result.Ok(Dto);

            MockCategoryLogic.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(GetResult);
            MockCategoryLogic.Setup(x => x.UpdateAsync(It.IsAny<Category>())).ReturnsAsync(UpdateResult);
            MockMapper.Setup(x => x.Map<CategoryDto>(It.IsAny<Category>())).Returns(Dto);

            return controller;
        }

        [Fact]
        public async Task Should_Be_NotFound_When_GetResultIs_Failure()
        {
            var controller = Create();
            var property = "property";
            var message = "message";
            GetResult = Result.Failure<Category>(property, message);
            //act
            var result = await controller.Update(Category.Id, Dto);
            //assert
            result.Should()
                .BeNotFound<Result<Category>>(property, message, string.Empty);
        }
        [Fact]
        public async Task Should_Be_BadRequest_When_UpdateResultIs_Failure()
        {
            var controller = Create();
            var property = "property";
            var message = "message";
            UpdateResult = Result.Failure<Category>(property, message);
            //act
            var result = await controller.Update(Category.Id, Dto);
            //assert
            result.Should()
                .BeBadRequest<Result<Category>>(property, message, string.Empty);
        }
        [Fact]
        public async Task Should_Be_Ok_When_ResultAre_Ok()
        {

            var controller = Create();
            //act
            var result = await controller.Add(Dto);
            //assert
            result.Should()
                .BeOk(DtoResult);
        }
    }
}
