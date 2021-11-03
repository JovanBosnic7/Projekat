using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Bex.Models
{
    [MetadataType(typeof(PosiljkaPlacanjeTipMetadata))]
    public partial class PosiljkaPlacanjeTip
    {
        public class PosiljkaPlacanjeTipMetadata
        {
           
            public int Id { get; set; }
            public string Naziv { get; set; }

            private PosiljkaPlacanjeTipMetadata() { }
        }
    }
}