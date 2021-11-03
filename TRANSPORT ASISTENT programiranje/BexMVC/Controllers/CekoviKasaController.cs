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
    [ClaimsAuthentication(Resource = "CekoviKasa", Operation = "Read, All")]
    public class CekoviKasaController : Controller
    {
        public CekoviKasaController() : this(new SecurityUow(System.Web.HttpContext.Current.GetOwinContext()), new BexUow())
        { }
        public CekoviKasaController(
            ISecurityUow securityUow, IBexUow bexUow)
        {
            SecurityUow = securityUow;
            BexUow = bexUow;
        }
        public ActionResult Index()
        {
            TempData.ToList().ForEach(keyValue =>
                ModelState.AddModelError("", keyValue.Value.ToString()));
            TempData.Clear();

            ViewBag.BankaNaziv = new SelectList(BexUow.Banke.AllAsNoTracking, "NazivBanke", "NazivBanke");
            ViewBag.NerealizovaniCekoviSuma=BexUow.Cekovi.AllAsNoTracking.Where(x => x.DatumRealizacije == null && x.Storno==false && x.InternoRazduzen==false).Sum(x =>x.IznosCeka);

            var monday = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Monday);
            var friday = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Friday);

            ViewBag.OveNedeljeDospeva= BexUow.Cekovi.AllAsNoTracking.Where(x => (x.DatumDospeca >= monday && x.DatumDospeca <= friday) && x.Storno == false && x.InternoRazduzen == false).Sum(x => x.IznosCeka);

            return View();
        }



        [HttpGet]
        public ActionResult GetKasaCekovi(string pageSize, int pageNumber, string sortColumn, string sortOrder, string search, string searchColumn, string searchTerms)
        {

            //var skip = (pageNumber - 1) * pageSize;

            var cekoviKasaData = BexUow.Cekovi.AllAsNoTracking.Where(x => x.Storno==false)
                                 .GroupBy(x => new
                                 {
                                     x.Banka.NazivBanke,
                                     x.BankaId,
                                     x.DatumDospeca,
                                     x.DatumRealizacije
                                 })
                                 .Select (g => new CekoviIndexData()
                                 {
                                    Id=g.Max(x => x.Id),
                                    DatumDospeca=g.Key.DatumDospeca,
                                    BankaNaziv=g.Key.NazivBanke,
                                    BankaId=g.Key.BankaId,
                                    DatumRealizacije=g.Key.DatumRealizacije,
                                    Iznos=g.Sum(x => x.IznosCeka),
                                    Realizovano= g.Sum(x => (x.DatumRealizacije!=null && x.InternoRazduzen==false)  ? x.IznosCeka : 0),
                                    BrojCekova=g.Count()
                                 }).AsEnumerable();

            ViewBag.NerealizovaniCekoviSuma = cekoviKasaData.Where(x => x.DatumRealizacije==null).Sum(x => x.Iznos);
            if (!String.IsNullOrEmpty(searchTerms))
            {
                cekoviKasaData = GetSearchCekoviKasaData(searchTerms, cekoviKasaData).AsEnumerable();
            }

            var total = cekoviKasaData.Count();

            int pageSizeInt = (pageSize != "Sve") ? System.Convert.ToInt32(pageSize) : total;

            var skip = (pageNumber - 1) * pageSizeInt;

            if (sortOrder.Equals("desc"))
                cekoviKasaData = cekoviKasaData.OrderByDescending(s => s.GetType().GetProperty(sortColumn).GetValue(s)).ToList().Skip(skip).Take(pageSizeInt);
            else
                cekoviKasaData = cekoviKasaData.OrderBy(s => s.GetType().GetProperty((sortColumn == "") ? "Id" : sortColumn).GetValue(s)).ToList().Skip(skip).Take(pageSizeInt);


            var jsonData = new TableJsonIndexData<CekoviIndexData>()
            {
                total = total,
                rows = cekoviKasaData
            };
            var jsonResult = Json(jsonData, "TableJsonIndexData", JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;

        }

        public IEnumerable<CekoviIndexData> GetSearchCekoviKasaData(string searchTerms, IEnumerable<CekoviIndexData> cekoviData)
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
                else if (searchColumn.Equals("DatumRealizacije") && !String.IsNullOrEmpty(searchTxt))
                {
                    DateTime datumFirst, datumSecond;
                    string[] arrDatumAll = searchTxt.Split('t', 'o');

                    string[] arrDatumFirst = arrDatumAll[0].Trim().Split('-');
                    datumFirst = new DateTime(int.Parse(arrDatumFirst[0]), int.Parse(arrDatumFirst[1]), int.Parse(arrDatumFirst[2]));

                    string[] arrDatumSecond = arrDatumAll[2].Trim().Split('-');
                    datumSecond = new DateTime(int.Parse(arrDatumSecond[0]), int.Parse(arrDatumSecond[1]), int.Parse(arrDatumSecond[2]));

                    cekoviData = cekoviData.Where(k => k.DatumRealizacije >= datumFirst && k.DatumDospeca <= datumSecond);
                }
                else if (searchColumn.Equals("Iznos") && !String.IsNullOrEmpty(searchTxt))
                {
                    int iznosInt = System.Convert.ToInt32(searchTxt);
                    cekoviData = cekoviData.Where(k => k.Iznos.Equals(iznosInt));
                }
                else if (searchColumn.Equals("BankaNaziv") && !String.IsNullOrEmpty(searchTxt))
                {
                    cekoviData = cekoviData.Where(k => k.BankaNaziv.ToUpper().Contains(searchTxt.ToUpper()));
                }
            }
            return cekoviData;
        }        

        public JsonResult RealizujPoSpecifikaciji(SelectedValue values)
        {
            foreach (var val in values.SelectedValues)
            {
                int cekId = val.Id;
                var cek = BexUow.Cekovi.Find(cekId);
                DateTime datumDospeca = cek.DatumDospeca;
                string bankaId = cek.BankaId.ToString();
                var cekovi = BexUow.Cekovi.GetAll(true).Where(x => x.DatumDospeca == datumDospeca && x.BankaId == System.Convert.ToInt32(bankaId) && x.InternoRazduzen == false && x.Storno == false && x.BrojSpecifikacije > 0);
                foreach (var val1 in cekovi)
                {
                    int cekId1 = val.Id;
                    var cek1 = BexUow.Cekovi.Find(cekId1);
                    cek1.DatumRealizacije = DateTime.Now;
                    BexUow.Cekovi.Update(cek1);
                    var uowCommandResult = BexUow.SubmitChanges();
                }
            }
            return Json(new { success = "true" });
        }

        public JsonResult KreirajSpecifikacije(SelectedValue values)
        {
            foreach (var val in values.SelectedValues)
            {
                int cekId = val.Id;
                var cek = BexUow.Cekovi.Find(cekId);
                DateTime datumDospeca = cek.DatumDospeca;
                string bankaId = cek.BankaId.ToString();

                AddSpecifikacija(datumDospeca, bankaId);
            }
            return Json(new { success = "true" });
        }

        public ActionResult AddSpecifikacija(DateTime datumDospeca, string bankaId)
        {
            DateTime datum = DateTime.Now.Date;
            int bankaIdInt = 0;

            if (bankaId != "")
            {
                bankaIdInt = System.Convert.ToInt32(bankaId);
            }

            var cekovi = BexUow.Cekovi.GetAll(true).Where(x => x.DatumDospeca == datumDospeca && x.BankaId == bankaIdInt && x.InternoRazduzen == false && x.Storno == false && x.BrojSpecifikacije == 0);
            int poslednjibrojspecifikacije = BexUow.Cekovi.AllAsNoTracking.OrderByDescending(c => c.BrojSpecifikacije).Take(1).FirstOrDefault().BrojSpecifikacije;
            int sledecibrojspec = 0;
            int rbCeka = 0;

            foreach (var val in cekovi)
            {
                int cekId = val.Id;

                if ((rbCeka % 13 == 0) || (rbCeka == 0))
                {
                    if (rbCeka == 0)
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

            var commandResult = BexUow.SubmitChanges();
            if (commandResult.IsSuccessful)
            {
                return RedirectToAction("KreiranjeSpecifikacija", "CekoviKasa", new { kreirajSpec = 1, datumDospeca, bankaId });
                //return View(cekReturn);               
            }

            ExceptionSolver.PrepareModelState(ModelState, commandResult);
            return RedirectToAction("KreiranjeSpecifikacija", "CekoviKasa", new { kreirajSpec = 1, datumDospeca, bankaId });
            //return View(cekReturn);
        }

        public ActionResult DetailsCekovi(string bankaId, string datumDospeca)
        {
            if (bankaId == null || datumDospeca==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.bankaId = bankaId;
            ViewBag.datumDospeca = datumDospeca;

            return View();
        }

        [HttpGet]
        public ActionResult GetCekoviData(string bankaId, string datumDospeca)
        {
            DateTime datum = DateTime.Now.Date;
            int bankaIdInt = 0;
            if (datumDospeca != "")
            {
                string[] arrDatumFirst = datumDospeca.Split('/');
                datum = new DateTime(int.Parse(arrDatumFirst[2]), int.Parse(arrDatumFirst[1]), int.Parse(arrDatumFirst[0]));
            }
            if (bankaId != "")
            {
                bankaIdInt = System.Convert.ToInt32(bankaId);
            }
            var cekoviData = BexUow.Cekovi.GetAll(true).Where(x => x.DatumDospeca == datum && x.BankaId == bankaIdInt).Select(x =>
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
                                                             DatumRealizacije = x.DatumRealizacije,
                                                             ZastupnikNaziv = x.KontaktUloga.Nadimak.ToString(),
                                                             ProvizijaIznos = x.CekProvizija.Iznos,
                                                             Napomena = x.Napomena ?? "",
                                                             Storno = x.Storno

                                                         }).AsEnumerable();

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
