using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OrderSystem.Core.Models;

using OrderSystem.Core.Controllers.AdminCtrl.QueryModel;
using System.Linq.Expressions;
using OrderSystem.Core.Helper;

namespace OrderSystem.Core.Controllers.AdminCtrl
{
    [Authorize]
    public class OrdersController : Controller
    {
        private OrderDbContext db = new OrderDbContext();

        // GET: Orders
        public ActionResult Index()
        {
            var pageCount = db.Orders.Count();
            ViewBag.PageCount = pageCount;
            ViewBag.PageIndex = 1;
            return View(db.Orders.OrderByDescending(o => o.Id).Take(8).ToList());
        }

        [HttpPost]
        // GET: Orders
        public ActionResult Index(OrderQueryModel oqm)
        {
            var list = new List<Expression<Func<Order, bool>>>();

            if (!string.IsNullOrEmpty(oqm.OrderCode))
            {
                list.Add(o => o.OrderCode==oqm.OrderCode);
            }
            if (!string.IsNullOrEmpty(oqm.CPhone))
            {
                list.Add(o => o.Customer.CPhone.Equals(oqm.CPhone));
            }
            if (oqm.Status !=OrderStatus.Err)
            {
                list.Add(o => oqm.Status==o.OrderStatus);
            }
            if (oqm.OrderEndDate != DateTime.MinValue&&oqm.OrderStartDate<=oqm.OrderEndDate)
            {
                list.Add(o=>o.OrderDate<=oqm.OrderEndDate);
            }
            if (oqm.OrderStartDate != DateTime.MinValue&&oqm.OrderStartDate<=oqm.OrderEndDate)
            {
                list.Add(o=>o.OrderDate>oqm.OrderStartDate);
            }

            Expression<Func<Order, bool>> _express = PredicateBuilder.True<Order>();
            foreach (var item in list)
            {
                _express = _express.And(item);
            }
            var count = db.Orders.Where<Order>(_express).OrderByDescending(o => o.Id).Count();
            var pageCount =Math.Ceiling((double)(count /oqm.PageSize));
            ViewBag.PageCount = pageCount;
            ViewBag.PageIndex = oqm.PageIndex;
            var queryData = db.Orders.Where<Order>(_express).OrderByDescending(o=>o.Id).Skip((oqm.PageIndex-1)*oqm.PageSize).Take(oqm.PageSize);
            return View(queryData.ToList());
        }

        // GET: Orders/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Include("Customer").First(e=>e.Id==(Guid)id);
            List<OrderItem> ois = db.OrderItems.Where(o => o.OrderId == order.Id).ToList();
            List<TaskOrderModel> tom = db.Tasks.Where(k=>k.OrderCode==order.OrderCode).ToList();
            ViewBag.ois=ois;
            ViewBag.tom = tom;
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // GET: Orders/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Orders/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,OrderCode,OrderDate,ComplteDate,OrderStatus,Remark")] Order order)
        {
            if (ModelState.IsValid)
            {
                order.Id = Guid.NewGuid();
                db.Orders.Add(order);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(order);
        }

        // GET: Orders/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Orders/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,OrderCode,OrderDate,ComplteDate,OrderStatus,Remark")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(order);
        }

        // GET: Orders/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Order order = db.Orders.Find(id);
            db.Orders.Remove(order);
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
