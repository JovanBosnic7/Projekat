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
  
    public class VozniParkController : Controller
    {
        public VozniParkController() : this(new SecurityUow(System.Web.HttpContext.Current.GetOwinContext()), new BexUow())
        { }
        public VozniParkController(
            ISecurityUow securityUow, IBexUow bexUow)
        {
            SecurityUow = securityUow;
            BexUow = bexUow;
        }
        public ActionResult Index(int? statusId)
        {
            TempData.ToList().ForEach(keyValue =>
                ModelState.AddModelError("", keyValue.Value.ToString()));
            TempData.Clear();
            ViewBag.Model = new SelectList(BexUow.VozniParkModel.AllAsNoTracking, "NazivModela", "NazivModela");
            ViewBag.Marka = new SelectList(BexUow.VozniParkMarka.AllAsNoTracking, "NazivMarke", "NazivMarke");
            ViewBag.Statusi = new SelectList(BexUow.VozniParkStatus.AllAsNoTracking, "NazivStatusa", "NazivStatusa");
            ViewBag.Kategorija = new SelectList(BexUow.VozniParkKategorija.AllAsNoTracking, "KategorijaNaziv", "KategorijaNaziv");
            ViewBag.StatusId = (statusId == null) ? 0 : statusId;
            return View();

        }

        [HttpGet]      
        public async Task<ActionResult> GetVozniPark(int pageSize, int pageNumber, string sortColumn, string sortOrder, string search, string searchColumn, string searchTerms)
        {

            var applicationUser = await SecurityUow.UserManager.FindUserByIdAsync(User.Identity.GetUserId()) as ApplicationUser;
            var korisnikPrograma = BexUow.KorisniciPrograma.Find(x => x.AspNetUserId == applicationUser.Id);

            var skip = (pageNumber - 1) * pageSize;

            var vozniParkData = BexUow.VozniPark.AllAsNoTracking.Where(x => x.FirmaId==korisnikPrograma.FirmaId).Select(x =>
                                                         new VozniParkIndexData1
                                                         {
                                                             Id = x.Id,
                                                             GarazniBroj = x.GarazniBroj ?? "",
                                                             Oznaka = x.Oznaka.ToString() ?? "",
                                                             Kategorija = x.VozniParkKategorija.KategorijaNaziv.ToString(),
                                                             Karoserija = x.VozniParkKaroserija.NazivKaroserije,
                                                             PodKaroserija=x.VozniParkPodKaroserija.NazivPodKaroserije,
                                                             Marka = x.VozniParkModel.VozniParkMarka.NazivMarke.ToString(),
                                                             Model = x.VozniParkModel.NazivModela.ToString(),
                                                             TipVozila = x.TipVozila.ToString(),
                                                             Godiste = x.GodinaProizvodnje,
                                                             Sasija = x.Sasija ?? "",
                                                             BrojMotora = x.Motor,
                                                             SnagaMotora=x.Snaga,
                                                             Zapremina=x.Zapremina,
                                                             Masa=x.Masa,
                                                             Nosivost=x.Nosivost,
                                                             NDM=x.NDM,
                                                             NabavnaKM=x.KmNabavna,
                                                             PropisanaPotrosnja = x.PropisanaPotrosnja,
                                                             BrojVrata=x.BrojVrata,
                                                             BrMestaSedenje=x.BrMestaSedenje,
                                                             BrMestaStajanje=x.BrMestaStajanje,
                                                             PogonskiPneumatici=x.PogonskiPneumatici,
                                                             UpravljackiPneumatici=x.UpravljackiPneumatici,
                                                             VrstaGoriva = x.Gorivo.NazivGoriva,
                                                             Menjac=x.VozniParkMenjac.NazivMenjaca,
                                                             Status = x.VozniParkStatus.NazivStatusa.ToString(),
                                                             Napomena = x.Napomena ?? "",
                                                             DatumRegistracije = x.DatumRegistracije,
                                                             Sifra=x.Sifra
                                                         }).AsEnumerable();



            if (!String.IsNullOrEmpty(searchTerms))
            {
                vozniParkData = GetSearchVozniParkData(searchTerms, vozniParkData).AsEnumerable(); 
            }

            var total = vozniParkData.Count();

            if (sortOrder.Equals("desc"))
                vozniParkData = vozniParkData.OrderByDescending(s => s.GetType().GetProperty(sortColumn).GetValue(s)).ToList().Skip(skip).Take(pageSize);
            else
                vozniParkData = vozniParkData.OrderBy(s => s.GetType().GetProperty((sortColumn == "") ? "Id" : sortColumn).GetValue(s)).ToList().Skip(skip).Take(pageSize);
            

            var jsonData = new TableJsonIndexData<VozniParkIndexData1>()
            {
                total = total,
                rows = vozniParkData
            };
            var jsonResult = Json(jsonData, "TableJsonIndexData", JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
            
        }

        public IEnumerable<VozniParkIndexData1> GetSearchVozniParkData(string searchTerms, IEnumerable<VozniParkIndexData1> searchDataSet)
        {
            string[] terms = searchTerms.Split(',');
            var vozniPark = searchDataSet;

            foreach (string t in terms)
            {
                string[] searchCT = t.Split(':');

                string searchColumn = "";
                string searchTxt = "";


                searchColumn = searchCT[0];
                searchTxt = searchCT[1];

                if (searchColumn.Equals("GarazniBroj") && !String.IsNullOrEmpty(searchTxt))
                {
                    vozniPark = vozniPark.Where(k => k.GarazniBroj.ToUpper().Contains(searchTxt.ToUpper()));
                }
                else if (searchColumn.Equals("Oznaka") && !String.IsNullOrEmpty(searchTxt))
                {
                    vozniPark = vozniPark.Where(k => (k.Oznaka == null ? "" : k.Oznaka).ToUpper().Contains(searchTxt.ToUpper()));
                }
                else if (searchColumn.Equals("Kategorija") && !String.IsNullOrEmpty(searchTxt))
                {
                    vozniPark = vozniPark.Where(k => k.Kategorija.ToUpper().Contains(searchTxt.ToUpper()));
                }
                else if (searchColumn.Equals("Karoserija") && !String.IsNullOrEmpty(searchTxt))
                {
                    vozniPark = vozniPark.Where(k => k.Karoserija.ToUpper().Contains(searchTxt.ToUpper()));
                }
                else if (searchColumn.Equals("PodKaroserija") && !String.IsNullOrEmpty(searchTxt))
                {
                    vozniPark = vozniPark.Where(k => k.PodKaroserija.ToUpper().Contains(searchTxt.ToUpper()));
                }
                else if (searchColumn.Equals("Marka") && !String.IsNullOrEmpty(searchTxt))
                {
                    vozniPark = vozniPark.Where(k => k.Marka.ToUpper().Contains(searchTxt.ToUpper()));
                }
                if (searchColumn.Equals("Model") && !String.IsNullOrEmpty(searchTxt))
                {
                    vozniPark = vozniPark.Where(k => k.Model.ToString().ToUpper().Contains(searchTxt.ToUpper()));
                }
                else if (searchColumn.Equals("TipVozila") && !String.IsNullOrEmpty(searchTxt))
                {
                    vozniPark = vozniPark.Where(k => k.TipVozila.ToUpper().Contains(searchTxt.ToUpper()));
                }
                else if (searchColumn.Equals("Godiste") && !String.IsNullOrEmpty(searchTxt))
                {
                    vozniPark = vozniPark.Where(k => k.Godiste.Equals(System.Convert.ToInt32(searchTxt)));
                }
                else if (searchColumn.Equals("Sasija") && !String.IsNullOrEmpty(searchTxt))
                {
                    vozniPark = vozniPark.Where(k => k.Sasija.ToUpper().Contains(searchTxt.ToUpper()));
                }
                else if (searchColumn.Equals("BrojMotora") && !String.IsNullOrEmpty(searchTxt))
                {
                    vozniPark = vozniPark.Where(k => k.BrojMotora.ToUpper().Contains(searchTxt.ToUpper()));
                }
                else if (searchColumn.Equals("SnagaMotora") && !String.IsNullOrEmpty(searchTxt))
                {
                    vozniPark = vozniPark.Where(k => k.SnagaMotora.Equals(System.Convert.ToInt32(searchTxt)));
                }
                else if (searchColumn.Equals("Zapremina") && !String.IsNullOrEmpty(searchTxt))
                {
                    vozniPark = vozniPark.Where(k => k.Zapremina.Equals(System.Convert.ToInt32(searchTxt)));
                }
                else if (searchColumn.Equals("Masa") && !String.IsNullOrEmpty(searchTxt))
                {
                    vozniPark = vozniPark.Where(k => k.Masa.Equals(System.Convert.ToInt32(searchTxt)));
                }
                else if (searchColumn.Equals("Nosivost") && !String.IsNullOrEmpty(searchTxt))
                {
                    vozniPark = vozniPark.Where(k => k.Nosivost.Equals(System.Convert.ToInt32(searchTxt)));
                }
                else if (searchColumn.Equals("NDM") && !String.IsNullOrEmpty(searchTxt))
                {
                    vozniPark = vozniPark.Where(k => k.NDM.Equals(System.Convert.ToInt32(searchTxt)));
                }
                else if (searchColumn.Equals("NabavnaKM") && !String.IsNullOrEmpty(searchTxt))
                {
                    vozniPark = vozniPark.Where(k => k.NabavnaKM.Equals(System.Convert.ToInt32(searchTxt)));
                }
                else if (searchColumn.Equals("PropisanaPotrosnja") && !String.IsNullOrEmpty(searchTxt))
                {
                    vozniPark = vozniPark.Where(k => k.PropisanaPotrosnja.Equals(System.Convert.ToDecimal(searchTxt)));
                }
                else if (searchColumn.Equals("BrojVrata") && !String.IsNullOrEmpty(searchTxt))
                {
                    vozniPark = vozniPark.Where(k => k.BrojVrata.Equals(System.Convert.ToDecimal(searchTxt)));
                }
                else if (searchColumn.Equals("BrMestaSedenje") && !String.IsNullOrEmpty(searchTxt))
                {
                    vozniPark = vozniPark.Where(k => k.BrMestaSedenje.Equals(System.Convert.ToDecimal(searchTxt)));
                }
                else if (searchColumn.Equals("BrMestaStajanje") && !String.IsNullOrEmpty(searchTxt))
                {
                    vozniPark = vozniPark.Where(k => k.BrMestaStajanje.Equals(System.Convert.ToDecimal(searchTxt)));
                }
                else if (searchColumn.Equals("PogonskiPneumatici") && !String.IsNullOrEmpty(searchTxt))
                {
                    vozniPark = vozniPark.Where(k => k.PogonskiPneumatici.ToUpper().Contains(searchTxt.ToUpper()));
                }
                else if (searchColumn.Equals("UpravljackiPneumatici") && !String.IsNullOrEmpty(searchTxt))
                {
                    vozniPark = vozniPark.Where(k => k.UpravljackiPneumatici.ToUpper().Contains(searchTxt.ToUpper()));
                }
                else if (searchColumn.Equals("VrstaGoriva") && !String.IsNullOrEmpty(searchTxt))
                {
                    vozniPark = vozniPark.Where(k => k.VrstaGoriva.ToUpper().Contains(searchTxt.ToUpper()));
                }
                else if (searchColumn.Equals("Menjac") && !String.IsNullOrEmpty(searchTxt))
                {
                    vozniPark = vozniPark.Where(k => k.Menjac.ToUpper().Contains(searchTxt.ToUpper()));
                }
                else if (searchColumn.Equals("Status") && !String.IsNullOrEmpty(searchTxt))
                {
                    vozniPark = vozniPark.Where(k => k.Status.ToUpper().Equals(searchTxt.ToUpper()));
                }
                else if (searchColumn.Equals("Napomena") && !String.IsNullOrEmpty(searchTxt))
                {
                    vozniPark = vozniPark.Where(k => k.Napomena.ToUpper().Contains(searchTxt.ToUpper()));
                }
                else if (searchColumn.Equals("DatumRegistracije") && !String.IsNullOrEmpty(searchTxt))
                {
                    DateTime datumFirst, datumSecond;
                    string[] arrDatumAll = searchTxt.Split('t', 'o');

                    string[] arrDatumFirst = arrDatumAll[0].Trim().Split('-');
                    datumFirst = new DateTime(int.Parse(arrDatumFirst[0]), int.Parse(arrDatumFirst[1]), int.Parse(arrDatumFirst[2]));

                    string[] arrDatumSecond = arrDatumAll[2].Trim().Split('-');
                    datumSecond = new DateTime(int.Parse(arrDatumSecond[0]), int.Parse(arrDatumSecond[1]), int.Parse(arrDatumSecond[2]));

                    vozniPark = vozniPark.Where(k => k.DatumRegistracije.HasValue && k.DatumRegistracije >= datumFirst && k.DatumRegistracije <= datumSecond);
                }            
            }
            return vozniPark;
        }

        public ActionResult Create()
        {
            ViewBag.GorivoId = new SelectList(BexUow.Gorivo.AllAsNoTracking, "Id", "NazivGoriva");
            ViewBag.MenjacId = new SelectList(BexUow.VozniParkMenjac.AllAsNoTracking, "Id", "NazivMenjaca");
            ViewBag.KaroserijaId = new SelectList(BexUow.VozniParkKaroserija.AllAsNoTracking, "Id", "NazivKaroserije");
            List<SelectListItem> listKategorija = BexUow.VozniParkKategorija.AllAsNoTracking.Select(x =>
                                                        new SelectListItem
                                                        {
                                                            Text = x.KategorijaNaziv + " " + x.NazivPodkategorije,
                                                            Value = x.Id.ToString()
                                                        }).ToList();

            ViewBag.KategorijaId = new SelectList(listKategorija, "Value", "Text");
            ViewBag.StatusVozilaId = new SelectList(BexUow.VozniParkStatus.AllAsNoTracking, "Id", "NazivStatusa");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(VozniPark vozniPark)
        {
            var applicationUser = await SecurityUow.UserManager.FindUserByIdAsync(User.Identity.GetUserId()) as ApplicationUser;
            var korisnikPrograma = BexUow.KorisniciPrograma.Find(x => x.AspNetUserId == applicationUser.Id);
            vozniPark.FirmaId = korisnikPrograma.FirmaId;

            if (ModelState.IsValid)
            {

                BexUow.VozniPark.Add(vozniPark);
                var commandResult = BexUow.SubmitChanges();

                //isti problem id i pretakanja
                if (commandResult.IsSuccessful)
                { return RedirectToAction("Index", "VozniPark"); }

                ExceptionSolver.PrepareModelState(ModelState, commandResult);
            }

            // kad je greska on ih ostavi na ovoj stranici i nemaju pojma da je do greske doslo
            ViewBag.GorivoId = new SelectList(BexUow.Gorivo.AllAsNoTracking, "Id", "NazivGoriva");
            ViewBag.MenjacId = new SelectList(BexUow.VozniParkMenjac.AllAsNoTracking, "Id", "NazivMenjaca");
            ViewBag.KaroserijaId = new SelectList(BexUow.VozniParkKaroserija.AllAsNoTracking, "Id", "NazivKaroserije");
            List<SelectListItem> listKategorija = BexUow.VozniParkKategorija.AllAsNoTracking.Select(x =>
                                                       new SelectListItem
                                                       {
                                                           Text = x.KategorijaNaziv + " " + x.NazivPodkategorije,
                                                           Value = x.Id.ToString()
                                                       }).ToList();

            ViewBag.KategorijaId = new SelectList(listKategorija, "Value", "Text");
            ViewBag.StatusVozilaId = new SelectList(BexUow.VozniParkStatus.AllAsNoTracking, "Id", "NazivStatusa");

            return View(vozniPark);
        }

        public ActionResult GetModel(string query)
        {
            return Json(_GetModel(query), JsonRequestBehavior.AllowGet);
        }
        private List<Autocomplete> _GetModel(string query)
        {
            List<Autocomplete> modelList = new List<Autocomplete>();
            try
            {
                modelList = BexUow.VozniParkModel.AllAsNoTracking
                                                   .Where(x => x.NazivModela.ToUpper().Contains(query.ToUpper()))
                                                   .Select(a => new Autocomplete
                                                   {
                                                       Name = a.NazivModela,
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
            return modelList;
        }

        public ActionResult Edit(int id)
        {

            VozniPark vozniPark = BexUow.VozniPark.Find(id);
            if (vozniPark == null)
            {
                return HttpNotFound();
            }
            ViewBag.VoziloId = id;
            ViewBag.VoziloRegOznaka = vozniPark.Oznaka;

            ViewBag.ModelNaziv = (vozniPark.ModelId > 0 && vozniPark.ModelId!=null) ? BexUow.VozniParkModel.Find(x => x.Id == vozniPark.ModelId).NazivModela : "";
            ViewBag.GorivoId = new SelectList(BexUow.Gorivo.AllAsNoTracking, "Id", "NazivGoriva", vozniPark.GorivoId);
            ViewBag.MenjacId = new SelectList(BexUow.VozniParkMenjac.AllAsNoTracking, "Id", "NazivMenjaca", vozniPark.MenjacId);
            ViewBag.KaroserijaId = new SelectList(BexUow.VozniParkKaroserija.AllAsNoTracking.Where(x => x.KategorijaId==vozniPark.KategorijaId), "Id", "NazivKaroserije", vozniPark.KaroserijaId);
            ViewBag.PodKaroserijaId = new SelectList(BexUow.VozniParkPodKaroserija.AllAsNoTracking.Where(x => x.KaroserijaId == vozniPark.KaroserijaId), "Id", "NazivPodKaroserije", vozniPark.PodKaroserijaId);
            List<SelectListItem> listKategorija = BexUow.VozniParkKategorija.AllAsNoTracking.Select(x =>
                                                        new SelectListItem
                                                        {
                                                            Text = x.KategorijaNaziv + " " + x.NazivPodkategorije,
                                                            Value = x.Id.ToString()
                                                        }).ToList();

            ViewBag.KategorijaId = new SelectList(listKategorija, "Value", "Text", vozniPark.KategorijaId);
            ViewBag.StatusVozilaId = new SelectList(BexUow.VozniParkStatus.AllAsNoTracking, "Id", "NazivStatusa", vozniPark.StatusVozilaId);

            return View(vozniPark);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(VozniPark vozniPark)
        {
            if (ModelState.IsValid)
            {
                var voziloUpdate = BexUow.VozniPark.Find(vozniPark.Id);

                voziloUpdate.GarazniBroj = vozniPark.GarazniBroj;
                voziloUpdate.Oznaka = vozniPark.Oznaka;
                voziloUpdate.DatumRegistracije = vozniPark.DatumRegistracije;
                voziloUpdate.ModelId = vozniPark.ModelId;
                voziloUpdate.TipVozila = vozniPark.TipVozila;
                voziloUpdate.KmNabavna = vozniPark.KmNabavna;
                voziloUpdate.StatusVozilaId = vozniPark.StatusVozilaId;
                voziloUpdate.Napomena = vozniPark.Napomena;
                voziloUpdate.Sasija = vozniPark.Sasija;
                voziloUpdate.Motor = vozniPark.Motor;
                voziloUpdate.GodinaProizvodnje = vozniPark.GodinaProizvodnje;
                voziloUpdate.Nosivost = vozniPark.Nosivost;
                voziloUpdate.Masa = vozniPark.Masa;
                voziloUpdate.BrojVrata = vozniPark.BrojVrata;
                voziloUpdate.Snaga = vozniPark.Snaga;
                voziloUpdate.GorivoId = vozniPark.GorivoId;
                voziloUpdate.MenjacId = vozniPark.MenjacId;
                voziloUpdate.KaroserijaId = vozniPark.KaroserijaId;
                voziloUpdate.KategorijaId = vozniPark.KategorijaId;
                voziloUpdate.PodKaroserijaId = vozniPark.PodKaroserijaId;
                voziloUpdate.FirmaId = vozniPark.FirmaId;


                BexUow.VozniPark.Update(voziloUpdate);
                var uowCommandResult = BexUow.SubmitChanges();

                if (uowCommandResult.IsSuccessful)
                { return RedirectToAction("Index", "VozniPark"); }

                ExceptionSolver.PrepareModelState(ModelState, uowCommandResult);
            }

            return HttpNotFound();
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.VoziloId = id.Value;
            ViewBag.Oznaka = BexUow.VozniPark.Find(id).Oznaka;
            return View();
        }

        [HttpGet]
        public ActionResult GetDnevnikDetailsData(int grupaId, int id)
        {
            var detailsTroskovi = BexUow.VozniParkDnevnik.GetAll(s => s.VpId == id && ((grupaId == 11) ? (s.VozniParkDnevnikTip.GrupaId == grupaId || s.VozniParkDnevnikTip.GrupaId == 21) : s.VozniParkDnevnikTip.GrupaId == grupaId)).OrderByDescending(s => s.Datum)
                                                        .Select(s => new {
                                                            Id = s.Id,
                                                            DnevnikDatum = s.Datum,
                                                            DnevnikTip = s.VozniParkDnevnikTip.NazivTipa,
                                                            DnevnikRegion = s.Region.NazivSkraceni,
                                                            DnevnikKm = s.Km,
                                                            DnevnikKolicina = s.Kolicina,
                                                            DnevnikCena = s.Cena,
                                                            DnevnikIznosDin = s.IznosDin,
                                                            DnevnikIznosEur = s.IznosEur,
                                                            DnevnikOpis = s.Opis,
                                                            DnevnikNapomena = s.Napomena
                                                        });

            return Json(detailsTroskovi, "", JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetDetailsAlarmIservis(int grupaId, int id)
        {
            var detailsAlarmIservis = BexUow.VozniParkAlarm.GetAll(s => s.VpAlarmTipAlarm.GrupaId == grupaId).OrderByDescending(s => s.Datum)
                                                                           .Select(s => new {
                                                                               Id = s.Id,
                                                                               Tip = s.VpAlarmTipAlarm.NazivAlarma,
                                                                               Datum = s.Datum,
                                                                               DatumIsteka = s.DatumIsteka,
                                                                               DatumAlarma = s.DatumAlarma,
                                                                               Km = s.Km,
                                                                               KmIsteka = s.KmIsteka,
                                                                               KmAlarma = s.KmAlarma,
                                                                               Napomena = s.Napomena,
                                                                               VpId = s.VpId,
                                                                               ZaposleniId = s.ZaposleniId
                                                                           });
            if (grupaId==3)
            {
                detailsAlarmIservis = detailsAlarmIservis.Where(x => x.ZaposleniId == id);
            }
            else
            {
                detailsAlarmIservis = detailsAlarmIservis.Where(x => x.VpId == id);
            }
           

            return Json(detailsAlarmIservis, "", JsonRequestBehavior.AllowGet);
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

        //[ClaimsAuthentication(Resource = "VozniPark", Operation = "ClickDodajStetu, All")]
        //public ActionResult CreateSteta(int vozniParkId = 0)
        //{

        //    var vozilo = BexUow.VozniPark.GetVozniParkNaziviData().FirstOrDefault(x => x.Id == vozniParkId);


        //    VozniPark vozniPark = BexUow.VozniPark.Find(vozniParkId);
        //    ////VozniParkSteta vozniParkSteta = BexUow.VozniParkSteta.Find(id);
        //    ViewBag.ZavisnaTabelaId = vozniParkId;
        //    ViewBag.RegOznaka = vozniPark.Oznaka;
        //    ViewBag.FirmaSteteId = new SelectList(BexUow.FirmaVP.AllAsNoTracking, "Id", "Naziv");
        //    ViewBag.StetocinaCentarId = new SelectList(BexUow.Region.AllAsNoTracking, "Id", "NazivSkraceni");
        //    ViewBag.KategorijaId = new SelectList(BexUow.StetaTip.AllAsNoTracking.Where(s => s.KategorijaId == 2), "Id", "NazivTipa");
        //    //ViewBag.Zaposleni = 0;
        //    //ViewBag.Zaposleni = new SelectList(BexUow.Zaposleni.AllAsNoTracking,"Id", "Id");
        //    //ViewBag.NalogIzdao = new SelectList(BexUow.KorisniciPrograma.AllAsNoTracking, "Id", "Id");


        //    return View();
        //}
        //[HttpPost]
        //public async Task<ActionResult> CreateSteta(VozniParkSteta vozniParkSteta)
        //{
        //    var applicationUser = await SecurityUow.UserManager.FindUserByIdAsync(User.Identity.GetUserId()) as ApplicationUser;
        //    var korisnikProgramaId = BexUow.KorisniciPrograma.Find(x => x.AspNetUserId == applicationUser.Id).Id;
        //    vozniParkSteta.UserUnosId = korisnikProgramaId;
        //    BexUow.VozniPark.DodajIzmeniStetu(vozniParkSteta.Id, vozniParkSteta.FirmaSteteId, vozniParkSteta.ZavisnaTabelaId, vozniParkSteta.UserUnosId, vozniParkSteta.KategorijaId, vozniParkSteta.Opis, vozniParkSteta.Napomena, vozniParkSteta.StetocinaZaposleniId, vozniParkSteta.StetocinaCentarId, vozniParkSteta.DatumPredajePravnoj, vozniParkSteta.IznosRsd,
        //    0, vozniParkSteta.IznosZaNaplatu, vozniParkSteta.UserUnosId, vozniParkSteta.Sporno, vozniParkSteta.Racun, vozniParkSteta.Kes, vozniParkSteta.DatumOdluke, vozniParkSteta.KnjigovodstveniManjak, 0, vozniParkSteta.PotpisanaOdluka, vozniParkSteta.Nenaplativo);
        //    return RedirectToAction("Index", "VozniPark");
        //}

        //[ClaimsAuthentication(Resource = "VozniPark", Operation = "ClickDodajAlarm, All")]
        //public ActionResult CreateAlarm(bool popravka=false,int vozniParkId = 0)
        //{
        //    VozniPark vozniPark = BexUow.VozniPark.Find(vozniParkId);
        //    ViewBag.VpId = vozniParkId;
        //    ViewBag.RegOznaka = vozniPark.Oznaka;

        //    var putniNalogLast = BexUow.PutniNalog.AllAsNoTracking.Where(x => x.VpId == vozniParkId).OrderByDescending(z => z.Id).Take(1).FirstOrDefault();
        //    if(popravka)
        //    {
        //        ViewBag.VpAlarmTip = new SelectList(BexUow.VozniParkDnevnikTip.AllAsNoTracking.Where(x => x.GrupaId == 31), "Id", "NazivTipa");
        //    }
        //    else
        //    {
        //        ViewBag.VpAlarmTip = new SelectList(BexUow.VozniParkDnevnikTip.AllAsNoTracking.Where(x => x.GrupaId == 11 || x.GrupaId == 21), "Id", "NazivTipa");
        //    }


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
        //public async Task<ActionResult> CreateAlarm(AlarmIndexData alarm)
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
        //        Opis = alarm.Opis ?? "",
        //        Napomena = alarm.Napomena,
        //        Cena = alarm.Cena
        //    };

        //    var applicationUser = await SecurityUow.UserManager.FindUserByIdAsync(User.Identity.GetUserId()) as ApplicationUser;
        //    var korisnikProgramaId = BexUow.KorisniciPrograma.Find(x => x.AspNetUserId == applicationUser.Id).Id;
        //    vpDnevnik.UserUnosId = korisnikProgramaId;

        //    BexUow.VozniParkDnevnik.Add(vpDnevnik);
        //    var commandResult = BexUow.SubmitChanges();
        //    if (commandResult.IsSuccessful)
        //    {
        //        VozniParkAlarm vpAlarm = new VozniParkAlarm();

        //        if (alarm.Id > 0)
        //        {
        //            vpAlarm = BexUow.VozniParkAlarm.Find(x => x.Id == alarm.Id);
        //        }
        //        else
        //        {
        //            vpAlarm = BexUow.VozniParkAlarm.Find(x => x.VpId == alarm.VpId && x.VpAlarmTip == alarm.VpAlarmTip);
        //        }

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
        //            vpAlarm.VpId = alarm.VpId;
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
        //        return Json("Uspešno ste dodali alarm", JsonRequestBehavior.AllowGet);
        //    }

        //    return Json("Greška pri dodavanju alarma", JsonRequestBehavior.AllowGet);
        //}

        //[ClaimsAuthentication(Resource = "VozniPark", Operation = "EditAlarm, All")]
        //public ActionResult EditAlarm(int alarmId, string oznaka)
        //{
        //    var vpAlarm = BexUow.VozniParkAlarm.AllAsNoTracking.Where(x => x.Id == alarmId).Select(x =>
        //                          new AlarmIndexData
        //                          {
        //                              Id = x.Id,
        //                              VpId = x.VpId,
        //                              Vozilo = x.VozniPark.NazivVozila,
        //                              Registracija = oznaka,
        //                              Alarm = x.VozniParkDnevnikTip.NazivTipa,
        //                              VpAlarmTip = x.VpAlarmTip,
        //                              Datum = x.Datum,
        //                              DatumAlarma = x.DatumAlarma,
        //                              Km = x.Km,
        //                              DatumIsteka = x.DatumIsteka,
        //                              KmIsteka = x.KmIsteka,
        //                              Napomena = x.Napomena,
        //                              Kolicina = x.VozniParkDnevnik.Kolicina,
        //                              Cena = x.VozniParkDnevnik.Cena,
        //                              IznosDin = x.VozniParkDnevnik.IznosDin,
        //                              IznosEur = x.VozniParkDnevnik.IznosEur,
        //                              Opis = x.VozniParkDnevnik.Opis
        //                          }).FirstOrDefault();


        //    if (vpAlarm == null)
        //    {
        //        var vpAlarmDnevnik = BexUow.VozniParkDnevnik.GetAll(true).Where(x => x.Id == alarmId)
        //                                                    .Select(x =>
        //                                                   new AlarmIndexData
        //                                                   {
        //                                                       Id = x.Id,
        //                                                       VpId = x.VpId,
        //                                                       Vozilo = x.VozniPark.NazivVozila,
        //                                                       Registracija = oznaka,
        //                                                       Alarm = x.VozniParkDnevnikTip.NazivTipa,
        //                                                       VpAlarmTip = x.DnevnikTipId,
        //                                                       Datum = x.Datum,
        //                                                       DatumAlarma = x.Datum,
        //                                                       Km = x.Km,
        //                                                       DatumIsteka = x.Datum,
        //                                                       //KmIsteka = x.KmIsteka,
        //                                                       Napomena = x.Napomena,
        //                                                       Kolicina = x.Kolicina,
        //                                                       Cena = x.Cena,
        //                                                       IznosDin = x.IznosDin,
        //                                                       IznosEur = x.IznosEur,
        //                                                       Opis = x.Opis
        //                                                   }).FirstOrDefault();

        //        ViewBag.VpAlarmTip = new SelectList(BexUow.VozniParkDnevnikTip.AllAsNoTracking, "Id", "NazivTipa", vpAlarmDnevnik.VpAlarmTip);

        //        return View(vpAlarmDnevnik);
        //    }
        //    else
        //    {


        //        ViewBag.VpAlarmTip = new SelectList(BexUow.VozniParkDnevnikTip.AllAsNoTracking, "Id", "NazivTipa", vpAlarm.VpAlarmTip);
        //        return View(vpAlarm);

        //    }


        //}


        //[ClaimsAuthentication(Resource = "VozniPark", Operation = "Create, All")]
        //public ActionResult Create()
        //{
        //    ViewBag.KarakteristikaId = new SelectList(BexUow.VozniParkKarakteristike.AllAsNoTracking, "Id", "Naziv");
        //    ViewBag.BojaId = new SelectList(BexUow.VozniParkBoja.AllAsNoTracking, "Id", "NazivBoje");

        //    return View();
        //}

        ////// POST: VozniPark/Create
        ////// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        ////// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(VozniPark vozniPark)
        //{
        //    if (ModelState.IsValid)
        //    {

        //        BexUow.VozniPark.Add(vozniPark);
        //        var commandResult = BexUow.SubmitChanges();

        //        //isti problem id i pretakanja
        //        if (commandResult.IsSuccessful)
        //        { return RedirectToAction("Edit", "VozniPark", new { id = vozniPark.Id }); }

        //        ExceptionSolver.PrepareModelState(ModelState, commandResult);
        //    }

        //    // kad je greska on ih ostavi na ovoj stranici i nemaju pojma da je do greske doslo
        //    ViewBag.KarakteristikaId = new SelectList(BexUow.VozniParkKarakteristike.AllAsNoTracking, "Id", "Naziv");
        //    ViewBag.BojaId = new SelectList(BexUow.VozniParkBoja.AllAsNoTracking, "Id", "NazivBoje");

        //    return View(vozniPark);

        //}

        //[ClaimsAuthentication(Resource = "VozniPark", Operation = "Edit, All")]
        //public ActionResult Edit(int id)
        //{

        //    VozniPark vozniPark = BexUow.VozniPark.Find(id);
        //    if (vozniPark == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.VoziloId = id;
        //    ViewBag.VoziloRegOznaka = vozniPark.Oznaka;

        //    ViewBag.KarakteristikaId = new SelectList(BexUow.VozniParkKarakteristike.AllAsNoTracking, "Id", "Naziv", vozniPark.KarakteristikaId );
        //    ViewBag.BojaId = new SelectList(BexUow.VozniParkBoja.AllAsNoTracking, "Id", "NazivBoje", vozniPark.BojaId);
        //    ViewBag.ModelNaziv = BexUow.VozniParkModel.Find(x => x.Id==vozniPark.ModelId).NazivModela;

        //    return View(vozniPark);
        //}
        //[ClaimsAuthentication(Resource = "VozniPark", Operation = "Delete, All")]
        public ActionResult Delete(int id)
        {
            var vozniParkData = BexUow.VozniPark.Find(id);
            vozniParkData.Aktivno = true; //setovanje vozila na storno
            vozniParkData.StatusVozilaId = 5; //setovanje vozila na rashod
            BexUow.VozniPark.Update(vozniParkData);
            var commandResult = BexUow.SubmitChanges();

            if (commandResult.IsSuccessful)
            {
                return RedirectToAction("Index");
                //alert('@TempData["alertMessage"]');
            }

            ExceptionSolver.PrepareModelState(ModelState, commandResult);
            return View();
        }

        //// POST: VozniPark/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(VozniPark vozniPark)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var voziloUpdate = BexUow.VozniPark.Find(vozniPark.Id);

        //        voziloUpdate.GarazniBroj = vozniPark.GarazniBroj;
        //        voziloUpdate.NazivVozila = vozniPark.NazivVozila;
        //        voziloUpdate.Oznaka = vozniPark.Oznaka;
        //        voziloUpdate.OznakaStara = vozniPark.OznakaStara;
        //        voziloUpdate.Odjavljeno = vozniPark.Odjavljeno;
        //        voziloUpdate.DatumRegistracije = vozniPark.DatumRegistracije;
        //        voziloUpdate.ModelId = vozniPark.ModelId;
        //        voziloUpdate.TipVozila = vozniPark.TipVozila;
        //        voziloUpdate.KarakteristikaId = vozniPark.KarakteristikaId;
        //        voziloUpdate.KmNabavna = vozniPark.KmNabavna;
        //        voziloUpdate.BojaId = vozniPark.BojaId;
        //        voziloUpdate.KmLimit = vozniPark.KmLimit;
        //        voziloUpdate.LinijskoVozilo = vozniPark.LinijskoVozilo;


        //        BexUow.VozniPark.Update(voziloUpdate);
        //        var uowCommandResult = BexUow.SubmitChanges();

        //        if (uowCommandResult.IsSuccessful)
        //        { return RedirectToAction("Edit", "VozniPark", new { id = vozniPark.Id }); }

        //        ExceptionSolver.PrepareModelState(ModelState, uowCommandResult);
        //    }

        //    return View(vozniPark);
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
        //    ViewBag.FirmaOsId = new SelectList(BexUow.FirmaVP.AllAsNoTracking, "Id", "Naziv", vozilo.FirmaOSid);
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
        //            ViewBag.FirmaOsId = new SelectList(BexUow.FirmaVP.AllAsNoTracking, "Id", "Naziv", vozniPark.FirmaOSid);
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
        //    List <SelectListItem> listKategorija = BexUow.VozniParkKategorija.AllAsNoTracking.Select(x =>
        //                                                 new SelectListItem
        //                                                 {
        //                                                     Text = x.KategorijaNaziv + " " +  x.NazivPodkategorije,
        //                                                     Value = x.Id.ToString()
        //                                                 }).ToList();

        //    ViewBag.KategorijaId = new SelectList(listKategorija, "Value", "Text", vozilo.KategorijaId);
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
        //        //vozniParkUpdate.TipVozila = vozniPark.TipVozila;
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
        //    ViewBag.Oznaka = BexUow.VozniPark.Find(id).Oznaka;

        //    return View();
        //}

        [HttpGet]
        public ActionResult GetTroskoviDetailsData(int id)
        {


            var detailsTroskovi = BexUow.VpTrosak.GetAll(s => s.VpId == id ).OrderByDescending(s => s.DatumUnosa)
                                                        .Select(s => new
                                                        {
                                                            Id = s.Id,
                                                            Datum = s.DatumUnosa,
                                                            Tip = (s.AlarmId!=null) ? s.VozniParkAlarm.VpAlarmTipAlarm.NazivAlarma : s.VpPopravke.VpPopravkeTip.Naziv,
                                                            CenaDin = s.CenaDinara,
                                                            CenaEur = s.CenaEura,
                                                            Napomena = s.VozniParkAlarm.Napomena
                                                        });

            return Json(detailsTroskovi, "", JsonRequestBehavior.AllowGet);
        }

        //[HttpGet]
        //public ActionResult GetKarakteristikeData(int id)
        //{
        //    var detailsKarakteristike = BexUow.VozniPark.GetAll(s => s.Id == id)
        //                                                .Select(s => new {
        //                                                    Id = s.Id,
        //                                                    Sasija = s.Sasija,
        //                                                    Motor = s.Motor,
        //                                                    Proizvedeno = s.GodinaProizvodnje,
        //                                                    Nosivost = s.Nosivost,
        //                                                    Opis = s.Opis,
        //                                                    Masa = s.Masa,
        //                                                    BrojVrata = s.BrojVrata,
        //                                                    Snaga = s.Snaga,
        //                                                    Zapremina = s.Zapremina,
        //                                                    BrojTockova = s.BrojTockova,
        //                                                    Gorivo = s.Gorivo.NazivGoriva,
        //                                                    Menjac = s.VozniParkMenjac.NazivMenjaca,
        //                                                    Karoserija = s.VozniParkKaroserija.NazivKaroserije,
        //                                                    Rezervoar = s.RezervoarZapremina,
        //                                                    Kljuceva = s.BrojKljuceva,
        //                                                    Oprema = s.Oprema,
        //                                                    Kategorija = s.VozniParkKategorija.KategorijaNaziv
        //                                                });

        //    return Json(detailsKarakteristike, "", JsonRequestBehavior.AllowGet);
        //}

        //[HttpGet]
        //public ActionResult GetZaduzenjeData(int id)
        //{
        //    var detailsZaduzenje = BexUow.VozniPark.GetAll(s => s.Id == id)
        //                                                .Select(s => new {
        //                                                    Id = s.Id,
        //                                                    Tip = s.VozniParkTip.Opis,
        //                                                    Status = s.VozniParkStatus.NazivStatusa,
        //                                                    Region = s.Region.NazivSkraceni,
        //                                                    Sektor = s.Sektor.NazivSektora,
        //                                                    OdgovornoLice = s.Kontakt.Naziv,
        //                                                    Vozac = s.Kontakt.Naziv,
        //                                                    DatumZaduzenja = s.DatumZaduzenja,
        //                                                    Opis = s.Opis,
        //                                                    Napomena = s.Napomena,
        //                                                    Firma = s.Firma.Naziv,
        //                                                    Potrosnja = s.PropisanaPotrosnja
        //                                                });

        //    return Json(detailsZaduzenje, "", JsonRequestBehavior.AllowGet);
        //}


        //[ClaimsAuthentication(Resource = "VozniPark", Operation = "ClickUbaciUZonu, All")]
        //public ActionResult DodajZonu (int id)
        //{
        //    Zona zona = new Zona();
        //    var vozilo = BexUow.VozniPark.Find(x => x.Id == id);

        //    if (vozilo.ZonaId > 0)
        //    {
        //        zona = BexUow.Zona.Find(x => x.Id == vozilo.ZonaId && x.ZonaTip.Id == 4);
        //        zona.NazivZone = vozilo.NazivVozila;
        //        BexUow.Zona.Update(zona);
        //    }
        //    else
        //    {
        //        zona = new Zona
        //        {
        //            NazivZone = vozilo.NazivVozila,
        //            Tip = 4,
        //            PodTip = 41,
        //            VoziloRegionId = id,
        //            RegionGrupaId = 0,
        //            Aktivan = true
        //        };
        //        BexUow.Zona.Add(zona);
        //    }



        //    BexUow.SubmitChanges();

        //    vozilo.ZonaId = zona.Id;
        //    BexUow.VozniPark.Update(vozilo);
        //    var commandResult = BexUow.SubmitChanges();

        //    if (commandResult.IsSuccessful)
        //    { return RedirectToAction("Index"); }

        //    ExceptionSolver.PrepareModelState(ModelState, commandResult);

        //    return RedirectToAction("../VozniPark");
        //    //var uowCommandResult = BexUow.VozniPark.UbaciUzonu(id);
        //    //return RedirectToAction("Index");
        //}

        //public async Task<List<string>> getAllClaims()
        //{
        //    List<string> listaRegiona = new List<string>();
        //    var applicationUser = await SecurityUow.UserManager.FindUserByIdAsync(User.Identity.GetUserId()) as ApplicationUser;

        //    foreach (var c in applicationUser.Claims)
        //    {
        //        if (c.ClaimType.Split('/').Last().Equals("Region"))
        //        {
        //            if (c.ClaimValue.Equals("All"))
        //            {
        //                listaRegiona = BexUow.Region.AllAsNoTracking.Select(r => r.Id).ToList().ConvertAll<string>(delegate (int i) { return i.ToString(); });
        //            }
        //            else
        //            {
        //                listaRegiona.Add(c.ClaimValue);
        //            }
        //        }
        //    }
        //    return listaRegiona;
        //}

        [HttpGet]
        public JsonResult GetStatus()
        {
            var statusList = BexUow.VozniParkStatus.AllAsNoTracking
                                            .Select(a => new
                                            {
                                                Value = a.NazivStatusa
                                            }
                                            ).ToList();

            return Json(statusList, "", JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetKategorija()
        {
            var kategorijaList = BexUow.VozniParkKategorija.AllAsNoTracking.OrderBy(x => x.Sort)
                                            .Select(a => new
                                            {
                                                Value = a.KategorijaNaziv,
                                                Sort = a.Sort
                                            }
                                            ).ToList();


            return Json(kategorijaList, "", JsonRequestBehavior.AllowGet);
        }

        public ActionResult NapuniKaroseriju(int kategorijaId)
        {
            var karoserije = BexUow.VozniParkKaroserija.AllAsNoTracking.Where(c => c.KategorijaId == kategorijaId);
            return Json(karoserije, JsonRequestBehavior.AllowGet);
        }
        public ActionResult NapuniPodKaroseriju(int karoserijaId)
        {
            var podKaroserije = BexUow.VozniParkPodKaroserija.AllAsNoTracking.Where(c => c.KaroserijaId == karoserijaId);
            return Json(podKaroserije, JsonRequestBehavior.AllowGet);
        }

        public ActionResult NapuniPodKaroserijuPoKategoriji(int kategorijaId)
        {
            var karoserija = BexUow.VozniParkKaroserija.AllAsNoTracking.Where(c => c.KategorijaId == kategorijaId).OrderBy(c => c.Id).Take(1).FirstOrDefault(); ;
            var podKaroserije = BexUow.VozniParkPodKaroserija.AllAsNoTracking.Where(c => c.KaroserijaId == karoserija.Id);
            return Json(podKaroserije, JsonRequestBehavior.AllowGet);
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
