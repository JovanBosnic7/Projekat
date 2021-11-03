using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.ModelConfiguration;
using Bex.Models;

namespace Bex.DAL.EF.Models
{
    public class UgovorObracunCenaConfiguration: EntityTypeConfiguration<UgovorObracunCena>
    {
        public UgovorObracunCenaConfiguration()
        {
            ToTable("UgovorObracunCena");

            HasKey(e => e.Id);

            HasOptional(e => e.Ugovor)
            .WithMany(e => e.UgovorObracunCena)
            .HasForeignKey(e => e.UgovorId);
        }
    }
}