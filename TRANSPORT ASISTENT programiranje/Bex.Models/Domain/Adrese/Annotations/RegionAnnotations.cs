using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bex.Models
{
    [MetadataType(typeof(RegionMetadata))]
    public partial class Region
    {
        public class RegionMetadata
        {
            
            public int Id { get; set; }

            public string NazivSkraceni { get; set; }

            public string NazivRegiona { get; set; }

            public string OznRegion { get; set; }

            public int? Tip { get; set; }

            public string BarKodRegiona { get; set; }

            public bool? Storno { get; set; }

            public bool? DeoGrupeRegiona { get; set; }

            public int? RegionGrupaId { get; set; }

            public bool? ZabranjenoProvlacenjeZadataka { get; set; }

            public int? Sort { get; set; }
            [ForeignKey("DimenzijeNav")]
            public int? DimId { get; set; }

            public DateTime? DI { get; set; }

            public DateTime? DU { get; set; }

            public object DimenzijeNav { get; set; }

            private RegionMetadata() { }
        }
    }
}