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
    [ClaimsAuthentication(Resource = "PutniNalog", Operation = "Read, All")]
    public class PutniNalogController : Controller
    {
        public PutniNalogController() : this(new SecurityUow(System.Web.HttpContext.Current.GetOwinContext()), new BexUow())
        { }
        public PutniNalogController(
            ISecurityUow securityUow, IBexUow bexUow)
        {
            SecurityUow = securityUow;
            BexUow = bexUow;
        }
        public ActionResult Index(int izmena, int? filterId)
        {
            TempData.ToList().ForEach(keyValue =>
                ModelState.AddModelError("", keyValue.Value.ToString()));
            TempData.Clear();
            ViewBag.Izmena = izmena;
            //ViewBag.Region = new SelectList(BexUow.Region.AllAsNoTracking, "NazivSkraceni", "NazivSkraceni");
            //ViewBag.RadnoMesto = new SelectList(BexUow.ZaposleniRadnoMesto.AllAsNoTracking, "InterniNaziv", "InterniNaziv");
            //ViewBag.PutniNalogTip = new SelectList(BexUow.PutniNalogTip.AllAsNoTracking, "NazivTipa", "NazivTipa");
            ViewBag.FilterId = (filterId == null) ? 0 : filterId;

            return View();

        }



        [HttpGet]
        public async Task<ActionResult> GetPutniNalog(int pageSize, int pageNumber, string sortColumn, string sortOrder, string search, string searchColumn, string searchTerms)
        {
            List<string> listaRegiona = await getAllClaims();

            var skip = (pageNumber - 1) * pageSize;            

            if (!String.IsNullOrEmpty(searchTerms))
            {
                var putniNalogData = GetSearchPutniNalogData(searchTerms,listaRegiona).AsEnumerable();
            
            var total = putniNalogData.Count();

            if (sortColumn == "")
            {
                sortColumn = "DatumStart";
                sortOrder = "desc";
            }


            if (sortOrder.Equals("desc"))
                putniNalogData = putniNalogData.OrderByDescending(s => s.GetType().GetProperty(sortColumn).GetValue(s)).ThenByDescending(s => s.VremeStart).ToList().Skip(skip).Take(pageSize);
            else
                putniNalogData = putniNalogData.OrderBy(s => s.GetType().GetProperty((sortColumn == "") ? "Id" : sortColumn).GetValue(s)).ToList().Skip(skip).Take(pageSize);



            var jsonData = new TableJsonIndexData<PutniNalogIndexData>()
            {
                total = total,
                rows = putniNalogData
            };
            var jsonResult = Json(jsonData, "TableJsonIndexData", JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        return null;
            
        }

        public IEnumerable<PutniNalogIndexData> GetSearchPutniNalogData(string searchTerms,List<string> listaRegiona)
        {
            
            DateTime datum = DateTime.Now.AddMonths(-6);

            //var putniNalogDataSet = (from p in BexUow.PutniNalog.AllAsNoTracking.Where(x => x.DatumStart > datum).Where(r => listaRegiona.Contains(r.RegionId.ToString()))
            //                         let reon = (from z in BexUow.Reon.AllAsNoTracking
            //                                          where z.Id == p.ReonId
            //                                          select z).OrderByDescending(z => z.Id).Take(1).FirstOrDefault()

            //                         //join r in BexUow.Reon?.AllAsNoTracking on p.ReonId equals r.Id into reonPutni
            //                         //from r in reonPutni.DefaultIfEmpty()

            //                     select new PutniNalogIndexData
            //                     {
            //                         Id = p.Id,
            //                         VpId = p.VpId,
            //                         Vozilo = p.VozniPark.Oznaka,
            //                         Vozac = p.KontaktVozac.Kontakt.Naziv.ToString() ?? "",
            //                         RadnoMesto = p.RadnoMesto.InterniNaziv ?? "",
            //                         Tip = p.PutniNalogTip.NazivTipa.ToString(),
            //                         Reon = reon.OznReona ?? "",
            //                         Region = reon.Region.NazivSkraceni ?? "",
            //                         Relacija = p.Relacija ?? "",
            //                         DatumStart = p.DatumStart,
            //                         DatumStop = p.DatumStop,
            //                         KmStart = p.KmStart,
            //                         KmStop = p.KmStop,
            //                         KmUkupno = p.KmUkupno,
            //                         UserUneo = "",
            //                         Napomena = p.Napomena,
            //                         Storno = p.Storno,
            //                         Litraza = p.Litara,
            //                         BrojSipanja = p.BrojSipanja

            //                     });
            var putniNalogDataSet = BexUow.PutniNalog.AllAsNoTracking
                                                .Where(x => x.DatumStart > datum && x.Storno==false)
                                                .Select(x =>
                                new PutniNalogIndexData
                                {
                                    Id = x.Id,
                                    VpId = x.VpId,
                                    Vozilo = x.VozniPark.Oznaka,
                                    Vozac = x.KontaktVozac.Naziv.ToString() ?? "",
                                    RadnoMesto = x.RadnoMesto.InterniNaziv ?? "",
                                    Tip = x.PutniNalogTip.NazivTipa.ToString(),
                                    Reon = x.Reon.OznReona ?? "",
                                    Region = x.Region.NazivSkraceni ?? "",
                                    RegionId = x.RegionId,
                                    Relacija = x.Relacija ?? "",
                                    DatumStart = x.DatumStart,
                                    VremeStart=x.VremeStart,
                                    DatumStop = x.DatumStop,
                                    KmStart = x.KmStart,
                                    KmStop = x.KmStop,
                                    KmUkupno = x.KmUkupno,
                                    UserUneo = x.UserUneo.Kontakt.Naziv,
                                    Napomena = x.Napomena,
                                    Storno = x.Storno,
                                    Litraza = x.Litara,
                                    BrojSipanja = x.BrojSipanja,
                                    GarazniBroj = x.VozniPark.GarazniBroj

                                });

            int brojRegiona = BexUow.Region.AllAsNoTracking.Where(x => x.Storno==false).Select(x => x.Id).Count();

            if (listaRegiona.Count()!=brojRegiona)
            {
                putniNalogDataSet = putniNalogDataSet.Where(r => listaRegiona.Contains(r.RegionId.ToString()));
            }


           

            string[] terms = searchTerms.Split(',');

            foreach (string t in terms)
            {
                string[] searchCT = t.Split(':');

                string searchColumn = "";
                string searchTxt = "";

                searchColumn = searchCT[0];
                searchTxt = searchCT[1];

                if (searchColumn.Equals("Vozilo") && !String.IsNullOrEmpty(searchTxt))
                {
                    putniNalogDataSet = putniNalogDataSet.Where(k => k.Vozilo.ToString().ToUpper().Contains(searchTxt.ToUpper()));
                }
                else if (searchColumn.Equals("Vozac") && !String.IsNullOrEmpty(searchTxt))
                {
                    putniNalogDataSet = putniNalogDataSet.Where(k => k.Vozac.ToString().ToUpper().Contains(searchTxt.ToUpper()));
                }
                else if (searchColumn.Equals("RadnoMesto") && !String.IsNullOrEmpty(searchTxt))
                {
                    putniNalogDataSet = putniNalogDataSet.Where(k => k.RadnoMesto.ToUpper().Contains(searchTxt.ToUpper()));
                }
                else if (searchColumn.Equals("Tip") && !String.IsNullOrEmpty(searchTxt))
                {
                    putniNalogDataSet = putniNalogDataSet.Where(k => k.Tip.ToUpper().Contains(searchTxt.ToUpper()));
                }
                else if (searchColumn.Equals("Reon") && !String.IsNullOrEmpty(searchTxt))
                {
                    putniNalogDataSet = putniNalogDataSet.Where(k => k.Reon.ToUpper().Contains(searchTxt.ToUpper()));
                }
                else if (searchColumn.Equals("Region") && !String.IsNullOrEmpty(searchTxt))
                {
                    putniNalogDataSet = putniNalogDataSet.Where(k => k.Region.ToUpper().Contains(searchTxt.ToUpper()));
                }
                else if (searchColumn.Equals("Relacija") && !String.IsNullOrEmpty(searchTxt))
                {
                    putniNalogDataSet = putniNalogDataSet.Where(k => k.Relacija.ToUpper().Contains(searchTxt.ToUpper()));
                }
                else if (searchColumn.Equals("DatumStart") && !String.IsNullOrEmpty(searchTxt))
                {
                    //string[] arrDatum = searchTxt.Split('/');
                    //DateTime thisDate1 = new DateTime(int.Parse(arrDatum[2]), int.Parse(arrDatum[0]), int.Parse(arrDatum[1]));
                    DateTime datumFirst, datumSecond;
                    string[] arrDatumAll = searchTxt.Split('t', 'o');

                    string[] arrDatumFirst = arrDatumAll[0].Trim().Split('-');
                    datumFirst = new DateTime(int.Parse(arrDatumFirst[0]), int.Parse(arrDatumFirst[1]), int.Parse(arrDatumFirst[2]));

                    string[] arrDatumSecond = arrDatumAll[2].Trim().Split('-');
                    datumSecond = new DateTime(int.Parse(arrDatumSecond[0]), int.Parse(arrDatumSecond[1]), int.Parse(arrDatumSecond[2]));

                    putniNalogDataSet = putniNalogDataSet.Where(k => k.DatumStart >= datumFirst && k.DatumStart <= datumSecond);
                }
                else if (searchColumn.Equals("DatumStop") && !String.IsNullOrEmpty(searchTxt))
                {
                    //string[] arrDatum = searchTxt.Split('/');
                    //DateTime thisDate1 = new DateTime(int.Parse(arrDatum[2]), int.Parse(arrDatum[0]), int.Parse(arrDatum[1]));

                    DateTime datumFirst, datumSecond;
                    string[] arrDatumAll = searchTxt.Split('t', 'o');

                    string[] arrDatumFirst = arrDatumAll[0].Trim().Split('-');
                    datumFirst = new DateTime(int.Parse(arrDatumFirst[0]), int.Parse(arrDatumFirst[1]), int.Parse(arrDatumFirst[2]));

                    string[] arrDatumSecond = arrDatumAll[2].Trim().Split('-');
                    datumSecond = new DateTime(int.Parse(arrDatumSecond[0]), int.Parse(arrDatumSecond[1]), int.Parse(arrDatumSecond[2]));

                    putniNalogDataSet = putniNalogDataSet.Where(k => k.DatumStop >= datumFirst && k.DatumStop <= datumSecond);
                }
                else if (searchColumn.Equals("KmStart") && !String.IsNullOrEmpty(searchTxt))
                {
                    int kmStartInt = System.Convert.ToInt32(searchTxt);
                    putniNalogDataSet = putniNalogDataSet.Where(k => k.KmStart.Equals(kmStartInt));
                }
                else if (searchColumn.Equals("KmStop") && !String.IsNullOrEmpty(searchTxt))
                {
                    int kmStopInt = System.Convert.ToInt32(searchTxt);
                    putniNalogDataSet = putniNalogDataSet.Where(k => k.KmStop.Equals(kmStopInt));
                }
                else if (searchColumn.Equals("KmUkupno") && !String.IsNullOrEmpty(searchTxt))
                {
                    int KmUkupnoInt = System.Convert.ToInt32(searchTxt);
                    putniNalogDataSet = putniNalogDataSet.Where(k => k.KmUkupno.Equals(KmUkupnoInt));
                }
                else if (searchColumn.Equals("UserUneo") && !String.IsNullOrEmpty(searchTxt))
                {
                    putniNalogDataSet = putniNalogDataSet.Where(k => k.UserUneo.ToUpper().Contains(searchTxt.ToUpper()));
                }
                else if (searchColumn.Equals("Napomena") && !String.IsNullOrEmpty(searchTxt))
                {
                    putniNalogDataSet = putniNalogDataSet.Where(k => k.Napomena.ToUpper().Contains(searchTxt.ToUpper()));
                }
                else if (searchColumn.Equals("Storno") && !String.IsNullOrEmpty(searchTxt))
                {
                    bool storno = System.Convert.ToBoolean(searchTxt);
                    putniNalogDataSet = putniNalogDataSet.Where(k => k.Storno == storno);
                }
                else if (searchColumn.Equals("GarazniBroj") && !String.IsNullOrEmpty(searchTxt))
                {
                    putniNalogDataSet = putniNalogDataSet.Where(k => k.GarazniBroj.ToUpper().Contains(searchTxt.ToUpper()));
                }

            }
            return putniNalogDataSet;
        }

        [ClaimsAuthentication(Resource = "PutniNalog", Operation = "Create, All")]
        public async Task<ActionResult> Create(int? id)
        {
            List<string> listaRegiona = await getAllClaims();
            PutniNalog putniNalog = BexUow.PutniNalog.Find(id);
            if (putniNalog == null)
            {
                
                ViewBag.VpId = new SelectList(BexUow.VozniPark.AllAsNoTracking.Where(r => listaRegiona.Contains(r.RegionId.ToString())), "Id", "NazivVozila");
                ViewBag.TipId = new SelectList(BexUow.PutniNalogTip.AllAsNoTracking, "Id", "NazivTipa",2);
                ViewBag.RegionId = new SelectList(BexUow.Region.AllAsNoTracking.Where(r => listaRegiona.Contains(r.Id.ToString())), "Id", "NazivSkraceni");
                ViewBag.ReonId = new SelectList(BexUow.Reon.AllAsNoTracking.Where(r => listaRegiona.Contains(r.RegionId.ToString())), "Id", "OznReona");

                return View();
            }
            else
            {
                //var vozac = BexUow.KorisniciPrograma.Find(putniNalog.VozacUserId);
                //ViewBag.Vozac = BexUow.Kontakts.Find(vozac.KontaktId).Naziv;
                ViewBag.Vozac = BexUow.Kontakts.Find(putniNalog.VozacId).Naziv;
                ViewBag.VpId = new SelectList(BexUow.VozniPark.AllAsNoTracking.Where(r => listaRegiona.Contains(r.RegionId.ToString())), "Id", "NazivVozila", putniNalog.VpId);
                ViewBag.TipId = new SelectList(BexUow.PutniNalogTip.AllAsNoTracking, "Id", "NazivTipa", putniNalog.TipId);
                ViewBag.RegionId = new SelectList(BexUow.Region.AllAsNoTracking.Where(r => listaRegiona.Contains(r.Id.ToString())), "Id", "NazivSkraceni", putniNalog.RegionId);
                ViewBag.ReonId = new SelectList(BexUow.Reon.AllAsNoTracking.Where(r => listaRegiona.Contains(r.RegionId.ToString())), "Id", "OznReona", putniNalog.ReonId);
                putniNalog.DatumStart = DateTime.Now.Date;
                putniNalog.VremeStart = DateTime.Now.TimeOfDay;
                putniNalog.KmStart = BexUow.PutniNalog.AllAsNoTracking.Where(x => x.VpId==putniNalog.VpId).Max(x => x.KmStop);
                putniNalog.DatumStop = DateTime.Now.Date;
                putniNalog.VremeStop = DateTime.Now.TimeOfDay;
                putniNalog.KmStop = 0;
                putniNalog.KmUkupno = putniNalog.KmStart;
                return View(putniNalog);
            }  
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(PutniNalog putniNalog)
        {
            if (ModelState.IsValid)
            {
                putniNalog.ZatvorenPutniNalog = (putniNalog.KmStart < putniNalog.KmStop) ? true : false;
                var applicationUser = await SecurityUow.UserManager.FindUserByIdAsync(User.Identity.GetUserId()) as ApplicationUser;
                var korisnikProgramaId = BexUow.KorisniciPrograma.Find(x => x.AspNetUserId == applicationUser.Id).Id;
                putniNalog.UserUnosId = korisnikProgramaId;

                BexUow.PutniNalog.Add(putniNalog);
                var commandResult = BexUow.SubmitChanges();

                if (commandResult.IsSuccessful)
                { return RedirectToAction("Index", "PutniNalog",new { izmena = 0 }); }

                ExceptionSolver.PrepareModelState(ModelState, commandResult);
            }


            return View();

        }

        [ClaimsAuthentication(Resource = "PutniNalog", Operation = "Edit, All")]
        public async Task<ActionResult> Edit(int id)
        {
            DateTime datumJuce = DateTime.Now.Date.AddDays(-Convert.ToDouble(1));
            var applicationUser = await SecurityUow.UserManager.FindUserByIdAsync(User.Identity.GetUserId()) as ApplicationUser;
            var ulogaId = BexUow.KorisniciPrograma.Find(x => x.AspNetUserId == applicationUser.Id).RoleId;
            List<string> listaRegiona = await getAllClaims();
            PutniNalog putniNalog = BexUow.PutniNalog.Find(id);
            if (putniNalog == null)
            {
                return HttpNotFound();
            }
            if (putniNalog.DatumStart<datumJuce && (ulogaId=="5" || ulogaId=="1"))//rukovodilac i magacioner
            {
                return RedirectToAction("Index", "PutniNalog", new { izmena = 1 }); 
            }
            //var vozac = BexUow.KorisniciPrograma.Find(putniNalog.VozacUserId);
            //ViewBag.Vozac = BexUow.Kontakts.Find(vozac.KontaktId).Naziv;
            ViewBag.Vozac = (putniNalog.VozacId == 0) ? "" : BexUow.Kontakts.Find(putniNalog.VozacId).Naziv;

            ViewBag.VpId = new SelectList(BexUow.VozniPark.AllAsNoTracking.Where(r => listaRegiona.Contains(r.RegionId.ToString())), "Id", "NazivVozila", putniNalog.VpId);
            ViewBag.TipId = new SelectList(BexUow.PutniNalogTip.AllAsNoTracking, "Id", "NazivTipa", putniNalog.TipId);
            ViewBag.RegionId = new SelectList(BexUow.Region.AllAsNoTracking.Where(r => listaRegiona.Contains(r.Id.ToString())), "Id", "NazivSkraceni", putniNalog.RegionId);
            if (putniNalog.ReonId == null)
            {
                ViewBag.ReonId = new SelectList(BexUow.Reon.AllAsNoTracking.Where(r => listaRegiona.Contains(r.RegionId.ToString())), "Id", "OznReona");
            }
            else
            {
                ViewBag.ReonId = new SelectList(BexUow.Reon.AllAsNoTracking.Where(r => listaRegiona.Contains(r.RegionId.ToString())), "Id", "OznReona", putniNalog.ReonId);
            }

            
            return View(putniNalog);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(PutniNalog putniNalog)
        {

            if (ModelState.IsValid)
            {

                putniNalog.ZatvorenPutniNalog=(putniNalog.KmStart < putniNalog.KmStop) ? true :  false;
                var applicationUser = await SecurityUow.UserManager.FindUserByIdAsync(User.Identity.GetUserId()) as ApplicationUser;
                var korisnikProgramaId = BexUow.KorisniciPrograma.Find(x => x.AspNetUserId == applicationUser.Id).Id;
                putniNalog.UserUnosId = korisnikProgramaId;
                putniNalog.KmUkupno = putniNalog.KmStop - putniNalog.KmStart;

                BexUow.PutniNalog.Update(putniNalog);
                var uowCommandResult = BexUow.SubmitChanges();

                if (uowCommandResult.IsSuccessful)
                { return RedirectToAction("Index", "PutniNalog",new { izmena = 0 }); }

                ExceptionSolver.PrepareModelState(ModelState, uowCommandResult);
            }

            return View(putniNalog);
        }

        [ClaimsAuthentication(Resource = "PutniNalog", Operation = "Delete, All")]
        public async Task<ActionResult> Delete(int id)
        {
            ViewBag.NijeDozvoljenoBrisanje = 0;
            DateTime datumJuce = DateTime.Now.Date.AddDays(-Convert.ToDouble(1));
            var applicationUser = await SecurityUow.UserManager.FindUserByIdAsync(User.Identity.GetUserId()) as ApplicationUser;
            var ulogaId = BexUow.KorisniciPrograma.Find(x => x.AspNetUserId == applicationUser.Id).RoleId;
            List<string> listaRegiona = await getAllClaims();
            PutniNalog putniNalog = BexUow.PutniNalog.Find(id);
            if (putniNalog == null)
            {
                return HttpNotFound();
            }
            if (putniNalog.DatumStart < datumJuce && (ulogaId == "5" || ulogaId == "1"))//rukovodilac i magacioner
            {
                ViewBag.NijeDozvoljenoBrisanje = 1;
            }
            return View(putniNalog);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(PutniNalog putniNalog)
        {
            var putniNalogData = BexUow.PutniNalog.Find(putniNalog.Id);
            putniNalogData.Storno = true;
            BexUow.PutniNalog.Update(putniNalogData);
            var commandResult = BexUow.SubmitChanges();

            if (commandResult.IsSuccessful)
            { return RedirectToAction("Index", "PutniNalog", new { izmena = 0 }); }

            ExceptionSolver.PrepareModelState(ModelState, commandResult);
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
