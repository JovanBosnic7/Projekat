using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.ModelConfiguration;
using Bex.Models;

namespace Bex.DAL.EF.Models
{
    public class UgovorKontaktBexConfiguration: EntityTypeConfiguration<UgovorKontaktBex>
    {
        public UgovorKontaktBexConfiguration()
        {
            ToTable("UgovorKontaktBex");

            HasKey(e => e.Id);

            Property(e => e.Id)
                .HasColumnName("IdKontaktSubBex");

            HasOptional(e => e.Ugovor)
            .WithMany(e => e.UgovorKontaktBex)
            .HasForeignKey(e => e.UgovorId);

        }
    }
}