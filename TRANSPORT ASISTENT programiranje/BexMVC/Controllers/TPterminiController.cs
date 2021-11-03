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
using AspNet.DAL.EF.UOW.Security;
using System.Web;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using AspNet.DAL.EF.Models.Security;
using Bex.Common.Interfaces;

namespace BexMVC.Controllers
{

    [ClaimsAuthentication(Resource = "TPtermini", Operation = "Read, All")]
    public class TPterminiController : Controller
    {
        public TPterminiController() : this(new SecurityUow(System.Web.HttpContext.Current.GetOwinContext()), new BexUow())
        { }
        public TPterminiController(
          ISecurityUow securityUow, IBexUow bexUow)
        {
            SecurityUow = securityUow;
            BexUow = bexUow;

        }
        public ActionResult Index(int? statusId, int? lokacijaId)
        {
            TempData.ToList().ForEach(keyValue =>
                ModelState.AddModelError("", keyValue.Value.ToString()));
            TempData.Clear();
            //ViewBag.Model = new SelectList(BexUow.VozniParkModel.AllAsNoTracking, "NazivModela", "NazivModela");
            //ViewBag.Marka = new SelectList(BexUow.VozniParkMarka.AllAsNoTracking, "NazivMarke", "NazivMarke");
            ViewBag.StatusTermina = new SelectList(BexUow.TPstatusTermina.AllAsNoTracking, "NazivStatusa", "NazivStatusa");
            //ViewBag.Kategorija = new SelectList(BexUow.VozniParkKategorija.AllAsNoTracking, "KategorijaNaziv", "KategorijaNaziv");
            ViewBag.LokacijaTP = new SelectList(BexUow.TPlokacije.AllAsNoTracking, "NazivLokacije", "NazivLokacije");
            //ViewBag.VpOprema = BexUow.VozniParkDnevnikTip.AllAsNoTracking.Where(x => x.GrupaId == 1).ToList();
            ViewBag.StatusId = (statusId == null) ? 0 : statusId;
            ViewBag.LokacijaId = (lokacijaId == null) ? 0 : lokacijaId;
            return View();

        }

        [HttpGet]
        public ActionResult GetTPtermini(int pageSize, int pageNumber, string sortColumn, string sortOrder, string search, string searchColumn, string searchTerms)
        {

            var skip = (pageNumber - 1) * pageSize;

            var tpTerminiData = BexUow.TPtermini.GetAll().Select(x =>
                                                         new TPterminiIndexData
                                                         {
                                                             Id = x.Id,
                                                             DatumTermina = x.DatumTermina,
                                                             //VremeTermina = x.TerminStart.ToString() + " - " + x.TerminEnd.ToString(),
                                                             TerminOd = x.TerminStart.ToString(),
                                                             TerminDo = x.TerminEnd.ToString(),
                                                             LokacijaTP = x.LokacijaTP.NazivLokacije,
                                                             Ime = x.ImeKlijenta,
                                                             Prezime = x.PrezimeKlijenta,
                                                             Telefon = x.TelefonKlijenta,
                                                             RegOznaka = x.RegOznaka,
                                                             Kategorija = x.KategorijaVozila.KategorijaNaziv,
                                                             Model = x.ModelVozila.VozniParkMarka.NazivMarke + " " + x.ModelVozila.NazivModela,
                                                             StatusTermina = x.StatusTermina.NazivStatusa,
                                                             Uneo = x.UserUnosa.Kontakt.Naziv,
                                                             Napomena = x.Napomena
                                                         }).AsEnumerable();

            if (!String.IsNullOrEmpty(searchTerms))
            {
                tpTerminiData = GetSearchTPterminiData(searchTerms).AsEnumerable();
            }

            var total = tpTerminiData.Count();

            if (sortColumn == "")
            {
                sortColumn = "DatumTermina";
                sortOrder = "desc";
            }

            if (sortOrder.Equals("desc"))
                tpTerminiData = tpTerminiData.OrderByDescending(s => s.GetType().GetProperty((sortColumn == "") ? "DatumTermina" : sortColumn).GetValue(s)).ThenBy(s => s.TerminOd).ThenBy(s => s.LokacijaTP).ToList().Skip(skip).Take(pageSize);
            else
                tpTerminiData = tpTerminiData.OrderBy(s => s.GetType().GetProperty((sortColumn == "") ? "DatumTermina" : sortColumn).GetValue(s)).ThenBy(s => s.TerminOd).ThenBy(s => s.LokacijaTP).ToList().Skip(skip).Take(pageSize);

            //var tocenje = BexUow.GorivoTocenje.AllAsNoTracking.Where(x => x.PutniNalog.VpId == putniNalog.VpId && x.Storno == false).OrderByDescending(z => z.Datum).ThenByDescending(z => z.Vreme).Take(1).FirstOrDefault();

            var jsonData = new TableJsonIndexData<TPterminiIndexData>()
            {
                total = total,
                rows = tpTerminiData
            };
            var jsonResult = Json(jsonData, "TableJsonIndexData", JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;

        }

        public IEnumerable<TPterminiIndexData> GetSearchTPterminiData(string searchTerms)
        {
            var tpTerminiDataSet = BexUow.TPtermini.AllAsNoTracking
                                                .Where(x => x.Storno == false)
                                                .Select(x =>
                                new TPterminiIndexData
                                {
                                    Id = x.Id,
                                    DatumTermina = x.DatumTermina,
                                    //VremeTermina = x.TerminStart.ToString() + " - " + x.TerminEnd.ToString(),
                                    TerminOd = x.TerminStart.ToString(),
                                    TerminDo = x.TerminEnd.ToString(),
                                    LokacijaTP = x.LokacijaTP.NazivLokacije,
                                    Ime = x.ImeKlijenta,
                                    Prezime = x.PrezimeKlijenta,
                                    Telefon = x.TelefonKlijenta,
                                    RegOznaka = x.RegOznaka,
                                    Kategorija = x.KategorijaVozila.KategorijaNaziv,
                                    Model = x.ModelVozila.VozniParkMarka.NazivMarke + " " + x.ModelVozila.NazivModela,
                                    StatusTermina = x.StatusTermina.NazivStatusa,
                                    Uneo = x.UserUnosa.Kontakt.Naziv,
                                    Napomena = x.Napomena
                                });

            string[] terms = searchTerms.Split(',');
            foreach (string t in terms)
            {
                string[] searchCT = t.Split(':');

                string searchColumn = "";
                string searchTxt = "";


                searchColumn = searchCT[0];
                searchTxt = searchCT[1];


                if (searchColumn.Equals("DatumTermina") && !String.IsNullOrEmpty(searchTxt))
                {
                    //string[] arrDatum = searchTxt.Split('/');
                    //DateTime thisDate1 = new DateTime(int.Parse(arrDatum[2]), int.Parse(arrDatum[0]), int.Parse(arrDatum[1]));
                    //2019-01-10 to 2019-01-10
                    DateTime datumFirst, datumSecond;
                    string[] arrDatumAll = searchTxt.Split('t', 'o');

                    string[] arrDatumFirst = arrDatumAll[0].Trim().Split('-');
                    datumFirst = new DateTime(int.Parse(arrDatumFirst[0]), int.Parse(arrDatumFirst[1]), int.Parse(arrDatumFirst[2]));

                    string[] arrDatumSecond = arrDatumAll[2].Trim().Split('-');
                    datumSecond = new DateTime(int.Parse(arrDatumSecond[0]), int.Parse(arrDatumSecond[1]), int.Parse(arrDatumSecond[2]));

                    tpTerminiDataSet = tpTerminiDataSet.Where(k => k.DatumTermina >= datumFirst && k.DatumTermina <= datumSecond);

                }
                else if (searchColumn.Equals("TerminOd") && !String.IsNullOrEmpty(searchTxt))
                {
                    tpTerminiDataSet = tpTerminiDataSet.Where(k => k.TerminOd.Contains(searchTxt));
                }
                else if (searchColumn.Equals("TerminDo") && !String.IsNullOrEmpty(searchTxt))
                {
                    tpTerminiDataSet = tpTerminiDataSet.Where(k => k.TerminDo.Contains(searchTxt));
                }
                else if (searchColumn.Equals("Ime") && !String.IsNullOrEmpty(searchTxt))
                {
                    tpTerminiDataSet = tpTerminiDataSet.Where(k => k.Ime.Contains(searchTxt));
                }
                else if (searchColumn.Equals("Prezime") && !String.IsNullOrEmpty(searchTxt))
                {
                    tpTerminiDataSet = tpTerminiDataSet.Where(k => k.Prezime.Contains(searchTxt));
                }
                else if (searchColumn.Equals("Telefon") && !String.IsNullOrEmpty(searchTxt))
                {
                    tpTerminiDataSet = tpTerminiDataSet.Where(k => k.Telefon.Contains(searchTxt));
                }
                else if (searchColumn.Equals("RegOznaka") && !String.IsNullOrEmpty(searchTxt))
                {
                    tpTerminiDataSet = tpTerminiDataSet.Where(k => k.RegOznaka.Contains(searchTxt));
                }
                else if (searchColumn.Equals("Uneo") && !String.IsNullOrEmpty(searchTxt))
                {
                    tpTerminiDataSet = tpTerminiDataSet.Where(k => k.Uneo.Contains(searchTxt));
                }
                else if (searchColumn.Equals("StatusTermina") && !String.IsNullOrEmpty(searchTxt))
                {
                    tpTerminiDataSet = tpTerminiDataSet.Where(k => k.StatusTermina.Contains(searchTxt));
                }
                else if (searchColumn.Equals("LokacijaTP") && !String.IsNullOrEmpty(searchTxt))
                {
                    tpTerminiDataSet = tpTerminiDataSet.Where(k => k.LokacijaTP.Contains(searchTxt));
                }
                else if (searchColumn.Equals("Kategorija") && !String.IsNullOrEmpty(searchTxt))
                {
                    tpTerminiDataSet = tpTerminiDataSet.Where(k => k.Kategorija.Contains(searchTxt));
                }
                else if (searchColumn.Equals("Napomena") && !String.IsNullOrEmpty(searchTxt))
                {
                    tpTerminiDataSet = tpTerminiDataSet.Where(k => k.Napomena.Contains(searchTxt));
                }
                else if (searchColumn.Equals("Model") && !String.IsNullOrEmpty(searchTxt))
                {
                    tpTerminiDataSet = tpTerminiDataSet.Where(k => k.Model.Contains(searchTxt));
                }

            }
            return tpTerminiDataSet;
        }



        [ClaimsAuthentication(Resource = "TPtermini", Operation = "Create, All")]
        public ActionResult Create()
        {
            ViewBag.LokacijaId = new SelectList(BexUow.TPlokacije.AllAsNoTracking, "IdLokacije", "NazivLokacije");
            ViewBag.StatusTerminaId = new SelectList(BexUow.TPstatusTermina.AllAsNoTracking, "IdStatusa", "NazivStatusa");
            List<SelectListItem> listKategorija = BexUow.VozniParkKategorija.AllAsNoTracking.Select(x =>
                                                        new SelectListItem
                                                        {
                                                            Text = (x.NazivPodkategorije != x.KategorijaNaziv ? x.KategorijaNaziv + " " + x.NazivPodkategorije : x.KategorijaNaziv),
                                                            Value = x.Id.ToString()
                                                        }).ToList();
            ViewBag.KategorijaVozilaId = new SelectList(listKategorija, "Value", "Text");
            ViewBag.ModelId = new SelectList(BexUow.VozniParkModel.AllAsNoTracking, "Id", "NazivModela");

            return View();
        }

        //// POST: VozniPark/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(TPtermini tpTermini)
        {


            if (ModelState.IsValid)
            {
                //tpTermini.ZatvorenPutniNalog = (putniNalog.KmStart < putniNalog.KmStop) ? true : false;
                var applicationUser = await SecurityUow.UserManager.FindUserByIdAsync(User.Identity.GetUserId()) as ApplicationUser;
                var korisnikProgramaId = BexUow.KorisniciPrograma.Find(x => x.AspNetUserId == applicationUser.Id).Id;
                tpTermini.UserUneoId = korisnikProgramaId;
                tpTermini.Storno = false;
                BexUow.TPtermini.Add(tpTermini);
                var commandResult = BexUow.SubmitChanges();

                if (commandResult.IsSuccessful)
                { return RedirectToAction("Index", "TPtermini"); }

                ExceptionSolver.PrepareModelState(ModelState, commandResult);
            }


            return View();
            //return View(tpTermini);

        }

        [ClaimsAuthentication(Resource = "TPtermini", Operation = "Edit, All")]
        public ActionResult Edit(int id)
        {

            TPtermini tpTermini = BexUow.TPtermini.Find(id);
            if (tpTermini == null)
            {
                return HttpNotFound();
            }
            //ViewBag.VoziloId = id;
            //ViewBag.VoziloRegOznaka = vozniPark.Oznaka;

            ViewBag.LokacijaId = new SelectList(BexUow.TPlokacije.AllAsNoTracking, "IdLokacije", "NazivLokacije", tpTermini.LokacijaId);
            ViewBag.StatusTerminaId = new SelectList(BexUow.TPstatusTermina.AllAsNoTracking, "IdStatusa", "NazivStatusa", tpTermini.StatusTerminaId);
            List<SelectListItem> listKategorija = BexUow.VozniParkKategorija.AllAsNoTracking.Select(x =>
                                                        new SelectListItem
                                                        {
                                                            //Text = x.KategorijaNaziv + " " + x.NazivPodkategorije,
                                                            Text = (x.NazivPodkategorije != x.KategorijaNaziv ? x.KategorijaNaziv + " " + x.NazivPodkategorije : x.KategorijaNaziv),
                                                            Value = x.Id.ToString()
                                                        }).ToList();
            ViewBag.KategorijaVozilaId = new SelectList(listKategorija, "Value", "Text", tpTermini.KategorijaVozilaId);
            if (tpTermini.ModelId > 0)
            {
                ViewBag.ModelNaziv = BexUow.VozniParkModel.Find(x => x.Id == tpTermini.ModelId).NazivModela;
            }

            return View(tpTermini);
        }

        // POST: VozniPark/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(TPtermini tpTermini)
        {
            if (ModelState.IsValid)
            {
                var applicationUser = await SecurityUow.UserManager.FindUserByIdAsync(User.Identity.GetUserId()) as ApplicationUser;
                var korisnikProgramaId = BexUow.KorisniciPrograma.Find(x => x.AspNetUserId == applicationUser.Id).Id;
                tpTermini.UserUneoId = korisnikProgramaId;
                tpTermini.Storno = false;
                BexUow.TPtermini.Update(tpTermini);
                var uowCommandResult = BexUow.SubmitChanges();

                if (uowCommandResult.IsSuccessful)
                { return RedirectToAction("Index", "TPtermini"); }

                ExceptionSolver.PrepareModelState(ModelState, uowCommandResult);
            }

            return View(tpTermini);
        }
        [ClaimsAuthentication(Resource = "TPtermini", Operation = "ClickZakljuciDan, All")]
        public ActionResult ZakljuciDan()
        {
            ViewBag.LokacijaId = new SelectList(BexUow.TPlokacije.AllAsNoTracking, "IdLokacije", "NazivLokacije");
            ViewBag.IznosKes = 0;
            ViewBag.IznosKartica = 0;
            ViewBag.IznosCek = 0;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ZakljuciDan(TPzakljuciDanIndexData tpZakljuciDan)
        {
            var applicationUser = await SecurityUow.UserManager.FindUserByIdAsync(User.Identity.GetUserId()) as ApplicationUser;
            var korisnikProgramaId = BexUow.KorisniciPrograma.Find(x => x.AspNetUserId == applicationUser.Id).Id;
            tpZakljuciDan.UserId = korisnikProgramaId;

            BexUow.TPzakljuci.ZakljuciDan(tpZakljuciDan.IznosKes, tpZakljuciDan.IznosKartica, tpZakljuciDan.IznosCek, tpZakljuciDan.LokacijaId, korisnikProgramaId);

            return RedirectToAction("Index", "TPtermini");
        }

        [HttpGet]
        public JsonResult GetStatus()
        {
            var statusList = BexUow.TPstatusTermina.AllAsNoTracking
                                            .Select(a => new
                                            {
                                                Value = a.NazivStatusa
                                            }
                                            ).ToList();

            return Json(statusList, "", JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetLokacija()
        {
            var lokacijaList = BexUow.TPlokacije.AllAsNoTracking
                                            .Select(a => new
                                            {
                                                Value = a.NazivLokacije
                                            }
                                            ).ToList();

            return Json(lokacijaList, "", JsonRequestBehavior.AllowGet);
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
