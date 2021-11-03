using System.Linq;
using System.Web.Mvc;
using Bex.Models;
using Bex.MVC.Exceptions;
using Bex.DAL.EF.UOW;
using Bex.Common;
using BexMVC.ViewModels;
using System;
using PagedList;
using System.Collections.Generic;
using AspNet.DAL.EF.UOW.Security;
using Bex.Common.Interfaces;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Owin.Security;
using AspNet.DAL.EF.Models.Security;
using System.Net;
using BexMVC.Helpers;
using System.Data.Entity.Core;
using System.Security.Claims;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BexMVC.Controllers
{
    public class UserController : Controller
    {

        public UserController() : this(new SecurityUow(System.Web.HttpContext.Current.GetOwinContext()), new BexUow())
        { }
        public UserController(
            ISecurityUow securityUow, IBexUow bexUow)
        {
            SecurityUow = securityUow;
            BexUow = bexUow;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> GetUserData(int pageSize, int pageNumber, string sortColumn, string sortOrder, string search, string searchColumn, string searchTerms) //, List<Object> searchTerms
        {
            var skip = (pageNumber - 1) * pageSize;

            var userData = (await SecurityUow.UserManager.GetUsersAsync()).Where(x => x.EmailConfirmed == false).AsEnumerable();

            if (!String.IsNullOrEmpty(searchTerms))
            {

                userData = GetSearchUserData(searchTerms, userData);

            }

            var total = userData.Count();

            if (sortOrder.Equals("desc"))
                userData = userData.OrderByDescending(s => s.GetType().GetProperty(sortColumn).GetValue(s)).ToList().Skip(skip).Take(pageSize);
            else
                userData = userData.OrderBy(s => s.GetType().GetProperty((sortColumn == "") ? "UserName" : sortColumn).GetValue(s)).ToList().Skip(skip).Take(pageSize);


            var jsonData = new TableJsonIndexData<ApplicationUser>()
            {
                total = total,
                rows = userData
            };


            var jsonResult = Json(jsonData, "TableJsonIndexData", JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;


        }

        public IEnumerable<ApplicationUser> GetSearchUserData(string searchTerms, IEnumerable<ApplicationUser> searchDataSet)
        {
            string[] terms = searchTerms.Split(',');
            string searchColumn = "";
            string searchTxt = "";

            var user = searchDataSet;

            foreach (string t in terms)
            {
                searchColumn = "";
                searchTxt = "";

                string[] searchCT = t.Split(':');
                searchColumn = searchCT[0];
                searchTxt = searchCT[1];

                if (!String.IsNullOrEmpty(searchTxt))
                {
                    if (searchColumn.Equals("UserName"))
                    {
                        user = user.Where(k => !String.IsNullOrEmpty(k.UserName)).Where(k => k.UserName.ToUpper().Contains(searchTxt.ToUpper()));
                    }
                    else if (searchColumn.Equals("Email"))
                    {
                        user = user.Where(k => !String.IsNullOrEmpty(k.Email)).Where(k => k.Email.ToUpper().Contains(searchTxt.ToUpper()));
                    }
                    else if (searchColumn.Equals("AccessFailedCount"))
                    {
                        int brojOmasaja = System.Int32.Parse(searchTxt);
                        user = user.Where(k => k.AccessFailedCount >= brojOmasaja);
                    }

                }

            }

            return user;
        }

        public async Task<ActionResult> Remove(string id)
        { 
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var applicationUser = await SecurityUow.UserManager.FindUserByIdAsync(id) as ApplicationUser;
            if (applicationUser == null)
            {
                return HttpNotFound();
            }

            var result = await SecurityUow.UserManager.DeleteAsync(applicationUser);

            return Json(new { success = "true" });
        }

        
        public async Task<ActionResult> Edit(string id, string returnUrl)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var applicationUser = await SecurityUow.UserManager.FindUserByIdAsync(id) as ApplicationUser;
            //var bexUser = BexUow.KorisniciPrograma.Find(x => x.AspNetUserId == applicationUser.Id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }


            ViewBag.AspNetUserId = id;
            ViewBag.UserName = applicationUser.UserName + " / " + applicationUser.Email;
            ViewBag.RegionId = new SelectList(BexUow.Region.AllAsNoTracking, "Id", "NazivSkraceni");

            var roles = await SecurityUow.RoleManager.GetRolesAsync();
            ViewBag.RoleId = new SelectList(roles, "Id", "Name");

            ViewBag.ReturnUrl = returnUrl;

            return View();
        }

        /* 
         * Korisnici programa se dupliraju ako vec postoje jer se jedni koriste za BZ (AspNetUserId = null) a drugi za nov (AspNetUserId ima) 
            i zbog toga KI odluka da se ne povezuju novi sa starim postojecim 
         */
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(UserIndexData model, string returnUrl)
        {

            if (ModelState.IsValid)
            {

                var applicationUser = await SecurityUow.UserManager.FindUserByIdAsync(model.AspNetUserId) as ApplicationUser;

                var korisnikPrograma = new KorisniciPrograma
                {
                    AspNetUserId = model.AspNetUserId,
                    KorisnickoIme = applicationUser.UserName,
                    KontaktId = model.KontaktId,
                    BarKod = model.BarKod,
                    RegionId = model.RegionId,
                    Aktivan = model.Aktivan,
                    Klijent = model.Klijent,
                    RoleId = model.RoleId

                };

                /* IdStari sluzi samo za stare korisnike */
                BexUow.KorisniciPrograma.Add(korisnikPrograma);
                var commandResult = BexUow.SubmitChanges();

                /* dodavanje Claim za region */
                applicationUser.Claims.Add(new IdentityUserClaim
                {
                    ClaimType = "www.bex.rs/Region",
                    ClaimValue = model.RegionId.ToString(),
                    UserId = model.AspNetUserId
                });

                var allClaims = BexUow.KorisniciProgramaClaimsRoles.AllAsNoTracking.Where(x => x.RoleId == model.RoleId).AsEnumerable();
                foreach (var claim in allClaims)
                {
                    var claimId = claim.ClaimId;
                    var claimDetails = BexUow.KorisniciProgramaClaims.AllAsNoTracking.Where(x => x.Id == claimId).FirstOrDefault();

                    applicationUser.Claims.Add(new IdentityUserClaim
                    {
                        ClaimType = claimDetails.Type,
                        ClaimValue = claimDetails.Value,
                        UserId = model.AspNetUserId
                    });
                }

                string code = await SecurityUow.UserManager.GenerateEmailConfirmationTokenAsync(model.AspNetUserId);
                var result = await SecurityUow.UserManager.ConfirmEmailAsync(model.AspNetUserId, code);

                //return View(result.Succeeded ? "ConfirmedEmail" : "Error");
                if ((commandResult.IsSuccessful) && (result.Succeeded))
                {
                    if (String.IsNullOrEmpty(returnUrl))
                    {
                        return RedirectToAction("Index");
                    }
                    else if (returnUrl.Equals("KorisniciPrograma"))
                    {
                        return RedirectToAction("../KorisniciPrograma");
                    }

                    
                }

            }

            return View(model);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            { BexUow.Dispose(); }
            base.Dispose(disposing);
        }

        private IExceptionSolver ExceptionSolver { get; }
        private IBexUow BexUow { get; }

        private ISecurityUow SecurityUow { get; }


    }
}
