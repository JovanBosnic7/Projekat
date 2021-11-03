namespace Bex.Models
{
    using AspNet.DAL.EF.Models.Security;
    using System;
    using System.Collections.Generic;

    public  partial class NapomenaPosiljka
    {
       
        public int Id { get; set; }
        public int? PosiljkaId { get; set; }

        public string TipZadatka { get; set; }
        public int? PodTipId { get; set; }
        public string Napomena { get; set; }

        public int? UserUnosId { get; set; }

        public virtual Posiljka Posiljka { get; set; }

        public virtual KorisniciPrograma User { get; set; }
        public virtual NapomenaPosiljkaPodTip NapomenaPosiljkaPodTip { get; set; }
    }
}
