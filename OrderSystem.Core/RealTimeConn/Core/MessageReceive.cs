using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrderSystem.Core.RealTimeConn.Core
{
    public class MessageReceive
    {
         private readonly static Lazy<MessageReceive> _instance = new Lazy<MessageReceive>(() => new MessageReceive(GlobalHost.ConnectionManager.GetHubContext<ChatHub>()));

        private IHubContext Context { get; set; }
        private MessageReceive(IHubContext pcc)
        {
            Context = pcc;
        }
        public static MessageReceive Instance
        {
            get
            {
                return _instance.Value;
            }
        }

        public void BoardMessage(string phone,string msg) {
            var bmsg = "{\"phone\":\"" + phone + "\",\"context\":\"" + msg + "\"}";
            Context.Clients.All.reciveMsg(bmsg);
        }
    }
}