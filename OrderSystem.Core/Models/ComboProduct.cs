using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OrderSystem.Core.Models
{
    public class ComboProduct
    {
        
        [Key]
        public int Id { get; set; }
        [StringLength(20)]
        public string ComboCode { get; set; }
        [StringLength(256)]
        public string ComboName { get; set; }
        public virtual ICollection<ComboProductItem> ComboItems { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime ValidStartDate { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime ValidEndDate { get; set; }

        public float Price { get; set; }
        /// <summary>
        /// 组合产品价格，多个数量合并后价格
        /// </summary>
        public float ProductPrice { get; set; }
        /// <summary>
        /// 租车产品返点，多个数量合并后返点
        /// </summary>
        public float ProductCommision { get; set; }
        public float ProductSettlement { get; set; }
        public float ProductContractPrice { get; set; }
      
    }
}