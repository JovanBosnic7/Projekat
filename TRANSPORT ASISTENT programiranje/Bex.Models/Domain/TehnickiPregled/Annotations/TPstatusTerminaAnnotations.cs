using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Bex.Models
{
    [MetadataType(typeof(TPstatusTerminaMetadata))]
    public partial class TPstatusTermina
    {
        public class TPstatusTerminaMetadata
        {
           
            public int IdStatusa { get; set; }
            public string NazivStatusa { get; set; }

            private TPstatusTerminaMetadata() { }
        }
    }
}