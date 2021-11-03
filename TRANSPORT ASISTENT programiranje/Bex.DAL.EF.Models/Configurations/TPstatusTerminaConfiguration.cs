using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.ModelConfiguration;
using Bex.Models;

namespace Bex.DAL.EF.Models
{
    public class TPstatusTerminaConfiguration: EntityTypeConfiguration<TPstatusTermina>
    {
        public TPstatusTerminaConfiguration()
        {
            ToTable("TPstatusTermina");

            HasKey(e => e.IdStatusa);

            Property(e => e.IdStatusa)
                .HasColumnName("IdStatusa");
        }
    }
}