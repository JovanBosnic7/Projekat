using System.Linq;
using System.Net;
using System.Web.Mvc;
using Bex.Models;
using Bex.MVC.Exceptions;
using Bex.DAL.EF.UOW;
using Bex.Common;
using BexMVC.ViewModels;

namespace BexMVC.Controllers
{
    public class KontaktDelatnostController : Controller
    {
        public KontaktDelatnostController() : this(new BexUow(), new ExceptionSolver())
        { }
        public KontaktDelatnostController(
            IBexUow bexUow,
            IExceptionSolver exceptionSolver)
        {
            BexUow = bexUow;
            ExceptionSolver = exceptionSolver;
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KontaktDelatnost kontaktDelatnost = BexUow.KontaktDelatnost.Find(id);
            if (kontaktDelatnost == null)
            {
                return HttpNotFound();
            }
            return View(kontaktDelatnost);
        }

        // GET: KontaktTelefon/Create
        public ActionResult Create(int kontaktId = 0)
        {
            if (ModelState.IsValid)
            {
                var kontakt = BexUow.Kontakts.Find(k => k.Id == kontaktId);
                ViewBag.KontaktId = kontakt.Id;
                ViewBag.KontaktNaziv = kontakt.Naziv;

                
            }
            else
            { ModelState.AddModelError("", "MVC_Handled_ModelValidation"); }
            
            return View();
        }

        //// POST: KontaktTelefon/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(KontaktDelatnost kontaktDelatnost, int kontaktId = 0)
        {
            if (ModelState.IsValid)
            {
                kontaktDelatnost.KontaktId = kontaktId;
                BexUow.KontaktDelatnost.Add(kontaktDelatnost);
                var commandResult = BexUow.SubmitChanges();

                if (commandResult.IsSuccessful)
                { return RedirectToAction("../Kontakt"); }

                ExceptionSolver.PrepareModelState(ModelState, commandResult);
            }
            return  View(kontaktDelatnost);
            
        }

        // GET: KontaktTelefon/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KontaktDelatnost kontaktDelatnost = BexUow.KontaktDelatnost.Find(id);
            if (kontaktDelatnost == null)
            {
                return HttpNotFound();
            }
            ViewBag.KontaktId = 11;
            return View(kontaktDelatnost);
        }

        // POST: KontaktTelefon/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(KontaktDelatnost kontaktDelatnost)
        {
            if (ModelState.IsValid)
            {
                BexUow.KontaktDelatnost.Update(kontaktDelatnost);
                var uowCommandResult = BexUow.SubmitChanges();

                if (uowCommandResult.IsSuccessful)
                { return RedirectToAction("Details", new { id = kontaktDelatnost.Id }); }

                ExceptionSolver.PrepareModelState(ModelState, uowCommandResult);
            }
            ViewBag.KontaktId = 11;
            return View(kontaktDelatnost);
        }

        // GET: KontaktTelefon/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KontaktDelatnost kontaktDelatnost = BexUow.KontaktDelatnost.Find(id);
            if (kontaktDelatnost == null)
            {
                return HttpNotFound();
            }
            return View(kontaktDelatnost);
        }

        // POST: KontaktTelefon/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            KontaktDelatnost kontaktDelatnost = BexUow.KontaktDelatnost.Find(id);
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
