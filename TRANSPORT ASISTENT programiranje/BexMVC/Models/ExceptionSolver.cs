using System;
using System.Linq;
using System.Web.Mvc;


namespace Bex.MVC.Exceptions
{
    public class ExceptionSolver : IExceptionSolver
    {
        public void PrepareModelState(
            ModelStateDictionary modelState, Exception exception, bool checkExceptionType = true)
        {
            while (exception != null)
            {
                if (checkExceptionType)
                { modelState.AddModelError("", $"System_Handled_{exception.GetType().Name}"); }

                modelState.AddModelError("", exception.Message);

                exception = exception.InnerException;
            }
        }

        public void PrepareModelState(
            ModelStateDictionary modelState, Bex.Common.IUowCommandResult commandResult)
        {
            modelState.AddModelError("", commandResult.Description);

            if (commandResult.Errors.Count > 0)
            {
                // in production this should be part of description
                modelState.AddModelError(
                    "",
                    $"({commandResult.Exception.GetType().Name}): {commandResult.Exception.Message}");

                commandResult.Errors.ToList()
                    .ForEach(keyValuePair =>
                    {
                        var item = modelState[keyValuePair.Key];

                        if (item == null)
                        { item = modelState[""]; }

                        if (!item.Errors.Any(modelError =>
                            modelError.ErrorMessage == keyValuePair.Value))
                        { item.Errors.Add(keyValuePair.Value); }
                    });
            }
            else
            { PrepareModelState(modelState, commandResult.Exception, false); }
        }

        public void PrepareTempData(
            TempDataDictionary tempData, ModelStateDictionary modelState)
        {
            modelState.Values.ToList().ForEach(value =>
                value.Errors.ToList().ForEach(error =>
                    tempData.Add(error.ErrorMessage.ToString(), error.ErrorMessage.ToString())));
        }
    }
}