using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bex.Models
{
    [MetadataType(typeof(TicketMetadata))]
    public partial class Ticket
    {
        public class TicketMetadata
        {
            public int Id { get; set; }
            public int RoditeljTicketId { get; set; }

            [ForeignKey("KorisniciPrograma")]
            public int UserUneoId { get; set; }

            [ForeignKey("KontaktPrijavio")]
            public int KontaktPrijavioId { get; set; }

            public DateTime DatumUnosa { get; set; }

            public DateTime DatumIzmene { get; set; }
            public String Tekst { get; set; }
            [ForeignKey("Status")]
            public int StatusId { get; set; }
            [ForeignKey("NacinPrijave")]
            public int NacinPrijaveId { get; set; }
            public DateTime Rok { get; set; }

            public int Prioritet { get; set; }
            [ForeignKey("KontaktPrihvatio")]
            public int PrihvatioKontaktId { get; set; }

            public object KorisniciPrograma { get; set; }
            public object KontaktPrijavio { get; set; }
            public object Status { get; set; }
            public object NacinPrijave { get; set; }
            public object KontaktPrihvatio { get; set; }
            private TicketMetadata() { }
        }
    }
}