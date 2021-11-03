namespace Bex.Models
{
    using System;
    using System.Collections.Generic;

    public  partial class ZaposleniZanimanje
    {
       
        public int Id { get; set; }

        public int? Sifra { get; set; }

        public string Opis { get; set; }
    }
}
