using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bex.Models
{
    [MetadataType(typeof(VozniParkAlarmMetadata))]
    public partial class VozniParkAlarm
    {
        public class VozniParkAlarmMetadata
        {

            public int Id { get; set; }
            [ForeignKey("VozniPark")]
            public int? VpId { get; set; }
            [ForeignKey("Zaposleni")]
            public int? ZaposleniId { get; set; }
            [ForeignKey("VpAlarmTipAlarm")]
            public int? VpAlarmTip { get; set; }
            public DateTime? Datum { get; set; }
            public int? Km { get; set; }
            public DateTime? DatumIsteka { get; set; }
            public int? KmIsteka { get; set; }
            public DateTime? DatumAlarma { get; set; }
            public int? KmAlarma { get; set; }
            public string Napomena { get; set; }

            public object VpAlarmTipAlarm { get; set; }
            public object VozniPark { get; set; }
            public object Zaposleni { get; set; }

            private VozniParkAlarmMetadata() { }
        }
    }
}
