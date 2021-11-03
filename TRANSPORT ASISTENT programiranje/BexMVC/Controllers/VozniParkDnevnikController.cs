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

namespace BexMVC.Controllers
{
    public class VozniParkDnevnikController : Controller
    {
        public VozniParkDnevnikController() : this(new BexUow(), new ExceptionSolver())
        { }
        public VozniParkDnevnikController(
            IBexUow bexUow,
            IExceptionSolver exceptionSolver)
        {
            BexUow = bexUow;
            ExceptionSolver = exceptionSolver;
        }

        public ActionResult Index(int? id, int? page)
        {
            TempData.ToList().ForEach(keyValue =>
                ModelState.AddModelError("", keyValue.Value.ToString()));
            TempData.Clear();
            var viewModel = new TroskoviIndexData()
            {
                VozniParkDnevnik = BexUow.VozniParkDnevnik.AllAsNoTracking.ToList()
            };

            return View(viewModel);
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
