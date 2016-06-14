using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrderSystem.Core.Models
{
    public class ProductQueryModel
    {
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
    }

    public class OrderDetailsQuery
    {
        public string OrderCode { get; set; }
        public string CertCode { get; set; }
        public string PhoneCode { get; set; }
        public DateTime OrderStartDate { get; set; }
        public DateTime OrderEndDate { get; set; }

    }
}