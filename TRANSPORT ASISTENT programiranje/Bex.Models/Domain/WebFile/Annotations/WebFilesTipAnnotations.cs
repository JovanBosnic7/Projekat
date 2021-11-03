using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Bex.Models
{
    [MetadataType(typeof(WebFilesTipMetadata))]
    public partial class WebFilesTip
    {
        public class WebFilesTipMetadata
        {
           
            public int Id { get; set; }
           
            public string Naziv { get; set; }

            private WebFilesTipMetadata() { }
        }
    }
}