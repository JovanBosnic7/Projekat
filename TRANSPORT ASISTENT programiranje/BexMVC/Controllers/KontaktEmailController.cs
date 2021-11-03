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
    public class KontaktEmailController : Controller
    {
        public KontaktEmailController() : this(new BexUow(), new ExceptionSolver())
        { }
        public KontaktEmailController(
            IBexUow bexUow,
            IExceptionSolver exceptionSolver)
        {
            BexUow = bexUow;
            ExceptionSolver = exceptionSolver;
        }


        // GET: KontaktTelefon
        //public ActionResult Index()
        //{
        //    TempData.ToList().ForEach(keyValue =>
        //        ModelState.AddModelError("", keyValue.Value.ToString()));
        //    TempData.Clear();
        //    var viewModel = new KontaktTelefonData();

        //    viewModel.KontaktTelefoni = BexUow.KontaktTelefon.AllAsNoTracking;


        //    //var kontaktTelefons = BexUow.KontaktTelefon.Include(k => k.Kontakt);
        //    return View(viewModel);
        //}

        // GET: KontaktTelefon/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KontaktEmail kontaktEmail = BexUow.KontaktEmail.Find(id);
            if (kontaktEmail == null)
            {
                return HttpNotFound();
            }
            return View(kontaktEmail);
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
        public ActionResult Create(KontaktEmail kontaktEmail, int kontaktId = 0)
        {
            if (ModelState.IsValid)
            {
                kontaktEmail.KontaktId = kontaktId;
                BexUow.KontaktEmail.Add(kontaktEmail);
                var commandResult = BexUow.SubmitChanges();

                if (commandResult.IsSuccessful)
                { return RedirectToAction("../Kontakt"); }

                ExceptionSolver.PrepareModelState(ModelState, commandResult);
            }
            return  View(kontaktEmail);
            
        }

        // GET: KontaktTelefon/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KontaktEmail kontaktEmail = BexUow.KontaktEmail.Find(id);
            if (kontaktEmail == null)
            {
                return HttpNotFound();
            }
            ViewBag.KontaktId = 11;
            return View(kontaktEmail);
        }

        // POST: KontaktTelefon/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(KontaktEmail kontaktEmail)
        {
            if (ModelState.IsValid)
            { 
                BexUow.KontaktEmail.Update(kontaktEmail);
                var uowCommandResult = BexUow.SubmitChanges();

                if (uowCommandResult.IsSuccessful)
                { return RedirectToAction("Details", new { id = kontaktEmail.Id }); }

                ExceptionSolver.PrepareModelState(ModelState, uowCommandResult);
            }
            ViewBag.KontaktId = 11;
            return View(kontaktEmail);
        }

        // GET: KontaktTelefon/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KontaktEmail kontaktEmail = BexUow.KontaktEmail.Find(id);
            if (kontaktEmail == null)
            {
                return HttpNotFound();
            }
            return View(kontaktEmail);
        }

        // POST: KontaktTelefon/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            KontaktEmail kontaktEmail = BexUow.KontaktEmail.Find(id);
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
