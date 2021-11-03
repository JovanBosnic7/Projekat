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
using System.Data.Entity.Core.Objects;
using System.Threading.Tasks;
using AspNet.DAL.EF.UOW.Security;
using AspNet.DAL.EF.Models.Security;
using Microsoft.AspNet.Identity;
using Bex.Common.Interfaces;
using System.Web;
using BexMVC.Filters;

namespace BexMVC.Controllers
{

    /* Authorize - ne moze ne ulogovani korisnici */
    [Authorize]
    [ClaimsAuthentication(Resource = "Magacin", Operation = "Read, All")]
    public class MagacinController : Controller
    {
        
        public MagacinController() : this(new BexUow(), new SecurityUow(System.Web.HttpContext.Current.GetOwinContext()), new ExceptionSolver())
        { }
        public MagacinController(
            IBexUow bexUow,
            ISecurityUow securityUow,
            IExceptionSolver exceptionSolver)
        {
            BexUow = bexUow;
            SecurityUow = securityUow;
            ExceptionSolver = exceptionSolver;

        }
        public ActionResult Index(int? id, int? page, string searchString)
        {
            TempData.ToList().ForEach(keyValue =>
                ModelState.AddModelError("", keyValue.Value.ToString()));
            TempData.Clear();
            //ViewBag.Model = new SelectList(BexUow.VozniParkModel.AllAsNoTracking, "NazivModela", "NazivModela");
            //ViewBag.Statusi = new SelectList(BexUow.VozniParkStatus.AllAsNoTracking, "NazivStatusa", "NazivStatusa");
           


            return View();

        }

        public async Task<List<string>> getAllClaims()
        {
            List<string> listaRegiona = new List<string>();
            var applicationUser = await SecurityUow.UserManager.FindUserByIdAsync(User.Identity.GetUserId()) as ApplicationUser;

            foreach (var c in applicationUser.Claims)
            {
                if (c.ClaimType.Split('/').Last().Equals("Region"))
                {
                    if (c.ClaimValue.Equals("All"))
                    {
                        listaRegiona = BexUow.Region.AllAsNoTracking.Select(r => r.Id).ToList().ConvertAll<string>(delegate (int i) { return i.ToString(); });
                    }
                    else
                    {
                        listaRegiona.Add(c.ClaimValue);
                    }
                }

            }

            return listaRegiona;
        }

        [HttpGet]
       
        public async Task<ActionResult> GetZaduzenja(int pageSize, int pageNumber, string sortColumn, string sortOrder, string search, string searchColumn, string searchTerms)
        {
            List<string> listaRegiona = await getAllClaims();
            DateTime datumDanas = DateTime.Today.Date;

            var skip = (pageNumber - 1) * pageSize;

            var zaduzenjeDataAll = (from r in BexUow.Reon.AllAsNoTracking.Where(r => r.Storno == false).Where (x => listaRegiona.Contains(x.RegionId.ToString()))
                                    let zaduzenje = (from z in BexUow.ZaduzenjaKurira.AllAsNoTracking
                                                     where z.ReonId == r.Id
                                                     select z).OrderByDescending(z => z.Id).Take(1).FirstOrDefault()
                                    let razduzenje = (from a in BexUow.KurirRazduzenje.AllAsNoTracking
                                                      where a.ReonId == r.Id && a.KurirId == zaduzenje.KurirUserId && a.Datum == DateTime.Today
                                                      select a).OrderByDescending(a => a.Id).Take(1).FirstOrDefault()

                                    let dostave = (from d in BexUow.PosiljkaZadatak.AllAsNoTracking.Where(p => p.Tip == "D" && p.Status == 0 && p.AktuelanOd <= datumDanas)
                                                   where d.ReonId == r.Id
                                                   select d).Count()

                                    let user = (from u in BexUow.KorisniciPrograma.AllAsNoTracking
                                                where u.idStari == zaduzenje.KurirUserId
                                                select u).OrderByDescending(a => a.Id).Take(1).FirstOrDefault()

                                    //where r.RegionId == 2

                                    select new MagacinIndexData
                                 {
                                     Id = r.Id,
                                     DatumZaduzenja = zaduzenje.DatumZaduzenja,
                                     VremeZaduzenja = zaduzenje.VremeZaduzenja,
                                     KurirId = zaduzenje.KurirUserId,
                                     KurirNaziv =  user.Kontakt.Naziv.ToString(), 
                                     ReonNaziv = r.OznReona,
                                     Vozilo = zaduzenje.Zona.NazivZone.ToString(),
                                     ZonaId = zaduzenje.ZonaId,
                                     UkupnoRazduzio = razduzenje.UkupnoRazduzio,
                                     DanasDostava=(dostave<50) ? 1 : -1 
                                 }).ToList();

            var zaduzenjeData = zaduzenjeDataAll;

            var total = zaduzenjeDataAll.Count();


            if (sortOrder.Equals("desc"))
                zaduzenjeData = zaduzenjeData.OrderByDescending(s => s.GetType().GetProperty(sortColumn).GetValue(s)).ToList().Skip(skip).Take(pageSize).ToList();
            else
                zaduzenjeData = zaduzenjeData.OrderBy(s => s.GetType().GetProperty((sortColumn == "") ? "Id" : sortColumn).GetValue(s)).ToList().Skip(skip).Take(pageSize).ToList();



            if (!String.IsNullOrEmpty(searchTerms))
            {
                total = GetSearchZaduzenjaKuriraData(searchTerms, zaduzenjeDataAll).Count();

                zaduzenjeData = GetSearchZaduzenjaKuriraData(searchTerms, zaduzenjeDataAll).Select(x =>
                                                         new MagacinIndexData
                                                         {
                                                             Id = x.Id,
                                                             DatumZaduzenja = x.DatumZaduzenja,
                                                             VremeZaduzenja = x.VremeZaduzenja,
                                                             KurirId = x.KurirId,
                                                             KurirNaziv = x.KurirNaziv,
                                                             ReonNaziv = x.ReonNaziv,
                                                             Vozilo = x.Vozilo,
                                                             ZonaId = x.ZonaId,
                                                             UkupnoRazduzio = x.UkupnoRazduzio
                                                         }).ToList();
                if (sortOrder.Equals("desc"))
                {
                    zaduzenjeData = zaduzenjeData.OrderByDescending(s => s.GetType().GetProperty((sortColumn == "") ? "Id" : sortColumn).GetValue(s))
                                                 .ToList()
                                                 .Skip(skip)
                                                 .Take(pageSize).ToList();
                }
                else
                {
                    zaduzenjeData = zaduzenjeData.OrderBy(s => s.GetType().GetProperty((sortColumn == "") ? "Id" : sortColumn).GetValue(s))
                                                 .ToList()
                                                 .Skip(skip)
                                                 .Take(pageSize).ToList();
                }

                                            
            }


            var jsonData = new TableJsonIndexData<MagacinIndexData>()
            {
                total = total,
                rows = zaduzenjeData
            };
            var jsonResult = Json(jsonData, "TableJsonIndexData", JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
            
        }

        public IEnumerable<MagacinIndexData> GetSearchZaduzenjaKuriraData(string searchTerms, List<MagacinIndexData> searchDataSet)
        {

            string[] terms = searchTerms.Split(',');
            string searchColumn = "";
            string searchTxt = "";

            //string searchColumnDatumZaduzenja = "";
            //string searchColumnVremeZaduzenja = "";
            string searchColumnKurirNaziv = "";
            string searchColumnReonNaziv = "";
            string searchColumnVozilo = "";


            var zaduzenjeKurira = searchDataSet;
            //DataSet.AsQueryable();

            foreach (string t in terms)
            {
                string[] searchCT = t.Split(':');
                searchColumn = searchCT[0];
                searchTxt = searchCT[1];

                if (!String.IsNullOrEmpty(searchTxt))
                {
                    //if (searchColumn.Equals("DatumZaduzenja"))
                    //{
                    //    string[] arrDatum = searchTxt.Split('/');
                    //    DateTime thisDate1 = new DateTime(int.Parse(arrDatum[2]), int.Parse(arrDatum[0]), int.Parse(arrDatum[1]));
                    //    zaduzenjeKurira = zaduzenjeKurira.Where(k => k.DatumZaduzenja == thisDate1).ToList();

                    //}
                    //else 
                    // ivn nece da radi pretragu po nazivu i po vozilu jer su vezne tabele
                    if (searchColumn.Equals("KurirNaziv"))
                    {
                        searchColumnKurirNaziv = searchTxt;
                        zaduzenjeKurira = zaduzenjeKurira.Where(k => k.KurirNaziv.ToUpper().Contains(searchColumnKurirNaziv.ToUpper())).ToList();
                    }
                    else if (searchColumn.Equals("ReonNaziv"))
                    {
                        searchColumnReonNaziv = searchTxt;
                        zaduzenjeKurira = zaduzenjeKurira.Where(k => k.ReonNaziv.ToUpper().Contains(searchColumnReonNaziv.ToUpper())).ToList();
                    }
                    else if (searchColumn.Equals("Vozilo"))
                    {
                        searchColumnVozilo = searchTxt;
                        zaduzenjeKurira = zaduzenjeKurira.Where(k => k.Vozilo.ToUpper().Contains(searchColumnVozilo.ToUpper())).ToList();
                    }

                }
            }
            return zaduzenjeKurira;
        }

        [ClaimsAuthentication(Resource = "Magacin", Operation = "Click, All")]
        public async Task<JsonResult> KreirajZbirneOtkupe()
        {
            var listaRegion = await getAllClaims();
            var uowCommandResult = BexUow.ZaduzenjaKurira.KreirajZbirneOtkupe(listaRegion);
            if (uowCommandResult.IsSuccessful)
            {
                return Json(new { success = "true" });
            }
            ExceptionSolver.PrepareModelState(ModelState, uowCommandResult);
            return Json(new { success = "false" });
        }

        [ClaimsAuthentication(Resource = "Magacin", Operation = "Click, ClickNapomenaPreuzimanje, ClickNapomenaPreuzimanje, All")]
        public JsonResult UpisiNapomenu(SelectedValueIndexData values) //string posiljkeIds, int reonId, int napomenaId
        {
            int napomenaId = values.napomenaId;
            string napomenaTxt = BexUow.NapomenaPosiljkaPodTip.Find(x => x.Id == napomenaId).NazivPodTipa2;
            int reonId = values.reonId;
            int kurirId = values.kurirId;
            
            foreach (var val in values.selectedValues)
            {
                int posiljkaId = val.posiljkaId;
                string tipZadatka = val.tipZadatka;

                var imaRazduzenjeSpecifikacija = BexUow.KurirRazduzenjeSpecifikacija.Find(x => x.PosiljkaId == posiljkaId && x.TipZadatka == tipZadatka);
                if (imaRazduzenjeSpecifikacija != null)
                {
                    BexUow.KurirRazduzenjeSpecifikacija.Remove(imaRazduzenjeSpecifikacija);
                    //var commandResult1 = BexUow.SubmitChanges();
                }
                
                var kurirRazduzenjeSpecifikacija = new KurirRazduzenjeSpecifikacija
                {
                        KurirId = kurirId,
                        ReonId = reonId,
                        PosiljkaId = posiljkaId,
                        TipZadatka = tipZadatka,
                        NapomenaPodTipId = napomenaId,
                        Otkup = 0,
                        Usluga = 0,
                        Datum = DateTime.Today
                };

                BexUow.KurirRazduzenjeSpecifikacija.Add(kurirRazduzenjeSpecifikacija);
               // var commandResult2 = BexUow.SubmitChanges();

                var napomenaPosiljka = new NapomenaPosiljka
                {
                    PosiljkaId = posiljkaId,
                    TipZadatka = tipZadatka,
                    PodTipId = napomenaId,
                    Napomena = napomenaTxt,
                    UserUnosId = kurirId
                };

                BexUow.NapomenaPosiljka.Add(napomenaPosiljka);
               // var commandResult3 = BexUow.SubmitChanges();


                var zadatak = BexUow.PosiljkaZadatak.Find(x => x.PosiljkaId == posiljkaId && x.Tip == tipZadatka);
                zadatak.NapomenaKasnjenjeLast = napomenaTxt;
                BexUow.PosiljkaZadatak.Update(zadatak);
                //var commandResult4 = BexUow.SubmitChanges();

            }

            var commandResult = BexUow.SubmitChanges();
            if (commandResult.IsSuccessful)
            {
                return Json(new { success = "true" });
                //return RedirectToAction("Index");
                //RedirectToAction("DetailsObavljeneDostave", new { reonId = reonId, kurirId = kurirId, pageSize = 10, pageNumber = 1 });
            }

            ExceptionSolver.PrepareModelState(ModelState, commandResult);

            return Json(new { success = "false" });
            //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            //return RedirectToAction("GetObavljeneDostaveDetailsData", new { reonId = 119, pageSize = 10, pageNumber = 1 });
            //pageNumber=1&pageSize=10&searchTerms=&sortColumn=&sortOrder=asc&offset=0&limit=10&reonId=119
            //return View();
        }

        [ClaimsAuthentication(Resource = "Magacin", Operation = "ClickReklamacija, All")]
        public async Task<JsonResult> PrijaviReklamaciju(SelectedValueIndexData values) //string posiljkeIds, int reonId, int napomenaId=podtipid reklamacije (ostecena, nije stigla ....)
        {
            int podTipId = values.napomenaId;
            int napomenaId = 4;
            string napomenaTxt = BexUow.NapomenaPosiljkaPodTip.Find(x => x.Id == napomenaId).NazivPodTipa2;
            int reonId = values.reonId;
            int kurirId = values.kurirId;

            var applicationUser = await SecurityUow.UserManager.FindUserByIdAsync(User.Identity.GetUserId()) as ApplicationUser;
            var korisnikProgramaId = BexUow.KorisniciPrograma.Find(x => x.AspNetUserId == applicationUser.Id).Id;
            foreach (var val in values.selectedValues)
            {
                int posiljkaId = val.posiljkaId;
                string tipZadatka = val.tipZadatka;

                var reklamacija = new PrijavaReklamacijaZalba
                {
                    TipPrijaveId = 1,
                    PodTipPrijaveId = podTipId,
                    PosiljkaId = posiljkaId,
                    NacinPrijaveId = 5,
                    PrijavioUserId = korisnikProgramaId,
                    StatusPrijaveId = 1,
                    Opis = ""
                };

                BexUow.PrijavaReklamacijaZalba.Add(reklamacija);

                //var imaRazduzenjeSpecifikacija = BexUow.KurirRazduzenjeSpecifikacija.Find(x => x.PosiljkaId == posiljkaId && x.TipZadatka == "D");
                var imaRazduzenjeSpecifikacija = BexUow.KurirRazduzenjeSpecifikacija.Find(x => x.PosiljkaId == posiljkaId && x.TipZadatka == tipZadatka);
                if (imaRazduzenjeSpecifikacija != null)
                {
                    BexUow.KurirRazduzenjeSpecifikacija.Remove(imaRazduzenjeSpecifikacija);
                }

                var kurirRazduzenjeSpecifikacija = new KurirRazduzenjeSpecifikacija
                {
                    KurirId = kurirId,
                    ReonId = reonId,
                    PosiljkaId = posiljkaId,
                    //TipZadatka = "D",
                    TipZadatka = tipZadatka,
                    NapomenaPodTipId = napomenaId,
                    Otkup = 0,
                    Usluga = 0,
                    Datum = DateTime.Today
                };

                BexUow.KurirRazduzenjeSpecifikacija.Add(kurirRazduzenjeSpecifikacija);
                // var commandResult2 = BexUow.SubmitChanges();

                var napomenaPosiljka = new NapomenaPosiljka
                {
                    PosiljkaId = posiljkaId,
                    //TipZadatka = "D",
                    TipZadatka = tipZadatka,
                    PodTipId = napomenaId,
                    Napomena = napomenaTxt,
                    UserUnosId = kurirId
                };

                BexUow.NapomenaPosiljka.Add(napomenaPosiljka);
                // var commandResult3 = BexUow.SubmitChanges();


                //var zadatak = BexUow.PosiljkaZadatak.Find(x => x.PosiljkaId == posiljkaId && x.Tip == "D");
                var zadatak = BexUow.PosiljkaZadatak.Find(x => x.PosiljkaId == posiljkaId && x.Tip == tipZadatka);
                zadatak.NapomenaKasnjenjeLast = napomenaTxt;
                BexUow.PosiljkaZadatak.Update(zadatak);
            }

            var commandResult = BexUow.SubmitChanges();
            if (commandResult.IsSuccessful)
            {
                return Json(new { success = "true" });
            }
            ExceptionSolver.PrepareModelState(ModelState, commandResult);
            return Json(new { success = "false" });
        }


        [HttpGet]
        public async Task<ActionResult> GetBankaData(int? regionId)
        {
            var listaRegiona = await getAllClaims();          
            DateTime datumStat = DateTime.Today.Date;

            BankaPazara banka = BexUow.BankaPazara.Find(x => listaRegiona.Contains(x.RegionId.ToString()) && x.Datum == datumStat);

            var bankaData = new
            {
                pazar = banka.PazarZaUplatu,
                otkupUplatni = banka.OtkupZaUplatu,
                otkupIsplatni=banka.OtkupZaIsplatu
            };

            return Json(bankaData, "", JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult> GetStatData(int? regionId)
        {
            List<string> listaRegiona = await getAllClaims();
            //regionId = 2;
            DateTime datumDanas = DateTime.Today.Date;
            DateTime datum25 = DateTime.Parse("2019-02-08");

            var aktuelniZadaci = (from zadaci in BexUow.PosiljkaZadatak.AllAsNoTracking
                                  join reon in BexUow.Reon.AllAsNoTracking on zadaci.ReonId equals reon.Id
                                  join posiljka in BexUow.Posiljke.AllAsNoTracking on zadaci.PosiljkaId equals posiljka.Id
                                  where posiljka.Storno == false && zadaci.Status == 0
                                  select new
                                  {
                                      PosiljkaId = zadaci.PosiljkaId.ToString(),
                                      TipZadatka = zadaci.Tip,
                                      Aktuelan = zadaci.AktuelanOd,
                                      RegionId = reon.RegionId,
                                      StatusPosiljke = posiljka.StatusId
                                  })
                                   .Where(x => listaRegiona.Contains(x.RegionId.ToString()) && System.Data.Entity.DbFunctions.TruncateTime(x.Aktuelan) <= datumDanas && System.Data.Entity.DbFunctions.TruncateTime(x.Aktuelan) > datum25).ToList();

            var obavljeniZadaci = (from zadaci in BexUow.PosiljkaZadatak.AllAsNoTracking
                                  join reon in BexUow.Reon.AllAsNoTracking on zadaci.ReonId equals reon.Id
                                  join posiljka in BexUow.Posiljke.AllAsNoTracking on zadaci.PosiljkaId equals posiljka.Id
                                  where posiljka.Storno == false 
                                   select new
                                  {
                                      PosiljkaId = zadaci.PosiljkaId.ToString(),
                                      TipZadatka = zadaci.Tip,
                                      DatumIzvrsenja = zadaci.DatumIzvrsenja,
                                      RegionId = reon.RegionId,
                                      StatusPosiljke = posiljka.StatusId
                                  })
                       .Where(x => listaRegiona.Contains(x.RegionId.ToString()) && System.Data.Entity.DbFunctions.TruncateTime(x.DatumIzvrsenja) == datumDanas).ToList();


            int? aktuelnoPreuzimanja = aktuelniZadaci.Count(p => p.TipZadatka == "P");
            int? obavljenoPreuzimanja = obavljeniZadaci.Count(p => p.TipZadatka == "P");

            int? aktuelnoDostava = aktuelniZadaci.Count(p => p.TipZadatka == "D") + aktuelniZadaci.Count(p => p.TipZadatka == "V");
            int? obavljenoDostava = obavljeniZadaci.Count(p => p.TipZadatka == "D") + obavljeniZadaci.Count(p => p.TipZadatka == "V");

            int? aktuelnoOtkupa = aktuelniZadaci.Count(p => p.TipZadatka == "N");
            int? obavljenoOtkupa = obavljeniZadaci.Count(p => p.TipZadatka == "N");

            int? aktuelnoOtpremnica = aktuelniZadaci.Count(p => p.TipZadatka == "T");
            int? obavljenoOtpremnica = obavljeniZadaci.Count(p => p.TipZadatka == "T");

            int? aktuelnoCekova = aktuelniZadaci.Count(p => p.TipZadatka == "C");
            int? obavljenoCekova = obavljeniZadaci.Count(p => p.TipZadatka == "C");


            var statData = new
            {
                aktuelnoP = aktuelnoPreuzimanja,
                obavljenoP = obavljenoPreuzimanja,
                aktuelnoD = aktuelnoDostava,
                obavljenoD = obavljenoDostava,
                aktuelnoN = aktuelnoOtkupa,
                obavljenoN = obavljenoOtkupa,
                aktuelnoT = aktuelnoOtpremnica,
                obavljenoT = obavljenoOtpremnica,
                aktuelnoC = aktuelnoCekova,
                obavljenoC = obavljenoCekova
            };

            return Json(statData, "", JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [ClaimsAuthentication(Resource = "Magacin", Operation = "Create, All")]
        public ActionResult SaveRazduzenje(int reonId, int kurirId, int zonaId, decimal? ukupnoRazduzio)
        {
            if (ModelState.IsValid)
            {
                decimal? ukupnoOtkupa = BexUow.KurirRazduzenjeSpecifikacija.AllAsNoTracking.Where(x => x.ReonId == reonId && x.KurirId == kurirId && x.Datum == DateTime.Today.Date)
                                                                                   .Sum(o => o.Otkup);


                decimal? ukupnoUsluga = BexUow.KurirRazduzenjeSpecifikacija.AllAsNoTracking.Where(x => x.ReonId == reonId && x.KurirId == kurirId && x.Datum == DateTime.Today.Date)
                                                                                        .Sum(u => u.Usluga);
                var kurirRazduzenje = new KurirRazduzenje
                {
                    ReonId = reonId,
                    KurirId = kurirId,
                    UkupnoOtkupa = ukupnoOtkupa,
                    UkupnoPazara = ukupnoUsluga,
                    UkupnoNaplatio = ukupnoOtkupa + ukupnoUsluga,
                    UkupnoRazduzio = ukupnoRazduzio,
                    Razlika = ukupnoRazduzio - (ukupnoOtkupa + ukupnoUsluga),
                    Napomena = "",
                    UserUnosId = 0,
                    Datum = DateTime.Today

                };

                BexUow.KurirRazduzenje.Add(kurirRazduzenje);
                var commandResult = BexUow.SubmitChanges();
                if (commandResult.IsSuccessful)
                {
                    return RedirectToAction("Razduzenje", new {  reonId,  kurirId, zonaId, pageSize = 10, pageNumber = 1 });
                    //RedirectToAction("Index");
                }
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        
        public async Task<ActionResult> Razduzenje(int? reonId, int? kurirId, int? zonaId)
        {
            DateTime datum25 = DateTime.Parse("2019-02-08");
            List<string> listaRegiona = await getAllClaims();
            if (reonId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //ViewBag.NapomenePreuzimanja = new SelectList(BexUow.NapomenaPosiljkaPodTip.AllAsNoTracking.Where(x=>x.TipId==951), "NazivPodTipa2", "NazivPodTipa2");
            ViewBag.NapomenePreuzimanja = BexUow.NapomenaPosiljkaPodTip.AllAsNoTracking.Where(x => x.TipId == 951).ToList();
            //ViewBag.NapomeneDostave = new SelectList(BexUow.NapomenaPosiljkaPodTip.AllAsNoTracking.Where(x => x.TipId == 952), "NazivPodTipa2", "NazivPodTipa2");
            ViewBag.NapomeneDostave = BexUow.NapomenaPosiljkaPodTip.AllAsNoTracking.Where(x => x.TipId == 952).ToList();
            ViewBag.NapomeneOtkupnine = BexUow.NapomenaPosiljkaPodTip.AllAsNoTracking.Where(x => x.TipId == 963).ToList();
            ViewBag.NapomeneOtpremnice = BexUow.NapomenaPosiljkaPodTip.AllAsNoTracking.Where(x => x.TipId == 964).ToList();
            ViewBag.PromenaReona = BexUow.Reon.AllAsNoTracking.Where(x => listaRegiona.Contains(x.RegionId.ToString())).ToList();
            //x => listaRegiona.Contains(x.RegionId.ToString())
            ViewBag.Reklamacija = BexUow.PrijavaPodTip.AllAsNoTracking.Where(x => x.TipId == 1).ToList();
            ViewBag.ReonId = reonId.Value;
            ViewBag.KurirId = kurirId.Value;
            ViewBag.ZonaId = zonaId.Value;

            string reonNaziv = BexUow.Reon.Find(x => x.Id == reonId && x.Storno == false).OznReona;
            var applicationUser = await SecurityUow.UserManager.FindUserByIdAsync(User.Identity.GetUserId()) as ApplicationUser;
            var korisnikProgramaId = BexUow.KorisniciPrograma.Find(x => x.AspNetUserId == applicationUser.Id).Id;
            int? kurirKontaktId = BexUow.KorisniciPrograma.Find(x => x.Id == kurirId && x.Aktivan == true).KontaktId; //  je prazno ivn???
            string kurirNaziv = BexUow.Kontakts.Find(x => x.Id == kurirKontaktId).Naziv;

            int ukupnoObavljenihDostavaKurira = BexUow.KurirRazduzenjeSpecifikacija.AllAsNoTracking
                                                     .Where(x => x.TipZadatka != "P" && x.KurirId == kurirId && x.Status == true).Count();


            int ukupnoDostavaKurira = BexUow.KurirRazduzenjeSpecifikacija.AllAsNoTracking
                                                     .Where(x => x.TipZadatka != "P" && x.KurirId == kurirId).Count();

           
           
            int ukupnoObavljenihPreuzimanjaDanas = BexUow.PosiljkaZadatak.AllAsNoTracking.Where(x => x.Tip == "P" && x.ReonId == reonId)
                                                                               .Where(x => x.Status == 1 && System.Data.Entity.DbFunctions.TruncateTime(x.DatumIzvrsenja) == DateTime.Today.Date)
                                                                               .Where(x => x.Posiljka.Storno == false)
                                                                               .Count();

            int ukupnoNeobavljenihPreuzimanjaDanas = BexUow.PosiljkaZadatak.AllAsNoTracking.Where(x => x.Tip == "P" && x.ReonId == reonId)
                                                                                        .Where(x => x.Status == 0 && x.AktuelanOd <= DateTime.Today.Date && x.AktuelanOd > datum25)
                                                                                        .Where(x => x.Posiljka.Storno == false)
                                                                                        .Count();

            int ukupnoNeobavljenihPreuzimanjaBezNapomeneDanas = BexUow.PosiljkaZadatak.AllAsNoTracking.Where(x => x.Tip == "P" && x.ReonId == reonId && x.NapomenaKasnjenjeLast == "")
                                                                                        .Where(x => x.Status == 0 && x.AktuelanOd <= DateTime.Today.Date && x.AktuelanOd > datum25)
                                                                                        .Where(x => x.Posiljka.Storno == false)
                                                                                        .Count();

            int ukupnoPreuzimanjaNareonu = ukupnoObavljenihPreuzimanjaDanas + ukupnoNeobavljenihPreuzimanjaDanas;

            //x.ReonId == reonId && izbacila reon
            decimal? ukupnoOtkupa = BexUow.KurirRazduzenjeSpecifikacija.AllAsNoTracking.Where(x =>  x.KurirId == kurirId && x.Datum == DateTime.Today.Date)
                                                                                    .Sum(o => o.Otkup);

            //x.ReonId == reonId && izbacila reon
            decimal? ukupnoUsluga = BexUow.KurirRazduzenjeSpecifikacija.AllAsNoTracking.Where(x =>  x.KurirId == kurirId && x.Datum == DateTime.Today.Date)
                                                                                    .Sum(u => u.Usluga);



            //int countBezStatusaSpecifikacija = BexUow.KurirRazduzenjeSpecifikacija.AllAsNoTracking.Where(x => x.KurirId == kurirId)
            //                                                            .Where(x => x.Status == false && x.NapomenaPodTipId == 0)
            //                                                            .Count();

            int countBezStatusaSpecifikacija = (from razduzenje in BexUow.KurirRazduzenjeSpecifikacija.AllAsNoTracking
                                                where razduzenje.KurirId == kurirId && razduzenje.Status == false && razduzenje.NapomenaPodTipId == 0
                                                join posiljka in BexUow.Posiljke.AllAsNoTracking on razduzenje.PosiljkaId equals posiljka.Id
                                                where posiljka.Storno == false
                                                join zadatak in BexUow.PosiljkaZadatak.AllAsNoTracking on razduzenje.PosiljkaId equals zadatak.PosiljkaId
                                                select new 
                                                {
                                                    PosiljkaId = razduzenje.PosiljkaId
                                                })
                                                    .ToList().Count();

            //int countBezStatusa = countBezStatusaSpecifikacija + ukupnoNeobavljenihPreuzimanjaDanas;
            int countBezStatusa = countBezStatusaSpecifikacija + ukupnoNeobavljenihPreuzimanjaBezNapomeneDanas;


            MagacinDetailsIndexData detailsZadaci = new MagacinDetailsIndexData
            {
                ReonId = reonId,
                ReonNaziv = reonNaziv,
                KurirId = kurirId,
                KurirNaziv = kurirNaziv,
                UkupnoObavljenihDostavaDanas = ukupnoObavljenihDostavaKurira,
                UkupnoDostavaNareonu = ukupnoDostavaKurira,
                UkupnoObavljenihPreuzimanjaDanas = ukupnoObavljenihPreuzimanjaDanas,
                UkupnoPreuzimanjaNareonu = ukupnoPreuzimanjaNareonu,
                UkupnoPazara = ukupnoUsluga,
                UkupnoOtkupa = ukupnoOtkupa,
                CountBezStatusa = countBezStatusa
                //,
                //ListaZadatakaRazduzenjaKurira = GetZadaciDetailsData(reonId.Value, zonaId.Value, kurirId.Value)
        };


            return View(detailsZadaci);
        }

        
        public List<ZadaciIndexData> GetZadaciDetailsData(int reonId,int zonaId, int kurirId)
        {
            DateTime datum25 = DateTime.Parse("2019-02-08");
            var preuzimanjaNaReonu = (from zadaci in BexUow.PosiljkaZadatak.AllAsNoTracking
                                      join reon in BexUow.Reon.AllAsNoTracking on zadaci.ReonId equals reon.Id
                                      join posiljka in BexUow.Posiljke.AllAsNoTracking on zadaci.PosiljkaId equals posiljka.Id
                                      //join user in BexUow.KorisniciPrograma.AllAsNoTracking on zadaci.IzvrsioId equals user.Id
                                      //join kontakt in BexUow.Kontakts.AllAsNoTracking on user.KontaktId equals kontakt.Id
                                      where posiljka.Storno == false
                                      select new ZadaciIndexData
                                      {
                                          PosiljkaId = zadaci.PosiljkaId.ToString(),
                                          TipZadatka = zadaci.Tip,
                                          KlijentDetails = zadaci.NazivKlijenta + ", " + zadaci.Adresa,
                                          KontaktOsoba = zadaci.KontaktIme,
                                          KontaktTel = zadaci.KontaktTelefon,
                                          KontaktTel2 = zadaci.KontaktTelefon2,
                                          NapomenaZadatak = zadaci.NapomenaKasnjenjeLast,
                                          StatusZadatka = zadaci.Status,
                                          Reon = reon.OznReona,
                                          ReonId = reon.Id,
                                          DatumObavljanja = System.Data.Entity.DbFunctions.TruncateTime(zadaci.DatumIzvrsenja),
                                          VremeObavljanja = zadaci.VremeIzvrsenja,
                                          //Izvrsio = kontakt.Naziv, //zadaci.User.Kontakt.Naziv,
                                          Aktuelan = zadaci.AktuelanOd,
                                          KurirId=0
                                      })
                                    //.Where(x => x.TipZadatka != "P" && x.ReonId == reonId)
                                    .Where(x => x.TipZadatka == "P" && x.ReonId == reonId && x.StatusZadatka == 0 && System.Data.Entity.DbFunctions.TruncateTime(x.Aktuelan) > datum25 && System.Data.Entity.DbFunctions.TruncateTime(x.Aktuelan) <= DateTime.Today.Date).ToList();
                                   //.Where(x => x.ReonId == reonId)
                                   //.Where(x => x.StatusZadatka == 0 && System.Data.Entity.DbFunctions.TruncateTime(x.Aktuelan) <= DateTime.Today.Date && System.Data.Entity.DbFunctions.TruncateTime(x.Aktuelan) > datum25 && x.TipZadatka=="P").ToList();

            

            var zadaciPoSpecifikaciji = (from spec in BexUow.KurirRazduzenjeSpecifikacija.AllAsNoTracking
                                         join napomena in BexUow.NapomenaPosiljkaPodTip.AllAsNoTracking on spec.NapomenaPodTipId equals napomena.Id into pp
                                         from napomena in pp.DefaultIfEmpty()
                                         join reon in BexUow.Reon.AllAsNoTracking on spec.ReonId equals reon.Id

                                         //join zadaci in BexUow.PosiljkaZadatak.AllAsNoTracking 
                                         //on new { Key1=spec.PosiljkaId,Key2=spec.TipZadatka } equals new {Key1=zadaci.PosiljkaId,Key2=zadaci.Tip} into pd
                                         //from zadaci in pd.DefaultIfEmpty()
                                         join zadaci in BexUow.PosiljkaZadatak.AllAsNoTracking on new { Key1 = spec.PosiljkaId, Key2 = spec.TipZadatka } 
                                         equals new { Key1 = zadaci.PosiljkaId, Key2 = zadaci.Tip }

                                         //join user in BexUow.KorisniciPrograma.AllAsNoTracking on zadaci.IzvrsioId equals user.Id 
                                         //join kontakt in BexUow.Kontakts.AllAsNoTracking on user.KontaktId equals kontakt.Id

                                         join posiljka in BexUow.Posiljke.AllAsNoTracking on zadaci.PosiljkaId equals posiljka.Id
                                         where posiljka.Storno == false

                                         select new ZadaciIndexData
                                      {
                                          PosiljkaId = spec.PosiljkaId.ToString(),
                                          TipZadatka = spec.TipZadatka,
                                          KlijentDetails = zadaci.NazivKlijenta + ", " + zadaci.Adresa,
                                          KontaktOsoba = zadaci.KontaktIme,
                                          KontaktTel = zadaci.KontaktTelefon,
                                          KontaktTel2 = zadaci.KontaktTelefon2,
                                          //NapomenaZadatak = zadaci.NapomenaKasnjenjeLast,
                                          NapomenaZadatak = napomena.NazivPodTipa,
                                          StatusZadatka = spec.Status ? 1 : 0,
                                          //StatusZadatka = zadaci.Status,
                                          Reon = reon.OznReona,
                                          ReonId = reon.Id,
                                          //DatumObavljanja = System.Data.Entity.DbFunctions.TruncateTime(zadaci.DatumIzvrsenja),
                                          //VremeObavljanja = zadaci.VremeIzvrsenja,
                                          //Izvrsio = kontakt.Naziv, //zadaci.User.Kontakt.Naziv,
                                          Aktuelan = zadaci.AktuelanOd,
                                          KurirId=spec.KurirId
                                      })
                       //.Where(x => x.TipZadatka != "P" && x.ReonId == reonId)
                       .Where(x => x.KurirId == kurirId).ToList();


            var zbirneOtkupnine = (from spec in BexUow.PosiljkaOtkupZbirni.AllAsNoTracking
                                         join reon in BexUow.Reon.AllAsNoTracking on spec.ReonId equals reon.Id
                                         //join user in BexUow.KorisniciPrograma.AllAsNoTracking on spec.IzvrsioId equals user.Id
                                         //join kontakt in BexUow.Kontakts.AllAsNoTracking on user.KontaktId equals kontakt.Id

                                   select new ZadaciIndexData
                                         {
                                             PosiljkaId = spec.BarKod,
                                             TipZadatka = "Z",
                                             KlijentDetails = spec.NazivKlijenta + ", " + spec.Adresa,
                                             KontaktOsoba = "",
                                             KontaktTel = spec.Telefon,
                                             KontaktTel2 = "",
                                             NapomenaZadatak = spec.PoslednjaNapomena,
                                             StatusZadatka = 0,
                                             Reon = reon.OznReona,
                                             ReonId = reon.Id,
                                             DatumObavljanja = System.Data.Entity.DbFunctions.TruncateTime(spec.DatumIzvrsenja),
                                             VremeObavljanja = spec.VremeIzvrsenja,
                                             //Izvrsio = kontakt.Naziv, //spec.User.Kontakt.Naziv,
                                             Aktuelan = spec.Stamp,
                                             KurirId = kurirId
                                         })
           //.Where(x => x.TipZadatka != "P" && x.ReonId == reonId)
           .Where(x => x.ReonId == reonId && x.DatumObavljanja==null).ToList();

            var detailsZadaciDostave = preuzimanjaNaReonu.Union(zadaciPoSpecifikaciji).Union(zbirneOtkupnine).OrderBy(x => x.TipZadatka).OrderBy(x => x.StatusZadatka).OrderBy(x => x.NapomenaZadatak).ToList();

            return detailsZadaciDostave;
        }

        public IEnumerable<ZadaciIndexData> GetSearchZadaciIndexData(string searchTerms, List<ZadaciIndexData> searchDataSet)
        {

            string[] terms = searchTerms.Split(',');
            string searchColumn = "";
            string searchTxt = "";

            
            //int searchColumnPosiljkaId = 0;
            string searchColumnKlijentDetails = "";
            string searchColumnNapomenaZadatak = "";
            string searchColumnIzvrsio = "";
            string searchColumnStatus = "";
            string searchColumnTip = "";
            string searchColumnReonZadatak = "";


            var zadaciKurira = searchDataSet;
           

            foreach (string t in terms)
            {
                string[] searchCT = t.Split(':');
                searchColumn = searchCT[0];
                searchTxt = searchCT[1];

                if (!String.IsNullOrEmpty(searchTxt))
                {
                    if (searchColumn.Equals("StatusZadatka") && !searchTxt.Equals("0"))
                    {
                        searchColumnStatus = searchTxt;
                        if (searchColumnStatus.Equals("1"))
                            zadaciKurira = zadaciKurira.Where(k => k.StatusZadatka == 0).ToList();
                        else if(searchColumnStatus.Equals("2"))
                            zadaciKurira = zadaciKurira.Where(k => k.StatusZadatka == 1).ToList();
                        else if (searchColumnStatus.Equals("3"))
                            zadaciKurira = zadaciKurira.Where(k => k.NapomenaZadatak == null || k.NapomenaZadatak == "").ToList();
                        else if (searchColumnStatus.Equals("4"))
                            zadaciKurira = zadaciKurira.Where(k => k.StatusZadatka == 0).ToList(); //ivn nije dodata provera skena
                        else if (searchColumnStatus.Equals("5"))
                            zadaciKurira = zadaciKurira.Where(k => k.StatusZadatka == 0 && (k.NapomenaZadatak == null || k.NapomenaZadatak == "")).ToList();

                    }
                    else if (searchColumn.Equals("PosiljkaId"))
                    {
                        //searchColumnPosiljkaId = int.Parse(searchTxt);
                        //zadaciKurira = zadaciKurira.Where(k => k.PosiljkaId == searchColumnPosiljkaId).ToList();
                        zadaciKurira = zadaciKurira.Where(k => k.PosiljkaId == searchTxt).ToList();
                    }
                    else if (searchColumn.Equals("TipZadatka") && !searchTxt.Equals("0"))
                    {
                        searchColumnTip = searchTxt;
                        if (searchColumnTip.Equals("1"))
                            zadaciKurira = zadaciKurira.Where(k => k.TipZadatka == "P").ToList();
                        else if (searchColumnTip.Equals("2"))
                            zadaciKurira = zadaciKurira.Where(k => k.TipZadatka == "D").ToList();
                        else if (searchColumnTip.Equals("3"))
                            zadaciKurira = zadaciKurira.Where(k => k.TipZadatka == "V").ToList();
                        else if (searchColumnTip.Equals("4"))
                            zadaciKurira = zadaciKurira.Where(k => k.TipZadatka == "N").ToList();
                        else if (searchColumnTip.Equals("5"))
                            zadaciKurira = zadaciKurira.Where(k => k.TipZadatka == "T").ToList();
                        else if (searchColumnTip.Equals("6"))
                            zadaciKurira = zadaciKurira.Where(k => k.TipZadatka == "C").ToList();
                    }
                    else if (searchColumn.Equals("KlijentDetails"))
                    {
                        searchColumnKlijentDetails = searchTxt;
                        zadaciKurira = zadaciKurira.Where(k => k.KlijentDetails.ToUpper().Contains(searchColumnKlijentDetails.ToUpper())).ToList();
                    }
                    else if (searchColumn.Equals("Reon"))
                    {
                        searchColumnReonZadatak = searchTxt;
                        zadaciKurira = zadaciKurira.Where(k => k.Reon.ToUpper().Contains(searchColumnReonZadatak.ToUpper())).ToList();
                    }
                    else if (searchColumn.Equals("NapomenaZadatak"))
                    {
                        searchColumnNapomenaZadatak = searchTxt;
                        zadaciKurira = zadaciKurira.Where(k => k.NapomenaZadatak.ToUpper().Contains(searchColumnNapomenaZadatak.ToUpper())).ToList();
                    }
                    else if (searchColumn.Equals("DatumObavljanja"))
                    {
                        DateTime datumFirst, datumSecond;
                        string[] arrDatumAll = searchTxt.Split('t', 'o');

                        string[] arrDatumFirst = arrDatumAll[0].Trim().Split('-');
                        datumFirst = new DateTime(int.Parse(arrDatumFirst[0]), int.Parse(arrDatumFirst[1]), int.Parse(arrDatumFirst[2]));

                        string[] arrDatumSecond = arrDatumAll[2].Trim().Split('-');
                        datumSecond = new DateTime(int.Parse(arrDatumSecond[0]), int.Parse(arrDatumSecond[1]), int.Parse(arrDatumSecond[2]));

                        zadaciKurira = zadaciKurira.Where(k => k.DatumObavljanja >= datumFirst && k.DatumObavljanja <= datumSecond).ToList();
                        //string[] arrDatum = searchTxt.Split('/');
                        //DateTime thisDate1 = new DateTime(int.Parse(arrDatum[2]), int.Parse(arrDatum[0]), int.Parse(arrDatum[1]));
                        //zadaciKurira = zadaciKurira.Where(k => k.DatumObavljanja == thisDate1).ToList();
                    }
                    else if (searchColumn.Equals("Izvrsio"))
                    {
                        searchColumnIzvrsio = searchTxt;
                        zadaciKurira = zadaciKurira.Where(k => k.Izvrsio.ToUpper().Contains(searchColumnIzvrsio.ToUpper())).ToList();
                    }
                    else if (searchColumn.Equals("Aktuelan"))
                    {
                        //string[] arrDatum = searchTxt.Split('/');
                        //DateTime thisDate1 = new DateTime(int.Parse(arrDatum[2]), int.Parse(arrDatum[0]), int.Parse(arrDatum[1]));
                        //zadaciKurira = zadaciKurira.Where(k => k.Aktuelan == thisDate1).ToList();
                        DateTime datumFirst, datumSecond;
                        string[] arrDatumAll = searchTxt.Split('t', 'o');

                        string[] arrDatumFirst = arrDatumAll[0].Trim().Split('-');
                        datumFirst = new DateTime(int.Parse(arrDatumFirst[0]), int.Parse(arrDatumFirst[1]), int.Parse(arrDatumFirst[2]));

                        string[] arrDatumSecond = arrDatumAll[2].Trim().Split('-');
                        datumSecond = new DateTime(int.Parse(arrDatumSecond[0]), int.Parse(arrDatumSecond[1]), int.Parse(arrDatumSecond[2]));

                        zadaciKurira = zadaciKurira.Where(k => k.Aktuelan >= datumFirst && k.Aktuelan <= datumSecond).ToList();
                    }

                }
            }
            return zadaciKurira;
        }


        [HttpGet]
        public ActionResult GetZadaciRazduzenjeDetailsData(int pageSize, int pageNumber, string sortColumn, string sortOrder, string search, string searchColumn, string searchTerms, int reonId, int zonaId, int kurirId)
        {         
            var skip = (pageNumber - 1) * pageSize;

            var detailsZadaciAll = GetZadaciDetailsData(reonId,zonaId,kurirId);

            int total = detailsZadaciAll.Count();

            var detailsZadaci = detailsZadaciAll;

            if (sortOrder.Equals("desc"))
                detailsZadaci = detailsZadaci.OrderByDescending(s => s.GetType().GetProperty(sortColumn).GetValue(s)).ToList().Skip(skip).Take(pageSize).ToList();
            else
                detailsZadaci = detailsZadaci.OrderBy(s => s.GetType().GetProperty((sortColumn == "") ? "TipZadatka" : sortColumn).GetValue(s)).ToList().Skip(skip).Take(pageSize).ToList();


            if (!String.IsNullOrEmpty(searchTerms))
            {
                total = GetSearchZadaciIndexData(searchTerms, detailsZadaciAll).Count();

                detailsZadaci = GetSearchZadaciIndexData(searchTerms, detailsZadaciAll).Select(x =>
                                                         new ZadaciIndexData
                                                         {
                                                             PosiljkaId = x.PosiljkaId,
                                                             TipZadatka = x.TipZadatka,
                                                             KlijentDetails = x.KlijentDetails,
                                                             KontaktOsoba = x.KontaktOsoba,
                                                             KontaktTel = x.KontaktTel,
                                                             KontaktTel2 = x.KontaktTel2,
                                                             NapomenaZadatak = x.NapomenaZadatak,
                                                             StatusZadatka = x.StatusZadatka,
                                                             Reon = x.Reon,
                                                             ReonId = x.ReonId,
                                                             DatumObavljanja = x.DatumObavljanja,
                                                             VremeObavljanja = x.VremeObavljanja,
                                                             Izvrsio = x.Izvrsio,
                                                             Aktuelan = x.Aktuelan
                                                         }).ToList();
                if (sortOrder.Equals("desc"))
                {
                    detailsZadaci = detailsZadaci.OrderByDescending(s => s.GetType().GetProperty((sortColumn == "") ? "TipZadatka" : sortColumn).GetValue(s))
                                                 .ToList()
                                                 .Skip(skip)
                                                 .Take(pageSize).ToList();
                }
                else
                {
                    detailsZadaci = detailsZadaci.OrderBy(s => s.GetType().GetProperty((sortColumn == "") ? "TipZadatka" : sortColumn).GetValue(s))
                                                 .ToList()
                                                 .Skip(skip)
                                                 .Take(pageSize).ToList();
                }


            }

            

            var jsonData = new TableJsonIndexData<ZadaciIndexData>()
            {
                total = total,
                rows = detailsZadaci
            };
            var jsonResult = Json(jsonData, "TableJsonIndexData", JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        public ActionResult DetailsRazduzenje(int? id, string zbirniBarKod)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.PosiljkaId = id.Value;
            ViewBag.BarKod = zbirniBarKod;

            return View();
        }

        [HttpGet]
        public ActionResult GetSkenoviData(int? id)
        {

            var detailsSkenovi = (from skenRead in BexUow.SkenRead.AllAsNoTracking
                                   join pakettip in BexUow.PaketTip.AllAsNoTracking on skenRead.TipPaketaId equals pakettip.Id                           
                                  join region in BexUow.Region?.AllAsNoTracking on skenRead.RegionId equals region.Id
                                   join zona in BexUow.Zona?.AllAsNoTracking on skenRead.ZonaId equals zona.Id
                                  join user in BexUow.KorisniciPrograma.AllAsNoTracking on skenRead.UserId equals user.Id
                                  join kontakt in BexUow.Kontakts.AllAsNoTracking on user.KontaktId equals kontakt.Id
                                  join reon in BexUow.Reon?.AllAsNoTracking on skenRead.ReonId equals reon.Id into pp
                                  from reon in pp.DefaultIfEmpty()
                                  select new
                                   {
                                       SkenId = skenRead.Id,
                                       PosiljkaId=skenRead.PosiljkaId,
                                       BarKod=skenRead.BarKod,
                                       RbPaketa=skenRead.RbPaketa,
                                       TipPaketa=pakettip.Naziv,
                                       DatumSkena=skenRead.DatumSkena,
                                       VremeSkena=skenRead.VremeSkena,
                                       ReonSken=reon.OznReona,
                                       RegonSken=region.NazivSkraceni,
                                       ZonaSkena=zona.NazivZone,
                                       Skenirao= kontakt.Naziv, //user.Kontakt.Naziv,
                                       VremePDA=skenRead.VremePDA,
                                       NazivSkena=skenRead.NazivSkena

                                   }).Where(s => s.PosiljkaId == id).OrderByDescending(s => s.VremePDA).ToList();

            return Json(detailsSkenovi, "", JsonRequestBehavior.AllowGet);
        }

        //public ActionResult DetailsZbirneSpecifikacija(string barKod)
        //{
        //    if (barKod == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    ViewBag.BarKod = barKod;

        //    return View();
        //}

        [HttpGet]
        public ActionResult GetZbirnaSpecifikacijaData(string barKod)
        {

            var detailsOtkupSpecifikacija = (from otkupSpec in BexUow.PosiljkaOtkupZbirniStavka.AllAsNoTracking
                                             join zadatak in BexUow.PosiljkaZadatak.AllAsNoTracking.Where(z => z.Tip == "N") on otkupSpec.PosiljkaId equals zadatak.PosiljkaId
                                             join usl in BexUow.PosiljkaUsluga?.AllAsNoTracking.Where(u => u.TipUslugeId==7) on otkupSpec.PosiljkaId equals usl.PosiljkaId into pp
                                             from usl in pp.DefaultIfEmpty()
                                             select new
                                  {
                                    BarKod=otkupSpec.BarKod,
                                    PosiljkaId=otkupSpec.PosiljkaId,
                                    NazivKlijenta=zadatak.NazivKlijenta,
                                    Adresa=zadatak.Adresa,
                                    Otkup=usl.CenaUsluge

                                  }).Where(s => s.BarKod == barKod).ToList();

            return Json(detailsOtkupSpecifikacija, "", JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            { BexUow.Dispose(); }
            base.Dispose(disposing);
        }

        

        private IExceptionSolver ExceptionSolver { get; }
        private IBexUow BexUow { get; }
        private ISecurityUow SecurityUow { get; set; }
    }
}
