using System.Linq;
using System.Net;
using System.Web.Mvc;
using Bex.Models;

using Bex.DAL.EF.UOW;
using Bex.Common;

using System;

using System.Collections.Generic;


using System.Web.UI;
using Bex.Common.Interfaces;
using AspNet.DAL.EF.UOW.Security;
using System.Web;
using AspNet.DAL.EF.Models.Security;

using System.Threading.Tasks;
using DDtrafic.ViewModels;
using DDtrafic.MVC.Exceptions;
using System.Web;
using DDtrafic.Helpers;
using System.Data.Entity.Core;
using Microsoft.AspNet.Identity;

namespace DDtrafic.Controllers
{
  
    public class PutniNalogController : Controller
    {
        public PutniNalogController() : this(new SecurityUow(System.Web.HttpContext.Current.GetOwinContext()), new BexUow())
        { }
        public PutniNalogController(
            ISecurityUow securityUow, IBexUow bexUow)
        {
            SecurityUow = securityUow;
            BexUow = bexUow;
        }
        public ActionResult Index()
        {
            TempData.ToList().ForEach(keyValue =>
                ModelState.AddModelError("", keyValue.Value.ToString()));
            TempData.Clear();

            return View();

        }

        [HttpGet]
        public async Task<ActionResult> GetPutniNalog(int pageSize, int pageNumber, string sortColumn, string sortOrder, string search, string searchColumn, string searchTerms)
        {
            var applicationUser = await SecurityUow.UserManager.FindUserByIdAsync(User.Identity.GetUserId()) as ApplicationUser;
            var korisnikPrograma = BexUow.KorisniciPrograma.Find(x => x.AspNetUserId == applicationUser.Id);

            var skip = (pageNumber - 1) * pageSize;

            var putniNalogData = BexUow.PutniNalog.GetAll(true).Where(x => x.VozniPark.FirmaId==korisnikPrograma.FirmaId)
                                                    .Select(x =>
                                                         new PutniNalogIndexData
                                                         {
                                                             Id = x.Id,
                                                             Vozilo = x.VozniPark.Oznaka,
                                                             Vozac=x.Zaposleni.ImeIprezime,
                                                             Relacija = x.Relacija,
                                                             DatumStart = x.DatumStart,
                                                             DatumStop = x.DatumStop,
                                                             VremeStart = x.VremeStart,
                                                             VremeStop = x.VremeStop,
                                                             KmStart = x.KmStart,
                                                             KmStop = x.KmStop,
                                                             KmUkupno = x.KmUkupno,
                                                             Napomena = x.Napomena.ToString()

                                                         }).AsEnumerable();
            if (!String.IsNullOrEmpty(searchTerms))
            {
                putniNalogData = GetSearchAlarmData(searchTerms, putniNalogData).AsEnumerable();
            }

            var total = putniNalogData.Count();

            if (sortOrder.Equals("desc"))
                putniNalogData = putniNalogData.OrderByDescending(s => s.GetType().GetProperty(sortColumn).GetValue(s)).ToList().Skip(skip).Take(pageSize);
            else
                putniNalogData = putniNalogData.OrderBy(s => s.GetType().GetProperty((sortColumn == "") ? "DatumStart" : sortColumn).GetValue(s)).ToList().Skip(skip).Take(pageSize);


            var jsonData = new TableJsonIndexData<PutniNalogIndexData>()
            {
                total = total,
                rows = putniNalogData
            };

            var jsonResult = Json(jsonData, "TableJsonIndexData", JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;

        }

        public IEnumerable<PutniNalogIndexData> GetSearchAlarmData(string searchTerms, IEnumerable<PutniNalogIndexData> putniNalogData)
        {
            string[] terms = searchTerms.Split(',');

            foreach (string t in terms)
            {
                string[] searchCT = t.Split(':');

                string searchColumn = "";
                string searchTxt = "";

                searchColumn = searchCT[0];
                searchTxt = searchCT[1];


                if (searchColumn.Equals("DatumStart") && !String.IsNullOrEmpty(searchTxt))
                {
                    DateTime datumFirst, datumSecond;
                    string[] arrDatumAll = searchTxt.Split('t', 'o');

                    string[] arrDatumFirst = arrDatumAll[0].Trim().Split('-');
                    datumFirst = new DateTime(int.Parse(arrDatumFirst[0]), int.Parse(arrDatumFirst[1]), int.Parse(arrDatumFirst[2]));

                    string[] arrDatumSecond = arrDatumAll[2].Trim().Split('-');
                    datumSecond = new DateTime(int.Parse(arrDatumSecond[0]), int.Parse(arrDatumSecond[1]), int.Parse(arrDatumSecond[2]));

                    putniNalogData = putniNalogData.Where(k => k.DatumStart.HasValue && k.DatumStart >= datumFirst && k.DatumStart <= datumSecond);
                }
                else if (searchColumn.Equals("DatumStop") && !String.IsNullOrEmpty(searchTxt))
                {
                    DateTime datumFirst, datumSecond;
                    string[] arrDatumAll = searchTxt.Split('t', 'o');

                    string[] arrDatumFirst = arrDatumAll[0].Trim().Split('-');
                    datumFirst = new DateTime(int.Parse(arrDatumFirst[0]), int.Parse(arrDatumFirst[1]), int.Parse(arrDatumFirst[2]));

                    string[] arrDatumSecond = arrDatumAll[2].Trim().Split('-');
                    datumSecond = new DateTime(int.Parse(arrDatumSecond[0]), int.Parse(arrDatumSecond[1]), int.Parse(arrDatumSecond[2]));

                    putniNalogData = putniNalogData.Where(k => k.DatumStop.HasValue && k.DatumStop.Value.Date >= datumFirst && k.DatumStop.Value.Date <= datumSecond);

                }                
                else if (searchColumn.Equals("Relacija") && !String.IsNullOrEmpty(searchTxt))
                {
                    putniNalogData = putniNalogData.Where(k => k.Relacija.ToUpper().Contains(searchTxt.ToUpper()));
                }
                else if (searchColumn.Equals("Vozilo") && !String.IsNullOrEmpty(searchTxt))
                {
                    putniNalogData = putniNalogData.Where(k => k.Vozilo.ToUpper().Contains(searchTxt.ToUpper()));
                }
                else if (searchColumn.Equals("Vozac") && !String.IsNullOrEmpty(searchTxt))
                {
                    putniNalogData = putniNalogData.Where(k => k.Vozac.ToUpper().Contains(searchTxt.ToUpper()));
                }
                else if (searchColumn.Equals("KmStart") && !String.IsNullOrEmpty(searchTxt))
                {
                    int KmIstekaInt = System.Convert.ToInt32(searchTxt);
                    putniNalogData = putniNalogData.Where(k => k.KmStart.Equals(KmIstekaInt));
                }
                else if (searchColumn.Equals("KmStop") && !String.IsNullOrEmpty(searchTxt))
                {
                    int KmAlarmaInt = System.Convert.ToInt32(searchTxt);
                    putniNalogData = putniNalogData.Where(k => k.KmStop.Equals(KmAlarmaInt));
                }
                else if (searchColumn.Equals("KmUkupno") && !String.IsNullOrEmpty(searchTxt))
                {
                    int KmAlarmaInt = System.Convert.ToInt32(searchTxt);
                    putniNalogData = putniNalogData.Where(k => k.KmUkupno.Equals(KmAlarmaInt));
                }
                else if (searchColumn.Equals("Napomena") && !String.IsNullOrEmpty(searchTxt))
                {
                    putniNalogData = putniNalogData.Where(k => k.Napomena.ToUpper().Contains(searchTxt.ToUpper()));
                }


            }
            return putniNalogData;
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PutniNalogIndexData putniNalog)
        {
            if (ModelState.IsValid)
            {
                BexUow.PutniNalog.Add(new PutniNalog
                {
                    VpId = putniNalog.VpId,
                    VozacId= putniNalog.VozacId,
                    Relacija = putniNalog.Relacija,                    
                    DatumStart = putniNalog.DatumStart,
                    DatumStop = putniNalog.DatumStop,
                    VremeStart = putniNalog.VremeStart,
                    VremeStop = putniNalog.VremeStop,
                    KmStart = putniNalog.KmStart,
                    KmStop= putniNalog.KmStop,
                    KmUkupno = putniNalog.KmStop - putniNalog.KmStart,
                    Napomena = putniNalog.Napomena

                });

                var commandResult = BexUow.SubmitChanges();
                commandResult = BexUow.SubmitChanges();
                if (!commandResult.IsSuccessful)
                { return RedirectToAction("Index", "PutniNalog"); }

                ExceptionSolver.PrepareModelState(ModelState, commandResult);
                //return Json("Uspešno ste dodali alarm", JsonRequestBehavior.AllowGet);
            }

            return View(putniNalog);
        }

        public ActionResult Edit(int Id)
        {
            var putniNalog = BexUow.PutniNalog.AllAsNoTracking.Where(x => x.Id == Id).Select(x =>
                                  new PutniNalogIndexData
                                  {
                                      Id = x.Id,
                                      VpId = x.VpId,
                                      VozacId = x.VozacId,
                                      Vozac = x.Zaposleni.ImeIprezime,
                                      Vozilo = x.VozniPark.Oznaka,
                                      Relacija = x.Relacija,
                                      DatumStart = x.DatumStart,
                                      DatumStop = x.DatumStop,
                                      VremeStart = x.VremeStart,
                                      VremeStop = x.VremeStop,
                                      KmStart = x.KmStart,
                                      KmStop = x.KmStop,
                                      Napomena = x.Napomena
                                  }).FirstOrDefault();

            ViewBag.Oznaka = (putniNalog.VpId > 0 && putniNalog.VpId != null) ? BexUow.VozniPark.Find(x => x.Id == putniNalog.VpId).Oznaka : "";
            ViewBag.Zaposleni = (putniNalog.VozacId > 0 && putniNalog.VozacId != null) ? BexUow.Zaposleni.Find(x => x.Id == putniNalog.VozacId).ImeIprezime : "";

            return View(putniNalog);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PutniNalogIndexData putniNalog)
        {
            if (ModelState.IsValid)
            {
                var putniNalogUpdate = BexUow.PutniNalog.Find(putniNalog.Id);
                putniNalogUpdate.VpId = putniNalog.VpId;
                putniNalogUpdate.VozacId = putniNalog.VozacId;
                putniNalogUpdate.Relacija = putniNalog.Relacija;
                putniNalogUpdate.DatumStart = putniNalog.DatumStart;
                putniNalogUpdate.DatumStop = putniNalog.DatumStop;
                putniNalogUpdate.VremeStart = putniNalog.VremeStart;
                putniNalogUpdate.VremeStop = putniNalog.VremeStop;
                putniNalogUpdate.KmStart = putniNalog.KmStart;
                putniNalogUpdate.KmStop = putniNalog.KmStop;
                putniNalogUpdate.KmUkupno = putniNalog.KmStop - putniNalog.KmStart;
                putniNalogUpdate.Napomena = putniNalog.Napomena;

                BexUow.PutniNalog.Update(putniNalogUpdate);
              
                
                var uowCommandResult = BexUow.SubmitChanges();

                if (uowCommandResult.IsSuccessful)
                { return RedirectToAction("Index", "PutniNalog"); }

                ExceptionSolver.PrepareModelState(ModelState, uowCommandResult);
            }

            return HttpNotFound();
        }

        public async Task<ActionResult> GetVozila(string query)
        {
            var applicationUser = await SecurityUow.UserManager.FindUserByIdAsync(User.Identity.GetUserId()) as ApplicationUser;
            var korisnikPrograma = BexUow.KorisniciPrograma.Find(x => x.AspNetUserId == applicationUser.Id);

            return Json(_GetVozila(query,korisnikPrograma.FirmaId), JsonRequestBehavior.AllowGet);
        }
        private List<Autocomplete> _GetVozila(string query,int? firmaId)
        {


            List<Autocomplete> vozilaList = new List<Autocomplete>();
            try
            {
                vozilaList = BexUow.VozniPark.AllAsNoTracking
                                                   .Where(x => x.Oznaka.ToUpper().Contains(query.ToUpper()) && x.FirmaId == firmaId)
                                                   .Select(a => new Autocomplete
                                                   {
                                                       Name = a.Oznaka,
                                                       Id = a.Id
                                                   }
                                                   )
                                                   .ToList();
            }
            catch (EntityCommandExecutionException eceex)
            {
                if (eceex.InnerException != null)
                {
                    throw eceex.InnerException;
                }
                throw;
            }
            catch
            {
                throw;
            }
            return vozilaList;
        }

        public async Task<ActionResult> GetZaposleni(string query)
        {
            var applicationUser = await SecurityUow.UserManager.FindUserByIdAsync(User.Identity.GetUserId()) as ApplicationUser;
            var korisnikPrograma = BexUow.KorisniciPrograma.Find(x => x.AspNetUserId == applicationUser.Id);

            return Json(_GetZaposleni(query,korisnikPrograma.FirmaId), JsonRequestBehavior.AllowGet);
        }
        private List<Autocomplete> _GetZaposleni(string query,int? firmaId)
        {


            List<Autocomplete> zaposleniList = new List<Autocomplete>();
            try
            {
                zaposleniList = BexUow.Zaposleni.AllAsNoTracking
                                                   .Where(x => x.ImeIprezime.ToUpper().Contains(query.ToUpper()) && x.FirmaId == firmaId)
                                                   .Select(a => new Autocomplete
                                                   {
                                                       Name = a.ImeIprezime,
                                                       Id = a.Id
                                                   }
                                                   )
                                                   .ToList();
            }
            catch (EntityCommandExecutionException eceex)
            {
                if (eceex.InnerException != null)
                {
                    throw eceex.InnerException;
                }
                throw;
            }
            catch
            {
                throw;
            }
            return zaposleniList;
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
