using System.Collections.Generic;
using Bex.Models;
using System;

namespace DDtrafic.ViewModels
{
   public class PopravkeIndexData
   {
        public int Id { get; set; }
        public int VpId { get; set; }
        public string Registracija { get; set; }
        public DateTime Datum { get; set; }
        public string Tip { get; set; }
        public int TipId { get; set; }
        public string Delovi { get; set; }
        public string BrojRacuna { get; set; }
        public string Napomena { get; set; }
        public decimal? IznosDin { get; set; }
        public decimal? IznosEur { get; set; }
    }
}