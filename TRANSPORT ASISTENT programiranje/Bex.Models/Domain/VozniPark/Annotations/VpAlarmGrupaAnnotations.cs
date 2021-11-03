using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Bex.Models
{
    [MetadataType(typeof(VpAlarmGrupaMetadata))]
    public partial class VpAlarmGrupa
    {
        public class VpAlarmGrupaMetadata
        {
            public int Id { get; set; }
            public string Naziv { get; set; }

            private VpAlarmGrupaMetadata() { }
        }
    }
}