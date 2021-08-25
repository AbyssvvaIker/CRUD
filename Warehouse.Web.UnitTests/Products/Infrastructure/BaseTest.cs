using AutoMapper;
using Moq;
using Warehouse.Core.Interfaces;
using Warehouse.Web.Controllers;

namespace Warehouse.Web.UnitTests.Products.Infrastructure
{
    public class BaseTest
    {
        protected Mock<IProductLogic> MockProductLogic { get; set; }
        protected Mock<ICategoryLogic> MockCategoryLogic { get; set; }
        protected Mock<IMapper> MockMapper { get; set; }

        protected virtual ProductsController Create()
        {
            MockCategoryLogic = new Mock<ICategoryLogic>();
            MockProductLogic = new Mock<IProductLogic>();
            MockMapper = new Mock<IMapper>();

            return new ProductsController(MockProductLogic.Object, MockCategoryLogic.Object, MockMapper.Object);
        }
    }
}
