namespace Bex.Models
{
    using AspNet.DAL.EF.Models.Security;
    using System;
    using System.Collections.Generic;

    public  partial class SkenRead
    {
        public int Id { get; set; }
        public int? PosiljkaId { get; set; }
        public string BarKod { get; set; }
        public int? RbPaketa { get; set; }
        public int? TipPaketaId { get; set; }
        public DateTime? DatumSkena { get; set; }
        public TimeSpan? VremeSkena { get; set; }
        public int? ReonId { get; set; }
        public int? RegionId { get; set; }
        public int? ZonaId { get; set; }
        public int? UserId { get; set; }
        public DateTime? VremePDA { get; set; }
        public string NazivSkena { get; set; }


        public virtual Reon Reon { get; set; }
        public virtual Region Region { get; set; }
        public virtual Zona Zona { get; set; }

        public virtual KorisniciPrograma User { get; set; }
        public virtual Posiljka Posiljka { get; set; }

    }
}
