using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bex.Models
{
    [MetadataType(typeof(VpTrosakMetadata))]
    public partial class VpTrosak
    {
        public class VpTrosakMetadata
        {
            public int Id { get; set; }
            public DateTime? DatumUnosa { get; set; }
            [ForeignKey("VozniPark")]
            public int? VpId { get; set; }
            [ForeignKey("VozniParkAlarm")]
            public int? AlarmId { get; set; }
            [ForeignKey("VpPopravke")]
            public int? PopravkaId { get; set; }
            public decimal? CenaDinara { get; set; }
            public decimal? CenaEura { get; set; }

            public object VozniParkAlarm { get; set; }
            public object VozniPark { get; set; }
            public object VpPopravke { get; set; }
            private VpTrosakMetadata() { }
        }
    }
}
