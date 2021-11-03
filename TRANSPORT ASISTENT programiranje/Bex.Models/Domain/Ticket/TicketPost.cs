namespace Bex.Models
{
    using System;
    using System.Collections.Generic;

    public  partial class TicketPost

    {
        public int Id { get; set; }
        public int TicketId { get; set; }
        public string Tekst { get; set; }
        public int UserUnosaId { get; set; }
        public DateTime DatumUnosa { get; set; }
        
       

        public virtual Ticket Ticket { get; set; }
        public virtual KorisniciPrograma KorisniciPrograma { get; set; }
    }
}
