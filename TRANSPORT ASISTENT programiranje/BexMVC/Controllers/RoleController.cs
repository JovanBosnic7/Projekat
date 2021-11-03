using AspNet.DAL.EF.UOW.Security;
using Bex.Common.Interfaces;
using BexMVC.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;


namespace BexMVC.Controllers
{
    [Authorize(Roles = "Admins")]
    public class RoleController : Controller
    {
        public RoleController() :
            this(new SecurityUow(System.Web.HttpContext.Current.GetOwinContext()))
        { }
        public RoleController(ISecurityUow securityUow)
        {
            SecurityUow = securityUow;
        }

        public async Task<ActionResult> Index()
        {
            var roles = await SecurityUow.RoleManager.GetRolesAsync();

            var model = new List<RoleViewModel>();
            foreach (var role in roles)
            { model.Add(new RoleViewModel { Id = role.Id, Name = role.Name }); }

            return View(model);
        }

        private ISecurityUow SecurityUow { get; set; }
    }
}