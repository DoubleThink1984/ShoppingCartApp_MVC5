using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShoppingCartApp.Models;
using ShoppingCartApp.Presentation;

namespace ShoppingCartApp.Controllers
{
    public class HomeController : Controller
    {        
        public ActionResult Index()
        {
            List<Product> featuredProducts = ModelLists.Products.Where(x => x.FeaturedProduct == true && x.InactiveProduct == false && x.ProductImages.Count > 0).ToList();
            /*If there are no featured products, populate list with 4 products*/
            if (featuredProducts.Count == 0)
            {
                List<Product> productsWithImages = ModelLists.Products.Where(x=> x.InactiveProduct == false && x.ProductImages.Count > 0).ToList();
                List<Product> products = new List<Product>();
                if (productsWithImages != null && productsWithImages.Count >0)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        products.Add(productsWithImages[i]);
                    }
                }
                return View(products);
            } 

            return View(featuredProducts);
        }

        public ActionResult About()
        {
            ViewBag.Message = "We sell discounted toys and collectables!!!";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        //Called by Shared _Layout view to display categories menu
        [ChildActionOnly]
        public  PartialViewResult Menu()
        {
            return PartialView("_Menu");
        }

        public ActionResult Error()
        {
            return View();
        }

    }
}