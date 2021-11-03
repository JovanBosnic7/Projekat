using System;
using System.Web;
using System.Web.Mvc;
using BexMVC.Filters;
using Microsoft.Owin.Security;

namespace BexMVC.Controllers
{
    [ClaimsAuthentication(Resource = "claims", Operation = "Read")]
    public class SecretController : Controller
    {
        public ActionResult Index()
        {
            var claims = AuthenticationManager.User.Claims;

            return View(claims);
        }

        private IAuthenticationManager AuthenticationManager
        {
            get { return HttpContext.GetOwinContext().Authentication; }
        }
    }
}