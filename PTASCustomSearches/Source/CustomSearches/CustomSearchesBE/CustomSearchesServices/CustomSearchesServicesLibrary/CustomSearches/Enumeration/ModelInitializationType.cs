namespace CustomSearchesServicesLibrary.CustomSearches.Enumeration
{
    /// <summary>
    /// Enumerates the different types of model initialization.
    /// </summary>
    public enum ModelInitializationType
    {
        /// <summary>
        /// Initializes the full object without dependencies.
        /// A complete representation of the object which may include direct children owned by the object.
        /// Example (Dataset => DatasetPostProcesses, Charts, Expressions)
        /// </summary>
        FullObject,

        /// <summary>
        /// Initializes the full object with dependencies, which may include a complete tree of children for the object.
        /// Example (Dataset => DatasetPostProcesses ==> Expressions, Charts ==> Expressions)
        /// </summary>
        FullObjectWithDepedendencies,

        /// <summary>
        /// Initializes the object summary. (used to show object in list).
        /// Should be a lightweight/minimal representation of the object, used for sending lists of objects.
        /// </summary>
        Summary
    }
}
