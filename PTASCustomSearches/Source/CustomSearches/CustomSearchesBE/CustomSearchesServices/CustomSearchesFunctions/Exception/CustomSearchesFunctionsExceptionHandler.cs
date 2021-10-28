namespace CustomSearchesFunctions.Exception
{
    using System;
    using System.Net;
    using System.Threading.Tasks;
    using CustomSearchesServicesLibrary.CustomSearches.Model.CustomSearches;
    using CustomSearchesServicesLibrary.Exception;
    using CustomSearchesServicesLibrary.Exception.Model;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Handles and maps exceptions that come from the different providers to the exceptions that the server should output.  Also logs the internal exception.
    /// </summary>
    public static class CustomSearchesFunctionsExceptionHandler
    {
        /// <summary>
        /// Maps an untyped exception.
        /// </summary>
        /// <param name="lambda">The lambda to execute.</param>
        /// <param name="req">The request.</param>
        /// <param name="log">The log.</param>
        /// <returns>The output response for the input exception.</returns>
        /// <exception cref="System.ArgumentNullException">If any of the parameters is NULL.</exception>
        public static async Task<IActionResult> GlobalExceptionHandler(Func<Task<IActionResult>> lambda, HttpRequest req, ILogger log)
        {
            try
            {
                return await lambda();
            }
            catch (CustomSearchesRequestBodyException requestBodyException)
            {
                return CustomSearchesFunctionsExceptionHandler.HandleCustomSearchesRequestBodyException(
                    requestBodyException,
                    req,
                    log);
            }
            catch (CustomSearchesDatabaseException databaseException)
            {
                return CustomSearchesFunctionsExceptionHandler.HandleCustomSearchesDatabaseException(
                    databaseException,
                    req,
                    log);
            }
            catch (CustomSearchesJsonException jsonException)
            {
                return CustomSearchesFunctionsExceptionHandler.HandleCustomSearchesJsonException(
                    jsonException,
                    req,
                    log);
            }
            catch (CustomSearchesEntityNotFoundException entityNotFoundException)
            {
                return CustomSearchesFunctionsExceptionHandler.HandleCustomSearchesEntityNotFoundException(
                    entityNotFoundException,
                    req,
                    log);
            }
            catch (CustomSearchesConflictException conflictException)
            {
                return CustomSearchesFunctionsExceptionHandler.HandleCustomSearchesConflictException(
                    conflictException,
                    req,
                    log);
            }
            catch (ArgumentNullException argumentNullException)
            {
                return CustomSearchesFunctionsExceptionHandler.HandleArgumentNullException(
                    argumentNullException,
                    req,
                    log);
            }
            catch (ArgumentOutOfRangeException argumentOutOfRangeException)
            {
                return CustomSearchesFunctionsExceptionHandler.HandleArgumentOutOfRangeException(
                    argumentOutOfRangeException,
                    req,
                    log);
            }
            catch (NotSupportedException notSupportedException)
            {
                return CustomSearchesFunctionsExceptionHandler.HandleNotSupportedException(
                    notSupportedException,
                    req,
                    log);
            }
            catch (InvalidExpressionResultException invalidExpressionResultException)
            {
                return CustomSearchesFunctionsExceptionHandler.HandleInvalidExpressionResultException(invalidExpressionResultException, req, log);
            }
            catch (CustomExpressionValidationException validationException)
            {
                return CustomSearchesFunctionsExceptionHandler.HandleExpressionValidationException(validationException, req, log);
            }
            catch (CustomSearchValidationException validationException)
            {
                return CustomSearchesFunctionsExceptionHandler.HandleValidationException(validationException, req, log);
            }
            catch (FolderManagerException folderManagerException)
            {
                return CustomSearchesFunctionsExceptionHandler.HandleFolderManagerException(
                    folderManagerException,
                    req,
                    log);
            }
            catch (InvalidTokenException invalidTokenException)
            {
                return CustomSearchesFunctionsExceptionHandler.HandleInvalidTokenException(
                    invalidTokenException,
                    req,
                    log);
            }
            catch (AuthorizationException authorizationException)
            {
                return CustomSearchesFunctionsExceptionHandler.HandleAuthorizationException(
                    authorizationException,
                    req,
                    log);
            }
            catch (System.Exception ex)
            {
                return CustomSearchesFunctionsExceptionHandler.HandleUntypedException(ex, req, log);
            }
        }

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
            CustomSearchesFunctionsExceptionHandler.HandleCustomSearchException(ex, req, log);

            ErrorResultModel error = new ErrorResultModel { Message = CustomSearchesFunctionsErrorMessages.UnhandledExceptionError };
            ObjectResult result = new ObjectResult(error);
            result.StatusCode = (int)HttpStatusCode.InternalServerError;

            return result;
        }

        /// <summary>
        /// Maps an argument null exception.
        /// </summary>
        /// <param name="ex">The exception.</param>
        /// <param name="req">The request.</param>
        /// <param name="log">The log.</param>
        /// <returns>The output response for the input exception.</returns>
        public static IActionResult HandleArgumentNullException(ArgumentNullException ex, HttpRequest req, ILogger log)
        {
            CustomSearchesFunctionsExceptionHandler.HandleCustomSearchException(ex, req, log);

            ErrorResultModel error = new ErrorResultModel { Message = string.Format(CustomSearchesFunctionsErrorMessages.ArgumentNullExceptionError, ex.ParamName) };
            ObjectResult result = new ObjectResult(error);
            result.StatusCode = (int)HttpStatusCode.BadRequest;

            return result;
        }

        /// <summary>
        /// Maps an argument null exception.
        /// </summary>
        /// <param name="ex">The exception.</param>
        /// <param name="req">The request.</param>
        /// <param name="log">The log.</param>
        /// <returns>The output response for the input exception.</returns>
        public static IActionResult HandleArgumentOutOfRangeException(ArgumentOutOfRangeException ex, HttpRequest req, ILogger log)
        {
            CustomSearchesFunctionsExceptionHandler.HandleCustomSearchException(ex, req, log);

            ErrorResultModel error = new ErrorResultModel
            {
                Message = string.Format(CustomSearchesFunctionsErrorMessages.ArgumentOutOfRangeExceptionError, ex.ParamName),
                Details = ex.GetBaseException().Message
            };

            ObjectResult result = new ObjectResult(error);
            result.StatusCode = (int)HttpStatusCode.BadRequest;

            return result;
        }

        /// <summary>
        /// Maps a not supported exception.
        /// </summary>
        /// <param name="ex">The exception.</param>
        /// <param name="req">The request.</param>
        /// <param name="log">The log.</param>
        /// <returns>The output response for the input exception.</returns>
        public static IActionResult HandleNotSupportedException(NotSupportedException ex, HttpRequest req, ILogger log)
        {
            CustomSearchesFunctionsExceptionHandler.HandleCustomSearchException(ex, req, log);

            ErrorResultModel error = new ErrorResultModel { Message = ex.GetBaseException().Message };
            ObjectResult result = new ObjectResult(error);
            result.StatusCode = (int)HttpStatusCode.MethodNotAllowed;

            return result;
        }

        /// <summary>
        /// Maps an exception from the request body deserialization.
        /// </summary>
        /// <param name="ex">The exception.</param>
        /// <param name="req">The request.</param>
        /// <param name="log">The log.</param>
        /// <returns>The output response for the input exception.</returns>
        public static IActionResult HandleCustomSearchesRequestBodyException(CustomSearchesRequestBodyException ex, HttpRequest req, ILogger log)
        {
            CustomSearchesFunctionsExceptionHandler.HandleCustomSearchException(ex, req, log);

            ErrorResultModel error = new ErrorResultModel
            {
                Message = ex.Message,
                Details = ex.GetBaseException().Message
            };

            ObjectResult result = new ObjectResult(error);
            result.StatusCode = (int)HttpStatusCode.BadRequest;

            return result;
        }

        /// <summary>
        /// Maps an exception from the database.
        /// </summary>
        /// <param name="ex">The exception.</param>
        /// <param name="req">The request.</param>
        /// <param name="log">The log.</param>
        /// <returns>The output response for the input exception.</returns>
        public static IActionResult HandleCustomSearchesDatabaseException(CustomSearchesDatabaseException ex, HttpRequest req, ILogger log)
        {
            CustomSearchesFunctionsExceptionHandler.HandleCustomSearchException(ex, req, log);

            ErrorResultModel error = new ErrorResultModel
            {
                Message = ex.Message,
                Details = ex.GetBaseException().Message
            };

            ObjectResult result = new ObjectResult(error);
            result.StatusCode = (int)HttpStatusCode.BadRequest;

            return result;
        }

        /// <summary>
        /// Maps an exception from the json.
        /// </summary>
        /// <param name="ex">The exception.</param>
        /// <param name="req">The request.</param>
        /// <param name="log">The log.</param>
        /// <returns>The output response for the input exception.</returns>
        public static IActionResult HandleCustomSearchesJsonException(CustomSearchesJsonException ex, HttpRequest req, ILogger log)
        {
            CustomSearchesFunctionsExceptionHandler.HandleCustomSearchException(ex, req, log);

            ErrorResultModel error = new ErrorResultModel
            {
                Message = ex.Message,
                Details = ex.GetBaseException().Message
            };

            ObjectResult result = new ObjectResult(error);
            result.StatusCode = (int)HttpStatusCode.BadRequest;

            return result;
        }

        /// <summary>
        /// Maps an entity not found exception.
        /// </summary>
        /// <param name="ex">The exception.</param>
        /// <param name="req">The request.</param>
        /// <param name="log">The log.</param>
        /// <returns>The output response for the input exception.</returns>
        public static IActionResult HandleCustomSearchesEntityNotFoundException(
            CustomSearchesEntityNotFoundException ex,
            HttpRequest req,
            ILogger log)
        {
            CustomSearchesFunctionsExceptionHandler.HandleCustomSearchException(ex, req, log);
            ErrorResultModel error = new ErrorResultModel { Message = ex.GetBaseException().Message };
            ObjectResult result = new ObjectResult(error);
            result.StatusCode = (int)HttpStatusCode.NotFound;

            return result;
        }

        /// <summary>
        /// Maps an conflict exception.
        /// </summary>
        /// <param name="ex">The exception.</param>
        /// <param name="req">The request.</param>
        /// <param name="log">The log.</param>
        /// <returns>The output response for the input exception.</returns>
        public static IActionResult HandleCustomSearchesConflictException(
            CustomSearchesConflictException ex,
            HttpRequest req,
            ILogger log)
        {
            CustomSearchesFunctionsExceptionHandler.HandleCustomSearchException(ex, req, log);

            var response = new ConflictExceptionResponse
            {
                Message = ex.GetBaseException().Message,
                PostProcessState = ex.PostProcessState,
                DatasetState = ex.DatasetState,
                JobId = ex.JobId,
                DbLockType = ex.DbLockType
            };

            ObjectResult result = new ObjectResult(response);
            result.StatusCode = (int)HttpStatusCode.Conflict;

            return result;
        }

        /// <summary>
        /// Maps an invalid expression result exception.
        /// </summary>
        /// <param name="ex">The exception.</param>
        /// <param name="req">The request.</param>
        /// <param name="log">The log.</param>
        /// <returns>The output response for the input exception.</returns>
        public static IActionResult HandleInvalidExpressionResultException(
            InvalidExpressionResultException ex,
            HttpRequest req,
            ILogger log)
        {
            CustomSearchesFunctionsExceptionHandler.HandleCustomSearchException(ex, req, log);
            ErrorResultModel error = new ErrorResultModel { Message = ex.GetBaseException().Message };
            ObjectResult result = new ObjectResult(error);
            result.StatusCode = (int)HttpStatusCode.InternalServerError;

            return result;
        }

        /// <summary>
        /// Maps an conflict exception.
        /// </summary>
        /// <param name="ex">The exception.</param>
        /// <param name="req">The request.</param>
        /// <param name="log">The log.</param>
        /// <returns>The output response for the input exception.</returns>
        public static IActionResult HandleExpressionValidationException(
            CustomExpressionValidationException ex,
            HttpRequest req,
            ILogger log)
        {
            CustomSearchesFunctionsExceptionHandler.HandleCustomSearchException(ex, req, log);
            var response = new ExpressionValidationFailedResponse
            {
                Message = ex.GetBaseException().Message,
                ValidationErrors = ex.ValidationErrors
            };

            ObjectResult result = new ObjectResult(response);

            switch (ex.CustomExpressionValidationExceptionType)
            {
                case CustomSearchesServicesLibrary.Enumeration.CustomExpressionValidationExceptionType.Execution:
                    result.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;
                case CustomSearchesServicesLibrary.Enumeration.CustomExpressionValidationExceptionType.Reference:
                    result.StatusCode = (int)HttpStatusCode.Conflict;
                    break;
            }

            return result;
        }

        /// <summary>
        /// Maps a validation exception.
        /// </summary>
        /// <param name="ex">The exception.</param>
        /// <param name="req">The request.</param>
        /// <param name="log">The log.</param>
        /// <returns>The output response for the input exception.</returns>
        public static IActionResult HandleValidationException(
            CustomSearchValidationException ex,
            HttpRequest req,
            ILogger log)
        {
            CustomSearchesFunctionsExceptionHandler.HandleCustomSearchException(ex, req, log);
            ErrorResultModel error = new ErrorResultModel
            {
                Message = ex.Message,
                Details = ex.GetBaseException().Message
            };

            ObjectResult result = new ObjectResult(error);
            result.StatusCode = (int)HttpStatusCode.BadRequest;

            return result;
        }

        /// <summary>
        /// Maps an folder manager exception.
        /// </summary>
        /// <param name="ex">The exception.</param>
        /// <param name="req">The request.</param>
        /// <param name="log">The log.</param>
        /// <returns>The output response for the input exception.</returns>
        public static IActionResult HandleFolderManagerException(
            FolderManagerException ex,
            HttpRequest req,
            ILogger log)
        {
            CustomSearchesFunctionsExceptionHandler.HandleCustomSearchException(ex, req, log);
            ErrorResultModel error = new ErrorResultModel { Message = ex.GetBaseException().Message };
            ObjectResult result = new ObjectResult(error);

            switch (ex.FolderManagerExceptionType)
            {
                case CustomSearchesServicesLibrary.Enumeration.FolderManagerExceptionType.InvalidFolderFormat:
                    result.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;
                case CustomSearchesServicesLibrary.Enumeration.FolderManagerExceptionType.FolderNotFound:
                    result.StatusCode = (int)HttpStatusCode.NotFound;
                    break;
                case CustomSearchesServicesLibrary.Enumeration.FolderManagerExceptionType.Forbidden:
                    result.StatusCode = (int)HttpStatusCode.Forbidden;
                    break;
                default:
                    break;
            }

            return result;
        }

        /// <summary>
        /// Maps an invalid token exception.
        /// </summary>
        /// <param name="ex">The exception.</param>
        /// <param name="req">The request.</param>
        /// <param name="log">The log.</param>
        /// <returns>The output response for the input exception.</returns>
        public static IActionResult HandleInvalidTokenException(
            InvalidTokenException ex,
            HttpRequest req,
            ILogger log)
        {
            CustomSearchesFunctionsExceptionHandler.HandleCustomSearchException(ex, req, log);
            ErrorResultModel error = new ErrorResultModel { Message = CustomSearchesFunctionsErrorMessages.InvalidTokenExceptionError };
            ObjectResult result = new ObjectResult(error);
            result.StatusCode = (int)HttpStatusCode.BadRequest;

            return result;
        }

        /// <summary>
        /// Maps an authorization exception.
        /// </summary>
        /// <param name="ex">The exception.</param>
        /// <param name="req">The request.</param>
        /// <param name="log">The log.</param>
        /// <returns>The output response for the input exception.</returns>
        public static IActionResult HandleAuthorizationException(
            AuthorizationException ex,
            HttpRequest req,
            ILogger log)
        {
            CustomSearchesFunctionsExceptionHandler.HandleCustomSearchException(ex, req, log);
            ErrorResultModel error = new ErrorResultModel { Message = ex.GetBaseException().Message };
            ObjectResult result = new ObjectResult(error);
            result.StatusCode = (int)HttpStatusCode.Forbidden;

            return result;
        }

        /// <summary>
        /// Handles the custom search exception.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <param name="req">The req.</param>
        /// <param name="log">The log.</param>
        private static void HandleCustomSearchException(
            Exception ex,
            HttpRequest req,
            ILogger log)
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

            log.LogError(ex, $"Function level exception: {ex.ToString()} ");
        }
    }
}
