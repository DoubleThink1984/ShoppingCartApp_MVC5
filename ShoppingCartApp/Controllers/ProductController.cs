using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ShoppingCartApp.Models;
using ShoppingCartApp.Presentation;
using PagedList;

namespace ShoppingCartApp.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product Search
        public ActionResult Index(string searchWord, int? page)
        {
            var products = ModelLists.Products.Where(x => x.InactiveProduct == false).OrderBy(x => x.ProductName).ToPagedList(page ?? 1, 12);

            //Category category = new Category();

            if (searchWord == "" || searchWord == null)
            {
                ViewBag.searchWord = "";
                return View(products);
            }
            ViewBag.searchWord = searchWord;
            var productsSearched = ModelLists.Products.Where(x => x.ProductName.ToLower().Contains(searchWord.ToLower()) && x.InactiveProduct == false).OrderBy(x => x.ProductName).ToPagedList(page ?? 1, 12);

            return View(productsSearched);
        }

        public ActionResult Details(int? id)
        {
            Cart productCart = new Cart();
            int? remainingQuantity = null;
            ShoppingCart shoppingCart = ShoppingCart.GetCart(this);
            List<Cart> carts = shoppingCart.GetCartProducts();
            Product product = new Product();
            bool inCart = false;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            try
            {
                product = ModelLists.Products.Where(prod => prod.ProductID == id).Single();
            }
            catch (Exception)
            {
                return HttpNotFound();
            }
            if (carts.Count > 0)
            {
                productCart = carts.Where(x => x.ProductID == id).SingleOrDefault();
                if (productCart != null)
                {
                    inCart = true;
                    if (product.LimitOne)
                    {
                        remainingQuantity = 0;
                    }
                    else
                    {
                        remainingQuantity = productCart.Product.Quantity - productCart.Quantity;
                    }
                }
                else
                {
                    remainingQuantity = product.Quantity;
                }
            }
            else
            {
                remainingQuantity = product.Quantity;
            }

            if (product.InactiveProduct)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (product == null)
            {
                return HttpNotFound();
            }

            ProductDetailsView bp = new ProductDetailsView
            {
                Product = product,
                Quantity = remainingQuantity,
                InCart = inCart
            };
            
            return View(bp);
        }
    }
}