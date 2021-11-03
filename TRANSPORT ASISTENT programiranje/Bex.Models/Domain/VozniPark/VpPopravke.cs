using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bex.Models
{
    public partial class VpPopravke
    {

        public int Id { get; set; }
        public int VpId { get; set; }
        public int TipId { get; set; }
        public DateTime Datum { get; set; }
        public string Delovi { get; set; }
        public string BrojRacuna { get; set; }
        public string Napomena { get; set; }

        public virtual VozniPark VozniPark { get; set; }
        public virtual VpPopravkeTip VpPopravkeTip { get; set; }

    }
}
