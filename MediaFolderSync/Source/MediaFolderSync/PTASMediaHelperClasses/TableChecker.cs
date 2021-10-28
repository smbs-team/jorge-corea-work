namespace PTASMediaHelperClasses
{
  using System;
  using System.Collections.Generic;
  using System.Data;
  using System.Data.SqlClient;

  /// <summary>
  /// Media checker class.
  /// </summary>
  public class TableChecker
  {
    /// <summary>
    /// Maps for the tables.
    /// </summary>
    private static readonly Dictionary<string, TableInfo> TableMapping = new Dictionary<string, TableInfo>()
    {
      {
        "accy",
        new TableInfo
        {
          TableName = "AccyMedia",
          IndexColumnName = "AccyGuid",
          MediaColumnName = "AccyMedGuid",
        }
      },
      {
        "bldgpermit", new TableInfo
        {
          TableName = "BldgPermitMedia",
          IndexColumnName = "PermitGuid",
          MediaColumnName = "BpMedGuid",
        }
      },
      {
            "commbldg",
            new TableInfo
        {
          TableName = "CommBldgMedia",
          IndexColumnName = "BldgGuid",
          MediaColumnName = "BldgMedGuid",
        }
      },
      {
            "float",
            new TableInfo
        {
          TableName = "FloatMedia",
          IndexColumnName = "FhcGuid",
          MediaColumnName = "FloatMedGuid",
        }
      },
      {
            "hinote",
            new TableInfo
        {
          TableName = "HINoteMedia",
          IndexColumnName = "HinGuid",
          MediaColumnName = "HinMedGuid",
        }
      },
      {
            "land",
            new TableInfo
        {
          TableName = "LandMedia",
          IndexColumnName = "LndGuid",
          MediaColumnName = "LndMedGuid",
        }
      },
      {
            "mhacct",
            new TableInfo
        {
          TableName = "MhAcctMedia",
          IndexColumnName = "MhGuid",
          MediaColumnName = "MhMedGuid",
        }
      },
      {
            "resbldg",
            new TableInfo
        {
          TableName = "ResBldgMedia",
          IndexColumnName = "BldgGuid",
          MediaColumnName = "BldgMedGuid",
        }
      },
      {
            "reviewnote",
            new TableInfo
        {
          TableName = "ReviewNoteMedia",
          IndexColumnName = "RnGuid",
          MediaColumnName = "RnMedGuid",
        }
      },
      {
            "rpnote",
            new TableInfo
        {
          TableName = "RpNoteMedia",
          IndexColumnName = "RpnGuid",
          MediaColumnName = "RpnMedGuid",
        }
      },
      {
            "salenote",
            new TableInfo
        {
          TableName = "SaleNoteMedia",
          IndexColumnName = "SnGuid",
          MediaColumnName = "SnMedGuid",
        }
      },
    };

    /// <summary>
    /// Initializes a new instance of the <see cref="TableChecker"/> class.
    /// </summary>
    /// <param name="tableName">Name of the table to check.</param>
    /// <param name="connectionString">Database connection string.</param>
    public TableChecker(string tableName, string connectionString)
    {
      if (!TableMapping.TryGetValue(tableName, out TableInfo ti))
      {
        throw new Exception($"Could not find table {tableName}.");
      }

      this.TableInfo = ti;
      this.ConnectionString = connectionString;
    }

    /// <summary>
    /// Gets database connection string.
    /// </summary>
    private string ConnectionString { get; }

    private TableInfo TableInfo { get; }

    /// <summary>
    /// Checks the records on a table for existance in azure.
    /// </summary>
    /// <param name="startId">Starting id to process.</param>
    /// <param name="lastId">Output last id processed.</param>
    /// <param name="checkFile">Action to execute.</param>
    /// <param name="recordCount">Max number of records to process.</param>
    /// <returns>True if there are more records left.</returns>
    public bool Check(int startId, out int lastId, Action<Guid, string> checkFile, int recordCount = 1000)
    {
      var query = $"select top {recordCount} {this.TableInfo.MediaColumnName}, {this.TableInfo.IndexColumnName}, id, FileExtension from {this.TableInfo.TableName} where id>'{startId}' order by id";
      var count = 0;
      Guid indexGuid;
      int lastProcessed = 0;
      using (IDbConnection conn = this.CreateConnection())
      {
        conn.Open();
        using (IDbCommand cmd = this.CreateCommand(query, conn))
        {
          var r = cmd.ExecuteReader();
          while (r.Read())
          {
            Guid fileGuid = (Guid)r.GetValue(0);
            indexGuid = (Guid)r.GetValue(1);
            lastProcessed = (int)r.GetValue(2);
            string fileExtension = (string)r.GetValue(3);
            checkFile(fileGuid, fileExtension);
            count++;
          }
        }

        conn.Close();
      }

      // returns true if needs to continue
      lastId = lastProcessed;
      return count == recordCount;
    }

    /// <summary>
    /// Creates a SQL command for executing. Helps decoupling from SQL server.
    /// </summary>
    /// <param name="query">Query to execute.</param>
    /// <param name="connection">Connection to run the query against.</param>
    /// <returns>A SQL command ready to execute.</returns>
    protected virtual IDbCommand CreateCommand(string query, IDbConnection connection)
    {
      return new SqlCommand(query, (SqlConnection)connection);
    }

    /// <summary>
    /// Creates a connection to the SQL server. Helps decouple from SQL server.
    /// </summary>
    /// <returns>The new connection.</returns>
    protected virtual IDbConnection CreateConnection()
    {
      return new SqlConnection(this.ConnectionString);
    }
  }
}
