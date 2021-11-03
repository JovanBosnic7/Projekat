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
using BexMVC.Helpers;
using System.Data.Entity.Core;
using BexMVC.Filters;

namespace BexMVC.Controllers
{
   
    [ClaimsAuthentication(Resource = "GorivoKartica", Operation = "Read, All")]
    public class GorivoKarticaController : Controller
    {
        public GorivoKarticaController() : this(new BexUow(), new ExceptionSolver())
        { }
        public GorivoKarticaController(
            IBexUow bexUow,
            IExceptionSolver exceptionSolver)
        {
            BexUow = bexUow;
            ExceptionSolver = exceptionSolver;

        }
        public ActionResult Index()
        {
            TempData.ToList().ForEach(keyValue =>
                ModelState.AddModelError("", keyValue.Value.ToString()));
            TempData.Clear();

            //ViewBag.Pumpa = new SelectList(BexUow.GorivoPumpa.AllAsNoTracking, "NazivPumpe", "NazivPumpe");

            return View();
        }

        [HttpGet]      
        public ActionResult GetGorivoKartice(int pageSize, int pageNumber, string sortColumn, string sortOrder, string search, string searchColumn, string searchTerms)
        {

            var skip = (pageNumber - 1) * pageSize;

            var gorivoData = BexUow.GorivoKartica.GetAll(true).Where(x => x.Storno==false).Select(x =>
                                                         new GorivoKarticaIndexData
                                                         {
                                                             Id = x.Id,
                                                             NazivKartice = x.NazivKartice ?? "",
                                                             Pumpa = x.GorivoPumpa.NazivPumpe ?? "",
                                                             DatumProizvodnje = x.DatumProizvodnje,
                                                             DatumIsteka = x.DatumIsteka,
                                                             Vozilo = x.VozniPark.Oznaka ?? "",
                                                             Pincode = x.Pincode ?? "",
                                                             BrojKartice = x.BrojKartice ?? ""
                                                         }).AsEnumerable();

            if (!String.IsNullOrEmpty(searchTerms))
            {
                gorivoData = GetSearchGorivoKarticaData(searchTerms, gorivoData).AsEnumerable();
                                                    //.Select(x =>
                                                    //     new GorivoKarticaIndexData
                                                    //     {
                                                    //         Id = x.Id,
                                                    //         NazivKartice = x.NazivKartice,
                                                    //         Pumpa = x.Pumpa,
                                                    //         DatumProizvodnje = x.DatumProizvodnje,
                                                    //         DatumIsteka = x.DatumIsteka,
                                                    //         Vozilo = x.Vozilo,
                                                    //         Pincode = x.Pincode,
                                                    //         BrojKartice = x.BrojKartice
                                                    //     }).AsEnumerable();
            }

            var total = gorivoData.Count();

            if (sortOrder.Equals("desc"))
                gorivoData = gorivoData.OrderByDescending(s => s.GetType().GetProperty(sortColumn).GetValue(s)).ToList().Skip(skip).Take(pageSize);
            else
                gorivoData = gorivoData.OrderBy(s => s.GetType().GetProperty((sortColumn == "") ? "Id" : sortColumn).GetValue(s)).ToList().Skip(skip).Take(pageSize);
            

            var jsonData = new TableJsonIndexData<GorivoKarticaIndexData>()
            {
                total = total,
                rows = gorivoData
            };
            var jsonResult = Json(jsonData, "TableJsonIndexData", JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
            
        }

        public IEnumerable<GorivoKarticaIndexData> GetSearchGorivoKarticaData(string searchTerms, IEnumerable<GorivoKarticaIndexData> searchDataSet)
        {
            string[] terms = searchTerms.Split(',');

            var gorivo = searchDataSet;



            foreach (string t in terms)
            {
                string searchColumn = "";
                string searchTxt = "";

                string[] searchCT = t.Split(':');
                searchColumn = searchCT[0];
                searchTxt = searchCT[1];


                if (searchColumn.Equals("NazivKartice") && !String.IsNullOrEmpty(searchTxt))
                {
                    gorivo = gorivo.Where(k => k.NazivKartice.ToUpper().Contains(searchTxt.ToUpper()));

                }
                else if (searchColumn.Equals("Pumpa") && !String.IsNullOrEmpty(searchTxt))
                {
                    gorivo = gorivo.Where(k => k.Pumpa.ToUpper().Contains(searchTxt.ToUpper()));
                }
                else if (searchColumn.Equals("Vozilo") && !String.IsNullOrEmpty(searchTxt))
                {
                    gorivo = gorivo.Where(k => k.Vozilo.ToUpper().Contains(searchTxt.ToUpper()));
                }
                else if (searchColumn.Equals("Pincode") && !String.IsNullOrEmpty(searchTxt))
                {
                    gorivo = gorivo.Where(k => k.Pincode.ToUpper().Contains(searchTxt.ToUpper()));
                }
                else if (searchColumn.Equals("BrojKartice") && !String.IsNullOrEmpty(searchTxt))
                {
                    gorivo = gorivo.Where(k => k.BrojKartice.ToUpper().Contains(searchTxt.ToUpper()));
                }
                else if (searchColumn.Equals("DatumProizvodnje") && !String.IsNullOrEmpty(searchTxt))
                {
                    //string[] arrDatum = searchTxt.Split('/');
                    //DateTime thisDate1 = new DateTime(int.Parse(arrDatum[2]), int.Parse(arrDatum[0]), int.Parse(arrDatum[1]));
                    DateTime datumFirst, datumSecond;
                    string[] arrDatumAll = searchTxt.Split('t', 'o');

                    string[] arrDatumFirst = arrDatumAll[0].Trim().Split('-');
                    datumFirst = new DateTime(int.Parse(arrDatumFirst[0]), int.Parse(arrDatumFirst[1]), int.Parse(arrDatumFirst[2]));

                    string[] arrDatumSecond = arrDatumAll[2].Trim().Split('-');
                    datumSecond = new DateTime(int.Parse(arrDatumSecond[0]), int.Parse(arrDatumSecond[1]), int.Parse(arrDatumSecond[2]));

                    gorivo = gorivo.Where(k => k.DatumProizvodnje >= datumFirst && k.DatumProizvodnje <= datumSecond);
                }
                else if (searchColumn.Equals("DatumIsteka") && !String.IsNullOrEmpty(searchTxt))
                {
                    //string[] arrDatum = searchTxt.Split('/');
                    //DateTime thisDate1 = new DateTime(int.Parse(arrDatum[2]), int.Parse(arrDatum[0]), int.Parse(arrDatum[1]));
                    DateTime datumFirst, datumSecond;
                    string[] arrDatumAll = searchTxt.Split('t', 'o');

                    string[] arrDatumFirst = arrDatumAll[0].Trim().Split('-');
                    datumFirst = new DateTime(int.Parse(arrDatumFirst[0]), int.Parse(arrDatumFirst[1]), int.Parse(arrDatumFirst[2]));

                    string[] arrDatumSecond = arrDatumAll[2].Trim().Split('-');
                    datumSecond = new DateTime(int.Parse(arrDatumSecond[0]), int.Parse(arrDatumSecond[1]), int.Parse(arrDatumSecond[2]));


                    gorivo = gorivo.Where(k => k.DatumIsteka >= datumFirst && k.DatumIsteka <= datumSecond);
                }
                
            }
            return gorivo;
        }

        [ClaimsAuthentication(Resource = "GorivoKartica", Operation = "Create, All")]
        public ActionResult Create()
        {

            ViewBag.PumpaId = new SelectList(BexUow.GorivoPumpa.AllAsNoTracking.OrderBy(p => p.NazivPumpe), "Id", "NazivPumpe");
            ViewBag.VpId = new SelectList(BexUow.VozniPark.AllAsNoTracking.OrderBy(v=> v.Oznaka), "Id", "Oznaka");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(GorivoKartica gorivoKartica)
        {
            if (ModelState.IsValid)
            {
                BexUow.GorivoKartica.Add(gorivoKartica);
                var commandResult = BexUow.SubmitChanges();

                if (commandResult.IsSuccessful)
                {
                    //return RedirectToAction("Index", "GorivoKartica"); 
                    return Json("Uspešno ste dodali gorivo karticu", JsonRequestBehavior.AllowGet);
                }

                ExceptionSolver.PrepareModelState(ModelState, commandResult);
            }

            return Json("Greška pri dodavanju gorivo kartice", JsonRequestBehavior.AllowGet);
            //return View();

        }

        [ClaimsAuthentication(Resource = "GorivoKartica", Operation = "Create, All")]
        public ActionResult Edit(int id)
        { 
            var gorivoData = BexUow.GorivoKartica.Find(id);

            ViewBag.PumpaId = new SelectList(BexUow.GorivoPumpa.AllAsNoTracking.OrderBy(p=> p.NazivPumpe), "Id", "NazivPumpe", gorivoData.PumpaId);
            ViewBag.VpId = new SelectList(BexUow.VozniPark.AllAsNoTracking.OrderBy(v => v.Oznaka), "Id", "Oznaka", gorivoData.VpId);


            return View(gorivoData);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(GorivoKartica gorivoKartica)
        {
            if (ModelState.IsValid)
            {
                BexUow.GorivoKartica.Update(gorivoKartica);
                var commandResult = BexUow.SubmitChanges();

                if (commandResult.IsSuccessful)
                {
                    //return RedirectToAction("Index", "GorivoKartica"); 
                    return Json("Uspešno ste izmenili gorivo karticu", JsonRequestBehavior.AllowGet);
                }

                ExceptionSolver.PrepareModelState(ModelState, commandResult);
            }

            return Json("Greška pri izmeni gorivo kartice", JsonRequestBehavior.AllowGet);
            //return View();

        }

        [ClaimsAuthentication(Resource = "GorivoKartica", Operation = "Delete, All")]
        public ActionResult Delete(int id)
        {
            var karticaData = BexUow.GorivoKartica.Find(id);
            karticaData.Storno = true;
            BexUow.GorivoKartica.Update(karticaData);
            var commandResult = BexUow.SubmitChanges();

            if (commandResult.IsSuccessful)
            { return RedirectToAction("Index", "GorivoKartica"); }

            ExceptionSolver.PrepareModelState(ModelState, commandResult);
            return View();
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
