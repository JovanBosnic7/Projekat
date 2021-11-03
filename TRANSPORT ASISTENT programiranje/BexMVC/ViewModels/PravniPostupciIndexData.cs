using System.Collections.Generic;
using Bex.Models;
using System;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations.Schema;

namespace BexMVC.ViewModels
{
   public class PravniPostupciIndexData
    {        
        public int Id { get; set; }
        public DateTime DatumUnosa { get; set; }
        public string Firma { get; set; }
        public string Oblast { get; set; }
        public string Vrsta { get; set; }
        public string NadlezniOrgan { get; set; }
        public string BrojPredmeta { get; set; }
        public DateTime Datum { get; set; }
        public string Lice { get; set; }
        public string Opis { get; set; }
        public string Vrednost { get; set; }
        public string Zastupnik { get; set; }
        public DateTime DatumSlOdgovora { get; set; }
        public DateTime DatumZavrsetka { get; set; }
        public bool Zavrseno { get; set; }
        public int MinKazna { get; set; }
        public int MaxKazna { get; set; }
        public int OcekivanaKazna { get; set; }
        public string Upisao { get; set; }
    }
}