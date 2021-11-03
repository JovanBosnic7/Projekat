using System.Collections.Generic;
using Bex.Models;
using System;

namespace BexMVC.ViewModels
{
   public class VozniParkPoDanuChart
    {

        public string PeriodDatumGorivo { get; set; }
        public decimal? SumGorivo { get; set; }

        public string PeriodDatumKm { get; set; }
        public decimal? SumKm { get; set; }

    }
}