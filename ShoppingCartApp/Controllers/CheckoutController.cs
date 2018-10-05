using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using ShoppingCartApp.Models;
using ShoppingCartApp.Presentation;
using System.Threading.Tasks;
using System.Text;
using System.Net;

namespace ShoppingCartApp.Controllers
{
    [Authorize]
    public class CheckoutController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        // GET: Checkout
        public ActionResult Index()
        {
            Order order = new Order();

            return View(order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index([Bind(Include = "Name,Addr1,Country,City,State,PostalCode,email,phone,Comments,ShipName,ShipAddr1,ShipCountry,ShipCity,ShipState,ShipPostalCode,CardType,CardNumber,CVC,ExpirationDate,CardHolderName")] Order order)
        {
            bool flag = false;
            if (order.CardHolderName == null)
            {
                ViewBag.CardHolderName = "The Cardholder Name field is required";
                flag = true;
            }
            if (order.CardNumber == null)
            {
                ViewBag.CardNumber = "The Card Number field is required";
                flag = true;
            }
            if (order.CardType == null)
            {
                ViewBag.CardType = "Thie Card Type field is required";
                flag = true;
            }
            if (order.CVC == null)
            {
                ViewBag.CVC = "The CVC field is required";
                flag = true;
            }
            if (order.ExpirationDate == null)
            {
                ViewBag.ExpirationDate = "The Expiration Date field is required";
                flag = true;
            }
            if (flag)
            {
                return View(order);
            }

            ShoppingCart cart = ShoppingCart.GetCart(this);
            var shoppingCarts = db.ShoppingCarts.Include("Product").Where(x => x.CartID == User.Identity.Name).ToList();
            string[] creditDetails = { order.CardHolderName, order.CardType , order.CVC, order.ExpirationDate, Encryption.Encrypt(order.CardNumber) };
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < creditDetails.Length; i++)
            {
                if (i == creditDetails.Length - 1)
                {
                    sb.Append(creditDetails[i]);
                }
                else
                {
                    sb.Append(creditDetails[i]).Append("_");
                }
            }

            order.OrderDate = DateTime.Now;
            order.UserName = User.Identity.Name;
            order.Total = cart.GetCartDecimalTotal();
            order.Details = sb.ToString();

            if (ModelState.IsValid)
            {
                db.Orders.Add(order);

                foreach (var shoppingCart in shoppingCarts)
                {
                    decimal price;
                    if (shoppingCart.Product.WildmanPrice != null)
                    {
                        price = shoppingCart.Product.WildmanPrice ?? 0;
                    }
                    else
                    {
                        price = shoppingCart.Product.Price;
                    }
                    OrderItem orderItem = new OrderItem
                    {
                        Cost = price,
                        ProductID = shoppingCart.ProductID,
                        Quantity = shoppingCart.Quantity,
                        OrderDetailID = order.OrderID
                    };
                    db.OrderItems.Add(orderItem);
                    db.ShoppingCarts.Remove(shoppingCart);
                    Product product = db.Products.Find(shoppingCart.ProductID);
                    product.Quantity -= shoppingCart.Quantity;
                    product.Quantity = (product.Quantity < 0) ? 0 : product.Quantity;
                    db.Entry(product).State = EntityState.Modified;
                }
                await db.SaveChangesAsync();
                ModelLists.NewModelList();
            }
            
            return RedirectToAction("OrderReview", new { id = order.OrderID});
        }
        
        public ActionResult OrderReview(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Where(x => x.OrderID == id).Single();
            if (User.Identity.Name != order.UserName)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }      
            order.DecryptAndPopulateCreditCardFields(order.Details);
            order.CardNumber = "************" + order.CardNumber.Substring(Math.Max(0, order.CardNumber.Length - 4));
            OrderDetailView orderDetail = new OrderDetailView()
            {
                Order = order,
                OrderItems = order.OrderItems
            };
            
            return View(orderDetail);
        }        
    }
}