using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Bex.Models
{
    [MetadataType(typeof(NapomenaPosiljkaPodTipMetadata))]
    public partial class NapomenaPosiljkaPodTip
    {
        public class NapomenaPosiljkaPodTipMetadata
        {

            public int Id { get; set; }
            public int? GrupaId { get; set; }
            public int? TipId { get; set; }
            public string NazivPodTipa { get; set; }

            public string NazivPodTipa2 { get; set; }

            public string NazivPodTipaKurir { get; set; }

            public string NazivPodTipaZaIzvestaj { get; set; }

            private NapomenaPosiljkaPodTipMetadata() { }
        }
    }
}