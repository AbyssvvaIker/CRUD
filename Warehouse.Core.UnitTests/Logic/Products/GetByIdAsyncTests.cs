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
using Warehouse.Core.UnitTests.Logic.Products.Infrastructure;

namespace Warehouse.Core.UnitTests.Logic.Products
{
    public class GetByIdAsyncTests : BaseTest
    {
        public Product Product;
        public void CorrectFlow()
        {
            Product = Builder<Product>
                .CreateNew()
                .Build();

            mockProductRepository.Setup(x => x.GetByIdAsync(Product.Id)).ReturnsAsync(Product);
            mockValidator.SetValidationSuccess();
        }

        public override ProductLogic Create()
        {
            var productLogic = base.Create();
            CorrectFlow();

            return productLogic;
        }
        [Fact]
        public async Task Should_Return_ResultOk()
        {
            //var id = Guid.NewGuid();
            //var product = Builder<Product>
            //    .CreateNew()
            //    .With(x => x.Id = id)
            //    .Build();

            //var mockProductRepository = new Mock<IProductRepository>();
            //mockProductRepository.Setup(x => x.GetByIdAsync(id)).ReturnsAsync(product);
            //var mockValidator = new Mock<IValidator<Product>>();

            //var productLogic = new ProductLogic(mockProductRepository.Object, mockValidator.Object);
            var productLogic = Create();
            mockProductRepository.Setup(x => x.GetByIdAsync(Product.Id)).ReturnsAsync(Product);

            var result = await productLogic.GetByIdAsync(Product.Id);

            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Value.Should().BeSameAs(Product);

            mockProductRepository.Verify(
                x => x.GetByIdAsync(It.IsAny<Guid>()),
                Times.Once);
        }
        [Fact]
        public async Task Should_Return_ResultFailure_When_ProductDoesNotExist()
        {
            //var id = Guid.NewGuid();

            //var mockProductRepository = new Mock<IProductRepository>();
            //mockProductRepository.Setup(x => x.GetByIdAsync(id)).ReturnsAsync((Product)null);
            //var mockValidator = new Mock<IValidator<Product>>();

            //var productLogic = new ProductLogic(mockProductRepository.Object, mockValidator.Object);
            var productLogic = Create();
            mockProductRepository.Setup(x => x.GetByIdAsync(Product.Id)).ReturnsAsync((Product)null);

            var result = await productLogic.GetByIdAsync(Product.Id);

            result.Should().NotBeNull();
            result.Success.Should().BeFalse();
            result.Value.Should().BeSameAs(null);

            mockProductRepository.Verify(
                x => x.GetByIdAsync(It.IsAny<Guid>()),
                Times.Once);
        }
    }
}
