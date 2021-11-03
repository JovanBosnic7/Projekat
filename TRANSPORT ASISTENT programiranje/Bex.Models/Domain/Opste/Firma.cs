namespace Bex.Models
{
    using System;
    using System.Collections.Generic;

    public  partial class Firma
    {
       
        public int Id { get; set; }

        public string Naziv { get; set; }

        public string Adresa { get; set; }
        public int? Delatnost { get; set; }
        public string MaticniBroj { get; set; }
        public string Pib { get; set; }
        public bool Storno { get; set; }
        public int? FirmaIdVP { get; set; }
}
}
