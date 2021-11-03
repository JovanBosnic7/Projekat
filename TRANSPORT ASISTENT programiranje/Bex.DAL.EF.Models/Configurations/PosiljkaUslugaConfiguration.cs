using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.ModelConfiguration;
using Bex.Models;

namespace Bex.DAL.EF.Models
{
    public class PosiljkaUslugaConfiguration: EntityTypeConfiguration<PosiljkaUsluga>
    {
        public PosiljkaUslugaConfiguration()
        {
            ToTable("PosiljkaUsluga");

            HasKey(e => e.Id);

            Property(e => e.Id)
                .HasColumnName("IdPosiljkaUsluga");

            HasOptional(e => e.Posiljka)
            .WithMany(e => e.PosiljkaUsluga)
            .HasForeignKey(e => e.PosiljkaId)
            .WillCascadeOnDelete(false);
        }
    }
}