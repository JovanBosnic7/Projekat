using System.Collections.Generic;
using Bex.Models;
using System;

namespace BexMVC.ViewModels
{
   public class StetaIndexData
   {
        public int Id { get; set; }
        public string FirmaStete { get; set; }
        public string Vozilo { get; set; }
        public string NalogIzdao { get; set; }
        public string NalogSektor { get; set; }
        public string StetaTip { get; set; }
        public string Opis { get; set; }
        public string Napomena { get; set; }
        public string StetocinaZaposleni { get; set; }
        public string StetocinaCentar { get; set; }
        public DateTime? DatumPredajePravnojSluzbi { get; set; }
        public int? IznosRsd { get; set; }
        public int? IznosZaNaplatu { get; set; }
        public DateTime? DatumUnosa { get; set; }
        public string UserUnos { get; set; }
        public bool? Sporno { get; set; }
        public bool? Racun { get; set; }
        public bool? Kes { get; set; }
        public bool? Storno { get; set; }
        public DateTime? DatumOdluke { get; set; }
        public bool? KnjigovodstveniManjak { get; set; }
        public bool? Valuta { get; set; }
        public bool? PotpisanaOdluka { get; set; }
        public bool? Nenaplativo { get; set; }



    }
}