using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Bex.Models
{
    [MetadataType(typeof(PosiljkaNapomenaTipMetadata))]
    public partial class PosiljkaNapomenaTip
    {
        public class PosiljkaNapomenaTipMetadata
        {
           
            public int Id { get; set; }
            public string Naziv { get; set; }

            private PosiljkaNapomenaTipMetadata() { }
        }
    }
}