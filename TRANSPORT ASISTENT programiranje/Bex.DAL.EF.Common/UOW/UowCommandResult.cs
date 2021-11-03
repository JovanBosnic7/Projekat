using Bex.Common;
using System;
using System.Collections.Generic;


namespace Bex.DAL.EF.Common.UOW
{
    public class UowCommandResult : IUowCommandResult,
        ICommandResult
    {
        public UowCommandResult()
        {
            IsSuccessful = false;
            Description = "Default CommandResult";

            Errors = new Dictionary<string, string>();
        }

        public bool IsSuccessful { get; set; }
        public Exception Exception { get; set; }
        public string Description { get; set; }
        public int ObjectsWritten { get; set; }

        public IDictionary<string, string> Errors { get; }
    }
}
