using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Bex.Models
{
    [MetadataType(typeof(VozniParkStatusMetadata))]
    public partial class VozniParkStatus
    {
        public class VozniParkStatusMetadata
        {
           
            public int Id { get; set; }
            public string NazivStatusa { get; set; }

            private VozniParkStatusMetadata() { }
        }
    }
}