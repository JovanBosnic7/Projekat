namespace Bex.Models
{
    using System;
    using System.Collections.Generic;

    public  partial class NapomenaPosiljkaPodTip
    {
       
        public int Id { get; set; }
        public int? GrupaId { get; set; }
        public int? TipId { get; set; }
        public string NazivPodTipa { get; set; }

        public string NazivPodTipa2 { get; set; }

        public string NazivPodTipaKurir { get; set; }

        public string NazivPodTipaZaIzvestaj{ get; set; }

        public virtual ICollection<NapomenaPosiljka> NapomenaPosiljka { get; set; }


    }
}
