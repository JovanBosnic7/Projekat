using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bex.Models
{
    [MetadataType(typeof(UgovorObracunCenaMetadata))]
    public partial class UgovorObracunCena
    {
        public class UgovorObracunCenaMetadata
        {
            public int Id { get; set; }
            public int UgovorId { get; set; }
            public int? PovlasceneCeneKlijentuId { get; set; }
            public int? NaplataId { get; set; }
            public bool BezZastitneCene { get; set; }
            public int? ZastitnaCena { get; set; }
            public bool NaplataPoFakturi { get; set; }
            public int? FakturaProc { get; set; }
            public int? ZastitnaCenaFak { get; set; }
            public int? DodatakZaGorivo { get; set; }
            public DateTime DatumUnosa { get; set; }

            public object Ugovor { get; set; }
            public object UgovorNacinNaplate { get; set; }
            public object UgovorPovlascenaCenaTip { get; set; }

            private UgovorObracunCenaMetadata() { }
        }
    }
}