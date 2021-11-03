namespace Bex.Models
{
    using System;
    using System.Collections.Generic;

    public partial class TPtermini
    {
        public int Id { get; set; }
        public DateTime? DatumTermina { get; set; }
        public TimeSpan? TerminStart { get; set; }
        public TimeSpan? TerminEnd { get; set; }
        public string ImeKlijenta { get; set; }
        public string PrezimeKlijenta { get; set; }
        public string TelefonKlijenta { get; set; }
        public string RegOznaka { get; set; }
        public int? KategorijaVozilaId { get; set; }
        public int? ModelId { get; set; }
        public int? StatusTerminaId { get; set; }
        public int? UserUneoId { get; set; }
        public int? LokacijaId { get; set; }
        public string Napomena { get; set; }
        public bool? Storno { get; set; }

        public virtual VozniParkKategorija KategorijaVozila { get; set; }
        public virtual VozniParkModel ModelVozila { get; set; }
        public virtual TPlokacije LokacijaTP { get; set; }
        public virtual TPstatusTermina StatusTermina { get; set; }
        public virtual KorisniciPrograma UserUnosa { get; set; }
    }
}
