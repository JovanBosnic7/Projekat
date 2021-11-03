using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Bex.Models
{
    [MetadataType(typeof(FilterSpisakKolonaMetadata))]
    public partial class FilterSpisakKolona
    {
        public class FilterSpisakKolonaMetadata
        {
            public int Id { get; set; }
            public string Naziv { get; set; }
            public int Filter { get; set; }
            public bool Selektovan { get; set; }

            private FilterSpisakKolonaMetadata() { }
        }
    }
}