using System.Collections.Generic;
using Bex.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace DDtrafic.ViewModels
{
   public class ZaposleniIndexData
   {
        
        public int? Id { get; set; }
        public string ImeIprezime { get; set; }
        public string Adresa { get; set; }
        public string Jmbg { get; set; }
        public string Pol { get; set; }
        public string Zanimanje { get; set; }
        public string SS { get; set; }
        public string RadniStaz { get; set; }
        public string Slava { get; set; }
        public string Kategorije { get; set; }
        public string BrojLK { get; set; }
        public DateTime? DatumLicnaKarta { get; set; }
        public string SupIzdao { get; set; }
        public DateTime? DatumLekarskog { get; set; }
        public DateTime? DatumVozacka { get; set; }
        public bool? Aktivan { get; set; }        
        public DateTime? DatumZaposlenja  { get; set; }
        public string RadnoMesto { get; set; }
        public string Telefon { get; set; }
        public DateTime? DatumPrijave { get; set; }
        public DateTime? ProbniRad { get; set; }
        public DateTime? DatumOdajve { get; set; }
        public DateTime? NaOdredjenoDo { get; set; }
        public string TekuciRacun { get; set; }
        public int? Plata { get; set; }
        public string Napomena { get; set; }
        public string Sifra { get; set; }

    }
}