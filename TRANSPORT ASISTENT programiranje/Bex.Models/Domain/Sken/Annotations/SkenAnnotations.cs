namespace Bex.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    [MetadataType(typeof(SkenMetadata))]
    public  partial class Sken
    {
        public class SkenMetadata
        {
            public int Id { get; set; }
            public int? SkenStartId { get; set; }
            public string BarKod { get; set; }
            public DateTime? DatumUpisaNaServer { get; set; }
            public DateTime? DatumSkeniranjaNaPDA { get; set; }
            public object SkenStart { get; set; }

            private SkenMetadata() { }
        }

    }
}
