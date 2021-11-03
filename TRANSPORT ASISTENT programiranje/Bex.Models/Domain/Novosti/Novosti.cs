namespace Bex.Models
{
    using System;
    using System.Collections.Generic;

    public  partial class Novosti
    {
       
        public int Id { get; set; }
        public DateTime Datum { get; set; }
        public string Naslov { get; set; }
        public string Tekst { get; set; }
        public string Link { get; set; }
        public bool Aktivno { get; set; }

    }
}
