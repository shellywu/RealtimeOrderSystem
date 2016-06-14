using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrderSystem.Core.Models
{
    public class Supplier
    {
        public int Id { get; set; }
        /// <summary>
        /// 供应商名称
        /// </summary>
        public string SupplierName { get; set; }
        /// <summary>
        /// 联系人
        /// </summary>
        public string Contract { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 座机
        /// </summary>
        public string Call { get; set; }
        /// <summary>
        /// 传真
        /// </summary>
        public string FixNumber { get; set; }
        /// <summary>
        /// 备注信息
        /// </summary>
        public string Remark { get; set; }

    }
}