namespace Bex.Models
{
    using System;
    using System.Collections.Generic;

    public  partial class ZonaPodTip
    {
       
        public int Id { get; set; }
        public int? TipId { get; set; }
        public string Opis { get; set; }

        public virtual ZonaTip ZonaTip { get; set; }

    }
}
