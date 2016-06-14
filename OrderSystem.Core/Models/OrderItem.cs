using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OrderSystem.Core.Models
{
    public class OrderItem
    {
        [Key]
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }

        [ForeignKey("OrderId")]
        public virtual Order Order { get; set; }
        /// <summary>
        /// 产品使用人信息
        /// </summary>
        public ICollection<PersonInfo> Persons { get; set; }
        public string ProductId { get; set; }
        /// <summary>
        /// 是产品还是套餐
        /// </summary>
        public ProductType ProductType { get; set; }
        [DataType(DataType.DateTime)]
        /// <summary>
        /// 预计使用时间
        /// </summary>
        public DateTime StartDate { get; set; }
        [DataType(DataType.DateTime)]
        /// <summary>
        /// 预计结束使用时间
        /// </summary>
        public DateTime EndDate { get; set; }
        [StringLength(256)]
        /// <summary>
        /// 订单项内容描述 类型/产品名称/数量
        /// </summary>
        public string Describe { get; set; }

        public int Quantity { get; set; }

        public float TotalPrice { get; set; }
        /// <summary>
        /// 券号验证时间
        /// </summary>
        public DateTime? CertificateDate { get; set; }
        /// <summary>
        /// 券号
        /// </summary>
        public string CertificateNum { get; set; }
        public string Remark { get; set; }
        [DefaultValue(0)]
        /// <summary>
        /// 用于标示订单项的状态，例如是否属于修改订单
        /// </summary>
        public int State { get; set; }
        [DefaultValue(0)]
        /// <summary>
        /// 用于促销调价
        /// </summary>
        public decimal CustomerPrice { get; set; }

    }
}