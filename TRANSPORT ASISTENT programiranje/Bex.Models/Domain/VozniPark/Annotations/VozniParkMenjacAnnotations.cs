using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Bex.Models
{
    [MetadataType(typeof(VozniParkMenjacMetadata))]
    public partial class VozniParkMenjac
    {
        public class VozniParkMenjacMetadata
        {
           
            public int Id { get; set; }

            public string NazivMenjaca { get; set; }

            private VozniParkMenjacMetadata() { }
        }
    }
}