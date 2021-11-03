using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Bex.Models
{
    [MetadataType(typeof(OpstinaMetadata))]
    public partial class Opstina
    {
        public class OpstinaMetadata
        {
           
            public int Id { get; set; }

            public string OpstinaNaziv { get; set; }

            private OpstinaMetadata() { }
        }
    }

}