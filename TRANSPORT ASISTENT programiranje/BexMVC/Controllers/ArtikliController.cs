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
    [ClaimsAuthentication(Resource = "Artikli", Operation = "Read, All")]
    public class ArtikliController : Controller
    {
        public ArtikliController() : this(new SecurityUow(System.Web.HttpContext.Current.GetOwinContext()), new BexUow())
        { }
        public ArtikliController(
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
        public ActionResult GetArtikli(string pageSize, int pageNumber, string sortColumn, string sortOrder, string search, string searchColumn, string searchTerms)
        {

            var artikliData = (from a in BexUow.Artikli.GetAll(true).Where(x => x.ArtGrupaId==36)//samo trebovanja
                               let lager = (from l in BexUow.Lager.AllAsNoTracking
                                          where l.ArtId == a.Id
                                          select l).FirstOrDefault()
                               select new ArtikliIndexData
                                    {
                                        Id = a.Id,
                                        Sifra = a.Sifra ?? "",
                                        Grupa = a.ArtikliGrupa.Naziv ?? "",
                                        Opis = a.Opis ?? "",
                                        Kolicina = lager.Kolicina,
                                        Napomena = a.Napomena ?? "",
                                        Nav = a.NavOk
                                    }).AsEnumerable();


            if (!String.IsNullOrEmpty(searchTerms))
            {
                artikliData = GetSearchArtikliData(searchTerms, artikliData).AsEnumerable();
            }

            var total = artikliData.Count();

            int pageSizeInt = (pageSize != "Sve") ? System.Convert.ToInt32(pageSize) : total;

            var skip = (pageNumber - 1) * pageSizeInt;

            if (sortOrder.Equals("desc"))
                artikliData = artikliData.OrderByDescending(s => s.GetType().GetProperty(sortColumn).GetValue(s)).ToList().Skip(skip).Take(pageSizeInt);
            else
                artikliData = artikliData.OrderBy(s => s.GetType().GetProperty((sortColumn == "") ? "Id" : sortColumn).GetValue(s)).ToList().Skip(skip).Take(pageSizeInt);


            var jsonData = new TableJsonIndexData<ArtikliIndexData>()
            {
                total = total,
                rows = artikliData
            };
            var jsonResult = Json(jsonData, "TableJsonIndexData", JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        public IEnumerable<ArtikliIndexData> GetSearchArtikliData(string searchTerms, IEnumerable<ArtikliIndexData> artikliData)
        {
            string[] terms = searchTerms.Split(',');

            foreach (string t in terms)
            {
                string[] searchCT = t.Split(':');

                string searchColumn = "";
                string searchTxt = "";

                searchColumn = searchCT[0];
                searchTxt = searchCT[1];


                if (searchColumn.Equals("Broj") && !String.IsNullOrEmpty(searchTxt))
                {
                    int idInt = System.Convert.ToInt32(searchTxt);
                    artikliData = artikliData.Where(k => k.Id.Equals(idInt));
                }
                else if (searchColumn.Equals("Šifra") && !String.IsNullOrEmpty(searchTxt))
                {
                    artikliData = artikliData.Where(k => k.Sifra.ToUpper().Contains(searchTxt.ToUpper()));
                }
                else if (searchColumn.Equals("Grupa") && !String.IsNullOrEmpty(searchTxt))
                {
                    artikliData = artikliData.Where(k => k.Grupa.ToUpper().Contains(searchTxt.ToUpper()));
                }
                else if (searchColumn.Equals("Opis") && !String.IsNullOrEmpty(searchTxt))
                {
                    artikliData = artikliData.Where(k => k.Opis.ToUpper().Contains(searchTxt.ToUpper()));
                }
                else if (searchColumn.Equals("Kolicina") && !String.IsNullOrEmpty(searchTxt))
                {
                    decimal Kolicina = System.Convert.ToInt32(searchTxt);
                    artikliData = artikliData.Where(k => k.Kolicina.Equals(Kolicina));
                }
                else if (searchColumn.Equals("Napomena") && !String.IsNullOrEmpty(searchTxt))
                {
                    artikliData = artikliData.Where(k => k.Napomena.ToUpper().Contains(searchTxt.ToUpper()));
                }
                else if (searchColumn.Equals("Nav") && !String.IsNullOrEmpty(searchTxt))
                {
                    int navInt = System.Convert.ToInt32(searchTxt);
                    artikliData = artikliData.Where(k => k.Nav.Equals(navInt));
                }                
            }
            return artikliData;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ClaimsAuthentication(Resource = "Artikli", Operation = "LagerU/I, All")]
        public async Task<ActionResult> SaveUlazIzlazLager(int artId, int kolicina)
        {
            var applicationUser = await SecurityUow.UserManager.FindUserByIdAsync(User.Identity.GetUserId()) as ApplicationUser;
            var korisnikProgramaId = BexUow.KorisniciPrograma.Find(x => x.AspNetUserId == applicationUser.Id).Id;

            int tipId = (kolicina < 0) ? 40 : 39;

            if (ModelState.IsValid)
            {
                var vpDnevnik = new VozniParkDnevnik
                {
                    ArtId = artId,
                    UserUnosId = korisnikProgramaId,
                    Kolicina = kolicina,
                    MagacinId = 50,
                    DnevnikTipId = tipId
                };

                BexUow.VozniParkDnevnik.Add(vpDnevnik);

                var lager = BexUow.Lager.Find(x => x.ArtId == artId && x.MagacinId == 50);

                if (lager!=null)//postoji lager za taj artikal u magacinu trebovanja
                {
                    lager.Kolicina = lager.Kolicina + kolicina;
                    lager.DatumIzmene = DateTime.Now.Date;
                    lager.VremeIzmene = DateTime.Now.TimeOfDay;

                    BexUow.Lager.Update(lager);
                }
                else
                {
                    lager = new Lager
                    {
                        MagacinId = 50,
                        ArtId = artId,
                        Kolicina = kolicina,
                        DatumIzmene = DateTime.Now.Date,
                        VremeIzmene = DateTime.Now.TimeOfDay
                    };

                    BexUow.Lager.Add(lager);
                }

                var commandResult = BexUow.SubmitChanges();
                
                if (commandResult.IsSuccessful)
                {
                    return RedirectToAction("Index");
                }
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
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
