using System;
using System.Collections.Generic;

namespace Bex.Common
{
    public interface IUowCommandResult : ICommandResult
    {
        string Description { get; set; }
        int ObjectsWritten { get; set; }

        IDictionary<string, string> Errors { get; }
    }
}
