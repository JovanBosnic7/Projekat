using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.ModelConfiguration;
using Bex.Models;

namespace Bex.DAL.EF.Models
{
    public class PosiljkaNapomenaConfiguration: EntityTypeConfiguration<PosiljkaNapomena>
    {
        public PosiljkaNapomenaConfiguration()
        {
            ToTable("PosiljkaNapomena");

            HasKey(e => e.Id);

            Property(e => e.Id)
                .HasColumnName("IdPosiljkaNapomena");

            HasOptional(e => e.Posiljka)
            .WithMany(e => e.PosiljkaNapomena)
            .HasForeignKey(e => e.PosiljkaId)
            .WillCascadeOnDelete(false);
        }
    }
}