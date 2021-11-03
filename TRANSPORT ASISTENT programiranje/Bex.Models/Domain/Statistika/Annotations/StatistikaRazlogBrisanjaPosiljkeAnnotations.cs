using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bex.Models
{
    [MetadataType(typeof(StatistikaRazlogBrisanjaMetadata))]

    public partial class StatistikaRazlogBrisanjaPosiljke
    {
        public class StatistikaRazlogBrisanjaMetadata
        {
            public int Id { get; set; }
            public DateTime Datum { get; set; }
            public int Sat { get; set; }
            public int RazlogBrisanja { get; set; }
            public int? UkupnoPosiljki { get; set; }

            private StatistikaRazlogBrisanjaMetadata() { }

        }
    }

}