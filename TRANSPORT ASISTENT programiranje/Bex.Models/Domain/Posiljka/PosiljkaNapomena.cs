namespace Bex.Models
{
    using System;
    using System.Collections.Generic;

    public  partial class PosiljkaNapomena
    {
       
        public int Id { get; set; }
        public int? PosiljkaId { get; set; }
        public int? NapomenaTipId { get; set; }
        public string Napomena { get; set; }

        public virtual Posiljka Posiljka { get; set; }
        public virtual PosiljkaNapomenaTip PosiljkaNapomenaTip { get; set; }
    }
}
