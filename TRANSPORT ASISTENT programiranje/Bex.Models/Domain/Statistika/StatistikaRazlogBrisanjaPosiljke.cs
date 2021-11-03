namespace Bex.Models
{
    using System;
    using System.Collections.Generic;

    public  partial class StatistikaRazlogBrisanjaPosiljke
    {
        public int Id { get; set; }
        public DateTime Datum { get; set; }
        public int Sat { get; set; }
        public int RazlogBrisanja { get; set; }
        public int? UkupnoPosiljki { get; set; }

    }
}
