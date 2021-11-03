namespace Bex.Models
{
    using System;
    using System.Collections.Generic;

    public  partial class ZaposleniNapomena
    {
       
        public int Id { get; set; }
        public int? ZaposleniId { get; set; }

        public int? UneoUserId { get; set; }

        public string Napomena { get; set; }

        public DateTime DatumUnosa { get; set; }

        public virtual Zaposleni Zaposleni { get; set; }
        public virtual KorisniciPrograma KorisniciPrograma { get; set; }
    }
}
