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
    public class DeleteAsyncTests :BaseTest
    {
        public Category Category;
        public void CorrectFlow()
        {
            Category = Builder<Category>
                .CreateNew()
                .Build();
            mockValidator.SetValidationSuccess();
        }

        public override CategoryLogic Create()
        {
            var categoryLogic = base.Create();
            CorrectFlow();


            return categoryLogic;
        }

        [Fact]
        public async Task Should_Throw_ArgumentNullException_When_GivenCategory_Null()
        {
            var categoryLogic = Create();
            Func<Task> act = async () => await categoryLogic.DeleteAsync(null);

            await act.Should().ThrowAsync<ArgumentNullException>();
            mockProductRepository.Verify(
                x => x.DeleteByCategoryIdAsync(It.IsAny<Guid>()),
                Times.Never);
            mockCategoryRepository.Verify(
                x => x.Delete(It.IsAny<Category>()),
                Times.Never);
             mockCategoryRepository.Verify(
                x => x.SaveChangesAsync(),
                Times.Never);
        }

        [Fact]
        public async Task Should_Return_ResultOk()
        {
            var categoryLogic = Create();
            var result =await categoryLogic.DeleteAsync(Category);

            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            mockProductRepository.Verify(
                x => x.DeleteByCategoryIdAsync(It.IsAny<Guid>()),
                Times.Once);
            mockCategoryRepository.Verify(
                x => x.Delete(It.IsAny<Category>()),
                Times.Once);
            mockCategoryRepository.Verify(
                x => x.SaveChangesAsync(),
                Times.Once);
        }
    }
}
