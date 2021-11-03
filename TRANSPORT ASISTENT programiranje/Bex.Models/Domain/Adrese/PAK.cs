namespace Bex.Models
{
    using System;
    using System.Collections.Generic;
    public  partial class PAK
    {
        public int Id { get; set; }

        public int? ReoncicId { get; set; }
        public string NazivReoncica { get; set; }

        public int? ReonId { get; set; }
        public string NazivReona { get; set; }

        public int? RegionId { get; set; }
        public string NazivRegiona { get; set; }

        public int? OpstinaId { get; set; }
        public string NazivOpstine { get; set; }

        public int? MestoId { get; set; }
        public string NazivMesta { get; set; }

        public int? UlicaId { get; set; }
        public string NazivUlice { get; set; }

        public int? OdBroja { get; set; }

        public int? DoBroja { get; set; }

        public int? Parnost { get; set; }

        public int? PttId { get; set; }

        public int? PakZaStampu { get; set; }

        public string Napomena { get; set; }

        public int? UserUnosId { get; set; }

        public int? UserIzmeneId { get; set; }

        public DateTime? DI { get; set; }

        public virtual Reon Reon { get; set; }

        public virtual Reoncic Reoncic { get; set; }

        public virtual Street Street { get; set; }

        public virtual Place Place { get; set; }

        public virtual Region Region { get; set; }

        public virtual Zip Zip { get; set; }
        public virtual ICollection<PosiljkaZadatak> PosiljkaZadatak { get; set; }

    }
}
