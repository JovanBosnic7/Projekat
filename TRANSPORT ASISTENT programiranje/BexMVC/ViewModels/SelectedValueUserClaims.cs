using System.Collections.Generic;
using Bex.Models;
using System;

namespace BexMVC.ViewModels
{
   public class SelectedValueUserClaims
    {

        

        public string userId { get; set; }

        //public string roleName { get; set; }
        public List<SelectedRowValueUserClaims> selectedValues { get; set; }

    }

    public partial class SelectedRowValueUserClaims
    {
        public int Id { get; set; }
        
    }

   


}