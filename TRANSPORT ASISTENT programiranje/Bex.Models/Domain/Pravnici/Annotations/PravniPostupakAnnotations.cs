using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Bex.Models
{
    [MetadataType(typeof(PravniPostupakMetadata))]

    public partial class PravniPostupak
    {
        public class PravniPostupakMetadata
        {
            public int Id { get; set; }
            public DateTime DatumUnosa { get; set; }
            [ForeignKey("Firma")]
            public int? FirmaId { get; set; }
            [ForeignKey("PPoblast")]
            public int? OblastId { get; set; }
            [ForeignKey("PPvrsta")]
            public int? VrstaId { get; set; }
            public bool? Tuzeni { get; set; }
            public bool? Tuzilac { get; set; }
            public string NadlezanOrgan { get; set; }
            public string BrojPredmeta { get; set; }
            public DateTime? Datum { get; set; }
            public string Lice { get; set; }
            public string Opis { get; set; }
            public string Vrednost { get; set; }
            public string Zastupnik { get; set; }
            public DateTime? DatumSledeceg { get; set; }
            public DateTime? DatumZavrsetka { get; set; }
            public string Napomena { get; set; }
            [ForeignKey("UserUneo")]
            public int UserUneoId { get; set; }
            public int? MinimalnaKazna { get; set; }
            public int? MaksimalanaKazna { get; set; }
            public int? OcekivanaKazna { get; set; }

            public object Firma { get; set; }
            public object PPvrsta { get; set; }
            public object PPoblast { get; set; }
            public object UserUneo { get; set; }

           
            private PravniPostupakMetadata() { }

        }
    }

}