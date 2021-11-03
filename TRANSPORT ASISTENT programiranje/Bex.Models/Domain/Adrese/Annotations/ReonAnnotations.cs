using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bex.Models
{
    [MetadataType(typeof(ReonMetadata))]
    public partial class Reon
    {
        public class ReonMetadata
        {
            
            public int Id { get; set; }

            public string OznReona { get; set; }
            [ForeignKey("Region")]
            public int? RegionId { get; set; }

            public string NazivReona { get; set; }

            public bool? Storno { get; set; }

            public string BarKodReona { get; set; }
            [ForeignKey("ReonTip")]
            public int? Tip { get; set; }

            public int? KmDoReona { get; set; }

            public int? OptimalnaKilometraza { get; set; }

            public object Region { get; set; }
            public object ReonTip { get; set; }

            private ReonMetadata() { }
        }
   
    }
}