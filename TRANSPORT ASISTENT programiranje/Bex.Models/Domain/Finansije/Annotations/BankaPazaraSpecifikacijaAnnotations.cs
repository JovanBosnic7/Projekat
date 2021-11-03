using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bex.Models
{
    [MetadataType(typeof(BankaPazaraSpecifikacijaMetadata))]

    public partial class BankaPazaraSpecifikacija
    {
        public class BankaPazaraSpecifikacijaMetadata
        {
            public int Id { get; set; }
            public DateTime DatumPazara { get; set; }
            public string TipZadatka { get; set; }
            public int? RegionUplateId { get; set; }
            public decimal PazarZaUplatu { get; set; }
            public decimal OtkupZaUplatu { get; set; }
            public int? RegionIsplateId { get; set; }
            public decimal OtkupZaIsplatu { get; set; }

            private BankaPazaraSpecifikacijaMetadata() { }

        }
    }

}