using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bex.Models
{
    [MetadataType(typeof(FirmaVPMetadata))]
    public partial class FirmaVP
    {
        public class FirmaVPMetadata
        {

            public int Id { get; set; }
            public string Naziv { get; set; }

            private FirmaVPMetadata() { }
        }
    }
}
