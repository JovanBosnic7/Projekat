using System.Collections.Generic;
using Bex.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace BexMVC.ViewModels
{
    public class UgovorIndexData
    {
        public int? Id { get; set; }

        public string Nosilac { get; set; }
        public string Region { get; set; }
        public int? BrojUgovora { get; set; }
        public DateTime? DatumUgovora { get; set; }
        public int? VerzijaUgovora { get; set; }
        public DateTime? DatumVerzije { get; set; }
        public DateTime? DatumObnove { get; set; }
        public string Ugovorio { get; set; }
        public string Agent { get; set; }
        //public string Posrednik { get; set; }
        //public bool Aktivan { get; set; }

        public IEnumerable<Ugovor> Ugovor { get; set; }

    }
}