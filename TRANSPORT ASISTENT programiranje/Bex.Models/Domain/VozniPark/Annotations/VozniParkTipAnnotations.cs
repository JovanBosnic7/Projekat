using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Bex.Models
{
    [MetadataType(typeof(VozniParkTipMetadata))]
    public partial class VozniParkTip
    {
        public class VozniParkTipMetadata
        {
           
            public int Id { get; set; }

            public string Opis { get; set; }

            private VozniParkTipMetadata() { }
        }
    }
}