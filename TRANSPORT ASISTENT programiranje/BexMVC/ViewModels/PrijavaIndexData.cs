using System.Collections.Generic;
using Bex.Models;
using System;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations.Schema;

namespace BexMVC.ViewModels
{
   public class PrijavaIndexData
    {
        
        public int? PrijavaId { get; set; }

        public string TipPrijaveNaziv { get; set; }

        public string PodTipPrijaveNaziv { get; set; }        

        public string NacinPrijaveNaziv { get; set; }

        public int? PosiljkaId { get; set; }

        public string PrijavioUserNaziv { get; set; }

        public string PrijavioNaziv { get; set; }

        //public string PrijavioPrezime { get; set; }

        public string PrijavioEmail{ get; set; }
        public string PrijavioTelefon { get; set; }

        public string Opis { get; set; }

        public string StatusPrijaveNaziv { get; set; }

        public DateTime? DatumPrijave { get; set; }
        public DateTime? DatumPoslednjeIzmene { get; set; }

        
    }
}