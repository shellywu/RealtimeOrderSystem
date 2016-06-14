using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OrderSystem.Core.Models
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }
        [StringLength(20),Required]
        public string CName { get; set; }
        [StringLength(13),Required]
        public string CPhone { get; set; }
        [StringLength(30)]
        public string CIdentity { get; set; }
        public CredentialType IdentityType { get; set; }
        public MemberLevel Level { get; set; }
        [StringLength(128)]
        public string CEmail { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}