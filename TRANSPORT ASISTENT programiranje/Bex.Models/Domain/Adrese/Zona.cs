namespace Bex.Models
{
    using System;
    using System.Collections.Generic;

    public  partial class Zona
    {
       
        public int Id { get; set; }
        public string NazivZone { get; set; }
        public int? Tip { get; set; }
        public int? PodTip { get; set; }
        public int? VoziloRegionId { get; set; }
        public int? RegionGrupaId { get; set; }
        public string Opis { get; set; }
        public bool Aktivan { get; set; }

        public virtual ICollection<Paket> Paket { get; set; }
        public virtual ICollection<PaketZadatak> PaketZadatak { get; set; }

        public virtual ZonaTip ZonaTip { get; set; }
        public virtual ZonaPodTip ZonaPodTip { get; set; }

        public virtual ICollection<KurirZaduzenje> KurirZaduzenje { get; set; }


    }
}
