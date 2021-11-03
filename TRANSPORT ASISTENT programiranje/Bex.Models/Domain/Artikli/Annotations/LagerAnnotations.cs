using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bex.Models
{
    [MetadataType(typeof(LagerMetadata))]

    public partial class Lager
    {
        public class LagerMetadata
        {
            public int Id { get; set; }
            [ForeignKey("MagacinSpisak")]
            public int? MagacinId { get; set; }
            [ForeignKey("Artikli")]
            public int? ArtId { get; set; }
            public decimal? Kolicina { get; set; }
            public DateTime? DatumIzmene { get; set; }
            public TimeSpan? VremeIzmene { get; set; }

            public object MagacinSpisak { get; set; }
            public object Artikli { get; set; }

            private LagerMetadata() { }

        }
    }

}