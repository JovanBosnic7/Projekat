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
using BexMVC.Filters;

namespace BexMVC.Controllers
{
    [Authorize]
    [ClaimsAuthentication(Resource = "Reon", Operation = "Read, All")]
    public class ReonController : Controller
    {
        public ReonController() : this(new BexUow(), new ExceptionSolver())
        { }
        public ReonController(
            IBexUow bexUow,
            IExceptionSolver exceptionSolver)
        {
            BexUow = bexUow;
            ExceptionSolver = exceptionSolver;
        }
        public ActionResult Index(int? id, int? page, string searchReon, string searchReonNaziv, string searchRegion, string searchTip, string searchBarKod, string searchKilometraza, string searchOptimalna, string searchStorniran)
        {
            TempData.ToList().ForEach(keyValue =>
                ModelState.AddModelError("", keyValue.Value.ToString()));
            TempData.Clear();
            return View();


        }

        [HttpGet]
        public ActionResult GetReoni(int pageSize, int pageNumber, string sortColumn, string sortOrder, string search, string searchColumn, string searchTerms)
        {

            var skip = (pageNumber - 1) * pageSize;

            var total = BexUow.Reon.GetTotalReonData();

            var reonData = BexUow.Reon.GetReonData().Select(x =>
                                                         new ReonIndexData
                                                         {
                                                             ReonId = x.Id,
                                                             OznakaReona = x.OznReona,
                                                             NazivReona = x.NazivReona,
                                                             NazivRegiona = x.Region?.NazivSkraceni,
                                                             TipReona = x.ReonTip.Opis,
                                                             BarkodReona = x.BarKodReona,
                                                             KmReona = x.KmDoReona,
                                                             KmOptimalna=x.OptimalnaKilometraza

                                                         });



            if (sortOrder.Equals("desc"))
                reonData = reonData.OrderByDescending(s => s.GetType().GetProperty(sortColumn).GetValue(s)).ToList().Skip(skip).Take(pageSize);
            else
                reonData = reonData.OrderBy(s => s.GetType().GetProperty((sortColumn == "") ? "ReonId" : sortColumn).GetValue(s)).ToList().Skip(skip).Take(pageSize);

            if (!String.IsNullOrEmpty(searchTerms))
            {
                total = BexUow.Reon.GetSearchReonData(searchTerms).Count();

                reonData = BexUow.Reon.GetSearchReonData(searchTerms).Select(x =>
                                                       new ReonIndexData
                                                       {
                                                           ReonId = x.Id,
                                                           OznakaReona = x.OznReona,
                                                           NazivReona = x.NazivReona,
                                                           NazivRegiona = x.Region?.NazivSkraceni,
                                                           TipReona = x.ReonTip.Opis,
                                                           BarkodReona = x.BarKodReona,
                                                           KmReona = x.KmDoReona,
                                                           KmOptimalna=x.OptimalnaKilometraza

                                                       });
                if (sortOrder.Equals("desc"))
                {
                    reonData = reonData.OrderByDescending(s => s.GetType().GetProperty((sortColumn == "") ? "ReonId" : sortColumn).GetValue(s))
                                                 .ToList()
                                                 .Skip(skip)
                                                 .Take(pageSize);
                }
                else
                {
                    reonData = reonData.OrderBy(s => s.GetType().GetProperty((sortColumn == "") ? "ReonId" : sortColumn).GetValue(s))
                                                 .ToList()
                                                 .Skip(skip)
                                                 .Take(pageSize);
                }


            }

            var jsonData = new TableJsonIndexData<ReonIndexData>()
            {
                total = total,
                rows = reonData
            };
            var jsonResult = Json(jsonData, "TableJsonIndexData", JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;

        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reon reon = BexUow.Reon.Find(id);
            if (reon == null)
            {
                return HttpNotFound();
            }
            return View(reon);
        }

        
        [ClaimsAuthentication(Resource = "Reon", Operation = "Create, All")]
        public ActionResult Create()
        {
            ViewBag.RegionId = new SelectList(BexUow.Region.AllAsNoTracking, "Id", "NazivSkraceni");
            ViewBag.Tip = new SelectList(BexUow.ReonTip.AllAsNoTracking, "Id", "Opis");
            return View();
        }

        //// POST: KontaktTelefon/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Reon reon, int? regionId, int? tip)
        {
            if (ModelState.IsValid)
            {
                reon.RegionId = regionId;
                reon.Tip = tip;
                BexUow.Reon.Add(reon);
                var commandResult = BexUow.SubmitChanges();

                if (commandResult.IsSuccessful)
                { return RedirectToAction("Index"); }

                ExceptionSolver.PrepareModelState(ModelState, commandResult);
            }
            ViewBag.RegionId = new SelectList(BexUow.Region.AllAsNoTracking, "Id", "NazivSkraceni", reon.RegionId);
            ViewBag.Tip = new SelectList(BexUow.ReonTip.AllAsNoTracking, "Id", "Opis", reon.Tip);
            return  View(reon);
            
        }

        [ClaimsAuthentication(Resource = "Reon", Operation = "Edit, All")]
        public ActionResult Edit(int? id)
        {
            ViewBag.RegionId = new SelectList(BexUow.Region.AllAsNoTracking, "Id", "NazivSkraceni");
            ViewBag.Tip = new SelectList(BexUow.ReonTip.AllAsNoTracking, "Id", "Opis");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reon reon = BexUow.Reon.Find(id);
            if (reon == null)
            {
                return HttpNotFound();
            }
            ViewBag.RegionId = new SelectList(BexUow.Region.AllAsNoTracking, "Id", "NazivSkraceni", reon.RegionId);
            ViewBag.Tip = new SelectList(BexUow.ReonTip.AllAsNoTracking, "Id", "Opis", reon.Tip);
            return View(reon);
        }

        // POST: KontaktTelefon/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Reon reon)
        {
            if (ModelState.IsValid)
            {

                BexUow.Reon.Update(reon);
                var uowCommandResult = BexUow.SubmitChanges();

                if (uowCommandResult.IsSuccessful)
                { return RedirectToAction("Details", new { id = reon.Id }); }

                ExceptionSolver.PrepareModelState(ModelState, uowCommandResult);
            }
            return View(reon);
        }

        [ClaimsAuthentication(Resource = "Reon", Operation = "Delete, All")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reon reon = BexUow.Reon.Find(id);
            if (reon == null)
            {
                return HttpNotFound();
            }
            return View(reon);
        }

        // POST: KontaktTelefon/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Reon reon = BexUow.Reon.Find(id);
            //db.KontaktTelefons.Remove(kontaktTelefon);
            //db.SaveChanges();
            return RedirectToAction("Index");
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
