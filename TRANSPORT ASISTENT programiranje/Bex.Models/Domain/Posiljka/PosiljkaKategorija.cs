namespace Bex.Models
{
    using System;
    using System.Collections.Generic;

    public  partial class PosiljkaKategorija
    {
       
        public int Id { get; set; }
        public string NazivKategorije { get; set; }
        public int? VrstaPosiljkeId { get; set; }
        public bool Nevazece { get; set; }
        public int Sort { get; set; }
        public bool MnozitiKolicinom { get; set; }
        public decimal OdKoliko { get; set; }
        public decimal DoKoliko { get; set; }
        public int SpecificanTip { get; set; }
        public int TipKategorije { get; set; }
        public string OpisCenovnik1 { get; set; }
        public string OpisCenovnik2 { get; set; }
        public bool? ZaVrstuPoFakturi { get; set; }
        public int SadrzajDefaultId { get; set; }

    }
}
