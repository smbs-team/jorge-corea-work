using System;
using System.IO;

namespace PTASSyncService.Utilities
{
    /// <summary>
    /// Class to log errors.
    /// </summary>
    public class LogClass
    {

        /// <summary>
        /// Reports an error into the log file
        /// </summary>
        /// <param name="className">Name of the class that generated the error.</param>
        /// <param name="errorFriendly">Error message</param>
        /// <param name="ex">Thrown exception</param>
        /// <param name="configuration">App configuration settings</param>
        public static void ReportError(string className, string errorFriendly, Exception ex, Settings configuration)
        {
            WriteToTestText("Class: " + className, configuration);
            WriteToTestText("Error message: " + errorFriendly, configuration);
            WriteToTestText("Error - " + ex.Message + " " + ex.StackTrace, configuration);
            
            Exception inner = ex.InnerException;
            string padding = "   ";
            while (inner != null)
            {
                LogClass.WriteToTestText(padding + "inner - " + inner.Message + " " + inner.StackTrace, configuration);
                padding += "   ";
                inner = inner.InnerException;
            }
        }

        /// <summary>
        /// Writes an error message into the log file.
        /// </summary>
        /// <param name="message">Erro message.</param>
        /// <param name="configuration">App configuration settings</param>
        public static void WriteToTestText(string message, Settings configuration)
        {
            try
            {
                if (configuration.logLevel >= 1)
                {
                    using (var f = File.Open(configuration.logFile, FileMode.OpenOrCreate))
                    {
                        f.Seek(0, SeekOrigin.End);
                        StreamWriter writer = new StreamWriter(f);
                        writer.Write(message + "\r\n");
                        writer.Flush();
                    }
                }
            }
            catch (Exception)
            {

            }

        }
    }
}