using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bex.Models
{
    [MetadataType(typeof(ZaposleniNapomenaMetadata))]
    public partial class ZaposleniNapomena
    {
        public class ZaposleniNapomenaMetadata
        {

            public int Id { get; set; }
            [ForeignKey("Zaposleni")]
            public int ZaposleniId { get; set; }

            [ForeignKey("KorisniciPrograma")]
            public int UneoUserId { get; set; }

            public int Napomena { get; set; }

            public DateTime DatumUnosa { get; set; }

            public object Zaposleni { get; set; }
            public object KorisniciPrograma { get; set; }

            private ZaposleniNapomenaMetadata() { }
        }
    }
}