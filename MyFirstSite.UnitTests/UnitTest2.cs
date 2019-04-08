using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MyFirstSite.Domain.Abstract;
using MyFirstSite.Domain.Entities;
using MyFirstSite.WebUI.Controllers;
using MyFirstSite.WebUI.Models;

namespace MyFirstSite.UnitTests
{
    [TestClass]
    public class CartTest
    {
        [TestMethod]
        public void Can_Add_New_Line(){
            //arrange
            Product p1 = new Product { ProductID = 1, Name = "P1" };
            Product p2 = new Product { ProductID = 2, Name = "P2" };
            Cart cart = new Cart();

            //action
            cart.AddItem(p1, 3);
            cart.AddItem(p2, 5);

            //assert
            CartLine[] results = cart.Lines.ToArray();
            Assert.IsTrue(results.Count() == 2);
            Assert.AreEqual(p1, results[0].Product);
            Assert.AreEqual(p2, results[1].Product);
        }

        [TestMethod]
        public void Can_Add_Quantity_For_Existing_Lines() {
            //arrange
            Product p1 = new Product { ProductID = 1, Name = "P1" };
            Product p2 = new Product { ProductID = 2, Name = "P2" };
            Cart cart = new Cart();

            //action
            cart.AddItem(p1, 5);
            cart.AddItem(p2, 2);
            cart.AddItem(p1, 5);

            //assert
            CartLine[] results = cart.Lines.ToArray();
            Assert.IsTrue(results.Count() == 2);
            Assert.AreEqual(10, results[0].Quantity);
            Assert.AreEqual(2, results[1].Quantity);
        }

        [TestMethod]
        public void Can_Remove_Line() {
            //arrange
            Product p1 = new Product { ProductID = 1, Name = "P1" };
            Product p2 = new Product { ProductID = 2, Name = "P2" };
            Cart cart = new Cart();

            //action
            cart.AddItem(p1, 10);
            cart.AddItem(p2, 20);
            cart.AddItem(p2, 5);
            cart.RemoveItem(p2);

            //assert
            CartLine[] result = cart.Lines.ToArray();
            Assert.AreEqual(0, result.Where(p => p.Product == p2).Count());
            Assert.AreEqual(1, result.Count());
        }

        [TestMethod]
        public void Calculate_Cart_Total() {
            //arrange
            Product p1 = new Product { ProductID = 1, Name = "P1",Price=100M };
            Product p2 = new Product { ProductID = 2, Name = "P2",Price=200M };
            Cart cart = new Cart();

            //action
            cart.AddItem(p1, 1);
            cart.AddItem(p2, 2);

            //assert
            Assert.AreEqual(cart.ComputeTotalValue(), 500M);

        }

        [TestMethod]
        public void Can_Clear_Content() {
            //arrange
            Product p1 = new Product { ProductID = 1, Name = "P1" };
            Product p2 = new Product { ProductID = 2, Name = "P2" };
            Cart cart = new Cart();
            //action
            cart.AddItem(p1, 10);
            cart.AddItem(p2, 20);
            cart.Clear();
            //assert
            CartLine[] result = cart.Lines.ToArray();
            Assert.AreEqual(result.Count(), 0);
        }

        ////////////////////////////////////////////////////////////////
        [TestMethod]
        public void Can_Add_To_Cart() {
            //arrange
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(p => p.Products).Returns(new Product[] {
                new Product{ ProductID=1, Name="P1", Category="ASP CORE"},
            }.AsQueryable());
            Cart cart = new Cart();

            CartController target = new CartController(mock.Object, null);

            //action
            target.AddToCart(1, null, cart);

            //assert
            Assert.AreEqual(cart.Lines.Count(), 1);
            Assert.AreEqual(cart.Lines.ToArray()[0].Product.ProductID, 1);

        }

        [TestMethod]
        public void Adding_Product_To_Cart_Redirect_To_Index() {
            //arrange
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(p => p.Products).Returns(new Product[] {
                new Product{ProductID=1, Name="P1", Category="ASP"}
            });
            Cart cart = new Cart();
            CartController target = new CartController(mock.Object, null);

            //action
            RedirectToRouteResult result = target.AddToCart(1, "myUrl", cart);

            //assert
            Assert.AreEqual(result.RouteValues["action"], "Index");
            Assert.AreEqual(result.RouteValues["returnUrl"], "myUrl");
        }

        [TestMethod]
        public void Can_View_Cart_Contents() {
            //arrange
            Cart cart = new Cart();
            CartController target = new CartController(null, null);
            //action
            CartViewModel result = (CartViewModel)target.Index("myUrl", cart).ViewData.Model;
            //assert
            Assert.AreEqual(result.Cart, cart);
            Assert.AreEqual(result.ReturnUrl, "myUrl");
        }
    }
}
