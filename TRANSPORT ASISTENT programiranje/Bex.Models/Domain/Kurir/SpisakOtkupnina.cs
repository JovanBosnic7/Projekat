namespace Bex.Models
{
    using System;
    using System.Collections.Generic;

    public  partial class SpisakOtkupnina
    {
        public string TipZadatka { get; set; }
        public string RegionReonT { get; set; }
        public string Posiljalac { get; set; }
        public string AdresaP { get; set; }
        public int? PosiljkaId { get; set; }
        public string KontaktTelefoniP { get; set; }
        public int? KurirId { get; set; }
        public int? Id { get; set; }
        public bool? Status { get; set; }
        public string BarKod { get; set; }
        public decimal? Otkupnina { get; set; }


    }
}
