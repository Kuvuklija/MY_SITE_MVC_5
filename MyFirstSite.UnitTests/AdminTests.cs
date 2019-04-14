using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MyFirstSite.Domain.Abstract;
using MyFirstSite.Domain.Entities;
using MyFirstSite.WebUI.Controllers;

namespace MyFirstSite.UnitTests
{
    [TestClass]
    public class AdminTests
    {
        [TestMethod]
        public void Index_Contains_All_Products(){
            //arrange
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[] {
                new Product{ProductID=1, Name="P1"},
                new Product{ProductID=2, Name="P2"},
                new Product{ProductID=3, Name="P3"}
            });
            AdminController target = new AdminController(mock.Object);
            //action
            Product[] result = ((IEnumerable<Product>)target.Index().ViewData.Model).ToArray();
            //assert
            Assert.AreEqual(result.Count(), 3);
            Assert.AreEqual("P1", result[0].Name);
            Assert.AreEqual("P2", result[1].Name);
            Assert.AreEqual("P3", result[2].Name);
        }
    }
}
