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
    public class KorisniciProgramaConfiguration : EntityTypeConfiguration<KorisniciPrograma>
    {
        public KorisniciProgramaConfiguration()
        {
            ToTable("KorisniciPrograma");

            HasKey(z => z.Id);

            Property(p => p.Id)
                //.HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity)// has to be but in db will be non-identity
                .HasColumnName("Id");

            //Property(p => p.KontaktId)
            //    .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None)// by default
            //    .HasColumnName("KontaktId");            

            //HasRequired(c => c.Kontakt)
            //.WithMany(p => p.User)
            //.HasForeignKey(c => c.KontaktId)
            //.WillCascadeOnDelete();

        }
    }
}
