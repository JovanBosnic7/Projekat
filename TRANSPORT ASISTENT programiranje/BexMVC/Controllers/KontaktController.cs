using System;
using System.Collections.Generic;
using System.Data;

using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Bex.DAL.EF.UOW;
using Bex.MVC.Exceptions;
using Bex.Common;
using Bex.Models;
using PagedList;
using BexMVC.ViewModels;
using BexMVC.Helpers;
using System.Data.Entity.Core;
using BexMVC.Filters;

namespace BexMVC.Controllers
{
    [Authorize]
    [ClaimsAuthentication(Resource = "Kontakt", Operation = "Read, All")]
    public class KontaktController : Controller
    {
        public KontaktController() : this(new BexUow(), new ExceptionSolver())
        { }
        public KontaktController(
            IBexUow bexUow,
            IExceptionSolver exceptionSolver)
        {
            BexUow = bexUow;
            ExceptionSolver = exceptionSolver;
        }

        // GET: /Movies/
        public ActionResult Index(int? id, bool? pravno, int? page, string searchString)
        {
            TempData.ToList().ForEach(keyValue =>
                ModelState.AddModelError("", keyValue.Value.ToString()));
            TempData.Clear();
            return View();
        }

        [HttpGet]
        public ActionResult GetKontakt(int pageSize, int pageNumber, string sortColumn, string sortOrder, string search, string searchColumn, string searchTerms)
        {
            var skip = (pageNumber - 1) * pageSize;

            var total = 0;

            if (!String.IsNullOrEmpty(searchTerms))
            {
                //total = BexUow.Kontakts.GetSearchKontaktData(searchTerms).Count();

                var kontaktAll = BexUow.Kontakts.GetAll(true).Select(x =>
                                                         new KontaktIndexData
                                                         {
                                                             Id = x.Id,
                                                             KontaktNaziv = x.Naziv,
                                                             Adresa = x.AdresaTekst,
                                                             Pravno = x.PravnoLice,
                                                             Stranac = x.Stranac
                                                         }).AsEnumerable();



                //var kontaktsData = BexUow.Kontakts.GetSearchKontaktDataRepository(searchTerms).Select(x =>
                var kontaktsData = GetSearchKontaktData(searchTerms, kontaktAll).Select(x =>
                                                         new KontaktIndexData
                                                         {
                                                             Id = x.Id,
                                                             KontaktNaziv = x.KontaktNaziv,
                                                             Adresa = x.Adresa,
                                                             Pravno = x.Pravno,
                                                             Stranac = x.Stranac
                                                         });

                total = kontaktsData.Count();

                if (sortOrder.Equals("desc"))
                {
                    kontaktsData = kontaktsData.OrderByDescending(s => s.GetType().GetProperty((sortColumn == "") ? "Id" : sortColumn).GetValue(s))
                                                 .ToList()
                                                 .Skip(skip)
                                                 .Take(pageSize);
                }
                else
                {
                    kontaktsData = kontaktsData.OrderBy(s => s.GetType().GetProperty((sortColumn == "") ? "Id" : sortColumn).GetValue(s))
                                                 .ToList()
                                                 .Skip(skip)
                                                 .Take(pageSize);
                }

                var jsonData = new TableJsonIndexData<KontaktIndexData>()
                {
                    total = total,
                    rows = kontaktsData
                };

                var jsonResult = Json(jsonData, "TableJsonIndexData", JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }



            return null;

        }

        public IEnumerable<KontaktIndexData> GetSearchKontaktData(string searchTerms, IEnumerable<KontaktIndexData> searchDataSet)
        {

            string[] terms = searchTerms.Split(',');
            string searchColumn = "";
            string searchTxt = "";

            string searchColumnNaziv = "";
            string searchColumnAdresa = "";
            bool searchColumnPravno = false;
            bool searchColumnStranac = false;

            var kontakt = searchDataSet;



            foreach (string t in terms)
            {
                string[] searchCT = t.Split(':');
                searchColumn = searchCT[0];
                searchTxt = searchCT[1];

                if (!String.IsNullOrEmpty(searchTxt))
                {
                    if (searchColumn.Equals("KontaktNaziv"))
                    {
                        searchColumnNaziv = searchTxt;
                        kontakt = kontakt.Where(k => k.KontaktNaziv.ToUpper().Contains(searchColumnNaziv.ToUpper()));

                    }
                    else if (searchColumn.Equals("Adresa"))
                    {
                        searchColumnAdresa = searchTxt;
                        kontakt = kontakt.Where(k => k.Adresa.ToUpper().Contains(searchColumnAdresa.ToUpper()));
                    }
                    else if (searchColumn.Equals("Pravno") && !searchTxt.Equals("0"))
                    {
                        searchColumnPravno = searchTxt.Equals("1") ? true : false;
                        kontakt = kontakt.Where(k => k.Pravno == searchColumnPravno);
                    }
                    else if (searchColumn.Equals("Stranac") && !searchTxt.Equals("0"))
                    {
                        searchColumnStranac = searchTxt.Equals("1") ? true : false; ;
                        kontakt = kontakt.Where(k => k.Stranac == searchColumnStranac);
                    }
                }


            }

            return kontakt;
        }


        public ActionResult GetAdrese(string query)
        { 
            return Json(_GetAdrese(query), JsonRequestBehavior.AllowGet);
        }

        private List<Autocomplete> _GetAdrese(string query)
        {
            List<Autocomplete> addressList = new List<Autocomplete>();
            try
            {
                //var results = BexUow.Addresses.GetSearchAddressData("NazivUlice:" + query).Take(10).ToList();
                //BexUow.Addresses.GetAll().Where(m => m.Street.StreetName.Contains(query)).Take(10).ToList();

                var results = (from t in BexUow.Addresses.GetAddressAutocompliteData(query)
                                    group t by new { Col1 = t.Street.StreetName, Col2 = t.Place.PlaceName}
                                into grp
                                    select new
                                    {
                                        NazivUlice = grp.Key.Col1,
                                        NazivMesta = grp.Key.Col2
                                       
                                    }).Take(10).ToList();

                foreach (var r in results)
                {
                    // create objects
                    Autocomplete address = new Autocomplete();

                    address.Name = string.Format("{0} - {1}", r.NazivUlice, r.NazivMesta);
                    //models.Name = r.Naziv;
                    address.Id = 1;
                    addressList.Add(address);
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
            return addressList;
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
       
        public ActionResult Create(KontaktIndexData kontakt)
        {

            if (ModelState.IsValid)
            {

                int pakId = BexUow.Addresses.GetPakId(kontakt.Adresa, kontakt.BrojTxt);
                if(pakId == 0)
                {
                    ModelState.AddModelError("PakId", "Adresa mora biti izabrana iz liste");
                }
                kontakt.PakId = pakId;

                Kontakt a = new Kontakt
                {
                    Ime = kontakt.Ime,
                    Prezime = kontakt.Prezime,
                    AdresaTekst = kontakt.Adresa,
                    BrojTxt = kontakt.BrojTxt,
                    PakId = kontakt.PakId,
                    PravnoLice=kontakt.Pravno,
                    Stranac=kontakt.Stranac
                };
             
                BexUow.Kontakts.Add(a);

                var commandResult = BexUow.SubmitChanges();

               

                if (commandResult.IsSuccessful)
                {
                    return RedirectToAction("Details", new { id = kontakt.Id, pravno = kontakt.Pravno });
                } 

                ExceptionSolver.PrepareModelState(ModelState, commandResult);
            }
            else
            {
                if (kontakt.PakId == null)
                {
                    ModelState.AddModelError(kontakt.PakId.ToString(), "Adresa mora biti izabrana iz liste");
                }
                else
                {
                    ModelState.AddModelError("", "MVC_Handled_ModelValidation");
                }
                
            }
         
            return View(kontakt);
        }

        [ChildActionOnly]
        public ActionResult EditDetailsPravnoPartial(int kontaktId)
        {
            ViewBag.KontaktId = kontaktId;
            ViewBag.AddPravnoLice = false;
            ViewBag.DelatnostId = new SelectList(BexUow.KontaktDelatnost.AllAsNoTracking, "Id", "NazivDelatnosti");
            KontaktPravnoLice kontaktPravno = BexUow.KontaktPravnoLice.AllAsNoTracking.Where(p => p.KontaktId == kontaktId).FirstOrDefault();
            if (kontaktPravno is null)
            {
                return View();
            }
            else
            {
                return View(kontaktPravno);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditDetailsPravnoPartial(KontaktPravnoLice pravnoLiceModel)
        {
            if (ModelState.IsValid)
            {
                KontaktPravnoLice kontaktPravno = BexUow.KontaktPravnoLice.AllAsNoTracking.Where(f => f.KontaktId == pravnoLiceModel.KontaktId).FirstOrDefault();
                if (kontaktPravno.Id > 0)
                {
                    pravnoLiceModel.Id = kontaktPravno.Id;
                    BexUow.KontaktPravnoLice.Update(pravnoLiceModel);
                }
                else
                {
                    BexUow.KontaktPravnoLice.Add(pravnoLiceModel);
                }
                var commandResult = BexUow.SubmitChanges();

                if (commandResult.IsSuccessful)
                {
                    ViewBag.KontaktId = pravnoLiceModel.KontaktId;
                    ViewBag.AddFizickoLice = false;
                    return PartialView(kontaktPravno);
                }
                ExceptionSolver.PrepareModelState(ModelState, commandResult);
            }
            return View(pravnoLiceModel);
        }

        [ChildActionOnly]
        public ActionResult EditDetailsFizickoPartial(int kontaktId)
        {
            ViewBag.KontaktId = kontaktId;
            ViewBag.AddFizickoLice = false;
            KontaktFizickoLice kontaktFizicko = BexUow.KontaktFizickoLice.AllAsNoTracking.Where(f => f.KontaktId==kontaktId).FirstOrDefault();
            if (kontaktFizicko is null)
            {
                return View();
            }
            else
            {
                return View(kontaktFizicko);
            }
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditDetailsFizickoPartial(KontaktFizickoLice model)
        {
            if (ModelState.IsValid)
            {
                KontaktFizickoLice kontaktFizicko = BexUow.KontaktFizickoLice.AllAsNoTracking.Where(f => f.KontaktId == model.KontaktId).FirstOrDefault();
                if (kontaktFizicko.Id>0)
                {
                    model.Id = kontaktFizicko.Id;
                    BexUow.KontaktFizickoLice.Update(model);
                }
                else
                {
                    BexUow.KontaktFizickoLice.Add(model);
                }                
                var commandResult = BexUow.SubmitChanges();

                if (commandResult.IsSuccessful)
                {
                    ViewBag.KontaktId = model.KontaktId;
                    ViewBag.AddFizickoLice = false;
                    return PartialView(kontaktFizicko);
                }
                ExceptionSolver.PrepareModelState(ModelState, commandResult);
            }
            return View(model);
        }

        [ChildActionOnly]
        public ActionResult CreateDetailsAdresePartial(int kontaktId)
        {
            ViewBag.KontaktId = kontaktId;
            ViewBag.AddAdresa = false;            
            return View();
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateDetailsAdresePartial(KontaktAdresa kontaktAdresaModel)
        {
            if (ModelState.IsValid)
            {
                int pakId = BexUow.Addresses.GetPakId(kontaktAdresaModel.AdresaTxt, kontaktAdresaModel.BrojTxt);
                if (pakId == 0)
                {
                    ModelState.AddModelError("PakId", "Adresa mora biti izabrana iz liste");
                }
                kontaktAdresaModel.PakId = pakId;

                BexUow.KontaktAdresa.Add(kontaktAdresaModel);
                var commandResult = BexUow.SubmitChanges();

                if (commandResult.IsSuccessful)
                {
                    ViewBag.KontaktId = kontaktAdresaModel.KontaktId;
                    ViewBag.AddAdresa = false;
                    return PartialView(kontaktAdresaModel);
                }
            }
            return View(kontaktAdresaModel);
        }

        [ChildActionOnly]
        public ActionResult CreateDetailsDelatnostiPartial(int kontaktId)
        {
            ViewBag.KontaktId = kontaktId;
            ViewBag.AddDelatnost = false;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateDetailsDelatnostiPartial(KontaktDelatnost kontaktDelatnostModel)
        {
            if (ModelState.IsValid)
            {
                BexUow.KontaktDelatnost.Add(kontaktDelatnostModel);
                var commandResult = BexUow.SubmitChanges();

                if (commandResult.IsSuccessful)
                {
                    ViewBag.KontaktId = kontaktDelatnostModel.KontaktId;
                    ViewBag.AddDelatnost = false;
                    return PartialView(kontaktDelatnostModel);
                }
            }
            return View(kontaktDelatnostModel);
        }

        [ChildActionOnly]
        public ActionResult CreateDetailsEmailPartial(int kontaktId)
        {
            ViewBag.KontaktId = kontaktId;
            ViewBag.AddEmail = false;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateDetailsEmailPartial(KontaktEmail kontaktEmailModel)
        {
            if (ModelState.IsValid)
            {
                BexUow.KontaktEmail.Add(kontaktEmailModel);
                var commandResult = BexUow.SubmitChanges();

                if (commandResult.IsSuccessful)
                {
                    ViewBag.KontaktId = kontaktEmailModel.KontaktId;
                    ViewBag.AddEmail = false;
                    return PartialView(kontaktEmailModel);
                }
            }
            return View(kontaktEmailModel);
        }

        [ChildActionOnly]
        public ActionResult CreateDetailsTelefoniPartial(int kontaktId)
        {
            ViewBag.KontaktId = kontaktId;
            ViewBag.AddTelefon = false;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateDetailsTelefoniPartial(KontaktTelefon kontaktTelModel)
        {
            if (ModelState.IsValid)
            {
                BexUow.KontaktTelefon.Add(kontaktTelModel);
                var commandResult = BexUow.SubmitChanges();

                if (commandResult.IsSuccessful)
                {
                    ViewBag.KontaktId = kontaktTelModel.KontaktId;
                    ViewBag.AddTelefon = false;
                    return PartialView(kontaktTelModel);
                }
            }
            return View(kontaktTelModel);
        }        

        [ChildActionOnly]
        public ActionResult CreateDetailsZiroRacuniPartial(int kontaktId)
        {
            ViewBag.KontaktId = kontaktId;
            ViewBag.AddZiroRacun = false;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateDetailsZiroRacuniPartial(KontaktZiroRacun kontaktZracunModel)
        {
            if (ModelState.IsValid)
            {
                BexUow.KontaktZiroRacun.Add(kontaktZracunModel);
                var commandResult = BexUow.SubmitChanges();

                if (commandResult.IsSuccessful)
                {
                    ViewBag.KontaktId = kontaktZracunModel.KontaktId;
                    ViewBag.AddZiroRacun = false;
                    return PartialView(kontaktZracunModel);
                }
            }
            return View(kontaktZracunModel);
        }


        //public ActionResult DetailsFizickoPravno(int? id, string isPravno)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }

        //    ViewBag.KontaktId = id.Value;


        //    KontaktPravnoLice kontaktPravnoLice = new KontaktPravnoLice();
        //    KontaktFizickoLice kontaktFizickoLice = new KontaktFizickoLice();

        //    if (isPravno.Equals("true"))
        //    {
        //        kontaktPravnoLice = BexUow.KontaktPravnoLice.Find(p => p.KontaktId == id);
        //        ViewBag.Pravno = 1;
        //        return View();
        //    }
        //    else
        //    {
        //        kontaktFizickoLice = BexUow.KontaktFizickoLice.Find(p => p.KontaktId == id);
        //        ViewBag.Pravno = 0;
        //        return View();
        //    }

        //}

        [HttpGet]
        public ActionResult GetFizickoData(int? id)
        {
            var detailsFizicko = BexUow.KontaktFizickoLice.GetAll(s => s.KontaktId == id)
                                                        .Select(s => new {
                                                            MaticniBroj = s.MaticniBroj,
                                                            DatumRodjenja = s.DatumRodjenja,
                                                            MestoRodjenja = s.MestoRodjenja,
                                                            Drzavljanstvo = s.Drzavljanstvo,
                                                            BrojLicneKarte = s.BrojLicneKarte,
                                                            DatumIstekaLicneKarte = s.DatumIstekaLicneKarte,
                                                            MestoIzdavanjaLicneKarte = s.MestoIzdavanjaLicneKarte,
                                                            BrojVozackeDozvole = s.BrojVozackeDozvole,
                                                            DatumIstekaVozackeDozvole = s.DatumIstekaVozackeDozvole,
                                                            KategorijaVozackeDozvole = s.KategorijaVozackeDozvole,
                                                            MestoIzdavanjaVozackeDozvole = s.MestoIzdavanjaVozackeDozvole,
                                                            BrojPasosa = s.BrojPasosa,
                                                            DatumIstekaPasosa = s.DatumIstekaPasosa,
                                                            MestoIzdavanjaPasosa = s.MestoIzdavanjaPasosa,
                                                            Pol = ""
                                                        });

            return Json(detailsFizicko, "", JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetPravnoData(int? id)
        {
            var detailsPravno = BexUow.KontaktPravnoLice.GetAll(s => s.KontaktId == id)
                                                        .Select(s => new {
                                                            Poslovnica = s.Poslovnica,
                                                            Sediste = s.SedisteKontaktId,
                                                            PIB = s.PIB,
                                                            MaticniBroj = s.MaticniBroj,
                                                            Delatnost = s.DelatnostId,
                                                            Direktor = s.Direktor,
                                                            Sajt = s.WebSajt
                                                        });

            return Json(detailsPravno, "", JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetAdresaData(int? id)
        {
            var detailsAdresa = BexUow.KontaktAdresa.GetAll(s => s.KontaktId == id)
                                            .Select(s => new {
                                                Adresa = s.AdresaTxt,
                                                BrojTxt = s.BrojTxt
                                            });

            return Json(detailsAdresa, "", JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetTelefoniData(int? id)
        {
            var detailsTelefon = BexUow.KontaktTelefon.GetAll(s => s.KontaktId == id)
                                .Select(s => new {
                                    KontaktOsoba = s.KontaktOsoba,
                                    Telefon = s.Telefon
                                });

            return Json(detailsTelefon, "", JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetEmailData(int? id)
        {
            var detailsEmail = BexUow.KontaktEmail.GetAll(s => s.KontaktId == id)
                                .Select(s => new {
                                    EmailAdresa = s.EmailAdresa
                                });

            return Json(detailsEmail, "", JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetZiroRacunData(int? id)
        {
            var detailsZiroRacun = BexUow.KontaktZiroRacun.GetAll(s => s.KontaktId == id)
                                .Select(s => new {
                                    Banka = s.Banka,
                                    BrojZiroRacuna = s.BrojZiroRacuna
                                });

            return Json(detailsZiroRacun, "", JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetDelatnostiData(int? id)
        {
            var detailsDelatnost = BexUow.KontaktDelatnost.GetAll(s => s.KontaktId == id)
                                .Select(s => new {
                                    SifraDelatnosti = s.SifraDelatnosti,
                                    NazivDelatnosti = s.NazivDelatnosti
                                });

            return Json(detailsDelatnost, "", JsonRequestBehavior.AllowGet);
        }

        public ActionResult DetailsAdrese(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            KontaktAdresa detailsAdrese = BexUow.KontaktAdresa.Find(p => p.KontaktId == id);
           
           
            if (detailsAdrese == null)
            {
                return HttpNotFound();
            }
            return View(detailsAdrese);
        }

        
        //GET: /Kontakt/Details/1
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.KontaktId = id.Value;

            return View();
        }


        [ClaimsAuthentication(Resource = "Kontakt", Operation = "Create, All")]
        public ActionResult Create()
        {
            //PopulateAddressDropDownList(1);
            //ViewBag.AdresaId = new SelectList(BexUow.AdreseZaIzbor.AllAsNoTracking, "Id", "Ulica");
            return View();
        }



        [ClaimsAuthentication(Resource = "Kontakt", Operation = "Edit, All")]
        public ActionResult Edit(int? id)
        { 
            Kontakt kontakt = BexUow.Kontakts.Find(id);
            if (kontakt == null)
            {
                return HttpNotFound();
            }
            //ViewBag.KontaktId = id;
            ViewBag.Pravno = kontakt.PravnoLice;
            ViewBag.NazivKontakta = kontakt.Naziv;

            return View(kontakt);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Kontakt kontakt)
        {
            if (ModelState.IsValid)
            {
                BexUow.Kontakts.Update(kontakt);

                var uowCommandResult = BexUow.SubmitChanges();

                if (uowCommandResult.IsSuccessful)
                { return RedirectToAction("Edit", "Kontakt", new { id = kontakt.Id }); }

                ExceptionSolver.PrepareModelState(ModelState, uowCommandResult);
            }

            return View(kontakt);
        }

        // GET: /Kontakt/Delete/1
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        TempData["ModelError_Index"] = "Nije izabran kontakt koji se brise.";
        //        return RedirectToAction("Index");
        //    }

        //    var kontakt = BexUow.Kontakts.Find(id);
        //    if (kontakt == null)
        //    {
        //        TempData["ModelError_Index"] = $"Izabrani kontakt {id} nije pronadjen.";
        //        return RedirectToAction("Index");
        //    }

        //    return View(kontakt);
        //}

        [ClaimsAuthentication(Resource = "Kontakt", Operation = "Delete, All")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Kontakt kontakt = BexUow.Kontakts.Find(id);
            if (kontakt == null)
            {
                return HttpNotFound();
            }
            else
            {
                if (kontakt.PravnoLice == true)
                {
                    BexUow.KontaktPravnoLice.Remove(p => p.KontaktId == id);
                }
                else
                {
                    BexUow.KontaktFizickoLice.Remove(p => p.KontaktId == id);
                }

                BexUow.Kontakts.Remove(m => m.Id == id);

                var uowCommandResult = BexUow.SubmitChanges();

                if (!uowCommandResult.IsSuccessful)
                {
                    ExceptionSolver.PrepareModelState(ModelState, uowCommandResult);
                    ExceptionSolver.PrepareTempData(TempData, ModelState);
                }
            }
            
            return RedirectToAction("Index");
        }

        // DA BI POSTOJALA POTVRDA BRISANJA MORA DA IMA I STRANICA ZA TO
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    BexUow.KontaktFizickoLice.Remove(
        //        p => p.KontaktId == id);
        //    BexUow.KontaktPravnoLice.Remove(
        //        p => p.KontaktId == id);
        //    BexUow.Kontakts.Remove(m => m.Id == id);

        //    var uowCommandResult = BexUow.SubmitChanges();

        //    if (!uowCommandResult.IsSuccessful)
        //    {
        //        ExceptionSolver.PrepareModelState(ModelState, uowCommandResult);
        //        ExceptionSolver.PrepareTempData(TempData, ModelState);
        //    }

        //    return RedirectToAction("Index");
        //}

        /** Autocomplete data **/
        public ActionResult GetKontaktAutocomplete(string query, bool isPravno, bool isAll)
        {
            return Json(_GetKontaktAutocomplete(query, isPravno, isAll), JsonRequestBehavior.AllowGet);
        }
        private List<Autocomplete> _GetKontaktAutocomplete(string query, bool isPravno, bool isAll)
        {
            List<Autocomplete> kontaktList = new List<Autocomplete>();
            try
            {

                kontaktList = BexUow.Kontakts.GetKontaktAutocompliteData(query, isPravno, isAll)
                                             .Select(a => new Autocomplete
                                                {
                                                    Id = a.Id,
                                                    Name = string.Format("{0} - {1}", a.Naziv, a.AdresaTekst)
                                                }).ToList();


                

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
            return kontaktList;
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
