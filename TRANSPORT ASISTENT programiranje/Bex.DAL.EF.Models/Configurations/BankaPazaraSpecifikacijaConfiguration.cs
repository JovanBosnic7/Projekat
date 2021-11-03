using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.ModelConfiguration;
using Bex.Models;

namespace Bex.DAL.EF.Models
{
    public class BankaPazaraSpecifikacijaConfiguration: EntityTypeConfiguration<BankaPazaraSpecifikacija>
    {
        public BankaPazaraSpecifikacijaConfiguration()
        {
            ToTable("BankaPazaraSpecifikacija");

            HasKey(e => e.Id);

            
        }
    }
}