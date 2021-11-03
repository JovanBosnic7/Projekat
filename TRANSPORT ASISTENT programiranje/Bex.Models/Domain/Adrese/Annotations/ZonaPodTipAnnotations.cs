using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bex.Models
{
    [MetadataType(typeof(ZonaPodTipMetadata))]
    public partial class ZonaPodTip
    {
        public class ZonaPodTipMetadata
        {
           
            public int Id { get; set; }
            [ForeignKey("ZonaTip")]
            public int TipId { get; set; }
            public string Opis { get; set; }

            public object ZonaTip { get; set; }

            private ZonaPodTipMetadata() { }
        }
    }
}