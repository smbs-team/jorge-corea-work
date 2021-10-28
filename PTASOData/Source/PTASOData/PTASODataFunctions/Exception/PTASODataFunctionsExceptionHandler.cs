namespace PTASODataFunctions.Exception
{
    using System.Net;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Data.SqlClient;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using Microsoft.OData;
    using Newtonsoft.Json;

    /// <summary>
    /// Handles and maps exceptions that come from the different providers to the exceptions that the server should output.  Also logs the internal exception.
    /// </summary>
    public static class PTASODataFunctionsExceptionHandler
    {
        /// <summary>
        /// Maps an untyped exception.
        /// </summary>
        /// <param name="ex">The exception.</param>
        /// <param name="req">The request.</param>
        /// <param name="log">The log.</param>
        /// <returns>The output response for the input exception.</returns>
        /// <exception cref="System.ArgumentNullException">If any of the parameters is NULL.</exception>
        public static IActionResult HandleUntypedException(System.Exception ex, HttpRequest req, ILogger log)
        {
            if (ex == null)
            {
                throw new System.ArgumentNullException(nameof(ex));
            }

            if (req == null)
            {
                throw new System.ArgumentNullException(nameof(req));
            }

            if (log == null)
            {
                throw new System.ArgumentNullException(nameof(log));
            }

            log.LogError(ex, "Function level exception: {0}", ex.ToString());

            ErrorResultModel error = new ErrorResultModel { Message = PTASODataFunctionsErrorMessages.UnhandledExceptionError };
            ObjectResult result = new ObjectResult(error);
            result.StatusCode = (int)HttpStatusCode.InternalServerError;

            return result;
        }

        /// <summary>
        /// Maps an null argument exception.
        /// </summary>
        /// <param name="ex">The exception.</param>
        /// <param name="req">The request.</param>
        /// <param name="log">The log.</param>
        /// <returns>The output response for the input exception.</returns>
        /// <exception cref="System.ArgumentNullException">If any of the parameters is NULL.</exception>
        public static IActionResult HandleArgumentNullException(System.ArgumentNullException ex, HttpRequest req, ILogger log)
        {
            if (ex == null)
            {
                throw new System.ArgumentNullException(nameof(ex));
            }

            if (req == null)
            {
                throw new System.ArgumentNullException(nameof(req));
            }

            if (log == null)
            {
                throw new System.ArgumentNullException(nameof(log));
            }

            log.LogError(ex, "Function level exception: {0}", ex.ToString());

            ErrorResultModel error = new ErrorResultModel
            {
                Message = string.Format(PTASODataFunctionsErrorMessages.ArgumentNullExceptionError, ex.ParamName)
            };

            ObjectResult result = new ObjectResult(error);
            result.StatusCode = (int)HttpStatusCode.BadRequest;

            return result;
        }

        /// <summary>
        /// Maps an odata exception.
        /// </summary>
        /// <param name="ex">The exception.</param>
        /// <param name="req">The request.</param>
        /// <param name="log">The log.</param>
        /// <returns>The output response for the input exception.</returns>
        /// <exception cref="System.ArgumentNullException">If any of the parameters is NULL.</exception>
        public static IActionResult HandleODataException(ODataException ex, HttpRequest req, ILogger log)
        {
            if (ex == null)
            {
                throw new System.ArgumentNullException(nameof(ex));
            }

            if (req == null)
            {
                throw new System.ArgumentNullException(nameof(req));
            }

            if (log == null)
            {
                throw new System.ArgumentNullException(nameof(log));
            }

            log.LogError(ex, "Function level exception: {0}", ex.ToString());

            ErrorResultModel error = new ErrorResultModel
            {
                Message = string.Format(PTASODataFunctionsErrorMessages.ODataExceptionError, ex.Message)
            };

            ObjectResult result = new ObjectResult(error);
            result.StatusCode = (int)HttpStatusCode.BadRequest;

            return result;
        }

        /// <summary>
        /// Maps an argument out of range exception.
        /// </summary>
        /// <param name="ex">The exception.</param>
        /// <param name="req">The request.</param>
        /// <param name="log">The log.</param>
        /// <returns>The output response for the input exception.</returns>
        /// <exception cref="System.ArgumentNullException">If any of the parameters is NULL.</exception>
        public static IActionResult HandleArgumentOutOfRangeException(System.ArgumentOutOfRangeException ex, HttpRequest req, ILogger log)
        {
            if (ex == null)
            {
                throw new System.ArgumentNullException(nameof(ex));
            }

            if (req == null)
            {
                throw new System.ArgumentNullException(nameof(req));
            }

            if (log == null)
            {
                throw new System.ArgumentNullException(nameof(log));
            }

            log.LogError(ex, "Function level exception: {0}", ex.ToString());

            ErrorResultModel error = new ErrorResultModel
            {
                Message = string.Format(PTASODataFunctionsErrorMessages.ArgumentOutOfRangeExceptionError, ex.ParamName)
            };

            ObjectResult result = new ObjectResult(error);
            result.StatusCode = (int)HttpStatusCode.BadRequest;

            return result;
        }

        /// <summary>
        /// Maps a json exception.
        /// </summary>
        /// <param name="ex">The exception.</param>
        /// <param name="req">The request.</param>
        /// <param name="log">The log.</param>
        /// <returns>The output response for the input exception.</returns>
        /// <exception cref="System.ArgumentNullException">If any of the parameters is NULL.</exception>
        public static IActionResult HandleJsonException(JsonException ex, HttpRequest req, ILogger log)
        {
            if (ex == null)
            {
                throw new System.ArgumentNullException(nameof(ex));
            }

            if (req == null)
            {
                throw new System.ArgumentNullException(nameof(req));
            }

            if (log == null)
            {
                throw new System.ArgumentNullException(nameof(log));
            }

            log.LogError(ex, "Function level exception: {0}", ex.ToString());

            ErrorResultModel error = new ErrorResultModel
            {
                Message = ex is JsonSerializationException ?
                    string.Format(PTASODataFunctionsErrorMessages.JsonSerializationExceptionError, ex.Message) :
                    string.Format(PTASODataFunctionsErrorMessages.JsonReaderExceptionError, ex.Message)
            };

            ObjectResult result = new ObjectResult(error);
            result.StatusCode = (int)HttpStatusCode.BadRequest;

            return result;
        }

        /// <summary>
        /// Maps a database update exception.
        /// </summary>
        /// <param name="ex">The exception.</param>
        /// <param name="req">The request.</param>
        /// <param name="log">The log.</param>
        /// <returns>The output response for the input exception.</returns>
        /// <exception cref="System.ArgumentNullException">If any of the parameters is NULL.</exception>
        public static IActionResult HandleDbUpdateException(DbUpdateException ex, HttpRequest req, ILogger log)
        {
            if (ex == null)
            {
                throw new System.ArgumentNullException(nameof(ex));
            }

            if (req == null)
            {
                throw new System.ArgumentNullException(nameof(req));
            }

            if (log == null)
            {
                throw new System.ArgumentNullException(nameof(log));
            }

            log.LogError(ex, "Function level exception: {0}", ex.ToString());

            var exMessage = ex.Message;
            if (ex.InnerException != null)
            {
                exMessage = $"{exMessage} => {ex.InnerException.Message}";
            }

            ErrorResultModel error = new ErrorResultModel
            {
                Message = string.Format(PTASODataFunctionsErrorMessages.DbUpdateExceptionError, exMessage)
            };

            ObjectResult result = new ObjectResult(error);
            result.StatusCode = (int)HttpStatusCode.InternalServerError;

            return result;
        }

        /// <summary>
        /// Maps a sql exception.
        /// </summary>
        /// <param name="ex">The exception.</param>
        /// <param name="req">The request.</param>
        /// <param name="log">The log.</param>
        /// <returns>The output response for the input exception.</returns>
        /// <exception cref="System.ArgumentNullException">If any of the parameters is NULL.</exception>
        public static IActionResult HandleSqlException(SqlException ex, HttpRequest req, ILogger log)
        {
            if (ex == null)
            {
                throw new System.ArgumentNullException(nameof(ex));
            }

            if (req == null)
            {
                throw new System.ArgumentNullException(nameof(req));
            }

            if (log == null)
            {
                throw new System.ArgumentNullException(nameof(log));
            }

            log.LogError(ex, "Function level exception: {0}", ex.ToString());

            ErrorResultModel error = new ErrorResultModel
            {
                Message = string.Format(PTASODataFunctionsErrorMessages.SqlExceptionError, ex.Message)
            };

            ObjectResult result = new ObjectResult(error);
            result.StatusCode = (int)HttpStatusCode.InternalServerError;

            return result;
        }
    }
}
