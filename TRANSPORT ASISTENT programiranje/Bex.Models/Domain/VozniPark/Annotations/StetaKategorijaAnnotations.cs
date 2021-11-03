using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bex.Models
{
    [MetadataType(typeof(StetaKategorijaMetadata))]
    public partial class StetaKategorija
    {
        public class StetaKategorijaMetadata
        {

            public int Id { get; set; }
            public string Naziv { get; set; }

            private StetaKategorijaMetadata() { }
        }
    }
}
