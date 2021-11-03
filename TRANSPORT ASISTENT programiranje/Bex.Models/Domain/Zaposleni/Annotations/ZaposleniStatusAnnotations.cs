using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Bex.Models
{
    [MetadataType(typeof(ZaposleniStatusMetadata))]
    public partial class ZaposleniStatus
    {
        public class ZaposleniStatusMetadata
        {
           
            public int Id { get; set; }

            public string StatusRadnika { get; set; }

            private ZaposleniStatusMetadata() { }
        }
    }
}