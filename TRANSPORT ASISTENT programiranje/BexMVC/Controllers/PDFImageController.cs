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
using System.Web;
using System.IO;

namespace BexMVC.Controllers
{

    public class PDFImageController : Controller
    {
        public PDFImageController() : this(new BexUow(), new ExceptionSolver())
        { }
        public PDFImageController(
            IBexUow bexUow,
            IExceptionSolver exceptionSolver)
        {
            BexUow = bexUow;
            ExceptionSolver = exceptionSolver;
        }
        
        [HttpGet]
        public ActionResult SaveImagesPDF()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SaveImagesPDF(HttpPostedFileBase UploadedImage)
        {
            if (UploadedImage.ContentLength>0)
            {
                string ImageFileName = Path.GetFileName(UploadedImage.FileName);

                string FolderPath = Path.Combine(Server.MapPath("/UploadedImages"), ImageFileName);

                UploadedImage.SaveAs(FolderPath);
            }
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
