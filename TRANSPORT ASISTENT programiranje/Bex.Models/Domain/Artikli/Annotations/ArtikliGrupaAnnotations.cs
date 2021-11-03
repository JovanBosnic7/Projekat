using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bex.Models
{
    [MetadataType(typeof(ArtikliGrupaMetadata))]

    public partial class ArtikliGrupa
    {
        public class ArtikliGrupaMetadata
        {
            public int Id { get; set; }
            public string Naziv { get; set; }
            public bool? Storno { get; set; }

            private ArtikliGrupaMetadata() { }

        }
    }

}