namespace PTASMediaHelperClasses
{
  /// <summary>
  /// Generic Log Writer.
  /// </summary>
  public interface ILogger
  {
    /// <summary>
    /// Information message.
    /// </summary>
    /// <param name="info">Information to output.</param>
    void WriteInfo(string info);

    /// <summary>
    /// Error output.
    /// </summary>
    /// <param name="error">Error description.</param>
    void WriteError(string error);

    /// <summary>
    /// System warning.
    /// </summary>
    /// <param name="warning">Warning to output.</param>
    void WriteWarning(string warning);

    /// <summary>
    /// Not crucial output.
    /// </summary>
    /// <param name="output">Text to write.</param>
    void OptionalOutput(string output);
  }
}
