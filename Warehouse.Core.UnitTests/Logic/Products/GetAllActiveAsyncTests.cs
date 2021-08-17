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

namespace Warehouse.Core.UnitTests.Logic.Products
{
    public class GetAllActiveAsyncTests
    {
        [Fact]
        public async Task ShouldReturnListOfProducts()
        {
            var listActive = Builder<Product>
                .CreateListOfSize(3)
                .All()
                .With(x => x.Id = Guid.NewGuid())
                .With(x => x.IsActive = true)
                .Build();

            var mockProductRepository = new Mock<IProductRepository>();
            mockProductRepository.Setup(x => x.GetAllActiveAsync()).ReturnsAsync(listActive);
            var mockValidator = new Mock<IValidator<Product>>();

            var productLogic = new ProductLogic(mockProductRepository.Object, mockValidator.Object);

            var result = await productLogic.GetAllActiveAsync();

            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Value.Should().BeSameAs(listActive);

        }
        [Fact]
        public async Task ShouldReturnEmptyList()
        {
            var listActive = new List<Product>();

            var mockProductRepository = new Mock<IProductRepository>();
            mockProductRepository.Setup(x => x.GetAllActiveAsync()).ReturnsAsync(listActive);
            var mockValidator = new Mock<IValidator<Product>>();

            var productLogic = new ProductLogic(mockProductRepository.Object, mockValidator.Object);

            var result = await productLogic.GetAllActiveAsync();

            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Value.Should().BeSameAs(listActive);
        }
    }
}
