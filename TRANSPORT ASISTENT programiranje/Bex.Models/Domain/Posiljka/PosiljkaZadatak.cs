namespace Bex.Models
{
    using AspNet.DAL.EF.Models.Security;
    using System;
    using System.Collections.Generic;

    public  partial class PosiljkaZadatak
    {
       
        public int Id { get; set; }
        public int? PosiljkaId { get; set; }
        public string Tip { get; set; }
        public DateTime DatumEvidentiranja { get; set; }
        public TimeSpan? VremeEvidentiranja { get; set; }
        public DateTime DatumMin { get; set; }
        public DateTime DatumMax { get; set; }
        public TimeSpan? VremeMin { get; set; }
        public TimeSpan? VremeMax { get; set; }
        public int? NajavaMinuta { get; set; }
        public string KontaktIme { get; set; }
        public string KontaktTelefon { get; set; }
        public string KontaktTelefon2 { get; set; }
        public string Napomena { get; set; }

        public string NapomenaKasnjenjeLast { get; set; }
        public int Status { get; set; }
        public int? ReonId { get; set; }
        public int? AdresaId { get; set; }
        public string Adresa { get; set; }
        public DateTime? DatumIzvrsenja { get; set; }
        public TimeSpan? VremeIzvrsenja { get; set; }
        public int? IzvrsioId { get; set; }
        public bool? IzvrsitiUmagacinu { get; set; }
        public DateTime? AktuelanOd { get; set; }
        public int? SubId { get; set; }
        public string NazivKlijenta { get; set; }
        public int? PakId { get; set; }
        public virtual Posiljka Posiljka { get; set; }
        public virtual Reon Reon { get; set; }
        public virtual Kontakt Kontakt { get; set; }

        public virtual KorisniciPrograma User { get; set; }
        public virtual PAK PAK { get; set; }


    }
}
