namespace PTASMediaHelperClasses
{
  using System.Diagnostics;

  /// <summary>
  /// Logs to the local event log.
  /// </summary>
  public class EventLogLogger : ILogger
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="EventLogLogger"/> class.
    /// </summary>
    /// <param name="optionalLogger">Could have an internal logger for optional information.</param>
    public EventLogLogger(ILogger optionalLogger = null) => this.OptionalLogger = optionalLogger ?? new ConsoleLogger();

    /// <summary>
    /// Gets local logger for optional output, default is console.
    /// </summary>
    public ILogger OptionalLogger { get; }

    /// <inheritdoc/>
    public void OptionalOutput(string output) => this.OptionalLogger?.OptionalOutput(output);

    /// <inheritdoc/>
    public void WriteError(string error)
    {
      PTASEventManagerLibrary.ErrorLogger.LogEvent(error, EventLogEntryType.Error);
    }

    /// <inheritdoc/>
    public void WriteInfo(string info)
    {
      PTASEventManagerLibrary.ErrorLogger.LogEvent(info, EventLogEntryType.Information);
    }

    /// <inheritdoc/>
    public void WriteWarning(string warning)
    {
      PTASEventManagerLibrary.ErrorLogger.LogEvent(warning, EventLogEntryType.Warning);
    }
  }
}
