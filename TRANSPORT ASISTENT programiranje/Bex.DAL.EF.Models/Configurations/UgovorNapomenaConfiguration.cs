using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.ModelConfiguration;
using Bex.Models;

namespace Bex.DAL.EF.Models
{
    public class UgovorNapomenaConfiguration: EntityTypeConfiguration<UgovorNapomena>
    {
        public UgovorNapomenaConfiguration()
        {
            ToTable("UgovorNapomena");

            HasKey(e => e.Id);

            HasOptional(e => e.Ugovor)
            .WithMany(e => e.UgovorNapomena)
            .HasForeignKey(e => e.UgovorId);

        }
    }
}