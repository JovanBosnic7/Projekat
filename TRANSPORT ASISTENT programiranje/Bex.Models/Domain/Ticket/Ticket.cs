namespace Bex.Models
{
    using System;
    using System.Collections.Generic;

    public  partial class Ticket

    {
        public int Id { get; set; }
        public int RoditeljTicketId { get; set; }

        public int UserUneoId { get; set; }

        public int KontaktPrijavioId { get; set; }

        public DateTime DatumUnosa { get; set; }

        public DateTime DatumIzmene { get; set; }
        public String Tekst { get; set; }
        public int StatusId { get; set; }
        public int NacinPrijaveId { get; set; }
        public DateTime Rok { get; set; }

        public int Prioritet { get; set; }
        public int PrihvatioKontaktId { get; set; }


        public virtual KorisniciPrograma KorisniciPrograma { get; set; }
        public virtual Kontakt KontaktPrijavio { get; set; }

        public virtual TicketStatus Status { get; set; }

        public virtual PrijavaNacin NacinPrijave { get; set; }
        public virtual Kontakt KontaktPrihvatio { get; set; }

    }
}
