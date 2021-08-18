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
        public void CorrectFlow(Mock<IProductRepository> mockProductRepository,
        Mock<IValidator<Product>> mockValidator)
        {
            product = Builder<Product>
                .CreateNew()
                .Build();

            mockValidator.SetValidationSuccess();
        }

        public override ProductLogic Create()
        {
            var productLogic = base.Create();
            CorrectFlow(mockProductRepository, mockValidator);

            return productLogic;
        }
        [Fact]
        public async Task ShouldThrowArgumentNullException()
        {
            var mockProductRepository = new Mock<IProductRepository>();
            var mockValidator = new Mock<IValidator<Product>>();

            var productLogic = new ProductLogic(mockProductRepository.Object, mockValidator.Object);

            Func<Task> act = async () => await productLogic.DeleteAsync(null);

            await act.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task ShouldReturnResultOk()
        {
            var product = Builder<Product>
                 .CreateNew()
                 .Build();

            var mockProductRepository = new Mock<IProductRepository>();
            var mockValidator = new Mock<IValidator<Product>>();

            var productLogic = new ProductLogic(mockProductRepository.Object, mockValidator.Object);

            var result = await productLogic.DeleteAsync(product);

            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
        }
    }
}
