using System.Collections.Generic;
using Bex.Models;
using System;

namespace BexMVC.ViewModels
{
   public class KontaktIndexData
   {
        public int Id { get; set; }
      
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public string KontaktNaziv { get; set; }

        public string Adresa { get; set; }
        public string BrojTxt { get; set; }

        public int PakId { get; set; }

        public bool Pravno { get; set; }

        public bool Stranac { get; set; }

        
    }
}