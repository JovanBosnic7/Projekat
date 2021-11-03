
namespace Bex.DAL.EF.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Data.Entity.ModelConfiguration;
    using System.ComponentModel.DataAnnotations.Schema;
    using Bex.Models;

    public class SkenConfiguration : EntityTypeConfiguration<Sken>
    {
        public SkenConfiguration()
        {
            

            ToTable("Sken");

            HasKey(e => e.Id);

            HasOptional(e => e.SkenStart)
           .WithMany(e => e.Sken)
           .HasForeignKey(e => e.SkenStartId)
           .WillCascadeOnDelete(false);

        }
    }

}