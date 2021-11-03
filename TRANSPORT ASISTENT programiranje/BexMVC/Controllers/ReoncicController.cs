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
    [ClaimsAuthentication(Resource = "Reoncic", Operation = "Read, All")]
    public class ReoncicController : Controller
    {
        public ReoncicController() : this(new BexUow(), new ExceptionSolver())
        { }
        public ReoncicController(
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

            return View();


        }

        [HttpGet]
        public ActionResult GetReoncici(int pageSize, int pageNumber, string sortColumn, string sortOrder, string search, string searchColumn, string searchTerms)
        {

            var skip = (pageNumber - 1) * pageSize;

            var total = BexUow.Reoncic.GetTotalReoncicData();

            var reoncicData = BexUow.Reoncic.GetReoncicData().Select(x =>
                                                         new ReoncicIndexData
                                                         {
                                                             ReoncicId = x.Id,
                                                             OznakaReona = x.Reon?.OznReona,
                                                             RegionNaziv=x.Reon?.Region?.NazivSkraceni,
                                                             NazivReoncica = x.NazivReoncica,
                                                             PreuzimanjeDoDefault = x.PreuzimanjeDoDefault,
                                                             DatumPoslednjeOdjave = x.DatumPoslednjeOdjave,
                                                             VremePoslednjeOdjave = x.VremePoslednjeOdjave,
                                                             OdjavljujeSe = x.OdjavljujeSe,
                                                             DeoMesta = x.DeoMesta,
                                                             NapomenaOdjava = x.NapomenaOdjave

                                                         });



            if (sortOrder.Equals("desc"))
                reoncicData = reoncicData.OrderByDescending(s => s.GetType().GetProperty(sortColumn).GetValue(s)).ToList().Skip(skip).Take(pageSize);
            else
                reoncicData = reoncicData.OrderBy(s => s.GetType().GetProperty((sortColumn == "") ? "ReoncicId" : sortColumn).GetValue(s)).ToList().Skip(skip).Take(pageSize);

            if (!String.IsNullOrEmpty(searchTerms))
            {
                total = BexUow.Reoncic.GetSearchReoncicData(searchTerms).Count();

                reoncicData = BexUow.Reoncic.GetSearchReoncicData(searchTerms).Select(x =>
                                                       new ReoncicIndexData
                                                       {
                                                           ReoncicId = x.Id,
                                                           OznakaReona = x.Reon?.OznReona,
                                                           RegionNaziv = x.Reon?.Region?.NazivSkraceni,
                                                           NazivReoncica = x.NazivReoncica,
                                                           PreuzimanjeDoDefault = x.PreuzimanjeDoDefault,
                                                           DatumPoslednjeOdjave = x.DatumPoslednjeOdjave,
                                                           VremePoslednjeOdjave = x.VremePoslednjeOdjave,
                                                           OdjavljujeSe = x.OdjavljujeSe,
                                                           DeoMesta = x.DeoMesta,
                                                           NapomenaOdjava = x.NapomenaOdjave

                                                       });
                if (sortOrder.Equals("desc"))
                {
                    reoncicData = reoncicData.OrderByDescending(s => s.GetType().GetProperty((sortColumn == "") ? "ReoncicId" : sortColumn).GetValue(s))
                                                 .ToList()
                                                 .Skip(skip)
                                                 .Take(pageSize);
                }
                else
                {
                    reoncicData = reoncicData.OrderBy(s => s.GetType().GetProperty((sortColumn == "") ? "ReoncicId" : sortColumn).GetValue(s))
                                                 .ToList()
                                                 .Skip(skip)
                                                 .Take(pageSize);
                }


            }

            var jsonData = new TableJsonIndexData<ReoncicIndexData>()
            {
                total = total,
                rows = reoncicData
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
            Reoncic reoncic = BexUow.Reoncic.Find(id);
            if (reoncic == null)
            {
                return HttpNotFound();
            }
            return View(reoncic);
        }

        [ClaimsAuthentication(Resource = "Reoncic", Operation = "Create, All")]
        public ActionResult Create()
        {
            ViewBag.ReonId = new SelectList(BexUow.Reon.AllAsNoTracking, "Id", "OznReona");
            return View();
        }

        //// POST: KontaktTelefon/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Reoncic reoncic, int? reonId)
        {
            if (ModelState.IsValid)
            {
                reoncic.ReonId = reonId;
                BexUow.Reoncic.Add(reoncic);
                var commandResult = BexUow.SubmitChanges();

                if (commandResult.IsSuccessful)
                { return RedirectToAction("Index"); }

                ExceptionSolver.PrepareModelState(ModelState, commandResult);
            }
            ViewBag.ReonId = new SelectList(BexUow.Reon.AllAsNoTracking, "Id", "OznReona", reoncic.ReonId);
            return  View(reoncic);
            
        }

        [ClaimsAuthentication(Resource = "Reoncic", Operation = "Edit, All")]
        public ActionResult Edit(int? id)
        {
            ViewBag.ReonId = new SelectList(BexUow.Reon.AllAsNoTracking, "Id", "OznReona");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reoncic reoncic = BexUow.Reoncic.Find(id);
            if (reoncic == null)
            {
                return HttpNotFound();
            }
            return View(reoncic);
        }

        // POST: KontaktTelefon/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Reoncic reoncic)
        {
            if (ModelState.IsValid)
            {
                //BexUow.Entry(kontaktTelefon).State = EntityState.Modified;
                //db.SaveChanges();
                BexUow.Reoncic.Update(reoncic);
                var uowCommandResult = BexUow.SubmitChanges();

                if (uowCommandResult.IsSuccessful)
                { return RedirectToAction("Details", new { id = reoncic.Id }); }

                ExceptionSolver.PrepareModelState(ModelState, uowCommandResult);
            }
            ViewBag.ReonId = new SelectList(BexUow.Reon.AllAsNoTracking, "Id", "OznReona", reoncic.ReonId);
            return View(reoncic);
        }

        [ClaimsAuthentication(Resource = "Reoncic", Operation = "Delete, All")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reoncic reoncic = BexUow.Reoncic.Find(id);
            if (reoncic == null)
            {
                return HttpNotFound();
            }
            return View(reoncic);
        }

        // POST: KontaktTelefon/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Reoncic reoncic = BexUow.Reoncic.Find(id);
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
