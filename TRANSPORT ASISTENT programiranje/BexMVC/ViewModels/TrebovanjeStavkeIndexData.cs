using System.Collections.Generic;
using Bex.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace BexMVC.ViewModels
{
   public class TrebovanjeStavkeIndexData
   {
        public int? IdStavkeTrebovanja { get; set; }
        public int? Trebovanje { get; set; }
        public DateTime? DatumTrebovanja { get; set; }
        public string Artikal { get; set; }
        public int? Trebovano { get; set; }
        public int? Lager { get; set; }
        public int? Izdato { get; set; }
        public string Trebuje { get; set; }
        public string Adresa { get; set; }

    }
}