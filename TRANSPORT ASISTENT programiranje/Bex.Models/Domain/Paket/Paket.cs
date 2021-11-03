namespace Bex.Models
{
    using System;
    using System.Collections.Generic;

    public  partial class Paket

    {
       
        public int Id { get; set; }
        public string BarKod { get; set; }
        public int? PosiljkaId { get; set; }
        public int PaketRB { get; set; }
        public int TipPaketa { get; set; }
        public decimal Masa { get; set; }
        public int? ZonaId { get; set; }
        public bool Storno { get; set; }

        public virtual Posiljka Posiljka { get; set; }
        public virtual Zona Zona { get; set; }

        public virtual ICollection<PaketZadatak> PaketZadatak { get; set; }


    }
}
