using System.Collections.Generic;
using Bex.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace DDtrafic.ViewModels
{
   public class GorivoIndexData
    {
        public int Id { get; set; }
        public DateTime? Datum { get; set; }
        public TimeSpan? Vreme { get; set; }
        public string Registracija { get; set; }
        public decimal? PropisanaPotrosnja { get; set; }
        public string Pumpa { get; set; }
        public int Kilometraza { get; set; }
        public decimal Litara { get; set; }
        public decimal Cena { get; set; }
        public decimal Iznos { get; set; }
        public int PresaoKmOdPrethodnogSipanja { get; set; }
        public decimal ProsekOdPrethodnog { get; set; }
        public string Napomena { get; set; }

    }
}