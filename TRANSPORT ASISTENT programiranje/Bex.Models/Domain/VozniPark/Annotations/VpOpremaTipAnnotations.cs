using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bex.Models
{
    [MetadataType(typeof(VpOpremaTipMetadata))]
    public partial class VpOpremaTip
    {
        public class VpOpremaTipMetadata
        {
            public int Id { get; set; }
            public string Naziv { get; set; }
            [ForeignKey("VpOpremaGrupa")]
            public int GrupaId { get; set; }
            public int Sort { get; set; }
            public bool Storno { get; set; }

            public object VpOpremaGrupa { get; set; }
            private VpOpremaTipMetadata() { }
        }
    }
}