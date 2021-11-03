using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.ModelConfiguration;
using Bex.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bex.DAL.EF.Models
{
    public class UgovorFakturisanjeConfiguration: EntityTypeConfiguration<UgovorFakturisanje>
    {
        public UgovorFakturisanjeConfiguration()
        {
            ToTable("UgovorFakturisanje");

            HasKey(e => e.Id);

            Property(p => p.UgovorId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None)// by default
                .HasColumnName("UgovorId");

            HasOptional(e => e.Ugovor)
            .WithMany(e => e.UgovorFakturisanje)
            .HasForeignKey(e => e.UgovorId);
        }
    }
}