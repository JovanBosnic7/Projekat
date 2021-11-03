namespace Bex.Models
{
    using System;
    using System.Collections.Generic;

    public  partial class TicketPrimalac

    {
        public int Id { get; set; }
        public int TicketId { get; set; }
        public int PrimalacId { get; set; }
        public int TipId { get; set; }


        public virtual Ticket Ticket { get; set; }
        public virtual TicketPrimalacTip PrimalacTip { get; set; }

    }
}
