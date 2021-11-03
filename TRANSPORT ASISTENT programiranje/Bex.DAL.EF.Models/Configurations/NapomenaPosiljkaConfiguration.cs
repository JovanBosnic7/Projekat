using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.ModelConfiguration;
using Bex.Models;

namespace Bex.DAL.EF.Models
{
    public class NapomenaPosiljkaConfiguration : EntityTypeConfiguration<NapomenaPosiljka>
    {
        public NapomenaPosiljkaConfiguration()
        {
            ToTable("NapomenaPosiljka");

            HasKey(e => e.Id);

            HasOptional(e => e.Posiljka)
            .WithMany(e => e.NapomenaPosiljka)
            .HasForeignKey(e => e.PosiljkaId)
            .WillCascadeOnDelete(false);

            HasOptional(e => e.User) //tetka zakomentarisala, ccko otkomentarisao. treba proveriti
           .WithMany(e => e.NapomenaPosiljka)
           .HasForeignKey(e => e.UserUnosId)
           .WillCascadeOnDelete(false);

            HasOptional(e => e.NapomenaPosiljkaPodTip)
           .WithMany(e => e.NapomenaPosiljka)
           .HasForeignKey(e => e.PodTipId)
           .WillCascadeOnDelete(false);
        }
    }
}