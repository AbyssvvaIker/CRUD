using FizzWare.NBuilder;
using FluentAssertions;
using FluentValidation;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Warehouse.Core.Entities;
using Warehouse.Core.Interfaces.Repositories;
using Warehouse.Core.Logic;
using Warehouse.Core.UnitTests.Extensions;
using Warehouse.Core.UnitTests.Logic.Categories.Infrastructure;
using Xunit;

namespace Warehouse.Core.UnitTests.Logic.Categories
{
    public class GetAllActiveAsyncTests : BaseTest
    {
        public IEnumerable<Category> Categories;
        public void CorrectFlow()
        {
            Categories = Builder<Category>
                .CreateListOfSize(3)
                .All()
                .With(x => x.IsActive = true)
                .Build();

            mockCategoryRepository.Setup(x => x.GetAllActiveAsync()).ReturnsAsync(Categories);
            mockValidator.SetValidationSuccess();
        }

        public override CategoryLogic Create()
        {
            var categoryLogic = base.Create();
            CorrectFlow();

            return categoryLogic;
        }
        [Fact]
        public async Task Should_Return_ResultOk_With_ListOfCategories()
        {
            var categoryLogic = Create();
            mockCategoryRepository.Setup(x => x.GetAllActiveAsync()).ReturnsAsync(Categories);
            var result = await categoryLogic.GetAllActiveAsync();

            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Value.Should().BeSameAs(Categories);
            mockCategoryRepository.Verify(
                x => x.GetAllActiveAsync(),
                Times.Once);

        }
        [Fact]
        public async Task Should_Return_ResultOk_With_EmptyList_When_NoActiveCategories()
        {
            var categoryLogic = Create();
            mockCategoryRepository.Setup(x => x.GetAllActiveAsync()).ReturnsAsync(Categories);

            var result = await categoryLogic.GetAllActiveAsync();

            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Value.Should().BeSameAs(Categories);

            mockCategoryRepository.Verify(
                x => x.GetAllActiveAsync(),
                Times.Once);
        }
    }
}
