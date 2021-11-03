using System.Linq;
using System.Net;
using System.Web.Mvc;
using Bex.Models;
using Bex.MVC.Exceptions;
using Bex.DAL.EF.UOW;
using Bex.Common;
using BexMVC.ViewModels;
using System;
using PagedList;
using System.Collections.Generic;
using BexMVC.Helpers;
using System.Data.Entity.Core;
using BexMVC.Filters;
using System.Web.UI;
using Bex.Common.Interfaces;
using AspNet.DAL.EF.UOW.Security;
using System.Web;
using AspNet.DAL.EF.Models.Security;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;

namespace BexMVC.Controllers
{
   
    [ClaimsAuthentication(Resource = "VozniParkOprema", Operation = "Read, All")]
    public class VozniParkOpremaController : Controller
    {
        public VozniParkOpremaController() : this(new SecurityUow(System.Web.HttpContext.Current.GetOwinContext()), new BexUow())
        { }
        public VozniParkOpremaController(
            ISecurityUow securityUow, IBexUow bexUow)
        {
            SecurityUow = securityUow;
            BexUow = bexUow;
        }

        public ActionResult AddOprema(string id)
        {
            ViewBag.VpId = id;
            
            return View();
        }

        [HttpGet]

        public ActionResult GetOpremaData(int pageSize, int pageNumber, string sortColumn, string sortOrder, string search, string searchColumn, string searchTerms)
        {
            var skip = (pageNumber - 1) * pageSize;


            var opremaData = BexUow.VpOpremaPodTip.AllAsNoTracking
                                                  .Select(x=> new VozniParkOpremaIndexData {
                                                                    Id= x.Id,
                                                                    GrupaNaziv = x.VpOpremaTip.VpOpremaGrupa.Naziv,
                                                                    TipNaziv = x.VpOpremaTip.Naziv,
                                                                    PodtipNaziv = x.Naziv
                                                  }).AsEnumerable();


            if (!String.IsNullOrEmpty(searchTerms))
            {

                opremaData = GetSearchOpremaData(searchTerms, opremaData);

            }

            var total = opremaData.Count();

            if (sortOrder.Equals("desc"))
                opremaData = opremaData.OrderByDescending(s => s.GetType().GetProperty(sortColumn).GetValue(s)).ToList().Skip(skip).Take(pageSize);
            else
                opremaData = opremaData.OrderBy(s => s.GetType().GetProperty((sortColumn == "") ? "PodtipNaziv" : sortColumn).GetValue(s)).ToList().Skip(skip).Take(pageSize);


            var jsonData = new TableJsonIndexData<VozniParkOpremaIndexData>()
            {
                total = total,
                rows = opremaData
            };


            var jsonResult = Json(jsonData, "TableJsonIndexData", JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;


        }

        public IEnumerable<VozniParkOpremaIndexData> GetSearchOpremaData(string searchTerms, IEnumerable<VozniParkOpremaIndexData> searchDataSet)
        {
            string[] terms = searchTerms.Split(',');
            var oprema = searchDataSet;


            foreach (string t in terms)
            {
                string searchColumn = "";
                string searchTxt = "";

                string[] searchCT = t.Split(':');
                searchColumn = searchCT[0];
                searchTxt = searchCT[1];

                if (!String.IsNullOrEmpty(searchTxt))
                {
                    if (searchColumn.Equals("GrupaNaziv"))
                    {
                        oprema = oprema.Where(k => k.GrupaNaziv.ToUpper().Contains(searchTxt.ToUpper()));
                    }
                    else if (searchColumn.Equals("TipNaziv"))
                    {
                        oprema = oprema.Where(k => k.TipNaziv.ToUpper().Contains(searchTxt.ToUpper()));
                    }
                    else if (searchColumn.Equals("PodtipNaziv"))
                    {
                        oprema = oprema.Where(k => k.PodtipNaziv.ToUpper().Contains(searchTxt.ToUpper()));
                    }
                    
                }

            }

            return oprema;
        }

        public async Task<JsonResult> AddOpremaToVozilo(SelectedValue values)
        {
            int vpId = System.Convert.ToInt32(values.Id);

            var applicationUser = await SecurityUow.UserManager.FindUserByIdAsync(User.Identity.GetUserId()) as ApplicationUser;
            var korisnikProgramaId = BexUow.KorisniciPrograma.Find(x => x.AspNetUserId == applicationUser.Id).Id;

            foreach (var val in values.SelectedValues)
            {
                int opremaId = val.Id;
                BexUow.VpOprema.Add(new VpOprema {
                    VpId = vpId,
                    PodTipId = opremaId,
                    UserUnosId = korisnikProgramaId,
                    DatumUnosa = DateTime.Now
                });
                var commandResult = BexUow.SubmitChanges();
            }


            return Json(new { success = "true" });

        }

        [HttpGet]
        public ActionResult GetOpremaDetailsData(int id)
        {

            var detailsOprema = BexUow.VpOprema.AllAsNoTracking.Where(x => x.VpId == id)
                                      .Select(x=> new VozniParkOpremaIndexData {
                                          Id = x.Id,
                                          GrupaNaziv = x.VpOpremaPodTip.VpOpremaTip.VpOpremaGrupa.Naziv,
                                          TipNaziv = x.VpOpremaPodTip.VpOpremaTip.Naziv,
                                          PodtipNaziv = x.VpOpremaPodTip.Naziv,
                                          DatumUnosa = x.DatumUnosa,
                                          UserUneoNaziv = x.KorisniciPrograma.Kontakt.Naziv
                                      });

            return Json(detailsOprema, "", JsonRequestBehavior.AllowGet);
        }

        public ActionResult RemoveOprema(int id)
        {
            var vpOprema = BexUow.VpOprema.Find(id);
            BexUow.VpOprema.Remove(vpOprema);
           
            var commandResult = BexUow.SubmitChanges();
            if (commandResult.IsSuccessful)
            {
                return RedirectToAction("../VozniPark");
               
            }

            return View();
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
