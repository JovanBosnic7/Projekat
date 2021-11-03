using AspNet.DAL.EF.Models.Security;
using AspNet.DAL.EF.UOW.Security;
using Bex.Common;
using Bex.Common.Interfaces;
using Bex.DAL.EF.UOW;
using Bex.MVC.Exceptions;
using BexMVC.ViewModels;
using LinqToExcel;
using Microsoft.AspNet.Identity;
using OfficeOpenXml;
using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace BexMVC.Controllers
{
    public class GalleryAdminController : Controller
    {
        
        public GalleryAdminController() : this(new BexUow(), new SecurityUow(System.Web.HttpContext.Current.GetOwinContext()), new ExceptionSolver())
        { }
        public GalleryAdminController(
            IBexUow bexUow,
            ISecurityUow securityUow,
            IExceptionSolver exceptionSolver)
        {
            BexUow = bexUow;
            SecurityUow = securityUow;
            ExceptionSolver = exceptionSolver;
        }
       
        public ActionResult Create(int tipId, int id, int isProfile)
        {
            ImageEditorViewModel vm = new ImageEditorViewModel();
            ViewBag.Action = "Create";
            ViewBag.TipId = tipId;
            ViewBag.Id = id;
            ViewBag.IsProfile = isProfile;
            return PartialView(vm);
        }

        [HttpPost]
        public async Task<ActionResult> Create(ImageEditorViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var applicationUser = await SecurityUow.UserManager.FindUserByIdAsync(User.Identity.GetUserId()) as ApplicationUser;
                    var bexUserId = BexUow.KorisniciPrograma.Find(x => x.AspNetUserId == applicationUser.Id).Id;

                    var fileModel = WebFileViewModel.getEntityModel(model.FileImage, model.TipId, model.StraniId);
                    fileModel.StraniId = model.StraniId;
                    fileModel.TypeId = model.TipId;
                    fileModel.UserUneoId = bexUserId;
                    BexUow.WebFiles.Add(fileModel);
                    var commandResult = BexUow.SubmitChanges();

                    if (commandResult.IsSuccessful)
                    {
                        var entity = ImageEditorViewModel.getEnityModel(model);
                        entity.WebImageId = fileModel.Id;
                        entity.IsProfile = System.Convert.ToBoolean(model.isProfile);
                        entity.OrderNo = BexUow.Gallery.GetAll(true).Count() > 0 ? BexUow.Gallery.GetAll(true).Max(x => x.OrderNo) + 1 : 1;
                        BexUow.Gallery.Add(entity);
                        commandResult = BexUow.SubmitChanges();

                        if (commandResult.IsSuccessful)
                        {
                            if(model.TipId == 4)
                            {
                                if(System.Convert.ToBoolean(model.isProfile))
                                    return RedirectToAction("Edit", "Zaposleni", new { id = model.StraniId });
                                else
                                    return RedirectToAction("Index", "Zaposleni");
                            }else if(model.TipId == 2)
                            {
                                return RedirectToAction("Index", "VozniPark");
                            }
                               
                        }
                    }

                    return Json(new { success = true, Caption = model.Caption });
                }

                return Json(new { success = false, ValidationMessage = "Please check validation messages" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, ExceptionMessage = "Some error here" + ex.Message });
            }
        }

      

        public ActionResult _List(int tipId, int id)
        {
            
            //var list = BexUow.Gallery.GetAll().OrderBy(x => x.OrderNo)
            //            .Select(x => new ImageList
            //            {
            //                Id = x.Id,
            //                IsActive = x.IsActive,
            //                OrderNo = x.OrderNo,
            //                WebImageId = x.WebImageId,
            //                Title = x.Title
            //            }).ToList();


            var list = (from galerija in BexUow.Gallery.AllAsNoTracking
                        join webfiles in BexUow.WebFiles.AllAsNoTracking on galerija.WebImageId equals webfiles.Id
                        join webfilestype in BexUow.WebFilesTip.AllAsNoTracking on webfiles.TypeId equals webfilestype.Id
                        where webfiles.TypeId == tipId && webfiles.StraniId == id
                        select new ImageList
                        {
                            Id = galerija.Id,
                            IsActive = galerija.IsActive,
                            IsProfile = galerija.IsProfile,
                            OrderNo = galerija.OrderNo,
                            WebImageId = galerija.WebImageId,
                            Title = galerija.Title,
                            UpdateDate = webfiles.UpdateDate,
                            UserUneo = webfiles.KorisniciPrograma.Kontakt.Naziv,
                            IsFromSrv = (webfiles.Data == null) ? true : false,
                            PathImage = "http://" + WebFileViewModel.host + "/" + webfilestype.DirName + "/" + webfiles.StraniId + "/" + webfiles.FileName
                        }).ToList().OrderByDescending(x=>x.UpdateDate);

            return PartialView(list);
        }

        public ActionResult _ProfileImage(int tipId, int id, int isProfile)
        {
            bool _isProfile = System.Convert.ToBoolean(isProfile);
            var imageProfile = (from galerija in BexUow.Gallery.AllAsNoTracking
                        join webfiles in BexUow.WebFiles.AllAsNoTracking on galerija.WebImageId equals webfiles.Id
                        where webfiles.TypeId == tipId && webfiles.StraniId == id && galerija.IsProfile == _isProfile
                                select new ImageList
                        {
                            Id = galerija.Id,
                            IsActive = galerija.IsActive,
                            IsProfile = galerija.IsProfile,
                            OrderNo = galerija.OrderNo,
                            WebImageId = galerija.WebImageId,
                            Title = galerija.Title,
                            UpdateDate = webfiles.UpdateDate,
                            UserUneo = webfiles.KorisniciPrograma.Kontakt.Naziv
                        }).FirstOrDefault();

            return PartialView(imageProfile);
        }


        [HttpPost]
        public JsonResult ChangeActive(int Id, bool status)
        {
           
            var entity = BexUow.Gallery.Find(Id);
            entity.IsActive = status;
            var commandResult = BexUow.SubmitChanges();

            //if (commandResult.IsSuccessful)
            //{
            //}

            return Json(entity.Title);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            { BexUow.Dispose(); }
            base.Dispose(disposing);
        }

        private IExceptionSolver ExceptionSolver { get; }
        private IBexUow BexUow { get; }
        private ISecurityUow SecurityUow { get;  }
    }
}