
using Bex.Common;
using Bex.DAL.EF.UOW;
using DDtrafic.MVC.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DDtrafic.Controllers
{
    public class GalleryController : Controller
    {
        public GalleryController() : this(new BexUow(), new ExceptionSolver())
        { }
        public GalleryController(
            IBexUow bexUow,
            IExceptionSolver exceptionSolver)
        {
            BexUow = bexUow;
            ExceptionSolver = exceptionSolver;
        }
        // GET: Gallery
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult _List()
        {
            
            var list = BexUow.Gallery.GetAll().OrderBy(x => x.OrderNo)
                        .Select(x => new ImageList
                        {
                            Id = x.Id,
                            IsActive = x.IsActive,
                            OrderNo = x.OrderNo,
                            WebImageId = x.WebImageId,
                            Title = x.Title
                        }).ToList();

            return PartialView(list);
        }

        private IExceptionSolver ExceptionSolver { get; }
        private IBexUow BexUow { get; }


    }
}