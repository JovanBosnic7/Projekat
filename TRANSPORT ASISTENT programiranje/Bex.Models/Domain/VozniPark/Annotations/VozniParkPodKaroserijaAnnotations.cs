using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Bex.Models
{
    [MetadataType(typeof(VozniParkPodKaroserijaMetadata))]
    public partial class VozniParkPodKaroserija
    {
        public class VozniParkPodKaroserijaMetadata
        {
           
            public int Id { get; set; }
            public int KaroserijaId { get; set; }
            public string NazivPodKaroserije { get; set; }

            private VozniParkPodKaroserijaMetadata() { }
        }
    }
}