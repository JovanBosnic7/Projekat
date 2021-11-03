using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bex.Models
{
    [MetadataType(typeof(PrijavaNapomenaMetadata))]
    public partial class PrijavaNapomena
    {
        public class PrijavaNapomenaMetadata
        {

            public int Id { get; set; }
            [ForeignKey("PrijavaReklamacijaZalba")]
            public int? PrijavaId { get; set; }
            public DateTime? DatumUnosa { get; set; }
            [ForeignKey("KorisniciPrograma")]
            public int? UserUnosaId { get; set; }
            public string Napomena { get; set; }

            public object PrijavaReklamacijaZalba { get; set; }
            public object KorisniciPrograma { get; set; }

            private PrijavaNapomenaMetadata() { }
        }
    }
}