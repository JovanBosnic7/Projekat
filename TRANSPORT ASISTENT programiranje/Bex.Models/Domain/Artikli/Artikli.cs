namespace Bex.Models
{
    using System;
    using System.Collections.Generic;

    public  partial class Artikli
    {
        public int Id { get; set; }
        public int? ArtiTipId { get; set; }
        public bool? Sirovina { get; set; }
        public bool? Proizvod { get; set; }
        public bool? TrgovackaRoba { get; set; }
        public bool? ProdajaDozvoljena { get; set; }
        public int? ArtVrstaId { get; set; }
        public int? ArtGrupaId { get; set; }
        public int? ArtMarkaId { get; set; }
        public string Sifra { get; set; }
        public string Opis { get; set; }
        public string Oznaka { get; set; }
        public string Napomena { get; set; }
        public int? JmId { get; set; }
        public int? NavOk { get; set; }
        public bool? Storno { get; set; }

        public virtual ICollection<TrebovanjeStavke> TrebovanjeStavke { get; set; }
        public virtual ArtikliGrupa ArtikliGrupa { get; set; }
        public virtual ArtikliVrsta ArtikliVrsta { get; set; }
    }
}
