using Bex.Common;
using Bex.DAL.EF.Common.UOW;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;


namespace Bex.DAL.EF.UOW
{
    public class UowCommandResultFactory : IUowCommandResultFactory
    {
        public IUowCommandResult Invoke(Func<int> SaveChanges)
        {
            var uowCommandResult = new UowCommandResult();

            try
            {
                var objectsWritten = SaveChanges();

                uowCommandResult.ObjectsWritten = objectsWritten;
                uowCommandResult.IsSuccessful = objectsWritten > 0;
                uowCommandResult.Description = $"{objectsWritten} objects written.";

                if (objectsWritten == 0)
                {
                    uowCommandResult.Description = "UOW_Handled_NoException";
                    uowCommandResult.Exception = new Exception(
                        "There is nothing for saving or store update, insert, or delete statement affected an unexpected number of rows (0). Entities may have been modified or deleted since entities were loaded.");
                    uowCommandResult.Errors.Add("", "0 objects written.");
                }
            }
            catch (DbEntityValidationException exception)
            {
                uowCommandResult.Description = "EF_Handled_DbEntityValidationException";
                uowCommandResult.Exception = exception.GetBaseException();

                exception.EntityValidationErrors.ToList()
                    .ForEach(dbEntityValidationResult =>
                        dbEntityValidationResult.ValidationErrors.ToList()
                            .ForEach(dbValidationError =>
                            {
                                var propertyName = dbValidationError.PropertyName ?? "";
                                var errorMessage = dbValidationError.ErrorMessage ?? "";

                                var existingErrorforProperty = uowCommandResult.Errors
                                    .FirstOrDefault(e => e.Key == propertyName);
                                if (existingErrorforProperty.Key != null)
                                {
                                    errorMessage += $"{Environment.NewLine}{existingErrorforProperty.Value ?? ""}";
                                    uowCommandResult.Errors.Remove(propertyName);
                                }

                                uowCommandResult.Errors.Add(propertyName, errorMessage);
                            }));
            }
            catch (DbUpdateConcurrencyException exception)
            {
                uowCommandResult.Description = "EF_Handled_DbUpdateConcurrencyException";
                uowCommandResult.Exception = exception.GetBaseException();
                uowCommandResult.Errors.Add("", "0 objects written.");
            }
            catch (DbUpdateException exception)
            {
                uowCommandResult.Description = "SqlServer_Handled_DbUpdateException";
                uowCommandResult.Exception = exception.GetBaseException();

                FillErrors(
                    uowCommandResult.Exception as SqlException,
                    uowCommandResult.Errors);
            }
            catch (SqlException exception)
            {
                uowCommandResult.Description = "SqlServer_Handled_SqlException";
                uowCommandResult.Exception = exception;

                FillErrors(
                    exception,
                    uowCommandResult.Errors);
            }
            catch (Exception exception)
            {
                uowCommandResult.Description =
                    $"Unhandled Exception of type: {exception.GetType().Name}";
                uowCommandResult.Exception = exception;
            }

            return uowCommandResult;
        }

        private void FillErrors(
            SqlException exception, IDictionary<string, string> errors)
        {
            if (exception == null)
            { return; }

            for (int i = 0; i < exception.Errors.Count; i++)
            { errors.Add($"SqlError_{i}", exception.Errors[i].Message); } //.ToString() for full info
        }
    }
}
