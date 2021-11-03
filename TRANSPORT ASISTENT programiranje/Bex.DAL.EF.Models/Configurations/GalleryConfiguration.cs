using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.ModelConfiguration;
using Bex.Models;

namespace Bex.DAL.EF.Models
{
    public class GalleryConfiguration: EntityTypeConfiguration<Gallery>
    {
        public GalleryConfiguration()
        {
            ToTable("xxxGallery");

            HasKey(e => e.Id);

           
        }
    }
}