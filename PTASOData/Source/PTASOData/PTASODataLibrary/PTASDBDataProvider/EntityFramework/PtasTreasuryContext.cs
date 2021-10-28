namespace PTASODataLibrary.PtasDbDataProvider.EntityFramework
{
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata;
    using PTASODataLibrary.PtasDbDataProvider.PtasTreasuryModel;
    using PTASServicesCommon.TokenProvider;

    /// <summary>
    /// Treasury DB Context.
    /// </summary>
    /// <seealso cref="Microsoft.EntityFrameworkCore.DbContext" />
    [ExcludeFromCodeCoverage]
    public partial class PtasTreasuryContext : DbContext
    {
        private const string ConnectionStringPasswordSection = "password";

        /// <summary>
        /// Initializes a new instance of the <see cref="PtasTreasuryContext" /> class.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <param name="tokenProvider">The token provider.</param>
        /// <exception cref="System.ArgumentNullException">tokenProvider.</exception>
        public PtasTreasuryContext(DbContextOptions<PtasTreasuryContext> options, IServiceTokenProvider tokenProvider)
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
                if (!conn.ConnectionString.Contains(PtasTreasuryContext.ConnectionStringPasswordSection, System.StringComparison.OrdinalIgnoreCase))
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

        public virtual DbSet<EtaxAccountMaster> EtaxAccountMaster { get; set; }
        public virtual DbSet<EtaxBillingSummary> EtaxBillingSummary { get; set; }
        public virtual DbSet<EtaxReceipts> EtaxReceipts { get; set; }
        public virtual DbSet<EtaxTaxYearDetails> EtaxTaxYearDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EtaxAccountMaster>(entity =>
            {
                //// entity.HasNoKey();

                entity.ToView("ETaxAccountMaster", "ait");

                entity.Property(e => e.AccountStatus)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.CurrentMailingAddress)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.CustomerAccount)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.FiveYearOldOpen)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.Foreclosure)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.LenderCompanyCode)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.Lidsforeclosure)
                    .IsRequired()
                    .HasColumnName("LIDSForeclosure")
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.LidsorMams)
                    .IsRequired()
                    .HasColumnName("LIDSorMAMS")
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.LidssubjectToForeclosure)
                    .IsRequired()
                    .HasColumnName("LIDSSubjectToForeclosure")
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.ParcelNum).HasMaxLength(10);

                entity.Property(e => e.SubjectToForeclosure)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.TaxPayerName)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<EtaxBillingSummary>(entity =>
            {
                //// entity.HasNoKey();

                entity.ToView("ETaxBillingSummary", "ait");

                entity.Property(e => e.AccountNum)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.Amount).HasColumnType("numeric(38, 6)");

                entity.Property(e => e.CartType)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.DueDate).HasColumnType("datetime");

                entity.Property(e => e.ParcelNum).HasMaxLength(10);

                entity.Property(e => e.PaymentGroupId)
                    .IsRequired()
                    .HasMaxLength(67);
            });

            modelBuilder.Entity<EtaxReceipts>(entity =>
            {
                //// entity.HasNoKey();

                entity.ToView("ETaxReceipts", "ait");

                entity.Property(e => e.AccountNum)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.InterestAmount).HasColumnType("numeric(38, 6)");

                entity.Property(e => e.PaymentAmount).HasColumnType("numeric(32, 6)");

                entity.Property(e => e.PaymentDate).HasColumnType("datetime");

                entity.Property(e => e.PaymentId)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.ReceiptNum)
                    .IsRequired()
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<EtaxTaxYearDetails>(entity =>
            {
                //// entity.HasNoKey();

                entity.ToView("ETaxTaxYearDetails", "ait");

                entity.Property(e => e.AccountNum)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.Amount).HasColumnType("numeric(38, 6)");

                entity.Property(e => e.BillingClassification).HasMaxLength(30);

                entity.Property(e => e.ParcelNum).HasMaxLength(10);
            });

            this.OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
