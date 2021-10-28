namespace PTASODataLibrary.PtasDbDataProvider.EntityFramework
{
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using PTASODataLibrary.PtasDbDataProvider.PtasHistoricalModel;
    using PTASServicesCommon.TokenProvider;

    /// <summary>
    /// Historical DB Context.
    /// </summary>
    /// <seealso cref="Microsoft.EntityFrameworkCore.DbContext" />
    [ExcludeFromCodeCoverage]
    public partial class PtasHistoricalDbContext : DbContext
    {
        private const string ConnectionStringPasswordSection = "password";

        /// <summary>
        /// Initializes a new instance of the <see cref="PtasHistoricalDbContext" /> class.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <param name="tokenProvider">The token provider.</param>
        /// <exception cref="System.ArgumentNullException">tokenProvider.</exception>
        public PtasHistoricalDbContext(DbContextOptions<PtasHistoricalDbContext> options, IServiceTokenProvider tokenProvider)
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
                if (!conn.ConnectionString.Contains(PtasHistoricalDbContext.ConnectionStringPasswordSection, System.StringComparison.OrdinalIgnoreCase))
                {
                    string token = Task.Run(async () =>
                    {
                        return await tokenProvider.GetAccessTokenAsync(AzureTokenProvider.DbTokenEndpoint, AzureTokenProvider.TenantUrl);
                    }).Result;

                    conn.AccessToken = token;
                }
            }
        }

#pragma warning disable SA1600 // Elements should be documented
#pragma warning disable SA1300 // Element should begin with upper-case letter
#pragma warning disable SA1516 // Elements should be separated by blank line

        public virtual DbSet<AppraisalHistLastTransaction> AppraisalHistLastTransaction { get; set; }
        public virtual DbSet<PtasAppraisalHistory> PtasAppraisalHistory { get; set; }
        public virtual DbSet<PtasAppraisalHistoryTest> PtasAppraisalHistoryTest { get; set; }
        public virtual DbSet<PtasChangehistory> PtasChangehistory { get; set; }
        public virtual DbSet<PtasEstimateHistory> PtasEstimateHistory { get; set; }
        public virtual DbSet<PtasEstimateHistoryTest> PtasEstimateHistoryTest { get; set; }
        public virtual DbSet<PtasFloatingHomeReplacementCostRate> PtasFloatingHomeReplacementCostRate { get; set; }
        public virtual DbSet<PtasFloatingHomeSlipValues> PtasFloatingHomeSlipValues { get; set; }
        public virtual DbSet<PtasFloatingHomeValuation> PtasFloatingHomeValuation { get; set; }
        public virtual DbSet<PtasIncomemodel> PtasIncomemodel { get; set; }
        public virtual DbSet<PtasIncomemodeldetail> PtasIncomemodeldetail { get; set; }
        public virtual DbSet<PtasIncomerates> PtasIncomerates { get; set; }
        public virtual DbSet<PtasIncomevaluation> PtasIncomevaluation { get; set; }
        public virtual DbSet<PtasTaxRollHistory> PtasTaxRollHistory { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppraisalHistLastTransaction>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("AppraisalHistLastTransaction", "ptas");

                entity.Property(e => e.ApprMethod).HasMaxLength(100);

                entity.Property(e => e.AppraisalHistoryGuid).HasColumnName("appraisalHistoryGuid");

                entity.Property(e => e.AppraisedDate)
                    .HasColumnName("appraisedDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.AppraiserGuid).HasColumnName("appraiserGuid");

                entity.Property(e => e.AppraiserName)
                    .HasColumnName("appraiserName")
                    .HasMaxLength(100);

                entity.Property(e => e.CreatedByName)
                    .HasColumnName("createdByName")
                    .HasMaxLength(100);

                entity.Property(e => e.CreatedOn)
                    .HasColumnName("createdOn")
                    .HasColumnType("datetime");

                entity.Property(e => e.HasNote).HasColumnName("hasNote");

                entity.Property(e => e.ImageCode).HasMaxLength(1);

                entity.Property(e => e.ImpsValue).HasColumnName("impsValue");

                entity.Property(e => e.InterfaceFlag).HasColumnName("interfaceFlag");

                entity.Property(e => e.InterfaceFlagDesc).HasMaxLength(50);

                entity.Property(e => e.LandId).HasColumnName("landId");

                entity.Property(e => e.LandIdName).HasMaxLength(100);

                entity.Property(e => e.LandValue).HasColumnName("landValue");

                entity.Property(e => e.ModifiedByName)
                    .HasColumnName("modifiedByName")
                    .HasMaxLength(100);

                entity.Property(e => e.ModifiedOn)
                    .HasColumnName("modifiedOn")
                    .HasColumnType("datetime");

                entity.Property(e => e.NewConstrValue).HasColumnName("newConstrValue");

                entity.Property(e => e.Note)
                    .HasColumnName("note")
                    .HasMaxLength(500);

                entity.Property(e => e.ParcelGuid).HasColumnName("parcelGuid");

                entity.Property(e => e.ParcelIdName)
                    .HasColumnName("parcelIdName")
                    .HasMaxLength(100);

                entity.Property(e => e.PercentChange).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.PostDate)
                    .HasColumnName("postDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.RealPropId).HasColumnName("realPropId");

                entity.Property(e => e.RecName)
                    .HasColumnName("recName")
                    .HasMaxLength(100);

                entity.Property(e => e.RevalOrMaint)
                    .HasColumnName("revalOrMaint")
                    .HasMaxLength(100);

                entity.Property(e => e.RollYear).HasMaxLength(100);

                entity.Property(e => e.SplitCode).HasColumnName("splitCode");

                entity.Property(e => e.TaxAccountGuid).HasColumnName("taxAccountGuid");

                entity.Property(e => e.TaxAccountIdName)
                    .HasColumnName("taxAccountIdName")
                    .HasMaxLength(100);

                entity.Property(e => e.TaxYearGuid).HasColumnName("taxYearGuid");

                entity.Property(e => e.TaxYearIdName)
                    .HasColumnName("taxYearIdName")
                    .HasMaxLength(100);

                entity.Property(e => e.TotalValue).HasColumnName("totalValue");

                entity.Property(e => e.TransactionBy)
                    .HasColumnName("transactionBy")
                    .HasMaxLength(100);

                entity.Property(e => e.TransactionDate)
                    .HasColumnName("transactionDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.ValuationReason)
                    .HasColumnName("valuationReason")
                    .HasMaxLength(100);

                entity.Property(e => e.ValuationReasonId).HasColumnName("valuationReasonId");
            });

            modelBuilder.Entity<PtasAppraisalHistory>(entity =>
            {
                entity.HasKey(e => e.AppraisalHistoryGuid)
                    .HasName("PK_apprHistGuid3")
                    .IsClustered(false);

                entity.ToTable("ptas_appraisalHistory", "ptas");

                entity.HasIndex(e => e.RecType)
                    .HasName("idx_recType");

                entity.HasIndex(e => e.TaxYearIdName)
                    .HasName("IX_taxyearidname");

                entity.HasIndex(e => new { e.AppraisedDate, e.ParcelGuid })
                    .HasName("IX_parcelguid");

                entity.HasIndex(e => new { e.ParcelIdName, e.RollYear })
                    .HasName("idx_ParcelNum_Year")
                    .IsClustered();

                entity.HasIndex(e => new { e.ParcelGuid, e.RealPropId, e.TaxYearIdName })
                    .HasName("IX_appraisalhistory_realpropid_taxyearidname");

                entity.HasIndex(e => new { e.ApprMethod, e.ApprMethodId, e.ValuationReason, e.ValuationReasonId, e.RollYear })
                    .HasName("IX_RollYear");

                entity.HasIndex(e => new { e.AppraisedDate, e.LandId, e.ParcelGuid, e.InterfaceFlag, e.RevalOrMaint })
                    .HasName("IX_interfaceFlag_revalOrMaint");

                entity.HasIndex(e => new { e.RollYear, e.TaxYearIdName, e.TransactionDate, e.AppraisedDate, e.ParcelGuid })
                    .HasName("idx_parcelGuid");

                entity.HasIndex(e => new { e.ParcelIdName, e.ParcelGuid, e.RollYear, e.TaxYearIdName, e.TransactionDate, e.LandId })
                    .HasName("idx_landId");

                entity.HasIndex(e => new { e.AppraisedDate, e.ApprMethod, e.ApprMethodId, e.ValuationReason, e.LandValue, e.ImpsValue, e.InterfaceFlag, e.NewConstrValue, e.ParcelGuid, e.RollYear, e.RevalOrMaint })
                    .HasName("IX_parcelguid_RollYear_revalormaint");

                entity.HasIndex(e => new { e.AppraisalHistoryGuid, e.AppraisedDate, e.ApprMethod, e.CreatedOn, e.ImpsValue, e.LandValue, e.NewConstrValue, e.RevalOrMaint, e.TotalValue, e.RollYear, e.InterfaceFlag, e.ParcelGuid })
                    .HasName("idx_RollYear_interfaceFlag_parcelGuid_includes");

                entity.HasIndex(e => new { e.LandValue, e.TotalValue, e.ApprMethod, e.CreatedOn, e.NewConstrValue, e.AppraiserName, e.InterfaceFlag, e.ValuationReason, e.ParcelGuid, e.TaxYearIdName, e.RevalOrMaint, e.ImpsValue })
                    .HasName("Ix_parcelGuid_Taxyear_revalormaint_impvalue");

                entity.HasIndex(e => new { e.AppraisedDate, e.ApprMethod, e.ImpsValue, e.LandValue, e.NewConstrValue, e.ParcelGuid, e.RevalOrMaint, e.TotalValue, e.ApprMethodId, e.ValuationReason, e.ValuationReasonId, e.RollYear, e.InterfaceFlag })
                    .HasName("idx_rollyear_interfaceflag_includes2");

                entity.Property(e => e.AppraisalHistoryGuid)
                    .HasColumnName("appraisalHistoryGuid")
                    .ValueGeneratedNever();

                entity.Property(e => e.ApprMethod).HasMaxLength(100);

                entity.Property(e => e.AppraisedDate)
                    .HasColumnName("appraisedDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.AppraiserGuid).HasColumnName("appraiserGuid");

                entity.Property(e => e.AppraiserName)
                    .HasColumnName("appraiserName")
                    .HasMaxLength(100);

                entity.Property(e => e.CreatedByName)
                    .HasColumnName("createdByName")
                    .HasMaxLength(100);

                entity.Property(e => e.CreatedOn)
                    .HasColumnName("createdOn")
                    .HasColumnType("datetime");

                entity.Property(e => e.HasNote).HasColumnName("hasNote");

                entity.Property(e => e.ImageCode).HasMaxLength(1);

                entity.Property(e => e.ImpsValue).HasColumnName("impsValue");

                entity.Property(e => e.InterfaceFlag).HasColumnName("interfaceFlag");

                entity.Property(e => e.InterfaceFlagDesc).HasMaxLength(50);

                entity.Property(e => e.LandId).HasColumnName("landId");

                entity.Property(e => e.LandIdName).HasMaxLength(100);

                entity.Property(e => e.LandValue).HasColumnName("landValue");

                entity.Property(e => e.ModifiedByName)
                    .HasColumnName("modifiedByName")
                    .HasMaxLength(100);

                entity.Property(e => e.ModifiedOn)
                    .HasColumnName("modifiedOn")
                    .HasColumnType("datetime");

                entity.Property(e => e.NewConstrValue).HasColumnName("newConstrValue");

                entity.Property(e => e.Note)
                    .HasColumnName("note")
                    .HasMaxLength(500);

                entity.Property(e => e.ParcelGuid).HasColumnName("parcelGuid");

                entity.Property(e => e.ParcelIdName)
                    .HasColumnName("parcelIdName")
                    .HasMaxLength(100);

                entity.Property(e => e.PercentChange).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.PostDate)
                    .HasColumnName("postDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.RealPropId).HasColumnName("realPropId");

                entity.Property(e => e.RecName)
                    .HasColumnName("recName")
                    .HasMaxLength(100);

                entity.Property(e => e.RecType)
                    .HasColumnName("recType")
                    .HasMaxLength(1)
                    .IsFixedLength();

                entity.Property(e => e.RevalOrMaint)
                    .HasColumnName("revalOrMaint")
                    .HasMaxLength(100);

                entity.Property(e => e.RollYear).HasMaxLength(100);

                entity.Property(e => e.SplitCode).HasColumnName("splitCode");

                entity.Property(e => e.TaxAccountGuid).HasColumnName("taxAccountGuid");

                entity.Property(e => e.TaxAccountIdName)
                    .HasColumnName("taxAccountIdName")
                    .HasMaxLength(100);

                entity.Property(e => e.TaxYearGuid).HasColumnName("taxYearGuid");

                entity.Property(e => e.TaxYearIdName)
                    .HasColumnName("taxYearIdName")
                    .HasMaxLength(100);

                entity.Property(e => e.TotalValue).HasColumnName("totalValue");

                entity.Property(e => e.TransactionBy)
                    .HasColumnName("transactionBy")
                    .HasMaxLength(100);

                entity.Property(e => e.TransactionDate)
                    .HasColumnName("transactionDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.UnitGuid).HasColumnName("unitGuid");

                entity.Property(e => e.UnitIdName)
                    .HasColumnName("unitIdName")
                    .HasMaxLength(100);

                entity.Property(e => e.ValuationReason)
                    .HasColumnName("valuationReason")
                    .HasMaxLength(100);

                entity.Property(e => e.ValuationReasonId).HasColumnName("valuationReasonId");
            });

            modelBuilder.Entity<PtasAppraisalHistoryTest>(entity =>
            {
                entity.HasKey(e => e.AppraisalHistoryGuid)
                    .HasName("PK_apprHistGuid2Temp")
                    .IsClustered(false);

                entity.ToTable("ptas_appraisalHistory_TEST", "ptas");

                entity.Property(e => e.AppraisalHistoryGuid)
                    .HasColumnName("appraisalHistoryGuid")
                    .ValueGeneratedNever();

                entity.Property(e => e.ApprMethod).HasMaxLength(100);

                entity.Property(e => e.AppraisedDate)
                    .HasColumnName("appraisedDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.AppraiserGuid).HasColumnName("appraiserGuid");

                entity.Property(e => e.AppraiserName)
                    .HasColumnName("appraiserName")
                    .HasMaxLength(100);

                entity.Property(e => e.CreatedByName)
                    .HasColumnName("createdByName")
                    .HasMaxLength(100);

                entity.Property(e => e.CreatedOn)
                    .HasColumnName("createdOn")
                    .HasColumnType("datetime");

                entity.Property(e => e.HasNote).HasColumnName("hasNote");

                entity.Property(e => e.ImageCode).HasMaxLength(1);

                entity.Property(e => e.ImpsValue).HasColumnName("impsValue");

                entity.Property(e => e.InterfaceFlag).HasColumnName("interfaceFlag");

                entity.Property(e => e.InterfaceFlagDesc).HasMaxLength(50);

                entity.Property(e => e.LandId).HasColumnName("landId");

                entity.Property(e => e.LandIdName).HasMaxLength(100);

                entity.Property(e => e.LandValue).HasColumnName("landValue");

                entity.Property(e => e.ModifiedByName)
                    .HasColumnName("modifiedByName")
                    .HasMaxLength(100);

                entity.Property(e => e.ModifiedOn)
                    .HasColumnName("modifiedOn")
                    .HasColumnType("datetime");

                entity.Property(e => e.NewConstrValue).HasColumnName("newConstrValue");

                entity.Property(e => e.Note)
                    .HasColumnName("note")
                    .HasMaxLength(500);

                entity.Property(e => e.ParcelGuid).HasColumnName("parcelGuid");

                entity.Property(e => e.ParcelIdName)
                    .HasColumnName("parcelIdName")
                    .HasMaxLength(100);

                entity.Property(e => e.PercentChange).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.PostDate)
                    .HasColumnName("postDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.RealPropId).HasColumnName("realPropId");

                entity.Property(e => e.RecName)
                    .HasColumnName("recName")
                    .HasMaxLength(100);

                entity.Property(e => e.RecType)
                    .HasColumnName("recType")
                    .HasMaxLength(1)
                    .IsFixedLength();

                entity.Property(e => e.RevalOrMaint)
                    .HasColumnName("revalOrMaint")
                    .HasMaxLength(100);

                entity.Property(e => e.RollYear).HasMaxLength(100);

                entity.Property(e => e.SplitCode).HasColumnName("splitCode");

                entity.Property(e => e.TaxAccountGuid).HasColumnName("taxAccountGuid");

                entity.Property(e => e.TaxAccountIdName)
                    .HasColumnName("taxAccountIdName")
                    .HasMaxLength(100);

                entity.Property(e => e.TaxYearGuid).HasColumnName("taxYearGuid");

                entity.Property(e => e.TaxYearIdName)
                    .HasColumnName("taxYearIdName")
                    .HasMaxLength(100);

                entity.Property(e => e.TotalValue).HasColumnName("totalValue");

                entity.Property(e => e.TransactionBy)
                    .HasColumnName("transactionBy")
                    .HasMaxLength(100);

                entity.Property(e => e.TransactionDate)
                    .HasColumnName("transactionDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.UnitGuid).HasColumnName("unitGuid");

                entity.Property(e => e.UnitIdName)
                    .HasColumnName("unitIdName")
                    .HasMaxLength(100);

                entity.Property(e => e.ValuationReason)
                    .HasColumnName("valuationReason")
                    .HasMaxLength(100);

                entity.Property(e => e.ValuationReasonId).HasColumnName("valuationReasonId");
            });

            modelBuilder.Entity<PtasChangehistory>(entity =>
            {
                entity.HasKey(e => e.ChngDtlGuid)
                    .HasName("PK_chngDtlGuid")
                    .IsClustered(false);

                entity.ToTable("ptas_changehistory", "ptas");

                entity.HasIndex(e => e.DetailId)
                    .HasName("idx_DetailId");

                entity.HasIndex(e => new { e.Major, e.Minor })
                    .HasName("idx_MajMin");

                entity.HasIndex(e => new { e.ParcelGuid, e.EventDate })
                    .HasName("IX_EventDate");

                entity.HasIndex(e => new { e.UpdateDate, e.EventDate, e.ParcelGuid })
                    .HasName("idx_parcelGuid");

                entity.HasIndex(e => new { e.ParcelGuid, e.EntityDispName, e.AttribDispName, e.EventDate, e.DisplayValueNew })
                    .HasName("IX_ptas_changehistory_entity_attrib_DispName");

                entity.HasIndex(e => new { e.ParcelGuid, e.Major, e.Minor, e.ChngDtlGuid, e.ChngGuid, e.ModifiedRecordName, e.ModifiedRecordPk })
                    .HasName("idx_ModRecPK");

                entity.Property(e => e.ChngDtlGuid).ValueGeneratedNever();

                entity.Property(e => e.AttribDispName)
                    .HasColumnName("attribDispName")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.AttribSchemaName)
                    .HasColumnName("attribSchemaName")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ChngId).HasColumnName("chngID");

                entity.Property(e => e.DataTypeDesc)
                    .HasColumnName("dataTypeDesc")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DetailId)
                    .HasColumnName("detailID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.DisplayValueNew)
                    .HasColumnName("displayValueNew")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.DisplayValueNewMulti)
                    .HasColumnName("displayValueNewMulti")
                    .HasColumnType("ntext");

                entity.Property(e => e.DisplayValueOriginal)
                    .HasColumnName("displayValueOriginal")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.DisplayValueOriginalMulti)
                    .HasColumnName("displayValueOriginalMulti")
                    .HasColumnType("ntext");

                entity.Property(e => e.DocId)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.EntityDispName)
                    .HasColumnName("entityDispName")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.EntitySchemaName)
                    .HasColumnName("entitySchemaName")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.EventDate).HasColumnType("smalldatetime");

                entity.Property(e => e.EventTypeDesc)
                    .HasColumnName("eventTypeDesc")
                    .HasMaxLength(25);

                entity.Property(e => e.IdvalueNew)
                    .HasColumnName("IDValueNew")
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entity.Property(e => e.IdvalueOriginal)
                    .HasColumnName("IDValueOriginal")
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entity.Property(e => e.LegacyId).HasMaxLength(4);

                entity.Property(e => e.Major)
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Minor)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.ModifiedRecordName)
                    .HasColumnName("modifiedRecordName")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedRecordNewName)
                    .HasColumnName("modifiedRecordNewName")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedRecordPk).HasColumnName("modifiedRecordPK");

                entity.Property(e => e.ParcelGuid).HasColumnName("parcelGuid");

                entity.Property(e => e.ParentRecordName)
                    .HasColumnName("parentRecordName")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ParentRecordPk).HasColumnName("parentRecordPK");

                entity.Property(e => e.PropStatus)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.PtasName)
                    .HasColumnName("ptasName")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UpdateDate).HasColumnType("smalldatetime");

                entity.Property(e => e.UpdatedBy)
                    .HasColumnName("updatedBy")
                    .HasMaxLength(150);

                entity.Property(e => e.UpdatedByGuid).HasColumnName("updatedByGuid");
            });

            modelBuilder.Entity<PtasEstimateHistory>(entity =>
            {
                entity.HasKey(e => e.EstimateHistoryGuid)
                    .HasName("PK_estHistGuid")
                    .IsClustered(false);

                entity.ToTable("ptas_estimateHistory", "ptas");

                entity.HasIndex(e => e.RecType)
                    .HasName("idx_recType");

                entity.HasIndex(e => new { e.ParcelIdName, e.RollYearName })
                    .HasName("idx_ParcelNum_Year")
                    .IsClustered();

                entity.HasIndex(e => new { e.RollYearName, e.ParcelGuid })
                    .HasName("idx_parcelGuid");

                entity.HasIndex(e => new { e.BuildingIdName, e.LandIdName, e.ParcelGuid, e.ParcelIdName, e.RollYearName, e.LandId, e.BuildingGuid })
                    .HasName("idx_landId_bldgGuid");

                entity.HasIndex(e => new { e.LandId, e.BuildingIdName, e.LandIdName, e.ParcelGuid, e.ParcelIdName, e.RollYearName, e.BuildingGuid })
                    .HasName("idx_buildingGuid");

                entity.Property(e => e.EstimateHistoryGuid)
                    .HasColumnName("estimateHistoryGuid")
                    .ValueGeneratedNever();

                entity.Property(e => e.AssessmentYearGuid).HasColumnName("assessmentYearGuid");

                entity.Property(e => e.AssessmentYearIdName)
                    .HasColumnName("assessmentYearIdName")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.BuildingGuid).HasColumnName("buildingGuid");

                entity.Property(e => e.BuildingIdName)
                    .HasColumnName("buildingIdName")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CalculationDate)
                    .HasColumnName("calculationDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.CreatedByName)
                    .HasColumnName("createdByName")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedOn)
                    .HasColumnName("createdOn")
                    .HasColumnType("datetime");

                entity.Property(e => e.EstimateSrc)
                    .HasColumnName("estimateSrc")
                    .HasMaxLength(25);

                entity.Property(e => e.EstimateType)
                    .HasColumnName("estimateType")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.EstimateTypeId).HasColumnName("estimateTypeId");

                entity.Property(e => e.ImpsValue).HasColumnName("impsValue");

                entity.Property(e => e.LandId).HasColumnName("landId");

                entity.Property(e => e.LandIdName)
                    .HasColumnName("landIdName")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.LandValue).HasColumnName("landValue");

                entity.Property(e => e.ModifiedByName)
                    .HasColumnName("modifiedByName")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedOn)
                    .HasColumnName("modifiedOn")
                    .HasColumnType("datetime");

                entity.Property(e => e.ParcelGuid).HasColumnName("parcelGuid");

                entity.Property(e => e.ParcelIdName)
                    .HasColumnName("parcelIdName")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.PpTaxAccountId).HasColumnName("ppTaxAccountId");

                entity.Property(e => e.PpTaxAccountIdName)
                    .HasColumnName("ppTaxAccountIdName")
                    .HasMaxLength(100);

                entity.Property(e => e.RecName)
                    .HasColumnName("recName")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.RecType)
                    .HasColumnName("recType")
                    .HasMaxLength(1)
                    .IsFixedLength();

                entity.Property(e => e.RollYearGuid).HasColumnName("rollYearGuid");

                entity.Property(e => e.RollYearName)
                    .HasColumnName("rollYearName")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.TaxAccountId).HasColumnName("taxAccountId");

                entity.Property(e => e.TaxAccountIdName)
                    .HasColumnName("taxAccountIdName")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.TotalValue).HasColumnName("totalValue");

                entity.Property(e => e.UnitGuid).HasColumnName("unitGuid");

                entity.Property(e => e.UnitIdName)
                    .HasColumnName("unitIdName")
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<PtasEstimateHistoryTest>(entity =>
            {
                entity.HasKey(e => e.EstimateHistoryGuid)
                    .HasName("PK_estHistGuid_TEST")
                    .IsClustered(false);

                entity.ToTable("ptas_estimateHistory_TEST", "ptas");

                entity.Property(e => e.EstimateHistoryGuid)
                    .HasColumnName("estimateHistoryGuid")
                    .ValueGeneratedNever();

                entity.Property(e => e.AssessmentYearGuid).HasColumnName("assessmentYearGuid");

                entity.Property(e => e.AssessmentYearIdName)
                    .HasColumnName("assessmentYearIdName")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.BuildingGuid).HasColumnName("buildingGuid");

                entity.Property(e => e.BuildingIdName)
                    .HasColumnName("buildingIdName")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CalculationDate)
                    .HasColumnName("calculationDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.CreatedByName)
                    .HasColumnName("createdByName")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedOn)
                    .HasColumnName("createdOn")
                    .HasColumnType("datetime");

                entity.Property(e => e.EstimateSrc)
                    .HasColumnName("estimateSrc")
                    .HasMaxLength(25);

                entity.Property(e => e.EstimateType)
                    .HasColumnName("estimateType")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.EstimateTypeId).HasColumnName("estimateTypeId");

                entity.Property(e => e.ImpsValue).HasColumnName("impsValue");

                entity.Property(e => e.LandId).HasColumnName("landId");

                entity.Property(e => e.LandIdName)
                    .HasColumnName("landIdName")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.LandValue).HasColumnName("landValue");

                entity.Property(e => e.ModifiedByName)
                    .HasColumnName("modifiedByName")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedOn)
                    .HasColumnName("modifiedOn")
                    .HasColumnType("datetime");

                entity.Property(e => e.ParcelGuid).HasColumnName("parcelGuid");

                entity.Property(e => e.ParcelIdName)
                    .HasColumnName("parcelIdName")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.PpTaxAccountId).HasColumnName("ppTaxAccountId");

                entity.Property(e => e.PpTaxAccountIdName)
                    .HasColumnName("ppTaxAccountIdName")
                    .HasMaxLength(100);

                entity.Property(e => e.RecName)
                    .HasColumnName("recName")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.RollYearGuid).HasColumnName("rollYearGuid");

                entity.Property(e => e.RollYearName)
                    .HasColumnName("rollYearName")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.TaxAccountId).HasColumnName("taxAccountId");

                entity.Property(e => e.TaxAccountIdName)
                    .HasColumnName("taxAccountIdName")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.TotalValue).HasColumnName("totalValue");
            });

            modelBuilder.Entity<PtasFloatingHomeReplacementCostRate>(entity =>
            {
                entity.HasKey(e => e.FloatingHomeReplacementCostRateId)
                    .HasName("PK__ptas_flo__CBA2DB12CBC90B17");

                entity.ToTable("ptas_floatingHomeReplacementCostRate", "ptas");

                entity.Property(e => e.FloatingHomeReplacementCostRateId)
                    .HasColumnName("floatingHomeReplacementCostRateId")
                    .ValueGeneratedNever();

                entity.Property(e => e.AssessmentYearGuid).HasColumnName("assessmentYearGuid");

                entity.Property(e => e.AssessmentYearIdName)
                    .HasColumnName("assessmentYearIdName")
                    .HasMaxLength(50);

                entity.Property(e => e.GradeAverage)
                    .HasColumnName("gradeAverage")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e.GradeAverageMinus)
                    .HasColumnName("gradeAverageMinus")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e.GradeAveragePlus)
                    .HasColumnName("gradeAveragePlus")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e.GradeExcellent)
                    .HasColumnName("gradeExcellent")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e.GradeExcellentMinus)
                    .HasColumnName("gradeExcellentMinus")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e.GradeExcellentPlus)
                    .HasColumnName("gradeExcellentPlus")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e.GradeGood)
                    .HasColumnName("gradeGood")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e.GradeGoodMinus)
                    .HasColumnName("gradeGoodMinus")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e.GradeGoodPlus)
                    .HasColumnName("gradeGoodPlus")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e.SpecialityAreaGuid).HasColumnName("specialityAreaGuid");

                entity.Property(e => e.SpecialityAreaIdName)
                    .HasColumnName("specialityAreaIdName")
                    .HasMaxLength(50);

                entity.Property(e => e.SpecialityNeighborhoodGuid).HasColumnName("specialityNeighborhoodGuid");

                entity.Property(e => e.SpecialityNeighborhoodIdName)
                    .HasColumnName("specialityNeighborhoodIdName")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<PtasFloatingHomeSlipValues>(entity =>
            {
                entity.HasKey(e => e.FloatingHomeSlipValuesId)
                    .HasName("PK__ptas_flo__54393D3FECAAC200");

                entity.ToTable("ptas_floatingHomeSlipValues", "ptas");

                entity.Property(e => e.FloatingHomeSlipValuesId)
                    .HasColumnName("floatingHomeSlipValuesId")
                    .ValueGeneratedNever();

                entity.Property(e => e.AssessmentYearGuid).HasColumnName("assessmentYearGuid");

                entity.Property(e => e.AssessmentYearIdName)
                    .HasColumnName("assessmentYearIdName")
                    .HasMaxLength(50);

                entity.Property(e => e.Grade1)
                    .HasColumnName("grade1")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e.Grade2)
                    .HasColumnName("grade2")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e.Grade3)
                    .HasColumnName("grade3")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e.Grade4)
                    .HasColumnName("grade4")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e.Grade5)
                    .HasColumnName("grade5")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e.Grade6)
                    .HasColumnName("grade6")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e.Grade7)
                    .HasColumnName("grade7")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e.SlipQuality).HasColumnName("slipQuality");

                entity.Property(e => e.SpecialityAreaGuid).HasColumnName("specialityAreaGuid");

                entity.Property(e => e.SpecialityAreaIdName)
                    .HasColumnName("specialityAreaIdName")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<PtasFloatingHomeValuation>(entity =>
            {
                entity.HasKey(e => e.FloatingHomeValuationId)
                    .HasName("PK__ptas_flo__800798B99CFCFB34");

                entity.ToTable("ptas_floatingHomeValuation", "ptas");

                entity.Property(e => e.FloatingHomeValuationId)
                    .HasColumnName("floatingHomeValuationId")
                    .ValueGeneratedNever();

                entity.Property(e => e.AssessmentYearGuid).HasColumnName("assessmentYearGuid");

                entity.Property(e => e.AssessmentYearIdName)
                    .HasColumnName("assessmentYearIdName")
                    .HasMaxLength(50);

                entity.Property(e => e.BasementValue)
                    .HasColumnName("basementValue")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e.CitySlipValue)
                    .HasColumnName("citySlipValue")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e.DnrSlipValue)
                    .HasColumnName("dnrSlipValue")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e.FloatingHomeProjectGuid).HasColumnName("floatingHomeProjectGuid");

                entity.Property(e => e.FloatingHomeProjectIdName)
                    .HasColumnName("floatingHomeProjectIdName")
                    .HasMaxLength(50);

                entity.Property(e => e.FloatingHomeUnitGuid).HasColumnName("floatingHomeUnitGuid");

                entity.Property(e => e.FloatingHomeUnitIdName)
                    .HasColumnName("floatingHomeUnitIdName")
                    .HasMaxLength(50);

                entity.Property(e => e.LivingValue)
                    .HasColumnName("livingValue")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e.ParcelGuid).HasColumnName("parcelGuid");

                entity.Property(e => e.ParcelIdName)
                    .HasColumnName("parcelIdName")
                    .HasMaxLength(50);

                entity.Property(e => e.PcntNetConditionValue)
                    .HasColumnName("pcntNetConditionValue")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e.Rcnld)
                    .HasColumnName("RCNLD")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e.RcnldperSqft)
                    .HasColumnName("RCNLDperSqft")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e.RcnperSqft)
                    .HasColumnName("RCNperSqft")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e.RecordName)
                    .HasColumnName("recordName")
                    .HasMaxLength(50);

                entity.Property(e => e.SlipGradeValue)
                    .HasColumnName("slipGradeValue")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e.SmallHomeAdjustmentValue)
                    .HasColumnName("smallHomeAdjustmentValue")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e.SubjectParcelSlipValue)
                    .HasColumnName("subjectParcelSlipValue")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e.TotalHomeValue)
                    .HasColumnName("totalHomeValue")
                    .HasColumnType("decimal(18, 0)");
            });

            modelBuilder.Entity<PtasIncomemodel>(entity =>
            {
                entity.HasKey(e => e.IncomemodelId);

                entity.ToTable("ptas_incomemodel", "ptas");

                entity.Property(e => e.IncomemodelId)
                    .HasColumnName("incomemodelId")
                    .ValueGeneratedNever();

                entity.Property(e => e.AssessmentYear).HasColumnName("assessmentYear");

                entity.Property(e => e.GeoArea).HasColumnName("geoArea");

                entity.Property(e => e.GeoNbhd).HasColumnName("geoNbhd");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(100);

                entity.Property(e => e.ReadyForValuation).HasColumnName("readyForValuation");

                entity.Property(e => e.SpecArea).HasColumnName("specArea");

                entity.Property(e => e.SpecNbhd).HasColumnName("specNbhd");
            });

            modelBuilder.Entity<PtasIncomemodeldetail>(entity =>
            {
                entity.HasKey(e => e.RowGuid)
                    .HasName("PK_RowGuid");

                entity.ToTable("ptas_incomemodeldetail", "ptas");

                entity.Property(e => e.RowGuid).HasDefaultValueSql("(newid())");

                entity.Property(e => e.AssessmentYearId).HasColumnName("assessmentYearId");

                entity.Property(e => e.CurrentSectionUseCodes)
                    .HasColumnName("currentSectionUseCodes")
                    .HasMaxLength(100)
                    .IsFixedLength();

                entity.Property(e => e.GeoAreaId).HasColumnName("geoAreaId");

                entity.Property(e => e.GeoNbhdId).HasColumnName("geoNbhdId");

                entity.Property(e => e.Grade1)
                    .HasColumnName("grade1")
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Grade2)
                    .HasColumnName("grade2")
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Grade3)
                    .HasColumnName("grade3")
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Grade4)
                    .HasColumnName("grade4")
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Grade5)
                    .HasColumnName("grade5")
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Grade6)
                    .HasColumnName("grade6")
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Grade7)
                    .HasColumnName("grade7")
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.IncomeModelId).HasColumnName("incomeModelId");

                entity.Property(e => e.MaxEffectiveYearId).HasColumnName("maxEffectiveYearId");

                entity.Property(e => e.MaxSqFt).HasColumnName("maxSqFt");

                entity.Property(e => e.MinEffectiveYearId).HasColumnName("minEffectiveYearId");

                entity.Property(e => e.MinSqFt).HasColumnName("minSqFt");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(100);

                entity.Property(e => e.OperatingExpenseCalc).HasColumnName("operatingExpenseCalc");

                entity.Property(e => e.RateType)
                    .HasColumnName("rateType")
                    .HasMaxLength(50);

                entity.Property(e => e.SpecAreaId).HasColumnName("specAreaId");

                entity.Property(e => e.SpecNbhdId).HasColumnName("specNbhdId");

                entity.Property(e => e.Stratification).HasColumnName("stratification");
            });

            modelBuilder.Entity<PtasIncomerates>(entity =>
            {
                entity.HasKey(e => e.RowGuid)
                    .HasName("PK_RowGuid1");

                entity.ToTable("ptas_incomerates", "ptas");

                entity.Property(e => e.RowGuid).HasDefaultValueSql("(newid())");

                entity.Property(e => e.AssesmentYearId).HasColumnName("assesmentYearId");

                entity.Property(e => e.BuildingQuality).HasColumnName("buildingQuality");

                entity.Property(e => e.CapitalizationRate)
                    .HasColumnName("capitalizationRate")
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.CurrentSectionUseCodes).HasColumnName("currentSectionUseCodes");

                entity.Property(e => e.GeoAreaId).HasColumnName("geoAreaId");

                entity.Property(e => e.GeoNbhdId).HasColumnName("geoNbhdId");

                entity.Property(e => e.IncomeModelId).HasColumnName("incomeModelId");

                entity.Property(e => e.LeasingClass).HasColumnName("leasingClass");

                entity.Property(e => e.MaxEffectiveYearId).HasColumnName("maxEffectiveYearId");

                entity.Property(e => e.MaxSqFt).HasColumnName("maxSqFt");

                entity.Property(e => e.MinEffectiveYearId).HasColumnName("minEffectiveYearId");

                entity.Property(e => e.MinSqFt).HasColumnName("minSqFt");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(100);

                entity.Property(e => e.OperatingExpensePercent)
                    .HasColumnName("operatingExpensePercent")
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Rent)
                    .HasColumnName("rent")
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.SpecAreaId).HasColumnName("specAreaId");

                entity.Property(e => e.SpecNbhdId).HasColumnName("specNbhdId");

                entity.Property(e => e.VacancyAndCollectionLoss)
                    .HasColumnName("vacancyAndCollectionLoss")
                    .HasColumnType("decimal(18, 2)");
            });

            modelBuilder.Entity<PtasIncomevaluation>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("ptas_incomevaluation", "ptas");

                entity.Property(e => e.AccountNumberId).HasColumnName("accountNumberId");

                entity.Property(e => e.AssessmentYearId).HasColumnName("assessmentYearId");

                entity.Property(e => e.BuildingId).HasColumnName("buildingId");

                entity.Property(e => e.CapRate)
                    .HasColumnName("capRate")
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.CapRateParking)
                    .HasColumnName("capRateParking")
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.DailyRate)
                    .HasColumnName("dailyRate")
                    .HasColumnType("money");

                entity.Property(e => e.DailySpaces).HasColumnName("dailySpaces");

                entity.Property(e => e.DollarPerSqFt)
                    .HasColumnName("dollarPerSqFt")
                    .HasColumnType("money");

                entity.Property(e => e.EffectiveGrossIncome)
                    .HasColumnName("effectiveGrossIncome")
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.EffectiveGrossIncomeParking)
                    .HasColumnName("effectiveGrossIncomeParking")
                    .HasColumnType("money");

                entity.Property(e => e.EstimateHistoryId).HasColumnName("estimateHistoryId");

                entity.Property(e => e.EstimateType).HasColumnName("estimateType");

                entity.Property(e => e.ExceptionCode)
                    .HasColumnName("exceptionCode")
                    .HasMaxLength(100)
                    .IsFixedLength();

                entity.Property(e => e.IncomeModelDetailId).HasColumnName("incomeModelDetailId");

                entity.Property(e => e.IndicatedValue)
                    .HasColumnName("indicatedValue")
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.MonthlyRate)
                    .HasColumnName("monthlyRate")
                    .HasColumnType("money");

                entity.Property(e => e.MonthlySpaces).HasColumnName("monthlySpaces");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(100);

                entity.Property(e => e.NetOperatingIncome)
                    .HasColumnName("netOperatingIncome")
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.NetOperatingIncomeParking)
                    .HasColumnName("netOperatingIncomeParking")
                    .HasColumnType("money");

                entity.Property(e => e.NetSqft).HasColumnName("netSqft");

                entity.Property(e => e.OperatingExpensePercent)
                    .HasColumnName("operatingExpensePercent")
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.OperatingExpensesParking)
                    .HasColumnName("operatingExpensesParking")
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ParcelId).HasColumnName("parcelId");

                entity.Property(e => e.ParkingDistrictId).HasColumnName("parkingDistrictId");

                entity.Property(e => e.ParkingOccupancy)
                    .HasColumnName("parkingOccupancy")
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.PotentialGrossIncome)
                    .HasColumnName("potentialGrossIncome")
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ProjectId).HasColumnName("projectId");

                entity.Property(e => e.Rent)
                    .HasColumnName("rent")
                    .HasColumnType("money");

                entity.Property(e => e.RowId)
                    .HasColumnName("RowID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.SectionUseId).HasColumnName("sectionUseId");

                entity.Property(e => e.VacancyAndLossCollection)
                    .HasColumnName("vacancyAndLossCollection")
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.WeightedCapAmt)
                    .HasColumnName("weightedCapAmt")
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.WeightedCapAmtParking)
                    .HasColumnName("weightedCapAmtParking")
                    .HasColumnType("decimal(18, 2)");
            });

            modelBuilder.Entity<PtasTaxRollHistory>(entity =>
            {
                entity.HasKey(e => e.TaxRollHistoryGuid)
                    .HasName("PK_taxRollHistGuid")
                    .IsClustered(false);

                entity.ToTable("ptas_taxRollHistory", "ptas");

                entity.HasIndex(e => e.ParcelGuid)
                    .HasName("IX_parcelguid");

                entity.HasIndex(e => e.RecType)
                    .HasName("idx_recType");

                entity.HasIndex(e => e.TaxYearIdName)
                    .HasName("IX_taxyearidname");

                entity.HasIndex(e => new { e.ParcelIdName, e.TaxYearIdName })
                    .HasName("idx_ParcelNum_Year")
                    .IsClustered();

                entity.HasIndex(e => new { e.TaxYearIdName, e.ParcelGuid })
                    .HasName("idx_parcelGuid");

                entity.HasIndex(e => new { e.OmitYearIdName, e.ParcelGuid, e.ReceivableType, e.TaxStat })
                    .HasName("IX_receivableType_taxStat");

                entity.HasIndex(e => new { e.OmitYearIdName, e.ParcelGuid, e.TaxableImpValue, e.TaxableLandValue, e.TaxAccountId, e.TaxAccountIdName, e.TaxYearIdName, e.TaxStat, e.ReceivableType })
                    .HasName("IX_receivabletype");

                entity.HasIndex(e => new { e.OmitYearIdName, e.TaxableImpValue, e.TaxableLandValue, e.TaxAccountId, e.TaxAccountIdName, e.TaxYearIdName, e.TaxStat, e.ParcelGuid, e.ReceivableType })
                    .HasName("IX_parcelguid_receivabletype");

                entity.Property(e => e.TaxRollHistoryGuid)
                    .HasColumnName("taxRollHistoryGuid")
                    .ValueGeneratedNever();

                entity.Property(e => e.AcctStat)
                    .HasColumnName("acctStat")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.AppraiserImpValue).HasColumnName("appraiserImpValue");

                entity.Property(e => e.AppraiserLandValue).HasColumnName("appraiserLandValue");

                entity.Property(e => e.AppraiserTotalValue).HasColumnName("appraiserTotalValue");

                entity.Property(e => e.IsCurrent).HasColumnName("isCurrent");

                entity.Property(e => e.LevyCodeId).HasColumnName("levyCodeId");

                entity.Property(e => e.LevyCodeIdName)
                    .HasColumnName("levyCodeIdName")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Modifiedon)
                    .HasColumnName("modifiedon")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.NewDollars).HasColumnName("newDollars");

                entity.Property(e => e.OmitYearId).HasColumnName("omitYearId");

                entity.Property(e => e.OmitYearIdName)
                    .HasColumnName("omitYearIdName")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ParcelGuid).HasColumnName("parcelGuid");

                entity.Property(e => e.ParcelIdName)
                    .HasColumnName("parcelIdName")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.RecName)
                    .HasColumnName("recName")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.RecType)
                    .HasColumnName("recType")
                    .HasMaxLength(1)
                    .IsFixedLength();

                entity.Property(e => e.ReceivableType)
                    .HasColumnName("receivableType")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.TaxAccountId).HasColumnName("taxAccountId");

                entity.Property(e => e.TaxAccountIdName)
                    .HasColumnName("taxAccountIdName")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.TaxStat)
                    .HasColumnName("taxStat")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.TaxYearId).HasColumnName("taxYearId");

                entity.Property(e => e.TaxYearIdName)
                    .HasColumnName("taxYearIdName")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.TaxableImpValue).HasColumnName("taxableImpValue");

                entity.Property(e => e.TaxableLandValue).HasColumnName("taxableLandValue");

                entity.Property(e => e.TaxableTotal).HasColumnName("taxableTotal");

                entity.Property(e => e.TaxableValueReason)
                    .HasColumnName("taxableValueReason")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UnitGuid).HasColumnName("unitGuid");

                entity.Property(e => e.UnitIdName)
                    .HasColumnName("unitIdName")
                    .HasMaxLength(100);
            });

            this.OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
