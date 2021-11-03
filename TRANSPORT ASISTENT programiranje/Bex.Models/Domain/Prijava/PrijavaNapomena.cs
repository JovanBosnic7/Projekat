namespace Bex.Models
{
    using System;
    using System.Collections.Generic;

    public  partial class PrijavaNapomena

    {
        public int Id { get; set; }
        public int? PrijavaId { get; set; }
        public DateTime? DatumUnosa { get; set; }
        public int? UserUnosaId { get; set; }
        public string Napomena { get; set; }

        public virtual PrijavaReklamacijaZalba PrijavaReklamacijaZalba { get; set; }
        public virtual KorisniciPrograma KorisniciPrograma { get; set; }
    }
}
