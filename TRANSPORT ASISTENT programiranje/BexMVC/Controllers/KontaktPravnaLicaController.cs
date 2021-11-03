using Bex.Common;
using Bex.DAL.EF.UOW;
using Bex.Models;
using Bex.MVC.Exceptions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;


namespace BexMVC.Controllers
{
    public class KontaktPravnaLicaController : Controller
    {
        public KontaktPravnaLicaController() : this(new BexUow(), new ExceptionSolver())
        { }
        public KontaktPravnaLicaController(
            IBexUow bexUow,
            IExceptionSolver exceptionSolver)
        {
            BexUow = bexUow;
            ExceptionSolver = exceptionSolver;
        }

        // GET: KontaktPravnaLica
        //public ActionResult Index()
        //{
        //    var kontaktPravnaLica = db.KontaktPravnaLica.Include(k => k.Kontakt);
        //    return View(kontaktPravnaLica.ToList());
        //}

        // GET: KontaktPravnaLica/Details/5
        public ActionResult Details(int? id, int? kontaktId=0)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KontaktPravnoLice kontaktPravnoLice = BexUow.KontaktPravnoLice.Find(id);
            var kontakt = BexUow.Kontakts.Find(k => k.Id == kontaktId);
            ViewBag.KontaktId = kontakt.Id;
            ViewBag.KontaktNaziv = kontakt.Naziv;
            if (kontaktPravnoLice == null)
            {
                return HttpNotFound();
            }
            return View(kontaktPravnoLice);
        }

        // GET: KontaktPravnaLica/Create
        public ActionResult CreatePL(int kontaktId=0)
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

        // POST: KontaktPravnaLica/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(KontaktPravnoLice kontaktPravnaLica, int kontaktId = 0)
        {
            if (ModelState.IsValid)
            {
                if (ModelState.IsValid)
                {
                    kontaktPravnaLica.KontaktId = kontaktId;
                    //var kontaktTelefon = new KontaktTelefon
                    //{
                    //    KontaktId = kontaktId,
                    //    Fiksni = true,
                    //    Posao = false,
                    //    KontaktOsoba = kt.KontaktOsoba,
                    //    Telefon = kt.Telefon,
                    //    UserUnosId = 1
                    //};
                    BexUow.KontaktPravnoLice.Add(kontaktPravnaLica);
                    var commandResult = BexUow.SubmitChanges();

                    if (commandResult.IsSuccessful)
                    { return RedirectToAction("../Kontakt"); }

                    ExceptionSolver.PrepareModelState(ModelState, commandResult);
                }
                
            }

            
            return View(kontaktPravnaLica);
        }

        // GET: KontaktPravnaLica/Edit/5
        public ActionResult Edit(int? id = 0, int? kontaktId=0)
        {
            if (id == null)
            {
                TempData["ModelError_Index"] = "Bad request. Correct request needs value for id";
                return RedirectToAction("Index");
            }

            KontaktPravnoLice pravnoLice = BexUow.KontaktPravnoLice.Find(id);
            var kontakt = BexUow.Kontakts.Find(k => k.Id == kontaktId);


            if (kontakt == null)
            {
                TempData["ModelError_Index"] = $"Kontakt with Id = {id} is not found.";
                return RedirectToAction("Index");
            }


            ViewBag.KontaktId = kontakt.Id;
            ViewBag.KontaktNaziv = kontakt.Naziv;


            return View(pravnoLice);
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            //KontaktPravnoLice kontaktPravnaLica = db.KontaktPravnaLica.Find(id);
            //if (kontaktPravnaLica == null)
            //{
            //    return HttpNotFound();
            //}
            //ViewBag.KontaktId = new SelectList(db.Kontakt, "Id", "Ime", kontaktPravnaLica.KontaktId);
            //return View(kontaktPravnaLica);
        }

        // POST: KontaktPravnaLica/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(KontaktPravnoLice PravnaLica)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    BexUow.KontaktPravnoLice.Update(PravnaLica);

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
            return View(PravnaLica);
            //if (ModelState.IsValid)
            //{
            //    db.Entry(kontaktPravnaLica).State = EntityState.Modified;
            //    db.SaveChanges();
            //    return RedirectToAction("Index");
            //}
            //ViewBag.KontaktId = new SelectList(db.Kontakt, "Id", "Ime", kontaktPravnaLica.KontaktId);
            //return View(kontaktPravnaLica);
        }

        // GET: KontaktPravnaLica/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    KontaktPravnoLice kontaktPravnaLica = db.KontaktPravnaLica.Find(id);
        //    if (kontaktPravnaLica == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(kontaktPravnaLica);
        //}

        // POST: KontaktPravnaLica/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    KontaktPravnoLice kontaktPravnaLica = db.KontaktPravnaLica.Find(id);
        //    db.KontaktPravnaLica.Remove(kontaktPravnaLica);
        //    db.SaveChanges();
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
