namespace Bex.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public  partial class Reon
    {
       
        public int Id { get; set; }

        public string OznReona { get; set; }

        public int? RegionId { get; set; }

        public string NazivReona { get; set; }

        public bool? Storno { get; set; }

        public string BarKodReona { get; set; }

        public int? Tip { get; set; }

        public int? KmDoReona { get; set; }

        public int? OptimalnaKilometraza { get; set; }

        
        public virtual ICollection<PAK> PAKs { get; set; }
        public virtual ICollection<Reoncic> Reoncic { get; set; }

        public virtual ICollection<PosiljkaZadatak> PosiljkaZadatak { get; set; }
        public virtual ICollection<PosiljkaOtkupZbirni> PosiljkaOtkupZbirni { get; set; }
        public virtual ICollection<KurirZaduzenje> KurirZaduzenje { get; set; }

        public virtual ICollection<KurirRazduzenje> KurirRazduzenje { get; set; }
        public virtual ICollection<KurirRazduzenjeSpecifikacija> KurirRazduzenjeSpecifikacija { get; set; }

        public virtual Region Region { get; set; }
        public virtual ReonTip ReonTip { get; set; }


    }
}
