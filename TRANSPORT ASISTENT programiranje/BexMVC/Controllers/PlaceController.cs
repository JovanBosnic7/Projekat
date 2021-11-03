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
using BexMVC.Filters;

namespace BexMVC.Controllers
{
    [Authorize]
    
    public class PlaceController : Controller
    {
        public PlaceController() : this(new BexUow(), new ExceptionSolver())
        { }
        public PlaceController(
            IBexUow bexUow,
            IExceptionSolver exceptionSolver)
        {
            BexUow = bexUow;
            ExceptionSolver = exceptionSolver;
        }

        [ClaimsAuthentication(Resource = "Place", Operation = "Read, All")]
        public ActionResult Index(int? id, int? page, string searchMesto, string searchOpstina)
        {
            TempData.ToList().ForEach(keyValue =>
                ModelState.AddModelError("", keyValue.Value.ToString()));
            TempData.Clear();

            return View();
        }


        [HttpGet]
        public ActionResult GetMesta(int pageSize, int pageNumber, string sortColumn, string sortOrder, string search, string searchColumn, string searchTerms)
        {

            var skip = (pageNumber - 1) * pageSize;

            var total = BexUow.Place.GetTotalPlaceData();

            var mestaData = BexUow.Place.GetPlaceData().Select(x =>
                                                         new PlaceIndexData
                                                         {
                                                             MestoId = x.Id,
                                                             NazivMesta = x.PlaceName,
                                                             NazivOpstine = x.Opstina?.OpstinaNaziv,
                                                             Ptt=x.Ptt
                                                            
                                                         });



            if (sortOrder.Equals("desc"))
                mestaData = mestaData.OrderByDescending(s => s.GetType().GetProperty(sortColumn).GetValue(s)).ToList().Skip(skip).Take(pageSize);
            else
                mestaData = mestaData.OrderBy(s => s.GetType().GetProperty((sortColumn == "") ? "MestoId" : sortColumn).GetValue(s)).ToList().Skip(skip).Take(pageSize);




            //var vozniParkData = BexUow.VozniPark.GetVozniParkData().OrderByDescending(e => order).ToList().Skip(skip).Take(pageSize);

            if (!String.IsNullOrEmpty(searchTerms))
            {
                total = BexUow.Place.GetSearchPlaceData(searchTerms).Count();

                mestaData = BexUow.Place.GetSearchPlaceData(searchTerms).Select(x =>
                                                        new PlaceIndexData
                                                        {
                                                            MestoId = x.Id,
                                                            NazivMesta = x.PlaceName,
                                                            NazivOpstine = x.Opstina?.OpstinaNaziv,
                                                            Ptt = x.Ptt

                                                        });
                if (sortOrder.Equals("desc"))
                {
                    mestaData = mestaData.OrderByDescending(s => s.GetType().GetProperty((sortColumn == "") ? "MestoId" : sortColumn).GetValue(s))
                                                 .ToList()
                                                 .Skip(skip)
                                                 .Take(pageSize);
                }
                else
                {
                    mestaData = mestaData.OrderBy(s => s.GetType().GetProperty((sortColumn == "") ? "MestoId" : sortColumn).GetValue(s))
                                                 .ToList()
                                                 .Skip(skip)
                                                 .Take(pageSize);
                }

               
            }
          
            var jsonData = new TableJsonIndexData<PlaceIndexData>()
            {
                total = total,
                rows = mestaData
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
            Place place = BexUow.Place.Find(id);
            if (place == null)
            {
                return HttpNotFound();
            }
            return View(place);
        }

        [HttpGet]
        [ClaimsAuthentication(Resource = "Place", Operation = "PadajuciMenu, All")]
        public ActionResult GetMestaRelacija()
        {
            var mestaRelacija = BexUow.Place.AllAsNoTracking.Where(x=>x.OznakaMesta != null && x.OznakaMesta != "").Select(x => new
            {
                idMesta = x.OznakaMesta,
                nazivMesta =  x.OznakaMesta + " - " + x.PlaceName


            }).OrderByDescending(a=>a.nazivMesta);

            return Json(mestaRelacija, JsonRequestBehavior.AllowGet);
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
