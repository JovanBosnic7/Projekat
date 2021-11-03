namespace Bex.Models
{
    using System;
    using System.Collections.Generic;

    public  partial class TicketKategorija

    {
        public int Id { get; set; }
        public int SektorId { get; set; }
        public string Naziv { get; set; }
        public string Opis { get; set; }

        public virtual Sektor Sektor { get; set; }

    }
}
