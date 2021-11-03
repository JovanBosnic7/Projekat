using System;

namespace Bex.Common.Interfaces
{
    public interface ISecurityUow : IDisposable
    {
        ISignInRepository SignInManager { get; }
        IUserRepository UserManager { get; }
        IRoleRepository RoleManager { get; }

        
    }
}
