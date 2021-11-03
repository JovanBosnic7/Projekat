using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bex.Models
{
    [MetadataType(typeof(ArtikliMetadata))]

    public partial class Artikli
    {
        public class ArtikliMetadata
        {
            public int Id { get; set; }
            public int? ArtiTipId { get; set; }
            public bool? Sirovina { get; set; }
            public bool? Proizvod { get; set; }
            public bool? TrgovackaRoba { get; set; }
            public bool? ProdajaDozvoljena { get; set; }
            [ForeignKey("ArtikliVrsta")]
            public int? ArtVrstaId { get; set; }
            [ForeignKey("ArtikliGrupa")]
            public int? ArtGrupaId { get; set; }
            public int? ArtMarkaId { get; set; }
            public string Sifra { get; set; }
            public string Opis { get; set; }
            public string Oznaka { get; set; }
            public string Napomena { get; set; }
            public int? JmId { get; set; }
            public int? NavOk { get; set; }
            public bool? Storno { get; set; }

            public object ArtikliGrupa { get; set; }
            public object ArtikliVrsta { get; set; }

            //public object ArtikliTip { get; set; }
            //public object ArtikliVrsta { get; set; }
            //public object ArtikliGrupa { get; set; }
            //public object ArtikliMarka { get; set; }

            private ArtikliMetadata() { }

        }
    }

}