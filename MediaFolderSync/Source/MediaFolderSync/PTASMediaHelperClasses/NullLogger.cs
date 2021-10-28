namespace PTASMediaHelperClasses
{
  /// <summary>
  /// Do nothing logger.
  /// </summary>
  public class NullLogger : ILogger
  {
    /// <inheritdoc/>
    public void OptionalOutput(string output)
    {
    }

    /// <inheritdoc/>
    public void WriteError(string error)
    {
    }

    /// <inheritdoc/>
    public void WriteInfo(string info)
    {
    }

    /// <inheritdoc/>
    public void WriteWarning(string warning)
    {
    }
  }
}
