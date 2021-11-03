using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Interception;
using System.Linq;

namespace Bex.DAL.EF.Logging
{
    public class SqlLoggerFormatter : DatabaseLogFormatter
    {
        //public SqlLoggerFormatter(Action<string> writeAction)
        //    : base(writeAction)
        //{ }
        public SqlLoggerFormatter(DbContext context, Action<string> writeAction)
            : base(context, writeAction)
        { }

        public override void LogCommand<TResult>(
            DbCommand command, DbCommandInterceptionContext<TResult> interceptionContext)
        {
            var dbContexts = new List<DbContext>(interceptionContext.DbContexts);
            var dbContext = Context?.GetType().Name ??
                dbContexts.FirstOrDefault()?.GetType().Name ?? "DbContext";
            var asyncOrSync = (interceptionContext.IsAsync) ? "async" : "sync";
            Write(
                $"'{dbContext}' is executing {asyncOrSync} command{Environment.NewLine}" +
                $"{command.CommandText}{Environment.NewLine}");
        }

        public override void LogParameter<TResult>(
            DbCommand command, DbCommandInterceptionContext<TResult> interceptionContext, DbParameter parameter)
        { }

        public override void LogResult<TResult>(DbCommand command, DbCommandInterceptionContext<TResult> interceptionContext)
        {
            foreach (DbParameter parameter in command.Parameters)
            { MyLogParameter(command, interceptionContext, parameter); }
            base.LogResult(command, interceptionContext);
        }

        private void MyLogParameter<TResult>(
            DbCommand command, DbCommandInterceptionContext<TResult> interceptionContext, DbParameter parameter)
        { base.LogParameter(command, interceptionContext, parameter); }
    }
}
