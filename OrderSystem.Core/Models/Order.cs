using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OrderSystem.Core.Models
{
    public class Order
    {
        [Key]
        public Guid Id { get; set; }
        [StringLength(20)]
        /// <summary>
        /// 订单编号
        /// </summary>
        public string OrderCode { get; set; }

        public virtual ICollection<OrderItem> OrderItems { get; set; }
        
        /// <summary>
        /// 下单日期
        /// </summary>
        [DataType(DataType.DateTime)]
        public DateTime OrderDate { get; set; }

        /// <summary>
        /// 订单完成日期
        /// </summary>
        public DateTime ComplteDate { get; set; }

        public int CustomerId { get; set; }

        [ForeignKey("CustomerId")]
        /// <summary>
        /// 订单联系人
        /// </summary>
        public Customer Customer { get; set; }
    
        /// <summary>
        /// 订单创建人
        /// </summary>
        public virtual ApplicationUser CreateUser { get; set; }

        /// <summary>
        /// 订单状态
        /// </summary>
        public OrderStatus OrderStatus { get; set; }
        /// <summary>
        /// 订单备注信息
        /// </summary>
        public string Remark { get; set; }
    }
}