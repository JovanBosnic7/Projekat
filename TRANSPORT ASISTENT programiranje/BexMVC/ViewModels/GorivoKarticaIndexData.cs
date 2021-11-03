using System.Collections.Generic;
using Bex.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace BexMVC.ViewModels
{
   public class GorivoKarticaIndexData
    {
        public int Id { get; set; }
        public string NazivKartice { get; set; }
        public string Pumpa { get; set; }
        public DateTime DatumProizvodnje { get; set; }

        public DateTime DatumIsteka { get; set; }
        public string Vozilo { get; set; }
        public string Pincode { get; set; }
        public string BrojKartice { get; set; }

    }
}