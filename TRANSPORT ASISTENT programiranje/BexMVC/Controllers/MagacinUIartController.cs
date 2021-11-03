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
    [ClaimsAuthentication(Resource = "MagacinUIart", Operation = "Read, All")]
    public class MagacinUIartController : Controller
    {
        public MagacinUIartController() : this(new SecurityUow(System.Web.HttpContext.Current.GetOwinContext()), new BexUow())
        { }
        public MagacinUIartController(
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
            ViewBag.Id=0;
            return View();
        }



        [HttpGet]
        public ActionResult GetMagacinUIart(string pageSize, int pageNumber, string sortColumn, string sortOrder, string search, string searchColumn, string searchTerms)
        {

            var artData = (from a in BexUow.VozniParkDnevnik.GetAll(true).Where(x => (x.VozniParkDnevnikTip.GrupaId==91 || x.VozniParkDnevnikTip.GrupaId==31) && x.ArtId>0)//samo trebovanja
                           let lager = (from l in BexUow.Lager.AllAsNoTracking
                                        where l.ArtId == a.ArtId
                                        select l).FirstOrDefault()
                                    select new MagacinUIartIndexData
                                    {
                                        Id = a.Id,
                                        TipPromene=a.VozniParkDnevnikTip.NazivTipa,
                                        Magacin=a.MagacinSpisak.Naziv,
                                        Sifra = a.Artikli.Sifra ?? "",
                                        Grupa = a.Artikli.ArtikliGrupa.Naziv ?? "",
                                        Opis = a.Artikli.Opis ?? "",
                                        Kolicina = lager.Kolicina,
                                        Datum = a.Datum,
                                        Nav = a.NavOK
                                    }).AsEnumerable();


            if (!String.IsNullOrEmpty(searchTerms))
            {
                artData = GetSearchMagacinUIartData(searchTerms, artData).AsEnumerable();
            }

            var total = artData.Count();

            int pageSizeInt = (pageSize != "Sve") ? System.Convert.ToInt32(pageSize) : total;

            var skip = (pageNumber - 1) * pageSizeInt;

            if (sortOrder.Equals("desc"))
                artData = artData.OrderByDescending(s => s.GetType().GetProperty(sortColumn).GetValue(s)).ToList().Skip(skip).Take(pageSizeInt);
            else
                artData = artData.OrderBy(s => s.GetType().GetProperty((sortColumn == "") ? "Id" : sortColumn).GetValue(s)).ToList().Skip(skip).Take(pageSizeInt);


            var jsonData = new TableJsonIndexData<MagacinUIartIndexData>()
            {
                total = total,
                rows = artData
            };
            var jsonResult = Json(jsonData, "TableJsonIndexData", JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        public IEnumerable<MagacinUIartIndexData> GetSearchMagacinUIartData(string searchTerms, IEnumerable<MagacinUIartIndexData> artData)
        {
            string[] terms = searchTerms.Split(',');

            foreach (string t in terms)
            {
                string[] searchCT = t.Split(':');

                string searchColumn = "";
                string searchTxt = "";

                searchColumn = searchCT[0];
                searchTxt = searchCT[1];


                if (searchColumn.Equals("TipPromene") && !String.IsNullOrEmpty(searchTxt))
                {
                    artData = artData.Where(k => k.TipPromene.Equals(searchTxt));
                }
                else if (searchColumn.Equals("Magacin") && !String.IsNullOrEmpty(searchTxt))
                {
                    artData = artData.Where(k => k.Magacin.ToUpper().Contains(searchTxt.ToUpper()));
                }
                else if (searchColumn.Equals("Šifra") && !String.IsNullOrEmpty(searchTxt))
                {
                    artData = artData.Where(k => k.Sifra.ToUpper().Contains(searchTxt.ToUpper()));
                }
                else if (searchColumn.Equals("Grupa") && !String.IsNullOrEmpty(searchTxt))
                {
                    artData = artData.Where(k => k.Grupa.ToUpper().Contains(searchTxt.ToUpper()));
                }
                else if (searchColumn.Equals("Opis") && !String.IsNullOrEmpty(searchTxt))
                {
                    artData = artData.Where(k => k.Opis.ToUpper().Contains(searchTxt.ToUpper()));
                }
                else if (searchColumn.Equals("Kolicina") && !String.IsNullOrEmpty(searchTxt))
                {
                    decimal Kolicina = System.Convert.ToInt32(searchTxt);
                    artData = artData.Where(k => k.Kolicina.Equals(Kolicina));
                }
                else if (searchColumn.Equals("Datum") && !String.IsNullOrEmpty(searchTxt))
                {
                    //searchColumnDatumRegistracije = searchTxt;
                    string[] arrDatum = searchTxt.Split('/');
                    DateTime thisDate1 = new DateTime(int.Parse(arrDatum[2]), int.Parse(arrDatum[0]), int.Parse(arrDatum[1]));
                    artData = artData.Where(k => k.Datum.Equals(thisDate1));
                }
                else if (searchColumn.Equals("NavOK") && !String.IsNullOrEmpty(searchTxt))
                {
                    int navInt = System.Convert.ToInt32(searchTxt);
                    artData = artData.Where(k => k.Nav.Equals(navInt));
                }                
            }
            return artData;
        }


        [ValidateAntiForgeryToken]
        [ClaimsAuthentication(Resource = "MagacinUIart", Operation = "Nav, All")]
        public JsonResult PosaljiUnav(SelectedValue values)
        {
            foreach (var val in values.SelectedValues)
            {
                int idPromene = val.Id;
                var promena = BexUow.VozniParkDnevnik.Find(idPromene);
                var dim = new
                {
                    PostingData = promena.Datum,
                    ItemNo = promena.Artikli.Sifra ?? "",
                    MaterialType = promena.Artikli.ArtikliVrsta.NazivVrsteNav ?? "",
                    LocationCode = "SA-" + promena.Artikli.ArtikliVrsta.OznakaVrste ?? "",
                    Quantity = promena.Kolicina,
                    ProfitCentar = ""
                };
            }
            return Json(new { success = "true" });
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
