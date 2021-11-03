using System.Collections.Generic;
using Bex.Models;
using System;

namespace BexMVC.ViewModels
{
   public class MagacinUIartIndexData
   {
        public int Id { get; set; }
        public string TipPromene { get; set; }
        public string Magacin { get; set; }
        public string Sifra { get; set; }
        public string Grupa { get; set; }
        public string Opis { get; set; }
        public decimal? Kolicina { get; set; }
        public DateTime? Datum { get; set; }
        public bool? Nav { get; set; }       
    }
}