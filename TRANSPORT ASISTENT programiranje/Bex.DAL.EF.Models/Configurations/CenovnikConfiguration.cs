using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.ModelConfiguration;
using Bex.Models;

namespace Bex.DAL.EF.Models
{
    public class CenovnikConfiguration : EntityTypeConfiguration<Cenovnik>
    {
        public CenovnikConfiguration()
        {
            ToTable("Cenovnik");

            HasKey(e => e.IdCenovnika);

            Property(e => e.IdCenovnika)
                .HasColumnName("IdCenovnika");
        }
    }
}