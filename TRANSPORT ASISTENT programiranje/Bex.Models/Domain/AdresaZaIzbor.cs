namespace Bex.Models
{
    using System;
    using System.Collections.Generic;
    public partial class AdresaZaIzbor
    {
        public int Id { get; set; }

        public string Ulica { get; set; }

        public string Mesto { get; set; }

        public string Opstina { get; set; }

        public string Adresa { get; set; }

        public virtual ICollection<Kontakt> Kontakt { get; set; }


    }
}
