using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bex.Models
{
    [MetadataType(typeof(PrijavaPodTipMetadata))]
    public partial class PrijavaPodTip
    {
        public class PrijavaPodTipMetadata
        {

            public int Id { get; set; }
            [ForeignKey("PrijavaTip")]
            public int? TipId { get; set; }
            public string Naziv { get; set; }
            public bool? Storno { get; set; }

            public object PrijavaTip { get; set; }
            private PrijavaPodTipMetadata() { }
        }
    }
}