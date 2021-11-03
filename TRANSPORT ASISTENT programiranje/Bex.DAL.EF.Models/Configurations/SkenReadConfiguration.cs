
namespace Bex.DAL.EF.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Data.Entity.ModelConfiguration;
    using System.ComponentModel.DataAnnotations.Schema;
    using Bex.Models;

    public class SkenReadConfiguration : EntityTypeConfiguration<SkenRead>
    {
        public SkenReadConfiguration()
        {
            

            ToTable("SkenRead");

            HasKey(e => e.Id);

            HasOptional(e => e.Posiljka)
            .WithMany(e => e.SkenRead)
            .HasForeignKey(e => e.PosiljkaId)
            .WillCascadeOnDelete(false);

        }
    }

}