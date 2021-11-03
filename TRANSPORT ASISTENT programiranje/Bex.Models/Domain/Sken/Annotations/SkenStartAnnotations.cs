namespace Bex.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    [MetadataType(typeof(SkenStartMetadata))]
    public  partial class SkenStart
    {
        public class SkenStartMetadata
        {
            public int Id { get; set; }
            public int? UserId { get; set; }
            public int? TipId { get; set; }
            public int? ReonId { get; set; }
            public int? RegionId { get; set; }
            public int? LogovanjeId { get; set; }
            public int? ZonaId { get; set; }
            public DateTime? Datum { get; set; }
            public DateTime? VremeStart { get; set; }
            public DateTime? VremeStop { get; set; }
            public int? PoslatoSkenova { get; set; }
            public int? UpisanoSkenova { get; set; }

            public object User { get; set; }
            public object SkenTip { get; set; }
            public object Reon { get; set; }
            public object Region { get; set; }
            public object Zona { get; set; }
            private SkenStartMetadata() { }
        }

    }
}
