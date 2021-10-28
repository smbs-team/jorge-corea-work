namespace PTASFunctionMediaInfo
{
  using System.Collections.Generic;

  /// <summary>
  /// Static table info provider.
  /// These values would change only if a table was added or
  /// removed from the database.
  /// </summary>
  public static class TableInfoProvider
  {
    /// <summary>
    /// Dictionary of tables we want to use.
    /// </summary>
    private static readonly Dictionary<string, TableInfo> TableMapping = new Dictionary<string, TableInfo>()
    {
      {
            "accy",
            new TableInfo
        {
          TableName = "mr_AccyMedia",
          IndexColumnName = "AccyGuid",
          MediaColumnName = "AccyMedGuid",
        }
      },
      {
            "commbldg",
            new TableInfo
        {
          TableName = "mr_CommBldgMedia",
          IndexColumnName = "BldgMediaGuid",
          MediaColumnName = "BldgMedGuid",
        }
      },
      {
            "float",
            new TableInfo
        {
          TableName = "mr_FloatMedia",
          IndexColumnName = "FhcGuid",
          MediaColumnName = "FloatMedGuid",
        }
      },
      {
            "hinote",
            new TableInfo
        {
          TableName = "mr_HINoteMedia",
          IndexColumnName = "HinGuid",
          MediaColumnName = "HinMedGuid",
        }
      },
      {
            "land",
            new TableInfo
        {
          TableName = "mr_LandMedia",
          IndexColumnName = "LndGuid",
          MediaColumnName = "LndMedGuid",
        }
      },
      {
            "mhacct",
            new TableInfo
        {
          TableName = "mr_MhAcctMedia",
          IndexColumnName = "MhGuid",
          MediaColumnName = "LhMedGuid",
        }
      },
      {
            "resbldg",
            new TableInfo
        {
          TableName = "mr_ResBldgMedia",
          IndexColumnName = "BldgGuid",
          MediaColumnName = "BldgMedGuid",
        }
      },
      {
            "reviewnote",
            new TableInfo
        {
          TableName = "mr_ReviewNoteMedia",
          IndexColumnName = "RnGuid",
          MediaColumnName = "RnMedGuid",
        }
      },
      {
            "rpnote",
            new TableInfo
        {
          TableName = "mr_RpNoteMedia",
          IndexColumnName = "RpnGuid",
          MediaColumnName = "RpnMedGuid",
        }
      },
      {
            "salenote",
            new TableInfo
        {
          TableName = "mr_SaleNoteMedia",
          IndexColumnName = "SnGuid",
          MediaColumnName = "SnMedGuid",
        }
      },
    };

    /// <summary>
    /// tries to find the info on a table by it's name.
    /// </summary>
    /// <param name="tableName">Name of the table we want to fetch.</param>
    /// <returns>The found table info or null.</returns>
    public static TableInfo GetTableInfo(string tableName)
    {
      return TableMapping.TryGetValue(tableName.ToLower(), out TableInfo info) ? info : null;
    }
  }
}
