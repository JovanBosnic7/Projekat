using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace Bex.Models
{
    [MetadataType(typeof(StatKasnjenjaMetadata))]

    public partial class StatKasnjenja
    {
        public class StatKasnjenjaMetadata
        {
            public int Id { get; set; }
            public DateTime Datum { get; set; }
            public string Region { get; set; }
            public int UkupnoP { get; set; }
            public int KasniP { get; set; }
            public decimal ProcPreuzimanja { get; set; }
            public int UkupnoD { get; set; }
            public int KasniD { get; set; }
            public decimal ProcDostave { get; set; }
            public int UkupnoN { get; set; }
            public int KasniN { get; set; }
            public decimal ProcDostaveNzadataka { get; set; }
            public int UkupnoT { get; set; }
            public int KasniT { get; set; }
            public decimal ProcDostaveTzadataka { get; set; }
            public int KasniDviseDana { get; set; }
            public int KasniOtkupa { get; set; }
            public int KasniPovrata { get; set; }
            public int KasniOtpremnica { get; set; }
            public int KasniPovratnica { get; set; }
            public int KasniCekova { get; set; }
            public decimal Nedeljni { get; set; }
            public decimal Mesecni { get; set; }

            private StatKasnjenjaMetadata() { }

        }
    }

}