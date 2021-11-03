namespace Bex.Models
{
    using System;
    using System.Collections.Generic;

    public  partial class ZonaTip
    {
       
        public int Id { get; set; }
        public string Opis { get; set; }

        public virtual ICollection<ZonaPodTip> ZonaPodTip { get; set; }
    }
}
