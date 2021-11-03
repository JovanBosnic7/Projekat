using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Bex.Models
{
    [MetadataType(typeof(ZaposleniOsnovOsiguranjaMetadata))]
    public partial class ZaposleniOsnovOsiguranja
    {
        public class ZaposleniOsnovOsiguranjaMetadata
        {
           
            public int Id { get; set; }

            public string Naziv { get; set; }

            private ZaposleniOsnovOsiguranjaMetadata() { }
        }
    }
}