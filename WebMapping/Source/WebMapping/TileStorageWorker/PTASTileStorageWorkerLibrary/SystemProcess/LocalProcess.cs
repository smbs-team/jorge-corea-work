namespace PTASTileStorageWorkerLibrary.SystemProcess
{
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
