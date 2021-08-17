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
    public class UpdateAsyncTests
    {
        [Fact]
        public async Task ShouldThrowArgumentNullException()
        {
            var mockProductRepository = new Mock<IProductRepository>();
            var mockValidator = new Mock<IValidator<Product>>();

            var productLogic = new ProductLogic(mockProductRepository.Object, mockValidator.Object);

            Func<Task> act = async () => await productLogic.UpdateAsync(null);

            await act.Should().ThrowAsync<ArgumentNullException>();
            
        }

        [Fact]
        public async Task ShouldReturnResultFailure()
        {
            var product = Builder<Product>
                .CreateNew()
                .Build();

            var mockProductRepository = new Mock<IProductRepository>();
            var mockValidator = new Mock<IValidator<Product>>();
            mockValidator.SetValidationFailure("test", "test error message");

            var productLogic = new ProductLogic(mockProductRepository.Object, mockValidator.Object);

            var result = await productLogic.UpdateAsync(product);

            result.Should().NotBeNull();
            result.Success.Should().BeFalse();
            result.Errors.Should().HaveCount(1);
        }

        [Fact]
        public async Task ShouldReturnResultOk()
        {
            var product = Builder<Product>
                .CreateNew()
                .With(x => x.Id = Guid.NewGuid())
                .Build();

            var mockProductRepository = new Mock<IProductRepository>();
            var mockValidator = new Mock<IValidator<Product>>();
            var productLogic = new ProductLogic(mockProductRepository.Object, mockValidator.Object);
            mockValidator.SetValidationSuccess();
         
            var result = await productLogic.UpdateAsync(product);
            
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Value.Should().BeSameAs(product);
        }
    }
}
