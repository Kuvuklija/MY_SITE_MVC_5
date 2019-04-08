using MyFirstSite.Domain.Entities;
using System.Web.Mvc;

namespace MyFirstSite.Domain.Infrastructure.Binders{

    public class CartModelBinder:IModelBinder{

        private const string sessionKey = "Cart";

        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext) {
            
            //get object Cart from session
            Cart cart = null;
            if (controllerContext.HttpContext.Session != null) {
                cart = (Cart)controllerContext.HttpContext.Session[sessionKey];
            }

            //create instance Cart if it did't find
            if (cart == null) {
                cart = new Cart();
                if (controllerContext.HttpContext.Session != null) {
                    controllerContext.HttpContext.Session[sessionKey] = cart;
                }
            }
            return cart;
        }
    }
}
