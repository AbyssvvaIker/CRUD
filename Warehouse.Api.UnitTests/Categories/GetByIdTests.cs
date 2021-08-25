using FizzWare.NBuilder;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Warehouse.Api.Controllers;
using Warehouse.Api.DTOs.Category;
using Warehouse.Api.UnitTests.Categories.Infrastructure;
using Warehouse.Core;
using Warehouse.Core.Entities;

namespace Warehouse.Api.UnitTests.Categories
{
    public class GetByIdTests : BaseTest
    {
        protected Result<Category> ResultCategory { get; set; }
        protected Result<CategoryDto> ResultCategoryDto { get; set; }
        protected CategoryDto Dto { get; set; }
        protected override CategoryController Create()
        {
            var controller = base.Create();
            ResultCategory = Builder<Result<Category>>.CreateNew()
                .Build();

            Dto = Builder<CategoryDto>.CreateNew()
                .Build();

            ResultCategoryDto = Result.Ok(Dto);

            MockCategoryLogic.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(ResultCategory);
            MockMapper.Setup(x => x.Map<CategoryDto>(It.IsAny<CategoryDto>())).Returns(Dto);

            return controller;
        }
    }
}
