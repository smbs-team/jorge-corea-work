namespace PTASMediaHelperClasses
{
  using System.IO;
  using System.Xml.Linq;

  /// <summary>
  /// Saves and retrieves the progress of an operation from
  /// xml file.
  /// </summary>
  public class ProgressManager
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="ProgressManager"/> class.
    /// </summary>
    /// <param name="outputPath">Where to save the progress.</param>
    /// <param name="logger">Logger for output & messages.</param>
    public ProgressManager(string outputPath, ILogger logger)
    {
      if (string.IsNullOrEmpty(outputPath))
      {
        throw new System.ArgumentException("OutputPath was not provided", nameof(outputPath));
      }

      this.OutputPath = outputPath;
      this.Logger = logger;
    }

    /// <summary>
    /// Gets path for output.
    /// </summary>
    private string OutputPath { get; }

    /// <summary>
    /// Gets the current logger.
    /// </summary>
    private ILogger Logger { get; }

    /// <summary>
    /// Attempts to pull the last id processed from an xml file.
    /// </summary>
    /// <param name="tableName">Name of the table.</param>
    /// <returns>0 or the last record processed.</returns>
    public int GetLastProcessedRecord(string tableName)
    {
      var fileName = this.GetFileName(tableName);
      if (this.FileExists(fileName))
      {
        try
        {
          XDocument doc = this.LoadDocument(fileName);
          int lastRecord = int.Parse(doc.Element("root").Element("lastid").Value);
          this.Logger.OptionalOutput($"Got value {lastRecord} from file {fileName}");
          return lastRecord;
        }
        catch (System.Exception)
        {
          return 0;
        }
      }

      return 0;
    }

    /// <summary>
    /// Saves the id of the last processed table for a give table.
    /// </summary>
    /// <param name="tableName">Name of the table.</param>
    /// <param name="lastRecord">Id of record to save.</param>
    public void SaveLastRecordProcessed(string tableName, int lastRecord)
    {
      var fileName = this.GetFileName(tableName);
      XDocument srcTree = new XDocument(
        new XComment($"Table: {tableName} last id checked."),
        new XElement("root", new XElement("lastid", lastRecord.ToString())));
      this.Logger.OptionalOutput($"Saved {lastRecord} to {fileName}");
      this.SaveDocument(fileName, srcTree);
    }

    /// <summary>
    /// For mocking, saves the document.
    /// </summary>
    /// <param name="fileName">Filename to save.</param>
    /// <param name="srcDoc">XML Document to save.</param>
    protected virtual void SaveDocument(string fileName, XDocument srcDoc)
    {
      srcDoc.Save(fileName);
    }

    /// <summary>
    /// Checks for the existance of a file.
    /// </summary>
    /// <param name="fileName">File to check for.</param>
    /// <returns>True if the file exists, false otherwise.</returns>
    protected virtual bool FileExists(string fileName)
    {
      return File.Exists(fileName);
    }

    /// <summary>
    /// Loads a document from an XML file.
    /// </summary>
    /// <param name="fileName">File to load from.</param>
    /// <returns>Created document.</returns>
    protected virtual XDocument LoadDocument(string fileName)
    {
      return XDocument.Load(fileName);
    }

    private string GetFileName(string tableName) => $@"{this.OutputPath}/progress_{tableName}.xml";
  }
}
