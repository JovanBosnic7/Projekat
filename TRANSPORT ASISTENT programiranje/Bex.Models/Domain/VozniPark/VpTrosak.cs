using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bex.Models
{
    public partial class VpTrosak
    {

        public int Id { get; set; }
        public DateTime? DatumUnosa { get; set; }
        public int? VpId { get; set; }
        public int? AlarmId { get; set; }
        public int? PopravkaId { get; set; }
        public decimal? CenaDinara { get; set; }
        public decimal? CenaEura { get; set; }

        public virtual VozniParkAlarm VozniParkAlarm { get; set; }
        public virtual VozniPark VozniPark { get; set; }
        public virtual VpPopravke VpPopravke { get; set; }
    }
}
