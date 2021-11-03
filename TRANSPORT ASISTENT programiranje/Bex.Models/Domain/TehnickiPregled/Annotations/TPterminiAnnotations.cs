using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bex.Models
{
    [MetadataType(typeof(TPterminiMetadata))]

    public partial class TPtermini
    {
        public class TPterminiMetadata
        {
            public int Id { get; set; }
            public DateTime? DatumTermina { get; set; }
            public TimeSpan? TerminStart { get; set; }
            public TimeSpan? TerminEnd { get; set; }
            public string ImeKlijenta { get; set; }
            public string PrezimeKlijenta { get; set; }
            public string TelefonKlijenta { get; set; }
            public string RegOznaka { get; set; }
            [ForeignKey("KategorijaVozila")]
            public int? KategorijaVozilaId { get; set; }
            [ForeignKey("ModelVozila")]
            public int? ModelId { get; set; }
            [ForeignKey("StatusTermina")]
            public int? StatusTerminaId { get; set; }
            [ForeignKey("UserUnosa")]
            public int? UserUneoId { get; set; }
            [ForeignKey("LokacijaTP")]
            public int? LokacijaId { get; set; }
            public string Napomena { get; set; }
            public bool? Storno { get; set; }

            public object KategorijaVozila { get; set; }
            public object ModelVozila { get; set; }
            public object LokacijaTP { get; set; }
            public object StatusTermina { get; set; }
            public object UserUnosa { get; set; }

            private TPterminiMetadata() { }

        }
    }

}