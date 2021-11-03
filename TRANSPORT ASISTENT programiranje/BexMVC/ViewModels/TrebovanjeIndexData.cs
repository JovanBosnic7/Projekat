using System.Collections.Generic;
using Bex.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace BexMVC.ViewModels
{
   public class TrebovanjeIndexData
   {
        public int? IdTrebovanja { get; set; }
        public DateTime? DatumUnosa { get; set; }
        public string Trebuje { get; set; }
        public string Adresa { get; set; }
        public string ProfitniCentar { get; set; }
        public string Uneo { get; set; }
        public DateTime? DatumIzdavanja { get; set; }
        public string Izdao { get; set; }

    }
}