using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrderSystem.Core.Models
{
    public class CanelModel
    {
        public Guid Id { get; set; }
        public string OrderCode { get; set; }
        public DateTime OrderDate { get; set; }
        public string CName { get; set; }
        public string CPhone { get; set; }

        public OrderStatus OrderStatus { get; set; }
    }

    public class OrderDetail
    {
        public Guid Id { get; set; }
        public List<OrderItem> OrderItems { get; set; }
        public List<TaskOrderModel> Tasks { get; set; }

    }

    public class ChagneOrderDetial
    {
        public Guid Id { get; set; }
        public List<OrderItem> OrderItems { get; set; }
    }

}