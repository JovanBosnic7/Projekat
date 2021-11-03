namespace Bex.Models
{
    using AspNet.DAL.EF.Models.Security;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

    public  partial class Region
    {
        
        public int Id { get; set; }

        public string NazivSkraceni { get; set; }

        public string NazivRegiona { get; set; }

        public string OznRegion { get; set; }

        public int? Tip { get; set; }

        public string BarKodRegiona { get; set; }

        public bool? Storno { get; set; }

        public bool? DeoGrupeRegiona { get; set; }

        public int? RegionGrupaId { get; set; }

        public bool? ZabranjenoProvlacenjeZadataka { get; set; }

        public int? Sort { get; set; }

        public int? DimId { get; set; }

        public DateTime? DI { get; set; }

        public DateTime? DU { get; set; }

        public virtual DimenzijeNav DimenzijeNav { get; set; }

        public virtual ICollection<Reon> Reons { get; set; }
        
        public virtual ICollection<KorisniciPrograma> User { get; set; }

        //public virtual ICollection<VozniParkDnevnik> VozniParkDnevnik { get; set; }
    }
}
