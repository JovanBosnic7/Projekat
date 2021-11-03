namespace Bex.Models
{
    using System;
    using System.Collections.Generic;

    public  partial class VpOprema
    {
       
        public int Id { get; set; }
        public int VpId { get; set; }
        public int PodTipId { get; set; }
        public int UserUnosId { get; set; }
        public DateTime DatumUnosa { get; set; }

        public virtual VozniPark VozniPark { get; set; }
        public virtual VpOpremaPodTip VpOpremaPodTip { get; set; }
        public virtual KorisniciPrograma KorisniciPrograma { get; set; }
    }
}
