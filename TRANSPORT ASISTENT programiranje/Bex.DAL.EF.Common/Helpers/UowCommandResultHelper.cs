using System;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using Demos.Club.Common.Interfaces;
using Demos.Club.DAL.EF.Common.Interfaces;
using Demos.Club.DAL.EF.Common.Models;

namespace Demos.Club.DAL.EF.Common.Helpers
{
    public class UowCommandResultHelper : IUowCommandResultHelper
    {
        public IUowCommandResult Invoke(Func<int> f)
        {
            var uowCommandResult = new UowCommandResult();

            try
            {
                uowCommandResult.RecordsAffected = f();
                uowCommandResult.IsSuccessful = uowCommandResult.RecordsAffected > 0;
                var recordsAffected = uowCommandResult.RecordsAffected;
                uowCommandResult.Description = $"{recordsAffected} rows affected.";
                if (recordsAffected == 0)
                {
                    uowCommandResult.Description = "UowHandledError_NoException";
                    uowCommandResult.Exception = new Exception($"{recordsAffected} rows affected.");
                }
            }
            catch (DbEntityValidationException exception)
            {
                uowCommandResult.Description = "EfHandledError_DbEntityValidationException";
                uowCommandResult.Exception = exception;
            }
            catch (DbUpdateException exception)
            {
                uowCommandResult.Description = "SqlServerHandledError_DbUpdateException";
                uowCommandResult.Exception = GetMostInnerException(exception);
            }
            catch (SqlException exception)
            {
                uowCommandResult.Description = "SqlServerHandledError_SqlException";
                uowCommandResult.Exception = exception;
                for (int i = 0; i < exception.Errors.Count; i++)
                { uowCommandResult.Errors.Add("", exception.Errors[i].ToString()); }
            }
            catch (Exception exception)
            {
                uowCommandResult.Description = "UnexpectedException";
                uowCommandResult.Exception = exception;
            }

            return uowCommandResult;
        }

        private static Exception GetMostInnerException(Exception exception)
        {
            while (exception.InnerException != null)
            { exception = exception.InnerException; }

            return exception;
        }
    }
}
