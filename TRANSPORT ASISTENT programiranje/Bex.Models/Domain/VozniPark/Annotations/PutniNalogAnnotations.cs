using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bex.Models
{
    [MetadataType(typeof(PutniNalogMetadata))]
    public partial class PutniNalog
    {
        public class PutniNalogMetadata
        {

            public int Id { get; set; }
            [ForeignKey("VozniPark")]
            public int VpId { get; set; }
            [ForeignKey("Zaposleni")]
            public int? VozacId { get; set; }
            public string Relacija { get; set; }
            public DateTime? DatumStart { get; set; }
            public DateTime? DatumStop { get; set; }
            public TimeSpan? VremeStart { get; set; }
            public TimeSpan? VremeStop { get; set; }
            public int? KmStart { get; set; }
            public int? KmStop { get; set; }
            [Range(0,int.MaxValue,ErrorMessage ="Ukupna kilometraža ne može biti manja od 0. Proverite Km stop.")]
            public int? KmUkupno { get; set; }
            public string Napomena { get; set; }
            public bool Storno { get; set; }
            public int? BrojSipanja { get; set; }
            public decimal? Litara { get; set; }


            public object  VozniPark { get; set; }
            public object  Zaposleni { get; set; }

            private PutniNalogMetadata() { }
        }
    }
}
