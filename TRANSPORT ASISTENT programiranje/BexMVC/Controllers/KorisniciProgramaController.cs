

using AspNet.DAL.EF.Models.Security;
using AspNet.DAL.EF.UOW.Security;
using Bex.Common;
using Bex.Common.Interfaces;
using Bex.DAL.EF.UOW;
using Bex.Models;
using Bex.MVC.Exceptions;
using BexMVC.Filters;
using BexMVC.Helpers;
using BexMVC.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace BexMVC.Controllers
{
    [Authorize]
    [ClaimsAuthentication(Resource = "KorisniciPrograma", Operation = "Read, All")]
    public class KorisniciProgramaController : Controller
    {

        public KorisniciProgramaController() : this(new SecurityUow(System.Web.HttpContext.Current.GetOwinContext()), new BexUow())
        { }
        public KorisniciProgramaController(
            ISecurityUow securityUow, IBexUow bexUow)
        {
            SecurityUow = securityUow;
            BexUow = bexUow;
        }

        public ActionResult Index()
        {
            ViewBag.Regioni = new SelectList(BexUow.Region.AllAsNoTracking, "NazivSkraceni", "NazivSkraceni");

            return View();
        }

        [HttpGet]
   
        public ActionResult GetKorisniciData(int pageSize, int pageNumber, string sortColumn, string sortOrder, string search, string searchColumn, string searchTerms)
        {
            var skip = (pageNumber - 1) * pageSize;

            var userData = (from u in BexUow.KorisniciPrograma.AllAsNoTracking.Where(r => !String.IsNullOrEmpty(r.AspNetUserId))
                         let kontakt = (from k in BexUow.Kontakts.AllAsNoTracking
                                        where u.KontaktId == k.Id
                                        select k).OrderByDescending(z => z.Id).Take(1).FirstOrDefault()
                         let region = (from r in BexUow.Region.AllAsNoTracking
                                       where r.Id == u.RegionId
                                       select r).OrderByDescending(a => a.Id).Take(1).FirstOrDefault()

                         select new KorisniciProgramaIndexData
                         {
                             Id = u.AspNetUserId,
                             KorisnickoIme = u.KorisnickoIme,
                             KontaktNaziv = kontakt.Naziv.ToString(),
                             BarKod = u.BarKod,
                             RegionNaziv = region.NazivSkraceni.ToString(),
                             Klijent = u.Klijent,
                             Aktivan = u.Aktivan
                         }).AsEnumerable();

                                             
            if (!String.IsNullOrEmpty(searchTerms))
            {

                userData = GetSearchKorisniciProgramaData(searchTerms, userData)
                                                                .Select(x =>
                                                                     new KorisniciProgramaIndexData
                                                                     {
                                                                         Id = x.Id,
                                                                         KorisnickoIme = x.KorisnickoIme,
                                                                         KontaktNaziv = x.KontaktNaziv,
                                                                         BarKod = x.BarKod,
                                                                         RegionNaziv = x.RegionNaziv,
                                                                         Klijent = x.Klijent,
                                                                         Aktivan = x.Aktivan
                                                                     });
            }

            var total = userData.Count();

            if (sortOrder.Equals("desc"))
                userData = userData.OrderByDescending(s => s.GetType().GetProperty(sortColumn).GetValue(s)).ToList().Skip(skip).Take(pageSize);
            else
                userData = userData.OrderBy(s => s.GetType().GetProperty((sortColumn == "") ? "KontaktNaziv" : sortColumn).GetValue(s)).ToList().Skip(skip).Take(pageSize);


            var jsonData = new TableJsonIndexData<KorisniciProgramaIndexData>()
            {
                total = total,
                rows = userData
            };


            var jsonResult = Json(jsonData, "TableJsonIndexData", JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;


        }

        public IEnumerable<KorisniciProgramaIndexData> GetSearchKorisniciProgramaData(string searchTerms, IEnumerable<KorisniciProgramaIndexData> searchDataSet)
        {
            string[] terms = searchTerms.Split(',');
            
            var userDataSet = searchDataSet;

            foreach (string t in terms)
            {
                /* da bi search radio po svim kolonoma ovde mora da budu deklarisane promenljive */
                string searchColumn = "";
                string searchTxt = "";

                string[] searchCT = t.Split(':');
                searchColumn = searchCT[0];
                searchTxt = searchCT[1];

                if (!String.IsNullOrEmpty(searchTxt))
                {
                    if (searchColumn.Equals("KontaktNaziv"))
                    {
                        userDataSet = userDataSet.Where(k => !String.IsNullOrEmpty(k.KontaktNaziv)).Where(k => k.KontaktNaziv.ToUpper().Contains(searchTxt.ToUpper()));
                    }
                    else if (searchColumn.Equals("KorisnickoIme"))
                    {
                        userDataSet = userDataSet.Where(k => !String.IsNullOrEmpty(k.KorisnickoIme)).Where(k => k.KorisnickoIme.ToUpper().Contains(searchTxt.ToUpper()));
                    }
                    else if (searchColumn.Equals("BarKod"))
                    {
                        userDataSet = userDataSet.Where(k => !String.IsNullOrEmpty(k.BarKod)).Where(k => k.BarKod.ToUpper().Contains(searchTxt.ToUpper()));
                    }
                    else if (searchColumn.Equals("Region"))
                    {
                        userDataSet = userDataSet.Where(k => !String.IsNullOrEmpty(k.RegionNaziv)).Where(k => k.RegionNaziv.ToUpper().Contains(searchTxt.ToUpper()));
                    }
                    else if (searchColumn.Equals("Klijent"))
                    {
                        bool isKlijent = (searchTxt == "1") ? true : false;
                        userDataSet = userDataSet.Where(k => k.Klijent.Equals(isKlijent));
                    }
                    else if (searchColumn.Equals("Aktivan"))
                    {
                        bool isAktivan = (searchTxt == "1") ? true : false;
                        userDataSet = userDataSet.Where(k => k.Aktivan.Equals(isAktivan));
                    }
                }
            }

            return userDataSet;
        }

        [ClaimsAuthentication(Resource = "KorisniciPrograma", Operation = "Details, All")]
        public ActionResult DetailsClaims(string id)
        {
            ViewBag.UserId = id;

            return View();
        }


        [HttpGet]
        public async Task<ActionResult> GetClaimsData(string userId) //, string roleId
        {
             var applicationUser = await SecurityUow.UserManager.FindUserByIdAsync(userId) as ApplicationUser;

                var claimsData = (applicationUser.Claims).Select(x => new
                {
                    x.Id,
                    x.ClaimType,
                    Opis = BexUow.KorisniciProgramaClaims.AllAsNoTracking.Where(a=>a.Type.Equals(x.ClaimType) && a.Value.Equals(x.ClaimValue)).FirstOrDefault().Name,
                    Value = x.ClaimValue
                });


                return Json(claimsData, "", JsonRequestBehavior.AllowGet);
           
        }

        [HttpGet]
        public async Task<ActionResult> RemoveClaim(string claimType, string claimValue, string userId)
        {

            await SecurityUow.UserManager.RemoveClaimAsync(userId, new Claim(claimType, claimValue));

            
            /* pogledati napomenu jer ovo treba da bude partial view*/
            return RedirectToAction("Index");

        }

        public async Task<ActionResult> AddClaims(string id)
        {
            ViewBag.UserId = id;
            ViewBag.Role = new SelectList(await SecurityUow.RoleManager.GetRolesAsync(), "Id", "Name");
            return View();
        }

        public async Task<JsonResult> AddClaimsToUser(SelectedValue values)
        {
            string userId = values.Id;
            var applicationUser = await SecurityUow.UserManager.FindUserByIdAsync(userId) as ApplicationUser;

            foreach (var val in values.SelectedValues)
            {
                int claimId = val.Id;
                var claimDetails = BexUow.KorisniciProgramaClaims.AllAsNoTracking.Where(x => x.Id == claimId).FirstOrDefault();
                if (claimDetails.Value.Equals("All")){
                    string filterType = claimDetails.Type;
                    //var claimsData = (applicationUser.Claims).Where(x => x.ClaimType == filterType);
                    foreach (var cl in ((await SecurityUow.UserManager.GetClaimsAsync(userId)).Where(x => x.Type == filterType)))
                    //foreach(var cl in claimsData)
                    {
                        await SecurityUow.UserManager.RemoveClaimAsync(userId, new Claim(cl.Type, cl.Value));
                    }
                    
                }

                await SecurityUow.UserManager.AddClaimAsync(userId, new Claim(claimDetails.Type, claimDetails.Value));

            }


            return Json(new { success = "true" });

        }

        [HttpGet]
        [ClaimsAuthentication(Resource = "KorisniciPrograma", Operation = "Edit, All")]
        public async Task<ActionResult> Edit(string id)
        {

            var bexUser = BexUow.KorisniciPrograma.Find(x => x.AspNetUserId == id);
            KorisniciProgramaIndexData user = new KorisniciProgramaIndexData
            {
                Id = bexUser.AspNetUserId,
                KontaktId = bexUser.KontaktId,
                BarKod = bexUser.BarKod,
                RegionId = bexUser.RegionId,
                RoleId = bexUser.RoleId,
                Aktivan = bexUser.Aktivan,
                Klijent = bexUser.Klijent
            };


            ViewBag.AspNetUserId = bexUser.AspNetUserId;
            ViewBag.KontaktNaziv = BexUow.Kontakts.Find(x => x.Id == bexUser.KontaktId).Naziv;
            ViewBag.RegionId = new SelectList(BexUow.Region.AllAsNoTracking, "Id", "NazivSkraceni", bexUser.RegionId);

            var roles = await SecurityUow.RoleManager.GetRolesAsync();
            ViewBag.RoleId = new SelectList(roles, "Id", "Name", bexUser.RoleId);

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdateKorisniciPrograma(KorisniciProgramaIndexData model)
        {
            if (ModelState.IsValid)
            {
                KorisniciPrograma bexUser = BexUow.KorisniciPrograma.Find(x => x.AspNetUserId == model.Id);
                string oldRoleId = bexUser.RoleId;
                bexUser.AspNetUserId = model.Id;
                bexUser.KontaktId = model.KontaktId;
                bexUser.BarKod = model.BarKod;
                bexUser.RegionId = model.RegionId;
                bexUser.RoleId = model.RoleId;
                bexUser.Aktivan = model.Aktivan;
                bexUser.Klijent = model.Klijent;

                BexUow.KorisniciPrograma.Update(bexUser);

                var uowCommandResult = BexUow.SubmitChanges();

                if (uowCommandResult.IsSuccessful)
                {
                    if (oldRoleId != model.RoleId)
                    {
                        /* ukoliko je promenjena osnovna uloga usera - brisu se svi postojeci */
                        var applicationUser = await SecurityUow.UserManager.FindUserByIdAsync(model.Id) as ApplicationUser;
                        
                        //applicationUser.Claims.Clear();
                        //var allRemovedClaims = BexUow.KorisniciProgramaClaimsRoles.AllAsNoTracking.Where(x => x.RoleId == oldRoleId).AsEnumerable();
                        foreach (var claimRemove in (await SecurityUow.UserManager.GetClaimsAsync(model.Id)))
                        {                          
                            await SecurityUow.UserManager.RemoveClaimAsync(model.Id, new Claim(claimRemove.Type, claimRemove.Value));
                        }

                        /* a dodaju svi novi vezani za izabranu ulogu */
                        var allClaims = BexUow.KorisniciProgramaClaimsRoles.AllAsNoTracking.Where(x => x.RoleId == model.RoleId).AsEnumerable();                     
                        foreach (var claim in allClaims)
                        {
                            var claimId = claim.ClaimId;
                            var claimDetails = BexUow.KorisniciProgramaClaims.AllAsNoTracking.Where(x => x.Id == claimId).FirstOrDefault();

                            await SecurityUow.UserManager.AddClaimAsync(model.Id, new Claim(claimDetails.Type, claimDetails.Value));
                        }

                        
                    }
                    return RedirectToAction("Index");
                }
            }

            return View();
        }

        //[ChildActionOnly]
        ////[ClaimsAuthentication(Resource = "KorisniciPrograma", Operation = "Create")]
        //public ActionResult Register()
        //{
        //    //return PartialView("../Account/RegisterKorisniciPrograma");
        //    return RedirectToAction("../User/Edit", new { id = user.Id });
        //}

        
        //[ClaimsAuthentication(Resource = "KorisniciPrograma", Operation = "Create")]
        public ActionResult Create()
        {
            return RedirectToAction("../Account/RegisterKorisniciPrograma");
            //return View();           
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
