using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bex.Models
{
    [MetadataType(typeof(VozniParkMetadata))]
    public partial class VozniPark
    {

        public class VozniParkMetadata
        {

            public int Id { get; set; }
            public string GarazniBroj { get; set; }         
            public string Oznaka { get; set; }
            public string Napomena { get; set; }
            public DateTime? DatumRegistracije { get; set; }
            [ForeignKey("VozniParkModel")]
           
            public int ModelId { get; set; }
            public string Sasija { get; set; }
            public string Motor { get; set; }
            [ForeignKey("VozniParkMenjac")]
            public int MenjacId { get; set; }
            public decimal PropisanaPotrosnja { get; set; }
            public int? GodinaProizvodnje { get; set; }
            public int Nosivost { get; set; }
            public int Masa { get; set; }
            public int BrojVrata { get; set; }
            public decimal Snaga { get; set; }
            public int Zapremina { get; set; }
            public string TipVozila { get; set; }
            public int? KmNabavna { get; set; }
            [ForeignKey("Gorivo")]
            public int GorivoId { get; set; }
            [ForeignKey("VozniParkKaroserija")]
            public int KaroserijaId { get; set; }
            [ForeignKey("VozniParkPodKaroserija")]
            public int? PodKaroserijaId { get; set; }
            public int NDM { get; set; }
            [ForeignKey("VozniParkStatus")]
            public int StatusVozilaId { get; set; }
            [ForeignKey("VozniParkKategorija")]
            public int? KategorijaId { get; set; }

            public int? BrMestaSedenje { get; set; }
            public int? BrMestaStajanje { get; set; }
            public string PogonskiPneumatici { get; set; }
            public string UpravljackiPneumatici { get; set; }
            public int? FirmaId { get; set; }
            public bool Aktivno { get; set; }
            public string Sifra { get; set; }


            public object Gorivo { get; set; }
            public object VozniParkKaroserija { get; set; }
            public object VozniParkPodKaroserija { get; set; }
            public object VozniParkMenjac { get; set; }
            public object VozniParkKategorija { get; set; }
            public object VozniParkModel { get; set; }
            public object VozniParkStatus { get; set; }

            private VozniParkMetadata() { }
        }
    }
}
