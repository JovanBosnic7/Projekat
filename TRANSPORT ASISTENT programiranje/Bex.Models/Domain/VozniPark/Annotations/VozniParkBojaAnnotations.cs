using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Bex.Models
{
    [MetadataType(typeof(VozniParkBojaMetadata))]
    public partial class VozniParkBoja
    {
        public class VozniParkBojaMetadata
        {
           
            public int Id { get; set; }
            public string SifraBoje { get; set; }
            public string NazivBoje { get; set; }

            private VozniParkBojaMetadata() { }
        }
    }
}