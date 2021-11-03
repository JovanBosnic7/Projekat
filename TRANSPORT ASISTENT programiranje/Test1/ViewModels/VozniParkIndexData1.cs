using System.Collections.Generic;
using Bex.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace DDtrafic.ViewModels
{
   public class VozniParkIndexData1
   {
        public int Id { get; set; }
        public string GarazniBroj { get; set; }
        public string Oznaka { get; set; }
        public string Kategorija { get; set; }
        public string Karoserija { get; set; }
        public string PodKaroserija { get; set; }
        public string Marka { get; set; }
        public string Model { get; set; }
        public string TipVozila { get; set; }
        public int? Godiste { get; set; }
        public string Sasija { get; set; }
        public string BrojMotora { get; set; }
        public decimal? SnagaMotora { get; set; }
        public int? Zapremina { get; set; }
        public int? Masa { get; set; }
        public int? Nosivost { get; set; }
        public int? NDM { get; set; }
        public int? NabavnaKM { get; set; }
        public decimal? PropisanaPotrosnja { get; set; }
        public int? BrojVrata { get; set; }
        public int? BrMestaStajanje { get; set; }
        public int? BrMestaSedenje { get; set; }
        public string PogonskiPneumatici { get; set; }
        public string UpravljackiPneumatici { get; set; }
        public string VrstaGoriva { get; set; }
        public string Menjac { get; set; }
        public string Status { get; set; }
        public string Napomena { get; set; }
        public DateTime? DatumRegistracije { get; set; }
        public string Sifra { get; set; }

        public IEnumerable<VozniParkDnevnik> VozniParkDnevnik { get; set; }

    }
}