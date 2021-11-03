using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Bex.Models
{
    [MetadataType(typeof(VpOpremaGrupaMetadata))]
    public partial class VpOpremaGrupa
    {
        public class VpOpremaGrupaMetadata
        {
            public int Id { get; set; }

            public string Naziv { get; set; }
            public int Sort { get; set; }
            public bool Storno { get; set; }

            private VpOpremaGrupaMetadata() { }
        }
    }
}