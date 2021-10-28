namespace PTASMediaHelperClasses
{
  using System;

  /// <summary>
  /// A logger that outputs to the console.
  /// </summary>
  public class ConsoleLogger : ILogger
  {
    /// <summary>
    /// Not crucial output.
    /// </summary>
    /// <param name="output">Text to write.</param>
    public void OptionalOutput(string output) => Console.WriteLine(output);

    /// <summary>
    /// Error output.
    /// </summary>
    /// <param name="error">Error description.</param>
    public void WriteError(string error) => Console.Error.WriteLine(error);

    /// <summary>
    /// Information message.
    /// </summary>
    /// <param name="info">Information to output.</param>
    public void WriteInfo(string info) => Console.WriteLine(info);

    /// <summary>
    /// System warning.
    /// </summary>
    /// <param name="warning">Warning to output.</param>
    public void WriteWarning(string warning) => Console.WriteLine(warning);
  }
}
