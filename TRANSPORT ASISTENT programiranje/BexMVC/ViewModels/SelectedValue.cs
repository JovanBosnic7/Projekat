using System.Collections.Generic;
using Bex.Models;
using System;

namespace BexMVC.ViewModels
{
   public class SelectedValue
    {

        public string Id { get; set; }
        public List<SelectedRowValue> SelectedValues { get; set; }
 
    }

    public partial class SelectedRowValue
    {
        public int Id { get; set; }
       
    }

   


}