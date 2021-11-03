
namespace Bex.DAL.EF.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Data.Entity.ModelConfiguration;
    using System.ComponentModel.DataAnnotations.Schema;
    using Bex.Models;

    public class SkenStartConfiguration : EntityTypeConfiguration<SkenStart>
    {
        public SkenStartConfiguration()
        {
            

            ToTable("SkenStart");

            HasKey(e => e.Id);

        }
    }

}