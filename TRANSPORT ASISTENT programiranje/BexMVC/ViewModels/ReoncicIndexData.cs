using System;
using System.Collections.Generic;
using Bex.Models;


namespace BexMVC.ViewModels
{
   public class ReoncicIndexData
   {
        public IEnumerable<Reoncic> Reoncic { get; set; }
        public Reon Reon { get; set; }

        public int ReoncicId { get; set; }
        public string OznakaReona { get; set; }
        public string RegionNaziv { get; set; }
        public string NazivReoncica { get; set; }

        public TimeSpan? PreuzimanjeDoDefault { get; set; }
        public DateTime? DatumPoslednjeOdjave { get; set; }
        public TimeSpan? VremePoslednjeOdjave { get; set; }

        public bool? OdjavljujeSe { get; set; }
        public bool? DeoMesta { get; set; }
        public string NapomenaOdjava { get; set; }


    }
}