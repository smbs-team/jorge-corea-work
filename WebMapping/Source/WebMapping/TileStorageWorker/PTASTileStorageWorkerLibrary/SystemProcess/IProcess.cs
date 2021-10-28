namespace PTASTileStorageWorkerLibrary.SystemProcess
{
    /// <summary>
    /// Interface representing a process.
    /// </summary>
    public interface IProcess
    {
        /// <summary>
        /// Starts this instance.
        /// </summary>
        void Start();

        /// <summary>
        /// Waits for exit.
        /// </summary>
        void WaitForExit();
    }
}
