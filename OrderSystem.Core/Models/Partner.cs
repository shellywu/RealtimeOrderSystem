using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OrderSystem.Core.Models
{
    public class Partner
    {
        [Key]
        public int Id { get; set; }
        [StringLength(20)]
        public string Name { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime BeginDate { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime EndDate { get; set; }
        [StringLength(20)]
        /// <summary>
        /// 合作伙伴联系人
        /// </summary>
        public string PartnerContract { get; set; }
        [StringLength(20)]
        public string ParnerPhone { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}