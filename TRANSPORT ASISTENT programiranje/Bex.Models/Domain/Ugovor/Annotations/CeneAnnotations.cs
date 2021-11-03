using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bex.Models
{
    [MetadataType(typeof(CeneMetadata))]
    public partial class Cene
    {
        public class CeneMetadata
        {

            public int IdCene { get; set; }
            [ForeignKey("Cenovnik")]
            public int? CenovniRazredId { get; set; }

            public int? VrstaUslugeId { get; set; }
            //[ForeignKey("PosiljkaKategorija")] 
            //public int? KategorijaId { get; set; }
            public decimal? CenaMin { get; set; }
            public decimal? CenaProc { get; set; }
            public decimal? PopustProc { get; set; }
            public bool Nevazece { get; set; }

            public object Cenovnik { get; set; }
            //public object PosijkaKategorija { get; set; }

            private CeneMetadata() { }
        }
    }
}