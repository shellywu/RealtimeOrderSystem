using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrderSystem.Core.Models
{
    public class OrderListItemView
    {
        public Guid Id { get; set; }
        public string OrderCode { get; set; }
        public OrderStatus OrderStatus { get; set; }

        public string CustomerName { get; set; }

        public string Contract { get; set; }

        public List<string> ProductNames { get; set; }
        public DateTime OrderDate { get; set; }
        public OrderListItemView()
        {
            ProductNames = new List<string>();
        }

        public string Remark { get; set; }
    }
}