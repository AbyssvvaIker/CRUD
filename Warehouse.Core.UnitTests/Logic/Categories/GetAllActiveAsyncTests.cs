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
    public class GetAllActiveAsyncTests :BaseTest
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
            var listActive = Builder<Category>
                .CreateListOfSize(3)
                .All()
                .With(x => x.IsActive = true)
                .Build();

            var mockCategoryRepository = new Mock<ICategoryRepository>();
            mockCategoryRepository.Setup(x => x.GetAllActiveAsync()).ReturnsAsync(listActive);
            var mockProductRepository = new Mock<IProductRepository>();
            var mockValidator = new Mock<IValidator<Category>>();

            var categoryLogic = new CategoryLogic(mockCategoryRepository.Object, mockProductRepository.Object, mockValidator.Object);

            var result =await categoryLogic.GetAllActiveAsync();

            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Value.Should().BeSameAs(listActive);
            
        }
        [Fact]
        public async Task Should_Return_ResultOk_With_EmptyList_When_NoActiveCategories()
        {
            var listActive = new List<Category>();

            var mockCategoryRepository = new Mock<ICategoryRepository>();
            mockCategoryRepository.Setup(x => x.GetAllActiveAsync()).ReturnsAsync(listActive);
            var mockProductRepository = new Mock<IProductRepository>();
            var mockValidator = new Mock<IValidator<Category>>();

            var categoryLogic = new CategoryLogic(mockCategoryRepository.Object, mockProductRepository.Object, mockValidator.Object);

            var result = await categoryLogic.GetAllActiveAsync();

            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Value.Should().BeSameAs(listActive);
        }
    }
}
