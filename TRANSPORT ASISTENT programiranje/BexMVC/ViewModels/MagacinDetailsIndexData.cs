using System.Collections.Generic;
using Bex.Models;
using System;
using Bex.Models.Domain;

namespace BexMVC.ViewModels
{
   public class MagacinDetailsIndexData
    {


        public int UkupnoObavljenihDostavaDanas { get; set; }
        public int UkupnoDostavaNareonu { get; set; }
        public int UkupnoObavljenihPreuzimanjaDanas { get; set; }
        public int UkupnoPreuzimanjaNareonu { get; set; }

        public decimal? UkupnoOtkupa { get; set; }

        public decimal? UkupnoPazara { get; set; }

        public decimal? UkupnoNaplatio { get; set; }

        public int? ReonId { get; set; }

        public string ReonNaziv { get; set; }

        public int? KurirId { get; set; }

        public string KurirNaziv { get; set; }

        public int? CountBezStatusa { get; set; }

        public PosiljkaZadatakTip TipZadatka { get; set; }
        public PosiljkaZadatakStatus StatusZadatka { get; set; }
        public KurirRazduzenje KurirRazduzenje { get; set; }

        //public List<ZadaciIndexData> ListaZadatakaRazduzenjaKurira { get; set; }

    }
}