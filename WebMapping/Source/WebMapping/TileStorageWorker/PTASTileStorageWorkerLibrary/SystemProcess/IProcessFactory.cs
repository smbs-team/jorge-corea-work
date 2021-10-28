namespace PTASTileStorageWorkerLibrary.SystemProcess
{
    using System.Diagnostics;

    /// <summary>
    /// Class that creates processes.
    /// </summary>
    public interface IProcessFactory
    {
        /// <summary>
        /// Creates the process.
        /// </summary>
        /// <param name="startInfo">The start information.</param>
        /// <returns>A new process.</returns>
        IProcess CreateProcess(ProcessStartInfo startInfo);
    }
}
