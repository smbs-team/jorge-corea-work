namespace PTASTileStorageWorkerLibrary.SystemProcess
{
    using System;
    using System.Diagnostics;

    /// <summary>
    /// Process that will run locally.
    /// </summary>
    /// <seealso cref="PTASTileStorageWorkerLibrary.SystemProcess.IProcess" />
    public class LocalProcess : IProcess
    {
        /// <summary>
        /// The wrapped process.
        /// </summary>
        private Process process;

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalProcess"/> class.
        /// </summary>
        /// <param name="startInfo">The start information.</param>
        public LocalProcess(ProcessStartInfo startInfo)
        {
            this.process = new Process();
            this.process.StartInfo = startInfo;
        }

        /// <summary>
        /// Gets the exit code for the process.
        /// </summary>
        public int ExitCode
        {
            get
            {
               return this.process.ExitCode;
            }
        }

        /// <summary>
        /// Gets the underlying process.
        /// </summary>
        public Process Process
        {
            get
            {
                return this.process;
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (this.Process != null)
            {
                this.Process.Dispose();
            }
        }

        /// <summary>
        /// Starts this instance.
        /// </summary>
        public void Start()
        {
            this.process.Start();
        }

        /// <summary>
        /// Waits for exit.
        /// </summary>
        public void WaitForExit()
        {
            this.process.WaitForExit();
        }
    }
}
