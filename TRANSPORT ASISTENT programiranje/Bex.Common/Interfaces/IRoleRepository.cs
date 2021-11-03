using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace Bex.Common.Interfaces
{
    public interface IRoleRepository : IDisposable
    {
        Task<List<IRole>> GetRolesAsync();
    }
}
