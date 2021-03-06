using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bex.Models
{
    [MetadataType(typeof(KorisniciProgramaClaimsMetadata))]
    public partial class KorisniciProgramaClaims
    {
        public class KorisniciProgramaClaimsMetadata
        {

            public int Id { get; set; }
            public string Name { get; set; }
            public string Type { get; set; }
            public string Value { get; set; }

            public string Description { get; set; }

            public string Filter { get; set; }
            
            private KorisniciProgramaClaimsMetadata() { }
        }
    }
}
