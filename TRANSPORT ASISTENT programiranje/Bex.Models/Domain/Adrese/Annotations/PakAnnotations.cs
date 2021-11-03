using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bex.Models
{
    [MetadataType(typeof(PAKMetadata))]

    public partial class PAK
    {
        public class PAKMetadata
        {
            public int Id { get; set; }
            public int? ReoncicId { get; set; }
            [NotMapped]
            public string NazivReoncica { get; set; }

            public int? ReonId { get; set; }
            [NotMapped]
            public string NazivReona { get; set; }

            public int? RegionId { get; set; }
            [NotMapped]
            public string NazivRegiona { get; set; }

            public int? OpstinaId { get; set; }
            [NotMapped]
            public string NazivOpstine { get; set; }

            public int? MestoId { get; set; }
            [NotMapped]
            public string NazivMesta { get; set; }

            public int? UlicaId { get; set; }
            [NotMapped]
            public string NazivUlice { get; set; }

            public int? OdBroja { get; set; }

            public int? DoBroja { get; set; }

            public int? Parnost { get; set; }

            public int? PttId { get; set; }

            public int? PakZaStampu { get; set; }

            [MaxLength(10, ErrorMessage = "Napomena must be 10 characters or less"), MinLength(5)]
            public string Napomena { get; set; }

            public int? UserUnosId { get; set; }

            public int? UserIzmeneId { get; set; }

            public DateTime? DI { get; set; }

            private PAKMetadata() { }

        }
    }

}