using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

using OrderSystem.Core.Models;
using System.Text;

using Newtonsoft.Json;
using Newtonsoft.Json.Utilities;
using OrderSystem.Core.Extend.Model;

namespace OrderSystem.Core.Extend
{
    public class Text2AudHelper
    {
        private const string AppKey = "a6eAGvdYvNo2Zd25GCxQvbqW";
        private const string SucrityKey = "f3dd2bb7e7e87f235dd4aa71e48fd607";
        private const string MacAddress = "60A44C7142D3";
        private const string APIAddress = "http://tsn.baidu.com/text2audio";
        private const string OpenIdAddress = "https://openapi.baidu.com/oauth/2.0/token";
        private OrderDbContext db = new OrderDbContext();
        public MemoryStream GetAudio(string msg)
        {
            if (string.IsNullOrEmpty(msg)) {
                throw new Exception("获取语音参数错误，请填写文本");
            }
            var token = GetAccessToken();
            if (string.IsNullOrEmpty(token))
            {
                throw new Exception("获取token参数错误");
            }
            
            StringBuilder sms = new StringBuilder();
            sms.AppendFormat("tex={0}", msg);
            sms.AppendFormat("&lan={0}", "zh");
            sms.AppendFormat("&cuid={0}", MacAddress);
            sms.AppendFormat("&ctp={0}", 1);
            sms.AppendFormat("&tok={0}", token);
            
            byte[] byteArray = Encoding.UTF8.GetBytes(sms.ToString());

            Uri uri = new Uri(APIAddress);
            
            HttpWebRequest req = WebRequest.Create(uri) as HttpWebRequest;
            req.Method = "post";
            req.ContentType = "application/x-www-form-urlencoded";
            req.ContentLength = byteArray.Length;
            Stream newStream = req.GetRequestStream();
            newStream.Write(byteArray, 0, byteArray.Length);
            newStream.Close();

            //接收返回信息：
            HttpWebResponse response = req.GetResponse() as HttpWebResponse;
            if (response.ContentType.ToLower() == "audio/mp3")
            {
                MemoryStream ms = new MemoryStream();
                response.GetResponseStream().CopyTo(ms);
                return ms;
            }
            else
            {
                throw new Exception("合成语音信息错误");
            }
        }

        private string GetAccessToken()
        {
            var accessToken = db.AccessTokens.OrderByDescending(ac => ac.GetDate).FirstOrDefault();
            if (accessToken == null)
            {
                return RefreshToken();
            }
            else
            {
                if (accessToken.GetDate.AddSeconds(accessToken.Expires_in) > DateTime.Now)
                {
                    return accessToken.Access_token;
                }
                else
                {
                    return RefreshToken();
                }
            }
        }

        private string RefreshToken()
        {
            StringBuilder sms = new StringBuilder();
            sms.AppendFormat("grant_type={0}", "client_credentials");
            sms.AppendFormat("&client_id={0}", AppKey);//登陆平台，管理中心--基本资料--接口密码（28位密文）；复制使用即可。
            sms.AppendFormat("&client_secret={0}", SucrityKey);

            byte[] byteArray = Encoding.UTF8.GetBytes(sms.ToString());
            Uri uri = new Uri(OpenIdAddress);
            HttpWebRequest req = WebRequest.Create(uri) as HttpWebRequest;
            req.Method = "post";
            req.ContentType = "application/x-www-form-urlencoded";
            req.ContentLength = byteArray.Length;
            Stream newStream = req.GetRequestStream();
            newStream.Write(byteArray, 0, byteArray.Length);
            newStream.Close();

            //接收返回信息：
            HttpWebResponse response = req.GetResponse() as HttpWebResponse;
            if (response.ContentType.ToLower() == "application/json")
            {
                AccessToken at = new AccessToken();
                StreamReader result = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                at = JsonConvert.DeserializeObject<AccessToken>(result.ReadToEnd());
                at.GetDate = DateTime.Now;
               
                db.AccessTokens.Add(at);
                db.SaveChanges();
                return at.Access_token;
            }
            return string.Empty;
        }
    }
}