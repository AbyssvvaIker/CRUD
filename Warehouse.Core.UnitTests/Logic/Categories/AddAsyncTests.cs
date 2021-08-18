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
        public async Task Should_Return_AddedCategory_And_ResultOk()
        {
            ////arrange
            
            var categoryLogic = Create();
            //act
            var result = await categoryLogic.AddAsync(Category);
            //assert
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Value.Should().BeSameAs(Category);

            MockValidator.Verify(
                x => x.Validate(It.IsAny<Category>()),
                Times.Once);
            MockCategoryRepository.Verify(
                x => x.AddAsync(It.IsAny<Category>()),
                Times.Once);
            MockCategoryRepository.Verify(
                x => x.SaveChangesAsync(),
                Times.Once);
        }

        [Fact]
        public async Task Should_Return_ResultFailure_When_Validation_Failed()
        {
            ////arrange
            var categoryLogic = Create();
            MockCategoryRepository.Setup(x => x.AddAsync(Category)).ReturnsAsync((Category)null);
            string validatedProperty = "test";
            string errorMessage = "test error message";
            MockValidator.SetValidationFailure(validatedProperty, errorMessage);

            //act
            var result = await categoryLogic.AddAsync(Category);
            //assert
            result.Should().NotBeNull();
            result.Success.Should().BeFalse();
            result.Errors.Should().HaveCount(1);
            foreach (var err in result.Errors)
            {
                err.Should().BeEquivalentTo(new ErrorMessage()
                {
                    PropertyName = validatedProperty,
                    Message = errorMessage,
                });
            }
            MockValidator.Verify(
                x => x.Validate(It.IsAny<Category>()),
                Times.Once);
            MockCategoryRepository.Verify(
                x => x.AddAsync(It.IsAny<Category>()),
                Times.Never);
            MockCategoryRepository.Verify(
                x => x.SaveChangesAsync(),
                Times.Never);

        }

        [Fact]
        public void Should_Throw_ArgumentNullException_When_GivenCategory_Null()
        {
            var categoryLogic = Create();
            Func<Task> act = async () => await categoryLogic.AddAsync(null);

            act.Should().ThrowAsync<ArgumentNullException>();
            MockValidator.Verify(
                x => x.Validate(Category),
                Times.Never);
            MockCategoryRepository.Verify(
                x => x.AddAsync(It.IsAny<Category>()),
                Times.Never);
            MockCategoryRepository.Verify(
                x => x.SaveChangesAsync(),
                Times.Never);
        }
    }
}
