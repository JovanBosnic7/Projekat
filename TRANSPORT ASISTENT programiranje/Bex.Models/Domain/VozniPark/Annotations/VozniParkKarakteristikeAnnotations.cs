using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Bex.Models
{
    [MetadataType(typeof(VozniParkKarakteristikeMetadata))]
    public partial class VozniParkKarakteristike
    {
        public class VozniParkKarakteristikeMetadata
        {
           
            public int Id { get; set; }

            public string Naziv { get; set; }

            private VozniParkKarakteristikeMetadata() { }
        }
    }
}