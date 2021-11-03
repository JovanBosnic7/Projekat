using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bex.Models
{
    [MetadataType(typeof(PosiljkaMetadata))]
    public partial class Posiljka
    {
        public class PosiljkaMetadata
        {
            
            public int Id { get; set; }
            [ForeignKey("PosiljkaStatus")]
            public int StatusId { get; set; }
            [DataType(DataType.Date)]
            [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
            public DateTime DatumEvidentiranja { get; set; }
            [DataType(DataType.Time)]
            [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
            public DateTime VremeEvidentiranja { get; set; }
            [ForeignKey("PosiljkaVrsta")]
            public int VrstaPosiljkeId { get; set; }
            [ForeignKey("PosiljkaKategorija")]
            public int KategorijaPosiljkeId { get; set; }
            public int KategorijaIznos { get; set; }
            [ForeignKey("PosiljkaSadrzaj")]
            public int SadrzajId { get; set; }
            public int CenaUkupna { get; set; }
            public int UserDodaoId { get; set; }
            public int NarucilacId { get; set; }
            public int UkupnoPaketa { get; set; }
            public int ZakljucaoId { get; set; }
            //[ForeignKey("Ugovor")]
            public int UgovorId { get; set; }
            //[ForeignKey("Cenovnik")]
            public int CenovnikId { get; set; }
            public bool CenaPoPaketu { get; set; }
            public bool StornoZaKlijente { get; set; }
            public int NacinUnosaId { get; set; }
            public bool Storno { get; set; }
            public bool Arhivirana { get; set; }

            public object PosiljkaStatus { get; set; }
            public object PosiljkaVrsta { get; set; }
            public object PosiljkaKategorija { get; set; }
            public object PosiljkaSadrzaj { get; set; }
            public object PosiljkaUgovor { get; set; }

            //public object UserUneo { get; set; }

            private PosiljkaMetadata() { }
        }
    }
}
