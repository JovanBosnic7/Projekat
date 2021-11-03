using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

using Bex.Models;
using Bex.Common;
using Bex.MVC.Exceptions;
using Bex.DAL.EF.UOW;
using BexMVC.ViewModels;

namespace BexMVC.Controllers
{
    public class KontaktFizickoLiceController : Controller
    {
        public KontaktFizickoLiceController() : this(new BexUow(), new ExceptionSolver())
        { }
        public KontaktFizickoLiceController(
            IBexUow bexUow,
            IExceptionSolver exceptionSolver)
        {
            BexUow = bexUow;
            ExceptionSolver = exceptionSolver;
        }

        // GET: KontaktFizickoLice
        //public ActionResult Index(int? id,  int? page, string searchString)
        //{
        //    TempData.ToList().ForEach(keyValue =>
        //        ModelState.AddModelError("", keyValue.Value.ToString()));
        //    TempData.Clear();
        //    var viewModel = new KontaktIndexData();

        //    viewModel.KontaktsFizickaLica = BexUow.KontaktFizickoLice.AllAsNoTracking;


        //    if (searchString != null)
        //        page = 1;
        //    else
        //        searchString = ViewBag.CurrentFilte;

        //    ViewBag.CurrentFilter = searchString;

        //    if (!String.IsNullOrEmpty(searchString))
        //    {
        //        viewModel.Kontakts = BexUow.KontaktFizickoLice.GetAll(s => s.Naziv.ToUpper().Contains(searchString.ToUpper()));
        //    }



        //    int pageSize = 10;
        //    int pageNumber = (page ?? 1);
        //    //Used the following two formulas so that it doesn't round down on the returned integer
        //    decimal totalPages = ((decimal)(BexUow.KontaktFizickoLice.GetAll().Count() / (decimal)pageSize));
        //    ViewBag.TotalPages = Math.Ceiling(totalPages);
        //    //These next two functions could maybe be reduced to one function....would require some testing and building
        //    viewModel.Kontakts = BexUow.KontaktFizickoLice.GetAll().OrderBy(p => p.Id).ToPagedList(pageNumber, pageSize);
        //    //ViewBag.OnePageofTeachers = viewModel.Kontakts;
        //    ViewBag.PageNumber = pageNumber;



        //    return View(viewModel);
        //}

        // GET: KontaktFizickoLice/Details/5
        public ActionResult Details(int? id, int? kontaktId = 0)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KontaktFizickoLice kontaktFizickoLice = BexUow.KontaktFizickoLice.Find(id);
            var kontakt = BexUow.Kontakts.Find(k => k.Id == kontaktId);
            ViewBag.KontaktId = kontakt.Id;
            ViewBag.KontaktNaziv = kontakt.Naziv;
            if (kontaktFizickoLice == null)
            {
                return HttpNotFound();
            }
            return View(kontaktFizickoLice);

            //var fizickoLice = BexUow.KontaktFizickoLice.Find(id);
            //var kontakt = BexUow.Kontakts.Find(k => k.Id == kontaktId);
            //ViewBag.KontaktId = kontakt.Id;
            //ViewBag.KontaktNaziv = kontakt.Naziv;

            //BexUow.KontaktFizickoLice.Update(fizickoLice);

            //var uowCommandResult = BexUow.SubmitChanges();

            //if (!uowCommandResult.IsSuccessful)
            //{
            //    ExceptionSolver.PrepareModelState(ModelState, uowCommandResult);
            //    ExceptionSolver.PrepareTempData(TempData, ModelState);
            //}
        }

        // GET: KontaktFizickoLice/Create
        public ActionResult CreateFL(int kontaktId = 0)
        {
            if (ModelState.IsValid)
            {
                var kontakt = BexUow.Kontakts.Find(k => k.Id == kontaktId);
                ViewBag.KontaktId = kontakt.Id;
                ViewBag.KontaktNaziv = kontakt.Naziv;
            }
            else
            { ModelState.AddModelError("", "MVC_Handled_ModelValidation"); }
            //ViewBag.KontaktId = new SelectList(BexUow.Kontakts.Find(k => k.Id == kontaktId)., "Id", "Naziv",kontaktId);
            return View();
        }

        // POST: KontaktFizickoLice/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateFL( KontaktFizickoLice kontaktFizickoLice, int kontaktId = 0)
        {
            if (ModelState.IsValid)
            {
                BexUow.KontaktFizickoLice.Add(kontaktFizickoLice);
                var commandResult = BexUow.SubmitChanges();
                if (commandResult.IsSuccessful)
                {
                    return RedirectToAction("../Kontakt");
                }
            }

            //ViewBag.KontaktId = new SelectList(db.Kontakt, "Id", "Ime", kontaktFizickoLice.KontaktId);
            return View(kontaktFizickoLice);
        }

        // GET: KontaktFizickoLice/Edit/5
        public ActionResult Edit(int? id = 0, int? kontaktId=0)
        {
            
            if (id == null)
            {
                TempData["ModelError_Index"] = "Bad request. Correct request needs value for id";
                return RedirectToAction("Index");
            }

            KontaktFizickoLice fizickoLice = BexUow.KontaktFizickoLice.Find(id);
            var kontakt = BexUow.Kontakts.Find(k => k.Id == kontaktId);


            if (kontakt == null)
            {
                TempData["ModelError_Index"] = $"Kontakt with Id = {id} is not found.";
                return RedirectToAction("Index");
            }


            ViewBag.KontaktId = kontakt.Id;
            ViewBag.KontaktNaziv = kontakt.Naziv;


            return View(fizickoLice);
           



        }

        // POST: KontaktFizickoLice/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(KontaktFizickoLice kontaktFizickoLice)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    BexUow.KontaktFizickoLice.Update(kontaktFizickoLice);

                    var uowCommandResult = BexUow.SubmitChanges();

                    if (uowCommandResult.IsSuccessful)
                    { return RedirectToAction("../Kontakt"); }

                    ExceptionSolver.PrepareModelState(ModelState, uowCommandResult);
                }
                else
                { ModelState.AddModelError("", "MVC_Handled_ModelValidation"); }
            }
            catch (DataException /* dex */)
            {
                //Log the error (uncomment dex variable name after DataException and add a line here to write a log.
                ModelState.AddModelError(string.Empty, "Unable to save changes. Try again, and if the problem persists contact your system administrator.");
            }
            //PopulateAddressDropDownList(1);
            return View(kontaktFizickoLice);
        }

        // GET: KontaktFizickoLice/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    KontaktFizickoLice kontaktFizickoLice = BexUow.KontaktFizickoLice.Find(id);
        //    if (kontaktFizickoLice == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(kontaktFizickoLice);
        //}

        //// POST: KontaktFizickoLice/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    KontaktFizickoLice kontaktFizickoLice = BexUow.KontaktFizickoLice.Find(id);
        //    BexUow.KontaktFizickoLice.Remove(kontaktFizickoLice);
        //    BexUow.SubmitChanges();
        //    return RedirectToAction("Index");
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                BexUow.Dispose();
            }
            base.Dispose(disposing);
        }
        private IExceptionSolver ExceptionSolver { get; }
        private IBexUow BexUow { get; }
    }
}
