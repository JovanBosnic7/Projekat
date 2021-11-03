using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.ModelConfiguration;
using Bex.Models;

namespace Bex.DAL.EF.Models
{
    public class PosiljkaPlacanjeConfiguration: EntityTypeConfiguration<PosiljkaPlacanje>
    {
        public PosiljkaPlacanjeConfiguration()
        {
            ToTable("PosiljkaPlacanje");

            HasKey(e => e.Id);

            Property(e => e.Id)
                .HasColumnName("IdPosiljkaPlacanje");

            HasOptional(e => e.Posiljka)
            .WithMany(e => e.PosiljkaPlacanje)
            .HasForeignKey(e => e.PosiljkaId)
            .WillCascadeOnDelete(false);
        }
    }
}