using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Bex.Models
{
    [MetadataType(typeof(PosiljkaUslugaTipMetadata))]
    public partial class PosiljkaUslugaTip
    {
        public class PosiljkaUslugaTipMetadata
        {
           
            public int Id { get; set; }
            public string Naziv { get; set; }

            private PosiljkaUslugaTipMetadata() { }
        }
    }
}