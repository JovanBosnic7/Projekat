using System;
using System.Collections.Generic;
using Microsoft.AspNet.Identity;

namespace BexMVC.ViewModels
{
    public class IndexViewModel
    {
        public bool HasPassword { get; set; }
        public bool IsEmailConfirmed { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsPhoneNumberConfirmed { get; set; }
        public bool TwoFactor { get; set; }
        public bool BrowserRemembered { get; set; }
        public bool IsInAdmins { get; set; }
        public IList<UserLoginInfo> Logins { get; set; }
    }
}