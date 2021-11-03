using System.Collections.Generic;
using Bex.Models;
using System;

namespace BexMVC.ViewModels
{
   public class PosiljkeEvidentiraneChartArea
    {

        public string period { get; set; }

        public int? evidentiraneUkupno { get; set; }
        public int? evidentiranoPaketa { get; set; }
        public decimal? odnosPaketPosiljka { get; set; }

        public int? evidentiraneUkupno1Nedelja { get; set; }
        public int? evidentiranoPaketa1 { get; set; }
        public decimal? odnosPaketPosiljka1 { get; set; }

        public int? evidentiraneUkupno2Nedelja { get; set; }
        public int? evidentiranoPaketa2 { get; set; }
        public decimal? odnosPaketPosiljka2 { get; set; }

        public int? evidentiraneUkupno3Nedelja { get; set; }
        public int? evidentiranoPaketa3 { get; set; }
        public decimal? odnosPaketPosiljka3 { get; set; }

        public int? evidentiraneUkupno4Nedelja { get; set; }
        public int? evidentiranoPaketa4 { get; set; }
        public decimal? odnosPaketPosiljka4 { get; set; }

        public decimal? cenaPoPosiljci { get; set; }
        public decimal? cenaPoPaketu { get; set; }
        public decimal? cenaPoPosiljci1 { get; set; }
        public decimal? cenaPoPaketu1 { get; set; }

        public decimal? cenaPoPosiljci2 { get; set; }
        public decimal? cenaPoPaketu2 { get; set; }

        public decimal? cenaPoPosiljci3 { get; set; }
        public decimal? cenaPoPaketu3 { get; set; }

        //public decimal? odnosCenaPaketPosiljka { get; set; }

    }
}