namespace PTASEventManagerLibrary
{
  using System;
  using System.Diagnostics;

  /// <summary>
  /// simple wrapper for writing into the event log.
  /// </summary>
  public static class ErrorLogger
  {
    /// <summary>
    /// Write a message to the event log.
    /// </summary>
    /// <param name="msg">Message to Send.</param>
    /// <param name="eventType">info, error, warning, audit.</param>
    /// <param name="eventId">optional generating event.</param>
    /// <param name="category">optional category for the event.</param>
    public static void LogEvent(string msg, EventLogEntryType eventType = EventLogEntryType.Information, int eventId = 1, short category = 1)
    {
      try
      {
        using (EventLog eventLog = new EventLog("PTAS-LOG") { Source = "PTAS" })
        {
          eventLog.WriteEntry(msg, eventType, eventId, category);
        }
      }
      catch (Exception ex)
      {
        // Last resource, write to text file if it fails to write to the event log
        System.IO.File.AppendAllLines("c:/tmp/ptas-log.txt", new string[] { msg, ex.Message, ex.InnerException?.Message ?? string.Empty });

        // we don't rethrow the exception because we need to asume that the program was responsive by the time it emitted this message.
      }
    }
  }
}