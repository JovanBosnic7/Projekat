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
using BexMVC.Filters;
using Microsoft.AspNet.Identity;

namespace BexMVC.Controllers
{
    [Authorize]
    [ClaimsAuthentication(Resource = "Prijava", Operation = "Read, All")]
    public class PrijavaController : Controller
    {
        private ISecurityUow SecurityUow { get; set; }
        private IBexUow BexUow { get; }

        public PrijavaController() : this(new SecurityUow(System.Web.HttpContext.Current.GetOwinContext()), new BexUow())
        { }
        public PrijavaController(
            ISecurityUow securityUow, IBexUow bexUow)
        {
            SecurityUow = securityUow;
            BexUow = bexUow;
        }

        public ActionResult Index()
        {
            
            ViewBag.Tip = new SelectList(BexUow.PrijavaTip.AllAsNoTracking, "Naziv", "Naziv");
            ViewBag.PodTip = new SelectList(BexUow.PrijavaPodTip.AllAsNoTracking, "Naziv", "Naziv");
            ViewBag.NacinPrijave = new SelectList(BexUow.PrijavaNacin.AllAsNoTracking, "Naziv", "Naziv");
            ViewBag.Status = new SelectList(BexUow.PrijavaStatus.AllAsNoTracking, "Naziv", "Naziv");
            return View();
        }

        [HttpGet]
   
        public ActionResult GetPrijaveData(int pageSize, int pageNumber, string sortColumn, string sortOrder, string search, string searchColumn, string searchTerms) 
        {
            var skip = (pageNumber - 1) * pageSize;

            
            //var prijavaData = BexUow.PrijavaReklamacijaZalba.GetPrijavaData().AsEnumerable();

            var prijavaData = BexUow.PrijavaReklamacijaZalba.GetPrijavaData().Select(x =>
                                                                     new PrijavaIndexData
                                                                     {
                                                                         PrijavaId = x.Id,
                                                                         TipPrijaveNaziv = x.PrijavaTip.Naziv,
                                                                         PodTipPrijaveNaziv = x.PrijavaPodTip.Naziv,
                                                                         NacinPrijaveNaziv = x.PrijavaNacin.Naziv,
                                                                         PosiljkaId = x.PosiljkaId,
                                                                         PrijavioUserNaziv = x.KorisniciPrograma.Kontakt.Naziv,
                                                                         PrijavioNaziv = x.PrijavioIme + " " + x.PrijavioPrezime,
                                                                         PrijavioEmail = x.PrijavioEmail,
                                                                         PrijavioTelefon = x.PrijavioTelefon,
                                                                         Opis = x.Opis,
                                                                         StatusPrijaveNaziv = x.PrijavaStatus.Naziv,
                                                                         DatumPrijave = x.DatumPrijave,
                                                                         DatumPoslednjeIzmene = x.DatumPoslednjeIzmene
                                                                     }).AsEnumerable();


            if (!String.IsNullOrEmpty(searchTerms))
            {

                prijavaData = GetSearchPrijaveData(searchTerms, prijavaData).Select(x =>
                                                                     new PrijavaIndexData
                                                                     {
                                                                         PrijavaId = x.PrijavaId,
                                                                         TipPrijaveNaziv = x.TipPrijaveNaziv,
                                                                         PodTipPrijaveNaziv = x.PodTipPrijaveNaziv,
                                                                         NacinPrijaveNaziv = x.NacinPrijaveNaziv,
                                                                         PosiljkaId = x.PosiljkaId,
                                                                         PrijavioUserNaziv = x.PrijavioUserNaziv,
                                                                         PrijavioNaziv = x.PrijavioNaziv,
                                                                         PrijavioEmail = x.PrijavioEmail,
                                                                         PrijavioTelefon = x.PrijavioTelefon,
                                                                         Opis = x.Opis,
                                                                         StatusPrijaveNaziv = x.StatusPrijaveNaziv,
                                                                         DatumPrijave = x.DatumPrijave,
                                                                         DatumPoslednjeIzmene = x.DatumPoslednjeIzmene
                                                                     }).AsEnumerable();
                                                               
            }

            var total = prijavaData.Count();

            if (sortOrder.Equals("desc"))
                prijavaData = prijavaData.OrderByDescending(s => s.GetType().GetProperty(sortColumn).GetValue(s)).ToList().Skip(skip).Take(pageSize);
            else
                prijavaData = prijavaData.OrderBy(s => s.GetType().GetProperty((sortColumn == "") ? "DatumPoslednjeIzmene" : sortColumn).GetValue(s)).ToList().Skip(skip).Take(pageSize);


            var jsonData = new TableJsonIndexData<PrijavaIndexData>()
            {
                total = total,
                rows = prijavaData
            };


            var jsonResult = Json(jsonData, "TableJsonIndexData", JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;


        }

        public IEnumerable<PrijavaIndexData> GetSearchPrijaveData(string searchTerms, IEnumerable<PrijavaIndexData> searchDataSet)
        {
            string[] terms = searchTerms.Split(',');
            string searchColumn = "";
            string searchTxt = "";

            string tipPrijaveNaziv = "";
            string podTipPrijaveNaziv = "";
            string nacinPrijaveNaziv = "";
            int posiljkaId = 0;
            string prijavioUserNaziv = "";
            string prijavioNaziv = "";
            string prijavioEmail = "";
            string prijavioTelefon = "";
            string opis = "";
            string statusPrijaveNaziv = "";

            var prijave = searchDataSet;


            foreach (string t in terms)
            {
                string[] searchCT = t.Split(':');
                searchColumn = searchCT[0];
                searchTxt = searchCT[1];

                if (!String.IsNullOrEmpty(searchTxt))
                {
                    if (searchColumn.Equals("TipPrijaveNaziv"))
                    {
                        tipPrijaveNaziv = searchTxt;
                        prijave = prijave.Where(k => k.TipPrijaveNaziv.ToUpper().Contains(tipPrijaveNaziv.ToUpper()));
                    }
                    else if (searchColumn.Equals("PodTipPrijaveNaziv"))
                    {
                        podTipPrijaveNaziv = searchTxt;
                        prijave = prijave.Where(k => k.PodTipPrijaveNaziv.ToUpper().Contains(podTipPrijaveNaziv.ToUpper()));
                    }
                    else if (searchColumn.Equals("NacinPrijaveNaziv"))
                    {

                        nacinPrijaveNaziv = searchTxt;
                        prijave = prijave.Where(k => k.NacinPrijaveNaziv.ToUpper().Contains(nacinPrijaveNaziv.ToUpper()));
                    }
                    else if (searchColumn.Equals("DatumPrijave"))
                    {

                        string[] arrDatum = searchTxt.Split('/');
                        DateTime thisDate1 = new DateTime(int.Parse(arrDatum[2]), int.Parse(arrDatum[0]), int.Parse(arrDatum[1]));
                        prijave = prijave.Where(k => k.DatumPrijave == thisDate1);
                    }
                    else if (searchColumn.Equals("PosiljkaId"))
                    {
                        posiljkaId = System.Convert.ToInt32(searchTxt);
                        prijave = prijave.Where(k => k.PosiljkaId == posiljkaId);
                    }
                    //else if (searchColumn.Equals("PrijavioUserNaziv"))
                    //{
                    //    prijavioUserNaziv = searchTxt;
                    //    prijave = prijave.Where(k => k.KorisniciPrograma.Kontakt.Naziv.ToUpper().Contains(prijavioUserNaziv.ToUpper()));
                    //}
                    else if (searchColumn.Equals("PrijavioNaziv"))
                    {
                        prijavioNaziv = searchTxt;
                        prijave = prijave.Where(k => k.PrijavioNaziv.ToUpper().Contains(prijavioNaziv.ToUpper()));
                    }
                    else if (searchColumn.Equals("PrijavioEmail"))
                    {
                        prijavioEmail = searchTxt;
                        prijave = prijave.Where(k => k.PrijavioEmail.ToUpper().Contains(prijavioEmail.ToUpper()));
                    }
                    else if (searchColumn.Equals("PrijavioTelefon"))
                    {
                        prijavioTelefon = searchTxt;
                        prijave = prijave.Where(k => k.PrijavioTelefon.ToUpper().Contains(prijavioTelefon.ToUpper()));
                    }
                    else if (searchColumn.Equals("Opis"))
                    {
                        opis = searchTxt;
                        prijave = prijave.Where(k => k.Opis.ToUpper().Contains(opis.ToUpper()));
                    }
                    else if (searchColumn.Equals("StatusPrijaveNaziv"))
                    {
                        statusPrijaveNaziv = searchTxt;
                        prijave = prijave.Where(k => k.StatusPrijaveNaziv.ToUpper().Contains(statusPrijaveNaziv.ToUpper()));
                    }
                }

            }

            return prijave;
        }

        [ClaimsAuthentication(Resource = "Prijava", Operation = "Create, All")]
        public ActionResult Create()
        {
            ViewBag.TipPrijaveId = new SelectList(BexUow.PrijavaTip.AllAsNoTracking, "Id", "Naziv");
            ViewBag.PodTipPrijaveId = new SelectList(BexUow.PrijavaPodTip.AllAsNoTracking, "Id", "Naziv");
            ViewBag.NacinPrijaveId = new SelectList(BexUow.PrijavaNacin.AllAsNoTracking, "Id", "Naziv", 5);
            ViewBag.StatusPrijaveId = new SelectList(BexUow.PrijavaStatus.AllAsNoTracking, "Id", "Naziv");
            
            return View();
        }

        [ClaimsAuthentication(Resource = "Prijava", Operation = "Edit, All")]
        public ActionResult Edit(int? id)
        {
            ViewBag.TipPrijaveId = new SelectList(BexUow.PrijavaTip.AllAsNoTracking, "Id", "Naziv");
            ViewBag.PodTipPrijaveId = new SelectList(BexUow.PrijavaPodTip.AllAsNoTracking, "Id", "Naziv");
            ViewBag.NacinPrijaveId = new SelectList(BexUow.PrijavaNacin.AllAsNoTracking, "Id", "Naziv");
            ViewBag.StatusPrijaveId = new SelectList(BexUow.PrijavaStatus.AllAsNoTracking, "Id", "Naziv");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PrijavaReklamacijaZalba prijava = BexUow.PrijavaReklamacijaZalba.Find(id);
            if (prijava == null)
            {
                return HttpNotFound();
            }
            return View(prijava);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(PrijavaReklamacijaZalba prijava)
        {
            if (ModelState.IsValid)
            {
                var applicationUser = await SecurityUow.UserManager.FindUserByIdAsync(User.Identity.GetUserId()) as ApplicationUser;
                var korisnikProgramaId = BexUow.KorisniciPrograma.Find(x => x.AspNetUserId == applicationUser.Id).Id;
                prijava.UserIdPoslednjeIzmene = korisnikProgramaId;

                BexUow.PrijavaReklamacijaZalba.Update(prijava);
                var uowCommandResult = BexUow.SubmitChanges();

                if (uowCommandResult.IsSuccessful)
                { return RedirectToAction("../Prijava"); }
                
                //ExceptionSolver.PrepareModelState(ModelState, uowCommandResult);
            }

          

            return View();
        }

        [ChildActionOnly]
        public ActionResult CreateNapomenaPartial(string prijavaId)
        {
            ViewBag.PrijavaId = prijavaId;
            var prijavaNapomena = new PrijavaNapomena
            {
                PrijavaId = System.Convert.ToInt32(prijavaId)
            };
            return PartialView(prijavaNapomena);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateNapomenaPartial(PrijavaNapomena prijavaNapomena)
        {
            if (ModelState.IsValid)
            {
                var applicationUser = await SecurityUow.UserManager.FindUserByIdAsync(User.Identity.GetUserId()) as ApplicationUser;
                var korisnikProgramaId = BexUow.KorisniciPrograma.Find(x => x.AspNetUserId == applicationUser.Id).Id;
                prijavaNapomena.UserUnosaId = korisnikProgramaId;
                prijavaNapomena.DatumUnosa = DateTime.Now;

                BexUow.PrijavaNapomena.Add(prijavaNapomena);
                var uowCommandResult = BexUow.SubmitChanges();

                if (uowCommandResult.IsSuccessful)
                { return PartialView(prijavaNapomena); }

                    //ExceptionSolver.PrepareModelState(ModelState, uowCommandResult);
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        [HttpGet]
        public ActionResult GetPodTipPrijave(int? tipPrijaveId)
        {
            IEnumerable<SelectListItem> podTipoviPrijave = GetPodTipPrijaveData(tipPrijaveId);
            return Json(podTipoviPrijave, JsonRequestBehavior.AllowGet);
        }


        /* punjenje cmb-a podtipovima prijave na osnovu selektovanog tipa prijave */
        public IEnumerable<SelectListItem> GetPodTipPrijaveData(int? tipPrijaveId)
        {

            IEnumerable<SelectListItem> podTipoviPrijave = BexUow.PrijavaPodTip.GetAll(true).Where(x=>x.TipId == tipPrijaveId)
                                                            .Select(x =>
                                                                       new SelectListItem
                                                                       {
                                                                           Value = x.Id.ToString(),
                                                                           Text = x.Naziv
                                                                       }).ToList();
            return new SelectList(podTipoviPrijave, "Value", "Text");

        }

        [HttpGet]
        public ActionResult GetStatusPrijave(int? tipPrijaveId)
        {
            IEnumerable<SelectListItem> statusiPrijave = GetStatusPrijaveData(tipPrijaveId);
            return Json(statusiPrijave, JsonRequestBehavior.AllowGet);
        }

        /* punjenje cmb-a statusom prijave na osnovu selektovanog tipa prijave */
        public IEnumerable<SelectListItem> GetStatusPrijaveData(int? tipPrijaveId)
        {

            IEnumerable<SelectListItem> statusiPrijave = BexUow.PrijavaStatus.GetAll(true).Where(x => x.TipId == tipPrijaveId)
                                                            .Select(x =>
                                                                       new SelectListItem
                                                                       {
                                                                           Value = x.Id.ToString(),
                                                                           Text = x.Naziv
                                                                       }).ToList();
            return new SelectList(statusiPrijave, "Value", "Text");

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(PrijavaReklamacijaZalba prijava)
        {
            if (ModelState.IsValid)
            {
                var applicationUser = await SecurityUow.UserManager.FindUserByIdAsync(User.Identity.GetUserId()) as ApplicationUser;
                var korisnikProgramaId = BexUow.KorisniciPrograma.Find(x => x.AspNetUserId == applicationUser.Id).Id;
                prijava.PrijavioUserId = korisnikProgramaId;
                prijava.DatumPrijave = DateTime.Now;
                BexUow.PrijavaReklamacijaZalba.Add(prijava);
                var commandResult = BexUow.SubmitChanges();

                if (commandResult.IsSuccessful)
                { return RedirectToAction("../Prijava"); }

                //ExceptionSolver.PrepareModelState(ModelState, commandResult);
            }
            
            return View();

        }


        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.PrijavaId = id.Value;

            return View();
        }

        [ClaimsAuthentication(Resource = "Prijava", Operation = "DetailsNapomene, All")]
        [HttpGet]
        public ActionResult GetPrijavaNapomeneData(int? id)
        {

            var detailsNapomene = BexUow.PrijavaNapomena.GetAll(true).Where(x => x.PrijavaId == id)
                                                            .Select(x =>
                                                                       new 
                                                                       {
                                                                           DatumUnosa = x.DatumUnosa,
                                                                           UserUnosa = x.KorisniciPrograma.Kontakt.Naziv,
                                                                           Napomena = x.Napomena
                                                                       }).ToList();

            return Json(detailsNapomene, "", JsonRequestBehavior.AllowGet);
        }

        [ClaimsAuthentication(Resource = "Prijava", Operation = "DetailsLog, All")]
        [HttpGet]
        public ActionResult GetPrijavaLogData(int? id)
        {

            var detailsLog = BexUow.PrijavaReklamacijaZalbaLog.GetAll(true).Where(x => x.PrijavaId == id)
                                                            .Select(x =>
                                                                       new 
                                                                       {
                                                                           PrijavaId = x.Id,
                                                                           TipPrijaveNaziv = x.PrijavaTip.Naziv,
                                                                           PodTipPrijaveNaziv = x.PrijavaPodTip.Naziv,
                                                                           NacinPrijaveNaziv = x.PrijavaNacin.Naziv,
                                                                           PosiljkaId = x.PosiljkaId,
                                                                           PrijavioUserNaziv = x.KorisniciPrograma.Kontakt.Naziv,
                                                                           PrijavioNaziv = x.PrijavioIme + " " + x.PrijavioPrezime,
                                                                           PrijavioEmail = x.PrijavioEmail,
                                                                           PrijavioTelefon = x.PrijavioTelefon,
                                                                           Opis = x.Opis,
                                                                           StatusPrijaveNaziv = x.PrijavaStatus.Naziv,
                                                                           DatumPrijave = x.DatumPrijave,
                                                                           DatumPoslednjeIzmene = x.DatumIzmene,
                                                                           UserIzmene = x.UserIdIzmene.ToString()
                                                                       }).ToList();

            return Json(detailsLog, "", JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            { BexUow.Dispose(); }
            base.Dispose(disposing);
        }
    }
}
