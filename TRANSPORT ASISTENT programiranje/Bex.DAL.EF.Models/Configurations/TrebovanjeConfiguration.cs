using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration;
using System.ComponentModel.DataAnnotations.Schema;
using Bex.Models;

namespace Bex.DAL.EF.Models
{
    public class TrebovanjeConfiguration: EntityTypeConfiguration<Trebovanje>
    {
        public TrebovanjeConfiguration()
        {
            ToTable("Trebovanje");

            HasKey(z => z.IdTrebovanja);

            Property(p => p.IdTrebovanja)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity)// has to be but in db will be non-identity
                .HasColumnName("IdTrebovanja");


        }
    }
}
