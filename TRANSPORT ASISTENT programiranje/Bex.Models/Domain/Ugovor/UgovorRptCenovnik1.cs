namespace Bex.Models
{
    using System;
    using System.Collections.Generic;

    public partial class UgovorRptCenovnik1
    {
        public int? IdUgovora { get; set; }
        public int? Cenovnik1 { get; set; }
        public string Kategorija1 { get; set; }
        public decimal? Cena1 { get; set; }
        public decimal? Cena1bezPDV { get; set; }
        public decimal? PDV1 { get; set; }
        public string NazivVrste1 { get; set; }
        public int? IdVrste1 { get; set; }
        public decimal? CenaProc1 { get; set; }
        public int? IdKategorije1 { get; set; }
        public int? IdCene { get; set; }
    }
}
