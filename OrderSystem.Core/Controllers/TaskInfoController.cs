using Microsoft.AspNet.Identity.EntityFramework;
using OrderSystem.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

using Microsoft.AspNet.Identity;
using OrderSystem.Core.RealTimeConn.Core;

namespace OrderSystem.Core.Controllers
{
    [Authorize]
    public class TaskInfoController : ApiController
    {
        private OrderDbContext odb = new OrderDbContext();
        private ApplicationUserManager _aum = null;
        private ChangeState _changeState = null;
        public TaskInfoController() : this(ChangeState.Instance) { }
        public TaskInfoController(ChangeState cs)
        {
            _changeState = cs;
            _aum = new ApplicationUserManager(new UserStore<ApplicationUser>(odb));
        }


        [HttpGet]
        [ResponseType(typeof(TaskListView))]
        public async Task<IHttpActionResult> GetUnprocessTasks([FromUri]TaskQueryModel tqm)
        {
            try
            {
                TaskListView tlv = new TaskListView();
                tlv.Count = odb.Tasks.Where(t => (t.State == TaskState.Create || t.State == TaskState.CreateCancel) && t.ProductType == tqm.Pt).Count();
                List<TaskOrderModel> tom = new List<TaskOrderModel>();
                var roles = await _aum.GetRolesAsync(User.Identity.GetUserId());
                if (roles.ElementAt(0) == "admin")
                {
                    tom = odb.Tasks.Include("Personinfos").Where(t => (t.State == TaskState.Create || t.State == TaskState.CreateCancel||t.State==TaskState.Change) && t.ProductType == tqm.Pt).OrderByDescending(o => o.CreateDate).Skip(tqm.PageSize * (tqm.RowCount - 1)).Take(tqm.PageSize).ToList();
                }
                else
                {
                    var groupNub = GetProductTypeByRoles(roles);
                    tom = odb.Tasks.Include("Personinfos").Where(t => (t.State == TaskState.Create || t.State == TaskState.CreateCancel||t.State==TaskState.Change) && t.ProductType == tqm.Pt).OrderByDescending(o => o.CreateDate).Skip(tqm.PageSize * (tqm.RowCount - 1)).Take(tqm.PageSize).ToList();
                }
                tlv.Toms = tom;
                return Ok(tlv);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        public async Task<IHttpActionResult> PutUpdateTasks(UpdateTaskModel utm)
        {
            try
            {
                var task = odb.Tasks.Find(utm.Id);


                if (task == null)
                {
                    return NotFound();
                }

                var order = odb.Orders.FirstOrDefault(o => o.OrderCode == task.OrderCode);

                if (order == null)
                {
                    return NotFound();
                }
                
                task.State = utm.State;

                if (task.State == TaskState.Take)
                {
                    task.TakeDate = DateTime.Now;

                    task.TakePerson = User.Identity.GetUserId();

                    if (order.OrderStatus != OrderStatus.Partion && order.OrderStatus != OrderStatus.Processing)
                    {
                        order.OrderStatus = OrderStatus.Processing;
                    }

                    order.Remark += DateTime.Now.ToString("yy-MM-dd hh:mm:ss") + ":处理订单中\r\n";
                }
                if (task.State == TaskState.Finish)
                {
                    var tasks = odb.Tasks.Where(o => o.OrderCode == task.OrderCode).ToList();
                    var d = tasks.Where(t => t.State == TaskState.Create || t.State == TaskState.Take).Count();

                    task.CompleteDate = DateTime.Now;

                    if (d == 0)
                    {
                        if (tasks.FindAll(ta => ta.State == TaskState.OrderErr).Count > 0)
                        {
                            order.OrderStatus = OrderStatus.DoneWithErr;
                        }
                        else
                        {
                            order.OrderStatus = OrderStatus.Done;
                        }
                        order.Remark += "\r\n" + DateTime.Now.ToString("yy-MM-dd hh:mm:ss") + ":完成订单["+utm.Remark+"]";
                    }

                    if (d > 0)
                    {
                        order.OrderStatus = OrderStatus.Partion;
                        order.Remark += "\r\n" + DateTime.Now.ToString("yy-MM-dd hh:mm:ss") + task.ProductName + "出货成功["+utm.Remark+"]";
                    }
                }
                if (task.State == TaskState.OrderErr)
                {
                    var tasks = odb.Tasks.Where(o => o.OrderCode == task.OrderCode).ToList();
                    var d = tasks.Where(t => t.State == TaskState.Create || t.State == TaskState.Take).Count();

                    task.CompleteDate = DateTime.Now;

                    if (d == 0)
                    {
                        order.OrderStatus = OrderStatus.DoneWithErr;
                        order.Remark += "\r\n" + DateTime.Now.ToString("yy-MM-dd hh:mm:ss") + task.ProductName + ":下单失败[" + utm.Remark + "]";
                    }

                    if (d > 0)
                    {
                        order.OrderStatus = OrderStatus.Partion;
                        order.Remark += "\r\n" + DateTime.Now.ToString("yy-MM-dd hh:mm:ss") + task.ProductName + "出货失败[" + utm.Remark + "]";
                    }
                }
                if (task.State == TaskState.TakeCanceling)
                {

                    task.TakeDate = DateTime.Now;

                    task.TakePerson = User.Identity.GetUserId();

                    var tasks = odb.Tasks.Where(o => o.OrderCode == task.OrderCode).ToList();

                    order.OrderStatus = OrderStatus.Canceling;
                    order.Remark += "\r\n" + task.ProductName + "处理取消";
                }

                if (task.State == TaskState.Canceled)
                {
                    var tasks = odb.Tasks.Where(o => o.OrderCode == task.OrderCode).ToList();
                    var d = tasks.Where(t => t.State == TaskState.CreateCancel || t.State == TaskState.TakeCanceling).Count();

                    task.CompleteDate = DateTime.Now;

                    if (d == 0)
                    {
                        if (tasks.FindAll(ta => ta.State == TaskState.CancelFaild).Count > 0)
                        {
                            order.OrderStatus = OrderStatus.CancelFailed;
                        }
                        else
                        {
                            order.OrderStatus = OrderStatus.Canceled;
                        }
                        order.Remark += "\r\n" + DateTime.Now.ToString("yy-MM-dd hh:mm:ss") + ":退单完成["+utm.Remark+"]";
                    }

                    if (d > 0)
                    {
                        order.OrderStatus = OrderStatus.Canceling;
                        order.Remark +=  "\r\n" + DateTime.Now.ToString("yy-MM-dd hh:mm:ss") + task.ProductName + "退货完成["+utm.Remark+"]";
                    }
                }
                if (task.State == TaskState.CancelFaild)
                {
                    var tasks = odb.Tasks.Where(o => o.OrderCode == task.OrderCode).ToList();
                    var d = tasks.Where(t => t.State == TaskState.CreateCancel || t.State == TaskState.TakeCanceling).Count();

                    task.CompleteDate = DateTime.Now;

                    if (d == 0)
                    {
                        order.OrderStatus = OrderStatus.CancelFailed;
                        order.Remark += "\r\n" + DateTime.Now.ToString("yy-MM-dd hh:mm:ss") + task.ProductName + ":退单失败[" + utm.Remark + "]";
                    }

                    if (d > 0)
                    {
                        order.OrderStatus = OrderStatus.Canceling;
                        order.Remark +=  "\r\n" + DateTime.Now.ToString("yy-MM-dd hh:mm:ss") + task.ProductName + "退单失败["+utm.Remark+"]";
                    }
                }

                task.TaskRemark = utm.Remark;

                await odb.SaveChangesAsync();

                OrderUpdateModel oum = new OrderUpdateModel();
                oum.Id = order.Id;
                oum.State = order.OrderStatus;
                oum.Remark = order.Remark;
                _changeState.SendUpdateTaskState(task);
                _changeState.SendUpdateOrderState(oum);
                return Ok();
            }
            catch (Exception)
            {

                return InternalServerError();
            }
        }

        private List<ProductType> GetProductTypeByRoles(IList<string> roles)
        {
            List<ProductType> type = new List<ProductType>();
            foreach (var item in roles)
            {
                type.Add(GetTypeByRole(item));
            }
            return type;
        }

        private ProductType GetTypeByRole(string item)
        {
            ProductType typeId = ProductType.Travel;
            switch (item.ToLower())
            {
                case "tax": typeId = ProductType.Tax; break;
                case "hotel": typeId = ProductType.Hotel; break;
                case "tiket": typeId = ProductType.Titcket; break;
            }
            return typeId;
        }
    }

    public class TaskQueryModel
    {
        public int RowCount { get; set; }
        public int PageSize { get; set; }
        public ProductType Pt { get; set; }
    }

    public class UpdateTaskModel
    {
        public int Id { get; set; }
        public TaskState State { get; set; }
        public string Remark { get; set; }
    }

    public class TaskListView
    {
        public int Count { get; set; }
        public List<TaskOrderModel> Toms { get; set; }
        public TaskListView()
        {
            Toms = new List<TaskOrderModel>();
        }
    }
}
