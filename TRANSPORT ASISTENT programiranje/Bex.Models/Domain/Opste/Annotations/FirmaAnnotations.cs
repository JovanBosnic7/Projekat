using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Bex.Models
{
    [MetadataType(typeof(FirmaMetadata))]
    public partial class Firma
    {
        public class FirmaMetadata
        {
           
            public int Id { get; set; }

            public string Naziv { get; set; }

            public string Adresa { get; set; }
            public int? Delatnost { get; set; }
            public string MaticniBroj { get; set; }
            public string Pib { get; set; }
            public bool Storno { get; set; }
            public int? FirmaIdVP { get; set; }

            private FirmaMetadata() { }
        }
    }
}