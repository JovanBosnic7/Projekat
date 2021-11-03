using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bex.Models
{

    [MetadataType(typeof(KontaktAdresaMetadata))]
    public partial class KontaktAdresa
    {
        public class KontaktAdresaMetadata
        {

            public int Id { get; set; }
            [ForeignKey("Kontakt")]
            public int KontaktId { get; set; }

            public int PakId { get; set; }

            public string AdresaTxt { get; set; }

            public string BrojTxt { get; set; }

            public int UserUnosId { get; set; }

            public object Kontakt { get; set; }
            //public object Adresa { get; set; }

            private KontaktAdresaMetadata() { }

        }
    }
}
