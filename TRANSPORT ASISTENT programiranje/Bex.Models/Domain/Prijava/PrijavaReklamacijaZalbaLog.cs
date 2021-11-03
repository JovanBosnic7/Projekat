namespace Bex.Models
{
    using System;
    using System.Collections.Generic;

    public  partial class PrijavaReklamacijaZalbaLog

    {
        public int Id { get; set; }

        public int? PrijavaId { get; set; }
        public int? TipPrijaveId { get; set; }
        public int? PodTipPrijaveId { get; set; }
        public int? NacinPrijaveId { get; set; }
        public int? PosiljkaId { get; set; }
        public int? PrijavioUserId { get; set; }
        public string PrijavioIme { get; set; }
        public string PrijavioPrezime { get; set; }
        public string PrijavioEmail { get; set; }
        public string PrijavioTelefon { get; set; }
        public string Opis { get; set; }
        public int? StatusPrijaveId { get; set; }
        public DateTime? DatumPrijave { get; set; }
        public DateTime? DatumIzmene { get; set; }
        public int? UserIdIzmene { get; set; }

        public virtual PrijavaTip PrijavaTip { get; set; }
        public virtual PrijavaPodTip PrijavaPodTip { get; set; }
        public virtual PrijavaNacin PrijavaNacin { get; set; }
        public virtual KorisniciPrograma KorisniciPrograma { get; set; }
        public virtual PrijavaStatus PrijavaStatus { get; set; }
    }
}
