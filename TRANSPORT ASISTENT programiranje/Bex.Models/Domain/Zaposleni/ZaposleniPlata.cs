using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bex.Models
{
    public partial class ZaposleniPlata
    {
 
        public int Id { get; set; }
        public int ZaposleniId { get; set; }
        public int FirmaPlataId { get; set; }
        public decimal PlataKeoficijent { get; set; }
        public bool PlataNaRacun { get; set; }
        public decimal PlataDoprinosi { get; set; }
        public decimal PlataBruto { get; set; }
        public bool PlataMinimalac { get; set; }
        public bool PlataKes { get; set; }
        public decimal PlataNeto { get; set; }
        public int PlataUkupno { get; set; }
        public string PlataNapomena { get; set; }
        public virtual Zaposleni Zaposleni { get; set; }

    }
}
