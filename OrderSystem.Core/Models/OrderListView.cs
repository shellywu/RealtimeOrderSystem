using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrderSystem.Core.Models
{
    public class OrderListView
    {
        public int Count { get; set; }
        public List<OrderListItemView> OrderListViewItems { get; set; }
    }
}