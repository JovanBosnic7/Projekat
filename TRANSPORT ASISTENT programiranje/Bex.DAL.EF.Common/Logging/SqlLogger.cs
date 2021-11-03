using Bex.DAL.EF.Common;
using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.DependencyResolution;
using System.Data.Entity.Infrastructure.Interception;
using System.Diagnostics;
using System.IO;


namespace Bex.DAL.EF.Logging
{
    public class SqlLogger : ISqlLogger, IDbConfigurationInterceptor,
        IDisposable, IDbInterceptor
    {
        public bool IsOverwritting { get; set; }
        public string LogPath { get; set; }

        public void StartLogging()
        {
            if (LogPath != null)
            {
                StreamWriter = StreamWriter ?? GetStreamWriter();
                WriteLog = s => StreamWriter?.Write(s);
            }
            else
            { WriteLog = s => Debug.Write(s); } // s => Console.Write(s);

            //SetFormatter();
        }

        public void StopLogging()
        {
            StreamWriter?.Dispose();
            StreamWriter = null;

            WriteLog = s => { };

            //SetFormatter();
        }

        public void Loaded(DbConfigurationLoadedEventArgs loadedEventArgs, DbConfigurationInterceptionContext interceptionContext)
        {
            FormatterFactory = loadedEventArgs
                .DependencyResolver
                .GetService<Func<DbContext, Action<string>, DatabaseLogFormatter>>();

            StartLogging();
        }

        public void Dispose()
        { StreamWriter?.Dispose(); }

        private StreamWriter GetStreamWriter()
        {
            StreamWriter streamWriter = null;

            try
            {
                if (IsOverwritting)
                { streamWriter = File.CreateText(LogPath); }
                else
                { streamWriter = File.AppendText(LogPath); }
            }
            catch (IOException)
            { throw; }
            catch (Exception)
            { throw; }

            if (streamWriter != null)
            { streamWriter.AutoFlush = true; }

            return streamWriter;
        }

        private void SetFormatter()
        {
            if (Formatter != null)
            { DbInterception.Remove(Formatter); }

            Formatter = FormatterFactory(null, WriteLog);
            DbInterception.Add(Formatter);
        }

        private Func<DbContext, Action<string>, DatabaseLogFormatter> FormatterFactory { get; set; }
        private DatabaseLogFormatter Formatter { get; set; }
        private Action<string> WriteLog { get; set; }
        private StreamWriter StreamWriter { get; set; }
    }
}
