using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bex.Models
{
    [MetadataType(typeof(KurirRazduzenjeSpecifikacijaMetadata))]
    public partial class KurirRazduzenjeSpecifikacija
    {
        public class KurirRazduzenjeSpecifikacijaMetadata
        {
            public int Id { get; set; }
            public int? KurirId { get; set; }
            public int? ReonId { get; set; }
            public int? PosiljkaId { get; set; }
            public string TipZadatka { get; set; }
            public bool Status { get; set; }
            public int? UtovarenoPaketa { get; set; }
            public decimal? Otkup { get; set; }
            public decimal? Usluga { get; set; }
            public int? NapomenaPodTipId { get; set; }
            public DateTime Datum { get; set; }
            //public string Adresa { get; set; }
            public object Posiljka { get; set; }
            public object Reon { get; set; }

            private KurirRazduzenjeSpecifikacijaMetadata() { }
        }
    }
}
