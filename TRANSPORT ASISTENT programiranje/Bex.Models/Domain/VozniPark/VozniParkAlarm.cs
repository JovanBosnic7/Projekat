using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bex.Models
{
    public partial class VozniParkAlarm
    {
 
        public int Id { get; set; }
        public int? VpId { get; set; }
        public int? ZaposleniId { get; set; }
        public int? VpAlarmTip { get; set; }
        public DateTime? Datum { get; set; }
        public int? Km { get; set; }
        public DateTime? DatumIsteka { get; set; }
        public int? KmIsteka { get; set; }
        public DateTime? DatumAlarma { get; set; }
        public int? KmAlarma { get; set; }
        public string Napomena { get; set; }

      
        public virtual VpAlarmTip VpAlarmTipAlarm { get; set; }
        public virtual VozniPark VozniPark { get; set; }
        public virtual Zaposleni Zaposleni { get; set; }

    }
}
