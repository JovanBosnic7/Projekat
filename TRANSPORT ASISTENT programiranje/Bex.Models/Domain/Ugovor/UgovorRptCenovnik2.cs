namespace Bex.Models
{
    using System;
    using System.Collections.Generic;

    public partial class UgovorRptCenovnik2
    {
        public int? IdUgovora { get; set; }
        public int? Cenovnik2 { get; set; }
        public string Kategorija2 { get; set; }
        public decimal? Cena2 { get; set; }
        public decimal? Cena2bezPDV { get; set; }
        public decimal? PDV2 { get; set; }
        public string NazivVrste2 { get; set; }
        public int? IdVrste2 { get; set; }
        public decimal? CenaProc2 { get; set; }
        public int? IdKategorije2 { get; set; }
        public int? IdCene { get; set; }
    }
}
