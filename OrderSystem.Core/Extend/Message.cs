using OrderSystem.Core.Extend.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace OrderSystem.Core.Extend
{
    public class Message
    {
        private OrderSystem.Core.Models.OrderDbContext db = new Models.OrderDbContext();
        public string SendMessage(MessageModel mm) {
            mm.SendDate=DateTime.Now;
            StringBuilder sms = new StringBuilder();
            sms.AppendFormat("name={0}", "18389813333");
            sms.AppendFormat("&pwd={0}", "8F84441CF82261ED9A2CC10D3FA0");//登陆平台，管理中心--基本资料--接口密码（28位密文）；复制使用即可。
            sms.AppendFormat("&content={0}", mm.Context);
            sms.AppendFormat("&mobile={0}", mm.Phone);
            sms.AppendFormat("&sign={0}", "欢笑旅业");// 公司的简称或产品的简称都可以
            sms.Append("&type=pt");
            string resp = PushToWeb("http://web.duanxinwang.cc/asmx/smsservice.aspx", sms.ToString(), Encoding.UTF8);
            string[] msg = resp.Split(',');
            if (msg[0] == "0")
            {
                mm.SendSerial = msg[1];
                db.Messages.Add(mm);
                db.SaveChanges();
                return "ok";
            }
            else
            {
                return  "提交失败：错误信息=" + msg[1];
            }
        }

        private string PushToWeb(string weburl, string data, Encoding encode)
        {
            byte[] byteArray = encode.GetBytes(data);

            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(new Uri(weburl));
            webRequest.Method = "POST";
            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.ContentLength = byteArray.Length;
            Stream newStream = webRequest.GetRequestStream();
            newStream.Write(byteArray, 0, byteArray.Length);
            newStream.Close();

            //接收返回信息：
            HttpWebResponse response = (HttpWebResponse)webRequest.GetResponse();
            StreamReader aspx = new StreamReader(response.GetResponseStream(), encode);
            return aspx.ReadToEnd();
        }
        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
        }
    }
}