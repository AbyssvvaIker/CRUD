using FizzWare.NBuilder;
using FluentAssertions.AspNetCore.Mvc;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Warehouse.Core.Entities;
using Warehouse.Web.UnitTests.Categories.Infrastructure;
using Warehouse.Web;
using Warehouse.Web.ViewModels.Category;
using Xunit;
using Warehouse.Web.Controllers;

namespace Warehouse.Web.UnitTests.Categories
{
    public class CreateGetTests : BaseTest
    {
        protected CategoryViewModel ViewModel { get; set; }

        protected override CategoryController Create()
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
            var result = controller.Create();
            //assert
            result.Should()
                .BeViewResult()
                .WithDefaultViewName()
                .Model
                .Should()
                .BeOfType(typeof(CategoryViewModel));

            //no mock is used here
        }
    }
}
