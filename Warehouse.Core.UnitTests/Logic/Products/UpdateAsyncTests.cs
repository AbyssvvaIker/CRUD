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
    public class UpdateAsyncTests : BaseTest
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
            Func<Task> act = async () => await productLogic.UpdateAsync(null);

            await act.Should().ThrowAsync<ArgumentNullException>();

            mockValidator.Verify(
                x => x.Validate(It.IsAny<Product>()),
                Times.Never);
            mockProductRepository.Verify(
                x => x.SaveChangesAsync(),
                Times.Never);

        }

        [Fact]
        public async Task Should_Return_ResultFailure_When_ValidationFailed()
        {
            //var product = Builder<Product>
            //    .CreateNew()
            //    .Build();

            //var mockProductRepository = new Mock<IProductRepository>();
            //var mockValidator = new Mock<IValidator<Product>>();
            //string validatedProperty = "test";
            //string errorMessage = "test error message";
            //mockValidator.SetValidationFailure(validatedProperty, errorMessage);

            //var productLogic = new ProductLogic(mockProductRepository.Object, mockValidator.Object);
            var productLogic = Create();
            string validatedProperty = "test";
            string errorMessage = "test error message";
            mockValidator.SetValidationFailure(validatedProperty, errorMessage);


            var result = await productLogic.UpdateAsync(Product);

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
                x => x.SaveChangesAsync(),
                Times.Never);
        }

        [Fact]
        public async Task ShouldReturnResultOk()
        {
            //var product = Builder<Product>
            //    .CreateNew()
            //    .Build();

            //var mockProductRepository = new Mock<IProductRepository>();
            //var mockValidator = new Mock<IValidator<Product>>();
            //var productLogic = new ProductLogic(mockProductRepository.Object, mockValidator.Object);
            //mockValidator.SetValidationSuccess();
            var productLogic = Create();
            var result = await productLogic.UpdateAsync(Product);
            
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Value.Should().BeSameAs(Product);

            mockValidator.Verify(
                x => x.Validate(It.IsAny<Product>()),
                Times.Once);
            mockProductRepository.Verify(
                x => x.SaveChangesAsync(),
                Times.Once);
        }
    }
}
