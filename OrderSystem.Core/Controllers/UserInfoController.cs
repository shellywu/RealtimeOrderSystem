using Microsoft.AspNet.Identity.EntityFramework;
using OrderSystem.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using Microsoft.AspNet.Identity;
using System.Threading.Tasks;

namespace OrderSystem.Core.Controllers
{
    [Authorize]
    public class UserInfoController : ApiController
    {
        private OrderDbContext db = new OrderDbContext();
        private ApplicationUserManager _userManager;
        public UserInfoController()
        {
            _userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(db));
        }

        [HttpGet]
        public async Task<List<string>> GetUserRole() {
            var id = User.Identity.GetUserId();
            var role = await _userManager.GetRolesAsync(id);
            return role.ToList<string>();
        }
    }
}
