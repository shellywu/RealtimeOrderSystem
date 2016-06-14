using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using OrderSystem.Core.Models;

namespace OrderSystem.Core.RealTimeConn.Core
{
    public class OrderUpdateModel
    {
        public Guid Id { get; set; }
        public OrderStatus State { get; set; }
        public string Remark { get; set; }
    }
}