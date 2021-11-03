using System.Collections.Generic;
using Bex.Models;
using System;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations.Schema;

namespace BexMVC.ViewModels
{
   public class ZadaciIndexData
    {

        //public int? PosiljkaId { get; set; }
        public string PosiljkaId { get; set; }

        public string TipZadatka { get; set; }

        public string KlijentDetails { get; set; }

        public string KontaktOsoba { get; set; }

        public string KontaktTel { get; set; }

        public string KontaktTel2 { get; set; }

        public string NapomenaZadatak { get; set; }

        public int? StatusZadatka { get; set; }
        public int? StatusPosiljke { get; set; }

        public string Reon { get; set; }

        public int? ReonId { get; set; }
        public int? ZonaId { get; set; }
        public int? KurirId { get; set; }


        public DateTime? DatumObavljanja { get; set; }
        public TimeSpan? VremeObavljanja { get; set; }

        public string Izvrsio { get; set; }

        public DateTime? Aktuelan { get; set; }
    }
}