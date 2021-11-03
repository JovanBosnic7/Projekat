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
   
    [ClaimsAuthentication(Resource = "Trebovanje", Operation = "Read, All")]
    public class TrebovanjeController : Controller
    {
        public TrebovanjeController() : this(new BexUow(), new ExceptionSolver())
        { }
        public TrebovanjeController(
            IBexUow bexUow,
            IExceptionSolver exceptionSolver)
        {
            BexUow = bexUow;
            ExceptionSolver = exceptionSolver;

        }
        public ActionResult Index(int? statusId)
        {
            TempData.ToList().ForEach(keyValue =>
                ModelState.AddModelError("", keyValue.Value.ToString()));
            TempData.Clear();
            //ViewBag.Model = new SelectList(BexUow.VozniParkModel.AllAsNoTracking, "NazivModela", "NazivModela");
            //ViewBag.Marka = new SelectList(BexUow.VozniParkMarka.AllAsNoTracking, "NazivMarke", "NazivMarke");
            //ViewBag.Statusi = new SelectList(BexUow.VozniParkStatus.AllAsNoTracking, "NazivStatusa", "NazivStatusa");
            //ViewBag.Kategorija = new SelectList(BexUow.VozniParkKategorija.AllAsNoTracking, "KategorijaNaziv", "KategorijaNaziv");
            //ViewBag.VpOprema = BexUow.VozniParkDnevnikTip.AllAsNoTracking.Where(x => x.GrupaId == 1).ToList();
            //ViewBag.StatusId = (statusId == null) ? 0 : statusId;
            return View();

        }

        [HttpGet]      
        public ActionResult GetTrebovanje(int pageSize, int pageNumber, string sortColumn, string sortOrder, string search, string searchColumn, string searchTerms)
        {

            var skip = (pageNumber - 1) * pageSize;

            var trebovanjeData = BexUow.Trebovanje.GetAll(true).Select(x =>
                                                         new TrebovanjeIndexData
                                                         {
                                                             IdTrebovanja = x.IdTrebovanja,
                                                             DatumUnosa = x.DatumTrebovanja,
                                                             Trebuje = x.Kontakt.Naziv,
                                                             Adresa = x.Kontakt.AdresaTekst,
                                                             ProfitniCentar = "",
                                                             Uneo = x.UserUneo.Kontakt.Naziv,
                                                             DatumIzdavanja = x.DatumIzdavanja,
                                                             Izdao = x.UserIzdao.Kontakt.Naziv
                                                         }).AsEnumerable();

            if (!String.IsNullOrEmpty(searchTerms))
            {
                trebovanjeData = GetSearchTrebovanjeData(searchTerms, trebovanjeData)
                                                    .Select(x =>
                                                         new TrebovanjeIndexData
                                                         {
                                                             IdTrebovanja = x.IdTrebovanja,
                                                             DatumUnosa = x.DatumUnosa,
                                                             Trebuje = x.Trebuje,
                                                             Adresa = x.Adresa,
                                                             ProfitniCentar = x.ProfitniCentar,
                                                             Uneo = x.Uneo,
                                                             DatumIzdavanja = x.DatumIzdavanja,
                                                             Izdao = x.Izdao
                                                         }).AsEnumerable(); 
            }

            var total = trebovanjeData.Count();

            if (sortColumn == "")
            {
                sortColumn = "IdTrebovanja";
                sortOrder = "desc";
            }

            if (sortOrder.Equals("desc"))
                trebovanjeData = trebovanjeData.OrderByDescending(s => s.GetType().GetProperty((sortColumn == "") ? "IdTrebovanja" : sortColumn).GetValue(s)).ToList().Skip(skip).Take(pageSize);
            else
                trebovanjeData = trebovanjeData.OrderBy(s => s.GetType().GetProperty((sortColumn == "") ? "IdTrebovanja" : sortColumn).GetValue(s)).ToList().Skip(skip).Take(pageSize);
            

            var jsonData = new TableJsonIndexData<TrebovanjeIndexData>()
            {
                total = total,
                rows = trebovanjeData
            };
            var jsonResult = Json(jsonData, "TableJsonIndexData", JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
            
        }

        public IEnumerable<TrebovanjeIndexData> GetSearchTrebovanjeData(string searchTerms, IEnumerable<TrebovanjeIndexData> searchDataSet)
        {
            string[] terms = searchTerms.Split(',');
            string searchColumn = "";
            string searchTxt = "";

            string searchColumnModel = "";
            string searchColumnGarazniBroj = "";
            string searchColumnRegistracija = "";
            string searchColumnOznakaStara = "";
            string searchColumnRegion = "";
            string searchColumnStatus = "";
            string searchColumnMarka = "";
            string searchColumnOpis = "";
            string searchColumnSasija = "";
            string searchColumnKategorija = "";
            int searchColumnGodiste = 0;
            string searchColumnNapomena = "";
           
            int searchColumnBrojZone = 0;
            string searchColumnFirmaOS = "";
            string searchColumnNamena = "";
            string searchColumnBarkod = "";
            decimal searchColumnPropisanaPotrosnja = 0;
            string searchColumnSektor = "";


            var trebovanje = searchDataSet;



            foreach (string t in terms)
            {
                string[] searchCT = t.Split(':');
                searchColumn = searchCT[0];
                searchTxt = searchCT[1];


                //if (searchColumn.Equals("Model") && !String.IsNullOrEmpty(searchTxt))
                //{
                //    searchColumnModel = searchTxt;
                //    vozniPark = vozniPark.Where(k => k.Model.ToString().ToUpper().Equals(searchColumnModel.ToUpper()));

                //}
                //else if (searchColumn.Equals("Marka") && !String.IsNullOrEmpty(searchTxt))
                //{
                //    searchColumnMarka = searchTxt;
                //    vozniPark = vozniPark.Where(k => k.Marka.ToUpper().Equals(searchColumnMarka.ToUpper()));
                //}
                //else if (searchColumn.Equals("Registracija") && !String.IsNullOrEmpty(searchTxt))
                //{
                //    searchColumnRegistracija = searchTxt;
                //    vozniPark = vozniPark.Where(k => k.Registracija.ToUpper().Contains(searchColumnRegistracija.ToUpper()));
                //}
                //else if (searchColumn.Equals("OznakaStara") && !String.IsNullOrEmpty(searchTxt))
                //{
                //    searchColumnOznakaStara = searchTxt;
                //    vozniPark = vozniPark.Where(k => k.OznakaStara.ToUpper().Contains(searchColumnOznakaStara.ToUpper()));
                //}
                //else if (searchColumn.Equals("GarazniBroj") && !String.IsNullOrEmpty(searchTxt))
                //{
                //    searchColumnGarazniBroj = searchTxt;
                //    vozniPark = vozniPark.Where(k => k.GarazniBroj.ToUpper().Contains(searchColumnGarazniBroj.ToUpper()));
                //}
                //else if (searchColumn.Equals("Region") && !String.IsNullOrEmpty(searchTxt))
                //{
                //    searchColumnRegion = searchTxt;
                //    vozniPark = vozniPark.Where(k => k.Region.ToUpper().Equals(searchColumnRegion.ToUpper()));
                //}
                //else if (searchColumn.Equals("Statusi") && !String.IsNullOrEmpty(searchTxt))
                //{
                //    searchColumnStatus = searchTxt;
                //    vozniPark = vozniPark.Where(k => k.Status.ToUpper().Equals(searchColumnStatus.ToUpper()));
                //}
               
                //else if (searchColumn.Equals("Opis") && !String.IsNullOrEmpty(searchTxt))
                //{
                //    searchColumnOpis = searchTxt;
                //    vozniPark = vozniPark.Where(k => k.Opis.ToUpper().Contains(searchColumnOpis.ToUpper()));
                //}
                //else if (searchColumn.Equals("Sasija") && !String.IsNullOrEmpty(searchTxt))
                //{
                //    searchColumnSasija = searchTxt;
                //    vozniPark = vozniPark.Where(k => k.Sasija.ToUpper().Contains(searchColumnSasija.ToUpper()));
                //}
                //else if (searchColumn.Equals("Godiste") && !String.IsNullOrEmpty(searchTxt))
                //{
                //    searchColumnGodiste = System.Convert.ToInt32(searchTxt);
                //    vozniPark = vozniPark.Where(k => k.Godiste.Equals(searchColumnGodiste));
                //}
                //else if (searchColumn.Equals("Napomena") && !String.IsNullOrEmpty(searchTxt))
                //{
                //    searchColumnNapomena = searchTxt;
                //    vozniPark = vozniPark.Where(k => k.Napomena.ToUpper().Contains(searchColumnNapomena.ToUpper()));
                //}
                //else if (searchColumn.Equals("DatumRegistracije") && !String.IsNullOrEmpty(searchTxt))
                //{
                //    //searchColumnDatumRegistracije = searchTxt;
                //    string[] arrDatum = searchTxt.Split('/');
                //    DateTime thisDate1 = new DateTime(int.Parse(arrDatum[2]), int.Parse(arrDatum[0]), int.Parse(arrDatum[1]));
                //    vozniPark = vozniPark.Where(k => k.DatumRegistracije.Equals(thisDate1));
                //}
                //else if (searchColumn.Equals("DatumZaduzenja") && !String.IsNullOrEmpty(searchTxt))
                //{
                //    //searchColumnDatumZaduzenja = searchTxt;
                //    string[] arrDatum = searchTxt.Split('/');
                //    DateTime thisDate1 = new DateTime(int.Parse(arrDatum[2]), int.Parse(arrDatum[0]), int.Parse(arrDatum[1]));
                //    vozniPark = vozniPark.Where(k => k.DatumZaduzenja.Equals(thisDate1));
                //}
                //else if (searchColumn.Equals("BrojZone") && !String.IsNullOrEmpty(searchTxt))
                //{
                //    searchColumnBrojZone = System.Convert.ToInt32(searchTxt);
                //    vozniPark = vozniPark.Where(k => k.BrojZone.Equals(searchColumnBrojZone));
                //}
                //else if (searchColumn.Equals("FirmaOS") && !String.IsNullOrEmpty(searchTxt))
                //{
                //    searchColumnFirmaOS = searchTxt;
                //    vozniPark = vozniPark.Where(k => k.FirmaOS.ToUpper().Equals(searchColumnFirmaOS.ToUpper()));
                //}
                //else if (searchColumn.Equals("Namena") && !String.IsNullOrEmpty(searchTxt))
                //{
                //    searchColumnNamena = searchTxt;
                //    vozniPark = vozniPark.Where(k => k.Opis.ToUpper().Contains(searchColumnNamena.ToUpper()));
                //}
                //else if (searchColumn.Equals("Barkod") && !String.IsNullOrEmpty(searchTxt))
                //{
                //    searchColumnBarkod = searchTxt;
                //    vozniPark = vozniPark.Where(k => k.Barkod.ToUpper().Contains(searchColumnBarkod.ToUpper()));
                //}
                //else if (searchColumn.Equals("PropisanaPotrosnja") && !String.IsNullOrEmpty(searchTxt))
                //{
                //    searchColumnPropisanaPotrosnja = System.Convert.ToDecimal(searchTxt); ;
                //    vozniPark = vozniPark.Where(k => k.PropisanaPotrosnja.Equals(searchColumnPropisanaPotrosnja));
                //}
                //else if (searchColumn.Equals("Sektor") && !String.IsNullOrEmpty(searchTxt))
                //{
                //    searchColumnSektor = searchTxt;
                //    vozniPark = vozniPark.Where(k => k.Sektor.ToUpper().Contains(searchColumnSektor.ToUpper()));
                //}
                //else if (searchColumn.Equals("Kategorija") && !String.IsNullOrEmpty(searchTxt))
                //{
                //    searchColumnKategorija = searchTxt;
                //    vozniPark = vozniPark.Where(k => k.Kategorija.ToUpper().Equals(searchColumnKategorija.ToUpper()));
                //}

            }
            return trebovanje;
        }

        //    [ClaimsAuthentication(Resource = "VozniPark", Operation = "ClickAlarmOprema, All")]
        //    public JsonResult CreateAlarmOprema(SelectedValue values) 
        //{
        //    int opremaId = System.Convert.ToInt32(values.Id);
          

        //    foreach (var val in values.SelectedValues)
        //    {
        //        int vpId = val.Id;
              
        //        CreateAlarm(new AlarmIndexData
        //        {
        //           VpId = vpId,
        //           VpAlarmTip = opremaId,
        //           Datum = DateTime.Today

        //        });

                
        //    }
           
        //    return Json(new { success = "true" });
        //}



        //[ClaimsAuthentication(Resource = "VozniPark", Operation = "ClickDodajAlarm, All")]
        //public ActionResult CreateAlarm(int vozniParkId = 0)
        //{
        //    VozniPark vozniPark = BexUow.VozniPark.Find(vozniParkId);
        //    ViewBag.VpId = vozniParkId;
        //    ViewBag.RegOznaka = vozniPark.Oznaka;

        //    var putniNalogLast = BexUow.PutniNalog.AllAsNoTracking.Where(x => x.VpId == vozniParkId).OrderByDescending(z => z.Id).Take(1).FirstOrDefault();
            
        //    ViewBag.VpAlarmTip = new SelectList(BexUow.VozniParkDnevnikTip.AllAsNoTracking.Where(x=>x.GrupaId == 11 || x.GrupaId == 21 || x.GrupaId == 31), "Id", "NazivTipa");

        //    var alarmIndexData = new AlarmIndexData
        //    {
        //        Datum = DateTime.Today,
        //        Km = (putniNalogLast == null) ? 0 : putniNalogLast.KmStop,
        //        Kolicina = 1
        //    };

        //    return View(alarmIndexData);
        //}

        //[HttpGet]
        //public ActionResult GetVrednostiAlarma(int tipId)
        //{
        //    var dnevnikTip = BexUow.VozniParkDnevnikTip.AllAsNoTracking.FirstOrDefault(x => x.Id == tipId);
        //    int grupaId = dnevnikTip.GrupaId;
        //    DateTime datumIsteka = DateTime.Today;
        //    int? kmIsteka = 0;
        //    int? danaIsteka = 0;

        //    if (grupaId == 11)
        //    {
        //        int? danaIstekaDefault = dnevnikTip.DanaIstekaDefault;
        //        danaIsteka = dnevnikTip.DanaIstekaDefault;
        //        datumIsteka = DateTime.Today.AddDays(Convert.ToDouble(danaIstekaDefault));
        //        kmIsteka = 0;
                

        //    }
        //    else
        //    {
        //        var kmIstekaDefault = dnevnikTip.KmIstekaDefault;
        //        datumIsteka = DateTime.Today;
        //        kmIsteka = kmIstekaDefault;
        //    }

        //    var vrednostiAlarma = new
        //    {
        //        datumIsteka = datumIsteka.ToShortDateString().Replace("/", "-"),
        //        kmIsteka
        //    };

        //    return Json(vrednostiAlarma, JsonRequestBehavior.AllowGet);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult CreateAlarm(AlarmIndexData alarm)
        //{

        //    //var uowCommandResult = BexUow.VozniPark.DodajAlarm(alarm.VpId, alarm.VpAlarmTip, alarm.Datum, alarm.Km, alarm.DatumIsteka, alarm.KmIsteka, alarm.DatumAlarma, alarm.Napomena,alarm.Kolicina, alarm.Cena, alarm.IznosDin,alarm.IznosEur, alarm.Opis);
            

        //    int? regionId = BexUow.VozniPark.AllAsNoTracking.FirstOrDefault(x => x.Id == alarm.VpId).RegionId;

        //    VozniParkDnevnik vpDnevnik = new VozniParkDnevnik
        //    {
        //        VpId = alarm.VpId,
        //        RegionId = regionId,
        //        Datum = alarm.Datum,
        //        Km = alarm.Km,
        //        DnevnikTipId = alarm.VpAlarmTip,
        //        IznosDin = alarm.IznosDin,
        //        IznosEur = alarm.IznosEur,
        //        Opis = alarm.Opis,
        //        Napomena = alarm.Napomena,
        //        Cena = alarm.Cena
        //    };
        //    BexUow.VozniParkDnevnik.Add(vpDnevnik);
        //    var commandResult = BexUow.SubmitChanges();
        //    if (commandResult.IsSuccessful)
        //    {
        //        VozniParkAlarm vpAlarm = BexUow.VozniParkAlarm.Find(x => x.VpId == alarm.VpId && x.VpAlarmTip == alarm.VpAlarmTip);

        //        if(vpAlarm == null)
        //        {
        //            BexUow.VozniParkAlarm.Add(new VozniParkAlarm
        //            {
        //                VpId = alarm.VpId,
        //                VpAlarmTip = alarm.VpAlarmTip,
        //                VpDnevnikId = vpDnevnik.Id,
        //                Datum = alarm.Datum,
        //                Km = alarm.Km,
        //                DatumIsteka = alarm.DatumIsteka,
        //                KmIsteka = alarm.KmIsteka,
        //                DatumAlarma = alarm.DatumAlarma,
        //                Napomena = alarm.Napomena
                
        //            });
        //        }
        //        else
        //        {
        //            vpAlarm.VpId = alarm.Id;
        //            vpAlarm.VpAlarmTip = alarm.VpAlarmTip;
        //            vpAlarm.VpDnevnikId = vpDnevnik.Id;
        //            vpAlarm.Datum = alarm.Datum;
        //            vpAlarm.Km = alarm.Km;
        //            vpAlarm.DatumIsteka = alarm.DatumIsteka;
        //            vpAlarm.KmIsteka = alarm.KmIsteka;
        //            vpAlarm.DatumAlarma = alarm.DatumAlarma;
        //            vpAlarm.Napomena = alarm.Napomena;
        //            BexUow.VozniParkAlarm.Update(vpAlarm);
        //        }

        //        commandResult = BexUow.SubmitChanges();
        //    }

        //        return RedirectToAction("../VozniPark"); 
        //}

        //[ClaimsAuthentication(Resource = "VozniPark", Operation = "EditAlarm, All")]
        //public ActionResult EditAlarm(int alarmId, string oznaka)
        //{
        //    var vpAlarm = BexUow.VozniParkAlarm.GetAll().Where(x => x.Id == alarmId).Select(x =>
        //                                                   new AlarmIndexData
        //                                                   {
        //                                                       Id = x.Id,
        //                                                       VpId = x.VpId,
        //                                                       Vozilo = x.VozniPark.NazivVozila,
        //                                                       Registracija = oznaka,
        //                                                       Alarm = x.VozniParkDnevnikTip.NazivTipa,
        //                                                       VpAlarmTip = x.VpAlarmTip,
        //                                                       Datum = x.Datum,
        //                                                       DatumAlarma = x.DatumAlarma,
        //                                                       Km = x.Km,
        //                                                       DatumIsteka = x.DatumIsteka,
        //                                                       KmIsteka = x.KmIsteka,
        //                                                       Napomena = x.Napomena,
        //                                                       Kolicina = x.VozniParkDnevnik.Kolicina,
        //                                                       Cena = x.VozniParkDnevnik.Cena,
        //                                                       IznosDin = x.VozniParkDnevnik.IznosDin,
        //                                                       IznosEur = x.VozniParkDnevnik.IznosEur,
        //                                                       Opis = x.VozniParkDnevnik.Opis
        //                                                   }).FirstOrDefault();

        //    ViewBag.VpId = vpAlarm.VpId;
        //    ViewBag.RegOznaka = oznaka;

        //    ViewBag.VpAlarmTip = new SelectList(BexUow.VozniParkDnevnikTip.AllAsNoTracking, "Id", "NazivTipa", vpAlarm.VpAlarmTip);

        //    return View(vpAlarm);
        //}

        [ClaimsAuthentication(Resource = "Trebovanje", Operation = "Create, All")]
        public ActionResult Create()
        {
            //ViewBag.KarakteristikaId = new SelectList(BexUow.VozniParkKarakteristike.AllAsNoTracking, "Id", "Naziv");
            //ViewBag.BojaId = new SelectList(BexUow.VozniParkBoja.AllAsNoTracking, "Id", "NazivBoje");
            
            return View();
        }

        //// POST: VozniPark/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Trebovanje trebovanje)
        {
            if (ModelState.IsValid)
            {
                BexUow.Trebovanje.Add(trebovanje);
                var commandResult = BexUow.SubmitChanges();

                //isti problem id i pretakanja
                if (commandResult.IsSuccessful)
                { return RedirectToAction("Edit", "Trebovanje", new { id = trebovanje.IdTrebovanja }); }

                ExceptionSolver.PrepareModelState(ModelState, commandResult);
            }

            
            return View(trebovanje);

        }

        [ClaimsAuthentication(Resource = "Trebovanje", Operation = "Edit, All")]
        public ActionResult Edit(int id)
        {
             
            Trebovanje trebovanje = BexUow.Trebovanje.Find(id);
            if (trebovanje == null)
            {
                return HttpNotFound();
            }
            //ViewBag.VoziloId = id;
            //ViewBag.VoziloRegOznaka = vozniPark.Oznaka;

            //ViewBag.KarakteristikaId = new SelectList(BexUow.VozniParkKarakteristike.AllAsNoTracking, "Id", "Naziv", vozniPark.KarakteristikaId );
            //ViewBag.BojaId = new SelectList(BexUow.VozniParkBoja.AllAsNoTracking, "Id", "NazivBoje", vozniPark.BojaId);

            return View(trebovanje);
        }

        // POST: VozniPark/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Trebovanje trebovanje)
        {
            //if (ModelState.IsValid)
            //{
            //    var trebovanjeUpdate = BexUow.VozniPark.Find(vozniPark.Id);

            //    voziloUpdate.GarazniBroj = vozniPark.GarazniBroj;
            //    voziloUpdate.NazivVozila = vozniPark.NazivVozila;
            //    voziloUpdate.Oznaka = vozniPark.Oznaka;
            //    voziloUpdate.OznakaStara = vozniPark.OznakaStara;
            //    voziloUpdate.Aktivno = vozniPark.Aktivno;
            //    voziloUpdate.DatumRegistracije = vozniPark.DatumRegistracije;
            //    voziloUpdate.ModelId = vozniPark.ModelId;
            //    voziloUpdate.BarKod = vozniPark.BarKod;
            //    voziloUpdate.KarakteristikaId = vozniPark.KarakteristikaId;
            //    voziloUpdate.KmNabavna = vozniPark.KmNabavna;
            //    voziloUpdate.BojaId = vozniPark.BojaId;
            //    voziloUpdate.KmLimit = vozniPark.KmLimit;
            //    voziloUpdate.LinijskoVozilo = vozniPark.LinijskoVozilo;


            //    BexUow.VozniPark.Update(voziloUpdate);
            //    var uowCommandResult = BexUow.SubmitChanges();

            //    if (uowCommandResult.IsSuccessful)
            //    { return RedirectToAction("Edit", "VozniPark", new { id = vozniPark.Id }); }

            //    ExceptionSolver.PrepareModelState(ModelState, uowCommandResult);
            //}

            return View(trebovanje);
        }

        

        //public ActionResult GetModel(string query)
        //{
        //    return Json(_GetModel(query), JsonRequestBehavior.AllowGet);
        //}
        //private List<Autocomplete> _GetModel(string query)
        //{
        //    List<Autocomplete> modelList = new List<Autocomplete>();
        //    try
        //    {
        //        modelList = BexUow.VozniParkModel.AllAsNoTracking
        //                                           .Where(x => x.NazivModela.ToUpper().Contains(query.ToUpper()))
        //                                           .Select(a => new Autocomplete
        //                                           {
        //                                               Name = a.NazivModela,
        //                                               Id = a.Id
        //                                           }
        //                                           )
        //                                           .ToList();
        //    }
        //    catch (EntityCommandExecutionException eceex)
        //    {
        //        if (eceex.InnerException != null)
        //        {
        //            throw eceex.InnerException;
        //        }
        //        throw;
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //    return modelList;
        //}

        //[ChildActionOnly]
        //public ActionResult CreateZaduzenjeVozilaPartial(int voziloId)
        //{
        //    var vozilo = BexUow.VozniPark.GetVozniParkNaziviData().FirstOrDefault(x => x.Id == voziloId);

        //    ViewBag.VoziloId = voziloId;
        //    ViewBag.OdgovornoLice = vozilo.Kontakt?.Naziv;  
        //    ViewBag.Vozac = vozilo.KontaktVozac?.Naziv;
        //    ViewBag.TipId = new SelectList(BexUow.VozniParkTip.AllAsNoTracking, "Id", "Opis", vozilo.TipId);
        //    ViewBag.StatusVozilaId = new SelectList(BexUow.VozniParkStatus.AllAsNoTracking, "Id", "NazivStatusa", vozilo.StatusVozilaId);
        //    ViewBag.RegionId = new SelectList(BexUow.Region.AllAsNoTracking, "Id", "NazivSkraceni", vozilo.RegionId);
        //    ViewBag.SektorId = new SelectList(BexUow.Sektor.AllAsNoTracking, "Id", "NazivSektora", vozilo.SektorId);
        //    ViewBag.FirmaOsId = new SelectList(BexUow.Firma.AllAsNoTracking.Where(f => f.FirmaIdVP>0), "FirmaIdVP", "Naziv", vozilo.FirmaOSid);
        //    ViewBag.AddZaduzenjeVozila = false;
           
        //    return View(vozilo);

        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult CreateZaduzenjeVozilaPartial(VozniPark vozniPark)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var vozniParkUpdate = BexUow.VozniPark.Find(x => x.Id == vozniPark.Id);

        //        vozniParkUpdate.TipId = vozniPark.TipId;
        //        vozniParkUpdate.StatusVozilaId = vozniPark.StatusVozilaId;
        //        vozniParkUpdate.RegionId = vozniPark.RegionId;
        //        vozniParkUpdate.SektorId = vozniPark.SektorId;
        //        vozniParkUpdate.SubOdgovornoLiceId = vozniPark.SubOdgovornoLiceId;
        //        vozniParkUpdate.SubVozacId = vozniPark.SubVozacId;
        //        vozniParkUpdate.DatumZaduzenja = vozniPark.DatumZaduzenja;
        //        vozniParkUpdate.Opis = vozniPark.Opis;
        //        vozniParkUpdate.Napomena = vozniPark.Napomena;
        //        vozniParkUpdate.FirmaOSid = vozniPark.FirmaOSid;
        //        vozniParkUpdate.PropisanaPotrosnja = vozniPark.PropisanaPotrosnja;


        //        BexUow.VozniPark.Update(vozniParkUpdate);
        //        var commandResult = BexUow.SubmitChanges();

        //        if (commandResult.IsSuccessful)
        //        {
        //            ViewBag.AddZaduzenjeVozila = true;
        //            ViewBag.TipId = new SelectList(BexUow.VozniParkTip.AllAsNoTracking, "Id", "Opis", vozniPark.TipId);
        //            ViewBag.StatusVozilaId = new SelectList(BexUow.VozniParkStatus.AllAsNoTracking, "Id", "NazivStatusa", vozniPark.StatusVozilaId);
        //            ViewBag.RegionId = new SelectList(BexUow.Region.AllAsNoTracking, "Id", "NazivSkraceni", vozniPark.RegionId);
        //            ViewBag.SektorId = new SelectList(BexUow.Sektor.AllAsNoTracking, "Id", "NazivSektora", vozniPark.SektorId);
        //            ViewBag.FirmaOsId = new SelectList(BexUow.Firma.AllAsNoTracking, "FirmaIdVP", "Naziv", vozniPark.FirmaOSid);
        //            ViewBag.VoziloId = vozniPark.Id;
        //            ViewBag.OdgovornoLice = "";
        //                //vozniPark.Kontakt.Naziv;
        //            //ViewBag.Vozac = vozniPark.KontaktVozac.Naziv;
        //            return PartialView(vozniPark);
        //        }

        //        ExceptionSolver.PrepareModelState(ModelState, commandResult);
        //    }

        //    return View(vozniPark);

        //}

        //[ChildActionOnly]
        //public ActionResult CreateTehnickeKarakteristikeVozilaPartial(int voziloId)
        //{
        //    //int zaposleniIdInt = System.Convert.ToInt32(zaposleniId);
        //    var vozilo = BexUow.VozniPark.Find(x => x.Id == voziloId);

        //    ViewBag.VoziloId = voziloId;
        //    ViewBag.GorivoId = new SelectList(BexUow.Gorivo.AllAsNoTracking, "Id", "NazivGoriva", vozilo.GorivoId);
        //    ViewBag.MenjacId = new SelectList(BexUow.VozniParkMenjac.AllAsNoTracking, "Id", "NazivMenjaca", vozilo.MenjacId);
        //    ViewBag.KaroserijaId = new SelectList(BexUow.VozniParkKaroserija.AllAsNoTracking, "Id", "NazivKaroserije", vozilo.KaroserijaId);
        //    ViewBag.KategorijaId = new SelectList(BexUow.VozniParkKategorija.AllAsNoTracking, "Id", "KategorijaNaziv", vozilo.KategorijaId);
        //    ViewBag.AddTehnickeVozila = false;

        //    return View(vozilo);

        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult CreateTehnickeKarakteristikeVozilaPartial(VozniPark vozniPark)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var vozniParkUpdate = BexUow.VozniPark.Find(x => x.Id == vozniPark.Id);

        //        vozniParkUpdate.Sasija = vozniPark.Sasija;
        //        vozniParkUpdate.Motor = vozniPark.Motor;
        //        vozniParkUpdate.GodinaProizvodnje = vozniPark.GodinaProizvodnje;
        //        vozniParkUpdate.Nosivost = vozniPark.Nosivost;
        //        vozniParkUpdate.Opis = vozniPark.Opis;
        //        vozniParkUpdate.Masa = vozniPark.Masa;
        //        vozniParkUpdate.BrojVrata = vozniPark.BrojVrata;
        //        vozniParkUpdate.Snaga = vozniPark.Snaga;
        //        vozniParkUpdate.TipVozila = vozniPark.TipVozila;
        //        vozniParkUpdate.BrojTockova = vozniPark.BrojTockova;
        //        vozniParkUpdate.GorivoId = vozniPark.GorivoId;
        //        vozniParkUpdate.MenjacId = vozniPark.MenjacId;
        //        vozniParkUpdate.KaroserijaId = vozniPark.KaroserijaId;
        //        vozniParkUpdate.RezervoarZapremina = vozniPark.RezervoarZapremina;
        //        vozniParkUpdate.BrojKljuceva = vozniPark.BrojKljuceva;
        //        vozniParkUpdate.Oprema = vozniPark.Oprema;
        //        vozniParkUpdate.KategorijaId = vozniPark.KategorijaId;

        //        BexUow.VozniPark.Update(vozniParkUpdate);
        //        var commandResult = BexUow.SubmitChanges();

        //        if (commandResult.IsSuccessful)
        //        {
        //            ViewBag.VoziloId = vozniPark.Id;
        //            ViewBag.GorivoId = new SelectList(BexUow.Gorivo.AllAsNoTracking, "Id", "NazivGoriva", vozniPark.GorivoId);
        //            ViewBag.MenjacId = new SelectList(BexUow.VozniParkMenjac.AllAsNoTracking, "Id", "NazivMenjaca", vozniPark.MenjacId);
        //            ViewBag.KaroserijaId = new SelectList(BexUow.VozniParkKaroserija.AllAsNoTracking, "Id", "NazivKaroserije", vozniPark.KaroserijaId);
        //            ViewBag.KategorijaId = new SelectList(BexUow.VozniParkKategorija.AllAsNoTracking, "Id", "KategorijaNaziv", vozniPark.KategorijaId);
        //            ViewBag.AddTehnickeVozila = false;
        //            return PartialView(vozniPark);
        //        }

        //        ExceptionSolver.PrepareModelState(ModelState, commandResult);
        //    }

        //    return View(vozniPark);

        //}

        //[ClaimsAuthentication(Resource = "VozniPark", Operation = "Details, All")]
        //public ActionResult DetailsDnevnik(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    ViewBag.VoziloId = id.Value;

        //    return View();
        //}

        //[HttpGet]
        //public ActionResult GetDnevnikDetailsData(int grupaId, int id)
        //{
            

        //    var detailsTroskovi = BexUow.VozniParkDnevnik.GetAll(s => s.VpId == id && ((grupaId == 11) ?  (s.VozniParkDnevnikTip.GrupaId == grupaId || s.VozniParkDnevnikTip.GrupaId == 21) : s.VozniParkDnevnikTip.GrupaId == grupaId ))
        //                                                .Select(s=> new {
        //                                                    DnevnikDatum = s.Datum,
        //                                                    DnevnikTip = s.VozniParkDnevnikTip.NazivTipa ,
        //                                                    DnevnikRegion = s.Region.NazivSkraceni,
        //                                                    DnevnikKm = s.Km,
        //                                                    DnevnikKolicina = s.Kolicina,
        //                                                    DnevnikCena = s.Cena,
        //                                                    DnevnikIznosDin = s.IznosDin,
        //                                                    DnevnikIznosEur = s.IznosEur,
        //                                                    DnevnikOpis = s.Opis,
        //                                                    DnevnikNapomena = s.Napomena
        //                                                });

        //    return Json(detailsTroskovi, "", JsonRequestBehavior.AllowGet);
        //}


        //[ClaimsAuthentication(Resource = "VozniPark", Operation = "ClickUbaciUZonu, All")]
        //public ActionResult DodajZonu (int id)
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
