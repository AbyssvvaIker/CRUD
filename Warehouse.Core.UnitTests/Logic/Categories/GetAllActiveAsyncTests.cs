using FizzWare.NBuilder;
using FluentAssertions;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Warehouse.Core.Entities;
using Warehouse.Core.Logic;
using Warehouse.Core.UnitTests.Extensions;
using Warehouse.Core.UnitTests.Logic.Categories.Infrastructure;
using Xunit;
using Warehouse.Core.UnitTests.CustomAssertions;

namespace Warehouse.Core.UnitTests.Logic.Categories
{
    public class GetAllActiveAsyncTests : BaseTest
    {
        public IEnumerable<Category> Categories;
        public void CorrectFlow()
        {
            Categories = Builder<Category>
                .CreateListOfSize(3)
                .Build();

            MockCategoryRepository.Setup(x => x.GetAllActiveAsync()).ReturnsAsync(Categories);
            MockValidator.SetValidationSuccess();
        }

        protected override CategoryLogic Create()
        {
            var categoryLogic = base.Create();
            CorrectFlow();

            return categoryLogic;
        }
        [Fact]
        public async Task Should_Return_ResultOk_With_ListOfCategories()
        {
            //arrange
            var categoryLogic = Create();
            //act
            var result = await categoryLogic.GetAllActiveAsync();
            //assert
            //result.Should().NotBeNull();
            //result.Success.Should().BeTrue();
            //result.Value.Should().BeSameAs(Categories);
            result.Should().BeSuccess(Categories);
            MockCategoryRepository.Verify(
                x => x.GetAllActiveAsync(),
                Times.Once);
        }

    }
}
