using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Bex.Models
{
    [MetadataType(typeof(ZaposleniProgramZaposlenjaMetadata))]
    public partial class ZaposleniProgramZaposlenja
    {
        public class ZaposleniProgramZaposlenjaMetadata
        {
           
            public int Id { get; set; }
            public string Naziv { get; set; }

            private ZaposleniProgramZaposlenjaMetadata() { }
        }
    }
}