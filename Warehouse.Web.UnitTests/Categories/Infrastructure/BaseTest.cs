using AutoMapper;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Warehouse.Core.Interfaces;
using Warehouse.Web;
using Warehouse.Web.Controllers;

namespace Warehouse.Web.UnitTests.Categories.Infrastructure
{
    public class BaseTest
    {
        protected Mock<ICategoryLogic> MockCategoryLogic { get; set; }
        protected Mock<IMapper> MockMapper { get; set; }

        protected virtual CategoryController Create()
        {
            MockCategoryLogic = new Mock<ICategoryLogic>();
            MockMapper = new Mock<IMapper>();

            return new CategoryController(MockCategoryLogic.Object, MockMapper.Object);
        }
    }
}
