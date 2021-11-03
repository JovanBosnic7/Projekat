using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bex.Models
{
    [MetadataType(typeof(TicketPostMetadata))]
    public partial class TicketPost
    {
        public class TicketPostMetadata
        {

            public int Id { get; set; }
            [ForeignKey("Ticket")]
            public int TicketId { get; set; }

            public string Tekst { get; set; }

            [ForeignKey("KorisniciPrograma")]
            public int? UserUnosaId { get; set; }
            public DateTime? DatumUnosa { get; set; }
           
            

            public object Ticket { get; set; }
            public object KorisniciPrograma { get; set; }

            private TicketPostMetadata() { }
        }
    }
}