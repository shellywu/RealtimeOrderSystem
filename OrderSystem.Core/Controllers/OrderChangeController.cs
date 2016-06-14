using OrderSystem.Core.Models;
using OrderSystem.Core.RealTimeConn.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace OrderSystem.Core.Controllers
{
    public class OrderChangeController : ApiController
    {
        private OrderDbContext db = new OrderDbContext();
        private ChangeState _changeState = null;

        public OrderChangeController() : this(ChangeState.Instance) { }

        public OrderChangeController(ChangeState cs)
        {
            _changeState = cs;
        }

        [ResponseType(typeof(ChagneOrderDetial))]
        public ChagneOrderDetial GetOrderDetail(Guid id)
        {
            ChagneOrderDetial od = new ChagneOrderDetial();

            var order = db.Orders.Include("OrderItems").Where(o => o.Id == id).FirstOrDefault();
            od.Id = order.Id;
            od.OrderItems = order.OrderItems.ToList();
            return od;
        }
        [HttpPut]
        public OrderItem PutOrderItemDetail(Guid id)
        {
            var orderItem = db.OrderItems.Include("Persons").Where(oi => oi.Id == id).FirstOrDefault();
            return orderItem;
        }

        [HttpPost]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PostOrderItemDetail(ChangeOrderItemModel coim)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var order = db.Orders.Where(o => o.Id == coim.OrderId).FirstOrDefault();
            if (order == null)
            {
                return NotFound();
            }
            if (order.OrderStatus == OrderStatus.Partion) {
                return BadRequest();
            }
            try
            {
                OrderItem oi = new OrderItem();
                oi.Id = Guid.NewGuid();
                oi.ProductId = coim.OrderItem.ProductId;
                oi.ProductType = coim.OrderItem.ProductType;
                oi.CertificateDate = coim.OrderItem.CertificateDate;
                oi.CertificateNum = coim.OrderItem.CertificateNum;
                oi.CustomerPrice = 0;
                oi.Describe = coim.OrderItem.Describe;
                oi.EndDate = coim.OrderItem.EndDate;
                oi.StartDate = coim.OrderItem.StartDate;
                oi.TotalPrice = coim.OrderItem.TotalPrice;
                oi.OrderId = coim.OrderId;
                oi.Remark = coim.OrderItem.Remark;
                oi.Quantity = coim.OrderItem.Quantity;
                oi.Persons = new List<PersonInfo>();
                foreach (var item in coim.OrderItem.Persons)
                {
                    PersonInfo pi = item;
                    oi.Persons.Add(pi);
                }

                order.OrderItems.Add(oi);
                order.OrderItems.First(o => o.Id == coim.OrderItem.Id).State = 1;
                order.OrderStatus = OrderStatus.Partion;
                order.Remark += "修改订单信息，重新下单";
                db.SaveChanges();
               
              
                string err = string.Empty;
                TaskAdapter ta = new TaskAdapter();
                var tasks = ta.OrderItem2Task(oi, order.OrderCode,db, ref err);
                if (string.IsNullOrEmpty(err))
                {
                    ta.SendTask2Group(tasks, _changeState.Group);
                    OrderUpdateModel oum = new OrderUpdateModel();
                    oum.Id = order.Id;
                    oum.State = order.OrderStatus;
                    oum.Remark = order.Remark;
                    _changeState.SendUpdateOrderState(oum);
                }
                else
                {
                    throw new Exception(err);
                }
            }
            catch (Exception)
            {
                return InternalServerError(); 
            }
            return Ok();
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

    public class ChangeOrderItemModel
    {
        public Guid OrderId { get; set; }
        public OrderItem OrderItem { get; set; }
    }
}
