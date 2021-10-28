namespace PTASMediaHelperClasses
{
  using System;
  using System.IO;
  using System.Xml.Linq;

  /// <summary>
  /// Returns last run time from xml file
  /// also update the date to now.
  /// </summary>
  public class FileDateManager : IAssetDateManager
  {
    private DateTime startDate;

    /// <summary>
    /// Initializes a new instance of the <see cref="FileDateManager"/> class.
    /// </summary>
    /// <param name="docFile">Document file to get the date from.</param>
    public FileDateManager(string docFile)
    {
      if (string.IsNullOrEmpty(docFile))
      {
        throw new ArgumentException("Date file getter needs an input file to function.", nameof(docFile));
      }

      this.DocFile = docFile;
    }

    /// <summary>
    /// Gets document file to upload.
    /// </summary>
    private string DocFile { get; }

    /// <inheritdoc/>
    public DateTime GetDate()
    {
      this.startDate = DateTime.Now;
      return File.Exists(this.DocFile) ? this.GetLastDate() : this.GetNewDate();
    }

    /// <inheritdoc/>
    public void UpdateDate()
    {
      this.SaveDate(this.startDate);
    }

    /// <summary>
    /// Save or overwrite xml file.
    /// </summary>
    /// <param name="dateToSave">Date that has to be saved.</param>
    private void SaveDate(DateTime dateToSave)
    {
      XDocument srcTree = new XDocument(
        new XComment("Last Saved Date"),
        new XElement("root", new XElement("lastdate", dateToSave.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss"))));
      srcTree.Save(this.DocFile);
    }

    private DateTime GetLastDate()
    {
      // WARNING: Order important in the following lines, first needs to load
      // then save or else will never get the old date.
      XDocument doc = XDocument.Load(this.DocFile);
      return DateTime.Parse(doc.Element("root").Element("lastdate").Value);
    }

    /// <summary>
    /// Creates new date record and returns yesterday to do a checking.
    /// </summary>
    /// <returns>Yesterday date.</returns>
    private DateTime GetNewDate()
    {
      this.SaveDate(DateTime.Now);
      return DateTime.Now.AddDays(-1);
    }
  }
}
