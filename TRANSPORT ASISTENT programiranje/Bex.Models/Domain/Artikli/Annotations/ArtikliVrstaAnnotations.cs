using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bex.Models
{
    [MetadataType(typeof(ArtikliVrsteMetadata))]

    public partial class ArtikliVrsta
    {
        public class ArtikliVrsteMetadata
        {
            public int Id { get; set; }
            public string OznakaVrste { get; set; }
            public string NazivVrste { get; set; }
            public string NazivVrsteNav { get; set; }
            public bool? Storno { get; set; }

            private ArtikliVrsteMetadata() { }

        }
    }

}