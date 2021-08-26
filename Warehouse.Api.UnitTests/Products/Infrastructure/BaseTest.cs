using AutoMapper;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Warehouse.Api.Controllers;
using Warehouse.Core.Interfaces;

namespace Warehouse.Api.UnitTests.Products.Infrastructure
{
    public class BaseTest
    {
        protected Mock<IProductLogic> MockProductLogic { get; set; }
        protected Mock<IMapper> MockMapper { get; set; }

        protected virtual ProductsController Create()
        {
            MockProductLogic = new Mock<IProductLogic>();
            MockMapper = new Mock<IMapper>();

            return new ProductsController(MockProductLogic.Object, MockMapper.Object);
        }
    }
}
