using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.ModelConfiguration;
using Bex.Models;

namespace Bex.DAL.EF.Models
{
    public class KurirRazduzenjeSpecifikacijaConfiguration : EntityTypeConfiguration<KurirRazduzenjeSpecifikacija>
    {
        public KurirRazduzenjeSpecifikacijaConfiguration()
        {
            ToTable("KurirRazduzenjeSpecifikacija");

            HasKey(e => e.Id);

           
            HasOptional(e => e.Reon)
            .WithMany(e => e.KurirRazduzenjeSpecifikacija)
            .HasForeignKey(e => e.ReonId)
            .WillCascadeOnDelete(false);

            HasOptional(e => e.User)
            .WithMany(e => e.KurirRazduzenjeSpecifikacija)
            .HasForeignKey(e => e.KurirId)
            .WillCascadeOnDelete(false);

            HasOptional(e => e.Posiljka)
            .WithMany(e => e.KurirRazduzenjeSpecifikacija)
            .HasForeignKey(e => e.PosiljkaId)
            .WillCascadeOnDelete(false);
        }
    }
}