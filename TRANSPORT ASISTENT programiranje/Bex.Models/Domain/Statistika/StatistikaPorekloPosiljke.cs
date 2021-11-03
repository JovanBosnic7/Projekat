namespace Bex.Models
{
    using System;
    using System.Collections.Generic;

    public  partial class StatistikaPorekloPosiljke
    {
        public int Id { get; set; }
        public DateTime Datum { get; set; }
        public int Sat { get; set; }
        public int Poreklo { get; set; }
        public int? UkupnoPosiljki { get; set; }

    }
}
