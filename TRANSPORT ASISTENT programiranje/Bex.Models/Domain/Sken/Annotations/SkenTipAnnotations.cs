namespace Bex.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    [MetadataType(typeof(SkenTipMetadata))]

    public  partial class SkenTip
    {
        public class SkenTipMetadata
        {
            public int Id { get; set; }
            public string Naziv { get; set; }
            private SkenTipMetadata() { }
        }
  
    }
}
