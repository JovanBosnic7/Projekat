using System.Collections.Generic;
using Bex.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace BexMVC.ViewModels
{
   public class ZaposleniIndexData
   {
        
        public int? Id { get; set; }
        public string KontaktNaziv { get; set; }
        public string RegionNazivSkraceni { get; set; }
        public string SektorNaziv { get; set; }
        public string FirmaNaziv { get; set; }
        public bool? Aktivan { get; set; }        
        public DateTime? DatumZaposlenja  { get; set; }
        public string Jmbg { get; set; }
        public string Zanimanje { get; set; }
        public string SS { get; set; }
        public string RadniStaz { get; set; }
        public string RadnoMesto { get; set; }
        public string Sistematizacija { get; set; }
        public string Status { get; set; }
        public string Telefon { get; set; }
        public string Kategorija { get; set; }
        public string Grupa { get; set; }
        public DateTime? DatumPrijave { get; set; }
        public DateTime? ProbniRad { get; set; }
        public DateTime? DatumOdajve { get; set; }
        public DateTime? DatumLekarskog { get; set; }
        public DateTime? DatumLicnaKarta { get; set; }
        public DateTime? DatumVozacka { get; set; }
        public DateTime? DatumZdravstvena { get; set; }
        public DateTime? NaOdredjenoDo { get; set; }
        public string Slava { get; set; }
        public string TekuciRacun { get; set; }
        public string Adresa { get; set; }
        public string BrojLK { get; set; }
        public string OpstinaLK { get; set; }
        public string Napomena { get; set; }
        public string Program { get; set; }
        public bool? Invalidno { get; set; }
        public string BankaOvlascenje { get; set; }
        public string RazlogOtkaza { get; set; }
        public bool? IzjavaZaposleni { get; set; }
        public bool? IzjavaRukovodilac { get; set; }
        public string Pol { get; set; }
        public bool? Nav { get; set; }

    }
}