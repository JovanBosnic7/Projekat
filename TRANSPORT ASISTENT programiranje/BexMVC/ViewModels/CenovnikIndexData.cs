using System.Collections.Generic;
using Bex.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace BexMVC.ViewModels
{
    public class CenovnikIndexData
    {
        public int? IdCenovnika { get; set; }
        public int? BrojCenovnika { get; set; }
        public DateTime? DatumCenovnika { get; set; }
        public string Opis { get; set; }
        //public string KorisnikCenovnika { get; set; }
        //public bool Storno { get; set; }

        public IEnumerable<Cenovnik> Cenovnik { get; set; }
        //public IEnumerable<Ugovor> Ugovor { get; set; }
    }
}