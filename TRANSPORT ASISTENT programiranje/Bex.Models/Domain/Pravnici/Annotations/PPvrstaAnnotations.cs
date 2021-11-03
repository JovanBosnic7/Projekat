using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Bex.Models
{
    [MetadataType(typeof(PPvrstaMetadata))]

    public partial class PPvrsta
    {
        public class PPvrstaMetadata
        {
            public int Id { get; set; }
            public string Naziv { get; set; }
            public int Sort { get; set; }
            private PPvrstaMetadata() { }

        }
    }

}