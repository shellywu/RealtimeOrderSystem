using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrderSystem.Core.Models
{
    public class OrderItemModel
    {
        /// <summary>
        /// 预定日期
        /// </summary>
        public string CreateDate{ get; set; }
        /// <summary>
        /// 现在状态
        /// </summary>
        public string State { get; set; } 
       /// <summary>
       /// 景点
       /// </summary>
        public string Spot { get; set; }
        /// <summary>
        /// 使用日期
        /// </summary>
        public string UsedDate { get; set; }
        /// <summary>
        /// 使用结束日期
        /// </summary>
        public string UsedEndDate { get; set; }
        /// <summary>
        /// 客户名称
        /// </summary>
        public string CustomrName { get; set; }
        /// <summary>
        /// 客户电话
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 身份 
        /// </summary>
        public string Identity { get; set; }
        /// <summary>
        /// 渠道
        /// </summary>
        public string PartnerName { get; set; }
        /// <summary>
        /// 验证码
        /// </summary>
        public string CertificateNumber { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public int Quantity { get; set; }
        /// <summary>
        /// 校验日期
        /// </summary>
        public string CheckDate { get; set; }
        /// <summary>
        /// 售价
        /// </summary>
        public double Price { get; set; }
        /// <summary>
        /// 佣金
        /// </summary>
        public double Commision { get; set; }
        /// <summary>
        /// 应收
        /// </summary>
        public double Receivables { get; set; }
        /// <summary>
        /// 应付
        /// </summary>
        public double Pay { get; set; }
        /// <summary>
        /// 成本
        /// </summary>
        public double Cost { get; set; }
        /// <summary>
        /// 供应商
        /// </summary>
        public string Supplier { get; set; }
        /// <summary>
        /// 损益
        /// </summary>
        public double PayLost { get; set; }
        /// <summary>
        /// 收益
        /// </summary>
        public double Incomde { get; set; }

        public string Remark { get; set; }
    }
}