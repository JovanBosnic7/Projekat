using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.ModelConfiguration;
using Bex.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bex.DAL.EF.Models
{
    public class ReonConfiguration : EntityTypeConfiguration<Reon>
    {
        public ReonConfiguration()
        {
            ToTable("Reon");

            HasKey(e => e.Id);

            Property(e => e.Id)
                .HasColumnName("IdReona");

            Property(e => e.OznReona)
                .IsRequired()
                .HasMaxLength(3);

            Property(e => e.NazivReona)
                .HasMaxLength(50);

            Property(e => e.BarKodReona)
                .HasMaxLength(2);

            Property(p => p.RegionId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None)// by default
                .HasColumnName("RegionId");

            Property(p => p.Tip)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None)// by default
                .HasColumnName("Tip");

            HasRequired(c => c.Region)
                .WithMany(p => p.Reons)
                .HasForeignKey(c => c.RegionId);


        }
    }
}