using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bex.Models
{
    [MetadataType(typeof(DimenzijeNavMetadata))]

    public partial class DimenzijeNav
    {
        public class DimenzijeNavMetadata
        {
            public int Id { get; set; }
            public int? TipId { get; set; }
            public string NavCode { get; set; }
            public string Naziv { get; set; }
            public int? TipRedaId { get; set; }
            public bool? Storno { get; set; }

            private DimenzijeNavMetadata() { }

        }
    }

}