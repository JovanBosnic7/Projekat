using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bex.Models
{
    [MetadataType(typeof(PosiljkaPlacanjeMetadata))]
    public partial class PosiljkaPlacanje
    {
        public class PosiljkaPlacanjeMetadata
        {

            public int Id { get; set; }
            [ForeignKey("Posiljka")]
            public int PosiljkaId { get; set; }
            [ForeignKey("PosiljkaPlacanjeTip")]
            public int PlacanjeId { get; set; }
            public int PlatilacId { get; set; }
            public int Iznos { get; set; }
            public object Posiljka { get; set; }
            public object PosiljkaPlacanjeTip { get; set; }

            private PosiljkaPlacanjeMetadata() { }
        }
    }
}