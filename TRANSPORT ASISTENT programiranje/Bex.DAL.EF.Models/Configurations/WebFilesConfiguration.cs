using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.ModelConfiguration;
using Bex.Models;

namespace Bex.DAL.EF.Models
{
    public class WebFilesConfiguration : EntityTypeConfiguration<WebFiles>
    {
        public WebFilesConfiguration()
        {
            ToTable("xxxWebFiles");

            HasKey(e => e.Id);

           
        }
    }
}