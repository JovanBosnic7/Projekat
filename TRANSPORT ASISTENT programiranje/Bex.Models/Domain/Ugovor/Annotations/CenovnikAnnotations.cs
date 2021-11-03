using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bex.Models
{
    [MetadataType(typeof(CenovnikMetadata))]
    public partial class Cenovnik
    {
        public class CenovnikMetadata
        {

            public int? IdCenovnika { get; set; }
            public int? BrojCenovnika { get; set; }
            public DateTime? DatumCenovnika { get; set; }
            public string OpisCenovnika { get; set; }
            public bool Storno { get; set; }
            //public string KorisnikCenovnika { get; set; }

            private CenovnikMetadata() { }
        }
    }
}