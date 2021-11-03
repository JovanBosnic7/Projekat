using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bex.Models
{
    [MetadataType(typeof(PrijavaTipMetadata))]
    public partial class PrijavaTip
    {
        public class PrijavaTipMetadata
        {

            public int Id { get; set; }
            public string Naziv { get; set; }
            private PrijavaTipMetadata() { }
        }
    }
}