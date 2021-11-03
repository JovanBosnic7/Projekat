using System;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Bex.DAL.EF.Conventions
{
    public class IdOrKeyKeyDiscoveryConvention : Convention
    {
        public IdOrKeyKeyDiscoveryConvention()
        {
            Properties()
                .Where(p => p.Name == "Id" || p.Name == "Key")
                .Configure(p => p.IsKey());
        }
    }
}
