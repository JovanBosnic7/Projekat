namespace Bex.Models
{
    using System;
    using System.Collections.Generic;

    public partial class TPocitavanja
    {
        public int Id { get; set; }
        public int? KategorijaVozilaId { get; set; }
        public int? KaroserijaVozilaId { get; set; }
        public int? ModelId { get; set; }
        public string BrojSasije { get; set; }
        public string BrojMotora { get; set; }
        public int? BojaId { get; set; }
        public int? BrojVrata { get; set; }
        public int? KontrolorId { get; set; }
        public DateTime? DatumOcitavanja { get; set; }
        public int? UserUneoId { get; set; }
        public string Napomena { get; set; }
        public bool? Storno { get; set; }

        public virtual VozniParkKategorija KategorijaVozila { get; set; }
        public virtual VozniParkKaroserija KaroserijaVozila { get; set; }
        public virtual VozniParkModel ModelVozila { get; set; }
        public virtual VozniParkBoja BojaVozila { get; set; }
        public virtual Zaposleni Kontrolor { get; set; }
        public virtual KorisniciPrograma UserUnosa { get; set; }
    }
}
