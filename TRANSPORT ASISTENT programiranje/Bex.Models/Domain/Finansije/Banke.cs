namespace Bex.Models
{
    using System;
    using System.Collections.Generic;

    public  partial class Banke

    {
       
        public int Id { get; set; }
        public string Sifra { get; set; }
        public string NazivBanke { get; set; }
        public string BrojRacuna { get; set; }
        public string PocetniBrojRacuna { get; set; }
        public string SifraNav { get; set; }
        public bool ZaOtkupe { get; set; }

       
    }
}
