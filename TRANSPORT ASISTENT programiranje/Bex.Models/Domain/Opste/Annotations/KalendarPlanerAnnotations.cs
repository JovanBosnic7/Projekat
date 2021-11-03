using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace Bex.Models
{
    [MetadataType(typeof(KalendarPlanerMetadata))]

    public partial class KalendarPlaner
    {
        public class KalendarPlanerMetadata
        {
 
            public int Id{ get; set; }
            public string Naziv { get; set; }           
            public string Opis { get; set; }
            public DateTime? DatumStart { get; set; }
            public DateTime? DatumEnd { get; set; }
            public int UserId { get; set; }

            public string Color { get; set; }
         
            private KalendarPlanerMetadata() { }

        }
    }

}