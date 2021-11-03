using System.Collections.Generic;
using Bex.Models;
using System;

namespace BexMVC.ViewModels
{
   public class SelectedValueIndexData
    {

        public int reonId { get; set; }
        public int kurirId { get; set; }

        public int napomenaId { get; set; }
        public List<SelectedRowValue> selectedValues { get; set; }

    }

    public partial class SelectedRowValue 
    {
        public int posiljkaId { get; set; }
        public string tipZadatka { get; set; }
    }

    public partial class SelectedListId
    {
        public List<SelectedRowValue> selectedValues { get; set; }
    }


}