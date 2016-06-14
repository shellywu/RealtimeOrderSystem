using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using OrderSystem.Core.Extend;
using OrderSystem.Core.Extend.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrderSystem.Core.RealTimeConn.Core
{
    [HubName("chatHub")]
    public class ChatHub : Hub
    {
        public void Send(MessageModel msg)
        {
            Message m = new Message();
            var rm = m.SendMessage(msg);
            Clients.All.reciveMsg(rm);
        }
    }
}