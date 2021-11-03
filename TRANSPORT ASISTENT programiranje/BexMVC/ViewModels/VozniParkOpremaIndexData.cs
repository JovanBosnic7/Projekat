using System.Collections.Generic;
using Bex.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace BexMVC.ViewModels
{
   public class VozniParkOpremaIndexData
    {
        public int Id { get; set; }
        //public int VpId { get; set; }
        public string GrupaNaziv { get; set; }
        public string TipNaziv { get; set; }
        public string PodtipNaziv { get; set; }

        public DateTime DatumUnosa { get; set; }
        public string UserUneoNaziv { get; set; }

        

    }
}