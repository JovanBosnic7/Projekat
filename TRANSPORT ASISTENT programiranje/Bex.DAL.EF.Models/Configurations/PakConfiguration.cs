
namespace Bex.DAL.EF.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Data.Entity.ModelConfiguration;
    using System.ComponentModel.DataAnnotations.Schema;
    using Bex.Models;

    public class PakConfiguration : EntityTypeConfiguration<PAK>
    {
        public PakConfiguration()
        {
            

            ToTable("Pak");

            HasKey(e => e.Id);

            Property(e => e.Id)
                .HasColumnName("IdPak");

  
            Property(e => e.Napomena)
                .HasMaxLength(255);

            HasOptional(e => e.Reon)
                .WithMany(e => e.PAKs)
                .HasForeignKey(e => e.ReonId)
                .WillCascadeOnDelete(false);

            HasOptional(e => e.Reoncic)
                .WithMany(e => e.PAKs)
                .HasForeignKey(e => e.ReoncicId)
                .WillCascadeOnDelete(false);

            HasOptional(e => e.Street)
                .WithMany(e => e.PAKs)
                .HasForeignKey(e => e.UlicaId)
                .WillCascadeOnDelete(false);

            HasOptional(e => e.Place)
                .WithMany(e => e.PAKs)
                .HasForeignKey(e => e.MestoId)
                .WillCascadeOnDelete(false);

            HasOptional(e => e.Zip)
                .WithMany(e => e.PAKs)
                .HasForeignKey(e => e.PttId)
                .WillCascadeOnDelete(false);


        }
    }


}