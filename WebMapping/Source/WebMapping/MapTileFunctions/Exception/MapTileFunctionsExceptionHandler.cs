namespace PTASMapTileFunctions.Exception
{
    using System.Net;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Data.SqlClient;
    using Microsoft.Extensions.Logging;
    using PTASMapTileServicesLibrary.OverlapCalutionProvider.Exception;
    using PTASMapTileServicesLibrary.TileProvider.Exception;

    /// <summary>
    /// Handles and maps exceptions that come from the different providers to the exceptions that the server should output.  Also logs the internal exception.
    /// </summary>
    public static class MapTileFunctionsExceptionHandler
    {
        /// <summary>
        /// Maps an untyped exception.
        /// </summary>
        /// <param name="ex">The exception.</param>
        /// <param name="req">The request.</param>
        /// <param name="log">The log.</param>
        /// <returns>The output response for the input exception.</returns>
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

            log.LogError(ex, "Function level exception");

            ErrorResultModel error = new ErrorResultModel { Message = MapTileFunctionsErrorMessages.UnhandledExceptionError };
            ObjectResult result = new ObjectResult(error);
            result.StatusCode = (int)HttpStatusCode.InternalServerError;

            return result;
        }

        /// <summary>
        /// Maps an exception from the tile feature data provider.
        /// </summary>
        /// <param name="ex">The exception.</param>
        /// <param name="req">The request.</param>
        /// <param name="log">The log.</param>
        /// <returns>The output response for the input exception.</returns>
        public static IActionResult HandleTileProviderException(TileProviderException ex, HttpRequest req, ILogger log)
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

            log.LogError(ex, "Function level exception");

            ErrorResultModel error = new ErrorResultModel { Message = MapTileFunctionsErrorMessages.UnhandledExceptionError };
            ObjectResult result = new ObjectResult(error);
            result.StatusCode = (int)HttpStatusCode.InternalServerError;

            return result;
        }

        /// <summary>
        /// Maps an exception from the tile feature data provider.
        /// </summary>
        /// <param name="ex">The exception.</param>
        /// <param name="req">The request.</param>
        /// <param name="log">The log.</param>
        /// <returns>The output response for the input exception.</returns>
        public static IActionResult HandleTileFeatureDataProviderException(TileFeatureDataProviderException ex, HttpRequest req, ILogger log)
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

            log.LogError(ex, "Function level exception");

            if (ex.TileFeatureDataProviderExceptionCategory == TileFeatureDataProviderExceptionCategory.DatasetNotFound)
            {
                ErrorResultModel error = new ErrorResultModel { Message = MapTileFunctionsErrorMessages.DatasetNotFoundExceptionError };
                ObjectResult result = new ObjectResult(error);
                result.StatusCode = (int)HttpStatusCode.NotFound;
                return result;
            }
            else
            {
                if (ex.InnerException?.GetType() == typeof(SqlException))
                {
                    ErrorResultModel sqlError = new ErrorResultModel
                    {
                        Message = MapTileFunctionsErrorMessages.TileFeatureDataSqlException + $" Error: {ex.InnerException.Message}"
                    };

                    ObjectResult sqlResult = new ObjectResult(sqlError);
                    sqlResult.StatusCode = (int)HttpStatusCode.InternalServerError;
                    return sqlResult;
                }

                ErrorResultModel error = new ErrorResultModel { Message = MapTileFunctionsErrorMessages.UnhandledExceptionError };
                ObjectResult result = new ObjectResult(error);
                result.StatusCode = (int)HttpStatusCode.InternalServerError;
                return result;
            }
        }

        /// <summary>
        /// Maps an exception from the GeoLocatio provider.
        /// </summary>
        /// <param name="ex">The exception.</param>
        /// <param name="req">The request.</param>
        /// <param name="log">The log.</param>
        /// <returns>The output response for the input exception.</returns>
        public static IActionResult HandleGeoLocationProviderException(GeoLocationProviderException ex, HttpRequest req, ILogger log)
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

            log.LogError(ex, "Function level exception");

            ErrorResultModel error = new ErrorResultModel { Message = MapTileFunctionsErrorMessages.UnhandledExceptionError };
            ObjectResult result = new ObjectResult(error);
            result.StatusCode = (int)HttpStatusCode.InternalServerError;

            return result;
        }

        /// <summary>
        /// Maps an exception from the OverlapCalculation provider.
        /// </summary>
        /// <param name="ex">The exception.</param>
        /// <param name="req">The request.</param>
        /// <param name="log">The log.</param>
        /// <returns>The output response for the input exception.</returns>
        public static IActionResult HandleOverlapCalculationProviderException(OverlapCalculationProviderException ex, HttpRequest req, ILogger log)
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

            log.LogError(ex, "Function level exception");

            ErrorResultModel error = new ErrorResultModel { Message = MapTileFunctionsErrorMessages.UnhandledExceptionError };
            ObjectResult result = new ObjectResult(error);
            result.StatusCode = (int)HttpStatusCode.InternalServerError;

            return result;
        }
    }
}
