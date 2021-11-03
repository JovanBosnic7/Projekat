using System.Collections.Generic;
using Bex.Models;
using System;

namespace BexMVC.ViewModels
{
   public class PosiljkeEvidentiranePorekloChartArea
    {

        public string period { get; set; }

        public int? callcenter { get; set; }
        public int? klijentski { get; set; }

        public int? integracija { get; set; }

        public int? web { get; set; }

    }
}