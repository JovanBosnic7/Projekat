using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.ModelConfiguration;
using Bex.Models;

namespace Bex.DAL.EF.Models
{
    public class VozniParkTipConfiguration: EntityTypeConfiguration<VozniParkTip>
    {
        public VozniParkTipConfiguration()
        {
            ToTable("VozniParkTip");

            HasKey(e => e.Id);

            Property(e => e.Id)
                .HasColumnName("IdTipa");
        }
    }
}