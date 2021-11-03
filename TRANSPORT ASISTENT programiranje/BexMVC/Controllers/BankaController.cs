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
    [ClaimsAuthentication(Resource = "Banka", Operation = "Read, All")]
    public class BankaController : Controller
    {
        public BankaController() : this(new BexUow(), new ExceptionSolver())
        { }
        public BankaController(
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
            ViewBag.Regioni = new SelectList(BexUow.Region.AllAsNoTracking, "NazivSkraceni", "NazivSkraceni");
            //ViewBag.Sektori = new SelectList(BexUow.Sektor.AllAsNoTracking, "NazivSektora", "NazivSektora");
            //ViewBag.Firme = new SelectList(BexUow.Firma.AllAsNoTracking, "Naziv", "Naziv");
            return View();
        }

        /* zahtev za ulice za punjenje cmb-a */
        [HttpGet]
        public ActionResult GetBanka(int pageSize, int pageNumber, string sortColumn, string sortOrder, string search, string searchColumn, string searchTerms)
        {
            var skip = (pageNumber - 1) * pageSize;

            var bankaData = BexUow.BankaPazara.GetAll(true).Select(b =>
                                             new BankaIndexData
                                             {
                                                 Id = b.Id,
                                                 Datum = b.Datum,
                                                 RegionId = b.RegionId,
                                                 RegionNaziv = b.Region.NazivSkraceni,
                                                 PazarZaUplatu = b.PazarZaUplatu,
                                                 PazarUplacen = b.PazarUplacen,
                                                 OtkupZaUplatu = b.OtkupZaUplatu,
                                                 OtkupUplacen = b.OtkupUplacen,
                                                 OtkupZaIsplatu = b.OtkupZaIsplatu,
                                                 OtkupIsplacen = b.OtkupIsplacen,
                                             }).AsEnumerable();

            if (!String.IsNullOrEmpty(searchTerms))
            {
                bankaData = GetSearchBankaData(searchTerms, bankaData).Select(b =>
                                                         new BankaIndexData
                                                         {
                                                             Id = b.Id,
                                                             Datum = b.Datum,
                                                             RegionId = b.RegionId,
                                                             RegionNaziv = b.RegionNaziv,
                                                             PazarZaUplatu = b.PazarZaUplatu,
                                                             PazarUplacen = b.PazarUplacen,
                                                             OtkupZaUplatu = b.OtkupZaUplatu,
                                                             OtkupUplacen = b.OtkupUplacen,
                                                             OtkupZaIsplatu = b.OtkupZaIsplatu,
                                                             OtkupIsplacen = b.OtkupIsplacen,
                                                         });
            }

            var total = bankaData.Count();

            if (sortOrder.Equals("desc"))
                bankaData = bankaData.OrderByDescending(s => s.GetType().GetProperty(sortColumn).GetValue(s)).ToList().Skip(skip).Take(pageSize);
            else
                bankaData = bankaData.OrderBy(s => s.GetType().GetProperty((sortColumn == "") ? "Id" : sortColumn).GetValue(s)).ToList().Skip(skip).Take(pageSize);



            var jsonData = new TableJsonIndexData<BankaIndexData>()
            {
                total = total,
                rows = bankaData
            };

            var jsonResult = Json(jsonData, "TableJsonIndexData", JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;

            return jsonResult;
        }

        public IEnumerable<BankaIndexData> GetSearchBankaData(string searchTerms, IEnumerable<BankaIndexData> searchDataSet)

        {

            string[] terms = searchTerms.Split(',');
            string searchColumn = "";
            string searchTxt = "";


            string region = "";
            string datum = "";


            var banka = searchDataSet;



            foreach (string t in terms)
            {
                string[] searchCT = t.Split(':');
                searchColumn = searchCT[0];
                searchTxt = searchCT[1];



                if (searchColumn.Equals("Datum") && !String.IsNullOrEmpty(searchTxt))
                {
                    string[] arrDatum = searchTxt.Split('/');
                    DateTime thisDate1 = new DateTime(int.Parse(arrDatum[2]), int.Parse(arrDatum[0]), int.Parse(arrDatum[1]));
                    banka = banka.Where(k => k.Datum == thisDate1);
                }
                else if (searchColumn.Equals("Region") && !String.IsNullOrEmpty(searchTxt))
                {
                    region = searchTxt;
                    banka = banka.Where(k => k.RegionNaziv.ToUpper().Contains(region.ToUpper()));
                }

            }
            return banka;
        }

        public ActionResult DetailsOtkupZaIsplatu(DateTime? Datum, int? RegionId)
        {
            if (RegionId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            BankaPazaraSpecifikacija bankaSpecifikacija = BexUow.BankaPazaraSpecifikacija.Find(p => p.DatumPazara == Datum && p.RegionIsplateId == RegionId);
            if (bankaSpecifikacija == null)
            {
                return HttpNotFound();
            }
            return View(bankaSpecifikacija);
        }

        public ActionResult DetailsOtkupZaUplatu(DateTime? Datum, int? RegionId)
        {
            if (RegionId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.RegionId = RegionId.Value;

            return View();
        }

        [HttpGet]
        public ActionResult GetOtkupZaUplatuData(DateTime? Datum, int? RegionId)
        {

            var detailsOtkupZaUplatu = (from bankaSpecifikacija in BexUow.BankaPazaraSpecifikacija.AllAsNoTracking
                                        select new
                                        {
                                            bankaSpecifikacija.Id,
                                            bankaSpecifikacija.DatumPazara,
                                            bankaSpecifikacija.TipZadatka,
                                            bankaSpecifikacija.RegionUplateId,
                                            bankaSpecifikacija.PazarZaUplatu,
                                            bankaSpecifikacija.OtkupZaUplatu,
                                            bankaSpecifikacija.RegionIsplateId,
                                            bankaSpecifikacija.OtkupZaIsplatu
                                        }).Where(s => s.RegionUplateId == RegionId).ToList();

            //List<VozniParkDnevnik> detailsTroskovi = BexUow.VozniParkDnevnik.GetAll(p => p.VpId == id).ToList();

            return Json(detailsOtkupZaUplatu, "", JsonRequestBehavior.AllowGet);
        }

        // GET: KontaktTelefon/Create
        //public ActionResult Create(int kontaktId = 0)
        //{
        //    ViewBag.RegionId = new SelectList(BexUow.Region.AllAsNoTracking, "Id", "NazivSkraceni");
        //    ViewBag.SektorId = new SelectList(BexUow.Sektor.AllAsNoTracking, "Id", "NazivSektora");
        //    ViewBag.ZanimanjeId = new SelectList(BexUow.ZaposleniZanimanje.AllAsNoTracking, "Id", "Opis");
        //    ViewBag.RazlogOtkazaId = new SelectList(BexUow.ZaposleniRazlogOtkaza.AllAsNoTracking, "Id", "Opis");
        //    ViewBag.StrucnaSpremaId = new SelectList(BexUow.ZaposleniStrucnaSprema.AllAsNoTracking, "Id", "Opis");
        //    ViewBag.RadnoMestoId = new SelectList(BexUow.ZaposleniRadnoMesto.AllAsNoTracking, "Id", "InterniNaziv");
        //    ViewBag.FirmaPrijaveId = new SelectList(BexUow.Firma.AllAsNoTracking, "Id", "Naziv");
        //    ViewBag.FirmaUkojojRadiId = new SelectList(BexUow.Firma.AllAsNoTracking, "Id", "Naziv");
        //    ViewBag.OsiguranjeId = new SelectList(BexUow.ZaposleniOsnovOsiguranja.AllAsNoTracking, "Id", "Naziv");
        //    ViewBag.ProgramZaposlenjaId = new SelectList(BexUow.ZaposleniProgramZaposlenja.AllAsNoTracking, "Id", "Naziv");
        //    ViewBag.SlavaId = new SelectList(BexUow.ZaposleniSlava.AllAsNoTracking, "Id", "Naziv");
        //    ViewBag.StatusRadnika = new SelectList(BexUow.ZaposleniStatus.AllAsNoTracking, "Id", "StatusRadnika");
        //    //if (ModelState.IsValid)
        //    //{
        //    //    var kontakt = BexUow.Kontakts.Find(k => k.Id == kontaktId);
        //    //    ViewBag.KontaktId = kontakt.Id;
        //    //    ViewBag.KontaktNaziv = kontakt.Naziv;


        //    //}
        //    // else
        //    //{ ModelState.AddModelError("", "MVC_Handled_ModelValidation"); }

        //    return View();
        //}

        ////// POST: KontaktTelefon/Create
        ////// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        ////// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(Zaposleni zaposleni, int kontaktId = 0)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        zaposleni.KontaktId = kontaktId;
        //        BexUow.Zaposleni.Add(zaposleni);
        //        var commandResult = BexUow.SubmitChanges();

        //        if (commandResult.IsSuccessful)
        //        { return RedirectToAction("../Kontakt"); }

        //        ExceptionSolver.PrepareModelState(ModelState, commandResult);
        //    }
        //    ViewBag.RegionId = new SelectList(BexUow.Region.AllAsNoTracking, "Id", "NazivSkraceni", zaposleni.RegionId);
        //    ViewBag.SektorId = new SelectList(BexUow.Sektor.AllAsNoTracking, "Id", "NazivSektora", zaposleni.SektorId);
        //    ViewBag.ZanimanjeId = new SelectList(BexUow.ZaposleniZanimanje.AllAsNoTracking, "Id", "Opis", zaposleni.ZanimanjeId);
        //    ViewBag.RazlogOtkazaId = new SelectList(BexUow.ZaposleniRazlogOtkaza.AllAsNoTracking, "Id", "Opis", zaposleni.RazlogOtkazaId);
        //    ViewBag.StrucnaSpremaId = new SelectList(BexUow.ZaposleniStrucnaSprema.AllAsNoTracking, "Id", "Opis", zaposleni.StrucnaSpremaId);
        //    ViewBag.RadnoMestoId = new SelectList(BexUow.ZaposleniRadnoMesto.AllAsNoTracking, "Id", "InterniNaziv", zaposleni.RadnoMestoId);
        //    ViewBag.FirmaPrijaveId = new SelectList(BexUow.Firma.AllAsNoTracking, "Id", "Naziv", zaposleni.FirmaPrijaveId);
        //    ViewBag.FirmaUkojojRadiId = new SelectList(BexUow.Firma.AllAsNoTracking, "Id", "Naziv", zaposleni.FirmaUkojojRadiId);
        //    ViewBag.OsiguranjeId = new SelectList(BexUow.ZaposleniOsnovOsiguranja.AllAsNoTracking, "Id", "Naziv", zaposleni.OsiguranjeId);
        //    ViewBag.ProgramZaposlenjaId = new SelectList(BexUow.ZaposleniProgramZaposlenja.AllAsNoTracking, "Id", "Naziv", zaposleni.ProgramZaposlenjaId);
        //    ViewBag.SlavaId = new SelectList(BexUow.ZaposleniSlava.AllAsNoTracking, "Id", "Naziv", zaposleni.SlavaId);
        //    ViewBag.StatusRadnika = new SelectList(BexUow.ZaposleniStatus.AllAsNoTracking, "Id", "StatusRadnika", zaposleni.StatusRadnika);
        //    return View(zaposleni);

        //}

        ////private void PopulateRegionsDropDownList(object selectedAddress = null)
        ////{
        ////    var departmentsQuery = from d in BexUow.Region.AllAsNoTracking
        ////                               //orderby d.Ulica
        ////                           select d;


        ////    ViewBag.RegionId = new SelectList(departmentsQuery, "Id", "Region", selectedAddress);
        ////}

        //// GET: KontaktTelefon/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    ViewBag.RegionId = new SelectList(BexUow.Region.AllAsNoTracking, "Id", "NazivSkraceni");
        //    ViewBag.SektorId = new SelectList(BexUow.Sektor.AllAsNoTracking, "Id", "NazivSektora");
        //    ViewBag.ZanimanjeId = new SelectList(BexUow.ZaposleniZanimanje.AllAsNoTracking, "Id", "Opis");
        //    ViewBag.RazlogOtkazaId = new SelectList(BexUow.ZaposleniRazlogOtkaza.AllAsNoTracking, "Id", "Opis");
        //    ViewBag.StrucnaSpremaId = new SelectList(BexUow.ZaposleniStrucnaSprema.AllAsNoTracking, "Id", "Opis");
        //    ViewBag.RadnoMestoId = new SelectList(BexUow.ZaposleniRadnoMesto.AllAsNoTracking, "Id", "InterniNaziv");
        //    ViewBag.FirmaPrijaveId = new SelectList(BexUow.Firma.AllAsNoTracking, "Id", "Naziv");
        //    ViewBag.FirmaUkojojRadiId = new SelectList(BexUow.Firma.AllAsNoTracking, "Id", "Naziv");
        //    ViewBag.OsiguranjeId = new SelectList(BexUow.ZaposleniOsnovOsiguranja.AllAsNoTracking, "Id", "Naziv");
        //    ViewBag.ProgramZaposlenjaId = new SelectList(BexUow.ZaposleniProgramZaposlenja.AllAsNoTracking, "Id", "Naziv");
        //    ViewBag.SlavaId = new SelectList(BexUow.ZaposleniSlava.AllAsNoTracking, "Id", "Naziv");
        //    ViewBag.StatusRadnika = new SelectList(BexUow.ZaposleniStatus.AllAsNoTracking, "Id", "StatusRadnika");
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }

        //    Zaposleni zaposleni = BexUow.Zaposleni.Find(id);
        //    if (zaposleni == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.KontaktId = zaposleni.KontaktId;
        //    if (zaposleni.KontaktId != null)
        //    {
        //        var kontakt = BexUow.Kontakts.Find(k => k.Id == zaposleni.KontaktId);
        //        ViewBag.KontaktNaziv = kontakt.Naziv;
        //    }
        //    return View(zaposleni);
        //}

        //// POST: KontaktTelefon/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(Zaposleni zaposleni)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        //BexUow.Entry(kontaktTelefon).State = EntityState.Modified;
        //        //db.SaveChanges();
        //        BexUow.Zaposleni.Update(zaposleni);
        //        var uowCommandResult = BexUow.SubmitChanges();

        //        if (uowCommandResult.IsSuccessful)
        //        { return RedirectToAction("Details", new { id = zaposleni.Id }); }

        //        ExceptionSolver.PrepareModelState(ModelState, uowCommandResult);
        //    }
        //    ViewBag.KontaktId = 11;
        //    ViewBag.RegionId = new SelectList(BexUow.Region.AllAsNoTracking, "Id", "NazivSkraceni", zaposleni.RegionId);
        //    ViewBag.SektorId = new SelectList(BexUow.Sektor.AllAsNoTracking, "Id", "NazivSektora", zaposleni.SektorId);
        //    ViewBag.ZanimanjeId = new SelectList(BexUow.ZaposleniZanimanje.AllAsNoTracking, "Id", "Opis", zaposleni.ZanimanjeId);
        //    ViewBag.RazlogOtkazaId = new SelectList(BexUow.ZaposleniRazlogOtkaza.AllAsNoTracking, "Id", "Opis", zaposleni.RazlogOtkazaId);
        //    ViewBag.StrucnaSpremaId = new SelectList(BexUow.ZaposleniStrucnaSprema.AllAsNoTracking, "Id", "Opis", zaposleni.StrucnaSpremaId);
        //    ViewBag.RadnoMestoId = new SelectList(BexUow.ZaposleniRadnoMesto.AllAsNoTracking, "Id", "InterniNaziv", zaposleni.RadnoMestoId);
        //    ViewBag.FirmaPrijaveId = new SelectList(BexUow.Firma.AllAsNoTracking, "Id", "Naziv", zaposleni.FirmaPrijaveId);
        //    ViewBag.FirmaUkojojRadiId = new SelectList(BexUow.Firma.AllAsNoTracking, "Id", "Naziv", zaposleni.FirmaUkojojRadiId);
        //    ViewBag.OsiguranjeId = new SelectList(BexUow.ZaposleniOsnovOsiguranja.AllAsNoTracking, "Id", "Naziv", zaposleni.OsiguranjeId);
        //    ViewBag.ProgramZaposlenjaId = new SelectList(BexUow.ZaposleniProgramZaposlenja.AllAsNoTracking, "Id", "Naziv", zaposleni.ProgramZaposlenjaId);
        //    ViewBag.SlavaId = new SelectList(BexUow.ZaposleniSlava.AllAsNoTracking, "Id", "Naziv", zaposleni.SlavaId);
        //    ViewBag.StatusRadnika = new SelectList(BexUow.ZaposleniStatus.AllAsNoTracking, "Id", "StatusRadnika", zaposleni.StatusRadnika);
        //    return View(zaposleni);
        //}

        //// GET: Zaposleni/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Zaposleni zaposleni = BexUow.Zaposleni.Find(id);
        //    if (zaposleni == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    else
        //    {
        //        BexUow.Zaposleni.Remove(
        //                    p => p.Id == id);


        //        var uowCommandResult = BexUow.SubmitChanges();

        //        if (!uowCommandResult.IsSuccessful)
        //        {
        //            ExceptionSolver.PrepareModelState(ModelState, uowCommandResult);
        //            ExceptionSolver.PrepareTempData(TempData, ModelState);
        //        }
        //    }
        //    return RedirectToAction("Index");
        //}

        //// POST: Zaposleni/Delete/5
        ////[HttpPost, ActionName("Delete")]
        ////[ValidateAntiForgeryToken]
        ////public ActionResult Delete(int id)
        ////{
        ////    Zaposleni zaposleni = BexUow.Zaposleni.Find(id);
        ////    //db.KontaktTelefons.Remove(kontaktTelefon);
        ////    //db.SaveChanges();
        ////    return RedirectToAction("Index");
        ////}

        //public ActionResult GetKontakt(string query)
        //{
        //    return Json(_GetKontakt(query), JsonRequestBehavior.AllowGet);
        //}
        //private List<Autocomplete> _GetKontakt(string query)
        //{
        //    List<Autocomplete> kontaktList = new List<Autocomplete>();
        //    try
        //    {

        //        var results = (from t in BexUow.Kontakts.GetKontaktAutocompliteData(query)
        //                       select new
        //                       {
        //                           NazivKontakt = t.Naziv,
        //                           AdresaKontakt = t.AdresaTekst

        //                       }).Take(10).ToList();

        //        foreach (var r in results)
        //        {
        //            // create objects
        //            Autocomplete kontakt = new Autocomplete();

        //            kontakt.Name = string.Format("{0} - {1}", r.NazivKontakt, r.AdresaKontakt);
        //            //models.Name = r.Naziv;
        //            kontakt.Id = 1;
        //            kontaktList.Add(kontakt);
        //        }

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
        //    return kontaktList;
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
