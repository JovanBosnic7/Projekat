namespace Bex.Models
{
    using System;
    using System.Collections.Generic;

    public  partial class PravniPostupak
    {       
        public int Id { get; set; }
        public DateTime DatumUnosa { get; set; }
        public int? FirmaId { get; set; }
        public int? OblastId { get; set; }
        public int? VrstaId { get; set; }
        public bool? Tuzeni { get; set; }
        public bool? Tuzilac { get; set; }
        public string NadlezanOrgan { get; set; }
        public string BrojPredmeta { get; set; }
        public DateTime? Datum { get; set; }
        public string Lice { get; set; }
        public string Opis { get; set; }
        public string Vrednost { get; set; }
        public string Zastupnik { get; set; }
        public DateTime? DatumSledeceg { get; set; }
        public DateTime? DatumZavrsetka { get; set; }
        public string Napomena { get; set; }
        public int UserUneoId { get; set; }
        public int? MinimalnaKazna { get; set; }
        public int? MaksimalanaKazna { get; set; }
        public int? OcekivanaKazna { get; set; }
       
        public virtual Firma Firma { get; set; }
        public virtual PPvrsta PPvrsta { get; set; }
        public virtual PPoblast PPoblast { get; set; }
        public virtual KorisniciPrograma UserUneo { get; set; }
    }
}
