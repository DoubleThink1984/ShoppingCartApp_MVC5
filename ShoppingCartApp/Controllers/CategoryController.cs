using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShoppingCartApp.Models;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Data.Entity;
using ShoppingCartApp.Presentation;
using PagedList;

namespace ShoppingCartApp.Controllers
{
    public class CategoryController : Controller
    {
        // GET: Category
        public ActionResult Index()
        {
            return View();
        }
        
        public ActionResult Browse(int? id, int? page)
        {
            BrowseView browseView;
            

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = new Category();
            try
            {
                category = ModelLists.Categories.Where(cat => cat.ClassId == id).Single();
                browseView = new BrowseView()
                {
                    Category = category,
                    Products = category.Products.Where(x => x.InactiveProduct == false).OrderBy(x => x.ProductName).ToPagedList(page ?? 1, 12)
                };
            }
            catch (Exception)
            {
                return HttpNotFound();
            }

            return View(browseView);
        }
    }
}