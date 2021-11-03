using System;
using System.Collections.Generic;
using Bex.Models;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace BexMVC.ViewModels
{
   public class AddressEditViewModel
   {
        [Display(Name = "Pak broj")]
        public string PakBroj { get; set; }

        
        [Required]
        [Display(Name = "Mesto")]
        public string SelectedMesto { get; set; }
        public IEnumerable<SelectListItem> Mesta { get; set; }

        [Required]
        [Display(Name = "Ulice")]
        public string SelectedUlica { get; set; }
        public IEnumerable<SelectListItem> Ulice { get; set; }




    }
}