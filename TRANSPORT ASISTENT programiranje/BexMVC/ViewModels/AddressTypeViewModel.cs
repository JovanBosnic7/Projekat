using System;
using System.Collections.Generic;
using Bex.Models;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace BexMVC.ViewModels
{
   public class AddressTypeViewModel
   {
        [StringLength(38)]
        public string pakId { get; set; } // CustomerID , Carries the value in POST action.

        [Display(Name = "Address Type")]
        public string SelectedAddressType { get; set; }
        public IEnumerable<SelectListItem> AddressTypes { get; set; }




    }
}