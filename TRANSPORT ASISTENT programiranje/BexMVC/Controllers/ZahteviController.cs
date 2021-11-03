using AspNet.DAL.EF.Models.Security;
using AspNet.DAL.EF.UOW.Security;
using Bex.Common;
using Bex.Common.Interfaces;
using Bex.DAL.EF.UOW;
using Bex.Models;
using Bex.MVC.Exceptions;
using BexMVC.Filters;
using BexMVC.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Filters;

namespace BexMVC.Controllers
{
    [Authorize]
    [ClaimsAuthentication(Resource = "Zahtevi", Operation = "Read, All")]
    public class ZahteviController : Controller
    {

        public ZahteviController() : this(new BexUow(), new SecurityUow(System.Web.HttpContext.Current.GetOwinContext()), new ExceptionSolver())
        { }
        public ZahteviController(
            IBexUow bexUow,
            ISecurityUow securityUow,
            IExceptionSolver exceptionSolver)
        {
            BexUow = bexUow;
            SecurityUow = securityUow; 
            ExceptionSolver = exceptionSolver;
        }
        public ActionResult Index()
        {
                      
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> GetEvents()
        {
            var applicationUser = await SecurityUow.UserManager.FindUserByIdAsync(User.Identity.GetUserId()) as ApplicationUser;
            var bexUser = BexUow.KorisniciPrograma.Find(x => x.AspNetUserId == applicationUser.Id);
            var events = BexUow.KalendarPlaner.AllAsNoTracking.Where(x => x.UserId == bexUser.Id).ToList();

            return new JsonResult { Data = events, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            
        }

        [HttpGet]
        public async Task<JsonResult> SetEvents(string title, string start, string end)
        {
            var applicationUser = await SecurityUow.UserManager.FindUserByIdAsync(User.Identity.GetUserId()) as ApplicationUser;
            var bexUser = BexUow.KorisniciPrograma.Find(x => x.AspNetUserId == applicationUser.Id);
            var events = BexUow.KalendarPlaner.AllAsNoTracking.ToList();

            var planer = new KalendarPlaner
            {
                Naziv = title,
                Opis = "",
                DatumStart = DateTime.Parse(start),
                DatumEnd = DateTime.Parse(end),
                UserId = bexUser.Id
            };

            BexUow.KalendarPlaner.Add(planer);
            var commandResult = BexUow.SubmitChanges();
            if (commandResult.IsSuccessful)
            {
                return new JsonResult { Data = new { success = "false" }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            else
            {
                return new JsonResult { Data = new { success = "false" }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }



        }



        private IExceptionSolver ExceptionSolver { get; }
        private IBexUow BexUow { get; }

        private ISecurityUow SecurityUow { get; set; }

       

    }
}