namespace CustomSearchesServicesLibrary.CustomSearches.Model.Datasets
{
    using System;
    using System.Collections.Generic;
    using CustomSearchesEFLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.Auth;
    using CustomSearchesServicesLibrary.Auth.Model;
    using CustomSearchesServicesLibrary.CustomSearches.Enumeration;
    using CustomSearchesServicesLibrary.CustomSearches.Model.CustomSearches;
    using CustomSearchesServicesLibrary.CustomSearches.Model.DatasetPostProcesses;
    using CustomSearchesServicesLibrary.CustomSearches.Model.InteractiveCharts;

    /// <summary>
    /// Model for the data of dataset.
    /// </summary>
    public class DatasetData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DatasetData"/> class.
        /// </summary>
        public DatasetData()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DatasetData"/> class.
        /// </summary>
        /// <param name="dataset">The dataset.</param>
        /// <param name="initializationType">Type of the initialization.</param>
        /// <param name="userDetails">The user details.</param>
        public DatasetData(Dataset dataset, ModelInitializationType initializationType, Dictionary<Guid, UserInfoData> userDetails)
        {
            this.CreatedBy = dataset.CreatedBy;
            this.LastModifiedBy = dataset.LastModifiedBy;
            this.LastExecutedBy = dataset.LastExecutedBy;
            this.CreatedTimestamp = dataset.CreatedTimestamp;
            this.LastModifiedTimestamp = dataset.LastModifiedTimestamp;
            this.LastExecutionTimestamp = dataset.LastExecutionTimestamp;
            this.CustomSearchDefinitionId = dataset.CustomSearchDefinitionId;
            this.DatasetId = dataset.DatasetId;
            this.DatasetName = dataset.DatasetName;
            this.Comments = dataset.Comments;
            this.ExecuteStoreProcedureElapsedMs = dataset.ExecuteStoreProcedureElapsedMs;
            this.GeneratedTableName = dataset.GeneratedTableName;
            this.GenerateSchemaElapsedMs = dataset.GenerateSchemaElapsedMs;
            this.IsLocked = dataset.IsLocked;
            this.ParameterValues = JsonHelper.DeserializeObject<CustomSearchParameterValueData[]>(dataset.ParameterValues);
            this.ParentFolderId = dataset.ParentFolderId;
            this.UserId = dataset.UserId;
            this.TotalRows = dataset.TotalRows;
            this.DatasetState = dataset.DataSetState;
            this.DatasetPostProcessState = dataset.DataSetPostProcessState;

            if (dataset.ParentFolder != null)
            {
                UserDetailsHelper.GatherUserDetails(dataset.ParentFolder.User, userDetails);
            }

            UserDetailsHelper.GatherUserDetails(dataset.CreatedByNavigation, userDetails);
            UserDetailsHelper.GatherUserDetails(dataset.LastModifiedByNavigation, userDetails);
            UserDetailsHelper.GatherUserDetails(dataset.LastExecutedByNavigation, userDetails);
            UserDetailsHelper.GatherUserDetails(dataset.User, userDetails);

            if (initializationType == ModelInitializationType.FullObject ||
                initializationType == ModelInitializationType.FullObjectWithDepedendencies)
            {
            }

            if (initializationType == ModelInitializationType.FullObjectWithDepedendencies)
            {
                this.Dependencies = new DatasetDependenciesData();

                if (dataset.DatasetPostProcess != null && dataset.DatasetPostProcess.Count > 0)
                {
                    List<DatasetPostProcessData> postProcesses = new List<DatasetPostProcessData>();
                    foreach (var postProcess in dataset.DatasetPostProcess)
                    {
                        postProcesses.Add(new DatasetPostProcessData(postProcess, initializationType, userDetails));
                    }

                    this.Dependencies.PostProcesses = postProcesses.ToArray();
                }

                if (dataset.InteractiveChart != null && dataset.InteractiveChart.Count > 0)
                {
                    List<InteractiveChartData> charts = new List<InteractiveChartData>();
                    foreach (var chart in dataset.InteractiveChart)
                    {
                        charts.Add(new InteractiveChartData(chart, initializationType, userDetails));
                    }

                    this.Dependencies.InteractiveCharts = charts.ToArray();
                }
            }
        }

        /// <summary>
        /// Gets or sets the identifier of the dataset.
        /// </summary>
        public Guid DatasetId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the custom search definition.
        /// </summary>
        public int CustomSearchDefinitionId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the user.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of parent folder.
        /// </summary>
        public int? ParentFolderId { get; set; }

        /// <summary>
        /// Gets or sets the folder path.
        /// </summary>
        public string FolderPath { get; set; }

        /// <summary>
        /// Gets or sets the dataset name.
        /// </summary>
        public string DatasetName { get; set; }

        /// <summary>
        /// Gets or sets the parameter values.
        /// </summary>
        public CustomSearchParameterValueData[] ParameterValues { get; set; }

        /// <summary>
        /// Gets or sets the generated table name.
        /// </summary>
        public string GeneratedTableName { get; set; }

        /// <summary>
        /// Gets or sets the total rows field.
        /// </summary>
        public int TotalRows { get; set; }

        /// <summary>
        /// Gets or sets the generate schema elapsed ms.
        /// </summary>
        public int GenerateSchemaElapsedMs { get; set; }

        /// <summary>
        /// Gets or sets the execute store procedure elapsed ms.
        /// </summary>
        public int ExecuteStoreProcedureElapsedMs { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the dataset is locked.
        /// </summary>
        public bool IsLocked { get; set; }

        /// <summary>
        /// Gets or sets the created by field.
        /// </summary>
        public Guid CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the last modified by field.
        /// </summary>
        public Guid LastModifiedBy { get; set; }

        /// <summary>
        /// Gets or sets the created time stamp.
        /// </summary>
        public DateTime CreatedTimestamp { get; set; }

        /// <summary>
        /// Gets or sets the last modified time stamp.
        /// </summary>
        public DateTime LastModifiedTimestamp { get; set; }

        /// <summary>
        /// Gets or sets the last execution time stamp.
        /// </summary>
        public DateTime? LastExecutionTimestamp { get; set; }

        /// <summary>
        /// Gets or sets the dataset state.
        /// </summary>
        public string DatasetState { get; set; }

        /// <summary>
        /// Gets or sets the dataset post process state.
        /// </summary>
        public string DatasetPostProcessState { get; set; }

        /// <summary>
        /// Gets or sets the dataset comments.
        /// </summary>
        public string Comments { get; set; }

        /// <summary>
        /// Gets or sets the last execute by field.
        /// </summary>
        public Guid? LastExecutedBy { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the version of the custom search definition is outdated.
        /// </summary>
        public bool IsOutdated { get; set; }

        /// <summary>
        /// Gets or sets the dataset dependencies.
        /// </summary>
        public DatasetDependenciesData Dependencies { get; set; }
    }
}
