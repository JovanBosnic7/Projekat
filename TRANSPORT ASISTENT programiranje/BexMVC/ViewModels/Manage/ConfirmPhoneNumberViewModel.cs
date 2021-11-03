using System;
using System.ComponentModel.DataAnnotations;

namespace BexMVC.ViewModels
{
    public class ConfirmPhoneNumberViewModel
    {
        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }

        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
    }
}