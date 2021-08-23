using FizzWare.NBuilder;
using FluentAssertions.AspNetCore.Mvc;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Warehouse.Core.Entities;
using Warehouse.Core.UnitTests.Controllers.Products.Infrastructure;
using Warehouse.Web.Controllers;
using Warehouse.Web.ViewModels.Product;
using Xunit;

namespace Warehouse.Core.UnitTests.Controllers.Products
{
    public class CreateGetTests : BaseTest
    {
        protected ProductViewModel ViewModel { get; set; }

        protected override ProductsController Create()
        {
            var controller = base.Create();;

            return controller;
        }

        [Fact]
        public async Task Should_Return_View_With_Empty_ViewModel()
        {
            //arrange
            var controller = Create();
            //act
            var result =await controller.Create();
            //assert
            result.Should()
                .BeViewResult()
                .WithDefaultViewName()
                .Model
                .Should()
                .BeOfType(typeof(ProductViewModel));

            //no mock is used here
        }
    }
}
