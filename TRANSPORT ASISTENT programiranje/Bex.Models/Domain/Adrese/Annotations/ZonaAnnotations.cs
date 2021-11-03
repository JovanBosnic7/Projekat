using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bex.Models
{
    [MetadataType(typeof(ZonaMetadata))]
    public partial class Zona
    {
        public class ZonaMetadata
        {
           
            public int Id { get; set; }
            public string NazivZone { get; set; }
            [ForeignKey("ZonaTip")]
            public int Tip { get; set; }
            [ForeignKey("ZonaPodTip")]
            public int PodTip { get; set; }
            public int VoziloRegionId { get; set; }
            public int RegionGrupaId { get; set; }
            public string Opis { get; set; }
            public bool Aktivan { get; set; }

            public object ZonaTip { get; set; }
            public object ZonaPodTip { get; set; }

            

            private ZonaMetadata() { }
        }
    }
}