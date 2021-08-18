using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Moq;
using FluentAssertions;
using FizzWare.NBuilder;
using Warehouse.Core.Interfaces.Repositories;
using Warehouse.Core.Entities;
using Warehouse.Core.Interfaces;
using FluentValidation;
using Warehouse.Core.Logic;
using System.Threading.Tasks;
using Warehouse.Core.UnitTests.Extensions;
using Warehouse.Core.UnitTests.Logic.Categories.Infrastructure;

namespace Warehouse.Core.UnitTests.Logic.Categories
{
    public class GetByIdAsyncTests :BaseTest
    {
        public Category Category;
        public void CorrectFlow()
        {
            Category = Builder<Category>
                .CreateNew()
                .With(x => x.Id = Guid.NewGuid())
                .Build();

            MockCategoryRepository.Setup(x => x.GetByIdAsync(Category.Id)).ReturnsAsync(Category);
            MockValidator.SetValidationSuccess();
        }

        protected override CategoryLogic Create()
        {
            var categoryLogic = base.Create();
            CorrectFlow();


            return categoryLogic;
        }
        [Fact]
        public async Task Should_Return_ResultOk()
        {
            var categoryLogic = Create();
            MockCategoryRepository.Setup(x => x.GetByIdAsync(Category.Id)).ReturnsAsync(Category);
            var result =await categoryLogic.GetByIdAsync(Category.Id);

            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Value.Should().BeSameAs(Category);

            MockCategoryRepository.Verify(
                x => x.GetByIdAsync(It.IsAny<Guid>()),
                Times.Once);
        }
        [Fact]
        public async Task Should_Return_ResultFailure_When_CategoryDoesNotExist()
        {
            var categoryLogic = Create();
            MockCategoryRepository.Setup(x => x.GetByIdAsync(Category.Id)).ReturnsAsync((Category)null);

            var result = await categoryLogic.GetByIdAsync(Category.Id);

            result.Should().NotBeNull();
            result.Success.Should().BeFalse();
            result.Value.Should().BeSameAs(null);

            MockCategoryRepository.Verify(
                x => x.GetByIdAsync(It.IsAny<Guid>()),
                Times.Once);
        }
    }
}
