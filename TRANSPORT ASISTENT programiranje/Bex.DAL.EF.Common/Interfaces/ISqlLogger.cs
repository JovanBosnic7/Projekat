using System;
using System.Data.Entity.Infrastructure.Interception;

namespace Bex.DAL.EF.Common
{
    public interface ISqlLogger : IDisposable, IDbConfigurationInterceptor, IDbInterceptor
    {
        bool IsOverwritting { get; set; }
        string LogPath { get; set; }

        void StartLogging();
        void StopLogging();
    }
}