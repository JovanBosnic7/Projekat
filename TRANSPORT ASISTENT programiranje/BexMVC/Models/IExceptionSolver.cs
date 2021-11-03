using System;
using System.Web.Mvc;
using Bex.Common;

namespace Bex.MVC.Exceptions
{
    public interface IExceptionSolver
    {
        void PrepareModelState(
            ModelStateDictionary modelState, Exception exception, bool checkExceptionType = true);
        void PrepareModelState(
            ModelStateDictionary modelState, IUowCommandResult commandResult);
        void PrepareTempData(
            TempDataDictionary tempData, ModelStateDictionary modelState);
    }
}
