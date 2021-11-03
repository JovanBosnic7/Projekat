using System.Collections.Generic;
using Bex.Models;
using System;

namespace BexMVC.ViewModels
{
   public class PosiljkeObrisanePorekloChartArea
    {

        public string period { get; set; }
        public int? klijentski { get; set; }
        public int? shipping { get; set; }
        public int? magacioner { get; set; }

        public int? test { get; set; }
        public int? duplirana { get; set; }
        public int? posiljalac { get; set; }
        public int? narucilac { get; set; }
        public int? gabarit { get; set; }


    }
}