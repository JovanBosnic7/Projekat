using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bex.Models
{
    [MetadataType(typeof(StatistikaDanMetadata))]

    public partial class StatistikaDan
    {
        public class StatistikaDanMetadata
        {
            public int Id { get; set; }
            public DateTime Datum { get; set; }
            public int EvidentiranoPre5danaPosiljki { get; set; }
            public int EvidentiranoPre5danaPaketa { get; set; }
            public int? EvidentiranoPosiljki { get; set; }
            public int? PreuzetoPosiljki { get; set; }
            public int? DostavljenoPosiljki { get; set; }
            public int? EvidentiranoPaketa { get; set; }
            public int? PreuzetoPaketa { get; set; }
            public int? DostavljenoPaketa { get; set; }
            public decimal? CenaUkupnaZaEvidentirane { get; set; }
            public decimal? PazarP { get; set; }
            public decimal? PazarD { get; set; }
            public decimal? ProsecnaCenaPoPosiljci { get; set; }
            public decimal? ProsecnaCenaPoPaketu { get; set; }

            private StatistikaDanMetadata() { }

        }
    }

}