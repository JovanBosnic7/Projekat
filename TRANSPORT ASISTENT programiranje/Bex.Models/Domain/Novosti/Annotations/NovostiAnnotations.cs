using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Bex.Models
{
    [MetadataType(typeof(NovostiMetadata))]
    public partial class Novosti
    {
        public class NovostiMetadata
        {
            public int Id { get; set; }
            public DateTime Datum { get; set; }
            public string Naslov { get; set; }
            public string Tekst { get; set; }
            public string Link { get; set; }
            public bool Aktivno { get; set; }

            private NovostiMetadata() { }
        }
    }
}