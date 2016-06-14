using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrderSystem.Core.Models
{
    public class TaskViewModel
    {
        public string TaskCode { get; set; }

        public string ProductName { get; set; }
        public string ProductCode { get; set; }

        public ProductType ProductType { get; set; }

        public int Quantity { get; set; }

        /// <summary>
        /// 订单来源区分，套餐or单产品
        /// </summary>
        public string ProductFrom { get; set; }
        public string OrderCode { get; set; }
        public string Remark { get; set; }

        public DateTime CreateDate { get; set; }
        public DateTime CompleteDate { get; set; }
        /// <summary>
        /// 领取时间
        /// </summary>
        public DateTime TakeDate { get; set; }
        /// <summary>
        /// 领取人
        /// </summary>
        public string TakePerson { get; set; }

        public TaskState State { get; set; }
        public string TaskRemark { get; set; }
    }
}