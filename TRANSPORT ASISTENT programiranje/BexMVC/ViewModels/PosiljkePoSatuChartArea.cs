using System.Collections.Generic;
using Bex.Models;
using System;

namespace BexMVC.ViewModels
{
   public class PosiljkePoSatuChartArea 
    {

        public string period { get; set; }

        public int evidentirane { get; set; }
        public int preuzete { get; set; }

        public int dostavljene { get; set; }



    }
}