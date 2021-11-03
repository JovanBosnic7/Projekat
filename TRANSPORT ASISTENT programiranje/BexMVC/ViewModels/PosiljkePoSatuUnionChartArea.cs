using System.Collections.Generic;
using Bex.Models;
using System;

namespace BexMVC.ViewModels
{
   public class PosiljkePoSatuUnionChartArea 
    {

        public int? PeriodStatus { get; set; }
        public int PeriodVreme { get; set; }
        public DateTime? PeriodDatum { get; set; }
        public int? CountPosiljke { get; set; }
        public int? CountPaketa { get; set; }

        public decimal? CenaPoPosiljci { get; set; }
        public decimal? CenaPoPaketu { get; set; }

        public decimal? OdnosCenaPosPaket { get; set; }
    }
}