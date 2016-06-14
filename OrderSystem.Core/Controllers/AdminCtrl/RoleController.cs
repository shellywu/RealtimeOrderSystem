using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using OrderSystem.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OrderSystem.Core.Controllers.AdminCtrl
{
    public class RoleController : Controller
    {
        private RoleManager<IdentityRole> _rm = null;
        OrderDbContext db = new OrderDbContext();
        public RoleController()
        {
            _rm = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
        }
        // GET: Role
        public ActionResult Index()
        {

            var RoleManager = _rm.Roles;
            return View(RoleManager.ToList());
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create([Bind(Include = "RoleName")] RoleView rv)
        {
            if (ModelState.IsValid)
            {
                if (!_rm.RoleExists(rv.RoleName))
                {
                    _rm.Create(new IdentityRole(rv.RoleName));
                }
                return RedirectToAction("Index");
            }
            return View();
        }
    }

    public class RoleView
    {
        public Guid RoleId { get; set; }
        public string RoleName { get; set; }
    }
}