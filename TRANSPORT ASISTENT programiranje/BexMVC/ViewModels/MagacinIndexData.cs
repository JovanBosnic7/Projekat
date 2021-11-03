using System.Collections.Generic;
using Bex.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace BexMVC.ViewModels
{
   public class MagacinIndexData
    {
        public int Id { get; set; }
        public DateTime? DatumZaduzenja { get; set; }
        public TimeSpan? VremeZaduzenja { get; set; }

        public int? KurirId { get; set; }
        public string KurirNaziv { get; set; }

        public string ReonNaziv { get; set; }

        public string Vozilo { get; set; }

        public int? ZonaId { get; set; }

        public decimal? UkupnoRazduzio { get; set; }

        public int? DanasDostava { get; set; }

        


        //public string NapomenePreuzimanja { get; set; }



    }
}