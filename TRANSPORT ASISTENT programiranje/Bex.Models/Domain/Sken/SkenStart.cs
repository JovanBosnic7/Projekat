namespace Bex.Models
{
    using AspNet.DAL.EF.Models.Security;
    using System;
    using System.Collections.Generic;

    public  partial class SkenStart
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public int? TipId { get; set; }
        public int? ReonId { get; set; }
        public int? RegionId { get; set; }
        public int? LogovanjeId { get; set; }
        public int? ZonaId { get; set; }
        public DateTime? Datum { get; set; }
        public DateTime? VremeStart { get; set; }
        public DateTime? VremeStop { get; set; }
        public int? PoslatoSkenova { get; set; }
        public int? UpisanoSkenova { get; set; }

        public virtual KorisniciPrograma User { get; set; }
        public virtual SkenTip SkenTip { get; set; }
        public virtual Reon Reon { get; set; }
        public virtual Region Region { get; set; }
        public virtual Zona Zona { get; set; }

        public virtual ICollection<Sken> Sken { get; set; }

    }
}
