using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Bex.Models
{
    [MetadataType(typeof(PosiljkaVrstaMetadata))]
    public partial class PosiljkaVrsta
    {
        public class PosiljkaVrstaMetadata
        {
           
            public int Id { get; set; }
            public string NazivVrste { get; set; }
            public bool Nevazece { get; set; }

            private PosiljkaVrstaMetadata() { }
        }
    }
}