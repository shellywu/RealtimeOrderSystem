using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OrderSystem.Core.Models;

namespace OrderSystem.Core.Controllers.AdminCtrl
{
    [Authorize]

    public class ComboProductsController : Controller
    {
        private OrderDbContext db = new OrderDbContext();

        // GET: ComboProducts
        public async Task<ActionResult> Index()
        {
            return View(await db.ComboProducts.ToListAsync());
        }

        // GET: ComboProducts/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ComboProduct comboProduct = await db.ComboProducts.FindAsync(id);
            if (comboProduct == null)
            {
                return HttpNotFound();
            }
            return View(comboProduct);
        }

        // GET: ComboProducts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ComboProducts/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,ComboCode,ComboName,ValidStartDate,ValidEndDate,Price,ProductPrice,ProductCommision,ProductSettlement,ProductContractPrice")] ComboProduct comboProduct)
        {
            if (ModelState.IsValid)
            {
                db.ComboProducts.Add(comboProduct);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(comboProduct);
        }

        // GET: ComboProducts/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ComboProduct comboProduct = await db.ComboProducts.FindAsync(id);
            if (comboProduct == null)
            {
                return HttpNotFound();
            }
            return View(comboProduct);
        }

        // POST: ComboProducts/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,ComboCode,ComboName,ValidStartDate,ValidEndDate,Price,ProductPrice,ProductCommision,ProductSettlement,ProductContractPrice")] ComboProduct comboProduct)
        {
            if (ModelState.IsValid)
            {
                db.Entry(comboProduct).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(comboProduct);
        }

        // GET: ComboProducts/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ComboProduct comboProduct = await db.ComboProducts.FindAsync(id);
            if (comboProduct == null)
            {
                return HttpNotFound();
            }
            return View(comboProduct);
        }

        // POST: ComboProducts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ComboProduct comboProduct = await db.ComboProducts.FindAsync(id);
            db.ComboProducts.Remove(comboProduct);
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
