using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Bex.Models
{
    [MetadataType(typeof(VozniParkMarkaMetadata))]
    public partial class VozniParkMarka
    {
        public class VozniParkMarkaMetadata
        {
           
            public int Id { get; set; }

            public string NazivMarke{ get; set; }


            private VozniParkMarkaMetadata() { }
        }
    }
}