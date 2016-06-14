using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Microsoft.AspNet.SignalR;
using OrderSystem.Core.Models;
using Newtonsoft.Json;
using System.IO;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Newtonsoft.Json.Serialization;
using System.Threading.Tasks;

namespace OrderSystem.Core.RealTimeConn.Core
{
    public class OrderConnection : PersistentConnection, IDisposable
    {
        private OrderDbContext db = new OrderDbContext();
        private ApplicationUserManager _aum = null;
        private readonly ChangeState _changeState = null;
        public OrderConnection() : this(ChangeState.Instance) { }
        public OrderConnection(ChangeState cs)
        {
            _changeState = cs;
            _aum = new ApplicationUserManager(new UserStore<ApplicationUser>(db));
        }
        protected override bool AuthorizeRequest(IRequest request)
        {
            return request.User != null && request.User.Identity.IsAuthenticated;
        }
        protected override Task OnConnected(IRequest request, string connectionId)
        {
            var id = request.User.Identity.GetUserId();
            var tempRoles = _aum.GetRoles(id);
            foreach (var item in tempRoles)
            {
                if (item.ToLower() == "admin")
                {
                    Groups.Add(connectionId, "Tiket");
                    Groups.Add(connectionId, "Tax");
                    Groups.Add(connectionId, "Hotel");
                    Groups.Add(connectionId, "Service");
                }
                else
                {
                    Groups.Add(connectionId, item);
                }

            }

            return base.OnConnected(request, connectionId);
        }
        protected override Task OnReceived(IRequest request, string connectionId, string data)
        {
            string err = string.Empty;
            Order order = JsonConvert.DeserializeObject<Order>(data);
            TaskAdapter ta = new TaskAdapter();
            List<TaskOrderModel> taskOrders = new List<TaskOrderModel>();

            try
            {
                taskOrders = ta.Order2Task(order,ref err);
                if (string.IsNullOrEmpty(err))
                {
                    ta.SendTask2Group(taskOrders, Groups);
                    ResponseEntry re = new ResponseEntry();
                    re.T = "s";
                    re.D = data;
                    return Groups.Send("Service", GetObjectJson(re));
                }
                else
                {
                    throw new Exception(err);
                }
            }
            catch (Exception ex)
            {
                return Connection.Send(connectionId,ex.Message);
            }
        }

        private string GetObjectJson(ResponseEntry re)
        {
            JsonSerializerSettings jsseting = new JsonSerializerSettings();
            jsseting.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            jsseting.ContractResolver = new CamelCasePropertyNamesContractResolver();
            return JsonConvert.SerializeObject(re, Formatting.Indented, jsseting);
        }

        public void Dispose()
        {
            db.Dispose();
        }
    }

    public class ResponseEntry
    {
        public string T { get; set; }
        public object D { get; set; }

    }
}