using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.ModelConfiguration;
using Bex.Models;

namespace Bex.DAL.EF.Models
{
    public class PaketConfiguration: EntityTypeConfiguration<Paket>
    {
        public PaketConfiguration()
        {
            ToTable("Paket");

            HasKey(e => e.Id);

            Property(e => e.Id)
                .HasColumnName("IdPaketa");

            HasOptional(e => e.Posiljka)
            .WithMany(e => e.Paket)
            .HasForeignKey(e => e.PosiljkaId)
            .WillCascadeOnDelete(false);

            HasOptional(e => e.Zona)
            .WithMany(e => e.Paket)
            .HasForeignKey(e => e.ZonaId)
            .WillCascadeOnDelete(false);
        }
    }
}