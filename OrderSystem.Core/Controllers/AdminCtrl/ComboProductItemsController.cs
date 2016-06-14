using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OrderSystem.Core.Models;

namespace OrderSystem.Core.Controllers.AdminCtrl
{
    [Authorize]
    public class ComboProductItemsController : Controller
    {
        private OrderDbContext db = new OrderDbContext();

        // GET: ComboProductItems
        public ActionResult Index(int? id)
        {
            var comboProductItems = db.ComboProductItems.Include(c => c.ComboProduct).Include(c => c.Product).Where(c=>c.ComboProductId==id).ToList();
            ViewBag.ComboProductId = id;
            return View(comboProductItems.ToList());
        }

        // GET: ComboProductItems/Details/5
        public ActionResult Details(int? id,int?pid)
        {
            if (id == null||pid==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ComboProductItem comboProductItem = db.ComboProductItems.FirstOrDefault(c=>c.ComboProductId==id&&c.ProductId==pid);
            if (comboProductItem == null)
            {
                return HttpNotFound();
            }
            return View(comboProductItem);
        }

        // GET: ComboProductItems/Create
        public ActionResult Create(int?id)
        {
            ViewBag.ComboProductId = new SelectList(db.ComboProducts, "Id", "ComboCode",id);
            ViewBag.ProductId = new SelectList(db.Products, "Id", "ProductName");
            ViewBag.ComboId = id;
            return View();
        }

        // POST: ComboProductItems/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ComboProductId,ProductId,Quantity,ProductPrice,ProductCommision,ProductSettlement,ProductContractPrice")] ComboProductItem comboProductItem)
        {
            if (ModelState.IsValid)
            {
                db.ComboProductItems.Add(comboProductItem);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ComboProductId = new SelectList(db.ComboProducts, "Id", "ComboCode", comboProductItem.ComboProductId);
            ViewBag.ProductId = new SelectList(db.Products, "Id", "ProductName", comboProductItem.ProductId);
            return View(comboProductItem);
        }

        // GET: ComboProductItems/Edit/5
        public ActionResult Edit(int? id,int?pid)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ComboProductItem comboProductItem = db.ComboProductItems.FirstOrDefault(c=>c.ComboProductId==id&&c.ProductId==pid);
            if (comboProductItem == null)
            {
                return HttpNotFound();
            }
            ViewBag.ComboProductId = new SelectList(db.ComboProducts, "Id", "ComboCode", comboProductItem.ComboProductId);
            ViewBag.ProductId = new SelectList(db.Products, "Id", "ProductName", comboProductItem.ProductId);
            ViewBag.cid = id;
            ViewBag.pid = pid;
            return View(comboProductItem);
        }

        // POST: ComboProductItems/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ComboProductId,ProductId,Quantity,ProductPrice,ProductCommision,ProductSettlement,ProductContractPrice")] ComboProductItem comboProductItem)
        {
            if (ModelState.IsValid)
            {
                db.Entry(comboProductItem).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ComboProductId = new SelectList(db.ComboProducts, "Id", "ComboCode", comboProductItem.ComboProductId);
            ViewBag.ProductId = new SelectList(db.Products, "Id", "ProductName", comboProductItem.ProductId);
            return View(comboProductItem);
        }

        // GET: ComboProductItems/Delete/5
        public ActionResult Delete(int? id,int ?pid)
        {
            if (id == null||pid==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ComboProductItem comboProductItem = db.ComboProductItems.FirstOrDefault(c=>c.ComboProductId==id&&c.ProductId==pid);
            if (comboProductItem == null)
            {
                return HttpNotFound();
            }
            ViewBag.cid = id;
            ViewBag.pid = pid;
            return View(comboProductItem);
        }

        // POST: ComboProductItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id,int pid)
        {
            ComboProductItem comboProductItem = db.ComboProductItems.FirstOrDefault(c=>c.ComboProductId==id&&c.ProductId==pid);
            db.ComboProductItems.Remove(comboProductItem);
            db.SaveChanges();
            
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
