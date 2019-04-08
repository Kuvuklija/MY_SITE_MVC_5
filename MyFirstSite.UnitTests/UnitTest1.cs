using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MyFirstSite.Domain.Abstract;
using MyFirstSite.Domain.Entities;
using MyFirstSite.WebUI.Controllers;
using MyFirstSite.WebUI.HtmlHelpers;
using MyFirstSite.WebUI.Models;
//using MyFirstSite.WebUI.HtmlHelpers;

namespace MyFirstSite.UnitTests
{
    [TestClass]
    public class UnitTest1{
    
        [TestMethod]
        public void Can_Paginate() {

            //organisation
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[] {
                new Product{ProductID=1, Name="P1"},
                new Product{ProductID=2, Name="P2"},
                new Product{ProductID=3, Name="P3"},
                new Product{ProductID=4, Name="P4"},
                new Product{ProductID=5, Name="P5"}
            });
            ProductController target = new ProductController(mock.Object);
            target.PageSize = 3;

            //action
            ProductListViewModel resultModel = (ProductListViewModel)target.List(null,2).Model;

            //assert
            Product[] selectedProducts = resultModel.Products.ToArray();
            Assert.IsTrue(selectedProducts.Length==2);
            Assert.AreEqual("P4", selectedProducts[0].Name);
            Assert.AreEqual("P5", selectedProducts[1].Name);
        }

        [TestMethod]
        public void Can_Generate_Page_Links() {
            
            //arrive
            HtmlHelper myHelper = null; //!
            PagingInfo pagingInfo = new PagingInfo {
                CurrentPage = 2,
                ItemsPerPage=10,
                TotalItems=28
            };
            Func<int, string> pageDelegate = i => "Page" + i;

            //action
            MvcHtmlString result = myHelper.PageLinks(pagingInfo, pageDelegate); //! myHelper=null, but... because it's extending method for this

            //assert
            Assert.AreEqual(@"<a class=""btn btn-default"" href=""Page1"">1</a>"+
                            @"<a class=""selected btn-primary btn btn-default"" href=""Page2"">2</a>"+
                            @"<a class=""btn btn-default"" href=""Page3"">3</a>", result.ToHtmlString());
        }

        [TestMethod]
        public void Can_Send_Pagination_View_Model() {

            //organise
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(p => p.Products).Returns(new List<Product> {
                new Product{ProductID=1, Name="P1"},
                new Product{ProductID=2, Name="P2"},
                new Product{ProductID=3, Name="P3"},
                new Product{ProductID=4, Name="P4"},
                new Product{ProductID=5, Name="P5"}
            });
            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 3;

            //action
            ProductListViewModel result = (ProductListViewModel)controller.List(null,2).Model;

            //assert
            PagingInfo info = result.PagingInfo;
            Assert.AreEqual(info.CurrentPage, 2);
            Assert.AreEqual(info.TotalPages, 2);
            Assert.AreEqual(info.TotalItems, 5);

        }

        [TestMethod]
        public void Can_Filter_Products() {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(p => p.Products).Returns(new List<Product> {
                new Product{ProductID=1, Name="Reference", Category="ASP"},
                new Product{ProductID=2, Name="Enhancement", Category="Action"},
                new Product{ProductID=3, Name="Placeholder", Category="ASP"},
                new Product{ProductID=4, Name="Preference", Category="Action"},
                new Product{ProductID=5, Name="Impression", Category="Action"}
            });

            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 3;

            ProductListViewModel result = (ProductListViewModel)controller.List("ASP", 1).Model;
            Product[] array = result.Products.ToArray();
            Assert.AreEqual(array.Length, 2);
            Assert.IsTrue(array[0].Name == "Reference");
            Assert.IsTrue(array[1].Name == "Placeholder");
        }

        [TestMethod]
        public void Can_Create_Categories() {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[] {
                new Product{ProductID=1, Name="P1", Category="Apple"},
                new Product{ProductID=2, Name="P2", Category="Apple"},
                new Product{ProductID=3, Name="P3", Category="Samsung"},
                new Product{ProductID=4, Name="P4", Category="Microsoft"}
            });
            NavController target = new NavController(mock.Object);

            //action
            SelectedCategoryModel result=(SelectedCategoryModel)target.Menu().Model;
            string[] categories = result.categories.ToArray();

            //assert
            Assert.IsTrue(categories.Length==3);
            Assert.AreEqual(categories[0],"Apple");
            Assert.AreEqual(categories[1], "Microsoft");
            Assert.AreEqual(categories[2], "Samsung");
        }

        [TestMethod]
        public void Cannot_Checkout_Empty_Cart() {
            //arrange
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();
            Cart cart = new Cart();
            ShippingDetail shippingDetail = new ShippingDetail() { GiftWrap=true};
            CartController target = new CartController(null, mock.Object);

            //action
            ViewResult result = target.Checkout(shippingDetail, cart);

            //assert
            mock.Verify(m => m.ProcessorOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetail>()), Times.Never); //never invoke method this empty cart
            Assert.AreEqual("", result.ViewName); //return standart view
            Assert.AreEqual(false, result.ViewData.ModelState.IsValid);//because shipping is empty
        }

        [TestMethod]
        public void Cannot_Checkout_Empty_ShippingDetails() {
            //arrange
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();
            Cart cart = new Cart();
            cart.AddItem(new Product(), 1);
            ShippingDetail shippingDetail = new ShippingDetail() { GiftWrap = true }; //add method for 'false'
            CartController target = new CartController(null, mock.Object);
            //action
            target.ModelState.AddModelError("error", "error");
            ViewResult result = target.Checkout(shippingDetail, cart);
            //assert
            mock.Verify(m => m.ProcessorOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetail>()),Times.Never);
            Assert.AreEqual("", result.ViewName);
            Assert.AreEqual(false, result.ViewData.ModelState.IsValid);
        }

        [TestMethod]
        public void Can_Checkout_And_Submit_Order() {
            //arrange
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();
            Cart cart = new Cart();
            cart.AddItem(new Product(), 1);
            ShippingDetail shippingDetail=new ShippingDetail() {
                Name="Vadim",LastName="Ilray",Phone="89672844618",Email="c@mail.ru",GiftWrap=true,Address="Mockow"
            };
            CartController target = new CartController(null, mock.Object);
            //action
            ViewResult result = target.Checkout(shippingDetail, cart);
            //assert
            Assert.AreEqual("Completed", result.ViewName);
            Assert.AreEqual(true, result.ViewData.ModelState.IsValid);
        }
    }
}
