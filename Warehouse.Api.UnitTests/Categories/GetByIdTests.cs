using FizzWare.NBuilder;
//using FluentAssertions.AspNetCore.Mvc;
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
    public class GetByIdTests : BaseTest
    {
        protected Category Category { get; set; }
        protected Result<Category> CategoryResult { get; set; }
        protected Result<CategoryDto> DtoResult { get; set; }
        protected CategoryDto Dto { get; set; }
        protected override CategoryController Create()
        {
            var controller = base.Create();

            Category = Builder<Category>.CreateNew()
                .Build();

            Dto = Builder<CategoryDto>.CreateNew()
                .Build();

            CategoryResult = Result.Ok(Category);
            DtoResult = Result.Ok(Dto);

            MockCategoryLogic.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(CategoryResult);
            MockMapper.Setup(x => x.Map<CategoryDto>(It.IsAny<Category>())).Returns(Dto);

            return controller;
        }

        [Fact]
        public async Task Should_Be_NotFound_When_ResultIs_Failure()
        {
            //arrange
            var controller = Create();
            var property = "property";
            var message = "message";
            CategoryResult = Result.Failure<Category>(property, message);
            MockCategoryLogic.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(CategoryResult);
            //act
            var result =await controller.GetById(Category.Id);
            //assert
            result.Should()
                .BeNotFound<Category>(property,message);

            MockCategoryLogic.Verify(x =>
            x.GetByIdAsync(Category.Id),
            Times.Once);

            MockMapper.Verify(x =>
            x.Map<CategoryDto>(It.IsAny<Category>()),
            Times.Never);

        }

        [Fact]
        public async Task Should_Return_ResultDto_When_ResultIs_Ok()
        {
            //arrange
            var controller = Create();
            //act
            var result =await controller.GetById(Category.Id);
            //assert
            result.Should()
                .BeOk(Dto);

            MockCategoryLogic.Verify(x =>
            x.GetByIdAsync(Category.Id),
            Times.Once);

            MockMapper.Verify(x =>
            x.Map<CategoryDto>(Category),
            Times.Once);
        }
    }
}
