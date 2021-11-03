using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bex.Models
{
    [MetadataType(typeof(StatistikaPorekloPosiljkeMetadata))]

    public partial class StatistikaPorekloPosiljke
    {
        public class StatistikaPorekloPosiljkeMetadata
        {
            public int Id { get; set; }
            public DateTime Datum { get; set; }
            public int Sat { get; set; }
            public int Poreklo { get; set; }
            public int? UkupnoPosiljki { get; set; }

            private StatistikaPorekloPosiljkeMetadata() { }

        }
    }

}