using OrderSystem.Core.Helper;
using OrderSystem.Core.Models;
using OrderSystem.Core.RealTimeConn.Core;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace OrderSystem.Core.Controllers
{
    public class CancelProcessController : ApiController
    {
        private OrderDbContext db = new OrderDbContext();
        private ChangeState _changeState = null;

        public CancelProcessController() : this(ChangeState.Instance) { }

        public CancelProcessController(ChangeState cs)
        {
            _changeState = cs;
        }

        [ResponseType(typeof(OrderDetail))]
        public OrderDetail GetOrderDetail(Guid id)
        {
            OrderDetail od = new OrderDetail();

            var order = db.Orders.Include("OrderItems").Where(o => o.Id == id).FirstOrDefault();
            od.Id = order.Id;
            od.OrderItems = order.OrderItems.ToList();
            var queryTask = db.Tasks.Where(ta => ta.OrderCode == order.OrderCode);
            od.Tasks = queryTask.ToList();
            return od;
        }

        [ResponseType(typeof(List<CanelModel>))]
        public List<CanelModel> PostOrderDetail(OrderDetailsQuery odq)
        {
            var list = new List<Expression<Func<Order, bool>>>();

            if (!string.IsNullOrEmpty(odq.OrderCode))
            {
                list.Add(o => o.OrderCode == odq.OrderCode);
            }
            
            if (!string.IsNullOrEmpty(odq.PhoneCode))
            {
                list.Add(o => o.Customer.CPhone == odq.PhoneCode);
            }
            if (odq.OrderStartDate != DateTime.MinValue && odq.OrderEndDate.Date >= odq.OrderStartDate.Date)
            {
                list.Add(o => o.OrderDate > odq.OrderStartDate.Date);
            }
            if (odq.OrderEndDate != DateTime.MinValue && odq.OrderEndDate.Date >= odq.OrderStartDate.Date)
            {
                list.Add(o => o.OrderDate < odq.OrderEndDate.Date.AddHours(23).AddMinutes(59));
            }
            if (!string.IsNullOrEmpty(odq.CertCode))
            {
              var id=db.OrderItems.FirstOrDefault(oi=>oi.CertificateNum==odq.CertCode).OrderId;
              var orders = db.Orders.Include("Customer").Where(o=>o.Id==id).Select(c => new CanelModel
              {
                  Id = c.Id,
                  OrderCode = c.OrderCode,
                  OrderDate = c.OrderDate,
                  OrderStatus = c.OrderStatus,
                  CName = c.Customer.CName,
                  CPhone = c.Customer.CPhone
              }).ToList();
              return orders;
            }
            else
            {
                Expression<Func<Order, bool>> express = PredicateBuilder.True<Order>();
                foreach (var item in list)
                {
                    express = express.And(item);
                }

                var orders = db.Orders.Include("Customer").Where(express).OrderByDescending(o => o.OrderDate).Select(c => new CanelModel
                {
                    Id = c.Id,
                    OrderCode = c.OrderCode,
                    OrderDate = c.OrderDate,
                    OrderStatus = c.OrderStatus,
                    CName = c.Customer.CName,
                    CPhone = c.Customer.CPhone
                }).ToList();
                return orders;
            }
        }

        [HttpPut]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutOrderStatus(Guid id)
        {
            string err = string.Empty;
            try
            {
                var order = db.Orders.Include("OrderItems").FirstOrDefault(o => o.Id == id);
                if (order.OrderStatus != OrderStatus.Partion)
                {
                    ///取消还未下单的订单
                    var tasks = db.Tasks.Include(t=>t.Personinfos).Where(t => t.OrderCode == order.OrderCode).ToList();
                    if (tasks.Count > 0)
                    {
                        bool isCancelOrder = true;
                        foreach (var item in tasks)
                        {
                            if (item.State == TaskState.Create || item.State == TaskState.Take || item.State == TaskState.Change)
                            {
                                item.State = TaskState.Canceled;
                                await db.SaveChangesAsync();
                                _changeState.SendUpdateTaskState(item);
                            }
                            if (item.State == TaskState.Finish)
                            {
                                isCancelOrder = false;
                                TaskOrderModel tom = new TaskOrderModel();
                                tom.OrderCode = item.OrderCode;
                                tom.OrderItemId = item.OrderItemId;
                                tom.ProductCode = item.ProductCode;
                                tom.ProductFrom = item.ProductFrom;
                                tom.ProductName = item.ProductName;
                                tom.ProductType = item.ProductType;
                                tom.Quantity = item.Quantity;
                                tom.Remark = item.Remark;
                                tom.UsedEndDate = item.UsedEndDate;
                                tom.UsedStartDate = item.UsedStartDate;
                                foreach (var it in item.Personinfos)
                                {
                                    tom.Personinfos.Add(it);
                                }
                                tom.State = TaskState.CreateCancel;
                                db.Tasks.Add(tom);
                                await db.SaveChangesAsync();
                                _changeState.SendUpdateTaskState(tom, true);
                            }
                        }
                        if (isCancelOrder)
                        {
                            order.OrderStatus = OrderStatus.Canceled;
                            order.Remark = order.Remark + "订单取消完成";
                        }
                        else {
                            order.OrderStatus = OrderStatus.Cancel;
                        }
                    }
                    else
                    {
                        order.OrderStatus = OrderStatus.Canceled;
                    }
                    await db.SaveChangesAsync();
                    OrderUpdateModel oum = new OrderUpdateModel();
                    oum.Id = order.Id;
                    oum.State = order.OrderStatus;
                    oum.Remark = order.Remark;
                    _changeState.SendUpdateOrderState(oum);
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
}
