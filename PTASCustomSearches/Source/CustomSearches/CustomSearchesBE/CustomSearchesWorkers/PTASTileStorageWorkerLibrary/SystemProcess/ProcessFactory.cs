namespace PTASTileStorageWorkerLibrary.SystemProcess
{
    using System.Diagnostics;

    /// <summary>
    /// Process factory class.
    /// </summary>
    /// <seealso cref="PTASTileStorageWorkerLibrary.SystemProcess.IProcessFactory" />
    public class ProcessFactory : IProcessFactory
    {
        /// <summary>
        /// Creates the process.
        /// </summary>
        /// <param name="startInfo">The start information.</param>
        /// <returns>
        /// A new process.
        /// </returns>
        public IProcess CreateProcess(ProcessStartInfo startInfo)
        {
            return new LocalProcess(startInfo);
        }
    }
}
