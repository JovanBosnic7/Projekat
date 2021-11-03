using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bex.Models
{
    [MetadataType(typeof(PosiljkaObrisanaMetadata))]
    public partial class PosiljkaObrisana
    {
        public class PosiljkaObrisanaMetadata
        {

            public int Id { get; set; }
            public int PosiljkaId { get; set; }
            public int NapomenaPodTipId { get; set; }
            public int ObrisaoUserId { get; set; }
            public bool IzKlijentskog { get; set; }
            public DateTime DatumBrisanja { get; set; }

            public object Posiljka { get; set; }
            public object NapomenaPosiljkaPodTip { get; set; }
            public object UserObrisao { get; set; }

            private PosiljkaObrisanaMetadata() { }
        }
    }
}