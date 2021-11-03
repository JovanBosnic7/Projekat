using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Bex.Models
{
    [MetadataType(typeof(ReoncicMetadata))]
    public partial class Reoncic
    {
        public class ReoncicMetadata
        {
            
            public int Id { get; set; }

            public int? ReonId { get; set; }

            public string NazivReoncica { get; set; }

            public TimeSpan? PreuzimanjeDoDefault { get; set; }

            public bool? OdjavljujeSe { get; set; }

            public DateTime? DatumPoslednjeOdjave { get; set; }

            public TimeSpan? VremePoslednjeOdjave { get; set; }

            public int? Sort { get; set; }

            public bool? DeoMesta { get; set; }

            public string NapomenaOdjave { get; set; }
            public object Reon { get; set; }
            private ReoncicMetadata() { }
        }
    }
   
}