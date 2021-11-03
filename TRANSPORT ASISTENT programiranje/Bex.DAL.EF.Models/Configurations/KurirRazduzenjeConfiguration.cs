using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.ModelConfiguration;
using Bex.Models;

namespace Bex.DAL.EF.Models
{
    public class KurirRazduzenjeConfiguration : EntityTypeConfiguration<KurirRazduzenje>
    {
        public KurirRazduzenjeConfiguration()
        {
            ToTable("KurirRazduzenje");

            HasKey(e => e.Id);

            Property(e => e.Id)
                .HasColumnName("IdRazduzenja");

            HasOptional(e => e.Reon)
            .WithMany(e => e.KurirRazduzenje)
            .HasForeignKey(e => e.ReonId)
            .WillCascadeOnDelete(false);

            HasOptional(e => e.UserKurir) //tetka zakomentarisala, ccko vratio. treba pogledati
            .WithMany(e => e.KurirRazduzenje)
            .HasForeignKey(e => e.KurirId)
            .WillCascadeOnDelete(false);

        }
    }
}