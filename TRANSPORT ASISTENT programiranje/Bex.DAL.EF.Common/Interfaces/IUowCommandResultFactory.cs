using System;

namespace Bex.Common
{
    public interface IUowCommandResultFactory
    {
        IUowCommandResult Invoke(Func<int> submitChanges);
    }
}
