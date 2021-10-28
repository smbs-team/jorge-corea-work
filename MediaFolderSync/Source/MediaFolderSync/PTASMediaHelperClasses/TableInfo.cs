namespace PTASMediaHelperClasses
{
  /// <summary>
  /// Table information.
  /// </summary>
  internal class TableInfo
  {
    /// <summary>
    /// Gets or sets name of the table.
    /// </summary>
    internal string TableName { get; set; }

    /// <summary>
    /// Gets or sets which column we use as an index.
    /// </summary>
    internal string IndexColumnName { get; set; }

    /// <summary>
    /// Gets or sets column where we get the media columna name.
    /// </summary>
    internal string MediaColumnName { get; set; }
  }
}
