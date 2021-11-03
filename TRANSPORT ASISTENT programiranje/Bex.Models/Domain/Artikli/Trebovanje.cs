namespace Bex.Models
{
    using System;
    using System.Collections.Generic;

    public  partial class Trebovanje
    {
        public int? IdTrebovanja { get; set; }
        public DateTime? DatumTrebovanja { get; set; }
        public int? SubTrazioId { get; set; }
        public int? UserIzdaoId { get; set; }
        public DateTime? DatumIzdavanja { get; set; }
        public int? UserUneoId { get; set; }
        public bool? Arhivirano { get; set; }
        public bool? Storno { get; set; }

        public virtual Kontakt Kontakt { get; set; }
        public virtual KorisniciPrograma UserUneo { get; set; }
        public virtual KorisniciPrograma UserIzdao { get; set; }

    }
}
