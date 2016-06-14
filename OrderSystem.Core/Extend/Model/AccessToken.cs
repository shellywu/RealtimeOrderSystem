using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrderSystem.Core.Extend.Model
{
    public class AccessToken
    {
        public int Id { get; set; }
        public string Access_token { get; set; }
        /// <summary>
        /// 有效期（秒）
        /// </summary>
        public double Expires_in { get; set; }
        public string Refresh_token { get; set; }
        public string Session_key { get; set; }
        public string Session_secret { get; set; }
        /// <summary>
        /// 获取token时间
        /// </summary>
        public DateTime GetDate { get; set; }
    }
}