using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bex.Models
{
    [MetadataType(typeof(PaketMetadata))]
    public partial class Paket
    {
        public class PaketMetadata
        {

            public int Id { get; set; }
            public string BarKod { get; set; }
            [ForeignKey("Posiljka")]
            public int PosiljkaId { get; set; }
            public int PaketRB { get; set; }
            public int TipPaketa { get; set; }
            public decimal Masa { get; set; }
            [ForeignKey("Zona")]
            public int ZonaId { get; set; }
            public bool Storno { get; set; }

            public object Posiljka { get; set; }
            public object Zona { get; set; }

            private PaketMetadata() { }
        }
    }
}