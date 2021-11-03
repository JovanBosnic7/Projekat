using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bex.Models
{
    [MetadataType(typeof(ZaposleniMetadata))]
    public partial class Zaposleni
    {
        public class ZaposleniMetadata
        {

            public int Id { get; set; }
            public string ImeIprezime { get; set; }
            public string Adresa { get; set; }
            public string Jmbg { get; set; }
            public string Pol { get; set; }
            public string Kategorije { get; set; }
            public string BrojLK { get; set; }
            public DateTime? DatumZaposlenja { get; set; }
            public string Telefon { get; set; }
            public string TekuciRacun { get; set; }
            public int? Plata { get; set; }
            public string Napomena { get; set; }
            public bool? Aktivan { get; set; }
            public int? FirmaId { get; set; }
            public string Sifra { get; set; }

            private ZaposleniMetadata() { }
        }
    }
}
