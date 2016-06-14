using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OrderSystem.Core.Models
{
    public class Employee
    {
        [StringLength(4)]
        public string FirstName { get; set; }
        [StringLength(8)]
        public string LastName { get; set; }

        public string Name { get {
            return FirstName + LastName;
        } }

        public string Phone { get; set; }

        [StringLength(30)]
        public string AccountNumber { get; set; }
        public int IsActive{ get; set; }

        public DateTime CreateDate{ get; set; }

        public DateTime ModifyDate { get; set; }
        [StringLength(128)]
        public string CreateBy { get; set; }
    }
}