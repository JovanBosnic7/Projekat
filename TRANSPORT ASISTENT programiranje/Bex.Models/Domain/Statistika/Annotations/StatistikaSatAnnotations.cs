using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bex.Models
{
    [MetadataType(typeof(StatistikaSatMetadata))]

    public partial class StatistikaSat
    {
        public class StatistikaSatMetadata
        {
            public int Id { get; set; }
            public DateTime Datum { get; set; }
            public int Sat { get; set; }
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
            public decimal? PaketaPoPosiljci { get; set; }

            private StatistikaSatMetadata() { }

        }
    }

}