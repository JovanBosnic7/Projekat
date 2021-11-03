using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.ModelConfiguration;
using Bex.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bex.DAL.EF.Models
{
    public class PlaceConfiguration: EntityTypeConfiguration<Place>
    {
        public PlaceConfiguration()
        {
            ToTable("Place");

            HasKey(e => e.Id);

            Property(e => e.Id)
                .HasColumnName("IdPlace");

            Property(e => e.Id)
                .IsRequired();

            Property(p => p.OpstinaId)
               .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None)// by default
               .HasColumnName("OpstinaId");
        }
    }
}