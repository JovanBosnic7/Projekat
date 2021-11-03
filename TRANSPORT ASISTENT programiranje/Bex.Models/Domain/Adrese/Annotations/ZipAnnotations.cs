using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Bex.Models
{
    [MetadataType(typeof(ZipMetadata))]
    public partial class Zip
    {
        public class ZipMetadata
        {
           
            public int Id { get; set; }

            public int? ZipValue { get; set; }

            private ZipMetadata() { }
        }
    }
}