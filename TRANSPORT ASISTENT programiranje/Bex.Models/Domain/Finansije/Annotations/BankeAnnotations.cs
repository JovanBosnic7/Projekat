using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bex.Models
{
    [MetadataType(typeof(BankeMetadata))]

    public partial class Banke
    {
        public class BankeMetadata
        {
            public int Id { get; set; }
            public string Sifra { get; set; }
            public string NazivBanke { get; set; }
            public string BrojRacuna { get; set; }
            public string PocetniBrojRacuna { get; set; }
            public string SifraNav { get; set; }
            public bool ZaOtkupe { get; set; }

            private BankeMetadata() { }

        }
    }

}