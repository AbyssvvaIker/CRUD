﻿using FizzWare.NBuilder;
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
    public class UpdateAsyncTests :BaseTest
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
            Func<Task> act = async () => await categoryLogic.UpdateAsync(null);
            //assert
            await act.Should().ThrowAsync<ArgumentNullException>();

            MockValidator.Verify(
                x => x.Validate(It.IsAny<Category>()),
                Times.Never);
            MockCategoryRepository.Verify(
                x => x.SaveChangesAsync(),
                Times.Never);
        }

        [Fact]
        public async Task Should_Return_ResultFailure_When_ValidationFailed()
        {
            //arrange
            var categoryLogic = Create();
            var validatedProperty = "test";
            var errorMessage = "test error message";
            MockValidator.SetValidationFailure(validatedProperty, errorMessage);
            //act
            var result = await categoryLogic.UpdateAsync(Category);
            //assert
            //result.Should().NotBeNull();
            //result.Success.Should().BeFalse();
            //result.Errors.Should().HaveCount(1);
            result.Should().BeFailure(validatedProperty, errorMessage);

            //foreach (var err in result.Errors)
            //{
            //    err.Should().BeEquivalentTo(new ErrorMessage()
            //    {
            //        PropertyName = validatedProperty,
            //        Message = errorMessage,
            //    });
            //}

            MockValidator.Verify(
                x => x.Validate(Category),
                Times.Once);
            MockCategoryRepository.Verify(
                x => x.SaveChangesAsync(),
                Times.Never);
        }

        [Fact]
        public async Task Should_Return_ResultOk()
        {
            ////arrange
            var categoryLogic = Create();
            //act
            var result = await categoryLogic.UpdateAsync(Category);
            //assert
            //result.Should().NotBeNull();
            //result.Success.Should().BeTrue();
            //result.Value.Should().BeSameAs(Category);
            result.Should().BeSuccess(Category);

            MockValidator.Verify(
                x => x.Validate(Category),
                Times.Once);
            MockCategoryRepository.Verify(
                x => x.SaveChangesAsync(),
                Times.Once);
        }
    }
}
