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
        protected Result<Category> ResultCategory { get; set; }
        protected Result<CategoryDto> ResultCategoryDto { get; set; }
        protected CategoryDto Dto { get; set; }
        protected override CategoryController Create()
        {
            var controller = base.Create();

            Category = Builder<Category>.CreateNew()
                .Build();

            Dto = Builder<CategoryDto>.CreateNew()
                .Build();

            ResultCategory = Result.Ok(Category);
            ResultCategoryDto = Result.Ok(Dto);

            MockCategoryLogic.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(ResultCategory);
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
            ResultCategory = Result.Failure<Category>(property, message);

            MockCategoryLogic.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(ResultCategory);
            //act
            var result =await controller.GetById(Category.Id);
            //assert
            result.Should()
                .BeNotFound<Result<Category>>(message);



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
                .BeOk(ResultCategoryDto);
        }
    }
}
