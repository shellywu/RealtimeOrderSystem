using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using OrderSystem.Core.Models;
using System.Linq.Expressions;
using OrderSystem.Core.Helper;

namespace OrderSystem.Core.Controllers
{
    public class OrdersController : ApiController
    {
        private OrderDbContext db = new OrderDbContext();

        public OrderListView GetOrders([FromUri]OrderListQuery olq)
        {
            OrderListView olv = new OrderListView();
            olv.Count = db.Orders.Count();

            var orders = db.Orders.Include(o => o.Customer).Include(o => o.OrderItems).OrderByDescending(o => o.OrderDate).Skip(olq.PageSize * (olq.RowCount - 1)).Take(olq.PageSize).Select(o => new OrderListItemView
            {
                OrderCode = o.OrderCode,
                Id = o.Id,
                OrderStatus = o.OrderStatus,
                OrderDate = o.OrderDate,
                Contract = o.Customer.CPhone,
                CustomerName = o.Customer.CName,
                ProductNames = o.OrderItems.Where(oi=>oi.State==0).Select(oi => oi.Describe).ToList<string>(),
                Remark=o.Remark
            }).ToList();
            olv.OrderListViewItems = orders;
            return olv;
        }

        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutOrder(Guid id, Order order)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != order.Id)
            {
                return BadRequest();
            }

            db.Entry(order).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        [ResponseType(typeof(OrderListItemView))]
        public async Task<IHttpActionResult> PostOrder(Order order)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            order.Id = Guid.NewGuid();
            order.OrderCode = DateTime.Now.ToString("yyMMddhhmmss");
            order.ComplteDate = DateTime.Now;
            foreach (var item in order.OrderItems)
            {
                item.Id = Guid.NewGuid();
                foreach (var p in item.Persons)
                {
                    p.OrderItemId = item.Id;
                }
                item.OrderId = order.Id;
            }
            order.Remark = "创建订单";
            db.Orders.Add(order);

            try
            {
                await db.SaveChangesAsync();
                OrderListItemView oltv = new OrderListItemView();
                oltv.Id = order.Id;
                oltv.OrderCode = order.OrderCode;
                oltv.OrderDate = order.OrderDate;
                oltv.OrderStatus = order.OrderStatus;
                oltv.CustomerName = order.Customer.CName;
                oltv.Contract = order.Customer.CPhone;
                oltv.ProductNames = order.OrderItems.Select(oi => oi.Describe).ToList<string>();
                oltv.Remark = order.Remark;
                return Ok(oltv);
            }
            catch (DbUpdateException)
            {
                if (OrderExists(order.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }
        }

        // DELETE: api/Orders/5
        [ResponseType(typeof(Order))]
        public async Task<IHttpActionResult> DeleteOrder(Guid id)
        {
            Order order = await db.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            db.Orders.Remove(order);
            await db.SaveChangesAsync();

            return Ok(order);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool OrderExists(Guid id)
        {
            return db.Orders.Count(e => e.Id == id) > 0;
        }
    }

}