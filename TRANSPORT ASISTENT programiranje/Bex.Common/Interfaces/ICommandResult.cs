using System;

namespace Bex.Common
{
    public interface ICommandResult
    {
        bool IsSuccessful { get; set; }
        Exception Exception { get; set; }
    }
}
