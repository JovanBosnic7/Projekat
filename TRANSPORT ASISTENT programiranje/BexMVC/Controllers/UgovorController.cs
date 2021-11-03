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

namespace BexMVC.Controllers
{
    [Authorize]
    [ClaimsAuthentication(Resource = "Ugovor", Operation = "Read, All")]
    public class UgovorController : Controller
    {
        public UgovorController() : this(new BexUow(), new ExceptionSolver())
        { }
        public UgovorController(
            IBexUow bexUow,
            IExceptionSolver exceptionSolver)
        {
            BexUow = bexUow;
            ExceptionSolver = exceptionSolver;

        }
        public ActionResult Index(int? id, int? page, string searchString)
        {
            TempData.ToList().ForEach(keyValue =>
                ModelState.AddModelError("", keyValue.Value.ToString()));
            TempData.Clear();
            ViewBag.Region = new SelectList(BexUow.Region.AllAsNoTracking, "NazivSkraceni", "NazivSkraceni");
            // ViewBag.Statusi = new SelectList(BexUow.VozniParkStatus.AllAsNoTracking, "NazivStatusa", "NazivStatusa");
            return View();

        }

        [HttpGet]
        public ActionResult GetUgovor(int pageSize, int pageNumber, string sortColumn, string sortOrder, string search, string searchColumn, string searchTerms)
        {

            var skip = (pageNumber - 1) * pageSize;

            var total = BexUow.Ugovor.GetTotalUgovor();

            var ugovorData = BexUow.Ugovor.GetUgovorData().Select(x =>
                                                         new UgovorIndexData
                                                         {
                                                             Id = x.Id,
                                                             Nosilac = x.Kontakt?.Naziv.ToString(),
                                                             Region = x.Region?.NazivSkraceni.ToString(),
                                                             BrojUgovora = x.UgovorBroj,
                                                             DatumUgovora = x.DatumUgovora,
                                                             VerzijaUgovora = x.UgovorVerzija,
                                                             DatumVerzije = x.DatumPoslednjeVerzije,
                                                             DatumObnove = x.DatumObnove,
                                                             Ugovorio = x.KorisniciPrograma?.Kontakt?.Naziv.ToString(),
                                                             Agent = x.Zaposleni?.Kontakt?.Naziv.ToString(),
                                                             //Posrednik = x.KorisniciPrograma?.Kontakt?.Naziv.ToString(),
                                                             //Aktivan = x.Aktivan
                                                         });



            if (sortOrder.Equals("desc"))
                ugovorData = ugovorData.OrderByDescending(s => s.GetType().GetProperty(sortColumn).GetValue(s)).ToList().Skip(skip).Take(pageSize);
            else
                ugovorData = ugovorData.OrderBy(s => s.GetType().GetProperty((sortColumn == "") ? "Id" : sortColumn).GetValue(s)).ToList().Skip(skip).Take(pageSize);




            //    ////var vozniParkData = BexUow.VozniPark.GetVozniParkData().OrderByDescending(e => order).ToList().Skip(skip).Take(pageSize);

            if (!String.IsNullOrEmpty(searchTerms))
            {
                total = BexUow.Ugovor.GetSearchUgovorData(searchTerms).Count();

                ugovorData = BexUow.Ugovor.GetSearchUgovorData(searchTerms).Select(x =>
                                                         new UgovorIndexData
                                                         {
                                                             Id = x.Id,
                                                             Nosilac = x.Kontakt?.Naziv.ToString(),
                                                             Region = x.Region?.NazivSkraceni.ToString(),
                                                             BrojUgovora = x.UgovorBroj,
                                                             DatumUgovora = x.DatumUgovora,
                                                             VerzijaUgovora = x.UgovorVerzija,
                                                             DatumVerzije = x.DatumPoslednjeVerzije,
                                                             DatumObnove = x.DatumObnove,
                                                             Ugovorio = x.KorisniciPrograma?.Kontakt?.Naziv.ToString(),
                                                             Agent = x.Zaposleni?.Kontakt?.Naziv.ToString(),
                                                             //Posrednik = x.KorisniciPrograma?.Kontakt?.Naziv.ToString(),
                                                             //Aktivan = x.Aktivan
                                                         });
                if (sortOrder.Equals("desc"))
                {
                    ugovorData = ugovorData.OrderByDescending(s => s.GetType().GetProperty((sortColumn == "") ? "Id" : sortColumn).GetValue(s))
                                                 .ToList()
                                                 .Skip(skip)
                                                 .Take(pageSize);
                }
                else
                {
                    ugovorData = ugovorData.OrderBy(s => s.GetType().GetProperty((sortColumn == "") ? "Id" : sortColumn).GetValue(s))
                                                 .ToList()
                                                 .Skip(skip)
                                                 .Take(pageSize);
                }

                //    //    //vozniParkData = BexUow.VozniPark.GetSearchVozniParkData(searchTerms)
                //    //    //                                  .OrderByDescending(e => order)
                //    //    //                                  .ToList()
                //    //    //                                  .Skip(skip)
                //    //    //                                  .Take(pageSize);
            }
            //    ////List<VozniParkIndexData> dataVozniPark = new List<VozniParkIndexData>();
            //    ////foreach (var x in vozniParkData)
            //    ////{
            //    ////    var vozniPark = new VozniParkIndexData()
            //    ////    {

            //    ////        Id = x.Id,
            //    ////        Model = x.VozniParkModel?.NazivModela.ToString(),
            //    ////        GarazniBroj = x.GarazniBroj,
            //    ////        Registracija = x.Oznaka,
            //    ////        Region = x.Region?.NazivSkraceni.ToString(),
            //    ////        Status = x.VozniParkStatus?.NazivStatusa.ToString()

            //    ////    };
            //    ////    dataVozniPark.Add(vozniPark);
            //    ////}

            var jsonData = new TableJsonIndexData<UgovorIndexData>()
            {
                total = total,
                rows = ugovorData
            };
            var jsonResult = Json(jsonData, "TableJsonIndexData", JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;

        }
        //[HttpPost]
        //public string SpisakKolona(IEnumerable<FilterSpisakKolona> kolone)
        //{
        //    if (kolone.Count(k=>k.Selektovan) == 0)
        //    {
        //        return "Nije oznacena ni jedna kolona.";
        //    }
        //    else
        //    {

        //    }
        //}

        //public ActionResult Details(int? id, int? UgovorId)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    if (UgovorId != null)
        //    {
        //        var Ugovor = BexUow.Ugovor.Find(v => v.Id == UgovorId);
        //        ViewBag.UgovorId = Ugovor.Id;
        //        ViewBag.UgovorOznaka = Ugovor.Oznaka;
        //    }
        //    Ugovor ugovor = BexUow.Ugovor.GetUgovorData().ToList().Find(p => p.Id == id);
        //    if (ugovor == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(ugovor);
        //}

        // GET: KontaktTelefon/CreateI
        
        [ClaimsAuthentication(Resource = "Ugovor", Operation = "Create, All")]
        public ActionResult Create(int ugovorId = 0)
        {
            ViewBag.RegionId = new SelectList(BexUow.Region.AllAsNoTracking, "Id", "NazivSkraceni");
            ViewBag.KontaktIdNosilac = new SelectList(BexUow.Kontakts.AllAsNoTracking, "Id", "Naziv");
            //ViewBag.UgovorilacUserId = new SelectList(BexUow.KorisniciPrograma.AllAsNoTracking, "Id", "Naziv");
            //ViewBag.AgentZaposleniId = new SelectList(BexUow.KorisniciPrograma.AllAsNoTracking, "Id", "NazivZaposlenog");
            //ViewBag.UserIdDodao = new SelectList(BexUow.KorisniciPrograma.AllAsNoTracking, "Id", "Naziv");
            //ViewBag.PosrednikUserId = new SelectList(BexUow.Kontakts.AllAsNoTracking, "Id", "Naziv");
            //ViewBag.KompanijaId = new SelectList(BexUow.Kontakts.AllAsNoTracking, "Id", "Naziv");

            return View();
        }



        //GET: VozniPark/CreateAlarm/3
        //public ActionResult CreateAlarm(int vozniParkId = 0)
        //{
        //    ViewBag.VpId = vozniParkId;
        //    ViewBag.VpAlarmTip = new SelectList(BexUow.VozniParkDnevnikTip.AllAsNoTracking, "Id", "NazivTipa");
        //    //var uowCommandResult = BexUow.VozniPark.DodajAlarm(vozniParkId);
        //    return View();
        //}
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult CreateAlarm(AlarmIndexData alarm)
        //{
        //    //ViewBag.VpAlarmTip = new SelectList(BexUow.VozniParkDnevnikTip.AllAsNoTracking, "Id", "NazivTipa");
        //    var uowCommandResult = BexUow.VozniPark.DodajAlarm(alarm.VpId, alarm.VpAlarmTip, alarm.Datum, alarm.Km, alarm.DatumIsteka, alarm.KmIsteka, alarm.DatumAlarma, alarm.Napomena, alarm.Kolicina, alarm.Cena, alarm.IznosDin, alarm.IznosEur, alarm.Opis);
        //    return RedirectToAction("../VozniPark");
        //}

        //// POST: KontaktTelefon/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Ugovor ugovor)
        {
            if (ModelState.IsValid)
            {
                BexUow.Ugovor.Add(ugovor);
                var commandResult = BexUow.SubmitChanges();

                if (commandResult.IsSuccessful)
                { return RedirectToAction("../Ugovor"); }

                ExceptionSolver.PrepareModelState(ModelState, commandResult);
            }
            ViewBag.RegionId = new SelectList(BexUow.Region.AllAsNoTracking, "Id", "NazivSkraceni");
            ViewBag.KontaktIdNosilac = new SelectList(BexUow.Kontakts.AllAsNoTracking, "Id", "Naziv");
            //ViewBag.UgovorilacUserId = new SelectList(BexUow.Korisni.AllAsNoTracking, "Id", "Naziv");
            ViewBag.AgentZaposleniId = new SelectList(BexUow.Zaposleni.AllAsNoTracking, "Id", "NazivZaposlenog");
            //ViewBag.UserIdDodao = new SelectList(BexUow.Kontakts.AllAsNoTracking, "Id", "Naziv");
            //ViewBag.PosrednikUserId = new SelectList(BexUow.Kontakts.AllAsNoTracking, "Id", "Naziv");
            //ViewBag.KompanijaId = new SelectList(BexUow.Kontakts.AllAsNoTracking, "Id", "Naziv");
            return View(ugovor);

        }

        [ChildActionOnly]
        public ActionResult EditDetailsFakturisanjePartial(string ugovorId)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveFakturisanjePartial(UgovorFakturisanje ugovorFakturisanjeModel)
        {
            if (ModelState.IsValid)
            {
                BexUow.UgovorFakturisanje.Add(ugovorFakturisanjeModel);
                var commandResult = BexUow.SubmitChanges();

                if (commandResult.IsSuccessful)
                {
                    return RedirectToAction("Details", new { id = ugovorFakturisanjeModel.UgovorId });
                }
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }
        [ChildActionOnly]
        public ActionResult EditDetailsNapomenePartial(string ugovorId)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveNapomenePartial(UgovorNapomena ugovorNapomeneModel)
        {
            if (ModelState.IsValid)
            {
                BexUow.UgovorNapomena.Add(ugovorNapomeneModel);
                var commandResult = BexUow.SubmitChanges();

                if (commandResult.IsSuccessful)
                {
                    return RedirectToAction("Details", new { id = ugovorNapomeneModel.UgovorId });
                }
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }
        [ChildActionOnly]
        public ActionResult EditDetailsObecanoPartial(string ugovorId)
        {
            return View();
        }

        [ChildActionOnly]
        public ActionResult EditDetailsObracunCenaPartial(string ugovorId)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveObracunCenaPartial(UgovorObracunCena ugovorObracunCenaModel)
        {
            if (ModelState.IsValid)
            {
                BexUow.UgovorObracunCena.Add(ugovorObracunCenaModel);
                var commandResult = BexUow.SubmitChanges();

                if (commandResult.IsSuccessful)
                {
                    return RedirectToAction("Details", new { id = ugovorObracunCenaModel.UgovorId });
                }
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        [ChildActionOnly]
        public ActionResult EditDetailsVIPPartial(string ugovorId)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveVIPPartial(UgovorVip ugovorVIPModel)
        {
            if (ModelState.IsValid)
            {
                BexUow.UgovorVip.Add(ugovorVIPModel);
                var commandResult = BexUow.SubmitChanges();

                if (commandResult.IsSuccessful)
                {
                    return RedirectToAction("Details", new { id = ugovorVIPModel.UgovorId });
                }
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }
        //private void PopulateRegionsDropDownList(object selectedAddress = null)
        //{
        //    var departmentsQuery = from d in BexUow.Region.AllAsNoTracking
        //                               //orderby d.Ulica
        //                           select d;


        //    ViewBag.RegionId = new SelectList(departmentsQuery, "Id", "Region", selectedAddress);
        //}

        // GET: KontaktTelefon/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewBag.RegionId = new SelectList(BexUow.Region.AllAsNoTracking, "Id", "NazivSkraceni");
            ViewBag.KontaktIdNosilac = new SelectList(BexUow.Kontakts.AllAsNoTracking, "Id", "Naziv");
            //ViewBag.UgovorilacUserId = new SelectList(BexUow.Korisni.AllAsNoTracking, "Id", "Naziv");
            ViewBag.AgentZaposleniId = new SelectList(BexUow.Zaposleni.AllAsNoTracking, "Id", "NazivZaposlenog");
            //ViewBag.UserIdDodao = new SelectList(BexUow.Kontakts.AllAsNoTracking, "Id", "Naziv");
            //ViewBag.PosrednikUserId = new SelectList(BexUow.Kontakts.AllAsNoTracking, "Id", "Naziv");
            //ViewBag.KompanijaId = new SelectList(BexUow.Kontakts.AllAsNoTracking, "Id", "Naziv");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ugovor ugovor = BexUow.Ugovor.Find(id);
            if (ugovor == null)
            {
                return HttpNotFound();
            }
            return View(ugovor);
        }

        // POST: KontaktTelefon/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Ugovor ugovor)
        {
            if (ModelState.IsValid)
            {

                BexUow.Ugovor.Update(ugovor);
                var uowCommandResult = BexUow.SubmitChanges();

                if (uowCommandResult.IsSuccessful)
                { return RedirectToAction("Details", new { id = ugovor.Id }); }

                ExceptionSolver.PrepareModelState(ModelState, uowCommandResult);
            }
            ViewBag.RegionId = new SelectList(BexUow.Region.AllAsNoTracking, "Id", "NazivSkraceni");
            ViewBag.KontaktIdNosilac = new SelectList(BexUow.Kontakts.AllAsNoTracking, "Id", "Naziv");
            //ViewBag.UgovorilacUserId = new SelectList(BexUow.Korisni.AllAsNoTracking, "Id", "Naziv");
            ViewBag.AgentZaposleniId = new SelectList(BexUow.Zaposleni.AllAsNoTracking, "Id", "NazivZaposlenog");
            //ViewBag.UserIdDodao = new SelectList(BexUow.Kontakts.AllAsNoTracking, "Id", "Naziv");
            //ViewBag.PosrednikUserId = new SelectList(BexUow.Kontakts.AllAsNoTracking, "Id", "Naziv");
            //ViewBag.KompanijaId = new SelectList(BexUow.Kontakts.AllAsNoTracking, "Id", "Naziv");
            return View(ugovor);
        }
        [HttpGet]
        public ActionResult GetCenovnik(string query)
        {
            return Json(_GetCenovnik(query), JsonRequestBehavior.AllowGet);
        }

        private List<Autocomplete> _GetCenovnik(string query)
        {
            List<Autocomplete> cenovnikList = new List<Autocomplete>();
            try
            {
                var results = (from z in BexUow.Cenovnik.GetCenovnikAutocompleteData(query)
                               group z by new { Col1 = z.BrojCenovnika/*, Col2 = z.Kontakt.Naziv*/}
                               into grp
                               select new
                               {
                                   BrojCenovnika = grp.Key.Col1
                                   //NazivKorisnika = grp.Key.Col2
                               }).Take(10).ToList();

                foreach (var r in results)
                {
                    // create objects
                    Autocomplete models = new Autocomplete();

                    //models.Name = string.Format("{0}", r.Kontakt.Naziv.ToString());
                    models.Name = string.Format("{0}", r.BrojCenovnika/*, r.NazivKorisnika*/);
                    //models.Name = r.Naziv;
                    models.Id = 1;
                    cenovnikList.Add(models);
                }

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
            return cenovnikList;
        }
        [HttpGet]
        public ActionResult GetUser(string query)
        {
            return Json(_GetUser(query), JsonRequestBehavior.AllowGet);
        }

        private List<Autocomplete> _GetUser(string query)
        {
            List<Autocomplete> userList = new List<Autocomplete>();
            try
            {
                var results = (from z in BexUow.KorisniciPrograma.GetKorisniciProgramaAutocompliteData(query)
                               group z by new { Col1 = z.Id, Col2 = z.Kontakt.Naziv, Col3 = z.Region.NazivSkraceni }
                               into grp
                               select new
                               {
                                   IdKorisnika = grp.Key.Col1,
                                   NazivKorisnika = grp.Key.Col2,
                                   RegonKorisnika = grp.Key.Col3
                               }).Take(10).ToList();

                foreach (var r in results)
                {
                    // create objects
                    Autocomplete models = new Autocomplete();

                    //models.Name = string.Format("{0}", r.Kontakt.Naziv.ToString());
                    models.Name = string.Format("({0}) - {1} - {2}", r.IdKorisnika, r.NazivKorisnika, r.RegonKorisnika);
                    //models.Name = r.Naziv;
                    models.Id = r.IdKorisnika;
                    userList.Add(models);
                }

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
            return userList;
        }
        [HttpGet]
        public ActionResult GetZaposleni(string query)
        {
            return Json(_GetZaposleni(query), JsonRequestBehavior.AllowGet);
        }

        private List<Autocomplete> _GetZaposleni(string query)
        {
            List<Autocomplete> zaposleniList = new List<Autocomplete>();
            try
            {
                var results = (from z in BexUow.Zaposleni.GetZaposleniAutocompleteData(query)
                               group z by new { Col1 = z.Kontakt.Naziv }
                               into grp
                               select new
                               {
                                   NazivZaposlenog = grp.Key.Col1
                               }).Take(10).ToList();

                foreach (var r in results)
                {
                    // create objects
                    Autocomplete models = new Autocomplete();

                    //models.Name = string.Format("{0}", r.Kontakt.Naziv.ToString());
                    models.Name = string.Format("{0}", r.NazivZaposlenog);
                    //models.Name = r.Naziv;
                    models.Id = 1;
                    zaposleniList.Add(models);
                }

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

        [HttpGet]
        //public ActionResult SelectBoxModel(string term)
        //{
        //    //List<VozniParkModel> dataVozniPark = BexUow.VozniParkModel.GetAll().ToList();
        //    //if (!String.IsNullOrEmpty(term))
        //    //{
        //    //    dataVozniPark = dataVozniPark.Where(vp => vp.NazivModela.Contains(term)).ToList();
        //    //}

        //    //return Json(dataVozniPark, "VozniParkModel", JsonRequestBehavior.AllowGet);
        //}

        //public ActionResult GetModel(string query)
        //{
        //    return Json(_GetModel(query), JsonRequestBehavior.AllowGet);
        //}

        //private List<Autocomplete> _GetModel(string query)
        //{
        //    //List<Autocomplete> modelList = new List<Autocomplete>();
        //    //try
        //    //{
        //    //    var results = BexUow.VozniParkModel.GetAll().Where(m => m.NazivModela.Contains(query)).Take(10).ToList();

        //    //    foreach (var r in results)
        //    //    {
        //    //        // create objects
        //    //        Autocomplete models = new Autocomplete();

        //    //        //models.Name = string.Format("{0} {1}", r.NazivModela, r.NazivModela);
        //    //        models.Name = r.NazivModela;
        //    //        models.Id = r.Id;
        //    //        modelList.Add(models);
        //    //    }

        //    //}
        //    //catch (EntityCommandExecutionException eceex)
        //    //{
        //    //    if (eceex.InnerException != null)
        //    //    {
        //    //        throw eceex.InnerException;
        //    //    }
        //    //    throw;
        //    //}
        //    //catch
        //    //{
        //    //    throw;
        //    //}
        //    //return modelList;
        //}

        public ActionResult GetSub(string query)
        {
            return Json(_GetSub(query), JsonRequestBehavior.AllowGet);
        }

        private List<Autocomplete> _GetSub(string query)
        {
            List<Autocomplete> subList = new List<Autocomplete>();
            try
            {
                var results = BexUow.Kontakts.GetAll(true).Where(m => m.Naziv.Contains(query)).Take(10).ToList();

                foreach (var r in results)
                {
                    // create objects
                    Autocomplete models = new Autocomplete();

                    models.Name = string.Format("{0} {1}", r.Naziv, r.AdresaTekst);
                    //models.Name = r.Naziv;
                    models.Id = r.Id;
                    subList.Add(models);
                }

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
            return subList;
        }

        public ActionResult DetailsUgovorFakturisanje(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.UgovorId = id.Value;

            return View();
        }
        [HttpGet]
        public ActionResult GetUgovorFakturisanjeData(int? id)
        {
            var detailsUgovorFakturisanje = (from ugovorFakturisanje in BexUow.UgovorFakturisanje.AllAsNoTracking
                                             select new
                                             {
                                                 ugovorFakturisanje.UgovorId,
                                                 ugovorFakturisanje.FakturaDana,
                                                 ugovorFakturisanje.RabatProcenat,
                                                 ugovorFakturisanje.RabatMinBrojPaketa,
                                                 ugovorFakturisanje.RabatProcenatZaBrojPaketa,
                                                 ugovorFakturisanje.RabatMinIznosFakture,
                                                 ugovorFakturisanje.RabatProcenatZaIznosFakture,
                                                 ugovorFakturisanje.ValutaDana,
                                             }).Where(s => s.UgovorId == id).ToList();

            return Json(detailsUgovorFakturisanje, "", JsonRequestBehavior.AllowGet);
        }
        public ActionResult DetailsUgovorObracunCena(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.UgovorId = id.Value;

            return View();
        }
        [HttpGet]
        public ActionResult GetUgovorObracunCenaData(int? id)
        {
            var detailsUgovorObracunCena = (from ugovorObracunCena in BexUow.UgovorObracunCena.AllAsNoTracking
                                            select new
                                            {
                                                ugovorObracunCena.UgovorId,
                                                ugovorObracunCena.PovlasceneCeneKlijentuId,
                                                ugovorObracunCena.NaplataId,
                                                ugovorObracunCena.BezZastitneCene,
                                                ugovorObracunCena.NaplataPoFakturi,
                                                ugovorObracunCena.FakturaProc,
                                                ugovorObracunCena.ZastitnaCenaFak,
                                                ugovorObracunCena.DodatakZaGorivo,
                                            }).Where(s => s.UgovorId == id).ToList();

            return Json(detailsUgovorObracunCena, "", JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult GetUgovorCenovnik1Data(int? id)
        {
            var detailsUgovorCenovnik1 = (from ugovorCenovnik1 in BexUow.Ugovor.AllAsNoTracking
                                          join cenovnik1 in BexUow.Cenovnik.AllAsNoTracking on ugovorCenovnik1.Cenovnik1Id equals cenovnik1.IdCenovnika

                                          select new
                                          {
                                              ugovorCenovnik1.Id,
                                              cenovnik1.BrojCenovnika,
                                              cenovnik1.DatumCenovnika,
                                              cenovnik1.OpisCenovnika
                                          }).Where(s => s.Id == id).ToList();

            return Json(detailsUgovorCenovnik1, "", JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult GetUgovorCenovnik2Data(int? id)
        {
            var detailsUgovorCenovnik2 = (from ugovorCenovnik2 in BexUow.Ugovor.AllAsNoTracking
                                          join cenovnik2 in BexUow.Cenovnik.AllAsNoTracking on ugovorCenovnik2.Cenovnik1Id equals cenovnik2.IdCenovnika

                                          select new
                                          {
                                              ugovorCenovnik2.Id,
                                              cenovnik2.BrojCenovnika,
                                              cenovnik2.DatumCenovnika,
                                              cenovnik2.OpisCenovnika
                                          }).Where(s => s.Id == id).ToList();

            return Json(detailsUgovorCenovnik2, "", JsonRequestBehavior.AllowGet);
        }
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ugovor ugovor = BexUow.Ugovor.Find(id);
            if (ugovor == null)
            {
                return HttpNotFound();
            }
            return View(ugovor);
        }

        // POST: KontaktTelefon/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Ugovor ugovor = BexUow.Ugovor.Find(id);
            //db.KontaktTelefons.Remove(kontaktTelefon);
            //db.SaveChanges();
            return RedirectToAction("Index");
        }

        //public ActionResult DodajZonu(int id)
        //{
        //    var uowCommandResult = BexUow.VozniPark.UbaciUzonu(id);
        //    return RedirectToAction("Index");
        //}

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
