using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bex.Models
{
    [MetadataType(typeof(PlaceMetadata))]
    public partial class Place
    {
        public class PlaceMetadata
        {
           
            public int Id { get; set; }

            public string PlaceName { get; set; }
            [ForeignKey("Opstina")]
            public int? OpstinaId { get; set; }
            public string Ptt { get; set; }

            public string OznakaMesta { get; set; }
            public object Opstina { get; set; }


            private PlaceMetadata() { }
        }
    }

}