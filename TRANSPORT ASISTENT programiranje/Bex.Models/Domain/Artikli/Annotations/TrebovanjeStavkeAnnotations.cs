using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bex.Models
{
    [MetadataType(typeof(TrebovanjeStavkeMetadata))]

    public partial class TrebovanjeStavke
    {
        public class TrebovanjeStavkeMetadata
        {
            public int? IdStavkeTrebovanja { get; set; }
            [ForeignKey("Trebovanje")]
            public int? TrebovanjeId { get; set; }
            [ForeignKey("Artikal")]
            public int? ArtikalId { get; set; }
            public int? KolicinaTrazena { get; set; }
            public int? KolicinaIzdata { get; set; }

            public object Trebovanje { get; set; }
            public object Artikal { get; set; }

            private TrebovanjeStavkeMetadata() { }

        }
    }

}