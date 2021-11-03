using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bex.Models
{
    [MetadataType(typeof(VozniParkModelMetadata))]
    public partial class VozniParkModel
    {
        public class VozniParkModelMetadata
        {
           
            public int Id { get; set; }

            public string NazivModela { get; set; }
            [ForeignKey("VozniParkMarka")]
            public int MarkaId { get; set; }

            public object VozniParkMarka { get; set; }
            private VozniParkModelMetadata() { }
        }
    }
}