using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bex.Models
{
    [MetadataType(typeof(ZaposleniDanMetadata))]
    public partial class ZaposleniDan
    {
        public class ZaposleniDanMetadata
        {
            public int Id { get; set; }
            [ForeignKey("Zaposleni")]
            public int? ZaposleniID { get; set; }
            public DateTime? Datum { get; set; }
            public TimeSpan? VremeOd { get; set; }
            public TimeSpan? VremeDo { get; set; }
            [ForeignKey("ZaposleniDanStatus")]
            public int? StatusId { get; set; }
            public DateTime? DatumVremeUnosa { get; set; }
            public int? UserUnosId { get; set; }
            public int? ObavioStopovaP { get; set; }
            public int? ObavioStopovaD { get; set; }
            public int? ObavioStopovaN { get; set; }
            public int? RadioReona { get; set; }
            public bool Storno { get; set; }
           
            public object Zaposleni { get; set; }
            public object ZaposleniDanStatus { get; set; }

            private ZaposleniDanMetadata() { }
        }
    }
}
