using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bex.Models
{
    [MetadataType(typeof(KovertaZaDokumentMetadata))]
    public partial class KovertaZaDokument
    {
        public class KovertaZaDokumentMetadata
        {
            public string TipZadatka { get; set; }
            public string RegionReonT { get; set; }
            public string RegionReonD { get; set; }
            public string Naziv { get; set; }
            public string Posiljalac { get; set; }
            public string AdresaP { get; set; }
            public string Primalac { get; set; }
            public string AdresaD { get; set; }
            public string BarKod { get; set; }
            public decimal? Otkupnina { get; set; }
            public int? CenaUkupna { get; set; }
            public int? PosiljkaId { get; set; }
            public string KontaktImeP { get; set; }
            public string KontaktTelefoniP { get; set; }
            public string KontaktImeD { get; set; }
            public string KontaktTelefoniD { get; set; }
            public bool? VipT { get; set; }
            public string Napomena { get; set; }
            public decimal? Otpremnica { get; set; }
            public decimal? Povratnica { get; set; }
            public decimal? PlacenOdgovor { get; set; }
            public decimal? LicnoUrucenje { get; set; }
            public decimal? Vrednost { get; set; }
            public int? KurirId { get; set; }
            public int? Id { get; set; }
            public decimal? KesPosiljalac { get; set; }
            public decimal? KesPrimalac { get; set; }
            public decimal? FakturaPosiljalac { get; set; }
            public decimal? FakturaPrimalac { get; set; }
            public decimal? KesN { get; set; }
            public decimal? Platilac { get; set; }

            private KovertaZaDokumentMetadata() { }
        }
    }
}
