namespace Bex.Models
{
    using AspNet.DAL.EF.Models.Security;
    using System;
    using System.Collections.Generic;

    public  partial class PosiljkaObrisana
    {
       
        public int Id { get; set; }
        public int PosiljkaId { get; set; }
        public int? NapomenaPodTipId { get; set; }
        public int ObrisaoUserId { get; set; }
        public bool IzKlijentskog { get; set; }
        public DateTime DatumBrisanja { get; set; }

        public virtual Posiljka Posiljka { get; set; }
        public virtual NapomenaPosiljkaPodTip NapomenaPosiljkaPodTip { get; set; }
        public virtual KorisniciPrograma UserObrisao { get; set; }
    }
}
