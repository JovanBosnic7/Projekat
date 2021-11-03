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

    [ClaimsAuthentication(Resource = "Stete", Operation = "Read, All")]
    public class SteteController : Controller
    {
        public SteteController() : this(new SecurityUow(System.Web.HttpContext.Current.GetOwinContext()), new BexUow())
        { }
        public SteteController(
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
            //ViewBag.ModelId = new SelectList(BexUow.VozniParkModel.AllAsNoTracking, "NazivModela", "NazivModela");  
            //ViewBag.BojaId = new SelectList(BexUow.VozniParkBoja.AllAsNoTracking, "NazivBoje", "NazivBoje");
            //List<SelectListItem> listKategorija = BexUow.VozniParkKategorija.AllAsNoTracking.Select(x =>
            //                                            new SelectListItem
            //                                            {
            //                                                Text = x.KategorijaNaziv + " " + x.NazivPodkategorije,
            //                                                Value = x.Id.ToString()
            //                                            }).ToList();
            //ViewBag.KategorijaVozilaId = new SelectList(listKategorija, "Value", "Text");
            //ViewBag.KaroserijaVozilaId = new SelectList(BexUow.VozniParkKaroserija.AllAsNoTracking.OrderBy(k => k.NazivKaroserije), "NazivKaroserije", "NazivKaroserije");
            //ViewBag.KontrolorId = new SelectList(BexUow.Zaposleni.AllAsNoTracking.Where(k => k.RadnoMestoId == 11 & k.Aktivan == true), "NazivZaposlenog", "NazivZaposlenog");
            //ViewBag.Marka = new SelectList(BexUow.VozniParkMarka.AllAsNoTracking, "NazivMarke", "NazivMarke");
            //ViewBag.VpOprema = BexUow.VozniParkDnevnikTip.AllAsNoTracking.Where(x => x.GrupaId == 1).ToList();
            //ViewBag.StatusId = (statusId == null) ? 0 : statusId;
            //ViewBag.LokacijaId = (lokacijaId == null) ? 0 : lokacijaId;
            return View();

        }

        //[HttpGet]
        //public ActionResult GetStete(int pageSize, int pageNumber, string sortColumn, string sortOrder, string search, string searchColumn, string searchTerms)
        //{

        //    var skip = (pageNumber - 1) * pageSize;

        //    var steteData = BexUow.VozniParkSteta.GetAll().Select(x =>
        //                                                 new StetaIndexData
        //                                                 {
        //                                                     Id = x.Id,
        //                                                     DatumUnosa = x.DatumUnosa,
        //                                                     FirmaStete = x.Firma.Naziv,
        //                                                     NalogSektor = x.StetaKategorija.Naziv,
        //                                                     NalogIzdao = x.KorisnikIzdao.Kontakt.Naziv,
        //                                                     StetocinaZaposleni = x.StetocinaZaposleni.Kontakt.Naziv,
        //                                                     StetocinaCentar = x.StetocinaCentar.NazivSkraceni,
        //                                                     Vozilo = x.VozniPark.Oznaka,
        //                                                     StetaTip = x.StetaTip.NazivTipa,
        //                                                     Opis = x.Opis,
        //                                                     Napomena = x.Napomena,
        //                                                     IznosRsd = x.IznosRsd,
        //                                                     IznosZaNaplatu = x.IznosZaNaplatu,
        //                                                     DatumPredajePravnojSluzbi = x.DatumPredajePravnoj,
        //                                                     DatumOdluke = x.DatumOdluke
        //                                                 }).AsEnumerable();

        //    if (!String.IsNullOrEmpty(searchTerms))
        //    {
        //        steteData = GetSearchSteteData(searchTerms).AsEnumerable();
        //    }

        //    var total = steteData.Count();

        //    if (sortColumn == "")
        //    {
        //        sortColumn = "Id";
        //        sortOrder = "desc";
        //    }

        //    if (sortOrder.Equals("desc"))
        //        steteData = steteData.OrderByDescending(s => s.GetType().GetProperty((sortColumn == "") ? "Id" : sortColumn).GetValue(s)).ToList().Skip(skip).Take(pageSize);
        //    else
        //        steteData = steteData.OrderBy(s => s.GetType().GetProperty((sortColumn == "") ? "Id" : sortColumn).GetValue(s)).ToList().Skip(skip).Take(pageSize);

        //    //var tocenje = BexUow.GorivoTocenje.AllAsNoTracking.Where(x => x.PutniNalog.VpId == putniNalog.VpId && x.Storno == false).OrderByDescending(z => z.Datum).ThenByDescending(z => z.Vreme).Take(1).FirstOrDefault();

        //    var jsonData = new TableJsonIndexData<StetaIndexData>()
        //    {
        //        total = total,
        //        rows = steteData
        //    };
        //    var jsonResult = Json(jsonData, "TableJsonIndexData", JsonRequestBehavior.AllowGet);
        //    jsonResult.MaxJsonLength = int.MaxValue;
        //    return jsonResult;

        //}

        public IEnumerable<StetaIndexData> GetSearchSteteData(string searchTerms)
        {
            var steteDataSet = BexUow.VozniParkSteta.AllAsNoTracking
                                                .Where(x => x.Storno == false)
                                                .Select(x =>
                                new StetaIndexData
                                {
                                    Id = x.Id,
                                    DatumUnosa = x.DatumUnosa,
                                    FirmaStete = x.Firma.Naziv,
                                    NalogSektor = x.StetaKategorija.Naziv,
                                    NalogIzdao = x.KorisnikIzdao.Kontakt.Naziv,
                                    StetocinaZaposleni = x.StetocinaZaposleni.Kontakt.Naziv,
                                    StetocinaCentar = x.StetocinaCentar.NazivSkraceni,
                                    Vozilo = x.VozniPark.Oznaka,
                                    StetaTip = x.StetaTip.NazivTipa,
                                    Opis = x.Opis,
                                    Napomena = x.Napomena,
                                    IznosRsd = x.IznosRsd,
                                    IznosZaNaplatu = x.IznosZaNaplatu,
                                    DatumPredajePravnojSluzbi = x.DatumPredajePravnoj,
                                    DatumOdluke = x.DatumOdluke
                                });

            string[] terms = searchTerms.Split(',');
            foreach (string t in terms)
            {
                string[] searchCT = t.Split(':');

                string searchColumn = "";
                string searchTxt = "";


                searchColumn = searchCT[0];
                searchTxt = searchCT[1];


                if (searchColumn.Equals("DatumOdluke") && !String.IsNullOrEmpty(searchTxt))
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

                    steteDataSet = steteDataSet.Where(k => k.DatumOdluke >= datumFirst && k.DatumOdluke <= datumSecond);

                }
                else if (searchColumn.Equals("DatumPredajePravnojSluzbi") && !String.IsNullOrEmpty(searchTxt))
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

                    steteDataSet = steteDataSet.Where(k => k.DatumPredajePravnojSluzbi >= datumFirst && k.DatumPredajePravnojSluzbi <= datumSecond);

                }
                else if (searchColumn.Equals("DatumUnosa") && !String.IsNullOrEmpty(searchTxt))
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

                    steteDataSet = steteDataSet.Where(k => k.DatumUnosa >= datumFirst && k.DatumUnosa <= datumSecond);

                }
                else if (searchColumn.Equals("FirmaStete") && !String.IsNullOrEmpty(searchTxt))
                {
                    steteDataSet = steteDataSet.Where(k => k.FirmaStete.Contains(searchTxt));
                }
                else if (searchColumn.Equals("NalogSektor") && !String.IsNullOrEmpty(searchTxt))
                {
                    steteDataSet = steteDataSet.Where(k => k.NalogSektor.Contains(searchTxt));
                }
                else if (searchColumn.Equals("NalogIzdao") && !String.IsNullOrEmpty(searchTxt))
                {
                    steteDataSet = steteDataSet.Where(k => k.NalogIzdao.Contains(searchTxt));
                }
                else if (searchColumn.Equals("StetocinaZaposleni") && !String.IsNullOrEmpty(searchTxt))
                {
                    steteDataSet = steteDataSet.Where(k => k.StetocinaZaposleni.Contains(searchTxt));
                }
                else if (searchColumn.Equals("StetocinaCentar") && !String.IsNullOrEmpty(searchTxt))
                {
                    steteDataSet = steteDataSet.Where(k => k.StetocinaCentar.Contains(searchTxt));
                }
                else if (searchColumn.Equals("Vozilo") && !String.IsNullOrEmpty(searchTxt))
                {
                    steteDataSet = steteDataSet.Where(k => k.Vozilo.Contains(searchTxt));
                }
                else if (searchColumn.Equals("StetaTip") && !String.IsNullOrEmpty(searchTxt))
                {
                    steteDataSet = steteDataSet.Where(k => k.StetaTip.Contains(searchTxt));
                }
                else if (searchColumn.Equals("Opis") && !String.IsNullOrEmpty(searchTxt))
                {
                    steteDataSet = steteDataSet.Where(k => k.Opis.Contains(searchTxt));
                }
                else if (searchColumn.Equals("IznosRsd") && !String.IsNullOrEmpty(searchTxt))
                {
                    int iznosRsdInt = System.Convert.ToInt32(searchTxt);
                    steteDataSet = steteDataSet.Where(k => k.IznosRsd.Equals(iznosRsdInt));
                }
                else if (searchColumn.Equals("Napomena") && !String.IsNullOrEmpty(searchTxt))
                {
                    steteDataSet = steteDataSet.Where(k => k.Napomena.Contains(searchTxt));
                }
                else if (searchColumn.Equals("IznosZaNaplatu") && !String.IsNullOrEmpty(searchTxt))
                {
                    int iznosZaNaplatuInt = System.Convert.ToInt32(searchTxt);
                    steteDataSet = steteDataSet.Where(k => k.IznosZaNaplatu.Equals(iznosZaNaplatuInt));
                }
                else if (searchColumn.Equals("Id") && !String.IsNullOrEmpty(searchTxt))
                {
                    int idInt = System.Convert.ToInt32(searchTxt);
                    steteDataSet = steteDataSet.Where(k => k.Id.Equals(idInt));
                }


            }
            return steteDataSet;
        }



        [ClaimsAuthentication(Resource = "Stete", Operation = "Create, All")]
        public ActionResult Create(int? vozniParkId)
        {

            VozniPark vozniPark = BexUow.VozniPark.Find(vozniParkId);
            if (vozniParkId == null)
            {

                //ViewBag.ZavisnaTabelaId = vozniParkId;
                //ViewBag.RegOznaka = vozniPark.Oznaka;
                ViewBag.FirmaSteteId = new SelectList(BexUow.FirmaVP.AllAsNoTracking, "Id", "Naziv");
                ViewBag.StetocinaCentarId = new SelectList(BexUow.Region.AllAsNoTracking, "Id", "NazivSkraceni");
                ViewBag.KategorijaId = new SelectList(BexUow.StetaTip.AllAsNoTracking.Where(s => s.KategorijaId == 2), "Id", "NazivTipa");

            }
            else
            {
                ViewBag.ZavisnaTabelaId = vozniParkId;
                ViewBag.RegOznaka = vozniPark.Oznaka;
                ViewBag.FirmaSteteId = new SelectList(BexUow.FirmaVP.AllAsNoTracking, "Id", "Naziv");
                ViewBag.StetocinaCentarId = new SelectList(BexUow.Region.AllAsNoTracking, "Id", "NazivSkraceni");
                ViewBag.KategorijaId = new SelectList(BexUow.StetaTip.AllAsNoTracking.Where(s => s.KategorijaId == 2), "Id", "NazivTipa");

                
            }
            return View();

        }

        //// POST: VozniPark/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(VozniParkSteta stete)
        {
            var applicationUser = await SecurityUow.UserManager.FindUserByIdAsync(User.Identity.GetUserId()) as ApplicationUser;
            var korisnikProgramaId = BexUow.KorisniciPrograma.Find(x => x.AspNetUserId == applicationUser.Id).Id;
            stete.UserUnosId = korisnikProgramaId;
            BexUow.VozniPark.DodajIzmeniStetu(stete.Id, stete.FirmaSteteId, stete.ZavisnaTabelaId, stete.UserUnosId, stete.KategorijaId, stete.Opis, stete.Napomena, stete.StetocinaZaposleniId, stete.StetocinaCentarId, stete.DatumPredajePravnoj, stete.IznosRsd,
            0, stete.IznosZaNaplatu, stete.UserUnosId, stete.Sporno, stete.Racun, stete.Kes, stete.DatumOdluke, stete.KnjigovodstveniManjak, 0, stete.PotpisanaOdluka, stete.Nenaplativo);
            return RedirectToAction("Index", "VozniPark");

        }

        [ClaimsAuthentication(Resource = "Stete", Operation = "Edit, All")]
        public ActionResult Edit(int id)
        {

            VozniParkSteta stete = BexUow.VozniParkSteta.Find(id);
            if (stete == null)
            {
                return HttpNotFound();
            }
            ViewBag.StetocinaZaposleniId = (stete.StetocinaZaposleniId == -1) ? "" : BexUow.Kontakts.Find(BexUow.Zaposleni.Find(stete.StetocinaZaposleniId).KontaktId).Naziv;
            ViewBag.FirmaSteteId = new SelectList(BexUow.FirmaVP.AllAsNoTracking, "Id", "Naziv",stete.FirmaSteteId);
            ViewBag.StetocinaCentarId = new SelectList(BexUow.Region.AllAsNoTracking, "Id", "NazivSkraceni", stete.StetocinaCentarId);
            ViewBag.KategorijaId = new SelectList(BexUow.StetaTip.AllAsNoTracking.Where(s => s.KategorijaId == 2), "Id", "NazivTipa", stete.KategorijaId);

            //VozniPark vozilo = BexUow.VozniPark.Find(stete.ZavisnaTabelaId);
            //ViewBag.ZavisnaTabelaId = BexUow.Kontakts.Find(BexUow.Zaposleni.Find(stete.StetocinaZaposleniId).KontaktId).Naziv;
            ViewBag.ZavisnaTabelaId = new SelectList(BexUow.VozniPark.AllAsNoTracking, "Id", "Oznaka", stete.ZavisnaTabelaId);

            return View(stete);
        }

        // POST: VozniPark/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(VozniParkSteta stete)
        {
            BexUow.VozniPark.DodajIzmeniStetu(stete.Id, stete.FirmaSteteId, stete.ZavisnaTabelaId, stete.UserUnosId, stete.KategorijaId, stete.Opis, stete.Napomena, stete.StetocinaZaposleniId, stete.StetocinaCentarId, stete.DatumPredajePravnoj, stete.IznosRsd,
             0, stete.IznosZaNaplatu, stete.UserUnosId, stete.Sporno, stete.Racun, stete.Kes, stete.DatumOdluke, stete.KnjigovodstveniManjak, 0, stete.PotpisanaOdluka, stete.Nenaplativo);
            return RedirectToAction("Index", "Stete");
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

        [HttpGet]
        public ActionResult GetSteteDetailsData(int id)
        {

            var detailsStete = BexUow.VozniParkSteta.AllAsNoTracking.Where(x => x.ZavisnaTabelaId == id)
                                      .Select(x => new StetaIndexData
                                      {
                                          Id = x.Id,
                                          DatumUnosa = x.DatumUnosa,
                                          FirmaStete = x.Firma.Naziv,
                                          NalogSektor = x.StetaKategorija.Naziv,
                                          NalogIzdao = x.KorisnikIzdao.Kontakt.Naziv,
                                          StetocinaZaposleni = x.StetocinaZaposleni.Kontakt.Naziv,
                                          StetocinaCentar = x.StetocinaCentar.NazivSkraceni,
                                          Vozilo = x.VozniPark.Oznaka,
                                          StetaTip = x.StetaTip.NazivTipa,
                                          Opis = x.Opis,
                                          Napomena = x.Napomena,
                                          IznosRsd = x.IznosRsd,
                                          IznosZaNaplatu = x.IznosZaNaplatu,
                                          DatumPredajePravnojSluzbi = x.DatumPredajePravnoj,
                                          DatumOdluke = x.DatumOdluke
                                      });

            return Json(detailsStete, "", JsonRequestBehavior.AllowGet);
        }

        //public ActionResult RemoveOprema(int id)
        //{
        //    var vpOprema = BexUow.VpOprema.Find(id);
        //    BexUow.VpOprema.Remove(vpOprema);

        //    var commandResult = BexUow.SubmitChanges();
        //    if (commandResult.IsSuccessful)
        //    {
        //        return RedirectToAction("../VozniPark");

        //    }

        //    return View();
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
