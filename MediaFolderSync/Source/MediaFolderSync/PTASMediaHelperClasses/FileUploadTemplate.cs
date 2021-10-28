namespace PTASMediaHelperClasses
{
  using System;

  /// <summary>
  /// Template for file uploading. May use wildcards.
  /// </summary>
  public class FileUploadTemplate
  {
    /// <summary>
    /// Gets or sets path to the files.
    /// </summary>
    public string Path { get; set; }

    /// <summary>
    /// Gets or sets filter we want to use. DOS wildcards: *.jpg. FileName*.*, etc.
    /// </summary>
    public string Filter { get; set; }

    /// <summary>
    /// Gets or sets path to save the files to. Can be an OS path or an azure share.
    /// </summary>
    public string OutputPath { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether should we check subdirectories too.
    /// </summary>
    public bool Recursive { get; set; }

    /// <summary>
    /// Convert to string for display.
    /// </summary>
    /// <returns>Formatted string.</returns>
    public override string ToString() => $"{this.Path}-{this.Filter} => {this.OutputPath}";

    /// <summary>
    /// Convert to an azure path.
    /// </summary>
    /// <param name="fullPath">Complete output path.</param>
    /// <returns>The azure path to save to.</returns>
    public string GetAzureTargetPath(string fullPath) => (this.OutputPath.ToUpper() + fullPath.Substring(this.Path.Length - 1)).Replace(@"\", @"/").Replace("//", "/");
  }
}
