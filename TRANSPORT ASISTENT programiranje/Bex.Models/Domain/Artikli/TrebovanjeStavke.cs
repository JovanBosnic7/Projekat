namespace Bex.Models
{
    using System;
    using System.Collections.Generic;

    public  partial class TrebovanjeStavke
    {
        public int? IdStavkeTrebovanja { get; set; }
        public int? TrebovanjeId { get; set; }
        public int? ArtikalId { get; set; }
        public int? KolicinaTrazena { get; set; }
        public int? KolicinaIzdata { get; set; }

        public virtual Trebovanje Trebovanje { get; set; }
        public virtual Artikli Artikal { get; set; }

    }
}
