using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Bex.Models
{
    [MetadataType(typeof(VozniParkKategorijaMetadata))]
    public partial class VozniParkKategorija
    {
        public class VozniParkKategorijaMetadata
        {
           
            public int Id { get; set; }

            public string KategorijaNaziv { get; set; }

            public string NazivPodkategorije { get; set; }
            public int Sort { get; set; }

            private VozniParkKategorijaMetadata() { }
        }
    }
}