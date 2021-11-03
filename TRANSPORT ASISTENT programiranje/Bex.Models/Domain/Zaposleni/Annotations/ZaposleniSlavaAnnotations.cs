using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Bex.Models
{
    [MetadataType(typeof(ZaposleniSlavaMetadata))]
    public partial class ZaposleniSlava
    {
        public class ZaposleniSlavaMetadata
        {
           
            public int Id { get; set; }
            public string Naziv { get; set; }

            private ZaposleniSlavaMetadata() { }
        }
    }
}