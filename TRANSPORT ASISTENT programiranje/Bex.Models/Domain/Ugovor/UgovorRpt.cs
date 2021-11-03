namespace Bex.Models
{
    using System;
    using System.Collections.Generic;

    public partial class UgovorRpt
    {
        public int? IdUgovora { get; set; }
        public string NazivFirma { get; set; }
        public string AdresaFirma { get; set; }
        public string MaticniBrojFirma { get; set; }
        public string PIBfirma { get; set; }
        public int? Delatnost { get; set; }
        public string UgovorVerzija { get; set; }
        public string DatumPoslednjeVerzije { get; set; }
        public string Ugovorilac { get; set; }
        public string Nosilac { get; set; }
        public string Adresa { get; set; }
        public string PlaceName { get; set; }
        public int? PIB { get; set; }
        public string MaticniBroj { get; set; }
        public string ZastupnikKlijenta { get; set; }
        public int? Cenovnik1Id { get; set; }
        public int? NaplataId { get; set; }
        public bool? BezZastitneCene { get; set; }
        public decimal? ZastitnaCena { get; set; }
        public int? RabatMinBrojPaketa { get; set; }
        public decimal? RabatProcenatZaBrojPaketa { get; set; }
        public decimal? RabatMinIznosFakture { get; set; }
        public decimal? RabatProcenatZaIznosFakture { get; set; }
        public int? Cenovnik2Id { get; set; }
        public int? ValutaDana { get; set; }
        public int? FakturaDana { get; set; }
        public bool? FakturaEmailom { get; set; }
        public bool? FakturaPostom { get; set; }
        public bool? PravnoLice { get; set; }
        public bool? NaplataPoFakturi { get; set; }
        public decimal? FakturaProc { get; set; }
        public decimal? ZastitnaCenaFak { get; set; }
        public int? PovlasceneCeneKlijentuId { get; set; }
        public bool? OdobrenoBezgotovinsko { get; set; }
        public decimal? RabatProcenat { get; set; }
        public string BrojPunomocja { get; set; }
        public DateTime? DatumPunomocja { get; set; }
        public bool? Aktivan { get; set; }
        public string TekuciRacun { get; set; }
        public string Telefon1 { get; set; }
        public string Telefon2 { get; set; }
        public string Telefon3 { get; set; }
        public string Fax { get; set; }
        public string WebSite { get; set; }
        public string Mail { get; set; }
    }
}
