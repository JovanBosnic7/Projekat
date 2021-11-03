namespace Bex.Models
{
    using System;
    using System.Collections.Generic;

    public  partial class Lager
    {
        public int Id { get; set; }
        public int? MagacinId { get; set; }
        public int? ArtId { get; set; }
        public decimal? Kolicina { get; set; }
        public DateTime? DatumIzmene { get; set; }
        public TimeSpan? VremeIzmene { get; set; }

        public virtual MagacinSpisak MagacinSpisak { get; set; }
        public virtual Artikli Artikli { get; set; }

    }
}
