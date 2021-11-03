using System;
using System.Collections.Generic;
using Bex.Models;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace BexMVC.ViewModels
{
   public class StreetEditViewModel
   {
        public int StreetId { get; set; } //PostalAddressID

        public string PakId { get; set; } //CustomerID

        [Display(Name = "Address Line 1")]
        [StringLength(100)]
        public string StreetAddress1 { get; set; }

        [Display(Name = "Address Line 2")]
        [StringLength(100)]
        public string StreetAddress2 { get; set; }

        [Display(Name = "City / Town")]
        [StringLength(50)]
        public string City { get; set; }

        [Display(Name = "Zip / Postal Code")]
        [StringLength(10)]
        public string PostalCode { get; set; }

        [Required]
        [Display(Name = "Country")]
        public string SelectedCountryIso3 { get; set; }
        public IEnumerable<SelectListItem> Countries { get; set; }

        [Required]
        [Display(Name = "State / Region")]
        public string SelectedRegionCode { get; set; }
        public IEnumerable<SelectListItem> Regions { get; set; }




    }
}