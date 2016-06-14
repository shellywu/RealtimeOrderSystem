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
    public class PersonInfoesController : Controller
    {
        private OrderDbContext db = new OrderDbContext();

        // GET: PersonInfoes
        public ActionResult Index()
        {
            var personInfo = db.PersonInfo.Include(p => p.Orderitem);
            return View(personInfo.ToList());
        }

        // GET: PersonInfoes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PersonInfo personInfo = db.PersonInfo.Find(id);
            if (personInfo == null)
            {
                return HttpNotFound();
            }
            return View(personInfo);
        }

        // GET: PersonInfoes/Create
        public ActionResult Create()
        {
            ViewBag.OrderItemId = new SelectList(db.OrderItems, "Id", "ProductId");
            return View();
        }

        // POST: PersonInfoes/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,PersonName,PersonPhone,PersonIdentity,CredentialType,OrderItemId,Remark")] PersonInfo personInfo)
        {
            if (ModelState.IsValid)
            {
                db.PersonInfo.Add(personInfo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.OrderItemId = new SelectList(db.OrderItems, "Id", "ProductId", personInfo.OrderItemId);
            return View(personInfo);
        }

        // GET: PersonInfoes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PersonInfo personInfo = db.PersonInfo.Find(id);
            if (personInfo == null)
            {
                return HttpNotFound();
            }
            ViewBag.OrderItemId = new SelectList(db.OrderItems, "Id", "ProductId", personInfo.OrderItemId);
            return View(personInfo);
        }

        // POST: PersonInfoes/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,PersonName,PersonPhone,PersonIdentity,CredentialType,OrderItemId,Remark")] PersonInfo personInfo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(personInfo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.OrderItemId = new SelectList(db.OrderItems, "Id", "ProductId", personInfo.OrderItemId);
            return View(personInfo);
        }

        // GET: PersonInfoes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PersonInfo personInfo = db.PersonInfo.Find(id);
            if (personInfo == null)
            {
                return HttpNotFound();
            }
            return View(personInfo);
        }

        // POST: PersonInfoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PersonInfo personInfo = db.PersonInfo.Find(id);
            db.PersonInfo.Remove(personInfo);
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
