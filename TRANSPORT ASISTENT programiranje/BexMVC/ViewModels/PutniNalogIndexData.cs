using System.Collections.Generic;
using Bex.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace BexMVC.ViewModels
{
   public class PutniNalogIndexData
   {
        public int Id { get; set; }
        public int VpId { get; set; }
        public string Vozilo { get; set; }
        public string Vozac { get; set; }
        public string RadnoMesto { get; set; }

        public string Tip { get; set; }
        public string Reon { get; set; }

        public string Region { get; set; }
        public int? RegionId { get; set; }
        public string Relacija { get; set; }

        public DateTime? DatumStart { get; set; }

        public string DatumStartFilter { get; set; }
        public DateTime? DatumStop { get; set; }
        public string DatumStopFilter { get; set; }

        public TimeSpan? VremeStart { get; set; }
        public TimeSpan? VremeStop { get; set; }

        public int? KmStart { get; set; }
        public int? KmStop { get; set; }
        public int? KmUkupno { get; set; }
        public string UserUneo { get; set; }
        public string Napomena { get; set; }
        public bool Storno { get; set; }
        public decimal? Litraza { get; set; }
        public int? BrojSipanja { get; set; }
        public string GarazniBroj { get; set; }
    }
}