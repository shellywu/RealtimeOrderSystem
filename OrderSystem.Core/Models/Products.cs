using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OrderSystem.Core.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [StringLength(128)]
        public string ProductName { get; set; }
        public ProductType ProductType { get; set; }
        [StringLength(256)]
        public string City { get; set; }
        [StringLength(128)]
        public string Spot { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime ValidStartDate { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime ValidEndDate { get; set; }
        [StringLength(20)]
        [Required]
        public string ProductCode { get; set; }
        public int NeedFeedBack { get; set; }
        /// <summary>
        /// 产品价格
        /// </summary>
        public float Price { get; set; }
        /// <summary>
        /// 产品返点
        /// </summary>
        public float Commision { get; set; }
        /// <summary>
        /// 佣金
        /// </summary>
        public float Borkerage { get; set; }
        /// <summary>
        /// 产品结算价格
        /// </summary>
        public float Settlement { get; set; }
        /// <summary>
        /// 产品合同价格
        /// </summary>
        public float ContractPrice { get; set; }
        public float Cons { get; set; }

        public int PartnerId { get; set; }
        [ForeignKey("PartnerId")]
        public virtual Partner Partner { get; set; }

        public int SupplierId { get; set; }
        [ForeignKey("SupplierId")]
        public virtual Supplier Supplier { get; set; }

        /// <summary>
        /// 可能属于
        /// </summary>
        public virtual ICollection<ComboProductItem> ComboItems { get; set; }
    }
}