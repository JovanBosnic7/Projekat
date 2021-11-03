using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bex.Models
{
    public partial class PutniNalog
    {
 
        public int Id { get; set; }
        public int VpId { get; set; }
        public int? VozacId { get; set; }
        public string Relacija { get; set; }
        public DateTime? DatumStart { get; set; }
        public DateTime? DatumStop { get; set; }
        public TimeSpan? VremeStart { get; set; }
        public TimeSpan? VremeStop { get; set; }
        public int? KmStart { get; set; }     
        public int? KmStop { get; set; }
        public int? KmUkupno { get; set; }
        public string Napomena { get; set; }
        public bool Storno { get; set; }
        public int? BrojSipanja { get; set; }
        public decimal? Litara { get; set; }

        public virtual VozniPark VozniPark { get; set; }
        public virtual Zaposleni Zaposleni { get; set; }

    }
}
