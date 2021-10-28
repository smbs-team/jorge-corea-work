namespace PTASServicesCommon.DependencyInjection
{
    using System;

    /// <summary>
    /// Object factory.
    /// </summary>
    /// <typeparam name="T">Type of the object to be created.</typeparam>
    /// <seealso cref="PTASMapTileFunctions.Startup.IFactory{T}" />
    public class Factory<T> : IFactory<T>
    {
        /// <summary>
        /// The initialize function.
        /// </summary>
        private readonly Func<T> initFunction;

        /// <summary>
        /// Initializes a new instance of the <see cref="Factory{T}"/> class.
        /// </summary>
        /// <param name="initFunction">The initialize function.</param>
        public Factory(Func<T> initFunction)
        {
            this.initFunction = initFunction;
        }

        /// <summary>
        /// Creates the object instance.
        /// </summary>
        /// <returns>
        /// A newly created instance of T.
        /// </returns>
        public T Create()
        {
            return this.initFunction();
        }
    }
}
