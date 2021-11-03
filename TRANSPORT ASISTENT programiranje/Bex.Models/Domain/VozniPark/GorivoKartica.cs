namespace Bex.Models
{
    using System;
    using System.Collections.Generic;

    public  partial class GorivoKartica
    {
       
        public int Id { get; set; }
        
        public string NazivKartice { get; set; }

        public int PumpaId { get; set; }
        public int VpId { get; set; }

        public DateTime DatumProizvodnje { get; set; }

        public DateTime DatumIsteka { get; set; }

        public string Pincode { get; set; }
        public string BrojKartice { get; set; }
        public bool Storno { get; set; }
        public virtual GorivoPumpa GorivoPumpa { get; set; }
        public virtual VozniPark VozniPark { get; set; }

    }
}
