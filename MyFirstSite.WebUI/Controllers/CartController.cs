using MyFirstSite.Domain.Abstract;
using MyFirstSite.Domain.Entities;
using MyFirstSite.WebUI.Models;
using System.Linq;
using System.Web.Mvc;

namespace MyFirstSite.WebUI.Controllers{
    public class CartController : Controller{

        private IProductRepository repository;
        private IOrderProcessor orderProceccor;

        public CartController(IProductRepository repo, IOrderProcessor proc) {
            repository = repo;
            orderProceccor = proc;
        }

        public RedirectToRouteResult AddToCart(int productID, string returnUrl, Cart cart) {
            Product product = repository.Products
                .FirstOrDefault(p => p.ProductID == productID);
            if (product != null) {
                //GetCart().AddItem(product, 1);
                cart.AddItem(product, 1);
            }
            return RedirectToAction("Index", new { returnUrl }); //not view - another controller
        }

        public RedirectToRouteResult RemoveFromCart(int productID, string returnUrl, Cart cart) {
            Product product = repository.Products
                .FirstOrDefault(p => p.ProductID == productID);
            if (product != null) {
                //GetCart().RemoveItem(product);
                cart.RemoveItem(product);
            }
            return RedirectToAction("Index", new { returnUrl,cart });
        }

        //common method
        public ViewResult Index(string returnUrl, Cart cart) {
            return View(new CartViewModel {
                //Cart = GetCart(),
                Cart = cart,
                ReturnUrl = returnUrl
            });
        }

        //for vidget
        public PartialViewResult Summary(Cart cart) {
            return PartialView(cart);
        }

        public ViewResult Checkout() {
            return View(new ShippingDetail());
        }

        [HttpPost]
        public ViewResult Checkout(ShippingDetail shippingDetail, Cart cart) {
            if (cart.Lines.Count() == 0) {
                ModelState.AddModelError("", "Sorry your cart is empty!");
            }

            if (!shippingDetail.GiftWrap) {
                ModelState.AddModelError("GiftWrap", "Please set a flag");
            }

            if (ModelState.IsValid) {
                orderProceccor.ProcessorOrder(cart, shippingDetail); //send e-mail message
                cart.Clear();
                return View("Completed");
            }else{
                return View(shippingDetail);
            }
        }

        //private Cart GetCart() {
        //    Cart cart = (Cart)Session["Cart"];
        //    if (cart == null) {
        //        cart = new Cart();
        //        Session["cart"] = cart;
        //    }
        //    return cart;
        //}
    }
}