namespace CustomSearchesServicesLibrary.Misc
{
    /// <summary>
    /// Class that allows to pass reference to values via async methods.  Not enforced by compiler like out or ref parameters.
    /// </summary>
    /// <typeparam name="T">Referenced type.</typeparam>
    public class Ref<T>
    {
        /// <summary>
        /// Gets or sets the referenced value.
        /// </summary>
        public T Value { get; set; }
    }
}
