using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OrderSystem.Core.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;

namespace OrderSystem.Core.Controllers.AdminCtrl
{
    [Authorize(Roles = "admin")]
    public class EmployeeController : Controller
    {
        private OrderDbContext db = new OrderDbContext();
        private ApplicationUserManager _aum = null;

        public EmployeeController()
        {
            _aum = new ApplicationUserManager(new UserStore<ApplicationUser>(db));
        }

        // GET: Employee
        public async Task<ActionResult> Index()
        {
            return View(await db.Users.ToListAsync());
        }
        [HttpGet]
        public ActionResult Create()
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
            ViewBag.roles = new SelectList(roleManager.Roles, "Name", "Name");
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(RegisterViewModel user)
        {
            if (ModelState.IsValid)
            {
                var au = new ApplicationUser();
                au.Email = user.Email;
                au.UserName = user.Email;
                au.PhoneNumber = user.Phone;
                Employee eee = new Employee();
                eee.AccountNumber = user.AccountNumber;
                eee.FirstName = user.FirstName;
                eee.LastName = user.LastName;
                eee.CreateDate = DateTime.Now;
                eee.ModifyDate = DateTime.Now;
                eee.CreateBy = User.Identity.GetUserId();
                au.EmployeeInfo = eee;

                var result = await _aum.CreateAsync(au, "123!qwe");
                if (result.Succeeded)
                {
                    var roleResult = await _aum.AddToRoleAsync(au.Id, user.Role);
                    if (roleResult.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        return RedirectToAction("Create");
                    }
                }
            }
            return RedirectToAction("Create");
        }
        [HttpGet]
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user =await _aum.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = _aum.FindById(id.ToString());
            var result = await _aum.DeleteAsync(user);
            return RedirectToAction("Index");
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
