using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Bex.Models
{
    [MetadataType(typeof(GalleryMetadata))]
    public partial class Gallery
    {
        public class GalleryMetadata
        {
           
            public int Id { get; set; }
            public string Title { get; set; }
            public int WebImageId { get; set; }

            public bool IsActive { get; set; }

            public bool IsProfile { get; set; }
            public int? OrderNo { get; set; }

            private GalleryMetadata() { }
        }
    }
}