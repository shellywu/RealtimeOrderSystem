using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OrderSystem.Core.Models
{
    public class ComboProductItem
    {
        [Key, Column(Order = 1)]
        public int ComboProductId { get; set; }
        [ForeignKey("ComboProductId")]
        public ComboProduct ComboProduct { get; set; }
        [Key, Column(Order = 2)]
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Product Product { get; set; }
        public int Quantity { get; set; }
        /// <summary>
        /// 产品单价
        /// </summary>
        public float ProductPrice { get; set; }

        /// <summary>
        /// 产品单价
        /// </summary>
        public float ProductCommision { get; set; }
        /// <summary>
        /// 单件产品佣金
        /// </summary>
        public float Borkerage { get; set; }

        /// <summary>
        /// 产品单结算价
        /// </summary>
        public float ProductSettlement { get; set; }
        /// <summary>
        /// 产品单同价
        /// </summary>
        public float ProductContractPrice { get; set; }
    }
}