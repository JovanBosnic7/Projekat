using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bex.Models
{
    [MetadataType(typeof(UgovorFakturisanjeMetadata))]
    public partial class UgovorFakturisanje
    {
        public class UgovorFakturisanjeMetadata
        {

            public int Id { get; set; }
            [ForeignKey("Ugovor")]
            public int UgovorId { get; set; }
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

            public object Ugovor { get; set; }

            private UgovorFakturisanjeMetadata() { }
        }
    }
}