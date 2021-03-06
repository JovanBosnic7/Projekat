using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bex.Models
{
    [MetadataType(typeof(PosiljkaOtkupZbirniMetadata))]
    public partial class PosiljkaOtkupZbirni
    {
        public class PosiljkaOtkupZbirniMetadata
        {

            public int Id { get; set; }
            public string BarKod { get; set; }
            public int? ReonId { get; set; }
            public string NazivKlijenta { get; set; }
            public string Adresa { get; set; }
            public string Telefon { get; set; }
            public int? SubIdZaIzvestaje { get; set; }
            public string PoslednjaNapomena { get; set; }
            public decimal? UkupnoOtkupa { get; set; }
            public int? BrojOtkupa { get; set; }
            public DateTime Stamp { get; set; }
            public int? IzvrsioId { get; set; }
            public DateTime? DatumIzvrsenja { get; set; }
            public TimeSpan? VremeIzvrsenja { get; set; }

            public object User { get; set; }
            public object Reon { get; set; }

            private PosiljkaOtkupZbirniMetadata() { }
        }
    }
}
