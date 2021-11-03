using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bex.Models
{
    [MetadataType(typeof(GorivoTocenjeMetadata))]
    public partial class GorivoTocenje
    {
        public class GorivoTocenjeMetadata
        {

            public int Id { get; set; }
            [ForeignKey("PutniNalog")]
            public int PutniNalogId { get; set; }
            public DateTime? Datum { get; set; }
            public TimeSpan? Vreme { get; set; }
            public decimal Litara { get; set; }
            public decimal Cena { get; set; }
            public decimal Iznos { get; set; }
            public int Km { get; set; }
            public string Napomena { get; set; }
            public bool Storno { get; set; }
            [ForeignKey("GorivoPumpa")]
            public int? PumpaId { get; set; }
            public int PresaoKmOdPrethodnogSipanja { get; set; }
            public decimal ProsekOdPrethodnog { get; set; }

            public object PutniNalog { get; set; } 
            public object GorivoPumpa { get; set; }

            private GorivoTocenjeMetadata() { }
        }
    }
}
