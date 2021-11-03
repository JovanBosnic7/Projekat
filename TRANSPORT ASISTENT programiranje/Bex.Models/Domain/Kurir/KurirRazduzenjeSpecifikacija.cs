namespace Bex.Models
{
    using AspNet.DAL.EF.Models.Security;
    using System;
    using System.Collections.Generic;

    public  partial class KurirRazduzenjeSpecifikacija
    {

        public int Id { get; set; }
        public int? KurirId { get; set; }
        public int? ReonId { get; set; }
        public int? PosiljkaId { get; set; }
        public string TipZadatka { get; set; }
        public bool Status { get; set; }
        public int? UtovarenoPaketa { get; set; }
        public decimal? Otkup { get; set; }
        public decimal? Usluga { get; set; }
        public int? NapomenaPodTipId { get; set; }
        public DateTime Datum { get; set; }
        //public string Adresa { get; set; }
        public virtual Posiljka Posiljka { get; set; }
        public virtual Reon Reon { get; set; }
        public virtual KorisniciPrograma User { get; set; }


    }
}
