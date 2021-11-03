using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bex.Models
{
    [MetadataType(typeof(StreetMetadata))]
    public partial class Street
    {
        public class StreetMetadata
        {            
            public int? IdStari { get; set; }

            
            public int Id { get; set; }

            public string StreetName { get; set; }
            public int PlaceId { get; set; }

            [NotMapped]
            public string MestoNaziv { get; set; }

            public int? UserUnosId { get; set; }

            public int? UserIzmeneId { get; set; }

            public string Comment { get; set; }

            public DateTime? DI { get; set; }

            public object Place { get; set; }

            private StreetMetadata() { }
        }
    }
}