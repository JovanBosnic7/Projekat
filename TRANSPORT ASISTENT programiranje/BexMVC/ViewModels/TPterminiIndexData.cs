using System.Collections.Generic;
using Bex.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace BexMVC.ViewModels
{
    public class TPterminiIndexData
    {
        public int? Id { get; set; }
        public DateTime? DatumTermina { get; set; } //
        public string VremeTermina { get; set; }
        public string TerminOd { get; set; }
        public string TerminDo { get; set; }
        public string LokacijaTP { get; set; } //
        public string Ime { get; set; } //
        public string Prezime { get; set; } //
        public string Telefon { get; set; } // 
        public string RegOznaka { get; set; } //
        public string Kategorija { get; set; }
        public string Model { get; set; }
        public string StatusTermina { get; set; } //
        public string Uneo { get; set; } //
        public string Napomena { get; set; } //
    }
}