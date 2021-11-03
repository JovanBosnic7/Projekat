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

namespace BexMVC.Controllers
{
    public class ClaimsRolesController : Controller
    {

        public ClaimsRolesController() : this(new SecurityUow(System.Web.HttpContext.Current.GetOwinContext()), new BexUow())
        { }
        public ClaimsRolesController(
            ISecurityUow securityUow, IBexUow bexUow)
        {
            SecurityUow = securityUow;
            BexUow = bexUow;
        }

        public async Task<ActionResult> Index()
        {
            ViewBag.Role = new SelectList(await SecurityUow.RoleManager.GetRolesAsync(), "Id", "Name");
            ViewBag.Roles = await SecurityUow.RoleManager.GetRolesAsync();

            return View();
        }

        [HttpGet]
   
        public ActionResult GetClaimsRolesData(int pageSize, int pageNumber, string sortColumn, string sortOrder, string search, string searchColumn, string searchTerms) 
        {
            var skip = (pageNumber - 1) * pageSize;

            
            var claimsData = BexUow.KorisniciProgramaClaims.AllAsNoTracking.AsEnumerable();
            
            
            if (!String.IsNullOrEmpty(searchTerms))
            {

                claimsData = GetSearchClaimData(searchTerms, claimsData);
                                                               
            }

            var total = claimsData.Count();

            if (sortOrder.Equals("desc"))
                claimsData = claimsData.OrderByDescending(s => s.GetType().GetProperty(sortColumn).GetValue(s)).ToList().Skip(skip).Take(pageSize);
            else
                claimsData = claimsData.OrderBy(s => s.GetType().GetProperty((sortColumn == "") ? "Name" : sortColumn).GetValue(s)).ToList().Skip(skip).Take(pageSize);


            var jsonData = new TableJsonIndexData<KorisniciProgramaClaims>()
            {
                total = total,
                rows = claimsData
            };


            var jsonResult = Json(jsonData, "TableJsonIndexData", JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;


        }

        public IEnumerable<KorisniciProgramaClaims> GetSearchClaimData(string searchTerms, IEnumerable<KorisniciProgramaClaims> searchDataSet)
        {
            string[] terms = searchTerms.Split(',');
            var claims = searchDataSet;


            foreach (string t in terms)
            {
                string searchColumn = "";
                string searchTxt = "";

                string[] searchCT = t.Split(':');
                searchColumn = searchCT[0];
                searchTxt = searchCT[1];

                if (!String.IsNullOrEmpty(searchTxt))
                {
                    if (searchColumn.Equals("Filter"))
                    {
                        claims = claims.Where(k => k.Filter.ToUpper().Contains(searchTxt.ToUpper()));
                    }
                    else if (searchColumn.Equals("Name"))
                    {
                        claims = claims.Where(k => k.Name.ToUpper().Contains(searchTxt.ToUpper()));
                    }
                    else if (searchColumn.Equals("Value"))
                    {
                        claims = claims.Where(k => k.Value.ToUpper().Contains(searchTxt.ToUpper()));
                    }
                    else if (searchColumn.Equals("Role"))
                    {
                        string claimRoleId = searchTxt;
                        if(!claimRoleId.Equals("0"))
                        {
                            var claimsData = BexUow.KorisniciProgramaClaimsRoles.AllAsNoTracking.Where(x => x.RoleId == claimRoleId).AsEnumerable();
                           
                            List<KorisniciProgramaClaims> listaA = new List<KorisniciProgramaClaims>();
                            foreach (var claim in claimsData)
                            {
                                var aaa = claims.Where(x => x.Id == claim.ClaimId).FirstOrDefault();
                                listaA.Add(aaa);
                               
                            }
                          
                            claims = listaA;
                        }
                       
                    }
                    else if (searchColumn.Equals("Description"))
                    {
                        claims = claims.Where(k => k.Description.ToUpper().Contains(searchTxt.ToUpper()));
                    }
                }

            }

            return claims;
        }

        public JsonResult AddClaimsToRole(SelectedValue values) 
        {
            string roleId = values.Id;
            

            foreach (var val in values.SelectedValues)
            {
                int claimId = val.Id;

                var a = BexUow.KorisniciProgramaClaimsRoles.Find(x => x.RoleId == roleId && x.ClaimId==claimId);
                if (a == null)
                {

                    var roleClaims = new KorisniciProgramaClaimsRoles
                    {
                        RoleId = roleId,
                        ClaimId = claimId
                    };


                    BexUow.KorisniciProgramaClaimsRoles.Add(roleClaims);
                };

            }

            var commandResult = BexUow.SubmitChanges();
            if (commandResult.IsSuccessful)
            {
                return Json(new { success = "true" });
                
            }

            //ExceptionSolver.PrepareModelState(ModelState, commandResult);

            return Json(new { success = "false" });
           
        }

        public ActionResult DetailsRoles(string id)
        {
            ViewBag.ClaimId = id;

            return View();
        }

        [HttpGet]
        public async Task<ActionResult> GetRoleData(int claimId)
        {
            var listRole = BexUow.KorisniciProgramaClaimsRoles.AllAsNoTracking.Where(x => x.ClaimId == claimId).AsEnumerable();

            List<RoleIndexData> roleData = new List<RoleIndexData>();

            foreach (var role in listRole)
            {
                var name = (await SecurityUow.RoleManager.GetRolesAsync()).Where(x => x.Id == role.RoleId).FirstOrDefault().Name;

                var roleNames = new RoleIndexData
                {
                    RoleId = role.RoleId,
                    RoleName = name

                };

                roleData.Add(roleNames);
            }

            return Json(roleData, "", JsonRequestBehavior.AllowGet);
           
        }

        [HttpGet]
        public ActionResult RemoveRole(string roleId, int claimId)
        {
            var role = BexUow.KorisniciProgramaClaimsRoles.Find(x => x.RoleId == roleId && x.ClaimId == claimId);

            BexUow.KorisniciProgramaClaimsRoles.Remove(role);

            var commandResult = BexUow.SubmitChanges();

            return RedirectToAction("Index");

        }

        public ActionResult DetailsClaims(string id )
        {
            ViewBag.UserId = id;

            return View();
        }

        [HttpGet]
        public async Task<ActionResult> Edit(string id)
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

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> MoveUserToKorisniciPrograma( UserIndexData model)
        {
            
            if (ModelState.IsValid)
            {
                var korisnikPrograma = new KorisniciPrograma
                {
                    AspNetUserId = model.AspNetUserId,
                    KontaktId = model.KontaktId,
                    BarKod = model.BarKod,
                    RegionId = model.RegionId,
                    Aktivan = model.Aktivan,
                    Klijent = model.Klijent,
                    RoleId = model.RoleId
                    
                };

                BexUow.KorisniciPrograma.Add(korisnikPrograma);
                var commandResult = BexUow.SubmitChanges();

                

                string code = await SecurityUow.UserManager.GenerateEmailConfirmationTokenAsync(model.AspNetUserId);
                var result = await SecurityUow.UserManager.ConfirmEmailAsync(model.AspNetUserId, code);

                //return View(result.Succeeded ? "ConfirmedEmail" : "Error");
                if ((commandResult.IsSuccessful) && (result.Succeeded))
                { return RedirectToAction("../User"); }
                


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
