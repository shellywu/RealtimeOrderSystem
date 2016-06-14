using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrderSystem.Core.Models
{
    public class OrderListQuery
    {
        public int RowCount { get; set; }
        public int PageSize { get; set; }
        public Guid UserId { get; set; }

    }
}