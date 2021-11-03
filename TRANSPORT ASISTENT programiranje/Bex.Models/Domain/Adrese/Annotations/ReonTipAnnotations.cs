using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Bex.Models
{
    [MetadataType(typeof(ReonTipMetadata))]
    public partial class ReonTip
    {
        public class ReonTipMetadata
        {
            
            public int Id { get; set; }

            public string Opis { get; set; }

            public string OpisPom { get; set; }
           
            private ReonTipMetadata() { }
        }
   
    }
}