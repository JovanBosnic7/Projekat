using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bex.Models
{
    [MetadataType(typeof(VpOpremaPodTipMetadata))]
    public partial class VpOpremaPodTip
    {
        public class VpOpremaPodTipMetadata
        {
            public int Id { get; set; }
            public string Naziv { get; set; }
            [ForeignKey("VpOpremaTip")]
            public int TipId { get; set; }
            public int Sort { get; set; }
            public bool Storno { get; set; }

            public object VpOpremaTip { get; set; }

            private VpOpremaPodTipMetadata() { }
        }
    }
}