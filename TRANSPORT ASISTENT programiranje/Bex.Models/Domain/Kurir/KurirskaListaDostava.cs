namespace Bex.Models
{
    using System;
    using System.Collections.Generic;

    public  partial class KurirskaListaDostava
    {
        public int? PosiljkaId { get; set; }
        public string TipZadatka { get; set; }
        public string NazivReoncica { get; set; }
        public DateTime? AktuelanOd { get; set; }
        public bool? VipD { get; set; }
        public string UlicaNaziv { get; set; }
        public string NazivKlijenta { get; set; }
        public string Napomene { get; set; }
        public string Telefoni { get; set; }
        public bool? BezgotovinskoP { get; set; }
        public bool? BezgotovinskoD { get; set; }
        public string NazivKategorije { get; set; }
        public int? UkupnoPaketa { get; set; }
        public int? KurirId { get; set; }
        public string Naziv { get; set; }
        public bool? Status { get; set; }
        public decimal? Otpremnica { get; set; }
        public decimal? Povratnica { get; set; }
        public decimal? PlacenOdgovor { get; set; }
        public decimal? Otkup { get; set; }
        public decimal? Usluga { get; set; }
        public int? Id { get; set; }
    }
}
