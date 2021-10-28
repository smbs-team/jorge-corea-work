// <copyright file="ControllerExtensions.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASDynamicsCrudCore.Controllers
{
    using System;
    using System.Net;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using PTASDynamicsCrudHelperClasses.Exception;
    using Serilog;

    /// <summary>
    /// This class has methods to be executed for all the controllers.
    /// </summary>
    /// <typeparam name="T">Abstract type returned by controllers.</typeparam>
    public static class ControllerExtensions<T>
        where T : class
    {
        private const string HeaderLogDynamicException = "API Web Dynamics Interface/DynamicException: ";
        private const string HeaderLogSystemException = "API Web Dynamics Interface/System Exception: ";

        /// <summary>
        /// Execute the CRUD actions and check for standard exceptions.
        /// </summary>
        /// <param name="actionToExecute">Action to execute.</param>
        /// <returns>A <see cref="Task{IActionResult}"/> representing the result of the asynchronous operation.</returns>
        internal static async Task<ActionResult<T>> HandleExceptions(Func<Task<ActionResult<T>>> actionToExecute)
        {
            try
            {
                return await actionToExecute();
            }
            catch (DynamicsInterfaceException diex)
            {
                Log.Error($"{HeaderLogDynamicException}: status code: {diex.StatusCode}, message: {diex.Message}, inner exception: {diex.InnerException}");
                ObjectResult result = new ObjectResult(diex.Message)
                {
                    StatusCode = diex.StatusCode,
                };
                return result;
            }
            catch (System.Exception ex)
            {
                Log.Error($"{HeaderLogSystemException}: status code: {(int)HttpStatusCode.InternalServerError}, message: {ex.Message}, inner excepcion: {ex.InnerException})");
                ObjectResult result = new ObjectResult(ex.Message)
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                };

                return result;
            }
        }
    }
}