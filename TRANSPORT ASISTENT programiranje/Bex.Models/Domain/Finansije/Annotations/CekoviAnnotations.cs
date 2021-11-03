using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Bex.Models
{
    [MetadataType(typeof(CekoviMetadata))]

    public partial class Cekovi
    {
        public class CekoviMetadata
        {
            public int Id { get; set; }

            public string BrojCeka { get; set; }
            public DateTime? DatumDospeca { get; set; }

            public string BrojTekucegRacuna { get; set; }
            [Range(0, 5000, ErrorMessage = "Iznos čeka ne može biti veći od 5000rsd.")]
            public int? IznosCeka { get; set; }
            [ForeignKey("UserUneo")]
            public int? UserUneoId { get; set; }
            public DateTime? DatumUnosa { get; set; }
            public DateTime? DatumRealizacije { get; set; }
            [ForeignKey("Banka")]
            public int? BankaId { get; set; }
            public int? BrojSpecifikacije { get; set; }
            public bool Storno { get; set; }
            public string Napomena { get; set; }
            public bool InternoRazduzen { get; set; }
            [ForeignKey("KontaktUloga")]
            
            public int? ZastupnikId { get; set; }
            [ForeignKey("CekProvizija")]
            public int? ProvizijaId { get; set; }
            public object Banka { get; set; }
            public object KontaktUloga { get; set; }
            public object CekProvizija { get; set; }
            public object UserUneo { get; set; }

           
            private CekoviMetadata() { }

        }
    }

}