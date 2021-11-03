using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bex.Models
{
    [MetadataType(typeof(PosiljkaUslugaMetadata))]
    public partial class PosiljkaUsluga
    {
        public class PosiljkaUslugaMetadata
        {

            public int Id { get; set; }
            [ForeignKey("Posiljka")]
            public int PosiljkaId { get; set; }
            [ForeignKey("PosiljkaUslugaTip")]
            public int TipUslugeId { get; set; }
            public decimal Komada { get; set; }
            public decimal CenaUsluge { get; set; }
            public string Napomena { get; set; }

            public object Posiljka { get; set; }
            public object PosiljkaUslugaTip { get; set; }

            private PosiljkaUslugaMetadata() { }
        }
    }
}