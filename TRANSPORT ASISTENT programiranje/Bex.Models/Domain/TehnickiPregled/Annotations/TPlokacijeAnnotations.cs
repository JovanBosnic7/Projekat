using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bex.Models
{
    [MetadataType(typeof(TPlokacijeMetadata))]

    public partial class TPlokacije
    {
        public class TPlokacijeMetadata
        {
            public int IdLokacije { get; set; }
            public string NazivLokacije { get; set; }
          

            private TPlokacijeMetadata() { }

        }
    }

}