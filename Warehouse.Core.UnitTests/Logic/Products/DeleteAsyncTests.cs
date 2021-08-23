using FizzWare.NBuilder;
using FluentAssertions;
using Moq;
using System;
using System.Threading.Tasks;
using Warehouse.Core.Entities;
using Warehouse.Core.Logic;
using Warehouse.Core.UnitTests.CustomAssertions;
using Warehouse.Core.UnitTests.Extensions;
using Warehouse.Core.UnitTests.Logic.Products.Infrastructure;
using Xunit;

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

            MockValidator.SetValidationSuccess();
        }

        protected override ProductLogic Create()
        {
            var productLogic = base.Create();
            CorrectFlow();

            return productLogic;
        }
        [Fact]
        public async Task Should_Throw_ArgumentNullException_When_GivenProduct_Null()
        {
            //arrange
            var productLogic = Create();
            //act
            Func<Task> act = async () => await productLogic.DeleteAsync(null);
            //assert
            await act.Should().ThrowAsync<ArgumentNullException>();

            MockProductRepository.Verify(
                x => x.Delete(It.IsAny<Product>()),
                Times.Never);
            MockProductRepository.Verify(
               x => x.SaveChangesAsync(),
               Times.Never);
        }

        [Fact]
        public async Task Should_Return_ResultOk()
        {
            //arrange
            var productLogic = Create();
            //act
            var result = await productLogic.DeleteAsync(Product);
            //assert
            result.Should().BeSuccess();

            MockProductRepository.Verify(
                x => x.Delete(Product),
                Times.Once);
            MockProductRepository.Verify(
               x => x.SaveChangesAsync(),
               Times.Once);
        }
    }
}
