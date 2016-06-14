using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OrderSystem.Core.Models
{
    public class TaskOrderModel
    {
        [Key]
        public int Id { get; set; }
        [StringLength(64)]
        public string TaskCode { get; set; }

        [StringLength(128)]
        public string ProductName { get; set; }
        [StringLength(32)]
        public string ProductCode { get; set; }

        public ProductType ProductType { get; set; }

        public int Quantity { get; set; }

        public DateTime UsedStartDate { get; set; }
        public DateTime UsedEndDate { get; set; }

        public Guid OrderItemId { get; set; }

        public ICollection<PersonInfo> Personinfos { get; set; }

        [StringLength(10)]
        /// <summary>
        /// 订单来源区分，套餐or单产品
        /// </summary>
        public string ProductFrom { get; set; }
        [StringLength(20)]
        public string OrderCode { get; set; }
        [StringLength(1024)]
        public string Remark { get; set; }

        public DateTime CreateDate { get; set; }
        public DateTime CompleteDate { get; set; }
        /// <summary>
        /// 领取时间
        /// </summary>
        public DateTime TakeDate { get; set; }
        [StringLength(128)]
        /// <summary>
        /// 领取人
        /// </summary>
        public string TakePerson { get; set; }

        public TaskState State { get; set; }
        [StringLength(1024)]
        public string TaskRemark { get; set; }

        public TaskOrderModel()
        {
            CreateDate = DateTime.Now;
            CompleteDate = DateTime.Now;
            TakeDate = DateTime.Now;
            Personinfos = new List<PersonInfo>();
        }

    }
}