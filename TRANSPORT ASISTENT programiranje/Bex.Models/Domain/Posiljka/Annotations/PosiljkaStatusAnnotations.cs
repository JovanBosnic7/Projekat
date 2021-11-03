using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Bex.Models
{
    [MetadataType(typeof(PosiljkaStatusMetadata))]
    public partial class PosiljkaStatus
    {
        public class PosiljkaStatusMetadata
        {
           
            public int Id { get; set; }
            public string Naziv { get; set; }
            

            private PosiljkaStatusMetadata() { }
        }
    }
}