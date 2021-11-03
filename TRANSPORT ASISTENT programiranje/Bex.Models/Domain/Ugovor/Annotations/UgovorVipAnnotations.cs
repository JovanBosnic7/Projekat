using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bex.Models
{
    [MetadataType(typeof(UgovorVipMetadata))]
    public partial class UgovorVip
    {
        public class UgovorVipMetadata
        {
            public int Id { get; set; }
            public int UgovorId { get; set; }
            public bool VipD { get; set; }
            public string VipDvremeDo { get; set; }
            public DateTime VipDdatumDo { get; set; }
            public bool VipT { get; set; }
            public bool VipN { get; set; }
            public DateTime DatumUnosa { get; set; }

            public object Ugovor { get; set; }
            private UgovorVipMetadata() { }
        }
    }
}