using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bex.Models
{
    [MetadataType(typeof(PaketZadatakMetadata))]
    public partial class PaketZadatak
    {
        public class PaketZadatakMetadata
        {

            public int Id { get; set; }
            [ForeignKey("Paket")]
            public int IdPaketa { get; set; }
            //[ForeignKey("Posiljka")]
            public int PosiljkaId { get; set; }
            public string Tip { get; set; }
            [ForeignKey("Zona")]
            public int ZonaId { get; set; }
            [DataType(DataType.Date)]
            [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
            public DateTime DatumIzvrsenja { get; set; }
            [DataType(DataType.Time)]
            [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
            public DateTime VremeIzvrsenja { get; set; }
            [ForeignKey("User")]
            public int IzvrsioId { get; set; }

            public object Paket { get; set; }
            public object Zona { get; set; }
            public object User { get; set; }

            private PaketZadatakMetadata() { }
        }
    }
}