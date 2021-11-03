using System.Collections.Generic;
using Bex.Models;
using System;

namespace BexMVC.ViewModels
{
   public class ArtikliIndexData
   {
        public int Id { get; set; }
        public string Sifra { get; set; }
        public string Grupa { get; set; }
        public string Opis { get; set; }
        public decimal? Kolicina { get; set; }
        public string Napomena { get; set; }
        public int? Nav { get; set; }       
    }
}