using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bex.Models
{
    [MetadataType(typeof(BankaPazaraMetadata))]

    public partial class BankaPazara
    {
        public class BankaPazaraMetadata
        {
            public int Id { get; set; }
            public DateTime Datum { get; set; }
            [ForeignKey("Region")]
            public int? RegionId { get; set; }
            public decimal? PazarZaUplatu { get; set; }
            public decimal? PazarUplacen { get; set; }
            public decimal? OtkupZaUplatu { get; set; }
            public decimal? OtkupUplacen { get; set; }
            public decimal? OtkupZaIsplatu { get; set; }
            public decimal? OtkupIsplacen { get; set; }

            public object Region { get; set; }
            private BankaPazaraMetadata() { }

        }
    }

}