using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Bex.Models
{
    [MetadataType(typeof(GorivoMetadata))]
    public partial class Gorivo
    {
        public class GorivoMetadata
        {
           
            public int Id { get; set; }
            public string SifraGoriva { get; set; }
            public string NazivGoriva { get; set; }

            private GorivoMetadata() { }
        }
    }
}