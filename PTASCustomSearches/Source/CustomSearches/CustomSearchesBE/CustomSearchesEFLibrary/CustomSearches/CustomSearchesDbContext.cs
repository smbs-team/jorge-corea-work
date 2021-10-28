using System;
using System.Linq;
using System.Threading.Tasks;

using CustomSearchesEFLibrary.CustomSearches.Model;

using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

using PTASServicesCommon.TokenProvider;

namespace CustomSearchesEFLibrary.CustomSearches
{
    public partial class CustomSearchesDbContext : DbContext
    {
        /// <summary>
        /// The connection string password section.
        /// </summary>
        private const string ConnectionStringPasswordSection = "password";

        public CustomSearchesDbContext()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomSearchesDbContext" /> class.
        /// </summary>
        /// <param name="options">The options for this context.</param>
        /// <param name="tokenProvider">The token provider.</param>
        /// <param name="principalCredentials">The principal credentials.</param>
        /// <exception cref="System.ArgumentNullException">tokenProvider.</exception>
        public CustomSearchesDbContext(DbContextOptions<CustomSearchesDbContext> options, IServiceTokenProvider tokenProvider, ClientCredential principalCredentials)
            : base(options)
        {
            if (tokenProvider == null)
            {
                throw new System.ArgumentNullException(nameof(tokenProvider));
            }

            dynamic conn = null;
            try
            {
                conn = this.Database?.GetDbConnection();
            }
            catch (System.InvalidOperationException)
            {
                // System.InvalidOperationException is thrown when this is a memory database.  We consider the connection null in this case.
            }

            if (conn != null)
            {
                // We only used bearer token when password is not provided
                if (!conn.ConnectionString.Contains(CustomSearchesDbContext.ConnectionStringPasswordSection, System.StringComparison.OrdinalIgnoreCase))
                {
                    string token = null;
                    if (principalCredentials == null)
                    {
                        token = Task.Run(async () =>
                        {
                            return await tokenProvider.GetAccessTokenAsync(AzureTokenProvider.DbTokenEndpoint, AzureTokenProvider.TenantUrl);
                        }).Result;
                    }
                    else
                    {
                        token = Task.Run(async () =>
                        {
                            return await tokenProvider.GetAccessTokenAsync(AzureTokenProvider.DbTokenEndpoint, AzureTokenProvider.TenantId, principalCredentials);
                        }).Result;
                    }

                    conn.AccessToken = token;
                }
            }
        }

        public virtual DbSet<BackendUpdate> BackendUpdate { get; set; }
        public virtual DbSet<ChartTemplate> ChartTemplate { get; set; }
        public virtual DbSet<ChartType> ChartType { get; set; }
        public virtual DbSet<CustomSearchBackendEntity> CustomSearchBackendEntity { get; set; }
        public virtual DbSet<CustomSearchCategory> CustomSearchCategory { get; set; }
        public virtual DbSet<CustomSearchCategoryDefinition> CustomSearchCategoryDefinition { get; set; }
        public virtual DbSet<CustomSearchChartTemplate> CustomSearchChartTemplate { get; set; }
        public virtual DbSet<CustomSearchColumnDefinition> CustomSearchColumnDefinition { get; set; }
        public virtual DbSet<CustomSearchDefinition> CustomSearchDefinition { get; set; }
        public virtual DbSet<CustomSearchExpression> CustomSearchExpression { get; set; }
        public virtual DbSet<CustomSearchParameter> CustomSearchParameter { get; set; }
        public virtual DbSet<CustomSearchValidationRule> CustomSearchValidationRule { get; set; }
        public virtual DbSet<DataType> DataType { get; set; }
        public virtual DbSet<Dataset> Dataset { get; set; }
        public virtual DbSet<DatasetPostProcess> DatasetPostProcess { get; set; }
        public virtual DbSet<DatasetPostProcessSecondaryDataset> DatasetPostProcessSecondaryDataset { get; set; }
        public virtual DbSet<DatasetState> DatasetState { get; set; }
        public virtual DbSet<DatasetUserClientState> DatasetUserClientState { get; set; }
        public virtual DbSet<DbLockType> DbLockType { get; set; }
        public virtual DbSet<ExceptionPostProcessRule> ExceptionPostProcessRule { get; set; }
        public virtual DbSet<ExpressionRole> ExpressionRole { get; set; }
        public virtual DbSet<ExpressionType> ExpressionType { get; set; }
        public virtual DbSet<Folder> Folder { get; set; }
        public virtual DbSet<FolderType> FolderType { get; set; }
        public virtual DbSet<InteractiveChart> InteractiveChart { get; set; }
        public virtual DbSet<MetadataStoreItem> MetadataStoreItem { get; set; }
        public virtual DbSet<OwnerType> OwnerType { get; set; }
        public virtual DbSet<PostProcessType> PostProcessType { get; set; }
        public virtual DbSet<ProjectType> ProjectType { get; set; }
        public virtual DbSet<ProjectTypeCustomSearchDefinition> ProjectTypeCustomSearchDefinition { get; set; }
        public virtual DbSet<ProjectVersionType> ProjectVersionType { get; set; }
        public virtual DbSet<RangeType> RangeType { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<RscriptModel> RscriptModel { get; set; }
        public virtual DbSet<Stringmap> Stringmap { get; set; }
        public virtual DbSet<Systemuser> Systemuser { get; set; }
        public virtual DbSet<Systemuserroles> Systemuserroles { get; set; }
        public virtual DbSet<Team> Team { get; set; }
        public virtual DbSet<Teammembership> Teammembership { get; set; }
        public virtual DbSet<Teamprofiles> Teamprofiles { get; set; }
        public virtual DbSet<Teamroles> Teamroles { get; set; }
        public virtual DbSet<UserDataStoreItem> UserDataStoreItem { get; set; }
        public virtual DbSet<UserJobNotification> UserJobNotification { get; set; }
        public virtual DbSet<UserProject> UserProject { get; set; }
        public virtual DbSet<UserProjectDataset> UserProjectDataset { get; set; }

        /// <summary>
        /// Locks the dataset for altering.  Returns the dataset with the previous post process state if the lock
        /// was successful. NULL otherwise.
        /// </summary>
        /// <param name="datasetId">The dataset identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <param name="lockingJobId">The locking job identifier.</param>
        /// <returns>
        /// The locked dataset.
        /// </returns>
        public virtual async Task<Dataset> GetAlterDatasetLockAsync(Guid datasetId, Guid userId, int lockingJobId)
        {
            string stringCommand = $"exec [cus].[SP_GetAlterDatasetLock] @datasetId = N'{datasetId.ToString().ToLower()}', @userId = N'{userId.ToString().ToLower()}', @lockingJobId = {lockingJobId}";
            return (await this.Dataset.FromSqlRaw(stringCommand).ToListAsync()).FirstOrDefault();
        }

        /// <summary>
        /// Tests if it is possible to lock the dataset for altering.  Returns the dataset with the previous post process state
        /// if it is possible to lock. NULL otherwise.
        /// </summary>
        /// <param name="datasetId">The dataset identifier.</param>
        /// <param name="newState">The new state.</param>
        /// <param name="newPostProcessState">New state of the post process.</param>
        /// <param name="isRootLock">if set to <c>true</c> [is root lock].</param>
        /// <param name="userId">The user identifier.</param>
        /// <param name="lockingJobId">The locking job identifier.</param>
        /// <returns>
        /// The dataset.
        /// </returns>
        public virtual async Task<Dataset> TestAlterDatasetLockAsync(
            Guid datasetId,
            string newState,
            string newPostProcessState,
            bool isRootLock,
            Guid userId,
            int lockingJobId)
        {
            int isRootLockInt = isRootLock ? 1 : 0;
            string stringCommand =
                $"exec [cus].[SP_TestAlterDatasetLock] @datasetId = N'{datasetId.ToString().ToLower()}', " +
                $"@newPostProcessState = N'{newPostProcessState}', " +
                $"@newState = N'{newState}', " +
                $"@isRootLock = {isRootLockInt}, " +
                $"@userId = N'{userId.ToString().ToLower()}', " +
                $"@lockingJobId = {lockingJobId}";
            return (await this.Dataset.FromSqlRaw(stringCommand).ToListAsync()).FirstOrDefault();
        }

        /// <summary>
        /// Locks the dataset for altering.  Returns the dataset with the previous post process state if the lock
        /// was successful. NULL otherwise.
        /// </summary>
        /// <param name="datasetId">The dataset identifier.</param>
        /// <param name="newState">The new state.</param>
        /// <param name="newPostProcessState">New state of the post process.</param>
        /// <param name="isRootLock">if set to <c>true</c> [is root lock].</param>
        /// <param name="userId">The user identifier.</param>
        /// <param name="lockingJobId">The locking job identifier.</param>
        /// <returns>
        /// The locked dataset.
        /// </returns>
        public virtual async Task<Dataset> GetAlterDatasetLockAsyncV2(
            Guid datasetId,
            string newState,
            string newPostProcessState,
            bool isRootLock,
            Guid userId,
            int lockingJobId)
        {
            int isRootLockInt = isRootLock ? 1 : 0;
            string stringCommand =
                $"exec [cus].[SP_GetAlterDatasetLockV2] @datasetId = N'{datasetId.ToString().ToLower()}', " +
                $"@newPostProcessState = N'{newPostProcessState}', " +
                $"@newState = N'{newState}', " +
                $"@isRootLock = {isRootLockInt}, " +
                $"@userId = N'{userId.ToString().ToLower()}', " +
                $"@lockingJobId = {lockingJobId}";
            return (await this.Dataset.FromSqlRaw(stringCommand).ToListAsync()).FirstOrDefault();
        }

        /// <summary>
        /// Releases a dataset alter lock.  Returns the dataset with the previous post process state if the lock
        /// was successfully released. NULL otherwise.
        /// </summary>
        /// <param name="datasetId">The dataset identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <param name="newPostProcessState">The new post process state for the dataset.</param>
        /// <returns>
        /// The locked dataset.
        /// </returns>
        public virtual async Task<Dataset> ReleaseAlterDatasetLockAsync(Guid datasetId, Guid userId, string newPostProcessState)
        {
            string stringCommand = $"exec [cus].[SP_ReleaseAlterDatasetLock] @datasetId = '{datasetId.ToString()}', @newPostProcessState = '{newPostProcessState}', @userId = '{userId.ToString()}'";
            return (await this.Dataset.FromSqlRaw(stringCommand).ToListAsync()).FirstOrDefault();
        }

        /// <summary>
        /// Releases a dataset alter lock.  Returns the dataset with the previous post process state if the lock
        /// was successfully released. NULL otherwise.
        /// </summary>
        /// <param name="datasetId">The dataset identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <param name="newState">The new of the dataset.</param>
        /// <param name="newPostProcessState">The new post process state for the dataset.</param>
        /// <returns>
        /// The locked dataset.
        /// </returns>
        public virtual async Task<Dataset> ReleaseAlterDatasetLockAsyncV2(
            Guid datasetId,
            Guid userId,
            string newState,
            string newPostProcessState)
        {
            string stringCommand =
                $"exec [cus].[SP_ReleaseAlterDatasetLockV2] @datasetId = '{datasetId.ToString()}', " +
                $"@newPostProcessState = '{newPostProcessState}', " +
                $"@newState = '{newState}', " +
                $"@userId = '{userId.ToString()}'";
            return (await this.Dataset.FromSqlRaw(stringCommand).ToListAsync()).FirstOrDefault();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BackendUpdate>(entity =>
            {
                entity.ToTable("BackendUpdate", "cus");

                entity.HasIndex(e => e.DatasetPostProcessId)
                    .HasName("IX_BackendUpdate_PostProcessId");

                entity.HasIndex(e => new { e.DatasetPostProcessId, e.ExportState })
                    .HasName("IX_BackendUpdate_PostProcessId_ExportState");

                entity.Property(e => e.ExportState).HasMaxLength(256);

                entity.Property(e => e.SingleRowMajor).HasMaxLength(6);

                entity.Property(e => e.SingleRowMinor).HasMaxLength(4);

                entity.Property(e => e.UpdatesJson).IsRequired();

                entity.HasOne(d => d.DatasetPostProcess)
                    .WithMany(p => p.BackendUpdate)
                    .HasForeignKey(d => d.DatasetPostProcessId)
                    .HasConstraintName("FK_BackendUpdate_PostProcess");
            });

            modelBuilder.Entity<ChartTemplate>(entity =>
            {
                entity.ToTable("ChartTemplate", "cus");

                entity.HasIndex(e => e.ChartTitle);

                entity.Property(e => e.ChartTitle)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.ChartType)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.CreatedTimestamp)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.LastModifiedTimestamp)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.ChartTypeNavigation)
                    .WithMany(p => p.ChartTemplate)
                    .HasForeignKey(d => d.ChartType)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ChartTemplate_ToChartType");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.ChartTemplateCreatedByNavigation)
                    .HasForeignKey(d => d.CreatedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ChartTemplate_ToSystemUser_CreatedBy")
                    .IsRequired(false);

                entity.HasOne(d => d.LastModifiedByNavigation)
                    .WithMany(p => p.ChartTemplateLastModifiedByNavigation)
                    .HasForeignKey(d => d.LastModifiedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ChartTemplate_ToSystemUser_LastModifiedBy")
                    .IsRequired(false);
            });

            modelBuilder.Entity<ChartType>(entity =>
            {
                entity.HasKey(e => e.ChartType1)
                    .HasName("PK_ChartType_ChartType");

                entity.ToTable("ChartType", "cus");

                entity.Property(e => e.ChartType1)
                    .HasColumnName("ChartType")
                    .HasMaxLength(256);
            });

            modelBuilder.Entity<CustomSearchBackendEntity>(entity =>
            {
                entity.ToTable("CustomSearchBackendEntity", "cus");

                entity.HasIndex(e => e.CustomSearchDefinitionId);

                entity.HasIndex(e => e.CustomSearchKeyFieldName);

                entity.Property(e => e.BackendEntityKeyFieldName).HasMaxLength(4000);

                entity.Property(e => e.BackendEntityName).HasMaxLength(4000);

                entity.Property(e => e.CustomSearchKeyFieldName).HasMaxLength(256);

                entity.HasOne(d => d.CustomSearchDefinition)
                    .WithMany(p => p.CustomSearchBackendEntity)
                    .HasForeignKey(d => d.CustomSearchDefinitionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CustomSearchBackendEntity_ToCustomSearchDefinition");
            });

            modelBuilder.Entity<CustomSearchCategory>(entity =>
            {
                entity.ToTable("CustomSearchCategory", "cus");

                entity.HasIndex(e => e.CategoryName);

                entity.Property(e => e.CategoryDescription)
                    .IsRequired()
                    .HasMaxLength(2048);

                entity.Property(e => e.CategoryName)
                    .IsRequired()
                    .HasMaxLength(256);
            });

            modelBuilder.Entity<CustomSearchCategoryDefinition>(entity =>
            {
                entity.HasKey(e => new { e.CustomSearchCategoryId, e.CustomSearchDefinitionId })
                    .HasName("PK_CustomSearchCategory_Definition_CustomSearchCategoryIdCustomSearchDefinitionId");

                entity.ToTable("CustomSearchCategory_Definition", "cus");

                entity.HasOne(d => d.CustomSearchCategory)
                    .WithMany(p => p.CustomSearchCategoryDefinition)
                    .HasForeignKey(d => d.CustomSearchCategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CustomSearchCategory_Definition_ToCustomSearchCategory");

                entity.HasOne(d => d.CustomSearchDefinition)
                    .WithMany(p => p.CustomSearchCategoryDefinition)
                    .HasForeignKey(d => d.CustomSearchDefinitionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CustomSearchCategory_Definition_ToCustomSearchDefinition");
            });

            modelBuilder.Entity<CustomSearchChartTemplate>(entity =>
            {
                entity.HasKey(e => new { e.ChartTemplateId, e.CustomSearchDefinitionId })
                    .HasName("PK_CustomSearch_ChartTemplate_ChartTemplateIdCustomSearchDefinitionId");

                entity.ToTable("CustomSearch_ChartTemplate", "cus");

                entity.HasOne(d => d.ChartTemplate)
                    .WithMany(p => p.CustomSearchChartTemplate)
                    .HasForeignKey(d => d.ChartTemplateId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CustomSearch_ChartTemplate_To_ChartTemplate");

                entity.HasOne(d => d.CustomSearchDefinition)
                    .WithMany(p => p.CustomSearchChartTemplate)
                    .HasForeignKey(d => d.CustomSearchDefinitionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CustomSearch_ChartTemplate_To_CustomSearchDefinition");
            });

            modelBuilder.Entity<CustomSearchColumnDefinition>(entity =>
            {
                entity.ToTable("CustomSearchColumnDefinition", "cus");

                entity.HasIndex(e => e.ColumnType);

                entity.HasIndex(e => e.CustomSearchDefinitionId);

                entity.Property(e => e.BackendEntityFieldName).HasMaxLength(4000);

                entity.Property(e => e.BackendEntityName).HasMaxLength(4000);

                entity.Property(e => e.CanBeUsedAsLookup).HasDefaultValueSql("((0))");

                entity.Property(e => e.ColumDefinitionExtensions).HasMaxLength(4000);

                entity.Property(e => e.ColumnCategory).HasMaxLength(256);

                entity.Property(e => e.ColumnEditRoles).HasMaxLength(4000);

                entity.Property(e => e.ColumnName).HasMaxLength(256);

                entity.Property(e => e.ColumnType).HasMaxLength(256);

                entity.Property(e => e.DependsOnColumn).HasMaxLength(256);

                entity.HasOne(d => d.ColumnTypeNavigation)
                    .WithMany(p => p.CustomSearchColumnDefinition)
                    .HasForeignKey(d => d.ColumnType)
                    .HasConstraintName("FK_CustomSearchColumnDefinition_ToColumnType");

                entity.HasOne(d => d.CustomSearchDefinition)
                    .WithMany(p => p.CustomSearchColumnDefinition)
                    .HasForeignKey(d => d.CustomSearchDefinitionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CustomSearchColumnDefinition_ToCustomSearchDefinition");
            });

            modelBuilder.Entity<CustomSearchDefinition>(entity =>
            {
                entity.ToTable("CustomSearchDefinition", "cus");

                entity.HasIndex(e => e.CustomSearchDefinitionId)
                    .HasName("IX_CustomSearchValidationRule_CustomSearchDefinitionId");

                entity.HasIndex(e => e.CustomSearchName);

                entity.Property(e => e.CreatedTimestamp)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CustomSearchDescription)
                    .IsRequired()
                    .HasMaxLength(2048);

                entity.Property(e => e.CustomSearchName)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.DatasetEditRoles).HasMaxLength(4000);

                entity.Property(e => e.ExecutionRoles).HasMaxLength(4000);

                entity.Property(e => e.LastModifiedTimestamp)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.RowLevelEditRolesColumn).HasMaxLength(256);

                entity.Property(e => e.StoredProcedureName)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.TableInputParameterDbType).HasMaxLength(256);

                entity.Property(e => e.TableInputParameterName).HasMaxLength(256);
            });

            modelBuilder.Entity<CustomSearchExpression>(entity =>
            {
                entity.ToTable("CustomSearchExpression", "cus");

                entity.HasIndex(e => e.ChartTemplateId);

                entity.HasIndex(e => e.CustomSearchColumnDefinitionId)
                    .HasName("IX_CustomSearchExpression_CustomSearchColumnDefinitionID");

                entity.HasIndex(e => e.CustomSearchParameterId)
                    .HasName("IX_CustomSearchExpression_CustomSearchParameterID");

                entity.HasIndex(e => e.CustomSearchValidationRuleId);

                entity.HasIndex(e => e.DatasetChartId)
                    .HasName("IX_CustomSearchExpression_DatasetChartID");

                entity.HasIndex(e => e.DatasetId)
                    .HasName("IX_CustomSearchExpression_DatasetId");

                entity.HasIndex(e => e.DatasetPostProcessId)
                    .HasName("IX_CustomSearchExpression_DatasetPostProcessID");

                entity.HasIndex(e => e.ExceptionPostProcessRuleId);

                entity.HasIndex(e => e.ProjectTypeId)
                    .HasName("IX_CustomSearchExpression_ProjectTypeID");

                entity.HasIndex(e => e.RscriptModelId);

                entity.Property(e => e.Category).HasMaxLength(256);

                entity.Property(e => e.ColumnName).HasMaxLength(256);

                entity.Property(e => e.DatasetId).HasColumnName("DatasetID");

                entity.Property(e => e.ExpressionGroup).HasMaxLength(256);

                entity.Property(e => e.ExpressionRole)
                    .IsRequired()
                    .HasMaxLength(256)
                    .HasDefaultValueSql("('ColumnExpression')");

                entity.Property(e => e.ExpressionType)
                    .IsRequired()
                    .HasMaxLength(256)
                    .HasDefaultValueSql("('T-SQL')");

                entity.Property(e => e.OwnerType)
                    .HasMaxLength(256)
                    .HasDefaultValueSql("('Dataset')");

                entity.Property(e => e.RscriptModelId).HasColumnName("RScriptModelId");

                entity.HasOne(d => d.ChartTemplate)
                    .WithMany(p => p.CustomSearchExpression)
                    .HasForeignKey(d => d.ChartTemplateId)
                    .HasConstraintName("FK_CustomSearchExpression_ChartTemplate");

                entity.HasOne(d => d.CustomSearchColumnDefinition)
                    .WithMany(p => p.CustomSearchExpression)
                    .HasForeignKey(d => d.CustomSearchColumnDefinitionId)
                    .HasConstraintName("FK_CustomSearchExpression_CustomSearchColumnDefinition");

                entity.HasOne(d => d.CustomSearchParameter)
                    .WithMany(p => p.CustomSearchExpression)
                    .HasForeignKey(d => d.CustomSearchParameterId)
                    .HasConstraintName("FK_CustomSearchExpression_CustomSearchParameter");

                entity.HasOne(d => d.CustomSearchValidationRule)
                    .WithMany(p => p.CustomSearchExpression)
                    .HasForeignKey(d => d.CustomSearchValidationRuleId)
                    .HasConstraintName("FK_CustomSearchExpression_CustomSearchValidationRule");

                entity.HasOne(d => d.DatasetChart)
                    .WithMany(p => p.CustomSearchExpression)
                    .HasForeignKey(d => d.DatasetChartId)
                    .HasConstraintName("FK_CustomSearchExpression_ToInteractiveChart");

                entity.HasOne(d => d.Dataset)
                    .WithMany(p => p.CustomSearchExpression)
                    .HasForeignKey(d => d.DatasetId)
                    .HasConstraintName("FK_CustomSearchExpression_ToDataset");

                entity.HasOne(d => d.DatasetPostProcess)
                    .WithMany(p => p.CustomSearchExpression)
                    .HasForeignKey(d => d.DatasetPostProcessId)
                    .HasConstraintName("FK_CustomSearchExpression_DatasetPostProcess");

                entity.HasOne(d => d.ExceptionPostProcessRule)
                    .WithMany(p => p.CustomSearchExpression)
                    .HasForeignKey(d => d.ExceptionPostProcessRuleId)
                    .HasConstraintName("FK_CustomSearchExpression_ExceptionPostProcessRule");

                entity.HasOne(d => d.ExpressionRoleNavigation)
                    .WithMany(p => p.CustomSearchExpression)
                    .HasForeignKey(d => d.ExpressionRole)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CustomSearchExpression_ToExpressionRole");

                entity.HasOne(d => d.ExpressionTypeNavigation)
                    .WithMany(p => p.CustomSearchExpression)
                    .HasForeignKey(d => d.ExpressionType)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CustomSearchExpression_ToExpressionType");

                entity.HasOne(d => d.OwnerTypeNavigation)
                    .WithMany(p => p.CustomSearchExpression)
                    .HasForeignKey(d => d.OwnerType)
                    .HasConstraintName("FK_CustomSearchExpression_ToOwnerType");

                entity.HasOne(d => d.ProjectType)
                    .WithMany(p => p.CustomSearchExpression)
                    .HasForeignKey(d => d.ProjectTypeId)
                    .HasConstraintName("FK_CustomSearchExpression_ToProjectType");

                entity.HasOne(d => d.RscriptModel)
                    .WithMany(p => p.CustomSearchExpression)
                    .HasForeignKey(d => d.RscriptModelId)
                    .HasConstraintName("FK_CustomSearchExpression_ToRScriptModel");
            });

            modelBuilder.Entity<CustomSearchParameter>(entity =>
            {
                entity.ToTable("CustomSearchParameter", "cus");

                entity.HasIndex(e => e.CustomSearchDefinitionId);

                entity.HasIndex(e => e.ParameterName);

                entity.Property(e => e.DisplayName).HasMaxLength(256);

                entity.Property(e => e.OwnerType)
                    .HasMaxLength(256)
                    .HasDefaultValueSql("('CustomSearchDefinition')");

                entity.Property(e => e.ParameterDataType)
                    .IsRequired()
                    .HasMaxLength(256)
                    .HasDefaultValueSql("('int')");

                entity.Property(e => e.ParameterDescription)
                    .IsRequired()
                    .HasMaxLength(2048);

                entity.Property(e => e.ParameterExtensions).HasMaxLength(4000);

                entity.Property(e => e.ParameterGroupName)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.ParameterName)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.ParameterRangeType)
                    .IsRequired()
                    .HasMaxLength(256)
                    .HasDefaultValueSql("('NotRange')");

                entity.Property(e => e.RscriptModelId).HasColumnName("RScriptModelId");

                entity.HasOne(d => d.CustomSearchDefinition)
                    .WithMany(p => p.CustomSearchParameter)
                    .HasForeignKey(d => d.CustomSearchDefinitionId)
                    .HasConstraintName("FK_CustomSearchParameter_ToCustomSearchDefinition");

                entity.HasOne(d => d.OwnerTypeNavigation)
                    .WithMany(p => p.CustomSearchParameter)
                    .HasForeignKey(d => d.OwnerType)
                    .HasConstraintName("FK_CustomSearchParameter_ToOwnerType");

                entity.HasOne(d => d.ParameterDataTypeNavigation)
                    .WithMany(p => p.CustomSearchParameter)
                    .HasForeignKey(d => d.ParameterDataType)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CustomSearchParameter_ToDataType");

                entity.HasOne(d => d.ParameterRangeTypeNavigation)
                    .WithMany(p => p.CustomSearchParameter)
                    .HasForeignKey(d => d.ParameterRangeType)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CustomSearchParameter_ToRangeType");

                entity.HasOne(d => d.RscriptModel)
                    .WithMany(p => p.CustomSearchParameter)
                    .HasForeignKey(d => d.RscriptModelId)
                    .HasConstraintName("FK_CustomSearchParameter_ToRScriptModel");
            });

            modelBuilder.Entity<CustomSearchValidationRule>(entity =>
            {
                entity.ToTable("CustomSearchValidationRule", "cus");

                entity.HasIndex(e => e.CustomSearchValidationRuleId)
                    .HasName("IX_CustomSearchExpression_CustomSearchValidationRuleID");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(4000);

                entity.HasOne(d => d.CustomSearchDefinition)
                    .WithMany(p => p.CustomSearchValidationRule)
                    .HasForeignKey(d => d.CustomSearchDefinitionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CustomSearchValidationRule_ToCustomSearch");
            });

            modelBuilder.Entity<DataType>(entity =>
            {
                entity.HasKey(e => e.DataType1)
                    .HasName("PK_DataType_DataType");

                entity.ToTable("DataType", "cus");

                entity.Property(e => e.DataType1)
                    .HasColumnName("DataType")
                    .HasMaxLength(256);
            });

            modelBuilder.Entity<Dataset>(entity =>
            {
                entity.ToTable("Dataset", "cus");

                entity.HasIndex(e => e.CustomSearchDefinitionId);

                entity.HasIndex(e => e.ParentFolderId);

                entity.HasIndex(e => e.UserId);

                entity.Property(e => e.DatasetId).ValueGeneratedNever();

                entity.Property(e => e.CreatedTimestamp)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DataSetPostProcessState)
                    .IsRequired()
                    .HasMaxLength(256)
                    .HasDefaultValueSql("('NotProcessed')");

                entity.Property(e => e.DataSetState)
                    .IsRequired()
                    .HasMaxLength(256)
                    .HasDefaultValueSql("(N'NotProcessed')");

                entity.Property(e => e.DatasetName)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.DbLockTime).HasColumnType("datetime");

                entity.Property(e => e.DbLockType).HasMaxLength(256);

                entity.Property(e => e.GeneratedTableName)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.LastExecutionTimestamp).HasColumnType("datetime");

                entity.Property(e => e.LastModifiedTimestamp)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.LockingJobId).HasDefaultValueSql("((-1))");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.DatasetCreatedByNavigation)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_Dataset_ToSystemUser_CreatedBy")
                    .IsRequired(false);

                entity.HasOne(d => d.CustomSearchDefinition)
                    .WithMany(p => p.Dataset)
                    .HasForeignKey(d => d.CustomSearchDefinitionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Dataset_Definition_ToCustomSearchDefinition");

                entity.HasOne(d => d.DataSetStateNavigation)
                    .WithMany(p => p.Dataset)
                    .HasForeignKey(d => d.DataSetState)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Dataset_DatasetState_ToDatasetState");

                entity.HasOne(d => d.DbLockTypeNavigation)
                    .WithMany(p => p.Dataset)
                    .HasForeignKey(d => d.DbLockType)
                    .HasConstraintName("FK_Dataset_ToDbLockType");

                entity.HasOne(d => d.LastExecutedByNavigation)
                    .WithMany(p => p.DatasetLastExecutedByNavigation)
                    .HasForeignKey(d => d.LastExecutedBy)
                    .HasConstraintName("FK_Dataset_ToSystemUser_LastExecutedBy")
                    .IsRequired(false);

                entity.HasOne(d => d.LastModifiedByNavigation)
                    .WithMany(p => p.DatasetLastModifiedByNavigation)
                    .HasForeignKey(d => d.LastModifiedBy)
                    .HasConstraintName("FK_Dataset_ToSystemUser_LastModifiedBy")
                    .IsRequired(false);

                entity.HasOne(d => d.ParentFolder)
                    .WithMany(p => p.Dataset)
                    .HasForeignKey(d => d.ParentFolderId)
                    .HasConstraintName("FK_Dataset_ToParentFolderId");

                entity.HasOne(d => d.SourceDataset)
                    .WithMany(p => p.InverseSourceDataset)
                    .HasForeignKey(d => d.SourceDatasetId)
                    .HasConstraintName("FK_Dataset_ToDataset");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.DatasetUser)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Dataset_ToSystemUser_UserId")
                    .IsRequired(false);
            });

            modelBuilder.Entity<DatasetPostProcess>(entity =>
            {
                entity.ToTable("DatasetPostProcess", "cus");

                entity.HasIndex(e => e.DatasetId);

                entity.HasIndex(e => e.PostProcessType);

                entity.HasIndex(e => e.Priority);

                entity.Property(e => e.CreatedTimestamp)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.LastExecutionTimestamp).HasColumnType("datetime");

                entity.Property(e => e.LastModifiedTimestamp)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.PostProcessName).HasMaxLength(256);

                entity.Property(e => e.PostProcessRole).HasMaxLength(256);

                entity.Property(e => e.PostProcessSubType).HasMaxLength(256);

                entity.Property(e => e.PostProcessType)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.RscriptModelId).HasColumnName("RScriptModelId");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.DatasetPostProcessCreatedByNavigation)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_DatasetPostProcess_ToSystemUser_CreatedBy")
                    .IsRequired(false);

                entity.HasOne(d => d.Dataset)
                    .WithMany(p => p.DatasetPostProcess)
                    .HasForeignKey(d => d.DatasetId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DatasetPostProcess_ToDataset");

                entity.HasOne(d => d.LastModifiedByNavigation)
                    .WithMany(p => p.DatasetPostProcessLastModifiedByNavigation)
                    .HasForeignKey(d => d.LastModifiedBy)
                    .HasConstraintName("FK_DatasetPostProcess_ToSystemUser_LastModifiedBy")
                    .IsRequired(false);

                entity.HasOne(d => d.PostProcessTypeNavigation)
                    .WithMany(p => p.DatasetPostProcess)
                    .HasForeignKey(d => d.PostProcessType)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Dataset_ToPostProcessType");

                entity.HasOne(d => d.PrimaryDatasetPostProcess)
                    .WithMany(p => p.InversePrimaryDatasetPostProcess)
                    .HasForeignKey(d => d.PrimaryDatasetPostProcessId)
                    .HasConstraintName("FK_DatasetPostProcess_ToPrimaryPostProcess");

                entity.HasOne(d => d.RscriptModel)
                    .WithMany(p => p.DatasetPostProcess)
                    .HasForeignKey(d => d.RscriptModelId)
                    .HasConstraintName("FK_DatasetPostProcess_ToRScriptModel");
            });

            modelBuilder.Entity<DatasetPostProcessSecondaryDataset>(entity =>
            {
                entity.HasKey(e => new { e.DatasetPostProcessId, e.SecondaryDatasetId });

                entity.ToTable("DatasetPostProcess_SecondaryDataset", "cus");

                entity.HasOne(d => d.DatasetPostProcess)
                    .WithMany(p => p.DatasetPostProcessSecondaryDataset)
                    .HasForeignKey(d => d.DatasetPostProcessId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DatasetPostProcess_SecondaryDataset_ToPostProcess");

                entity.HasOne(d => d.SecondaryDataset)
                    .WithMany(p => p.DatasetPostProcessSecondaryDataset)
                    .HasForeignKey(d => d.SecondaryDatasetId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DatasetPostProcess_SecondaryDataset_ToDataset");
            });

            modelBuilder.Entity<DatasetState>(entity =>
            {
                entity.HasKey(e => e.DatasetState1)
                    .HasName("PK_DatasetState_DatasetState");

                entity.ToTable("DatasetState", "cus");

                entity.Property(e => e.DatasetState1)
                    .HasColumnName("DatasetState")
                    .HasMaxLength(256);
            });

            modelBuilder.Entity<DatasetUserClientState>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.DatasetId })
                    .HasName("PK_DatasetUserClientState_UserIdDatasetId");

                entity.ToTable("DatasetUserClientState", "cus");

                entity.HasOne(d => d.Dataset)
                    .WithMany(p => p.DatasetUserClientState)
                    .HasForeignKey(d => d.DatasetId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DatasetUserClientState_ToDataset");
            });

            modelBuilder.Entity<DbLockType>(entity =>
            {
                entity.HasKey(e => e.DbLockType1)
                    .HasName("PK_DbLockType_DbLockType");

                entity.ToTable("DbLockType", "cus");

                entity.Property(e => e.DbLockType1)
                    .HasColumnName("DbLockType")
                    .HasMaxLength(256);
            });

            modelBuilder.Entity<ExceptionPostProcessRule>(entity =>
            {
                entity.ToTable("ExceptionPostProcessRule", "cus");

                entity.HasIndex(e => e.DatasetPostProcessId)
                    .HasName("IX_ExceptionPostProcessRulet_DatasetPostProcessId");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(4000);

                entity.Property(e => e.GroupName).HasMaxLength(256);

                entity.HasOne(d => d.DatasetPostProcess)
                    .WithMany(p => p.ExceptionPostProcessRule)
                    .HasForeignKey(d => d.DatasetPostProcessId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ExceptionPostProcessRule_ToDatasetPostProcess");
            });

            modelBuilder.Entity<ExpressionRole>(entity =>
            {
                entity.HasKey(e => e.ExpressionRole1)
                    .HasName("PK_ExpressionRole_ExpressionRole");

                entity.ToTable("ExpressionRole", "cus");

                entity.Property(e => e.ExpressionRole1)
                    .HasColumnName("ExpressionRole")
                    .HasMaxLength(256);
            });

            modelBuilder.Entity<ExpressionType>(entity =>
            {
                entity.HasKey(e => e.ExpressionType1)
                    .HasName("PK_ExpressionType_ExpressionType");

                entity.ToTable("ExpressionType", "cus");

                entity.Property(e => e.ExpressionType1)
                    .HasColumnName("ExpressionType")
                    .HasMaxLength(256);
            });

            modelBuilder.Entity<Folder>(entity =>
            {
                entity.ToTable("Folder", "cus");

                entity.HasIndex(e => e.FolderName);

                entity.HasIndex(e => e.FolderType);

                entity.HasIndex(e => e.ParentFolderId);

                entity.HasIndex(e => e.UserId);

                entity.Property(e => e.FolderName)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.FolderType)
                    .IsRequired()
                    .HasMaxLength(256)
                    .HasDefaultValueSql("('User')");

                entity.HasOne(d => d.FolderTypeNavigation)
                    .WithMany(p => p.Folder)
                    .HasForeignKey(d => d.FolderType)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Folder_ToFolderType");

                entity.HasOne(d => d.ParentFolder)
                    .WithMany(p => p.InverseParentFolder)
                    .HasForeignKey(d => d.ParentFolderId)
                    .HasConstraintName("FK_Folder_ToFolder");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Folder)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Folder_ToSystemUser")
                    .IsRequired(false);
            });

            modelBuilder.Entity<FolderType>(entity =>
            {
                entity.HasKey(e => e.FolderType1)
                    .HasName("PK_FolderType_FolderType");

                entity.ToTable("FolderType", "cus");

                entity.Property(e => e.FolderType1)
                    .HasColumnName("FolderType")
                    .HasMaxLength(256);
            });

            modelBuilder.Entity<InteractiveChart>(entity =>
            {
                entity.ToTable("InteractiveChart", "cus");

                entity.HasIndex(e => e.DatasetId);

                entity.Property(e => e.ChartTitle)
                    .IsRequired()
                    .HasMaxLength(4000);

                entity.Property(e => e.ChartType)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.CreatedTimestamp)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.LastModifiedTimestamp)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.ChartTypeNavigation)
                    .WithMany(p => p.InteractiveChart)
                    .HasForeignKey(d => d.ChartType)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_InteractiveChart_ToChartType");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.InteractiveChartCreatedByNavigation)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_InteractiveChart_ToSystemUser_CreatedBy")
                    .IsRequired(false);

                entity.HasOne(d => d.Dataset)
                    .WithMany(p => p.InteractiveChart)
                    .HasForeignKey(d => d.DatasetId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_InteractiveChart_ToDataset");

                entity.HasOne(d => d.LastModifiedByNavigation)
                    .WithMany(p => p.InteractiveChartLastModifiedByNavigation)
                    .HasForeignKey(d => d.LastModifiedBy)
                    .HasConstraintName("FK_InteractiveChart_ToSystemUser_LastModifiedBy")
                    .IsRequired(false);
            });

            modelBuilder.Entity<MetadataStoreItem>(entity =>
            {
                entity.HasKey(e => new { e.StoreType, e.ItemName, e.Version })
                    .HasName("PK_MetadataStoreItem_VersionStoreTypeMetadataStoreItemId");

                entity.Property(e => e.StoreType)
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entity.Property(e => e.ItemName)
                    .HasMaxLength(64)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Version).HasDefaultValueSql("((-1))");

                entity.Property(e => e.MetadataStoreItemId).ValueGeneratedOnAddOrUpdate();
            });

            modelBuilder.Entity<OwnerType>(entity =>
            {
                entity.HasKey(e => e.OwnerType1)
                    .HasName("PK_OwnerType_OwnerType");

                entity.ToTable("OwnerType", "cus");

                entity.Property(e => e.OwnerType1)
                    .HasColumnName("OwnerType")
                    .HasMaxLength(256);
            });

            modelBuilder.Entity<PostProcessType>(entity =>
            {
                entity.HasKey(e => e.PostProcessType1)
                    .HasName("PK_PostProcessType_PostProcessType");

                entity.ToTable("PostProcessType", "cus");

                entity.Property(e => e.PostProcessType1)
                    .HasColumnName("PostProcessType")
                    .HasMaxLength(256);
            });

            modelBuilder.Entity<ProjectType>(entity =>
            {
                entity.ToTable("ProjectType", "cus");

                entity.Property(e => e.WaterFrontLotSizeColumnName).IsRequired().HasMaxLength(256);

                entity.Property(e => e.DryLotSizeColumnName).IsRequired().HasMaxLength(256);

                entity.Property(e => e.EffectiveLotSizeColumnName).IsRequired().HasMaxLength(256);

                entity.Property(e => e.ApplyModelUserFilterColumnName).HasMaxLength(256);

                entity.Property(e => e.BulkUpdateProcedureName).HasMaxLength(256);

                entity.Property(e => e.ProjectTypeName).HasMaxLength(256);
            });

            modelBuilder.Entity<ProjectTypeCustomSearchDefinition>(entity =>
            {
                entity.HasKey(e => new { e.ProjectTypeId, e.DatasetRole })
                    .HasName("PK_ProjectType_CustomSearchDefinition_ProjectTypeIdDatasetRole");

                entity.ToTable("ProjectType_CustomSearchDefinition", "cus");

                entity.Property(e => e.DatasetRole)
                    .HasMaxLength(256)
                    .HasDefaultValueSql("('Population')");

                entity.HasOne(d => d.CustomSearchDefinition)
                    .WithMany(p => p.ProjectTypeCustomSearchDefinition)
                    .HasForeignKey(d => d.CustomSearchDefinitionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProjectType_CustomSearchDefinition_ToCustomSearchDefinition");

                entity.HasOne(d => d.ProjectType)
                    .WithMany(p => p.ProjectTypeCustomSearchDefinition)
                    .HasForeignKey(d => d.ProjectTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProjectType_CustomSearchDefinition_ToProjectType");
            });

            modelBuilder.Entity<ProjectVersionType>(entity =>
            {
                entity.HasKey(e => e.ProjectVersionType1);

                entity.ToTable("ProjectVersionType", "cus");

                entity.Property(e => e.ProjectVersionType1)
                    .HasColumnName("ProjectVersionType")
                    .HasMaxLength(256);
            });

            modelBuilder.Entity<RangeType>(entity =>
            {
                entity.HasKey(e => e.RangeType1)
                    .HasName("PK_RangeType_RangeType");

                entity.ToTable("RangeType", "cus");

                entity.Property(e => e.RangeType1)
                    .HasColumnName("RangeType")
                    .HasMaxLength(256);
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("role", "dynamics");

                entity.HasIndex(e => e.Modifiedon)
                    .HasName("Idx_role_modifiedon");

                entity.Property(e => e.Roleid)
                    .HasColumnName("roleid")
                    .ValueGeneratedNever();

                entity.Property(e => e.BusinessunitidValue).HasColumnName("_businessunitid_value");

                entity.Property(e => e.Canbedeleted).HasColumnName("canbedeleted");

                entity.Property(e => e.Componentstate).HasColumnName("componentstate");

                entity.Property(e => e.CreatedbyValue).HasColumnName("_createdby_value");

                entity.Property(e => e.Createdon).HasColumnName("createdon");

                entity.Property(e => e.CreatedonbehalfbyValue).HasColumnName("_createdonbehalfby_value");

                entity.Property(e => e.Importsequencenumber).HasColumnName("importsequencenumber");

                entity.Property(e => e.Iscustomizable).HasColumnName("iscustomizable");

                entity.Property(e => e.Isinherited).HasColumnName("isinherited");

                entity.Property(e => e.Ismanaged).HasColumnName("ismanaged");

                entity.Property(e => e.ModifiedbyValue).HasColumnName("_modifiedby_value");

                entity.Property(e => e.Modifiedon).HasColumnName("modifiedon");

                entity.Property(e => e.ModifiedonbehalfbyValue).HasColumnName("_modifiedonbehalfby_value");

                entity.Property(e => e.Name).HasColumnName("name");

                entity.Property(e => e.Organizationid).HasColumnName("organizationid");

                entity.Property(e => e.Overriddencreatedon).HasColumnName("overriddencreatedon");

                entity.Property(e => e.Overwritetime).HasColumnName("overwritetime");

                entity.Property(e => e.ParentroleidValue).HasColumnName("_parentroleid_value");

                entity.Property(e => e.ParentrootroleidValue).HasColumnName("_parentrootroleid_value");

                entity.Property(e => e.Roleidunique).HasColumnName("roleidunique");

                entity.Property(e => e.RoletemplateidValue).HasColumnName("_roletemplateid_value");

                entity.Property(e => e.Solutionid).HasColumnName("solutionid");

                entity.Property(e => e.Versionnumber).HasColumnName("versionnumber");

                entity.HasOne(d => d.ParentroleidValueNavigation)
                    .WithMany(p => p.InverseParentroleidValueNavigation)
                    .HasForeignKey(d => d.ParentroleidValue)
                    .HasConstraintName("FK_role_role");
            });

            modelBuilder.Entity<RscriptModel>(entity =>
            {
                entity.ToTable("RScriptModel", "cus");

                entity.Property(e => e.RscriptModelId).HasColumnName("RScriptModelId");

                entity.Property(e => e.PredictedTsqlExpression).HasColumnName("PredictedTSqlExpression");

                entity.Property(e => e.Rscript).HasColumnName("RScript");

                entity.Property(e => e.RscriptDisplayName)
                    .HasColumnName("RScriptDisplayName")
                    .HasMaxLength(256);

                entity.Property(e => e.RscriptFileName)
                    .IsRequired()
                    .HasColumnName("RScriptFileName")
                    .HasMaxLength(2048)
                    .HasDefaultValueSql("('script.r')");

                entity.Property(e => e.RscriptFolderName)
                    .IsRequired()
                    .HasColumnName("RScriptFolderName")
                    .HasMaxLength(2048)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.RscriptModelName)
                    .HasColumnName("RScriptModelName")
                    .HasMaxLength(256);

                entity.Property(e => e.RscriptModelRole)
                    .HasColumnName("RScriptModelRole")
                    .HasMaxLength(256);

                entity.Property(e => e.RscriptResultsDefinition).HasColumnName("RScriptResultsDefinition");
            });

            modelBuilder.Entity<Stringmap>(entity =>
            {
                entity.ToTable("stringmap", "dynamics");

                entity.HasIndex(e => e.Displayorder)
                    .HasName("Idx_displayorder");

                entity.HasIndex(e => e.Modifiedon)
                    .HasName("Idx_stringmap_modifiedon");

                entity.HasIndex(e => new { e.Attributename, e.Objecttypecode })
                    .HasName("Idx_attributename_objecttypecode");

                entity.Property(e => e.Stringmapid)
                    .HasColumnName("stringmapid")
                    .ValueGeneratedNever();

                entity.Property(e => e.Attributename)
                    .HasColumnName("attributename")
                    .HasMaxLength(1000);

                entity.Property(e => e.Attributevalue).HasColumnName("attributevalue");

                entity.Property(e => e.Displayorder).HasColumnName("displayorder");

                entity.Property(e => e.Langid).HasColumnName("langid");

                entity.Property(e => e.Modifiedon)
                    .HasColumnName("modifiedon")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.Objecttypecode)
                    .HasColumnName("objecttypecode")
                    .HasMaxLength(1000);

                entity.Property(e => e.Organizationid).HasColumnName("organizationid");

                entity.Property(e => e.Value)
                    .HasColumnName("value")
                    .HasMaxLength(4000);

                entity.Property(e => e.Versionnumber).HasColumnName("versionnumber");
            });

            modelBuilder.Entity<Systemuser>(entity =>
            {
                entity.ToTable("systemuser", "dynamics");

                entity.HasIndex(e => e.Modifiedon)
                    .HasName("Idx_systemuser_modifiedon");

                entity.Property(e => e.Systemuserid)
                    .HasColumnName("systemuserid")
                    .ValueGeneratedNever();

                entity.Property(e => e.Accessmode).HasColumnName("accessmode");

                entity.Property(e => e.Address1Addressid).HasColumnName("address1_addressid");

                entity.Property(e => e.Address1Addresstypecode).HasColumnName("address1_addresstypecode");

                entity.Property(e => e.Address1City).HasColumnName("address1_city");

                entity.Property(e => e.Address1Composite).HasColumnName("address1_composite");

                entity.Property(e => e.Address1Country).HasColumnName("address1_country");

                entity.Property(e => e.Address1County).HasColumnName("address1_county");

                entity.Property(e => e.Address1Fax).HasColumnName("address1_fax");

                entity.Property(e => e.Address1Latitude).HasColumnName("address1_latitude");

                entity.Property(e => e.Address1Line1).HasColumnName("address1_line1");

                entity.Property(e => e.Address1Line2).HasColumnName("address1_line2");

                entity.Property(e => e.Address1Line3).HasColumnName("address1_line3");

                entity.Property(e => e.Address1Longitude).HasColumnName("address1_longitude");

                entity.Property(e => e.Address1Name).HasColumnName("address1_name");

                entity.Property(e => e.Address1Postalcode).HasColumnName("address1_postalcode");

                entity.Property(e => e.Address1Postofficebox).HasColumnName("address1_postofficebox");

                entity.Property(e => e.Address1Shippingmethodcode).HasColumnName("address1_shippingmethodcode");

                entity.Property(e => e.Address1Stateorprovince).HasColumnName("address1_stateorprovince");

                entity.Property(e => e.Address1Telephone1).HasColumnName("address1_telephone1");

                entity.Property(e => e.Address1Telephone2).HasColumnName("address1_telephone2");

                entity.Property(e => e.Address1Telephone3).HasColumnName("address1_telephone3");

                entity.Property(e => e.Address1Upszone).HasColumnName("address1_upszone");

                entity.Property(e => e.Address1Utcoffset).HasColumnName("address1_utcoffset");

                entity.Property(e => e.Address2Addressid).HasColumnName("address2_addressid");

                entity.Property(e => e.Address2Addresstypecode).HasColumnName("address2_addresstypecode");

                entity.Property(e => e.Address2City).HasColumnName("address2_city");

                entity.Property(e => e.Address2Composite).HasColumnName("address2_composite");

                entity.Property(e => e.Address2Country).HasColumnName("address2_country");

                entity.Property(e => e.Address2County).HasColumnName("address2_county");

                entity.Property(e => e.Address2Fax).HasColumnName("address2_fax");

                entity.Property(e => e.Address2Latitude).HasColumnName("address2_latitude");

                entity.Property(e => e.Address2Line1).HasColumnName("address2_line1");

                entity.Property(e => e.Address2Line2).HasColumnName("address2_line2");

                entity.Property(e => e.Address2Line3).HasColumnName("address2_line3");

                entity.Property(e => e.Address2Longitude).HasColumnName("address2_longitude");

                entity.Property(e => e.Address2Name).HasColumnName("address2_name");

                entity.Property(e => e.Address2Postalcode).HasColumnName("address2_postalcode");

                entity.Property(e => e.Address2Postofficebox).HasColumnName("address2_postofficebox");

                entity.Property(e => e.Address2Shippingmethodcode).HasColumnName("address2_shippingmethodcode");

                entity.Property(e => e.Address2Stateorprovince).HasColumnName("address2_stateorprovince");

                entity.Property(e => e.Address2Telephone1).HasColumnName("address2_telephone1");

                entity.Property(e => e.Address2Telephone2).HasColumnName("address2_telephone2");

                entity.Property(e => e.Address2Telephone3).HasColumnName("address2_telephone3");

                entity.Property(e => e.Address2Upszone).HasColumnName("address2_upszone");

                entity.Property(e => e.Address2Utcoffset).HasColumnName("address2_utcoffset");

                entity.Property(e => e.Applicationid).HasColumnName("applicationid");

                entity.Property(e => e.Applicationiduri).HasColumnName("applicationiduri");

                entity.Property(e => e.Azureactivedirectoryobjectid).HasColumnName("azureactivedirectoryobjectid");

                entity.Property(e => e.BusinessunitidValue).HasColumnName("_businessunitid_value");

                entity.Property(e => e.CalendaridValue).HasColumnName("_calendarid_value");

                entity.Property(e => e.Caltype).HasColumnName("caltype");

                entity.Property(e => e.CreatedbyValue).HasColumnName("_createdby_value");

                entity.Property(e => e.Createdon).HasColumnName("createdon");

                entity.Property(e => e.CreatedonbehalfbyValue).HasColumnName("_createdonbehalfby_value");

                entity.Property(e => e.Defaultfilterspopulated).HasColumnName("defaultfilterspopulated");

                entity.Property(e => e.DefaultmailboxValue).HasColumnName("_defaultmailbox_value");

                entity.Property(e => e.Defaultodbfoldername).HasColumnName("defaultodbfoldername");

                entity.Property(e => e.Disabledreason).HasColumnName("disabledreason");

                entity.Property(e => e.Displayinserviceviews).HasColumnName("displayinserviceviews");

                entity.Property(e => e.Domainname).HasColumnName("domainname");

                entity.Property(e => e.Emailrouteraccessapproval).HasColumnName("emailrouteraccessapproval");

                entity.Property(e => e.Employeeid).HasColumnName("employeeid");

                entity.Property(e => e.EntityimageTimestamp).HasColumnName("entityimage_timestamp");

                entity.Property(e => e.EntityimageUrl).HasColumnName("entityimage_url");

                entity.Property(e => e.Entityimageid).HasColumnName("entityimageid");

                entity.Property(e => e.Exchangerate)
                    .HasColumnName("exchangerate")
                    .HasColumnType("money");

                entity.Property(e => e.Firstname).HasColumnName("firstname");

                entity.Property(e => e.Fullname).HasColumnName("fullname");

                entity.Property(e => e.Governmentid).HasColumnName("governmentid");

                entity.Property(e => e.Homephone).HasColumnName("homephone");

                entity.Property(e => e.Identityid).HasColumnName("identityid");

                entity.Property(e => e.Importsequencenumber).HasColumnName("importsequencenumber");

                entity.Property(e => e.Incomingemaildeliverymethod).HasColumnName("incomingemaildeliverymethod");

                entity.Property(e => e.Internalemailaddress).HasColumnName("internalemailaddress");

                entity.Property(e => e.Invitestatuscode).HasColumnName("invitestatuscode");

                entity.Property(e => e.Isdisabled).HasColumnName("isdisabled");

                entity.Property(e => e.Isemailaddressapprovedbyo365admin).HasColumnName("isemailaddressapprovedbyo365admin");

                entity.Property(e => e.Isintegrationuser).HasColumnName("isintegrationuser");

                entity.Property(e => e.Islicensed).HasColumnName("islicensed");

                entity.Property(e => e.Issyncwithdirectory).HasColumnName("issyncwithdirectory");

                entity.Property(e => e.Jobtitle).HasColumnName("jobtitle");

                entity.Property(e => e.Lastname).HasColumnName("lastname");

                entity.Property(e => e.Middlename).HasColumnName("middlename");

                entity.Property(e => e.Mobilealertemail).HasColumnName("mobilealertemail");

                entity.Property(e => e.MobileofflineprofileidValue).HasColumnName("_mobileofflineprofileid_value");

                entity.Property(e => e.Mobilephone).HasColumnName("mobilephone");

                entity.Property(e => e.ModifiedbyValue).HasColumnName("_modifiedby_value");

                entity.Property(e => e.Modifiedon).HasColumnName("modifiedon");

                entity.Property(e => e.ModifiedonbehalfbyValue).HasColumnName("_modifiedonbehalfby_value");

                entity.Property(e => e.MsdynGdproptout).HasColumnName("msdyn_gdproptout");

                entity.Property(e => e.Nickname).HasColumnName("nickname");

                entity.Property(e => e.Organizationid).HasColumnName("organizationid");

                entity.Property(e => e.Outgoingemaildeliverymethod).HasColumnName("outgoingemaildeliverymethod");

                entity.Property(e => e.Overriddencreatedon).HasColumnName("overriddencreatedon");

                entity.Property(e => e.ParentsystemuseridValue).HasColumnName("_parentsystemuserid_value");

                entity.Property(e => e.Passporthi).HasColumnName("passporthi");

                entity.Property(e => e.Passportlo).HasColumnName("passportlo");

                entity.Property(e => e.Personalemailaddress).HasColumnName("personalemailaddress");

                entity.Property(e => e.Photourl).HasColumnName("photourl");

                entity.Property(e => e.PositionidValue).HasColumnName("_positionid_value");

                entity.Property(e => e.Preferredaddresscode).HasColumnName("preferredaddresscode");

                entity.Property(e => e.Preferredemailcode).HasColumnName("preferredemailcode");

                entity.Property(e => e.Preferredphonecode).HasColumnName("preferredphonecode");

                entity.Property(e => e.Processid).HasColumnName("processid");

                entity.Property(e => e.PtasLegacyid).HasColumnName("ptas_legacyid");

                entity.Property(e => e.QueueidValue).HasColumnName("_queueid_value");

                entity.Property(e => e.Salutation).HasColumnName("salutation");

                entity.Property(e => e.Setupuser).HasColumnName("setupuser");

                entity.Property(e => e.Sharepointemailaddress).HasColumnName("sharepointemailaddress");

                entity.Property(e => e.SiteidValue).HasColumnName("_siteid_value");

                entity.Property(e => e.Skills).HasColumnName("skills");

                entity.Property(e => e.Stageid).HasColumnName("stageid");

                entity.Property(e => e.TerritoryidValue).HasColumnName("_territoryid_value");

                entity.Property(e => e.Timezoneruleversionnumber).HasColumnName("timezoneruleversionnumber");

                entity.Property(e => e.Title).HasColumnName("title");

                entity.Property(e => e.TransactioncurrencyidValue).HasColumnName("_transactioncurrencyid_value");

                entity.Property(e => e.Traversedpath).HasColumnName("traversedpath");

                entity.Property(e => e.Userlicensetype).HasColumnName("userlicensetype");

                entity.Property(e => e.Userpuid).HasColumnName("userpuid");

                entity.Property(e => e.Utcconversiontimezonecode).HasColumnName("utcconversiontimezonecode");

                entity.Property(e => e.Versionnumber).HasColumnName("versionnumber");

                entity.Property(e => e.Windowsliveid).HasColumnName("windowsliveid");

                entity.Property(e => e.Yammeremailaddress).HasColumnName("yammeremailaddress");

                entity.Property(e => e.Yammeruserid).HasColumnName("yammeruserid");

                entity.Property(e => e.Yomifirstname).HasColumnName("yomifirstname");

                entity.Property(e => e.Yomifullname).HasColumnName("yomifullname");

                entity.Property(e => e.Yomilastname).HasColumnName("yomilastname");

                entity.Property(e => e.Yomimiddlename).HasColumnName("yomimiddlename");
            });

            modelBuilder.Entity<Systemuserroles>(entity =>
            {
                entity.HasKey(e => e.Systemuserroleid);

                entity.ToTable("systemuserroles", "dynamics");

                entity.HasIndex(e => e.Modifiedon)
                    .HasName("Idx_systemuserroles_modifiedon");

                entity.Property(e => e.Systemuserroleid)
                    .HasColumnName("systemuserroleid")
                    .ValueGeneratedNever();

                entity.Property(e => e.Modifiedon)
                    .HasColumnName("modifiedon")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.Roleid).HasColumnName("roleid");

                entity.Property(e => e.Systemuserid).HasColumnName("systemuserid");

                entity.Property(e => e.Versionnumber).HasColumnName("versionnumber");
            });

            modelBuilder.Entity<Team>(entity =>
            {
                entity.ToTable("team", "dynamics");

                entity.HasIndex(e => e.Modifiedon)
                    .HasName("Idx_team_modifiedon");

                entity.Property(e => e.Teamid)
                    .HasColumnName("teamid")
                    .ValueGeneratedNever();

                entity.Property(e => e.AdministratoridValue).HasColumnName("_administratorid_value");

                entity.Property(e => e.Azureactivedirectoryobjectid).HasColumnName("azureactivedirectoryobjectid");

                entity.Property(e => e.BusinessunitidValue).HasColumnName("_businessunitid_value");

                entity.Property(e => e.CreatedbyValue).HasColumnName("_createdby_value");

                entity.Property(e => e.Createdon).HasColumnName("createdon");

                entity.Property(e => e.CreatedonbehalfbyValue).HasColumnName("_createdonbehalfby_value");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.Emailaddress).HasColumnName("emailaddress");

                entity.Property(e => e.Exchangerate)
                    .HasColumnName("exchangerate")
                    .HasColumnType("money");

                entity.Property(e => e.Importsequencenumber).HasColumnName("importsequencenumber");

                entity.Property(e => e.Isdefault).HasColumnName("isdefault");

                entity.Property(e => e.Membershiptype).HasColumnName("membershiptype");

                entity.Property(e => e.ModifiedbyValue).HasColumnName("_modifiedby_value");

                entity.Property(e => e.Modifiedon).HasColumnName("modifiedon");

                entity.Property(e => e.ModifiedonbehalfbyValue).HasColumnName("_modifiedonbehalfby_value");

                entity.Property(e => e.Name).HasColumnName("name");

                entity.Property(e => e.Organizationid).HasColumnName("organizationid");

                entity.Property(e => e.Overriddencreatedon).HasColumnName("overriddencreatedon");

                entity.Property(e => e.Processid).HasColumnName("processid");

                entity.Property(e => e.QueueidValue).HasColumnName("_queueid_value");

                entity.Property(e => e.RegardingobjectidValue).HasColumnName("_regardingobjectid_value");

                entity.Property(e => e.Stageid).HasColumnName("stageid");

                entity.Property(e => e.Systemmanaged).HasColumnName("systemmanaged");

                entity.Property(e => e.TeamtemplateidValue).HasColumnName("_teamtemplateid_value");

                entity.Property(e => e.Teamtype).HasColumnName("teamtype");

                entity.Property(e => e.TransactioncurrencyidValue).HasColumnName("_transactioncurrencyid_value");

                entity.Property(e => e.Traversedpath).HasColumnName("traversedpath");

                entity.Property(e => e.Versionnumber).HasColumnName("versionnumber");

                entity.Property(e => e.Yominame).HasColumnName("yominame");
            });

            modelBuilder.Entity<Teammembership>(entity =>
            {
                entity.ToTable("teammembership", "dynamics");

                entity.HasIndex(e => e.Modifiedon)
                    .HasName("Idx_teammembership_modifiedon");

                entity.Property(e => e.Teammembershipid)
                    .HasColumnName("teammembershipid")
                    .ValueGeneratedNever();

                entity.Property(e => e.Modifiedon)
                    .HasColumnName("modifiedon")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.Systemuserid).HasColumnName("systemuserid");

                entity.Property(e => e.Teamid).HasColumnName("teamid");

                entity.Property(e => e.Versionnumber).HasColumnName("versionnumber");
            });

            modelBuilder.Entity<Teamprofiles>(entity =>
            {
                entity.HasKey(e => e.Teamprofileid);

                entity.ToTable("teamprofiles", "dynamics");

                entity.HasIndex(e => e.Modifiedon)
                    .HasName("Idx_teamprofiles_modifiedon");

                entity.Property(e => e.Teamprofileid)
                    .HasColumnName("teamprofileid")
                    .ValueGeneratedNever();

                entity.Property(e => e.Fieldsecurityprofileid).HasColumnName("fieldsecurityprofileid");

                entity.Property(e => e.Modifiedon)
                    .HasColumnName("modifiedon")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.Teamid).HasColumnName("teamid");

                entity.Property(e => e.Versionnumber).HasColumnName("versionnumber");
            });

            modelBuilder.Entity<Teamroles>(entity =>
            {
                entity.HasKey(e => e.Teamroleid);

                entity.ToTable("teamroles", "dynamics");

                entity.HasIndex(e => e.Modifiedon)
                    .HasName("Idx_teamroles_modifiedon");

                entity.Property(e => e.Teamroleid)
                    .HasColumnName("teamroleid")
                    .ValueGeneratedNever();

                entity.Property(e => e.Modifiedon)
                    .HasColumnName("modifiedon")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.Roleid).HasColumnName("roleid");

                entity.Property(e => e.Teamid).HasColumnName("teamid");

                entity.Property(e => e.Versionnumber).HasColumnName("versionnumber");
            });

            modelBuilder.Entity<UserDataStoreItem>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.StoreType, e.OwnerType, e.OwnerObjectId, e.ItemName })
                    .HasName("PK_UserDataStoreItem_UserDataStoreItemIdUserIdStoreTypeOwnerTypeOwnerObjectId");

                entity.HasIndex(e => new { e.UserId, e.StoreType, e.OwnerType, e.OwnerObjectId })
                    .HasName("IX_UserDataStoreItem_Secondary");

                entity.Property(e => e.StoreType)
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entity.Property(e => e.OwnerType)
                    .HasMaxLength(256)
                    .HasDefaultValueSql("('NoOwnerType')");

                entity.Property(e => e.OwnerObjectId)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ItemName)
                    .HasMaxLength(64)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UserDataStoreItemId).ValueGeneratedOnAddOrUpdate();

                entity.HasOne(d => d.OwnerTypeNavigation)
                    .WithMany(p => p.UserDataStoreItem)
                    .HasForeignKey(d => d.OwnerType)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserDataStoreItem_ToOwnerType_OwnerType");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserDataStoreItem)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserDataStoreItem_ToSystemUser_UserId")
                    .IsRequired(false);
            });

            modelBuilder.Entity<UserJobNotification>(entity =>
            {
                entity.HasKey(e => e.JobNotificationId);

                entity.HasIndex(e => e.JobId);

                entity.HasIndex(e => new { e.UserId, e.Dismissed, e.CreatedTimestamp })
                    .HasName("IX_UserJobNotification_Composite");

                entity.Property(e => e.CreatedTimestamp)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.JobNotificationText).IsRequired();

                entity.Property(e => e.JobNotificationType)
                    .IsRequired()
                    .HasMaxLength(16);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserJobNotification)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserJobNotification_ToSystemUser_UserId");
            });

            modelBuilder.Entity<UserProject>(entity =>
            {
                entity.ToTable("UserProject", "cus");

                entity.HasIndex(e => e.CreatedTimestamp);

                entity.HasIndex(e => e.ProjectTypeId);

                entity.HasIndex(e => e.RootVersionUserProjectId);

                entity.HasIndex(e => e.UserId);

                entity.HasIndex(e => e.VersionNumber);

                entity.Property(e => e.AssessmentDateFrom).HasColumnType("datetime");

                entity.Property(e => e.AssessmentDateTo).HasColumnType("datetime");

                entity.Property(e => e.Comments).HasDefaultValueSql("('')");

                entity.Property(e => e.CreatedTimestamp)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.LastModifiedTimestamp)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ProjectName)
                    .HasMaxLength(256)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.SelectedAreas).HasDefaultValueSql("('')");

                entity.Property(e => e.VersionNumber).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.UserProjectCreatedByNavigation)
                    .HasForeignKey(d => d.CreatedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserProject_ToSystemUser_CreatedBy")
                    .IsRequired(false);

                entity.HasOne(d => d.LastModifiedByNavigation)
                    .WithMany(p => p.UserProjectLastModifiedByNavigation)
                    .HasForeignKey(d => d.LastModifiedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserProject_ToSystemUser_LastModifiedBy")
                    .IsRequired(false);

                entity.Property(e => e.VersionType)
                    .IsRequired()
                    .HasMaxLength(256)
                    .HasDefaultValueSql("('Draft')");

                entity.HasOne(d => d.ProjectType)
                    .WithMany(p => p.UserProject)
                    .HasForeignKey(d => d.ProjectTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserProject_ToProjectType");

                entity.HasOne(d => d.RootVersionUserProject)
                    .WithMany(p => p.InverseRootVersionUserProject)
                    .HasForeignKey(d => d.RootVersionUserProjectId)
                    .HasConstraintName("FK_UserProject_ToUserProject");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserProjectUser)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserProject_ToSystemUser_UserId")
                    .IsRequired(false);

                entity.HasOne(d => d.VersionTypeNavigation)
                    .WithMany(p => p.UserProject)
                    .HasForeignKey(d => d.VersionType)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserProject_ProjectVersionType");
            });

            modelBuilder.Entity<UserProjectDataset>(entity =>
            {
                entity.HasKey(e => new { e.UserProjectId, e.DatasetId })
                    .HasName("PK_UserProject_Dataset_UserProjectIdDatasetId");

                entity.ToTable("UserProject_Dataset", "cus");

                entity.Property(e => e.DatasetRole)
                    .IsRequired()
                    .HasMaxLength(256)
                    .HasDefaultValueSql("(N'Population')");

                entity.HasOne(d => d.Dataset)
                    .WithMany(p => p.UserProjectDataset)
                    .HasForeignKey(d => d.DatasetId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserProject_Dataset_ToDataset");

                entity.HasOne(d => d.UserProject)
                    .WithMany(p => p.UserProjectDataset)
                    .HasForeignKey(d => d.UserProjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserProject_Dataset_ToUserProject");
            });

            this.OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}