using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ShoppingCartApp.Models;
using System.IO;
using System.Threading.Tasks;
using ShoppingCartApp.Presentation;

namespace ShoppingCartApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class MenusController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private string path;

        // GET: Menus
        public ActionResult Index()
        {
            return View(HierarchicalModelLists.Categories);
        }

        // GET: Menus/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category menu = ModelLists.Categories.Where(cat => cat.ClassId == id).Single();
            if (menu == null)
            {
                return HttpNotFound();
            }
            return View(menu);
        }

        // GET: Menus/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Menus/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ClassId,ClassName,ParentClassID,Description,ImageUrl")] Category menu, HttpPostedFileBase file)
        {
            
            if (file != null && file.ContentLength > 0)
            {
                var fileName = Path.GetFileName(file.FileName);
                path = Path.Combine(Server.MapPath("~/images"), fileName);
                menu.ImageUrl = "~/images/" + fileName;
            }
            try
            {
                if (ModelState.IsValid)
                {
                    if (file != null && file.ContentLength > 0)
                    {
                        file.SaveAs(path);
                    }
                    db.MenuItems.Add(menu);
                    await db.SaveChangesAsync();
                    ModelLists.NewModelList();
                    HierarchicalModelLists.NewHierarchicalModelLists();

                    return RedirectToAction("Index");
                }
            }
            catch (Exception)
            {
                ViewBag.Message = "Image must be PNG or JPG file";
                return View(menu);
            }
               
            return View(menu);
        }

        // GET: Menus/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category menu = ModelLists.Categories.Where(cat => cat.ClassId == id).Single();
            if (menu == null)
            {
                return HttpNotFound();
            }
            return View(menu);
        }

        // POST: Menus/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ClassId,ClassName,ParentClassID,Description,ImageUrl")] Category menu, HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
            {
                var fileName = Path.GetFileName(file.FileName);
                path = Path.Combine(Server.MapPath("~/images"), fileName);
                menu.ImageUrl = "~/images/" + fileName;
            }
            try
            {
                if (ModelState.IsValid)
                {
                    if (file != null && file.ContentLength > 0)
                    {
                        file.SaveAs(path);
                    }
                    db.Entry(menu).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    ModelLists.NewModelList();
                    HierarchicalModelLists.NewHierarchicalModelLists();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception)
            {
                ViewBag.Message = "Image must be PNG or JPG file";
                return View(menu); 
            }
            
            return View(menu);
        }

        // GET: Menus/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category menu = ModelLists.Categories.Where(cat => cat.ClassId == id).Single();
            if (menu == null)
            {
                return HttpNotFound();
            }
            return View(menu);
        }

        // POST: Menus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Category menu = await db.MenuItems.FindAsync(id);
            db.MenuItems.Remove(menu);
            await db.SaveChangesAsync();
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
