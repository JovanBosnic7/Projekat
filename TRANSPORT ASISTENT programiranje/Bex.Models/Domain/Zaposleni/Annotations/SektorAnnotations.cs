using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Bex.Models
{
    [MetadataType(typeof(SektorMetadata))]
    public partial class Sektor
    {
        public class SektorMetadata
        {
           
            public int Id { get; set; }

            public string NazivSektora { get; set; }

            private SektorMetadata() { }
        }
    }
}