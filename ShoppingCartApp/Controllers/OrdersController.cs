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
using System.Text;

namespace ShoppingCartApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class OrdersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Orders
        public async Task<ActionResult> Index(int? searchID)
        {
            if (searchID != null)
            {
                if (searchID > 0)
                {
                    return View(await db.Orders.Where(x => x.OrderID == searchID).OrderBy(x => x.archived).OrderByDescending(x => x.OrderDate).ToListAsync());
                }
            }
            ViewBag.searchWord = "";
            return View(await db.Orders.OrderBy(x => x.archived).OrderByDescending(x => x.OrderDate).ToListAsync());

        }

        // GET: Orders/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = await db.Orders.FindAsync(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            order.DecryptAndPopulateCreditCardFields(order.Details);

            OrderDetailView orderDetail = new OrderDetailView
            {
                Order = order,
                OrderItems = order.OrderItems
            };

            return View(orderDetail);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RegisterOrder(int id)
        {
            Order order = db.Orders.Find(id);
            order.archived = true;
            order.DecryptAndPopulateCreditCardFields(order.Details);

            string[] creditDetails = { order.CardHolderName, order.CardType, order.CVC, order.ExpirationDate,
                Encryption.Encrypt(order.CardNumber = "************" + order.CardNumber.Substring(Math.Max(0, order.CardNumber.Length - 4))) };

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

            order.Details = sb.ToString();
            db.Entry(order).State = EntityState.Modified;

            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        
        // GET: Orders/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = await db.Orders.FindAsync(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Name,Addr1,Country,City,State,PostalCode,email,phone,Comments,ShipName,ShipAddr1,ShipCountry,ShipCity,ShipState,ShipPostalCode,Details")] Order order)
        {
            TryUpdateModel(order);
            ModelState.Remove("CardNumber");
            ModelState.Remove("CVC");
            ModelState.Remove("CardHolderName");
            ModelState.Remove("ExpirationDate");

            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                await db.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = await db.Orders.FindAsync(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Order order = await db.Orders.FindAsync(id);
            db.Orders.Remove(order);
            await db.SaveChangesAsync();
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
