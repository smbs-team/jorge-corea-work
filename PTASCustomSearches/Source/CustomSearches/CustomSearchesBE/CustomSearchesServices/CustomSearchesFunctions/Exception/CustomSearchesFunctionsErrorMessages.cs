namespace CustomSearchesFunctions.Exception
{
    /// <summary>
    /// Class containing the different error messages returned by the odata functions.
    /// </summary>
    public static class CustomSearchesFunctionsErrorMessages
    {
        /// <summary>
        /// The unhandled exception error.
        /// </summary>
        public const string UnhandledExceptionError = "An unknown error has happened in the back-end.";

        /// <summary>
        /// The request body error.
        /// </summary>
        public const string ArgumentNullExceptionError = "The argument '{0}' is required.";

        /// <summary>
        /// Error text for ArgumentOutOfRangeException.
        /// </summary>
        public const string ArgumentOutOfRangeExceptionError = "The argument '{0}' has an invalid value.";

        /// <summary>
        /// The request body error.
        /// </summary>
        public const string RequestBodyError = "Cannot deserialize the body of the request.";

        /// <summary>
        /// The invalid token error.
        /// </summary>
        public const string InvalidTokenExceptionError = "The provided JWT token is invalid.";
    }
}
