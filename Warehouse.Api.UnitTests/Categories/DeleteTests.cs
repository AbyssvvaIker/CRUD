﻿using FizzWare.NBuilder;
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
            DeleteResult = Result.Ok(Category);
            DtoResult = Result.Ok(Dto);

            MockCategoryLogic.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(GetResult);
            MockCategoryLogic.Setup(x => x.DeleteAsync(It.IsAny<Category>())).ReturnsAsync(DeleteResult);
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
            var result = await controller.Delete(Category.Id);
            //assert
            result.Should()
                .BeNotFound<Result<Category>>(property, message, string.Empty);
        }

        [Fact]
        public async Task Should_Be_BadRequest_When_DeleteResultIs_Failure()
        {

            var controller = Create();
            var property = "property";
            var message = "message";
            DeleteResult = Result.Failure<Category>(property, message);
            //act
            var result = await controller.Delete(Category.Id);
            //assert
            result.Should()
                .BeBadRequest<Result<Category>>(property, message, string.Empty);
        }

    }
}
