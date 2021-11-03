using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bex.Models
{
    [MetadataType(typeof(MagacinSpisakMetadata))]

    public partial class MagacinSpisak
    {
        public class MagacinSpisakMetadata
        {
            public int Id { get; set; }
            public string Naziv { get; set; }
            public int? TipMagacina { get; set; }

            private MagacinSpisakMetadata() { }

        }
    }

}