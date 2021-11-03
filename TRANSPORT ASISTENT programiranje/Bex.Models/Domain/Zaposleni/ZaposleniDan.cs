using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bex.Models
{
    public partial class ZaposleniDan
    {
 
        public int Id { get; set; }
        public int? ZaposleniID { get; set; }
        public DateTime? Datum { get; set; }
        public TimeSpan? VremeOd { get; set; }
        public TimeSpan? VremeDo { get; set; }
        public int? StatusId { get; set; }
        public DateTime? DatumVremeUnosa { get; set; }
        public int? UserUnosId { get; set; }
        public int? ObavioStopovaP { get; set; }
        public int? ObavioStopovaD { get; set; }
        public int? ObavioStopovaN { get; set; }
        public int? RadioReona { get; set; }
        public bool Storno { get; set; }
        public virtual Zaposleni Zaposleni { get; set; }
        public virtual ZaposleniDanStatus ZaposleniDanStatus { get; set; }
    }
}
