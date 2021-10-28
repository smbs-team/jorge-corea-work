namespace PTASServicesCommon.DependencyInjection
{
    /// <summary>
    /// Interface that declares a contract on how to create objects.
    /// </summary>
    /// <typeparam name="T">Created object.</typeparam>
    public interface IFactory<T>
    {
        /// <summary>
        /// Creates the object instance.
        /// </summary>
        /// <returns>A newly created instance of T.</returns>
        T Create();
    }
}
