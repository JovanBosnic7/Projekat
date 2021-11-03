using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Bex.Models
{
    [MetadataType(typeof(ZonaTipMetadata))]
    public partial class ZonaTip
    {
        public class ZonaTipMetadata
        {
           
            public int Id { get; set; }
            public string Opis { get; set; }

            private ZonaTipMetadata() { }
        }
    }
}