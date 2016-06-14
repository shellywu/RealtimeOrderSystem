using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using OrderSystem.Core.Models;

namespace OrderSystem.Core.Controllers.AdminCtrl.QueryModel
{
    public class OrderQueryModel
    {
        /// <summary>
        /// 订单编码
        /// </summary>
        public string OrderCode { get; set; }
        private DateTime orderStartDate;
        private DateTime orderEndDate;
        public DateTime OrderStartDate
        {
            get
            {
                return this.orderStartDate.Date;
            }
            set
            {
                this.orderStartDate = value;
            }
        }
        public DateTime OrderEndDate
        {
            get
            {
                if (this.orderEndDate == DateTime.MinValue) {
                    return this.orderEndDate;
                }
                return this.orderEndDate.Date.AddHours(23).AddMinutes(59);
            }
            set {
                this.orderEndDate = value;
            }
        }

        /// <summary>
        /// 订单状态
        /// </summary>
        public OrderStatus Status { get; set; }
        /// <summary>
        /// 客户手机号码
        /// </summary>
        public string CPhone { get; set; }

        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}