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
    public class KontaktTelefonController : Controller
    {
        public KontaktTelefonController() : this(new BexUow(), new ExceptionSolver())
        { }
        public KontaktTelefonController(
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
            KontaktTelefon kontaktTelefon = BexUow.KontaktTelefon.Find(id);
            if (kontaktTelefon == null)
            {
                return HttpNotFound();
            }
            return View(kontaktTelefon);
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
        public ActionResult Create(KontaktTelefon kontaktTelefon, int kontaktId = 0)
        {
            if (ModelState.IsValid)
            {
                kontaktTelefon.KontaktId = kontaktId;
                //var kontaktTelefon = new KontaktTelefon
                //{
                //    KontaktId = kontaktId,
                //    Fiksni = true,
                //    Posao = false,
                //    KontaktOsoba = kt.KontaktOsoba,
                //    Telefon = kt.Telefon,
                //    UserUnosId = 1
                //};
                BexUow.KontaktTelefon.Add(kontaktTelefon);
                var commandResult = BexUow.SubmitChanges();

                if (commandResult.IsSuccessful)
                { return RedirectToAction("../Kontakt"); }

                ExceptionSolver.PrepareModelState(ModelState, commandResult);
            }
            return  View(kontaktTelefon);
            
        }

        // GET: KontaktTelefon/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KontaktTelefon kontaktTelefon = BexUow.KontaktTelefon.Find(id);
            if (kontaktTelefon == null)
            {
                return HttpNotFound();
            }
            ViewBag.KontaktId = 11;
            //new SelectList(BexUow.Kontakts.AllAsNoTracking, "Id", "Ime", kontaktTelefon.KontaktId);
            return View(kontaktTelefon);
        }

        // POST: KontaktTelefon/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(KontaktTelefon kontaktTelefon)
        {
            if (ModelState.IsValid)
            {
                //BexUow.Entry(kontaktTelefon).State = EntityState.Modified;
                //db.SaveChanges();
                BexUow.KontaktTelefon.Update(kontaktTelefon);
                var uowCommandResult = BexUow.SubmitChanges();

                if (uowCommandResult.IsSuccessful)
                { return RedirectToAction("Details", new { id = kontaktTelefon.Id }); }

                ExceptionSolver.PrepareModelState(ModelState, uowCommandResult);
            }
            ViewBag.KontaktId = 11;
            //new SelectList(BexUow.Kontakts.AllAsNoTracking, "Id", "Ime", kontaktTelefon.KontaktId);
            return View(kontaktTelefon);
        }

        // GET: KontaktTelefon/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KontaktTelefon kontaktTelefon = BexUow.KontaktTelefon.Find(id);
            if (kontaktTelefon == null)
            {
                return HttpNotFound();
            }
            return View(kontaktTelefon);
        }

        // POST: KontaktTelefon/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            KontaktTelefon kontaktTelefon = BexUow.KontaktTelefon.Find(id);
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
