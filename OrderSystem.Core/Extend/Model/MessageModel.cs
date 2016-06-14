using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrderSystem.Core.Extend.Model
{
    public class MessageModel
    {
        public int Id { get; set; }
        public string Phone { get; set; }
        public string Context { get; set; }
        public DateTime? SendDate { get; set; }
        public string SendSerial { get; set; }

        public string ReplayContext { get; set; }
        public DateTime? ReplayDate { get; set; }
    }
}