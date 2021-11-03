using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.ModelConfiguration;
using Bex.Models;

namespace Bex.DAL.EF.Models
{
    public class UgovorVipConfiguration: EntityTypeConfiguration<UgovorVip>
    {
        public UgovorVipConfiguration()
        {
            ToTable("UgovorVip");

            HasKey(e => e.Id);

            HasOptional(e => e.Ugovor)
            .WithMany(e => e.UgovorVip)
            .HasForeignKey(e => e.UgovorId);

        }
    }
}