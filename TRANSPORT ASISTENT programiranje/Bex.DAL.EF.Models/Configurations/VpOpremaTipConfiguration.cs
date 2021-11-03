using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.ModelConfiguration;
using Bex.Models;

namespace Bex.DAL.EF.Models
{
    public class VpOpremaTipConfiguration: EntityTypeConfiguration<VpOpremaTip>
    {
        public VpOpremaTipConfiguration()
        {
            ToTable("VpOpremaTip");

            HasKey(e => e.Id);

            Property(e => e.Id)
                .HasColumnName("IdTipa");
        }
    }
}