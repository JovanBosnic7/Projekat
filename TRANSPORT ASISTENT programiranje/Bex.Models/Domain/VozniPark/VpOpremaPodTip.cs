namespace Bex.Models
{
    using System;
    using System.Collections.Generic;

    public  partial class VpOpremaPodTip
    {

        public int Id { get; set; }
        public string Naziv { get; set; }
        public int TipId { get; set; }
        public int Sort { get; set; }
        public bool Storno { get; set; }

        public virtual VpOpremaTip VpOpremaTip { get; set; }
    }
}
