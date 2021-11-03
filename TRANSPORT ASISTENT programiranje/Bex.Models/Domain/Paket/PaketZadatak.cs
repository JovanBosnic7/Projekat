namespace Bex.Models
{
    using AspNet.DAL.EF.Models.Security;
    using System;
    using System.Collections.Generic;

    public  partial class PaketZadatak
    {
       
        public int Id { get; set; }
        public int? IdPaketa { get; set; }
        public int? PosiljkaId { get; set; }
        public string Tip { get; set; }
        public int? ZonaId { get; set; }
        public DateTime? DatumIzvrsenja { get; set; }
        public TimeSpan? VremeIzvrsenja { get; set; }
        public int? IzvrsioId { get; set; }

        public virtual Paket Paket { get; set; }
        public virtual Zona Zona { get; set; }
        public virtual KorisniciPrograma User { get; set; }

    }
}
