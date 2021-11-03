using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace Bex.Common.Interfaces
{
    public interface IClaimsRepository : IDisposable
    {
        Task<List<ClaimsIdentity>> GetClaimsAsync();
    }
}
