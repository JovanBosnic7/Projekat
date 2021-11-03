namespace Bex.Models
{
    using System;
    using System.Collections.Generic;

    public  partial class UgovorFakturisanje

    {
       
        public int Id { get; set; }
        public int? UgovorId { get; set; }
        public int FakturaDana { get; set; }
        public int? RabatProcenat { get; set; }
        public int? RabatMinBrojPaketa { get; set; }
        public int? RabatProcenatZaBrojPaketa { get; set; }
        public int? RabatMinIznosFakture { get; set; }
        public int? RabatProcenatZaIznosFakture { get; set; }
        public int? ValutaDana { get; set; }
        public bool FakturaEmailom { get; set; }
        public bool FakturaPostom { get; set; }
        public bool SpecifikacijaEmailom { get; set; }
        public bool SpecifikacijaPostom { get; set; }
        public bool OdobrenoBezgotovinsko { get; set; }
        public int? KontaktIdZaSlanje { get; set; }       
        public DateTime DatumUnosa { get; set; }

        public virtual Ugovor Ugovor { get; set; }
    }
}
