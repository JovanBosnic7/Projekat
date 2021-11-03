using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.ModelConfiguration;
using Bex.Models;

namespace Bex.DAL.EF.Models
{
    public class ZonaPodTipConfiguration: EntityTypeConfiguration<ZonaPodTip>
    {
        public ZonaPodTipConfiguration()
        {
            ToTable("ZonaPodTip");

            HasKey(e => e.Id);

            //Property(e => e.Id)
            //    .HasColumnName("Id");
            HasOptional(e => e.ZonaTip)
            .WithMany(e => e.ZonaPodTip)
            .HasForeignKey(e => e.TipId)
            .WillCascadeOnDelete(false);
        }
    }
}