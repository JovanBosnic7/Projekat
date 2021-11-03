using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Bex.Common;
using Bex.DAL.EF.UOW;
using Bex.Models;
using Bex.MVC.Exceptions;
using BexMVC.Filters;
using BexMVC.Models;
using BexMVC.ViewModels;

namespace BexMVC.Controllers
{
    [Authorize]
    [ClaimsAuthentication(Resource = "Posiljka", Operation = "Read, All")]
    public class PosiljkaController : Controller
    {
        public PosiljkaController() : this(new BexUow(), new ExceptionSolver())
        { }
        public PosiljkaController(
            IBexUow bexUow,
            IExceptionSolver exceptionSolver)
        {
            BexUow = bexUow;
            ExceptionSolver = exceptionSolver;
        }

        // GET: Posiljka
        public ActionResult Index(int? posiljkaId)
        {
            TempData.ToList().ForEach(keyValue =>
                ModelState.AddModelError("", keyValue.Value.ToString()));
            TempData.Clear();
            ViewBag.Statusi = new SelectList(BexUow.PosiljkaStatus.AllAsNoTracking, "Naziv", "Naziv");
            ViewBag.Sadrzaj = new SelectList(BexUow.PosiljkaSadrzaj.AllAsNoTracking, "Naziv", "Naziv");
            ViewBag.Kategorija = new SelectList(BexUow.PosiljkaKategorija.AllAsNoTracking, "NazivKategorije", "NazivKategorije");
            ViewBag.Vrsta = new SelectList(BexUow.PosiljkaVrsta.AllAsNoTracking, "NazivVrste", "NazivVrste");
            ViewBag.PosiljkaId = (posiljkaId == null) ? 0 : posiljkaId;
            //var posiljkas = db.Posiljkas.Include(p => p.PosiljkaKategorija).Include(p => p.PosiljkaSadrzaj).Include(p => p.PosiljkaStatus).Include(p => p.PosiljkaVrsta);
            return View();
        }

        

        [HttpGet]
        public ActionResult GetPosiljkaData(int pageSize, int pageNumber, string sortColumn, string sortOrder, string search, string searchColumn, string searchTerms, int posiljkaId) //, List<Object> searchTerms
        {
            var skip = (pageNumber - 1) * pageSize;

            var total = 0;

            if(posiljkaId > 0)
            {
                searchTerms = "PosiljkaId:" + posiljkaId.ToString();
            }
                
            
            if (!String.IsNullOrEmpty(searchTerms))
            {

                var posiljkeData = BexUow.Posiljke.GetSearchPosiljkaData(searchTerms).Select(x =>
                                                                     new PosiljkaIndexData
                                                                     {
                                                                         PosiljkaId = x.Id,
                                                                         UkupnoPaketa = x.UkupnoPaketa,
                                                                         Status = x.PosiljkaStatus.Naziv,
                                                                         Vrsta = x.PosiljkaVrsta.NazivVrste,
                                                                         Kategorija = x.PosiljkaKategorija.NazivKategorije,
                                                                         Sadrzaj = x.PosiljkaSadrzaj.Naziv,
                                                                         Ugovor = x.PosiljkaUgovor?.Kontakt?.Naziv,
                                                                         CenaUkupna = x.CenaUkupna,
                                                                         UserDodao = "", // x.UserUneo?.Kontakt?.Naziv,
                                                                         DatumEvidentiranja = x.DatumEvidentiranja,
                                                                         VremeEvidentiranja = x.VremeEvidentiranja,
                                                                         Posiljalac = x.PosiljkaZadatak.Where(p => p.Tip == "P").SingleOrDefault().NazivKlijenta,
                                                                         PosiljalacAdresa = x.PosiljkaZadatak.Where(p => p.Tip == "P").SingleOrDefault().Adresa,
                                                                         Primalac = x.PosiljkaZadatak.Where(p => p.Tip == "D").SingleOrDefault().NazivKlijenta,
                                                                         PrimalacAdresa = x.PosiljkaZadatak.Where(p => p.Tip == "D").SingleOrDefault().Adresa,
                                                                         Storno=x.Storno,
                                                                         Arhivirana=x.Arhivirana
                                                                     });

                total = posiljkeData.Count();

                if (sortOrder.Equals("desc"))
                {
                    posiljkeData = posiljkeData.OrderByDescending(s => s.GetType().GetProperty((sortColumn == "") ? "PosiljkaId" : sortColumn).GetValue(s))
                                               .ToList()
                                               .Skip(skip)
                                               .Take(pageSize);
                }
                else
                {
                    posiljkeData = posiljkeData.OrderBy(s => s.GetType().GetProperty((sortColumn == "") ? "PosiljkaId" : sortColumn).GetValue(s))
                                               .ToList()
                                               .Skip(skip)
                                               .Take(pageSize);
                }

                var jsonData = new TableJsonIndexData<PosiljkaIndexData>()
                {
                    total = total,
                    rows = posiljkeData
                };


                var jsonResult = Json(jsonData, "TableJsonIndexData", JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;

            }


            return null;

        }

        
        public ActionResult DetailsZadaci(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.PosiljkaId = id.Value;

            return View();
        }

        
        [ClaimsAuthentication(Resource = "Posiljka", Operation = "DetailsZadaci, All")]
        [HttpGet]
        public ActionResult GetZadaciData(int? id)
        {

            var detailsZadaci = (from zadaci in BexUow.PosiljkaZadatak.AllAsNoTracking
                                 join reon in BexUow.Reon.AllAsNoTracking on zadaci.ReonId equals reon.Id
                                 //join user in BexUow.Users.AllAsNoTracking on zadaci.IzvrsioId equals user.Id
                                 select new
                                 {
                                     BrojPosiljke = zadaci.PosiljkaId,
                                     TipZadatka = zadaci.Tip,
                                     MinimalniDatum = zadaci.DatumMin,
                                     MaksimalniDatum = zadaci.DatumMax,
                                     Najava = zadaci.NajavaMinuta,
                                     KontaktOsoba = zadaci.KontaktIme,
                                     KontaktTel = zadaci.KontaktTelefon,
                                     KontaktTel2 = zadaci.KontaktTelefon2,
                                     NapomenaZadatak = zadaci.Napomena,
                                     StatusZadatka = zadaci.Status,
                                     Reon = reon.OznReona,
                                     DatumObavljanja = zadaci.DatumIzvrsenja,
                                     VremeObavljanja = zadaci.VremeIzvrsenja,
                                     Izvrsio = "", //user.Kontakt.Naziv,
                                     ObavitiUmagacinu = zadaci.IzvrsitiUmagacinu,
                                     Aktuelan = zadaci.AktuelanOd,
                                 }).Where(s => s.BrojPosiljke == id).ToList();

            //List<VozniParkDnevnik> detailsTroskovi = BexUow.VozniParkDnevnik.GetAll(p => p.VpId == id).ToList();

            return Json(detailsZadaci, "", JsonRequestBehavior.AllowGet);
        }

        
        [ClaimsAuthentication(Resource = "Posiljka", Operation = "DetailsPaketi, All")]
        [HttpGet]
        public ActionResult GetPaketiData(int? id)
        {

            var detailsPaketi = (from paketi in BexUow.Paket.AllAsNoTracking
                                 join zona in BexUow.Zona.AllAsNoTracking on paketi.ZonaId equals zona.Id
                                 select new
                                 {
                                     BrojPosiljke=paketi.PosiljkaId,
                                     BarKodPaketa = paketi.BarKod,
                                     PaketRb = paketi.PaketRB,
                                     TipPaketa = paketi.TipPaketa,
                                     Masa = paketi.Masa,
                                     Zona = zona.NazivZone                                     
                                 }).Where(s => s.BrojPosiljke == id).ToList();

            return Json(detailsPaketi, "", JsonRequestBehavior.AllowGet);
        }

        
        [ClaimsAuthentication(Resource = "Posiljka", Operation = "DetailsUsluge, All")]
        [HttpGet]
        public ActionResult GetUslugeData(int? id)
        {

            var detailsUsluge = (from usluge in BexUow.PosiljkaUsluga.AllAsNoTracking
                                 join tipUsluge in BexUow.PosiljkaUslugaTip.AllAsNoTracking on usluge.TipUslugeId equals tipUsluge.Id
                                 select new
                                 {
                                     BrojPosiljke = usluge.PosiljkaId,
                                     TipUsluge = tipUsluge.Naziv,
                                     Komada = usluge.Komada,
                                     CenaUsluge = usluge.CenaUsluge,
                                     Napomena = usluge.Napomena
                                 }).Where(s => s.BrojPosiljke == id).ToList();

            return Json(detailsUsluge, "", JsonRequestBehavior.AllowGet);
        }

        
        [ClaimsAuthentication(Resource = "Posiljka", Operation = "DetailsPlacanja, All")]
        [HttpGet]
        public ActionResult GetPlacanjeData(int? id)
        {

           var detailsPlacanje = (from placanje in BexUow.PosiljkaPlacanje.AllAsNoTracking
                                 join placanjeTip in BexUow.PosiljkaPlacanjeTip.AllAsNoTracking on placanje.PlacanjeId equals placanjeTip.Id
                                 select new
                                 {
                                     BrojPosiljke = placanje.PosiljkaId,
                                     TipPlacanja = placanjeTip.Naziv,
                                     Iznos = placanje.Iznos
                                 }).Where(s => s.BrojPosiljke == id).ToList();

            return Json(detailsPlacanje, "", JsonRequestBehavior.AllowGet);
        }

        
        [ClaimsAuthentication(Resource = "Posiljka", Operation = "DetailsNapomene, All")]
        [HttpGet]
        public ActionResult GetNapomeneData(int? id)
        {

            var detailsNapomene = (from napomena in BexUow.PosiljkaNapomena.AllAsNoTracking
                                   join napomenaTip in BexUow.PosiljkaNapomenaTip.AllAsNoTracking on napomena.NapomenaTipId equals napomenaTip.Id
                                   select new
                                   {
                                       BrojPosiljke = napomena.PosiljkaId,
                                       TipNapomene = napomenaTip.Naziv,
                                       Napomena = napomena.Napomena
                                   }).Where(s => s.BrojPosiljke == id).ToList();

            return Json(detailsNapomene, "", JsonRequestBehavior.AllowGet);
        }

        
        [ClaimsAuthentication(Resource = "Posiljka", Operation = "DetailsNalozi, All")]
        [HttpGet]
        public ActionResult GetNaloziData(int? id)
        {

            var detailsNalozi = (from paketZadatak in BexUow.PaketZadatak.AllAsNoTracking
                                   join paket in BexUow.Paket.AllAsNoTracking on paketZadatak.IdPaketa equals paket.Id
                                 join paketTip in BexUow.PaketTip.AllAsNoTracking on paket.TipPaketa equals paketTip.Id
                                 join zona in BexUow.Zona.AllAsNoTracking on paketZadatak.ZonaId equals zona.Id
                                   
                                 //join user in BexUow.Users.AllAsNoTracking on paketZadatak.IzvrsioId equals user.Id
                                 select new
                                   {
                                       BrojPosiljke = paketZadatak.PosiljkaId,
                                       Paket = paketTip.Naziv + " " + paket.PaketRB,
                                       Tip = paketZadatak.Tip,
                                       Zona = zona.NazivZone,
                                       DatumIzvrsenja = paketZadatak.DatumIzvrsenja,
                                       VremeIzvrsenja = paketZadatak.VremeIzvrsenja,
                                       Obavio = paketZadatak.IzvrsioId
                                 }).Where(s => s.BrojPosiljke == id).ToList();

            return Json(detailsNalozi, "", JsonRequestBehavior.AllowGet);
        }

       

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            { BexUow.Dispose(); }
            base.Dispose(disposing);
        }

        private IExceptionSolver ExceptionSolver { get; }
        private IBexUow BexUow { get; }
    }
}
