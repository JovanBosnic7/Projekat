using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.ModelConfiguration;
using Bex.Models;

namespace Bex.DAL.EF.Models
{
    public class StatistikaSatConfiguration: EntityTypeConfiguration<StatistikaSat>
    {
        public StatistikaSatConfiguration()
        {
            ToTable("StatistikaSat");

            HasKey(e => e.Id);
            
        }
    }
}