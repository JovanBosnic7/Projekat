using System;
using System.Collections.Generic;
using Microsoft.AspNet.Identity;

namespace BexMVC.ViewModels
{
    public class RoleViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public virtual IList<IUser> Users { get; }
    }
}