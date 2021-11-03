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
    public class KontaktZiroRacunController : Controller
    {
        public KontaktZiroRacunController() : this(new BexUow(), new ExceptionSolver())
        { }
        public KontaktZiroRacunController(
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
            KontaktZiroRacun kontaktZiroRacun = BexUow.KontaktZiroRacun.Find(id);
            if (kontaktZiroRacun == null)
            {
                return HttpNotFound();
            }
            return View(kontaktZiroRacun);
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
        public ActionResult Create(KontaktZiroRacun kontaktZiroRacun, int kontaktId = 0)
        {
            if (ModelState.IsValid)
            {
                kontaktZiroRacun.KontaktId = kontaktId;
                BexUow.KontaktZiroRacun.Add(kontaktZiroRacun);
                var commandResult = BexUow.SubmitChanges();

                if (commandResult.IsSuccessful)
                { return RedirectToAction("../Kontakt"); }

                ExceptionSolver.PrepareModelState(ModelState, commandResult);
            }
            return  View(kontaktZiroRacun);
            
        }

        // GET: KontaktTelefon/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KontaktZiroRacun kontaktZiroRacun = BexUow.KontaktZiroRacun.Find(id);
            if (kontaktZiroRacun == null)
            {
                return HttpNotFound();
            }
            ViewBag.KontaktId = 11;
            return View(kontaktZiroRacun);
        }

        // POST: KontaktTelefon/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(KontaktZiroRacun kontaktZiroRacun)
        {
            if (ModelState.IsValid)
            {
                //BexUow.Entry(kontaktTelefon).State = EntityState.Modified;
                //db.SaveChanges();
                BexUow.KontaktZiroRacun.Update(kontaktZiroRacun);
                var uowCommandResult = BexUow.SubmitChanges();

                if (uowCommandResult.IsSuccessful)
                { return RedirectToAction("Details", new { id = kontaktZiroRacun.Id }); }

                ExceptionSolver.PrepareModelState(ModelState, uowCommandResult);
            }
            ViewBag.KontaktId = 11;
            return View(kontaktZiroRacun);
        }

        // GET: KontaktTelefon/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KontaktZiroRacun kontaktZiroRacun = BexUow.KontaktZiroRacun.Find(id);
            if (kontaktZiroRacun == null)
            {
                return HttpNotFound();
            }
            return View(kontaktZiroRacun);
        }

        // POST: KontaktTelefon/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            KontaktZiroRacun kontaktZiroRacun = BexUow.KontaktZiroRacun.Find(id);
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
