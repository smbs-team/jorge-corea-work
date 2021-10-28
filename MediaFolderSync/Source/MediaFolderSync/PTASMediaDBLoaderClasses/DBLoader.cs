namespace PTASMediaDBLoaderClasses
{
  using System;
  using System.Collections.Generic;
  using System.Data;
  using System.Data.SqlClient;
  using System.IO;
  using System.Linq;
  using PTASMediaHelperClasses;

  /// <summary>
  /// calculates the files that need to be copied off of the media directory
  /// by looking for updated info in the database.
  /// </summary>
  public class DBLoader : IFilesToProcessProvider
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="DBLoader" /> class.
    /// </summary>
    /// <param name="connectionString">connections to use.</param>
    /// <param name="lastAccess">the last access date.</param>
    /// <param name="rootFolder">root folder for files.</param>
    /// <param name="logger">Logger output manager.</param>
    /// <param name="queryToRun">SQL query that fetches the items to process based on an initial date.</param>
    public DBLoader(string connectionString, DateTime lastAccess, string rootFolder, ILogger logger, string queryToRun)
    {
      if (string.IsNullOrEmpty(connectionString))
      {
        throw new ArgumentException("Connection string was not supplied.", nameof(connectionString));
      }

      if (string.IsNullOrEmpty(rootFolder))
      {
        throw new ArgumentException("Root Folder was not supplied", nameof(rootFolder));
      }

      if (string.IsNullOrEmpty(queryToRun))
      {
        throw new ArgumentException("message", nameof(queryToRun));
      }

      this.ConnectionString = connectionString;
      this.LastAccess = lastAccess;
      this.RootFolder = rootFolder;
      this.Logger = logger ?? throw new ArgumentNullException(nameof(logger));
      this.QueryToRun = queryToRun;
    }

    /// <summary>
    /// Gets Connection to the database.
    /// </summary>
    private string ConnectionString { get; }

    /// <summary>
    /// Gets the Last access date.
    /// </summary>
    private DateTime LastAccess { get; }

    /// <summary>
    /// Gets the Root folder for the files.
    /// </summary>
    private string RootFolder { get; }

    /// <summary>
    /// Gets logger for output.
    /// </summary>
    private ILogger Logger { get; }

    private string QueryToRun { get; }

    /// <summary>
    /// Try to load the pending file list.
    /// </summary>
    /// <returns>The files loaded.</returns>
    public IEnumerable<FileUploadTemplate> GetFiles()
    {
      try
      {
        using (var conn = this.CreateSQLConnection(this.ConnectionString))
        {
          IDbCommand cmd = this.CreateSQLCommand(conn);
          IDataAdapter adapter = this.GetDataAdapter(cmd);

          DataTable dt = new DataTable();
          this.FillDataTable(adapter, dt);
          this.Logger.OptionalOutput("About to run query...");
          var tx = dt.AsEnumerable()
            .Select(x => $"{x.ItemArray[0]}.{x.ItemArray[1]}")
            .Select(cde =>
            {
              return new
              {
                route = $@"{cde[0]}/{cde[1]}/{cde[2]}/{cde[3]}/",
                FileName = cde,
              };
            })
            .Select(s => new FileUploadTemplate
            {
              Filter = Path.GetFileNameWithoutExtension(s.FileName) + "*.*",
              OutputPath = $"{s.route}",
              Path = (this.RootFolder + s.route).Replace(@"/", @"\"),
              Recursive = false,
            }).ToList();
          this.Logger.OptionalOutput($"Found [{tx.Count()}] records");
          return tx;
        }
      }
      catch (Exception ex)
      {
        this.Logger.WriteError(ex.Message);
        throw;
      }
    }

    /// <summary>
    /// Fill the table from the adapter. Allows for de-coupling of Sql Server specific dependencies.
    /// </summary>
    /// <param name="adapter">The data adapter.</param>
    /// <param name="table">The table to use.</param>
    protected virtual void FillDataTable(IDataAdapter adapter, DataTable table)
    {
      var asSqlAdapter = adapter as SqlDataAdapter;
      asSqlAdapter.Fill(table);
    }

    /// <summary>
    /// Get the data adapter to use. Allows for decoupling of SQL server specific dependencies.
    /// </summary>
    /// <param name="command">Command to execute.</param>
    /// <returns>Created adapter.</returns>
    protected virtual IDataAdapter GetDataAdapter(IDbCommand command) => new SqlDataAdapter(command as SqlCommand);

    /// <summary>
    /// Get the SQL command to use. Allows for decoupling of SQL.
    /// </summary>
    /// <param name="connection">Connection to use.</param>
    /// <returns>Connection to the database.</returns>
    protected virtual IDbCommand CreateSQLCommand(IDbConnection connection)
    {
      SqlCommand sqlCommand = new SqlCommand(this.QueryToRun, connection as SqlConnection);
      sqlCommand.Parameters.Add("LastCheckDate", SqlDbType.DateTime);
      sqlCommand.Parameters["LastCheckDate"].Value = this.LastAccess;
      return sqlCommand;
    }

    /// <summary>
    /// Create a connection to the database.
    /// </summary>
    /// <param name="connectionString">String to the connection.</param>
    /// <returns>Newly created connections.</returns>
    protected virtual IDbConnection CreateSQLConnection(string connectionString)
    {
      return new SqlConnection(connectionString);
    }
  }
}
