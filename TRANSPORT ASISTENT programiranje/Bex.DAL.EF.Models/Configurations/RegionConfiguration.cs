using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.ModelConfiguration;
using Bex.Models;

namespace Bex.DAL.EF.Models
{
    public class RegionConfiguration:EntityTypeConfiguration<Region>
    {
        public RegionConfiguration()
        {
            ToTable("region");

            HasKey(e => e.Id);

            Property(e => e.Id)
                .HasColumnName("IdRegiona");

            Property(e => e.NazivSkraceni)
                .HasMaxLength(5);

            Property(e => e.NazivRegiona)
                 .HasMaxLength(50);

            Property(e => e.OznRegion)
                 .HasMaxLength(1);

            Property(e => e.BarKodRegiona)
                 .HasMaxLength(1);

        }
    }
}