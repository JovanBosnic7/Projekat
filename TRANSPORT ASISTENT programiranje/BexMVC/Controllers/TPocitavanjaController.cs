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

    [ClaimsAuthentication(Resource = "TPocitavanja", Operation = "Read, All")]
    public class TPocitavanjaController : Controller
    {
        public TPocitavanjaController() : this(new SecurityUow(System.Web.HttpContext.Current.GetOwinContext()), new BexUow())
        { }
        public TPocitavanjaController(
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
            ViewBag.ModelId = new SelectList(BexUow.VozniParkModel.AllAsNoTracking, "NazivModela", "NazivModela");  
            ViewBag.BojaId = new SelectList(BexUow.VozniParkBoja.AllAsNoTracking, "NazivBoje", "NazivBoje");
            List<SelectListItem> listKategorija = BexUow.VozniParkKategorija.AllAsNoTracking.Select(x =>
                                                        new SelectListItem
                                                        {
                                                            Text = x.KategorijaNaziv + " " + x.NazivPodkategorije,
                                                            Value = x.Id.ToString()
                                                        }).ToList();
            ViewBag.KategorijaVozilaId = new SelectList(listKategorija, "Value", "Text");
            ViewBag.KaroserijaVozilaId = new SelectList(BexUow.VozniParkKaroserija.AllAsNoTracking.OrderBy(k => k.NazivKaroserije), "NazivKaroserije", "NazivKaroserije");
            //ViewBag.KontrolorId = new SelectList(BexUow.Zaposleni.AllAsNoTracking.Where(k => k.RadnoMestoId == 11 & k.Aktivan == true), "NazivZaposlenog", "NazivZaposlenog");
            //ViewBag.Marka = new SelectList(BexUow.VozniParkMarka.AllAsNoTracking, "NazivMarke", "NazivMarke");
            //ViewBag.VpOprema = BexUow.VozniParkDnevnikTip.AllAsNoTracking.Where(x => x.GrupaId == 1).ToList();
            //ViewBag.StatusId = (statusId == null) ? 0 : statusId;
            //ViewBag.LokacijaId = (lokacijaId == null) ? 0 : lokacijaId;
            return View();

        }

        [HttpGet]
        public ActionResult GetTPocitavanja (int pageSize, int pageNumber, string sortColumn, string sortOrder, string search, string searchColumn, string searchTerms)
        {

            var skip = (pageNumber - 1) * pageSize;

            var tpOcitavanjaData = BexUow.TPocitavanja.GetAll().Select(x =>
                                                         new TPocitavanjaIndexData
                                                         {
                                                             Id = x.Id,
                                                             DatumOcitavanja = x.DatumOcitavanja,
                                                             BrojSasije = x.BrojSasije,
                                                             BrojMotora = x.BrojMotora,
                                                             KategorijaVozila = x.KategorijaVozila.KategorijaNaziv,
                                                             KaroserijaVozila = x.KaroserijaVozila.NazivKaroserije,
                                                             Model = x.ModelVozila.VozniParkMarka.NazivMarke + " " + x.ModelVozila.NazivModela,
                                                             Boja = x.BojaVozila.NazivBoje,
                                                             BrojVrata = x.BrojVrata,
                                                             Kontrolor = x.Kontrolor.Kontakt.Naziv,
                                                             Uneo = x.UserUnosa.Kontakt.Naziv,
                                                             Napomena = x.Napomena
                                                            }).AsEnumerable();

            if (!String.IsNullOrEmpty(searchTerms))
            {
                tpOcitavanjaData = GetSearchTPocitavanjaData(searchTerms).AsEnumerable();
            }

            var total = tpOcitavanjaData.Count();

            if (sortColumn == "")
            {
                sortColumn = "Id";
                sortOrder = "desc";
            }

            if (sortOrder.Equals("desc"))
                tpOcitavanjaData = tpOcitavanjaData.OrderByDescending(s => s.GetType().GetProperty((sortColumn == "") ? "Id" : sortColumn).GetValue(s)).ToList().Skip(skip).Take(pageSize);
            else
                tpOcitavanjaData = tpOcitavanjaData.OrderBy(s => s.GetType().GetProperty((sortColumn == "") ? "Id" : sortColumn).GetValue(s)).ToList().Skip(skip).Take(pageSize);

            //var tocenje = BexUow.GorivoTocenje.AllAsNoTracking.Where(x => x.PutniNalog.VpId == putniNalog.VpId && x.Storno == false).OrderByDescending(z => z.Datum).ThenByDescending(z => z.Vreme).Take(1).FirstOrDefault();

            var jsonData = new TableJsonIndexData<TPocitavanjaIndexData>()
            {
                total = total,
                rows = tpOcitavanjaData
            };
            var jsonResult = Json(jsonData, "TableJsonIndexData", JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;

        }

        public IEnumerable<TPocitavanjaIndexData> GetSearchTPocitavanjaData(string searchTerms)
        {
            var tpOcitavanjaDataSet = BexUow.TPocitavanja.AllAsNoTracking
                                                .Where(x => x.Storno == false)
                                                .Select(x =>
                                new TPocitavanjaIndexData
                                {
                                    Id = x.Id,
                                    DatumOcitavanja = x.DatumOcitavanja,
                                    BrojSasije = x.BrojSasije,
                                    BrojMotora = x.BrojMotora,
                                    KategorijaVozila = x.KategorijaVozila.KategorijaNaziv,
                                    KaroserijaVozila =x.KaroserijaVozila.NazivKaroserije,
                                    Model = x.ModelVozila.VozniParkMarka.NazivMarke + " " + x.ModelVozila.NazivModela,
                                    Boja = x.BojaVozila.NazivBoje,
                                    BrojVrata = x.BrojVrata,
                                    Kontrolor = x.Kontrolor.Kontakt.Naziv,
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


                if (searchColumn.Equals("DatumOcitavanja") && !String.IsNullOrEmpty(searchTxt))
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

                    tpOcitavanjaDataSet = tpOcitavanjaDataSet.Where(k => k.DatumOcitavanja >= datumFirst && k.DatumOcitavanja <= datumSecond);

                }
                else if (searchColumn.Equals("KategorijaVozila") && !String.IsNullOrEmpty(searchTxt))
                {
                    tpOcitavanjaDataSet = tpOcitavanjaDataSet.Where(k => k.KategorijaVozila.Contains(searchTxt));
                }
                else if (searchColumn.Equals("KaroserijaVozila") && !String.IsNullOrEmpty(searchTxt))
                {
                    tpOcitavanjaDataSet = tpOcitavanjaDataSet.Where(k => k.KaroserijaVozila.Contains(searchTxt));
                }
                else if (searchColumn.Equals("Napomena") && !String.IsNullOrEmpty(searchTxt))
                {
                    tpOcitavanjaDataSet = tpOcitavanjaDataSet.Where(k => k.Napomena.Contains(searchTxt));
                }
                else if (searchColumn.Equals("Model") && !String.IsNullOrEmpty(searchTxt))
                {
                    tpOcitavanjaDataSet = tpOcitavanjaDataSet.Where(k => k.Model.Contains(searchTxt));
                }
                else if (searchColumn.Equals("BrojSasije") && !String.IsNullOrEmpty(searchTxt))
                {
                    tpOcitavanjaDataSet = tpOcitavanjaDataSet.Where(k => k.BrojSasije.Contains(searchTxt));
                }
                else if (searchColumn.Equals("BrojMotora") && !String.IsNullOrEmpty(searchTxt))
                {
                    tpOcitavanjaDataSet = tpOcitavanjaDataSet.Where(k => k.BrojMotora.Contains(searchTxt));
                }
                else if (searchColumn.Equals("Boja") && !String.IsNullOrEmpty(searchTxt))
                {
                    tpOcitavanjaDataSet = tpOcitavanjaDataSet.Where(k => k.Boja.Contains(searchTxt));
                }
                else if (searchColumn.Equals("BrojVrata") && !String.IsNullOrEmpty(searchTxt))
                {
                    tpOcitavanjaDataSet = tpOcitavanjaDataSet.Where(k => k.BrojVrata.Equals(searchTxt));
                }
                else if (searchColumn.Equals("Kontrolor") && !String.IsNullOrEmpty(searchTxt))
                {
                    tpOcitavanjaDataSet = tpOcitavanjaDataSet.Where(k => k.Kontrolor.Contains(searchTxt));
                }
                else if (searchColumn.Equals("Uneo") && !String.IsNullOrEmpty(searchTxt))
                {
                    tpOcitavanjaDataSet = tpOcitavanjaDataSet.Where(k => k.Uneo.Contains(searchTxt));
                }
                


            }
            return tpOcitavanjaDataSet;
        }



        [ClaimsAuthentication(Resource = "TPocitavanja", Operation = "Create, All")]
        public ActionResult Create()
        {
            //ViewBag.ModelId = new SelectList(BexUow.VozniParkModel.AllAsNoTracking, "Id", "NazivModela");
            ViewBag.BojaId = new SelectList(BexUow.VozniParkBoja.AllAsNoTracking, "Id", "NazivBoje");
            List<SelectListItem> listKategorija = BexUow.VozniParkKategorija.AllAsNoTracking.Select(x =>
                                                        new SelectListItem
                                                        {
                                                            Text = (x.NazivPodkategorije != x.KategorijaNaziv ? x.KategorijaNaziv + " " + x.NazivPodkategorije : x.KategorijaNaziv),
                                                            Value = x.Id.ToString()
                                                        }).ToList();
            ViewBag.KategorijaVozilaId = new SelectList(listKategorija, "Value", "Text");
            ViewBag.KaroserijaVozilaId = new SelectList(BexUow.VozniParkKaroserija.AllAsNoTracking.Where(k => k.Id >= 6 && k.Id <= 11).OrderBy(k => k.NazivKaroserije), "Id", "NazivKaroserije");
            //ViewBag.KontrolorId = new SelectList(BexUow.Zaposleni.AllAsNoTracking.Where(k=> k.RadnoMestoId == 11 & k.Aktivan == true).Join(Kontakt, "Id", "NazivZaposlenog");

            return View();
        }

        //// POST: VozniPark/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(TPocitavanja tpOcitavanja)
        {


            if (ModelState.IsValid)
            {
                //tpTermini.ZatvorenPutniNalog = (putniNalog.KmStart < putniNalog.KmStop) ? true : false;
                var applicationUser = await SecurityUow.UserManager.FindUserByIdAsync(User.Identity.GetUserId()) as ApplicationUser;
                var korisnikProgramaId = BexUow.KorisniciPrograma.Find(x => x.AspNetUserId == applicationUser.Id).Id;
                tpOcitavanja.UserUneoId = korisnikProgramaId;
                tpOcitavanja.Storno = false;
                BexUow.TPocitavanja.Add(tpOcitavanja);
                var commandResult = BexUow.SubmitChanges();

                if (commandResult.IsSuccessful)
                { return RedirectToAction("Index", "TPocitavanja"); }

                ExceptionSolver.PrepareModelState(ModelState, commandResult);
            }


            return View();
            //return View(tpTermini);

        }

        [ClaimsAuthentication(Resource = "TPocitavanja", Operation = "Edit, All")]
        public ActionResult Edit(int id)
        {

            TPocitavanja tpOcitavanja = BexUow.TPocitavanja.Find(id);
            if (tpOcitavanja == null)
            {
                return HttpNotFound();
            }
            
            ViewBag.BojaId = new SelectList(BexUow.VozniParkBoja.AllAsNoTracking, "Id", "NazivBoje", tpOcitavanja.BojaId);
            List<SelectListItem> listKategorija = BexUow.VozniParkKategorija.AllAsNoTracking.Select(x =>
                                                        new SelectListItem
                                                        {
                                                            //Text = x.KategorijaNaziv + " " + x.NazivPodkategorije,
                                                            Text = (x.NazivPodkategorije != x.KategorijaNaziv ? x.KategorijaNaziv + " " + x.NazivPodkategorije : x.KategorijaNaziv),
                                                            Value = x.Id.ToString()
                                                        }).ToList();
            ViewBag.KategorijaVozilaId = new SelectList(listKategorija, "Value", "Text", tpOcitavanja.KategorijaVozilaId);
            ViewBag.ModelNaziv = BexUow.VozniParkModel.Find(x => x.Id == tpOcitavanja.ModelId).NazivModela;
            //ViewBag.KaroserijaVozilaId = new SelectList(BexUow.VozniParkKaroserija.AllAsNoTracking.OrderBy(k => k.NazivKaroserije), "Id", "NazivKaroserije", tpOcitavanja.KaroserijaVozilaId);
            ViewBag.KaroserijaVozilaId = new SelectList(BexUow.VozniParkKaroserija.AllAsNoTracking.Where(k => k.Id >= 6 && k.Id <= 11).OrderBy(k => k.NazivKaroserije), "Id", "NazivKaroserije", tpOcitavanja.KaroserijaVozilaId);

            return View(tpOcitavanja);
        }

        // POST: VozniPark/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(TPocitavanja tpOcitavanja)
        {
            if (ModelState.IsValid)
            {
                var applicationUser = await SecurityUow.UserManager.FindUserByIdAsync(User.Identity.GetUserId()) as ApplicationUser;
                var korisnikProgramaId = BexUow.KorisniciPrograma.Find(x => x.AspNetUserId == applicationUser.Id).Id;
                tpOcitavanja.UserUneoId = korisnikProgramaId;
                tpOcitavanja.Storno = false;
                BexUow.TPocitavanja.Update(tpOcitavanja);
                var uowCommandResult = BexUow.SubmitChanges();

                if (uowCommandResult.IsSuccessful)
                { return RedirectToAction("Index", "TPocitavanja"); }

                ExceptionSolver.PrepareModelState(ModelState, uowCommandResult);
            }

            return View(tpOcitavanja);
        }

        //[HttpGet]
        //public JsonResult GetStatus()
        //{
        //    var statusList = BexUow.TPstatusTermina.AllAsNoTracking
        //                                    .Select(a => new
        //                                    {
        //                                        Value = a.NazivStatusa
        //                                    }
        //                                    ).ToList();

        //    return Json(statusList, "", JsonRequestBehavior.AllowGet);
        //}
        //[HttpGet]
        //public JsonResult GetLokacija()
        //{
        //    var lokacijaList = BexUow.TPlokacije.AllAsNoTracking
        //                                    .Select(a => new
        //                                    {
        //                                        Value = a.NazivLokacije
        //                                    }
        //                                    ).ToList();

        //    return Json(lokacijaList, "", JsonRequestBehavior.AllowGet);
        //}

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
