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
using Microsoft.AspNet.Identity;
using AspNet.DAL.EF.Models.Security;

namespace BexMVC.Controllers
{
    [Authorize]
    [ClaimsAuthentication(Resource = "Cekovi", Operation = "Read, All")]
    public class CekoviController : Controller
    {
        public CekoviController() : this(new SecurityUow(System.Web.HttpContext.Current.GetOwinContext()), new BexUow())
        { }
        public CekoviController(
            ISecurityUow securityUow, IBexUow bexUow)
        {
            SecurityUow = securityUow;
            BexUow = bexUow;
        }
        public ActionResult Index(int izmena)
        {
            TempData.ToList().ForEach(keyValue =>
                ModelState.AddModelError("", keyValue.Value.ToString()));
            TempData.Clear();

            ViewBag.Izmena = izmena;
            //ViewBag.BankaNaziv = new SelectList(BexUow.Banke.AllAsNoTracking, "NazivBanke", "NazivBanke");
            //ViewBag.ProvizijaIznos = new SelectList(BexUow.CekoviProvizija.AllAsNoTracking, "Iznos", "Iznos");

            return View();

        }

        [HttpGet]      
        public ActionResult GetCekovi(string pageSize, int pageNumber, string sortColumn, string sortOrder, string search, string searchColumn, string searchTerms)
        {


            var cekoviData = BexUow.Cekovi.GetAll(true).Select(x =>
                                                         new CekoviIndexData
                                                         {
                                                             Id = x.Id,
                                                             DatumDospeca = x.DatumDospeca,
                                                             BrojCeka = x.BrojCeka,
                                                             Iznos = x.IznosCeka,
                                                             TekuciRacun = x.BrojTekucegRacuna,
                                                             BankaNaziv = x.Banka.NazivBanke,
                                                             BankaId = x.BankaId,
                                                             BankaTekuciRacun = x.Banka.BrojRacuna,
                                                             DatumUnosa = x.DatumUnosa,
                                                             UserUneo = x.UserUneo.Kontakt.Naziv,
                                                             BrojSpecifikacije = x.BrojSpecifikacije,
                                                             InternoRazduzen = x.InternoRazduzen,
                                                             RealizovanCek = ((x.DatumRealizacije == null) ? false : true),
                                                             //DanasDostava = (dostave < 50) ? 1 : -1
                                                             DatumRealizacije = x.DatumRealizacije,
                                                             ZastupnikNaziv = x.KontaktUloga.Nadimak.ToString(),
                                                             ProvizijaIznos = x.CekProvizija.Iznos,
                                                             Napomena = x.Napomena ?? "",
                                                             Storno = x.Storno
                                                             
                                                         }).AsEnumerable();


            if (!String.IsNullOrEmpty(searchTerms))
            {
                cekoviData = GetSearchCekoviData(searchTerms, cekoviData).AsEnumerable(); 
            }

            var total = cekoviData.Count();

            int pageSizeInt = (pageSize != "Sve") ? System.Convert.ToInt32(pageSize) : total;

            var skip = (pageNumber - 1) * pageSizeInt;

            if (sortOrder.Equals("desc"))
                cekoviData = cekoviData.OrderByDescending(s => s.GetType().GetProperty(sortColumn).GetValue(s)).ToList().Skip(skip).Take(pageSizeInt);
            else
                cekoviData = cekoviData.OrderBy(s => s.GetType().GetProperty((sortColumn == "") ? "Id" : sortColumn).GetValue(s)).ToList().Skip(skip).Take(pageSizeInt);
            

            var jsonData = new TableJsonIndexData<CekoviIndexData>()
            {
                total = total,
                rows = cekoviData
            };
            var jsonResult = Json(jsonData, "TableJsonIndexData", JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
            
        }

        public IEnumerable<CekoviIndexData> GetSearchCekoviData(string searchTerms, IEnumerable<CekoviIndexData> cekoviData)
        {
            string[] terms = searchTerms.Split(',');

            //var cekoviData = searchDataSet;
            foreach (string t in terms)
            {
                string[] searchCT = t.Split(':');

                string searchColumn = "";
                string searchTxt = "";

                searchColumn = searchCT[0];
                searchTxt = searchCT[1];


                if (searchColumn.Equals("DatumDospeca") && !String.IsNullOrEmpty(searchTxt))
                {
                    DateTime datumFirst, datumSecond;
                    string[] arrDatumAll = searchTxt.Split('t', 'o');

                    string[] arrDatumFirst = arrDatumAll[0].Trim().Split('-');
                    datumFirst = new DateTime(int.Parse(arrDatumFirst[0]), int.Parse(arrDatumFirst[1]), int.Parse(arrDatumFirst[2]));

                    string[] arrDatumSecond = arrDatumAll[2].Trim().Split('-');
                    datumSecond = new DateTime(int.Parse(arrDatumSecond[0]), int.Parse(arrDatumSecond[1]), int.Parse(arrDatumSecond[2]));

                    cekoviData = cekoviData.Where(k => k.DatumDospeca >= datumFirst && k.DatumDospeca <= datumSecond);
                }
                else if (searchColumn.Equals("DatumUnosa") && !String.IsNullOrEmpty(searchTxt))
                {
                    DateTime datumFirst, datumSecond;
                    string[] arrDatumAll = searchTxt.Split('t', 'o');

                    string[] arrDatumFirst = arrDatumAll[0].Trim().Split('-');
                    datumFirst = new DateTime(int.Parse(arrDatumFirst[0]), int.Parse(arrDatumFirst[1]), int.Parse(arrDatumFirst[2]));

                    string[] arrDatumSecond = arrDatumAll[2].Trim().Split('-');
                    datumSecond = new DateTime(int.Parse(arrDatumSecond[0]), int.Parse(arrDatumSecond[1]), int.Parse(arrDatumSecond[2]));

                    cekoviData = cekoviData.Where(k => k.DatumUnosa.Value.Date >= datumFirst && k.DatumUnosa.Value.Date <= datumSecond);
                   
                }
                else if (searchColumn.Equals("DatumRealizacije") && !String.IsNullOrEmpty(searchTxt))
                {
                    DateTime datumFirst, datumSecond;
                    string[] arrDatumAll = searchTxt.Split('t', 'o');

                    string[] arrDatumFirst = arrDatumAll[0].Trim().Split('-');
                    datumFirst = new DateTime(int.Parse(arrDatumFirst[0]), int.Parse(arrDatumFirst[1]), int.Parse(arrDatumFirst[2]));

                    string[] arrDatumSecond = arrDatumAll[2].Trim().Split('-');
                    datumSecond = new DateTime(int.Parse(arrDatumSecond[0]), int.Parse(arrDatumSecond[1]), int.Parse(arrDatumSecond[2]));
                    cekoviData = cekoviData.Where(k => k.DatumRealizacije.Value >= datumFirst && k.DatumRealizacije.Value <= datumSecond);
                }
                
                else if (searchColumn.Equals("BrojCeka") && !String.IsNullOrEmpty(searchTxt))
                {
                    cekoviData = cekoviData.Where(k => k.BrojCeka.ToUpper().Contains(searchTxt.ToUpper()));
                }
                else if (searchColumn.Equals("Iznos") && !String.IsNullOrEmpty(searchTxt))
                {
                    int iznosInt = System.Convert.ToInt32(searchTxt);
                    cekoviData = cekoviData.Where(k => k.Iznos.Equals(iznosInt));
                }
                else if (searchColumn.Equals("TekuciRacun") && !String.IsNullOrEmpty(searchTxt))
                {
                    cekoviData = cekoviData.Where(k => k.TekuciRacun.ToUpper().Contains(searchTxt.ToUpper()));
                }
                else if (searchColumn.Equals("BankaNaziv") && !String.IsNullOrEmpty(searchTxt))
                {
                    cekoviData = cekoviData.Where(k => k.BankaNaziv.ToUpper().Contains(searchTxt.ToUpper()));
                }
                else if (searchColumn.Equals("BankaTekuciRacun") && !String.IsNullOrEmpty(searchTxt))
                {
                    cekoviData = cekoviData.Where(k => k.BankaTekuciRacun.ToUpper().Contains(searchTxt.ToUpper()));
                }
                else if (searchColumn.Equals("UserUneo") && !String.IsNullOrEmpty(searchTxt))
                {
                    cekoviData = cekoviData.Where(k => k.UserUneo.ToUpper().Contains(searchTxt.ToUpper()));
                }
                else if (searchColumn.Equals("BrojSpecifikacije") && !String.IsNullOrEmpty(searchTxt))
                {
                    int brSpecInt = System.Convert.ToInt32(searchTxt);
                    cekoviData = cekoviData.Where(k => k.BrojSpecifikacije.Equals(brSpecInt));
                }
                else if (searchColumn.Equals("InternoRazduzen") && !String.IsNullOrEmpty(searchTxt))
                {
                    bool internoRazduzen = (searchTxt == "1") ? true : false;
                    cekoviData = cekoviData.Where(k => k.InternoRazduzen.Equals(internoRazduzen));
                }
                else if (searchColumn.Equals("RealizovanCek") && !String.IsNullOrEmpty(searchTxt))
                {
                    bool realizovanCek = (searchTxt == "1") ? true : false;
                    cekoviData = cekoviData.Where(k => k.RealizovanCek.Equals(realizovanCek));
                }
                else if (searchColumn.Equals("ZastupnikNaziv") && !String.IsNullOrEmpty(searchTxt))
                {
                    cekoviData = cekoviData.Where(k => k.ZastupnikNaziv.ToUpper().Contains(searchTxt.ToUpper()));
                }
                else if (searchColumn.Equals("ProvizijaIznos") && !String.IsNullOrEmpty(searchTxt))
                {
                    int provizijaInt = System.Convert.ToInt32(searchTxt);
                    cekoviData = cekoviData.Where(k => k.ProvizijaIznos.Equals(provizijaInt));
                }
                else if (searchColumn.Equals("Napomena") && !String.IsNullOrEmpty(searchTxt))
                {
                    cekoviData = cekoviData.Where(k => k.Napomena.ToUpper().Contains(searchTxt.ToUpper()));
                }
                else if (searchColumn.Equals("Storno") && !String.IsNullOrEmpty(searchTxt))
                {
                    bool storno = (searchTxt == "1") ? true : false;
                    cekoviData = cekoviData.Where(k => k.Storno.Equals(storno));
                }



            }
            return cekoviData;
        }

        [ClaimsAuthentication(Resource = "Cekovi", Operation = "Create, All")]
        public ActionResult Create()
        {

            ViewBag.BankaId = new SelectList(BexUow.Banke.AllAsNoTracking, "Id", "NazivBanke");
            ViewBag.ProvizijaId = new SelectList(BexUow.CekoviProvizija.AllAsNoTracking, "Id", "Iznos");
            
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Cekovi cekovi)
        {
            if (ModelState.IsValid)
            {
                var cek = BexUow.Cekovi.Find(x => x.BrojCeka == cekovi.BrojCeka);
                if(cek != null)
                {
                    ModelState.AddModelError("", "Postoji ček sa istim brojem");
                    ViewBag.BankaId = new SelectList(BexUow.Banke.AllAsNoTracking, "Id", "NazivBanke");
                    ViewBag.ProvizijaId = new SelectList(BexUow.CekoviProvizija.AllAsNoTracking, "Id", "Iznos");
                    return View(cekovi);
                }
                
               var applicationUser = await SecurityUow.UserManager.FindUserByIdAsync(User.Identity.GetUserId()) as ApplicationUser;
                var korisnikProgramaId = BexUow.KorisniciPrograma.Find(x => x.AspNetUserId == applicationUser.Id).Id;
                cekovi.UserUneoId = korisnikProgramaId;
                cekovi.DatumUnosa = DateTime.Now;
                cekovi.DatumRealizacije = null;

                BexUow.Cekovi.Add(cekovi);
                var commandResult = BexUow.SubmitChanges();

                //if (commandResult.IsSuccessful)
                //{
                //    return RedirectToAction("Index", "Cekovi", new { izmena = 0});
                //}

                if(commandResult.IsSuccessful)
                {
                    return Json("Uspešno ste dodali ček", JsonRequestBehavior.AllowGet);
                }

                ExceptionSolver.PrepareModelState(ModelState, commandResult);
            }
            //return View();
            return Json("Greška pri dodavanju čeka.", JsonRequestBehavior.AllowGet);
        }

        [ClaimsAuthentication(Resource = "Cekovi", Operation = "Edit, All")]
        public ActionResult Edit(int id)
        {
            Cekovi cekovi = BexUow.Cekovi.Find(id);
            if (cekovi == null)
            {
                return HttpNotFound();
            }

            if (cekovi.DatumRealizacije!=null)
            {
                return RedirectToAction("Index", "Cekovi", new { izmena = 1});
            }


            ViewBag.Zastupnik = (cekovi.ZastupnikId > 0) ? BexUow.KontaktUloga.Find(cekovi.ZastupnikId).Nadimak : "";

            ViewBag.BankaId = new SelectList(BexUow.Banke.AllAsNoTracking, "Id", "NazivBanke", cekovi.BankaId);
            ViewBag.ProvizijaId = new SelectList(BexUow.CekoviProvizija.AllAsNoTracking, "Id", "Iznos", cekovi.ProvizijaId);

            return View(cekovi);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Cekovi cekovi)
        {
            if (ModelState.IsValid)
            {

                BexUow.Cekovi.Update(cekovi);
                var uowCommandResult = BexUow.SubmitChanges();

                if (uowCommandResult.IsSuccessful)
                { /*return RedirectToAction("Index", "Cekovi", new { izmena = 0 }); */
                    return Json("Uspešno ste izmenili ček", JsonRequestBehavior.AllowGet);
                }

                ExceptionSolver.PrepareModelState(ModelState, uowCommandResult);
            }

            //return View(cekovi);
            return Json("Greška pri izmeni čeka", JsonRequestBehavior.AllowGet);
        }

        [ClaimsAuthentication(Resource = "Cekovi", Operation = "ClickInternoRazduzen, All")]
        public ActionResult InternoRazduzen(int id)
        {

            var cek = BexUow.Cekovi.Find(id);
            cek.InternoRazduzen = true;

            BexUow.Cekovi.Update(cek);
            var uowCommandResult = BexUow.SubmitChanges();

            if (uowCommandResult.IsSuccessful)
            { return RedirectToAction("Index", "Cekovi", new { izmena = 0 }); }


            return View(cek);
        }

        public JsonResult RealizujPoSpecifikaciji(SelectedValue values)
        {

            foreach (var val in values.SelectedValues)
            {
                int cekId = val.Id;
                var cek = BexUow.Cekovi.Find(cekId);
                if (cek.BrojSpecifikacije > 0)
                {
                    cek.DatumRealizacije = DateTime.Now;
                    BexUow.Cekovi.Update(cek);
                    var uowCommandResult = BexUow.SubmitChanges();
                }              

            }

            return Json(new { success = "true" });

        }


        public ActionResult KreiranjeSpecifikacija(string kreirajSpec, string datumDospeca, string bankaId)
        {

            ViewBag.StampajSpecifikaciju = kreirajSpec;
            ViewBag.DatumDospecaSpec = datumDospeca;
            ViewBag.BankaIdSpec = bankaId;
            ViewBag.BankaId = new SelectList(BexUow.Banke.AllAsNoTracking, "Id", "NazivBanke");
            return View();
        }

        [HttpGet]
        public ActionResult GetCekoviZaSpecifikaciju(string datumDospeca, string banka)
        {
            DateTime? datum= null;
            int bankaId = 0;
            
            var cekovi=BexUow.Cekovi.GetAll(true).Where(x => x.InternoRazduzen == false && x.Storno == false && x.BrojSpecifikacije == 0)
                                             .Select (x =>
                                                new CekoviIndexData
                                                {
                                                    Id=x.Id,
                                                    BankaId = x.BankaId,
                                                    BankaNaziv = x.Banka.NazivBanke,
                                                    BrojCeka=x.BrojCeka,
                                                    Iznos=x.IznosCeka,
                                                    TekuciRacun=x.BrojTekucegRacuna,
                                                    DatumUnosa=x.DatumUnosa,
                                                    DatumDospeca=x.DatumDospeca
                                                }).OrderByDescending(x => x.DatumUnosa)
                                                            .AsEnumerable();

            if (!String.IsNullOrEmpty(datumDospeca) && !String.IsNullOrEmpty(banka))
            {
                bankaId = System.Convert.ToInt32(banka);
                string[] arrDatumFirst = datumDospeca.Split('-');
                datum = new DateTime(int.Parse(arrDatumFirst[2]), int.Parse(arrDatumFirst[1]), int.Parse(arrDatumFirst[0]));

                cekovi = cekovi.Where(x => x.DatumDospeca == datum && x.BankaId == bankaId);
            }
            else if (!String.IsNullOrEmpty(datumDospeca))
            {
                string[] arrDatumFirst = datumDospeca.Split('-');
                datum = new DateTime(int.Parse(arrDatumFirst[2]), int.Parse(arrDatumFirst[1]), int.Parse(arrDatumFirst[0]));

                cekovi = cekovi.Where(x => x.DatumDospeca == datum);
            }
            else if (!String.IsNullOrEmpty(banka))
            {
                bankaId = System.Convert.ToInt32(banka);
                cekovi = cekovi.Where(x => x.BankaId == bankaId);
            }

            return Json(cekovi, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddSpecifikacija(string datumDospeca, string bankaId)
        {
            DateTime datum = DateTime.Now.Date;
            int bankaIdInt = 0;
            if (datumDospeca != "")
            {
                string[] arrDatumFirst = datumDospeca.Split('-');
                datum = new DateTime(int.Parse(arrDatumFirst[2]), int.Parse(arrDatumFirst[1]), int.Parse(arrDatumFirst[0]));
            }

            if (bankaId != "")
            {
                bankaIdInt = System.Convert.ToInt32(bankaId);
            }

            var cekovi = BexUow.Cekovi.GetAll(true).Where(x => x.DatumDospeca == datum && x.BankaId == bankaIdInt && x.InternoRazduzen == false && x.Storno==false && x.BrojSpecifikacije==0);
            int poslednjibrojspecifikacije = BexUow.Cekovi.AllAsNoTracking.OrderByDescending(c => c.BrojSpecifikacije).Take(1).FirstOrDefault().BrojSpecifikacije;
            int sledecibrojspec = 0;
            int rbCeka = 0;

            foreach (var val in cekovi)
            {
                int cekId = val.Id;                
    
                if ((rbCeka % 13==0) || (rbCeka == 0))
                {
                    if (rbCeka==0)
                    {
                        sledecibrojspec = poslednjibrojspecifikacije + 1;
                    }
                    else
                    {
                        sledecibrojspec = sledecibrojspec + 1;
                    }
                   

                    var cek = BexUow.Cekovi.Find(x => x.Id == cekId);
                    cek.BrojSpecifikacije = sledecibrojspec;
                    BexUow.Cekovi.Update(cek);

                }
                else
                {
                    //postojeciBrojSpec= BexUow.Cekovi.AllAsNoTracking.Where(c => c.DatumDospeca==datum && c.BankaId==bankaIdInt).OrderByDescending(c => c.BrojSpecifikacije).Take(1).FirstOrDefault().BrojSpecifikacije;
                    var cek = BexUow.Cekovi.Find(x => x.Id == cekId);
                    cek.BrojSpecifikacije = sledecibrojspec;
                    BexUow.Cekovi.Update(cek);
                   
                }
                rbCeka = rbCeka + 1;

            }


            //Cekovi cekReturn = new Cekovi
            //{
            //    BankaId = bankaIdInt,
            //    DatumDospeca = datum
            //};

            var commandResult = BexUow.SubmitChanges();
            if (commandResult.IsSuccessful)
            {
                return RedirectToAction("KreiranjeSpecifikacija", "Cekovi", new { kreirajSpec = 1,  datumDospeca, bankaId  }); 
                //return View(cekReturn);               
            }

            ExceptionSolver.PrepareModelState(ModelState, commandResult);
            return RedirectToAction("KreiranjeSpecifikacija", "Cekovi", new { kreirajSpec = 1, datumDospeca, bankaId });
            //return View(cekReturn);
        }

        public ActionResult GetZastupnikeAutoComplete(string query)
        {
            return Json(_GetZastupnikeAutoComplete(query), JsonRequestBehavior.AllowGet);
        }
        private List<Autocomplete> _GetZastupnikeAutoComplete(string query)
        {
            List<Autocomplete> zastupnikList = new List<Autocomplete>();
            try
            {

                zastupnikList = BexUow.KontaktUloga.AllAsNoTracking.Where(x => x.Nadimak.Contains(query))
                                                .Select(a => new Autocomplete
                                                {
                                                    Id = a.Id,
                                                    Name = a.Nadimak
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
            return zastupnikList;
        }

        [HttpGet]
        public ActionResult GetCekoviStat()
        {
            DateTime proslaNedelja= DateTime.Today.AddDays(7);
            DateTime pre2Nedelje = DateTime.Today.AddDays(14);

            var monday = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Monday);
            var friday = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Friday);

            var monday1 = proslaNedelja.AddDays(-(int)proslaNedelja.DayOfWeek + (int)DayOfWeek.Monday);
            var friday1 = proslaNedelja.AddDays(-(int)proslaNedelja.DayOfWeek + (int)DayOfWeek.Friday);

            var monday2 = pre2Nedelje.AddDays(-(int)pre2Nedelje.DayOfWeek + (int)DayOfWeek.Monday);
            var friday2 = pre2Nedelje.AddDays(-(int)pre2Nedelje.DayOfWeek + (int)DayOfWeek.Friday);

            var cekovi = BexUow.Cekovi.AllAsNoTracking.Where(x => x.InternoRazduzen == false && x.DatumRealizacije == null && x.Storno==false);

            int ukupnoNerealizovanih = cekovi.Count();
            int ukupnoNerealizovanihIznos = cekovi.Sum(x => x.IznosCeka);

            int ukupnoNerealizovanihTekucaNedelja = cekovi.Where(x => (x.DatumDospeca >= monday && x.DatumDospeca <= friday)).Count();
            int ukupnoNerealizovanihIznosTekucaNedelja = cekovi.Where(x => (x.DatumDospeca >= monday && x.DatumDospeca <= friday)).Sum(x => x.IznosCeka);

            int ukupnoNerealizovanihProslaNedelja = cekovi.Where(x => (x.DatumDospeca >= monday1 && x.DatumDospeca <= friday1)).Count();
            int ukupnoNerealizovanihIznosProslaNedelja = cekovi.Where(x => (x.DatumDospeca >= monday1 && x.DatumDospeca <= friday1)).Sum(x => x.IznosCeka);

            int ukupnoNerealizovanihPre2Nedelje = cekovi.Where(x => (x.DatumDospeca >= monday2 && x.DatumDospeca <= friday2)).Count();
            int ukupnoNerealizovanihIznosPre2Nedelje = cekovi.Where(x => (x.DatumDospeca >= monday2 && x.DatumDospeca <= friday2)).Sum(x => x.IznosCeka);

            var cekoviData = new
            {
                broj = ukupnoNerealizovanih,
                iznos = ukupnoNerealizovanihIznos,
                brojTekuca=ukupnoNerealizovanihTekucaNedelja,
                iznosTekuca=ukupnoNerealizovanihIznosTekucaNedelja,
                brojProsla = ukupnoNerealizovanihProslaNedelja,
                iznosProsla = ukupnoNerealizovanihIznosProslaNedelja,
                brojPre2= ukupnoNerealizovanihPre2Nedelje,
                iznosPre2 = ukupnoNerealizovanihIznosPre2Nedelje
            };

            return Json(cekoviData, "", JsonRequestBehavior.AllowGet);

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
