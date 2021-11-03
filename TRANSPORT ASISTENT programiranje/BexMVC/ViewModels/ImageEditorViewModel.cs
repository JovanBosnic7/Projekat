using Bex.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BexMVC.ViewModels
{
    public class ImageEditorViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Caption { get; set; }

        [Required]
        public int TipId { get; set; }

        [Required]
        public int StraniId { get; set; }
        [Required]         
        public HttpPostedFileBase FileImage { get; set; }

        public int isProfile { get; set; }

        internal static Gallery getEnityModel(ImageEditorViewModel model)
        {
            return new Gallery
            {
                IsActive = true,
                Title = model.Caption, 
                OrderNo = 0,
            };
        }

    }

    public class ImageList
    {
        public int Id { get; set; }
        public bool IsActive { get; set; }

        public bool IsProfile { get; set; }
        public int? OrderNo { get; set; }
        public string Title { get; set; }

        public DateTime UpdateDate { get; set; }
        public string UserUneo { get; set; }
        public int WebImageId { get; set; }

        public bool IsFromSrv { get; set; }

        public string PathImage { get; set; }
    }
}