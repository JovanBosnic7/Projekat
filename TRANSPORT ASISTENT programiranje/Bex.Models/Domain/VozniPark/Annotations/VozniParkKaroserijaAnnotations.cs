using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Bex.Models
{
    [MetadataType(typeof(VozniParkKaroserijaMetadata))]
    public partial class VozniParkKaroserija
    {
        public class VozniParkKaroserijaMetadata
        {
           
            public int Id { get; set; }
            public int KategorijaId { get; set; }
            public string NazivKaroserije { get; set; }

            private VozniParkKaroserijaMetadata() { }
        }
    }
}