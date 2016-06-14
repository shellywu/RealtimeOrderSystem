using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OrderSystem.Core.Models
{
    public class PersonInfo
    {
        [Key]
        public int Id { get; set; }
        [StringLength(20), Required]
        public string PersonName { get; set; }
        [StringLength(20),Required]
        public string PersonPhone { get; set; }
        [StringLength(30), Required]
        public string PersonIdentity { get; set; }
        [Required]
        /// <summary>
        /// 证件类型
        /// </summary>
        public CredentialType CredentialType { get; set; }

        public Guid OrderItemId { get; set; }

        [ForeignKey("OrderItemId")]
        public OrderItem Orderitem { get; set; }

        public ICollection<TaskOrderModel> Tasks { get; set; }

        /// <summary>
        /// 重要信息备注
        /// </summary>
        public string Remark { get; set; }
    }
}