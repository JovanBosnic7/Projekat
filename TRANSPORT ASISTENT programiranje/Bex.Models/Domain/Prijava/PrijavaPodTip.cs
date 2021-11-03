namespace Bex.Models
{
    using System;
    using System.Collections.Generic;

    public  partial class PrijavaPodTip

    {
        public int Id { get; set; }
        public int? TipId { get; set; }
        public string Naziv { get; set; }
        public bool? Storno { get; set; }

        public virtual PrijavaTip PrijavaTip { get; set; }
        
    }
}
