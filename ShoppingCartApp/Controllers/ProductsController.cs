using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ShoppingCartApp.Models;
using ShoppingCartApp.Presentation;
using System.IO;

namespace ShoppingCartApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProductsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private string path;

        // GET: Products
        public ActionResult Index(string searchWord)
        {
            var products = ModelLists.Products.OrderBy(x => x.ProductName).ToList();

            if (searchWord == "" || searchWord == null)
            {
                ViewBag.searchWord = "";
                return View(products);
            }
            ViewBag.searchWord = searchWord;
            var productsSearched = ModelLists.Products.Where(x => x.ProductName.ToLower().Contains(searchWord.ToLower())).OrderBy(x => x.ProductName).ToList();



            return View(productsSearched);
        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = ModelLists.Products.Where(prod => prod.ProductID == id).Single();
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ProductID,ProductCode,ProductName,ShortDescription,LongDescription,Quantity,MSRP,Price,WildmanPrice,Length,Width,Height,ParcelPost,InactiveProduct,NoBackOrders,LimitOne,New,KeepInventory,SpecialOrder,FeaturedProduct,ClassID")] Product product, HttpPostedFileBase[] files)
        {
            for (int i = 0; i < files.Length; i++)
            {
                if (files[i] != null && files[i].ContentLength > 0)
                {
                    var extension = Path.GetExtension(files[i].FileName);
                    if (extension != ".png" && extension != ".jpg" && extension != ".jpeg")
                    {
                        ViewBag.Message = "One or more files has an invalid file extension";
                        return View(product);
                    }
                }
            }
            try
            {
                if (ModelState.IsValid)
                {
                    db.Products.Add(product);
                    await db.SaveChangesAsync();
                    for (int i = 0; i < files.Length; i++)
                    {
                        if (files[i] != null && files[i].ContentLength > 0)
                        {
                            var fileName = Path.GetFileName(files[i].FileName);
                            path = Path.Combine(Server.MapPath("~/images"), fileName);
                            ProductImages image = new ProductImages();
                            image.Path = "~/images/" + fileName;
                            image.ProductID = product.ProductID;
                            if (i == 0)
                            {
                                image.Featured = true;
                            }
                            files[i].SaveAs(path);
                            db.ProductImages.Add(image);
                            await db.SaveChangesAsync();
                        }
                    }
                    ModelLists.NewModelList();
                    HierarchicalModelLists.NewHierarchicalModelLists();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception)
            {
                return View(product);
            }
            
            return View(product);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id, string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = ModelLists.Products.Where(prod => prod.ProductID == id).Single();
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ProductID,ProductCode,ProductName,ShortDescription,LongDescription,Quantity,MSRP,Price,WildmanPrice,Length,Width,Height,ParcelPost,InactiveProduct,NoBackOrders,LimitOne,New,KeepInventory,SpecialOrder,FeaturedProduct,ClassID")] Product product, HttpPostedFileBase[] files, string returnUrl)
        {
            for (int i = 0; i < files.Length; i++)
            {
                if (files[i] != null && files[i].ContentLength > 0)
                {
                    var extension = Path.GetExtension(files[i].FileName);
                    if (extension != ".png" && extension != ".jpg" && extension != ".jpeg")
                    {
                        ViewBag.Message = "One or more files has an invalid file extension";
                        return View(product);
                    }
                }
            }
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                await db.SaveChangesAsync();
                for (int i = 0; i < files.Length; i++)
                {
                    if (files[i] != null && files[i].ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(files[i].FileName);
                        path = Path.Combine(Server.MapPath("~/images"), fileName);
                        ProductImages image = new ProductImages();
                        image.Path = "~/images/" + fileName;
                        image.ProductID = product.ProductID;
                        image.Featured = false;
                        files[i].SaveAs(path);
                        if (i == 0)
                        {
                            /*Ensure that at least one image is set as featured*/
                            ProductImages images = ModelLists.ProductImages.Where(x => x.ProductID == product.ProductID && x.Featured == true).OrderByDescending(x => x.Featured).SingleOrDefault();
                            if (images == null)
                            {
                                image.Featured = true;
                            }
                        }
                        db.ProductImages.Add(image);
                        await db.SaveChangesAsync();
                    }
                }
                ModelLists.NewModelList();
                HierarchicalModelLists.NewHierarchicalModelLists();

                if (product.InactiveProduct)
                {
                    return RedirectToAction("Index");
                }

                if (!string.IsNullOrEmpty(returnUrl))
                {
                    return Redirect(returnUrl);
                }

                return RedirectToAction("Index");
            }
            return View(product);
        }

        public PartialViewResult EditImage(int id)
        {
            List<ProductImages> images = ModelLists.ProductImages.Where(x => x.ProductID == id).OrderByDescending(x=> x.Featured).ToList();

            return PartialView("_EditImage", images);
        }
        
        public async Task<ActionResult> DeleteImage(int id, int productID)
        {
            ProductImages image = await db.ProductImages.Where(x => x.ID == id ).SingleAsync();
            db.ProductImages.Remove(image);
            await db.SaveChangesAsync();
            ModelLists.NewModelList();
            HierarchicalModelLists.NewHierarchicalModelLists();
            List<ProductImages> images = ModelLists.ProductImages.Where(x => x.ProductID == productID).ToList();
            List<ProductImages> defaultImages = ModelLists.ProductImages.Where(x => x.ProductID == productID && x.Featured).ToList();

            if (images.Count > 0 && defaultImages.Count == 0)
            {
                images[0].Featured = true;
                db.Entry(images[0]).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }

            return PartialView("_EditImage", images);
        }

        public async Task<ActionResult> PrimaryImage(int productID, int id)
        {
            wildman1Entities1 dbp = new wildman1Entities1();
            dbp.Make_Featured_Image(productID, id);
            await dbp.SaveChangesAsync();
            ModelLists.NewModelList();
            HierarchicalModelLists.NewHierarchicalModelLists();
            List<ProductImages> images = ModelLists.ProductImages.Where(x => x.ProductID == productID).OrderByDescending(x => x.Featured).ToList();
            return PartialView("_EditImage", images);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = ModelLists.Products.Where(prod => prod.ProductID == id).Single();
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            wildman1Entities1 dbp = new wildman1Entities1();
            Product product = await db.Products.FindAsync(id);
            db.Products.Remove(product);
            await db.SaveChangesAsync();
            dbp.Delete_All_Product_Images(id);
            await dbp.SaveChangesAsync();
            ModelLists.NewModelList();
            HierarchicalModelLists.NewHierarchicalModelLists();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
