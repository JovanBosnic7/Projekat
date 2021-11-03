using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.ModelConfiguration;
using Bex.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bex.DAL.EF.Models
{
    public class ReoncicConfiguration: EntityTypeConfiguration<Reoncic>
    {
        public ReoncicConfiguration()
        {
            ToTable("Reoncic");

            HasKey(e => e.Id);

            Property(e => e.Id)
                .HasColumnName("IdReoncica");

            Property(e => e.NazivReoncica)
                .HasMaxLength(50);

            Property(e => e.PreuzimanjeDoDefault)
                .HasColumnType("time");

            Property(e => e.DatumPoslednjeOdjave)
                .HasColumnType("date");

            Property(e => e.VremePoslednjeOdjave)
                .HasColumnType("time");

            Property(e => e.NapomenaOdjave)
                .HasMaxLength(50);

            Property(p => p.ReonId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None)// by default
                .HasColumnName("ReonId");

            HasRequired(c => c.Reon)
                .WithMany(p => p.Reoncic)
                .HasForeignKey(c => c.ReonId);
        }
    }
}