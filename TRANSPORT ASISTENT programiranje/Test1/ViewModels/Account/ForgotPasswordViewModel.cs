using System;
using System.ComponentModel.DataAnnotations;

namespace DDtrafic.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}