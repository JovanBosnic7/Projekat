using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.ModelConfiguration;
using Bex.Models;

namespace Bex.DAL.EF.Models
{
    public class UgovorConfiguration: EntityTypeConfiguration<Ugovor>
    {
        public UgovorConfiguration()
        {
            ToTable("Ugovor");

            HasKey(e => e.Id);

            Property(e => e.Id)
                .HasColumnName("IdUgovora");

            //HasRequired(c => c.Kontakt)
            //   .WithMany(p => p.Ugovor)
            //   .HasForeignKey(c => c.KontaktIdNosilac)
            //   .WillCascadeOnDelete();

        }
    }
}