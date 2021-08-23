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
using Warehouse.Core.UnitTests.CustomAssertions;

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
            MockValidator.SetValidationSuccess();
        }

        protected override CategoryLogic Create()
        {
            var categoryLogic = base.Create();
            CorrectFlow();


            return categoryLogic;
        }

        [Fact]
        public async Task Should_Throw_ArgumentNullException_When_GivenCategory_Null()
        {
            //arrange
            var categoryLogic = Create();
            
            //act
            Func<Task> act = async () => await categoryLogic.DeleteAsync(null);

            //assert
            await act.Should().ThrowAsync<ArgumentNullException>();
            MockProductRepository.Verify(
                x => x.DeleteByCategoryIdAsync(It.IsAny<Guid>()),
                Times.Never);
            MockCategoryRepository.Verify(
                x => x.Delete(It.IsAny<Category>()),
                Times.Never);
             MockCategoryRepository.Verify(
                x => x.SaveChangesAsync(),
                Times.Never);
        }

        [Fact]
        public async Task Should_Return_ResultOk()
        {
            //arrange
            var categoryLogic = Create();
            //act
            var result =await categoryLogic.DeleteAsync(Category);
            //assert
            result.Should().BeSuccess();

            MockProductRepository.Verify(
                x => x.DeleteByCategoryIdAsync(Category.Id),
                Times.Once);
            MockCategoryRepository.Verify(
                x => x.Delete(Category),
                Times.Once);
            MockCategoryRepository.Verify(
                x => x.SaveChangesAsync(),
                Times.Once);
        }
    }
}
