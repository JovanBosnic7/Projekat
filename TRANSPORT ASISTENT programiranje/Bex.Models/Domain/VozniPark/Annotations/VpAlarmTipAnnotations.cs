using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bex.Models
{
    [MetadataType(typeof(VpAlarmTipMetadata))]
    public partial class VpAlarmTip
    {
        public class VpAlarmTipMetadata
        {
            public int Id { get; set; }
            public string NazivAlarma { get; set; }
            [ForeignKey("VpAlarmGrupa")]
            public int GrupaId { get; set; }
            public int KmDoIsteka { get; set; }
            public int DanaDoIsteka { get; set; }
            public int DanaDoAlarma { get; set; }
            public int Sort { get; set; }


            public object VpAlarmGrupa { get; set; }
            private VpAlarmTipMetadata() { }
        }
    }
}