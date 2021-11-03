namespace Bex.Models
{
    using System;
    using System.Collections.Generic;

    public  partial class PosiljkaPlacanje
    {
       
        public int Id { get; set; }
        public int? PosiljkaId { get; set; }
        public int? PlacanjeId { get; set; }
        public int? PlatilacId { get; set; }
        public decimal Iznos { get; set; }

        public virtual Posiljka Posiljka { get; set; }
        public virtual PosiljkaPlacanjeTip PosiljkaPlacanjeTip { get; set; }


    }
}
