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
using Bex.Common.Interfaces;
using AspNet.DAL.EF.UOW.Security;
using System.Web;
using System.Threading.Tasks;
using AspNet.DAL.EF.Models.Security;
using Microsoft.AspNet.Identity;

namespace BexMVC.Controllers
{
    [Authorize]
    [ClaimsAuthentication(Resource = "Zaposleni", Operation = "Read, All")]
    public class ZaposleniController : Controller
    {
        public ZaposleniController() : this(new SecurityUow(System.Web.HttpContext.Current.GetOwinContext()), new BexUow(), new ExceptionSolver())
        { }
        public ZaposleniController(
            ISecurityUow securityUow,
            IBexUow bexUow,
            IExceptionSolver exceptionSolver)
        {
            SecurityUow = securityUow;
            BexUow = bexUow;
            ExceptionSolver = exceptionSolver;
        }
        public ActionResult Index(int? id, int? page, string searchString)
        {
            TempData.ToList().ForEach(keyValue =>
                ModelState.AddModelError("", keyValue.Value.ToString()));
            TempData.Clear();
            //ViewBag.Regioni = new SelectList(BexUow.Region.AllAsNoTracking, "NazivSkraceni", "NazivSkraceni");
            //ViewBag.Sektori = new SelectList(BexUow.Sektor.AllAsNoTracking, "NazivSektora", "NazivSektora");
            //ViewBag.Firme = new SelectList(BexUow.Firma.AllAsNoTracking, "Naziv", "Naziv");
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
        public async Task<ActionResult> GetZaposleni(int pageSize, int pageNumber, string sortColumn, string sortOrder, string search, string searchColumn, string searchTerms)
        {
            var skip = (pageNumber - 1) * pageSize;

            List<string> listaRegiona = await getAllClaims();

            var zaposleniData = (from z in BexUow.Zaposleni.AllAsNoTracking.Where(z => listaRegiona.Contains(z.RegionId.ToString()))
                                 let f = (from f in BexUow.KontaktFizickoLice.AllAsNoTracking
                                                  where f.KontaktId == z.KontaktId
                                                  select f).OrderByDescending(f => f.Id).Take(1).FirstOrDefault()
                                 let t = (from t in BexUow.KontaktTelefon.AllAsNoTracking
                                          where t.KontaktId == z.KontaktId
                                          select t).OrderByDescending(t => t.Id).Take(1).FirstOrDefault()
                                 let tr = (from tr in BexUow.KontaktZiroRacun.AllAsNoTracking
                                          where tr.KontaktId == z.KontaktId
                                          select tr).OrderByDescending(tr => tr.Id).Take(1).FirstOrDefault()
                                 //join f in BexUow.KontaktFizickoLice.AllAsNoTracking on z.KontaktId equals f.KontaktId
                                 //join t in BexUow.KontaktTelefon.AllAsNoTracking.Where(t => t.Posao==true) on z.KontaktId equals t.KontaktId
                                 //join tr in BexUow.KontaktZiroRacun.AllAsNoTracking on z.KontaktId equals tr.KontaktId
                                 select new ZaposleniIndexData
                                             {
                                                 Id = z.Id,
                                                 KontaktNaziv = z.Kontakt.Naziv ?? "",
                                                 RegionNazivSkraceni = z.Region.NazivSkraceni ?? "",
                                                 SektorNaziv = z.Sektor.NazivSektora ?? "",
                                                 FirmaNaziv = z.Firma.Naziv ?? "",
                                                 Aktivan = z.Aktivan ,
                                                 DatumZaposlenja = z.DatumZaposlenja,
                                                 Jmbg = f.MaticniBroj,
                                                 Zanimanje=z.ZaposleniZanimanje.Opis,
                                                 SS=z.ZaposleniStrucnaSprema.Opis,
                                                 RadniStaz=z.PrethodniRSgodina + "G" + z.PrethodniRSmesec + "M" + z.PrethodniRSdana + "D",
                                                 RadnoMesto=z.ZaposleniRadnoMesto.InterniNaziv,
                                                 Sistematizacija=z.ZaposleniRadnoMesto.SistematizacijaNaziv,
                                                 Status=z.ZaposleniStatus.StatusRadnika,
                                                 Telefon=t.Telefon,
                                                 Kategorija=f.KategorijaVozackeDozvole,
                                                 Grupa="",
                                                 DatumPrijave=z.DatumPrijave,
                                                 ProbniRad=z.DatumProbniRadDo,
                                                 DatumOdajve=z.DatumPrestankaRada,
                                                 DatumLekarskog=z.DatumLekarskog,
                                                 DatumLicnaKarta=f.DatumIstekaLicneKarte,
                                                 DatumVozacka=f.DatumIstekaVozackeDozvole,
                                                 NaOdredjenoDo=z.DatumNaOdredjenoDo,
                                                 Slava=z.ZaposleniSlava.Naziv,
                                                 TekuciRacun=tr.BrojZiroRacuna,
                                                 Adresa=z.Kontakt.AdresaTekst,
                                                 BrojLK=f.BrojLicneKarte,
                                                 OpstinaLK=f.MestoIzdavanjaLicneKarte,
                                                 Napomena =z.Napomena,
                                                 Program=z.ZaposleniProgramZaposlenja.Naziv,
                                                 Invalidno=z.InvalidnoLice,
                                                 BankaOvlascenje=z.OvlascenZaBanke,
                                                 RazlogOtkaza=z.ZaposleniRazlogOtkaza.Opis,
                                                 Pol=""                                            
                                             }).AsEnumerable();

            //var zaposleniData = BexUow.Zaposleni.GetAll(true).Select(x =>
            //                                 new ZaposleniIndexData
            //                                 {
            //                                     Id = x.Id,
            //                                     KontaktNaziv = x.Kontakt.Naziv ?? "",
            //                                     RegionNazivSkraceni = x.Region.NazivSkraceni ?? "",
            //                                     SektorNaziv = x.Sektor.NazivSektora ?? "",
            //                                     FirmaNaziv = x.Firma.Naziv ?? "",
            //                                     Aktivan = x.Aktivan ,
            //                                     DatumZaposlenja = x.DatumZaposlenja,
            //                                     Jmbg=x.Kontakt.f
            //                                 }).AsEnumerable();

            if (!String.IsNullOrEmpty(searchTerms))
            {
                zaposleniData = GetSearchZaposleniData(searchTerms, zaposleniData);
                                        //.Select(x => new ZaposleniIndexData
                                        //                 {
                                        //                     Id = x.Id,
                                        //                     KontaktNaziv = x.KontaktNaziv,
                                        //                     RegionNazivSkraceni = x.RegionNazivSkraceni,
                                        //                     SektorNaziv = x.SektorNaziv,
                                        //                     FirmaNaziv = x.FirmaNaziv,
                                        //                     Aktivan = x.Aktivan,
                                        //                     DatumZaposlenja = x.DatumZaposlenja
                                        //                 });
            }

            var total = zaposleniData.Count();

            if (sortOrder.Equals("desc"))
                zaposleniData = zaposleniData.OrderByDescending(s => s.GetType().GetProperty(sortColumn).GetValue(s)).ToList().Skip(skip).Take(pageSize);
            else
                zaposleniData = zaposleniData.OrderBy(s => s.GetType().GetProperty((sortColumn == "") ? "Id" : sortColumn).GetValue(s)).ToList().Skip(skip).Take(pageSize);



            var jsonData = new TableJsonIndexData<ZaposleniIndexData>()
            {
                total = total,
                rows = zaposleniData
            };

            var jsonResult = Json(jsonData, "TableJsonIndexData", JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;

            return jsonResult;
        }

        public IEnumerable<ZaposleniIndexData> GetSearchZaposleniData(string searchTerms, IEnumerable<ZaposleniIndexData> searchDataSet)
        {
            string[] terms = searchTerms.Split(',');

            var zaposleni = searchDataSet;

            foreach (string t in terms)
            {
                string searchColumn = "";
                string searchTxt = "";

                string[] searchCT = t.Split(':');
                searchColumn = searchCT[0];
                searchTxt = searchCT[1];

                if (!String.IsNullOrEmpty(searchTxt))
                {
                    if (searchColumn.Equals("KontaktNaziv"))
                    {
                        zaposleni = zaposleni.Where(k => !String.IsNullOrEmpty(k.KontaktNaziv)).Where(k => k.KontaktNaziv.ToUpper().Contains(searchTxt.ToUpper()));
                    }
                    else if (searchColumn.Equals("Region"))
                    {
                        zaposleni = zaposleni.Where(k => !String.IsNullOrEmpty(k.RegionNazivSkraceni)).Where(k => k.RegionNazivSkraceni.ToUpper().Contains(searchTxt.ToUpper()));
                    }
                    else if (searchColumn.Equals("Sektor"))
                    {
                        zaposleni = zaposleni.Where(k => k.SektorNaziv.ToUpper().Contains(searchTxt.ToUpper()));
                    }
                    else if (searchColumn.Equals("Firma"))
                    {
                        zaposleni = zaposleni.Where(k => !String.IsNullOrEmpty(k.FirmaNaziv)).Where(k => k.FirmaNaziv.ToUpper().Contains(searchTxt.ToUpper()));
                    }
                    else if (searchColumn.Equals("DatumZaposlenja"))
                    {
                        DateTime datumFirst, datumSecond;
                        string[] arrDatumAll = searchTxt.Split('t', 'o');

                        string[] arrDatumFirst = arrDatumAll[0].Trim().Split('-');
                        datumFirst = new DateTime(int.Parse(arrDatumFirst[0]), int.Parse(arrDatumFirst[1]), int.Parse(arrDatumFirst[2]));

                        string[] arrDatumSecond = arrDatumAll[2].Trim().Split('-');
                        datumSecond = new DateTime(int.Parse(arrDatumSecond[0]), int.Parse(arrDatumSecond[1]), int.Parse(arrDatumSecond[2]));

                        zaposleni = zaposleni.Where(k => k.DatumZaposlenja >= datumFirst && k.DatumZaposlenja <= datumSecond);


                        //string[] arrDatum = searchTxt.Split('/');
                        //DateTime thisDate1 = new DateTime(int.Parse(arrDatum[2]), int.Parse(arrDatum[0]), int.Parse(arrDatum[1]));
                        //zaposleni = zaposleni.Where(k => k.DatumZaposlenja == thisDate1);
                    }
                    else if (searchColumn.Equals("Aktivan"))
                    {
                        bool isAktivan = (searchTxt == "1") ? true : false;
                        zaposleni = zaposleni.Where(k => k.Aktivan.Equals(isAktivan));
                    }
                }
            }
            return zaposleni;
        }

        [ClaimsAuthentication(Resource = "Zaposleni", Operation = "Edit, All")]
        public ActionResult Edit(int id)
        {
            
            Zaposleni zaposleni = BexUow.Zaposleni.Find(p => p.Id == id);
            
            ViewBag.ZaposleniNaziv = BexUow.Kontakts.Find(x => x.Id == zaposleni.KontaktId).Naziv;
            ViewBag.ZaposleniId = zaposleni.Id;

            ViewBag.RazlogOtkazaId = new SelectList(BexUow.ZaposleniRazlogOtkaza.AllAsNoTracking, "Id", "Opis", zaposleni.RazlogOtkazaId);
            ViewBag.OsiguranjeId = new SelectList(BexUow.ZaposleniOsnovOsiguranja.AllAsNoTracking, "Id", "Naziv", zaposleni.OsiguranjeId);
            ViewBag.ProgramZaposlenjaId = new SelectList(BexUow.ZaposleniProgramZaposlenja.AllAsNoTracking, "Id", "Naziv", zaposleni.ProgramZaposlenjaId);
            ViewBag.StatusRadnika = new SelectList(BexUow.ZaposleniStatus.AllAsNoTracking, "Id", "StatusRadnika", zaposleni.StatusRadnika);

            if (zaposleni == null)
            {
                return HttpNotFound();
            }
            return View(zaposleni);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Zaposleni zaposleni)
        {
            if (ModelState.IsValid)
            {
                var zaposleniUpdate = BexUow.Zaposleni.Find(x => x.Id == zaposleni.Id);

                zaposleniUpdate.KontaktId = zaposleni.KontaktId;
                zaposleniUpdate.DatumZaposlenja = zaposleni.DatumZaposlenja;
                zaposleniUpdate.DatumPrijave = zaposleni.DatumPrijave;
                zaposleniUpdate.DatumNaOdredjenoDo = zaposleni.DatumNaOdredjenoDo;
                zaposleniUpdate.DatumPrestankaRada = zaposleni.DatumPrestankaRada;
                zaposleniUpdate.RazlogOtkazaId = zaposleni.RazlogOtkazaId;
                zaposleniUpdate.PrethodniRSgodina = zaposleni.PrethodniRSgodina;
                zaposleniUpdate.PrethodniRSmesec = zaposleni.PrethodniRSmesec;
                zaposleniUpdate.PrethodniRSdana = zaposleni.PrethodniRSdana;
                zaposleniUpdate.StatusRadnika = zaposleni.StatusRadnika;
                zaposleniUpdate.OsiguranjeId = zaposleni.OsiguranjeId;
                zaposleniUpdate.OvlascenZaBanke = zaposleni.OvlascenZaBanke;
                zaposleniUpdate.ProgramZaposlenjaId = zaposleni.ProgramZaposlenjaId;
                zaposleniUpdate.Napomena = zaposleni.Napomena;
                zaposleniUpdate.Aktivan = zaposleni.Aktivan;

                BexUow.Zaposleni.Update(zaposleniUpdate);
                var commandResult = BexUow.SubmitChanges();

                if (commandResult.IsSuccessful)
                { return RedirectToAction("Edit", "Zaposleni", new { id = zaposleni.Id }); }

                //ExceptionSolver.PrepareModelState(ModelState, commandResult);
            }

            return HttpNotFound();

        }

        public ActionResult _ProfileImage(int tipId, int id, int isProfile)
        {
            bool _isProfile = System.Convert.ToBoolean(isProfile);
            var imageProfile = (from galerija in BexUow.Gallery.AllAsNoTracking
                                join webfiles in BexUow.WebFiles.AllAsNoTracking on galerija.WebImageId equals webfiles.Id
                                where webfiles.TypeId == tipId && webfiles.StraniId == id && galerija.IsProfile == _isProfile
                                select new ImageList
                                {
                                    Id = galerija.Id,
                                    IsActive = galerija.IsActive,
                                    IsProfile = galerija.IsProfile,
                                    OrderNo = galerija.OrderNo,
                                    WebImageId = galerija.WebImageId,
                                    Title = galerija.Title,
                                    UpdateDate = webfiles.UpdateDate,
                                    UserUneo = webfiles.KorisniciPrograma.Kontakt.Naziv
                                }).FirstOrDefault();

            ViewBag.ZaposleniId = id;

            return PartialView(imageProfile);
        }


        [ChildActionOnly]
        public ActionResult CreateNapomenaPartial(string zaposleniId)
        {
            ViewBag.Id = zaposleniId;

            var zaposleniNapomena = new ZaposleniNapomena
            {
                ZaposleniId = System.Convert.ToInt32(zaposleniId)
            };
            return PartialView(zaposleniNapomena);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateNapomenaPartial(ZaposleniNapomena zaposleniNapomena)
        {
            if (ModelState.IsValid)
            {
                var applicationUser = await SecurityUow.UserManager.FindUserByIdAsync(User.Identity.GetUserId()) as ApplicationUser;
                var korisnikProgramaId = BexUow.KorisniciPrograma.Find(x => x.AspNetUserId == applicationUser.Id).Id;
                zaposleniNapomena.UneoUserId = korisnikProgramaId;
                zaposleniNapomena.DatumUnosa = DateTime.Now;

                BexUow.ZaposleniNapomena.Add(zaposleniNapomena);
                var uowCommandResult = BexUow.SubmitChanges();

                if (uowCommandResult.IsSuccessful)
                {
                   //ViewBag.Id = zaposleniNapomena.ZaposleniId;
                    return PartialView(zaposleniNapomena);
                }

                //ExceptionSolver.PrepareModelState(ModelState, uowCommandResult);
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }


        public ActionResult DetailsPlata(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            ZaposleniPlata zaposleniPlata = BexUow.ZaposleniPlata.Find(p => p.ZaposleniId == id);
            if (zaposleniPlata == null)
            {
                return HttpNotFound();
            }
            return View(zaposleniPlata);
        }

        [ClaimsAuthentication(Resource = "Zaposleni", Operation = "Create, All")]
        public ActionResult Create(int kontaktId = 0)
        {

            ViewBag.RazlogOtkazaId = new SelectList(BexUow.ZaposleniRazlogOtkaza.AllAsNoTracking, "Id", "Opis");
            ViewBag.OsiguranjeId = new SelectList(BexUow.ZaposleniOsnovOsiguranja.AllAsNoTracking, "Id", "Naziv");
            ViewBag.ProgramZaposlenjaId = new SelectList(BexUow.ZaposleniProgramZaposlenja.AllAsNoTracking, "Id", "Naziv");
            ViewBag.StatusRadnika = new SelectList(BexUow.ZaposleniStatus.AllAsNoTracking, "Id", "StatusRadnika");

            return View();
        }

        [HttpGet]
        public ActionResult GetDetailsKontakt(int? kontaktId)
        {
            var kontaktDetails = BexUow.KontaktFizickoLice.AllAsNoTracking.Where(x => x.KontaktId == kontaktId)
                                                            .Select(x =>
                                                                       new 
                                                                       {
                                                                           x.Drzavljanstvo,
                                                                           x.MaticniBroj,
                                                                           x.BrojLicneKarte,
                                                                           x.MestoIzdavanjaLicneKarte,
                                                                           x.DatumIstekaLicneKarte,
                                                                           x.BrojVozackeDozvole,
                                                                           x.DatumIstekaVozackeDozvole,
                                                                           x.Kontakt.Telefon.FirstOrDefault().Telefon,
                                                                           x.Kontakt.Email.FirstOrDefault().EmailAdresa,
                                                                           x.Kontakt.ZiroRacun.FirstOrDefault().BrojZiroRacuna
                                                                       }).ToList();
            return Json(kontaktDetails, JsonRequestBehavior.AllowGet);
        }


        

        [ChildActionOnly]
        public ActionResult CreateLicniPodaciPartial(string zaposleniId)
        {
            int zaposleniIdInt = System.Convert.ToInt32(zaposleniId);
            var zaposleni = BexUow.Zaposleni.Find(x => x.Id == zaposleniIdInt);

            ViewBag.ZaposleniId = zaposleniId;
            ViewBag.SlavaId = new SelectList(BexUow.ZaposleniSlava.AllAsNoTracking, "Id", "Naziv", zaposleni.SlavaId);
            ViewBag.AddLicniPodaci = false;
            ViewBag.HasInvalidnoLice = false;

            
            if (zaposleni != null) 
            {
                ViewBag.HasInvalidnoLice = (zaposleni.InvalidnoLice == true) ? true : false;
            }
            return View(zaposleni);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateLicniPodaciPartial(Zaposleni zaposleni)
        {
            if (ModelState.IsValid)
            {
                var zaposleniUpdate = BexUow.Zaposleni.Find(x => x.Id == zaposleni.Id);

                zaposleniUpdate.ImeRoditelja = zaposleni.ImeRoditelja;
                zaposleniUpdate.SlavaId = zaposleni.SlavaId;
                zaposleniUpdate.InvalidnoLice = zaposleni.InvalidnoLice;
                zaposleniUpdate.DatumLekarskog = zaposleni.DatumLekarskog;

                BexUow.Zaposleni.Update(zaposleniUpdate);
                var commandResult = BexUow.SubmitChanges();

                if (commandResult.IsSuccessful)
                {
                    ViewBag.AddLicniPodaci = true;
                    ViewBag.HasInvalidnoLice = (zaposleniUpdate.InvalidnoLice == true) ?  true : false;
                    ViewBag.SlavaId = new SelectList(BexUow.ZaposleniSlava.AllAsNoTracking, "Id", "Naziv", zaposleniUpdate.SlavaId);
                    ViewBag.ZaposleniId = zaposleni.Id;
                    return PartialView(zaposleni);
                }

                ExceptionSolver.PrepareModelState(ModelState, commandResult);
            }

            return View(zaposleni);

        }

        [ChildActionOnly]
        public ActionResult CreateObrazovanjePartial(string zaposleniId)
        {
            int zaposleniIdInt = System.Convert.ToInt32(zaposleniId);
            var zaposleni = BexUow.Zaposleni.Find(x => x.Id == zaposleniIdInt);

            ViewBag.ZaposleniId = zaposleniId;
            ViewBag.AddObrazovanje = false;
            ViewBag.StrucnaSpremaId = new SelectList(BexUow.ZaposleniStrucnaSprema.AllAsNoTracking, "Id", "Opis",zaposleni.StrucnaSpremaId);
            ViewBag.ZanimanjeId = new SelectList(BexUow.ZaposleniZanimanje.AllAsNoTracking, "Id", "Opis",zaposleni.ZanimanjeId);

            return View(zaposleni);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateObrazovanjePartial(Zaposleni zaposleni)
        {
            if (ModelState.IsValid)
            {
                var zaposleniUpdate = BexUow.Zaposleni.Find(x => x.Id == zaposleni.Id);

                zaposleniUpdate.StrucnaSpremaId = zaposleni.StrucnaSpremaId;
                zaposleniUpdate.ZanimanjeId = zaposleni.ZanimanjeId;
                
                BexUow.Zaposleni.Update(zaposleniUpdate);
                var commandResult = BexUow.SubmitChanges();

                if (commandResult.IsSuccessful)
                {
                    ViewBag.AddObrazovanje = true;
                    ViewBag.StrucnaSpremaId = new SelectList(BexUow.ZaposleniStrucnaSprema.AllAsNoTracking, "Id", "Opis",zaposleni.StrucnaSpremaId);
                    ViewBag.ZanimanjeId = new SelectList(BexUow.ZaposleniZanimanje.AllAsNoTracking, "Id", "Opis",zaposleni.ZanimanjeId);
                    ViewBag.ZaposleniId = zaposleni.Id;
                    return PartialView(zaposleni);
                }

                ExceptionSolver.PrepareModelState(ModelState, commandResult);
            }

            return View(zaposleni);

        }

        

        [ChildActionOnly]
        public ActionResult CreateZaposlenjePartial(string zaposleniId)
        {
            int zaposleniIdInt = System.Convert.ToInt32(zaposleniId);
            var zaposleni = BexUow.Zaposleni.Find(x => x.Id == zaposleniIdInt);

            ViewBag.ZaposleniId = zaposleniId;
            ViewBag.RegionId = new SelectList(BexUow.Region.AllAsNoTracking, "Id", "NazivSkraceni",zaposleni.RegionId);
            ViewBag.SektorId = new SelectList(BexUow.Sektor.AllAsNoTracking, "Id", "NazivSektora",zaposleni.SektorId);
            ViewBag.RadnoMestoId = new SelectList(BexUow.ZaposleniRadnoMesto.AllAsNoTracking, "Id", "InterniNaziv",zaposleni.RadnoMestoId);
            ViewBag.FirmaPrijaveId = new SelectList(BexUow.Firma.AllAsNoTracking, "Id", "Naziv",zaposleni.FirmaPrijaveId);

            ViewBag.AddZaposlenje = false;

            return View(zaposleni);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateZaposlenjePartial(Zaposleni zaposleni)
        {
            if (ModelState.IsValid)
            {
                var zaposleniUpdate = BexUow.Zaposleni.Find(x => x.Id == zaposleni.Id);

                zaposleniUpdate.RegionId = zaposleni.RegionId;
                zaposleniUpdate.SektorId = zaposleni.SektorId;
                zaposleniUpdate.FirmaPrijaveId = zaposleni.FirmaPrijaveId;
                zaposleniUpdate.RadnoMestoId = zaposleni.RadnoMestoId;
                zaposleniUpdate.KontaktSefId = zaposleni.KontaktSefId;

                BexUow.Zaposleni.Update(zaposleniUpdate);
                var commandResult = BexUow.SubmitChanges();

                if (commandResult.IsSuccessful)
                {
                    ViewBag.AddZaposlenje = true;
                    ViewBag.RegionId = new SelectList(BexUow.Region.AllAsNoTracking, "Id", "NazivSkraceni",zaposleni.RegionId);
                    ViewBag.SektorId = new SelectList(BexUow.Sektor.AllAsNoTracking, "Id", "NazivSektora",zaposleni.SektorId);
                    ViewBag.RadnoMestoId = new SelectList(BexUow.ZaposleniRadnoMesto.AllAsNoTracking, "Id", "InterniNaziv",zaposleni.RadnoMestoId);
                    ViewBag.FirmaPrijaveId = new SelectList(BexUow.Firma.AllAsNoTracking, "Id", "Naziv",zaposleni.FirmaPrijaveId);
                    ViewBag.ZaposleniId = zaposleni.Id;
                    return PartialView(zaposleni);
                }

                ExceptionSolver.PrepareModelState(ModelState, commandResult);
            }

            return View(zaposleni);

        }

        [ChildActionOnly]
        public ActionResult CreatePlataPartial(string zaposleniId)
        {
            ViewBag.ZaposleniId = zaposleniId;
            ViewBag.AddPlata = false;
            ViewBag.HasMinimalac = false;
            

            int zaposleniIdInt = System.Convert.ToInt32(zaposleniId);
            var zaposleniPlata = BexUow.ZaposleniPlata.Find(x => x.ZaposleniId == zaposleniIdInt);
            if (zaposleniPlata != null) 
            {
                ViewBag.HasMinimalac = (zaposleniPlata.PlataMinimalac == true) ? true : false;
                
            }
            else
            {
                zaposleniPlata = new ZaposleniPlata
                {
                    ZaposleniId = zaposleniIdInt
                };
                
            }

            return View(zaposleniPlata);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreatePlataPartial(ZaposleniPlata plata)
        {
            if (ModelState.IsValid)
            {
                var zaposleniPlata = BexUow.ZaposleniPlata.Find(x => x.ZaposleniId == plata.ZaposleniId);
                if (zaposleniPlata == null)
                {
                    BexUow.ZaposleniPlata.Add(plata);
                }
                else
                {
                    zaposleniPlata.ZaposleniId = plata.ZaposleniId;
                    zaposleniPlata.PlataBruto = plata.PlataBruto;
                    zaposleniPlata.PlataNeto = plata.PlataNeto;
                    zaposleniPlata.PlataNapomena = plata.PlataNapomena;
                    zaposleniPlata.PlataMinimalac = plata.PlataMinimalac;
                    BexUow.ZaposleniPlata.Update(zaposleniPlata);
                }

                var commandResult = BexUow.SubmitChanges();

                if (commandResult.IsSuccessful)
                {
                    ViewBag.AddPlata = true;
                    ViewBag.HasMinimalac = (plata.PlataMinimalac == true) ? true : false;
                    ViewBag.ZaposleniId = plata.ZaposleniId;
                    return PartialView(plata);
                }
            }

            return View(plata);

        }




        //// POST: KontaktTelefon/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Zaposleni zaposleni)
        {
            if (ModelState.IsValid)
            {
               
                BexUow.Zaposleni.Add(zaposleni);
                var commandResult = BexUow.SubmitChanges();

                if (commandResult.IsSuccessful)
                { return RedirectToAction("Edit", "Zaposleni", new { id = zaposleni.Id }); }

                ExceptionSolver.PrepareModelState(ModelState, commandResult);
            }
            
            return  View(zaposleni);
            
        }

        public ActionResult Details(int id)
        {
            ImageEditorViewModel vm = new ImageEditorViewModel();
            ViewBag.Action = "Create";
            ViewBag.TipId = 4;
            ViewBag.Id = id;
            return PartialView(vm);
            //return RedirectToAction("_List", "GalleryAdmin", new { tipId, id });
        }

        [ClaimsAuthentication(Resource = "Zaposleni", Operation = "DetailsNapomene, All")]
        [HttpGet]
        public ActionResult GetZaposleniNapomeneData(int? id)
        {

            var detailsNapomene = BexUow.ZaposleniNapomena.GetAll(true).Where(x => x.ZaposleniId == id)
                                                            .Select(x =>
                                                                       new
                                                                       {
                                                                           DatumUnosa = x.DatumUnosa,
                                                                           UserUnosa = x.KorisniciPrograma.Kontakt.Naziv,
                                                                           Napomena = x.Napomena
                                                                       }).ToList();

            return Json(detailsNapomene, "", JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetZaposleniAutoComplete(string query)
        {
          return Json(_GetZaposleniAutoComplete(query), JsonRequestBehavior.AllowGet);
        }
        private List<Autocomplete> _GetZaposleniAutoComplete(string query)
        {
            List<Autocomplete> zaposleniList = new List<Autocomplete>();
            try
            {

                zaposleniList = BexUow.Zaposleni.GetZaposleniAutocompleteData(query)
                                                .Select(a => new Autocomplete
                                                {
                                                    Id = a.KontaktId,
                                                    Name = string.Format("{0} - {1}", a.Kontakt.Naziv, a.ZaposleniRadnoMesto.InterniNaziv),
                                                    PomocniId = a.ZaposleniRadnoMesto.Id
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
            catch(Exception ex)
            {
                ViewBag.Error = ex.Message;
                throw;
            }
            return zaposleniList;
        }
        public ActionResult GetZaposleniPoStaromAutoComplete(string query)
        {
            return Json(_GetZaposleniPoStaromAutoComplete(query), JsonRequestBehavior.AllowGet);
        }
        private List<Autocomplete> _GetZaposleniPoStaromAutoComplete(string query)
        {
            List<Autocomplete> zaposleniList = new List<Autocomplete>();
            try
            {

                zaposleniList = BexUow.Zaposleni.GetZaposleniPoStaromAutocompleteData(query)
                                                .Select(a => new Autocomplete
                                                {
                                                    Id = a.Id,
                                                    Name = string.Format("{0} - {1}", a.Kontakt.Naziv, a.ZaposleniRadnoMesto.InterniNaziv),
                                                    PomocniId = a.ZaposleniRadnoMesto.Id
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
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                throw;
            }
            return zaposleniList;
        }

        //[ClaimsAuthentication(Resource = "Zaposleni", Operation = "EvidencijaRada, All")]

        public ActionResult Evidencija(string id)
        {
            ViewBag.StatusId = new SelectList(BexUow.ZaposleniDanStatus.AllAsNoTracking, "Id", "Naziv");
            ViewBag.SelectedListId = id;
            
            return View();
           
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Evidencija(ZaposleniDanIndexData zaposleniDan)
        {
            if (ModelState.IsValid)
            {

                var applicationUser = await SecurityUow.UserManager.FindUserByIdAsync(User.Identity.GetUserId()) as ApplicationUser;
                var korisnikProgramaId = BexUow.KorisniciPrograma.Find(x => x.AspNetUserId == applicationUser.Id).Id;
                List<string> listId = zaposleniDan.Id.Split(',').ToList();
                foreach (var val in listId)
                {
                    var zaposleni = new ZaposleniDan
                    {
                        ZaposleniID =System.Convert.ToInt32(val),
                        Datum = zaposleniDan.Datum,
                        VremeOd = zaposleniDan.VremeOd,
                        VremeDo = zaposleniDan.VremeDo,
                        StatusId = zaposleniDan.StatusId,
                        UserUnosId = korisnikProgramaId
                    };
                    BexUow.ZaposleniDan.Add(zaposleni);
                }

                var commandResult = BexUow.SubmitChanges();

                if (commandResult.IsSuccessful)
                { return RedirectToAction("Index", "Zaposleni", new { izmena = 0 }); }

                ExceptionSolver.PrepareModelState(ModelState, commandResult);
            }
            return View();
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
