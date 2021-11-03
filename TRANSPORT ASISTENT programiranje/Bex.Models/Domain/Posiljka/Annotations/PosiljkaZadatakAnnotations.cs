using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bex.Models
{
    [MetadataType(typeof(PosiljkaZadatakMetadata))]
    public partial class PosiljkaZadatak
    {
        public class PosiljkaZadatakMetadata
        {
            public int Id { get; set; }
            [ForeignKey("Posiljka")]
            public int PosiljkaId { get; set; }
            public string Tip { get; set; }
            [DataType(DataType.Date)]
            [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
            public DateTime DatumEvidentiranja { get; set; }
            [DataType(DataType.Time)]
            [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
            public TimeSpan? VremeEvidentiranja { get; set; }
            [DataType(DataType.Date)]
            [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
            public DateTime DatumMin { get; set; }
            [DataType(DataType.Date)]
            [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
            public DateTime DatumMax { get; set; }
            [DataType(DataType.Time)]
            [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
            public TimeSpan? VremeMin { get; set; }
            [DataType(DataType.Time)]
            [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
            public TimeSpan? VremeMax { get; set; }
            public int NajavaMinuta { get; set; }
            public string KontaktIme { get; set; }
            public string KontaktTelefon { get; set; }
            public string KontaktTelefon2 { get; set; }
            public string Napomena { get; set; }
            public string NapomenaKasnjenjeLast { get; set; }
            public int Status { get; set; }
            [ForeignKey("Reon")]
            public int ReonId { get; set; }
            public int AdresaId { get; set; }
            public string Adresa { get; set; }
            [DataType(DataType.Date)]
            [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
            public DateTime DatumIzvrsenja { get; set; }
            [DataType(DataType.Time)]
            [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
            public TimeSpan? VremeIzvrsenja { get; set; }
            public int IzvrsioId { get; set; }
            public bool IzvrsitiUmagacinu { get; set; }
            [DataType(DataType.Date)]
            [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
            public DateTime AktuelanOd { get; set; }
            public int SubId { get; set; }
            public string NazivKlijenta { get; set; }
            public int PakId { get; set; }

            public object Posiljka { get; set; }
            public object Reon { get; set; }
            public object Kontakt { get; set; }
            public object User { get; set; }
            public object PAK { get; set; }

            private PosiljkaZadatakMetadata() { }
        }
    }
}
