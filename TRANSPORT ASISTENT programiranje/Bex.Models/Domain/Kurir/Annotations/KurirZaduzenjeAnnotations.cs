using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bex.Models
{
    [MetadataType(typeof(KurirZaduzenjeMetadata))]
    public partial class KurirZaduzenje
    {
        public class KurirZaduzenjeMetadata
        {

            public int Id { get; set; }
            [DataType(DataType.Date)]
            [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
            public DateTime DatumZaduzenja { get; set; }
            [DataType(DataType.Time)]
            [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
            public DateTime VremeZaduzenja { get; set; }
            [ForeignKey("User")]
            public int KurirUserId { get; set; }
            [ForeignKey("Reon")]
            public int ReonId { get; set; }
            
            public int ZonaId { get; set; }

            public object User { get; set; }
            public object Reon { get; set; }
            public object Zona { get; set; }

            private KurirZaduzenjeMetadata() { }
        }
    }
}
