namespace Bex.Models
{
    using System;
    using System.Collections.Generic;

    public  partial class GorivoTocenje
    {
       
        public int Id { get; set; }
        public int PutniNalogId { get; set; }
        public DateTime? Datum { get; set; }
        public TimeSpan? Vreme { get; set; }
        public decimal Litara { get; set; }
        public decimal Cena { get; set; }
        public decimal Iznos { get; set; }
        public int Km { get; set; }
        public string Napomena { get; set; }
        public bool Storno { get; set; }
        public int? PumpaId { get; set; }
        public int PresaoKmOdPrethodnogSipanja { get; set; }
        public decimal ProsekOdPrethodnog { get; set; }

        public virtual PutniNalog PutniNalog { get; set; }
        public virtual GorivoPumpa GorivoPumpa { get; set; }

    }
}
