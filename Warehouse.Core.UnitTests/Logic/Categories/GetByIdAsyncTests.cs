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

            mockCategoryRepository.Setup(x => x.GetByIdAsync(Category.Id)).ReturnsAsync(Category);
            mockValidator.SetValidationSuccess();
        }

        public override CategoryLogic Create()
        {
            var categoryLogic = base.Create();
            CorrectFlow();


            return categoryLogic;
        }
        [Fact]
        public async Task ShouldReturnResultOk()
        {
            var id = Guid.NewGuid();
            var category = Builder<Category>
                .CreateNew()
                .With(x => x.Id = id)
                .Build();

            var mockCategoryRepository = new Mock<ICategoryRepository>();
            mockCategoryRepository.Setup(x => x.GetByIdAsync(id)).ReturnsAsync(category);
            var mockProductRepository = new Mock<IProductRepository>();
            var mockValidator = new Mock<IValidator<Category>>();

            var categoryLogic = new CategoryLogic(mockCategoryRepository.Object, mockProductRepository.Object,
                mockValidator.Object);

            var result =await categoryLogic.GetByIdAsync(id);

            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Value.Should().BeSameAs(category);
        }
        [Fact]
        public async Task ShouldReturnResultFailure()
        {
            var id = Guid.NewGuid();

            var mockCategoryRepository = new Mock<ICategoryRepository>();
            mockCategoryRepository.Setup(x => x.GetByIdAsync(id)).ReturnsAsync((Category)null);
            var mockProductRepository = new Mock<IProductRepository>();
            var mockValidator = new Mock<IValidator<Category>>();

            var categoryLogic = new CategoryLogic(mockCategoryRepository.Object, mockProductRepository.Object,
                mockValidator.Object);

            var result = await categoryLogic.GetByIdAsync(id);

            result.Should().NotBeNull();
            result.Success.Should().BeFalse();
            result.Value.Should().BeSameAs(null);
        }
    }
}
