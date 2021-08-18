﻿using System;
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
namespace Warehouse.Core.UnitTests.Logic.Categories
{
    public class DeleteAsyncTests
    {
        [Fact]
        public async Task ShouldThrowArgumentNullException()
        {
            var mockCategoryRepository = new Mock<ICategoryRepository>();
            var mockProductRepository = new Mock<IProductRepository>();
            var mockValidator = new Mock<IValidator<Category>>();

            var categoryLogic = new CategoryLogic(mockCategoryRepository.Object, mockProductRepository.Object, mockValidator.Object);

            Func<Task> act = async () => await categoryLogic.DeleteAsync((Category)null);

            await act.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task ShouldReturnResultOk()
        {
            var category = Builder<Category>
                 .CreateNew()
                 .Build();

            var mockCategoryRepository = new Mock<ICategoryRepository>();
            var mockProductRepository = new Mock<IProductRepository>();
            var mockValidator = new Mock<IValidator<Category>>();

            var categoryLogic = new CategoryLogic(mockCategoryRepository.Object, mockProductRepository.Object, mockValidator.Object);

            var result =await categoryLogic.DeleteAsync(category);

            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
        }
    }
}
