namespace PTASODataFunctions.Exception
{
    /// <summary>
    /// Class containing the different error messages returned by the odata functions.
    /// </summary>
    public static class PTASODataFunctionsErrorMessages
    {
        /// <summary>
        /// The unhandled exception error.
        /// </summary>
        public const string UnhandledExceptionError = "An unknown error has happened on the back-end.";

        /// <summary>
        /// The argument null exception error.
        /// </summary>
        public const string ArgumentNullExceptionError = "A value was not provided for the '{0}' parameter.";

        /// <summary>
        /// The argument for an odata exception.
        /// </summary>
        public const string ODataExceptionError = "OData error '{0}'.";

        /// <summary>
        /// The argument out of range exception error.
        /// </summary>
        public const string ArgumentOutOfRangeExceptionError = "The queried resource '{0}' does not exist.";

        /// <summary>
        /// The json serialization exception error.
        /// </summary>
        public const string JsonSerializationExceptionError = "An error occured during JSON deserialization. Details: {0}";

        /// <summary>
        /// The json reader exception error.
        /// </summary>
        public const string JsonReaderExceptionError = "An error occured while reading JSON text. Details: {0}";

        /// <summary>
        /// The update database exception error.
        /// </summary>
        public const string DbUpdateExceptionError = "An error occured while saving to the database. Error: {0}";

        /// <summary>
        /// The sql exception error.
        /// </summary>
        public const string SqlExceptionError = "An error occurred accessing the database. Error: {0}";
    }
}
