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
    public class DeleteAsyncTests : BaseTest
    {
        public Product Product;
        public void CorrectFlow()
        {
            Product = Builder<Product>
                .CreateNew()
                .Build();

            mockValidator.SetValidationSuccess();
        }

        public override ProductLogic Create()
        {
            var productLogic = base.Create();
            CorrectFlow();

            return productLogic;
        }
        [Fact]
        public async Task Should_Throw_ArgumentNullException_When_GivenProduct_Null()
        {
            //var mockProductRepository = new Mock<IProductRepository>();
            //var mockValidator = new Mock<IValidator<Product>>();

            //var productLogic = new ProductLogic(mockProductRepository.Object, mockValidator.Object);
            var productLogic = Create();

            Func<Task> act = async () => await productLogic.DeleteAsync(null);

            await act.Should().ThrowAsync<ArgumentNullException>();

            mockProductRepository.Verify(
                x => x.Delete(It.IsAny<Product>()),
                Times.Never);
            mockProductRepository.Verify(
               x => x.SaveChangesAsync(),
               Times.Never);
        }

        [Fact]
        public async Task Should_Return_ResultOk()
        {
            //var product = Builder<Product>
            //     .CreateNew()
            //     .Build();

            //var mockProductRepository = new Mock<IProductRepository>();
            //var mockValidator = new Mock<IValidator<Product>>();

            //var productLogic = new ProductLogic(mockProductRepository.Object, mockValidator.Object);
            var productLogic = Create();

            var result = await productLogic.DeleteAsync(Product);

            result.Should().NotBeNull();
            result.Success.Should().BeTrue();

            mockProductRepository.Verify(
                x => x.Delete(It.IsAny<Product>()),
                Times.Once);
            mockProductRepository.Verify(
               x => x.SaveChangesAsync(),
               Times.Once);

        }
    }
}
