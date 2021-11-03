namespace Bex.Models
{
    using System;
    using System.Collections.Generic;

    public  partial class PosiljkaUsluga
    {
       
        public int Id { get; set; }
        public int? PosiljkaId { get; set; }
        public int? TipUslugeId { get; set; }
        public decimal Komada { get; set; }
        public decimal? CenaUsluge { get; set; }
        public string Napomena { get; set; }
        public virtual Posiljka Posiljka { get; set; }
        public virtual PosiljkaUslugaTip PosiljkaUslugaTip { get; set; }

    }
}
