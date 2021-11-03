namespace Bex.Models
{
    using AspNet.DAL.EF.Models.Security;
    using System;
    using System.Collections.Generic;

    public  partial class KurirRazduzenje
    {
       
        public int Id { get; set; }
        public int? KurirId { get; set; }
        public int? ReonId { get; set; }

        public decimal? UkupnoOtkupa { get; set; }

        public decimal? UkupnoPazara { get; set; }

        public decimal? UkupnoNaplatio { get; set; }

        public decimal? UkupnoRazduzio { get; set; }

        public decimal? Razlika { get; set; }

        public string Napomena { get; set; }

        public int? UserUnosId { get; set; }

        public DateTime? Datum { get; set; }
        
        public virtual Reon Reon { get; set; }
        public virtual KorisniciPrograma UserKurir { get; set; }
        public virtual KorisniciPrograma UserUnos { get; set; }


    }
}
