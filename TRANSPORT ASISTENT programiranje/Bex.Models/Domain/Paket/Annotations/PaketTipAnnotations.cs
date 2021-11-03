using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Bex.Models
{
    [MetadataType(typeof(PaketTipMetadata))]

    public partial class PaketTip
    {
        public class PaketTipMetadata
        {
            public int Id { get; set; }

            public string Naziv { get; set; }

            private PaketTipMetadata() { }

        }
    }

}