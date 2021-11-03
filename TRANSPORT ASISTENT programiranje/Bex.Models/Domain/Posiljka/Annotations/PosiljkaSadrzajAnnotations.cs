using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Bex.Models
{
    [MetadataType(typeof(PosiljkaSadrzajMetadata))]
    public partial class PosiljkaSadrzaj
    {
        public class PosiljkaSadrzajMetadata
        {
           
            public int Id { get; set; }
            public string Naziv { get; set; }
            public bool Storno { get; set; }

            private PosiljkaSadrzajMetadata() { }
        }
    }
}