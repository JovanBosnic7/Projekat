using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.ModelConfiguration;
using Bex.Models;

namespace Bex.DAL.EF.Models
{
    public class PaketZadatakConfiguration: EntityTypeConfiguration<PaketZadatak>
    {
        public PaketZadatakConfiguration()
        {
            ToTable("PaketZadatak");

            HasKey(e => e.Id);

            Property(e => e.Id)
                .HasColumnName("IdPaketZadatak");

            HasOptional(e => e.Paket)
            .WithMany(e => e.PaketZadatak)
            .HasForeignKey(e => e.IdPaketa)
            .WillCascadeOnDelete(false);

            HasOptional(e => e.Zona)
            .WithMany(e => e.PaketZadatak)
            .HasForeignKey(e => e.ZonaId)
            .WillCascadeOnDelete(false);

            //HasOptional(e => e.User)
            //.WithMany(e => e.PaketZadatak)
            //.HasForeignKey(e => e.IzvrsioId)
            //.WillCascadeOnDelete(false);
        }
    }
}