using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using OrderSystem.Core.RealTimeConn.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrderSystem.Core.Models
{
    public class TaskAdapter
    {
        private OrderDbContext db = new OrderDbContext();

        private IConnectionGroupManager _groups = null;

        private bool _isCancelOrder = false;
        /// <summary>
        /// 将订单转化为task
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public List<TaskOrderModel> Order2Task(Order order, ref string err, bool isCancelOrder = false)
        {
            _isCancelOrder = isCancelOrder;
            List<TaskOrderModel> taskOrders = new List<TaskOrderModel>();
            try
            {
                List<OrderItem> orderItems = db.OrderItems.Include("Persons").Where(o => o.OrderId == order.Id).ToList();

                foreach (var item in orderItems)
                {
                    switch (item.ProductType)
                    {
                        case ProductType.Combo: taskOrders.AddRange(GetProducts(item, order.OrderCode)); break;
                        default: taskOrders.Add(GetProduct(item, order.OrderCode)); break;
                    }
                }

                db.Tasks.AddRange(taskOrders);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                err = ex.Message;
            }
            return taskOrders;
        }

        public List<TaskOrderModel> OrderItem2Task(OrderItem orderItem, string orderCode,OrderDbContext db, ref string err)
        {
            List<TaskOrderModel> taskOrders = new List<TaskOrderModel>();
            try
            {
                switch (orderItem.ProductType)
                {
                    case ProductType.Combo: taskOrders.AddRange(GetProducts(orderItem, orderCode)); break;
                    default: taskOrders.Add(GetProduct(orderItem, orderCode)); break;
                }

                foreach (var item in taskOrders)
                {
                    item.State = TaskState.Change;
                }

                db.Tasks.AddRange(taskOrders);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                err = ex.Message;
            }
            return taskOrders;
        }


        /// <summary>
        /// 通过单产品生成task
        /// </summary>
        /// <param name="item"></param>
        /// <param name="orderCode"></param>
        /// <returns></returns>
        private TaskOrderModel GetProduct(OrderItem item, string orderCode)
        {
            TaskOrderModel tom = new TaskOrderModel();
            int id = int.Parse(item.ProductId);
            var pro = db.Products.Where(p => p.Id == id).FirstOrDefault();
            tom.TaskCode = DateTime.Now.ToString("yyMMddhhmmss") + id;
            tom.ProductType = pro.ProductType;
            tom.ProductName = pro.ProductName;
            tom.ProductFrom = "单产品";
            tom.ProductCode = pro.ProductCode;
            tom.Quantity = item.Quantity;
            tom.OrderCode = orderCode;
            tom.UsedStartDate = item.StartDate;
            tom.UsedEndDate = item.EndDate;
            tom.OrderItemId = item.Id;
            tom.Remark = _isCancelOrder ? item.Remark + "【取消订单】" : item.Remark;
            tom.State = _isCancelOrder ? TaskState.CreateCancel : TaskState.Create;
            foreach (var p in item.Persons)
            {
                tom.Personinfos.Add(p);
            }

            return tom;
        }

        /// <summary>
        /// 通过复合产品生成task
        /// </summary>
        /// <param name="item"></param>
        /// <param name="orderId"></param>
        /// <returns></returns>
        private IEnumerable<TaskOrderModel> GetProducts(OrderItem item, string orderId)
        {
            List<TaskOrderModel> toms = new List<TaskOrderModel>();
            int id = int.Parse(item.ProductId);

            var combItems = db.ComboProductItems.Include("Product").Where(cp => cp.ComboProductId == id);
            foreach (var citem in combItems.ToList())
            {
                TaskOrderModel tom = new TaskOrderModel();
                tom.TaskCode = DateTime.Now.ToString("yyMMddhhmmss") + id;
                tom.ProductType = citem.Product.ProductType;
                tom.ProductName = citem.Product.ProductName;
                tom.ProductFrom = "套餐";
                tom.ProductCode = citem.Product.ProductCode;
                tom.Quantity = citem.Quantity * item.Quantity;
                tom.OrderCode = orderId;
                tom.UsedStartDate = item.StartDate;
                tom.UsedEndDate = item.EndDate;
                tom.OrderItemId = item.Id;
                foreach (var per in item.Persons)
                {
                    tom.Personinfos.Add(per);
                }
                tom.Remark = _isCancelOrder ? item.Remark + "【取消订单】" : item.Remark;
                tom.State = _isCancelOrder ? TaskState.CreateCancel : TaskState.Create;
                toms.Add(tom);
            }
            return toms;
        }


        public void Dispose()
        {
            db.Dispose();
        }

        internal void SendTask2Group(List<TaskOrderModel> taskOrders, Microsoft.AspNet.SignalR.IConnectionGroupManager Groups)
        {
            _groups = Groups;
            foreach (var item in taskOrders)
            {
                switch (item.ProductType)
                {
                    case ProductType.Hotel: Send2HotelGroup(item); break;
                    case ProductType.Tax: Send2TaxGroup(item); break;
                    case ProductType.Titcket: Send2TitcketGroup(item); break;
                }
            }
        }

        private void Send2TitcketGroup(TaskOrderModel item)
        {
            ResponseEntry re = new ResponseEntry();
            re.T = "k";
            re.D = item;
            this._groups.Send("Tiket", GetObjectJson(re));
        }

        private void Send2TaxGroup(TaskOrderModel item)
        {
            ResponseEntry re = new ResponseEntry();
            re.T = "t";
            re.D = item;
            this._groups.Send("Tax", GetObjectJson(re));
        }

        private void Send2HotelGroup(TaskOrderModel item)
        {
            ResponseEntry re = new ResponseEntry();
            re.T = "h";
            re.D = item;
            this._groups.Send("Hotel", GetObjectJson(re));
        }
        private string GetObjectJson(ResponseEntry re)
        {
            JsonSerializerSettings jsseting = new JsonSerializerSettings();
            jsseting.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            jsseting.ContractResolver = new CamelCasePropertyNamesContractResolver();
            return JsonConvert.SerializeObject(re, Formatting.Indented, jsseting);
        }
    }
}