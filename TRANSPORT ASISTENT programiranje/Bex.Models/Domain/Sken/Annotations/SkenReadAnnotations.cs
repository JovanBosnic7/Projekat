namespace Bex.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    [MetadataType(typeof(SkenReadtMetadata))]
    public  partial class SkenRead
    {
        public class SkenReadtMetadata
        {
            public int Id { get; set; }
            public int? PosiljkaId { get; set; }
            public string BarKod { get; set; }
            public int? RbPaketa { get; set; }
            public int? TipPaketaId { get; set; }
            public DateTime? DatumSkena { get; set; }
            public DateTime? VremeSkena { get; set; }
            public int? ReonId { get; set; }
            public int? RegionId { get; set; }
            public int? ZonaId { get; set; }
            public int? UserId { get; set; }
            public DateTime? VremePDA { get; set; }
            public string NazivSkena { get; set; }

            public object User { get; set; }
            public object Reon { get; set; }
            public object Region { get; set; }
            public object Zona { get; set; }
            public object Posiljka { get; set; }

            private SkenReadtMetadata() { }
        }

    }
}
