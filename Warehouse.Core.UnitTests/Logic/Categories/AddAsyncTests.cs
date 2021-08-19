using FizzWare.NBuilder;
using FluentAssertions;
using FluentValidation;
using Moq;
using System;
using System.Threading.Tasks;
using Warehouse.Core.Entities;
using Warehouse.Core.Interfaces.Repositories;
using Warehouse.Core.Logic;
using Warehouse.Core.UnitTests.Extensions;
using Warehouse.Core.UnitTests.Logic.Categories.Infrastructure;
using Xunit;
using Warehouse.Core.UnitTests.CustomAssertions;

namespace Warehouse.Core.UnitTests.Logic.Categories
{
    public class AddAsyncTests : BaseTest
    {
        public Category Category;
        public void CorrectFlow()
        {
            Category = Builder<Category>
                .CreateNew()
                .Build();

            MockCategoryRepository.Setup(x => x.AddAsync(Category)).ReturnsAsync(Category);
            MockValidator.SetValidationSuccess();
        }

        protected override CategoryLogic Create()
        {
            var categoryLogic = base.Create();
            CorrectFlow();


            return categoryLogic;
        }


        [Fact]
        public void Should_Throw_ArgumentNullException_When_GivenCategory_Null()
        {
            //arrange
            var categoryLogic = Create();
            //act
            Func<Task> act = async () => await categoryLogic.AddAsync(null);
            //assert
            act.Should().ThrowAsync<ArgumentNullException>();
            MockValidator.Verify(
                x => x.Validate(It.IsAny<Category>()),
                Times.Never);
            MockCategoryRepository.Verify(
                x => x.AddAsync(It.IsAny<Category>()),
                Times.Never);
            MockCategoryRepository.Verify(
                x => x.SaveChangesAsync(),
                Times.Never);
        }

        [Fact]
        public async Task Should_Return_ResultFailure_When_Validation_Failed()
        {
            ////arrange
            var categoryLogic = Create();
            var validatedProperty = "test";
            var errorMessage = "test error message";
            MockValidator.SetValidationFailure(validatedProperty, errorMessage);

            //act
            var result = await categoryLogic.AddAsync(Category);
            //assert
            result.Should().BeFailure(validatedProperty,errorMessage);
            MockValidator.Verify(
                x => x.Validate(Category),
                Times.Once);
            MockCategoryRepository.Verify(
                x => x.AddAsync(It.IsAny<Category>()),
                Times.Never);
            MockCategoryRepository.Verify(
                x => x.SaveChangesAsync(),
                Times.Never);

        }

       
        [Fact]
        public async Task Should_Return_AddedCategory_And_ResultOk()
        {
            ////arrange
            var categoryLogic = Create();
            //act
            var result = await categoryLogic.AddAsync(Category);
            //assert
            result.Should().BeSuccess(Category);
            MockValidator.Verify(
                x => x.Validate(Category),
                Times.Once);
            MockCategoryRepository.Verify(
                x => x.AddAsync(Category),
                Times.Once);
            MockCategoryRepository.Verify(
                x => x.SaveChangesAsync(),
                Times.Once);
        }

    }
}
