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
using BexMVC.Helpers;
using System.Data.Entity.Core;
using System.Collections.Generic;
using BexMVC.Filters;

namespace BexMVC.Controllers
{
    [Authorize]
    [ClaimsAuthentication(Resource = "Street", Operation = "Read, All")]
    public class StreetController : Controller
    {
        public StreetController() : this(new BexUow(), new ExceptionSolver())
        { }
        public StreetController(
            IBexUow bexUow,
            IExceptionSolver exceptionSolver)
        {
            BexUow = bexUow;
            ExceptionSolver = exceptionSolver;
        }

        public ActionResult Index(int? id, int? page, string searchUlica, string searchMesto)
        {
            TempData.ToList().ForEach(keyValue =>
                ModelState.AddModelError("", keyValue.Value.ToString()));
            TempData.Clear();

            return View();
        }

        [HttpGet]
        public ActionResult GetUlice(int pageSize, int pageNumber, string sortColumn, string sortOrder, string search, string searchColumn, string searchTerms)
        {

            var skip = (pageNumber - 1) * pageSize;

            var total = BexUow.Street.GetTotalStreetData();

            var streetData = BexUow.Street.GetStreetData().Select(x =>
                                                         new StreetIndexData
                                                         {
                                                             UlicaId = x.Id,
                                                             NazivMesta = x.Place?.PlaceName,
                                                             NazivUlice = x.StreetName

                                                         });



            if (sortOrder.Equals("desc"))
                streetData = streetData.OrderByDescending(s => s.GetType().GetProperty(sortColumn).GetValue(s)).ToList().Skip(skip).Take(pageSize);
            else
                streetData = streetData.OrderBy(s => s.GetType().GetProperty((sortColumn == "") ? "UlicaId" : sortColumn).GetValue(s)).ToList().Skip(skip).Take(pageSize);




            //var vozniParkData = BexUow.VozniPark.GetVozniParkData().OrderByDescending(e => order).ToList().Skip(skip).Take(pageSize);

            if (!String.IsNullOrEmpty(searchTerms))
            {
                total = BexUow.Street.GetSearchStreetData(searchTerms).Count();

                streetData = BexUow.Street.GetSearchStreetData(searchTerms).Select(x =>
                                                        new StreetIndexData
                                                        {
                                                            UlicaId = x.Id,
                                                            NazivMesta = x.Place?.PlaceName,
                                                            NazivUlice = x.StreetName

                                                        });
                if (sortOrder.Equals("desc"))
                {
                    streetData = streetData.OrderByDescending(s => s.GetType().GetProperty((sortColumn == "") ? "UlicaId" : sortColumn).GetValue(s))
                                                 .ToList()
                                                 .Skip(skip)
                                                 .Take(pageSize);
                }
                else
                {
                    streetData = streetData.OrderBy(s => s.GetType().GetProperty((sortColumn == "") ? "UlicaId" : sortColumn).GetValue(s))
                                                 .ToList()
                                                 .Skip(skip)
                                                 .Take(pageSize);
                }


            }

            var jsonData = new TableJsonIndexData<StreetIndexData>()
            {
                total = total,
                rows = streetData
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
            Street street = BexUow.Street.Find(id);
            if (street == null)
            {
                return HttpNotFound();
            }
            return View(street);
        }

        [ClaimsAuthentication(Resource = "Street", Operation = "Create, All")]
        public ActionResult Create(int placeId = 0)
        {
            ViewBag.PlaceId = new SelectList(BexUow.Place.AllAsNoTracking, "Id", "PlaceName");
            return View();
        }

        //// POST: KontaktTelefon/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Street street, int placeId = 0)
        {
            if (ModelState.IsValid)
            {
                street.PlaceId = placeId;
                BexUow.Street.Add(street);
                var commandResult = BexUow.SubmitChanges();

                if (commandResult.IsSuccessful)
                { return RedirectToAction("Index"); }

                ExceptionSolver.PrepareModelState(ModelState, commandResult);
            }
            ViewBag.PlaceId = new SelectList(BexUow.Place.AllAsNoTracking, "Id", "PlaceName", street.PlaceId);
            return View(street);

        }

        [ClaimsAuthentication(Resource = "Street", Operation = "Edit, All")]
        public ActionResult Edit(int? id)
        {
            ViewBag.PlaceId = new SelectList(BexUow.Place.AllAsNoTracking, "Id", "PlaceName");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Street street = BexUow.Street.Find(id);
            if (street == null)
            {
                return HttpNotFound();
            }
            //ViewBag.KontaktId = 11;
            return View(street);
        }

        // POST: KontaktTelefon/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Street street)
        {
            if (ModelState.IsValid)
            {
                //BexUow.Entry(kontaktTelefon).State = EntityState.Modified;
                //db.SaveChanges();
                BexUow.Street.Update(street);
                var uowCommandResult = BexUow.SubmitChanges();

                if (uowCommandResult.IsSuccessful)
                { return RedirectToAction("Details", new { id = street.Id }); }

                ExceptionSolver.PrepareModelState(ModelState, uowCommandResult);
            }
            //ViewBag.KontaktId = 11;
            ViewBag.PlaceId = new SelectList(BexUow.Place.AllAsNoTracking, "Id", "PlaceName",street.PlaceId);
            return View(street);
        }

        [ClaimsAuthentication(Resource = "Street", Operation = "Delete, All")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Street street = BexUow.Street.Find(id);
            if (street == null)
            {
                return HttpNotFound();
            }
            return View(street);
        }


        // POST: KontaktTelefon/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Street street = BexUow.Street.Find(id);
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

        public ActionResult GetMesto(string query)
        {
            return Json(_GetMesto(query), JsonRequestBehavior.AllowGet);
        }

        private List<Autocomplete> _GetMesto(string query)
        {
            List<Autocomplete> mestoList = new List<Autocomplete>();
            try
            {
                var results = BexUow.Place.GetAll(true).Where(m => m.PlaceName.Contains(query)).ToList();

                foreach (var r in results)
                {
                    Autocomplete models = new Autocomplete();
                    models.Name = r.PlaceName;
                    models.Id = r.Id;
                    mestoList.Add(models);
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
            return mestoList;
        }

        private IExceptionSolver ExceptionSolver { get; }
        private IBexUow BexUow { get; }
    }
}
