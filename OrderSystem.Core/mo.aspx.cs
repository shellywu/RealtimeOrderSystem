using OrderSystem.Core.Extend.Model;
using OrderSystem.Core.RealTimeConn.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OrderSystem.Core
{
    public partial class mo : System.Web.UI.Page
    {
        private OrderSystem.Core.Models.OrderDbContext db = new Models.OrderDbContext();
        private MessageReceive _changeState = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            _changeState = MessageReceive.Instance;
            string name = Request.Form["name"];  //在系统中配置的接收用户名
            string pwd = Request.Form["pwd"];   //在系统中配置的接收密码
            string args = Request.Form["args"]; //上行数据，根据文档4.1格式解析

            if (args.Contains("#@@#"))//多条上行一起推送过来的
            {
                MessageModel mm = new MessageModel();
                string[] allmo = args.Split(new string[] { "#@@#" }, StringSplitOptions.RemoveEmptyEntries);//拆分成一条一条的信息，放到数组中
                for (int i = 0; i < allmo.Length; i++)
                {
                    string[] mo = allmo[i].Split(new string[] { "#@#" }, StringSplitOptions.None);//这个地方要用None，空值不能移除
                    mm.Phone = mo[0];
                    mm.ReplayContext = mo[1];
                    mm.ReplayDate = DateTime.Parse(mo[2]);
                    //mo[3]  系统扩展码+发送时带的extno值  一般情况下账号的特服号即为系统扩展码。 如账号的特服号是 1001，发送时带的extno=888， mo[3]=1001888

                    db.Messages.Add(mm);
                    db.SaveChanges();
                    _changeState.BoardMessage(mm.Phone,mm.ReplayContext);
                }
            }
            else//只有一条上行信息
            {
                MessageModel mm = new MessageModel();
                string[] mo = args.Split(new string[] { "#@#" }, StringSplitOptions.None);//这个地方要用None，空值不能移除
                mm.Phone = mo[0];
                mm.ReplayContext = mo[1];
                mm.ReplayDate = DateTime.Parse(mo[2]);
                //mo[3]  系统扩展码+发送时带的extno值  一般情况下账号的特服号即为系统扩展码。 如账号的特服号是 1001，发送时带的extno=888， mo[3]=1001888

                db.Messages.Add(mm);
                db.SaveChanges();
                _changeState.BoardMessage(mm.Phone, mm.ReplayContext);
            }
        }
    }
}