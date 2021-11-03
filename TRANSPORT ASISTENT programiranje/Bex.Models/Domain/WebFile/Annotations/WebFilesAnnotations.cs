using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bex.Models
{
    [MetadataType(typeof(WebFilesMetadata))]
    public partial class WebFiles
    {
        public class WebFilesMetadata
        {
           
            public int Id { get; set; }
            public byte[] Data { get; set; }
            public bool IsActive { get; set; }

            public DateTime UpdateDate { get; set; }
            public string FileName { get; set; }
            public string FileExt { get; set; }

            public int FileLength { get; set; }
            public string ContentType { get; set; }

            public int StraniId { get; set; }
            [ForeignKey("WebFilesTip")]
            public int TypeId { get; set; }
            public int UserUneoId { get; set; }

            public object WebFilesTip { get; set; }

            private WebFilesMetadata() { }
        }
    }
}