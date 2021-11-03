namespace Bex.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    [MetadataType(typeof(SkenPracenjeMetadata))]
    public  partial class SkenPracenje
    {
        public class SkenPracenjeMetadata
        {
            public int Id { get; set; }
            public int? PosiljkaId { get; set; }
            public int? RegionUtovaraId { get; set; }
            public int? RegionIsporukeId { get; set; }
            public string BarKod { get; set; }
            public bool UtovarLinijskogZaCM { get; set; }
            public bool IstovarLinijskogUcm { get; set; }
            public bool UtovarLinijskogIzCM { get; set; }
            public bool IstovarLinijskogIzCM { get; set; }
            private SkenPracenjeMetadata() { }
        }
    }
}
