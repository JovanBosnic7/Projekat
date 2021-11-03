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
    public class ZaposleniPlataController : Controller
    {
        public ZaposleniPlataController() : this(new BexUow(), new ExceptionSolver())
        { }
        public ZaposleniPlataController(
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
            ZaposleniPlata zaposleniPlata = BexUow.ZaposleniPlata.Find(id);
            if (zaposleniPlata == null)
            {
                return HttpNotFound();
            }
            return View(zaposleniPlata);
        }

        // GET: KontaktTelefon/Create
        public ActionResult Create(int zaposleniId = 0)
        {
            if (ModelState.IsValid)
            {
                var zaposleni = BexUow.Zaposleni.Find(k => k.Id == zaposleniId);
                ViewBag.ZaposleniId = zaposleni.Id;
                ViewBag.ZaposleniNaziv = zaposleni.Kontakt.Naziv;

                
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
        public ActionResult Create(ZaposleniPlata zaposleniPlata, int zaposleniId = 0)
        {
            if (ModelState.IsValid)
            {
                zaposleniPlata.ZaposleniId = zaposleniId;
                BexUow.ZaposleniPlata.Add(zaposleniPlata);
                var commandResult = BexUow.SubmitChanges();

                if (commandResult.IsSuccessful)
                { return RedirectToAction("../Zaposleni"); }

                ExceptionSolver.PrepareModelState(ModelState, commandResult);
            }
            return  View(zaposleniPlata);
            
        }

        // GET: KontaktTelefon/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ZaposleniPlata zaposleniPlata = BexUow.ZaposleniPlata.Find(id);
            if (zaposleniPlata == null)
            {
                return HttpNotFound();
            }
            //ViewBag.ZaposleniId = 11;
            return View(zaposleniPlata);
        }

        // POST: KontaktTelefon/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ZaposleniPlata zaposleniPlata)
        {
            if (ModelState.IsValid)
            {
                //BexUow.Entry(kontaktTelefon).State = EntityState.Modified;
                //db.SaveChanges();
                BexUow.ZaposleniPlata.Update(zaposleniPlata);
                var uowCommandResult = BexUow.SubmitChanges();

                if (uowCommandResult.IsSuccessful)
                { return RedirectToAction("Details", new { id = zaposleniPlata.Id }); }

                ExceptionSolver.PrepareModelState(ModelState, uowCommandResult);
            }
            //ViewBag.KontaktId = 11;
            return View(zaposleniPlata);
        }

        // GET: KontaktTelefon/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ZaposleniPlata zaposleniPlata = BexUow.ZaposleniPlata.Find(id);
            if (zaposleniPlata == null)
            {
                return HttpNotFound();
            }
            return View(zaposleniPlata);
        }

        // POST: KontaktTelefon/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ZaposleniPlata zaposleniPlata = BexUow.ZaposleniPlata.Find(id);
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
