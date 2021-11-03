using AspNet.DAL.EF.Models.Security;
using AspNet.DAL.EF.UOW.Security;
using Bex.Common;
using Bex.Common.Interfaces;
using Bex.DAL.EF.UOW;
using DDtrafic.MVC.Exceptions;
using DDtrafic.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace DDtrafic.Controllers
{
    public class GalleryAdminController : Controller
    {
        
        public GalleryAdminController() : this(new BexUow(), new SecurityUow(System.Web.HttpContext.Current.GetOwinContext()))
        { }
        public GalleryAdminController(
            IBexUow bexUow,
            ISecurityUow securityUow)
        {
            BexUow = bexUow;
            SecurityUow = securityUow;
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
                    var fileModel = WebFileViewModel.getEntityModel(model.FileImage, model.TipId, model.StraniId);
                    fileModel.StraniId = model.StraniId;
                    fileModel.TypeId = model.TipId;
                    fileModel.UserUneoId = 0;
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
                            if(model.TipId == 2)
                            {
                                if(System.Convert.ToBoolean(model.isProfile))
                                    return RedirectToAction("Edit", "Zaposleni", new { id = model.StraniId });
                                else
                                    return RedirectToAction("Index", "Zaposleni");
                            }else if(model.TipId == 1)
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
                        where webfiles.TypeId == tipId && webfiles.StraniId == id && galerija.IsActive==true
                        select new ImageList
                        {
                            Id = galerija.Id,
                            IsActive = galerija.IsActive,
                            IsProfile = galerija.IsProfile,
                            OrderNo = galerija.OrderNo,
                            WebImageId = galerija.WebImageId,
                            Title = galerija.Title,
                            UpdateDate = webfiles.UpdateDate,
                            UserUneo = ""                           
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
                            UserUneo = ""
                        }).FirstOrDefault();

            return PartialView(imageProfile);
        }
        public ActionResult ChangeActive(int Id, bool status)
        {
           
            var entity = BexUow.Gallery.Find(Id);
            entity.IsActive = status;
            BexUow.Gallery.Update(entity);
            var uowCommandResult = BexUow.SubmitChanges();

            if (uowCommandResult.IsSuccessful)
            { return RedirectToAction("Index", "VozniPark"); }

            ExceptionSolver.PrepareModelState(ModelState, uowCommandResult);

            //if (commandResult.IsSuccessful)
            //{
            //}

            return HttpNotFound();

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