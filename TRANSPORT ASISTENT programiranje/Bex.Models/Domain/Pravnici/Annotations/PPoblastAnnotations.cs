using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Bex.Models
{
    [MetadataType(typeof(PPoblastMetadata))]

    public partial class PPoblast
    {
        public class PPoblastMetadata
        {
            public int Id { get; set; }
            public string Naziv { get; set; }
            public bool Tuzeni { get; set; }
            public bool Tuzilac { get; set; }

            private PPoblastMetadata() { }

        }
    }

}