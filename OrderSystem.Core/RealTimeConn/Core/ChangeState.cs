using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using OrderSystem.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrderSystem.Core.RealTimeConn.Core
{
    public class ChangeState
    {
        private readonly static Lazy<ChangeState> _instance = new Lazy<ChangeState>(() => new ChangeState(GlobalHost.ConnectionManager.GetConnectionContext<OrderConnection>()));

        private IPersistentConnectionContext Context { get; set; }
        private ChangeState(IPersistentConnectionContext pcc)
        {
            Context = pcc;
        }
        public static ChangeState Instance
        {
            get
            {
                return _instance.Value;
            }
        }

        public IConnectionGroupManager Group
        {
            get
            {
                return this.Context.Groups;
            }
        }

        public void SendUpdateTaskState(TaskOrderModel tom, bool isCreate = false)
        {
            JsonSerializerSettings jsseting = new JsonSerializerSettings();
            jsseting.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            jsseting.ContractResolver = new CamelCasePropertyNamesContractResolver();
            ResponseEntry re = new ResponseEntry();
            if (isCreate)
            {
                switch (tom.ProductType)
                {
                    case ProductType.Hotel: re.T = "h"; break;
                    case ProductType.Tax: re.T = "t"; break;
                    case ProductType.Titcket: re.T = "k"; break;
                }
            }
            else
            {
                re.T = "u";
            }
            re.D = tom;
            Context.Connection.Broadcast(JsonConvert.SerializeObject(re, Formatting.Indented, jsseting));
        }

        public void SendUpdateOrderState(OrderUpdateModel oum)
        {
            JsonSerializerSettings jsseting = new JsonSerializerSettings();
            jsseting.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            jsseting.ContractResolver = new CamelCasePropertyNamesContractResolver();
            ResponseEntry re = new ResponseEntry();
            re.T = "o";
            re.D = oum;
            Context.Connection.Broadcast(JsonConvert.SerializeObject(re, Formatting.Indented, jsseting));
        }
    }
}