using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.ModelConfiguration;
using Bex.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bex.DAL.EF.Models
{
    public class StreetConfiguration: EntityTypeConfiguration<Street>
    {
        public StreetConfiguration()
        {
            ToTable("Street");

            HasKey(e => e.Id);

            Property(e => e.Id)
                .HasColumnName("IdStreet");

            Property(e => e.Id)
                .IsRequired();

            Property(p => p.PlaceId)
               .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None)// by default
               .HasColumnName("PlaceId");
        }
    }
}