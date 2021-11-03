using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bex.Models
{
    [MetadataType(typeof(VpPopravkeMetadata))]
    public partial class VpPopravke
    {
        public class VpPopravkeMetadata
        {

            public int Id { get; set; }
            [ForeignKey("VozniPark")]
            public int VpId { get; set; }
            [ForeignKey("VpPopravkeTip")]
            public int TipId { get; set; }
            public DateTime Datum { get; set; }
            public string Delovi { get; set; }
            public string BrojRacuna { get; set; }
            public string Napomena { get; set; }

            public object VozniPark { get; set; }
            public object VpPopravkeTip { get; set; }

            private VpPopravkeMetadata() { }
        }
    }
}
