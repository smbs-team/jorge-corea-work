namespace PTASTileStorageWorkerLibrary.SystemProcess
{
    using System;
    using System.Diagnostics;

    /// <summary>
    /// Interface representing a process.
    /// </summary>
    public interface IProcess : IDisposable
    {
        /// <summary>
        /// Gets the exit code for the process.
        /// </summary>
        public int ExitCode { get; }

        /// <summary>
        /// Gets the underlying process.
        /// </summary>
        public Process Process { get; }

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
