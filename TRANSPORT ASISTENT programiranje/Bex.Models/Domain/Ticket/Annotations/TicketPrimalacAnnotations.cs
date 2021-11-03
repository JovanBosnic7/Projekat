using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bex.Models
{
    [MetadataType(typeof(TicketPrimalacMetadata))]
    public partial class TicketPrimalac
    {
        public class TicketPrimalacMetadata
        {

            public int Id { get; set; }
            [ForeignKey("Ticket")]
            public int TicketId { get; set; }
            public int PrimalacId { get; set; }

            [ForeignKey("PrimalacTip")]
            public int TipId { get; set; }

            public object Ticket { get; set; }
            public object PrimalacTip { get; set; }
            private TicketPrimalacMetadata() { }
        }
    }
}