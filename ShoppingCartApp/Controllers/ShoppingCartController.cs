using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShoppingCartApp.Models;
using ShoppingCartApp.Presentation;

namespace ShoppingCartApp.Controllers
{
    public class ShoppingCartController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: ShoppingCart
        public ActionResult Index()
        {
            ShoppingCart cart = ShoppingCart.GetCart(this);

            CartView viewModel = new CartView
            {
                CartEntries = cart.GetCartProducts(),
                CartTotal = cart.GetCartTotal()
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddToCart(int id, int quantity)
        {
            if (quantity == 0)
            {
                return RedirectToAction("Details", "Product", new { id = id});
            }
            // Retrieve the album from the database
            var addedProduct = db.Products.Single(x => x.ProductID == id);

            // Add it to the shopping cart
            var cart = ShoppingCart.GetCart(this);

            cart.AddProductToCart(addedProduct, quantity);

            // Go back to the main store page for more shopping
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult RemoveItem(int id)
        {
            var cart = ShoppingCart.GetCart(this);
            int productCount = cart.RemoveProductFromCart(id);
            var data = new Dictionary<string, string>
               {
                   { "DeleteID", id.ToString() },
                   { "CartTotal", cart.GetCartTotal() },
                   { "CartQuantity", cart.GetQuantity().ToString()}
               };

            return Json(data);
        }

        [HttpPost]
        public ActionResult ChangeItemCount(int id, int quantity)
        {
            var cart = ShoppingCart.GetCart(this);
            int productCount = cart.ChangeProductQuantity(id, quantity);
            RemoveCartView results = new RemoveCartView
            {
                CartTotal = cart.GetCartTotal(),
                CartQuantity = cart.GetQuantity(),
                ProductQuantity = productCount,
                DeleteID = id
            };

            return Json(results);
        }
        
        public ActionResult ClearCart()
        {
            var cart = ShoppingCart.GetCart(this);
            cart.EmptyCart();

            return RedirectToAction("Index");
        }

        public ActionResult Preview()
        {
            ShoppingCart cart = ShoppingCart.GetCart(this);

            CartPreview viewModel = new CartPreview
            {
                CartItemCount = cart.GetQuantity(),
                CartTotal = cart.GetCartTotal()
            };

            return PartialView("_Preview",viewModel);
        }
    }
}