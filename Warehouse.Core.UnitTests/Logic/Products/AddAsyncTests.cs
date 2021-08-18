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
using Warehouse.Core.UnitTests.Logic.Products.Infrastructure;

namespace Warehouse.Core.UnitTests.Logic.Products
{
    public class AddAsyncTests : BaseTest
    {
        public Product Product;
        public void CorrectFlow()
        {
            Product = Builder<Product>
                .CreateNew()
                .Build();

            mockProductRepository.Setup(x => x.AddAsync(Product)).ReturnsAsync(Product);
            mockValidator.SetValidationSuccess();
        }

        public override ProductLogic Create()
        {
            var productLogic = base.Create();
            CorrectFlow();

            return productLogic;
        }
        [Fact]
        public async Task Should_Return_AddedProduct_And_ResultOk()
        {
            var productLogic = Create();
            mockProductRepository.Setup(x => x.AddAsync(Product)).ReturnsAsync(Product);

            var result = await productLogic.AddAsync(Product);
            
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Value.Should().BeSameAs(Product);


            mockValidator.Verify(
                x => x.Validate(It.IsAny<Product>()),
                Times.Once);
            mockProductRepository.Verify(
                x => x.AddAsync(It.IsAny<Product>()),
                Times.Once);
            mockProductRepository.Verify(
                x => x.SaveChangesAsync(),
                Times.Once);
        }

        [Fact]
        public async Task Should_Return_ResultFailure_When_Validation_Failed()
        {
            var productLogic = Create();
            mockProductRepository.Setup(x => x.AddAsync(Product)).ReturnsAsync((Product)null);
            string validatedProperty = "test";
            string errorMessage = "test error message";
            mockValidator.SetValidationFailure(validatedProperty, errorMessage);

            var result = await productLogic.AddAsync(Product);

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

            mockValidator.Verify(
                x => x.Validate(It.IsAny<Product>()),
                Times.Once);
            mockProductRepository.Verify(
                x => x.AddAsync(It.IsAny<Product>()),
                Times.Never);
            mockProductRepository.Verify(
                x => x.SaveChangesAsync(),
                Times.Never);
        }

        [Fact]
        public void Should_Throw_ArgumentNullException_When_GivenProduct_Null()
        {
            var productLogic = Create();

            Func<Task> act = async () => await productLogic.AddAsync(null);

            act.Should().ThrowAsync<ArgumentNullException>();


            mockValidator.Verify(
                x => x.Validate(It.IsAny<Product>()),
                Times.Never);
            mockProductRepository.Verify(
                x => x.AddAsync(It.IsAny<Product>()),
                Times.Never);
            mockProductRepository.Verify(
                x => x.SaveChangesAsync(),
                Times.Never);
        }
    }
}
