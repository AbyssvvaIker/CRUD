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
    public class GetByIdAsyncTests : BaseTest
    {
        public Product CorrectFlow(Mock<IProductRepository> mockProductRepository,
        Mock<IValidator<Product>> mockValidator)
        {
            var product = Builder<Product>
                .CreateNew()
                .Build();

            mockProductRepository.Setup(x => x.GetByIdAsync(product.Id)).ReturnsAsync(product);
            mockValidator.SetValidationSuccess();
            return product;
        }

        public override ProductLogic Create()
        {
            var productLogic = base.Create();
            CorrectFlow(mockProductRepository, mockValidator);

            return productLogic;
        }
        [Fact]
        public async Task ShouldReturnResultOk()
        {
            var id = Guid.NewGuid();
            var product = Builder<Product>
                .CreateNew()
                .With(x => x.Id = id)
                .Build();

            var mockProductRepository = new Mock<IProductRepository>();
            mockProductRepository.Setup(x => x.GetByIdAsync(id)).ReturnsAsync(product);
            var mockValidator = new Mock<IValidator<Product>>();

            var productLogic = new ProductLogic(mockProductRepository.Object, mockValidator.Object);

            var result = await productLogic.GetByIdAsync(id);

            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Value.Should().BeSameAs(product);
        }
        [Fact]
        public async Task ShouldReturnResultFailure()
        {
            var id = Guid.NewGuid();

            var mockProductRepository = new Mock<IProductRepository>();
            mockProductRepository.Setup(x => x.GetByIdAsync(id)).ReturnsAsync((Product)null);
            var mockValidator = new Mock<IValidator<Product>>();

            var productLogic = new ProductLogic(mockProductRepository.Object, mockValidator.Object);

            var result = await productLogic.GetByIdAsync(id);

            result.Should().NotBeNull();
            result.Success.Should().BeFalse();
            result.Value.Should().BeSameAs(null);
        }
    }
}
