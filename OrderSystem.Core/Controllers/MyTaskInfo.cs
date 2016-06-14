using Microsoft.AspNet.Identity.EntityFramework;
using OrderSystem.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

using Microsoft.AspNet.Identity;

namespace OrderSystem.Core.Controllers
{
    public class MyTaskInfoController : ApiController
    {
        private OrderDbContext odb = new OrderDbContext();
        private ApplicationUserManager _aum = null;
        public MyTaskInfoController()
        {
            _aum = new ApplicationUserManager(new UserStore<ApplicationUser>(odb));
        }

        [HttpGet]
        [ResponseType(typeof(TaskListView))]
        // GET api/<controller>
        public async Task<IHttpActionResult> Get([FromUri]TaskQueryModel tqm)
        {
            try
            {
                TaskListView tlv = new TaskListView();
                tlv.Count = odb.Tasks.Where(t => (t.State == TaskState.Create || t.State == TaskState.Take || t.State == TaskState.TakeCanceling) && t.ProductType == tqm.Pt).Count();
                List<TaskOrderModel> tom = new List<TaskOrderModel>();
                var userid = User.Identity.GetUserId();
                var roles =await _aum.GetRolesAsync(userid);
                if (roles.ElementAt(0) == "admin")
                {
                    tom = odb.Tasks.Include("Personinfos").Where(t => t.TakePerson == userid.ToString()&&(t.State != TaskState.Create && t.State != TaskState.CreateCancel&&t.State!=TaskState.Change) && t.ProductType == tqm.Pt).OrderByDescending(o => o.CreateDate).Skip(tqm.PageSize * (tqm.RowCount - 1)).Take(tqm.PageSize).ToList();
                }
                else
                {
                    tom = odb.Tasks.Include("Personinfos").Where(t => t.TakePerson == userid.ToString() && (t.State != TaskState.Create && t.State != TaskState.CreateCancel && t.State != TaskState.Change) && t.ProductType == tqm.Pt).OrderByDescending(o => o.CreateDate).Skip(tqm.PageSize * (tqm.RowCount - 1)).Take(tqm.PageSize).ToList();
                }
                tlv.Toms = tom;
                return Ok(tlv);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            } 
        }
    }
}