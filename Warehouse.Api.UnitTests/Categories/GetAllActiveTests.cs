using FizzWare.NBuilder;
//using FluentAssertions;
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
    public class GetAllActiveTests : BaseTest
    {
        protected IEnumerable<Category> Categories { get; set; }
        protected Result<IEnumerable<Category>> CategoriesResult { get; set; }
        protected Result<IEnumerable<CategoryDto>> DtoResult { get; set; }
        protected IEnumerable<CategoryDto> Dto { get; set; }
        protected override CategoryController Create()
        {
            var controller = base.Create();

            Categories = Builder<Category>.CreateListOfSize(5)
                .Build();

            Dto = Builder<CategoryDto>.CreateListOfSize(5)
                .Build();

            CategoriesResult = Result.Ok(Categories);
            DtoResult = Result.Ok(Dto);

            MockCategoryLogic.Setup(x => x.GetAllActiveAsync()).ReturnsAsync(CategoriesResult);
            MockMapper.Setup(x => x.Map<IEnumerable<CategoryDto>>(It.IsAny<IEnumerable<Category>>()))
                .Returns(Dto);

            return controller;
        }
        [Fact]
        public async Task Should_Be_BadRequest_When_ResultIs_Failure()
        {
            //arrange
            var controller = Create();
            var property = "property";
            var message = "message";
            CategoriesResult = Result.Failure<IEnumerable<Category>>(property, message);
            MockCategoryLogic.Setup(x => x.GetAllActiveAsync()).ReturnsAsync(CategoriesResult);
            //act
            var result =await controller.GetAllActive();
            //assert
            result.Should()
                .BeBadRequest<IEnumerable<Category>>(property, message);

            MockCategoryLogic.Verify(x =>
            x.GetAllActiveAsync(),
            Times.Once);

            MockMapper.Verify(x =>
            x.Map<IEnumerable<CategoryDto>>(It.IsAny<IEnumerable<Category>>()),
            Times.Never);
        }
        [Fact]
        public async Task Should_Be_Ok_When_ResultIs_Ok()
        {
            //arrange
            var controller = Create();
            //act
            var result = await controller.GetAllActive();
            //assert
            result.Should()
                .BeOk(Dto);

            MockCategoryLogic.Verify(x =>
            x.GetAllActiveAsync(),
            Times.Once);

            MockMapper.Verify(x =>
            x.Map<IEnumerable<CategoryDto>>(Categories),
            Times.Once);
        }
    }
}
