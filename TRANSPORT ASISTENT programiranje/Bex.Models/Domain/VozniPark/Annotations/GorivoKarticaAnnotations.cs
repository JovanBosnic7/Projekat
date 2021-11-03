using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bex.Models
{
    [MetadataType(typeof(GorivoKarticaMetadata))]
    public partial class GorivoKartica
    {
        public class GorivoKarticaMetadata
        {

            public int Id { get; set; }

            public string NazivKartice { get; set; }
            [ForeignKey("GorivoPumpa")]
            public int PumpaId { get; set; }
            [ForeignKey("VozniPark")]
            public int VpId { get; set; }

            public DateTime DatumProizvodnje { get; set; }

            public DateTime DatumIsteka { get; set; }

            public string Pincode { get; set; }
            public string BrojKartice { get; set; }
            public bool Storno { get; set; }
            public object GorivoPumpa { get; set; }
            public object VozniPark { get; set; }

            private GorivoKarticaMetadata() { }
        }
    }
}
