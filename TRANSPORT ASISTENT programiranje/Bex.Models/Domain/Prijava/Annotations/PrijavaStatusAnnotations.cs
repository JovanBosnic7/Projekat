using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bex.Models
{
    [MetadataType(typeof(PrijavaStatusMetadata))]
    public partial class PrijavaStatus
    {
        public class PrijavaStatusMetadata
        {

            public int Id { get; set; }
            [ForeignKey("PrijavaTip")]
            public int? TipId { get; set; }
            public string Naziv { get; set; }

            public object PrijavaTip { get; set; }
            private PrijavaStatusMetadata() { }
        }
    }
}