using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using CustomSearchesValuationEFLibrary.Model;
using PTASServicesCommon.TokenProvider;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System.Threading.Tasks;

namespace CustomSearchesValuationEFLibrary
{
    public partial class ValuationDbContext : DbContext
    {
        /// <summary>
        /// The connection string password section.
        /// </summary>
        private const string ConnectionStringPasswordSection = "password";

        public ValuationDbContext()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomSearchesDbContext" /> class.
        /// </summary>
        /// <param name="options">The options for this context.</param>
        /// <param name="tokenProvider">The token provider.</param>
        /// <param name="principalCredentials">The principal credentials.</param>
        /// <exception cref="System.ArgumentNullException">tokenProvider</exception>
        public ValuationDbContext(DbContextOptions<ValuationDbContext> options, IServiceTokenProvider tokenProvider, ClientCredential principalCredentials)
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
                if (!conn.ConnectionString.Contains(ValuationDbContext.ConnectionStringPasswordSection, System.StringComparison.OrdinalIgnoreCase))
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
        public virtual DbSet<ptas_accessorydetail> ptas_accessorydetail { get; set; }
        public virtual DbSet<ptas_accessorydetail_ptas_mediarepository> ptas_accessorydetail_ptas_mediarepository { get; set; }
        public virtual DbSet<ptas_accessorydetail_snapshot> ptas_accessorydetail_snapshot { get; set; }
        public virtual DbSet<ptas_addresschangehistory> ptas_addresschangehistory { get; set; }
        public virtual DbSet<ptas_annexationparcelreview> ptas_annexationparcelreview { get; set; }
        public virtual DbSet<ptas_annexationtracker> ptas_annexationtracker { get; set; }
        public virtual DbSet<ptas_annualcostdistribution> ptas_annualcostdistribution { get; set; }
        public virtual DbSet<ptas_apartmentregion> ptas_apartmentregion { get; set; }
        public virtual DbSet<ptas_apartmentsupergroup> ptas_apartmentsupergroup { get; set; }
        public virtual DbSet<ptas_appealaccess> ptas_appealaccess { get; set; }
        public virtual DbSet<ptas_appraisalHistory> ptas_appraisalHistory { get; set; }
        public virtual DbSet<ptas_aptadjustedlevyrate> ptas_aptadjustedlevyrate { get; set; }
        public virtual DbSet<ptas_aptavailablecomparablesale> ptas_aptavailablecomparablesale { get; set; }
        public virtual DbSet<ptas_aptbuildingqualityadjustment> ptas_aptbuildingqualityadjustment { get; set; }
        public virtual DbSet<ptas_aptcloseproximity> ptas_aptcloseproximity { get; set; }
        public virtual DbSet<ptas_aptcommercialincomeexpense> ptas_aptcommercialincomeexpense { get; set; }
        public virtual DbSet<ptas_aptcomparablerent> ptas_aptcomparablerent { get; set; }
        public virtual DbSet<ptas_aptcomparablesale> ptas_aptcomparablesale { get; set; }
        public virtual DbSet<ptas_aptconditionadjustment> ptas_aptconditionadjustment { get; set; }
        public virtual DbSet<ptas_aptestimatedunitsqft> ptas_aptestimatedunitsqft { get; set; }
        public virtual DbSet<ptas_aptexpensehighend> ptas_aptexpensehighend { get; set; }
        public virtual DbSet<ptas_aptexpenseunitsize> ptas_aptexpenseunitsize { get; set; }
        public virtual DbSet<ptas_aptincomeexpense> ptas_aptincomeexpense { get; set; }
        public virtual DbSet<ptas_aptlistedrent> ptas_aptlistedrent { get; set; }
        public virtual DbSet<ptas_aptneighborhood> ptas_aptneighborhood { get; set; }
        public virtual DbSet<ptas_aptnumberofunitsadjustment> ptas_aptnumberofunitsadjustment { get; set; }
        public virtual DbSet<ptas_aptpoolandelevatorexpense> ptas_aptpoolandelevatorexpense { get; set; }
        public virtual DbSet<ptas_aptrentmodel> ptas_aptrentmodel { get; set; }
        public virtual DbSet<ptas_aptsalesmodel> ptas_aptsalesmodel { get; set; }
        public virtual DbSet<ptas_apttrending> ptas_apttrending { get; set; }
        public virtual DbSet<ptas_aptunittypeadjustment> ptas_aptunittypeadjustment { get; set; }
        public virtual DbSet<ptas_aptvaluation> ptas_aptvaluation { get; set; }
        public virtual DbSet<ptas_aptvaluationproject> ptas_aptvaluationproject { get; set; }
        public virtual DbSet<ptas_aptviewqualityadjustment> ptas_aptviewqualityadjustment { get; set; }
        public virtual DbSet<ptas_aptviewrankadjustment> ptas_aptviewrankadjustment { get; set; }
        public virtual DbSet<ptas_aptviewtypeadjustment> ptas_aptviewtypeadjustment { get; set; }
        public virtual DbSet<ptas_aptyearbuiltadjustment> ptas_aptyearbuiltadjustment { get; set; }
        public virtual DbSet<ptas_arcreasoncode> ptas_arcreasoncode { get; set; }
        public virtual DbSet<ptas_area> ptas_area { get; set; }
        public virtual DbSet<ptas_assessmentrollcorrection> ptas_assessmentrollcorrection { get; set; }
        public virtual DbSet<ptas_bookmark> ptas_bookmark { get; set; }
        public virtual DbSet<ptas_bookmarktag> ptas_bookmarktag { get; set; }
        public virtual DbSet<ptas_buildingdetail> ptas_buildingdetail { get; set; }
        public virtual DbSet<ptas_buildingdetail_commercialuse> ptas_buildingdetail_commercialuse { get; set; }
        public virtual DbSet<ptas_buildingdetail_commercialuse_snapshot> ptas_buildingdetail_commercialuse_snapshot { get; set; }
        public virtual DbSet<ptas_buildingdetail_ptas_mediarepository> ptas_buildingdetail_ptas_mediarepository { get; set; }
        public virtual DbSet<ptas_buildingdetail_snapshot> ptas_buildingdetail_snapshot { get; set; }
        public virtual DbSet<ptas_buildingsectionfeature> ptas_buildingsectionfeature { get; set; }
        public virtual DbSet<ptas_buildingsectionfeature_snapshot> ptas_buildingsectionfeature_snapshot { get; set; }
        public virtual DbSet<ptas_buildingsectionuse> ptas_buildingsectionuse { get; set; }
        public virtual DbSet<ptas_camanotes> ptas_camanotes { get; set; }
        public virtual DbSet<ptas_camanotes_ptas_mediarepository> ptas_camanotes_ptas_mediarepository { get; set; }
        public virtual DbSet<ptas_changehistory> ptas_changehistory { get; set; }
        public virtual DbSet<ptas_changereason> ptas_changereason { get; set; }
        public virtual DbSet<ptas_city> ptas_city { get; set; }
        public virtual DbSet<ptas_city_county> ptas_city_county { get; set; }
        public virtual DbSet<ptas_city_stateorprovince> ptas_city_stateorprovince { get; set; }
        public virtual DbSet<ptas_city_zipcode> ptas_city_zipcode { get; set; }
        public virtual DbSet<ptas_condocomplex> ptas_condocomplex { get; set; }
        public virtual DbSet<ptas_condocomplex_ptas_mediarepository> ptas_condocomplex_ptas_mediarepository { get; set; }
        public virtual DbSet<ptas_condocomplex_snapshot> ptas_condocomplex_snapshot { get; set; }
        public virtual DbSet<ptas_condounit> ptas_condounit { get; set; }
        public virtual DbSet<ptas_condounit_ptas_mediarepository> ptas_condounit_ptas_mediarepository { get; set; }
        public virtual DbSet<ptas_condounit_snapshot> ptas_condounit_snapshot { get; set; }
        public virtual DbSet<ptas_contaminatedlandreduction> ptas_contaminatedlandreduction { get; set; }
        public virtual DbSet<ptas_contaminationproject> ptas_contaminationproject { get; set; }
        public virtual DbSet<ptas_country> ptas_country { get; set; }
        public virtual DbSet<ptas_county> ptas_county { get; set; }
        public virtual DbSet<ptas_county_stateorprovince> ptas_county_stateorprovince { get; set; }
        public virtual DbSet<ptas_county_zipcode> ptas_county_zipcode { get; set; }
        public virtual DbSet<ptas_depreciationtable> ptas_depreciationtable { get; set; }
        public virtual DbSet<ptas_district> ptas_district { get; set; }
        public virtual DbSet<ptas_ebscostcenter> ptas_ebscostcenter { get; set; }
        public virtual DbSet<ptas_ebsfundnumber> ptas_ebsfundnumber { get; set; }
        public virtual DbSet<ptas_ebsmainaccount> ptas_ebsmainaccount { get; set; }
        public virtual DbSet<ptas_ebsproject> ptas_ebsproject { get; set; }
        public virtual DbSet<ptas_economicunit> ptas_economicunit { get; set; }
        public virtual DbSet<ptas_economicunit_accessorydetail> ptas_economicunit_accessorydetail { get; set; }
        public virtual DbSet<ptas_environmentalrestriction> ptas_environmentalrestriction { get; set; }
        public virtual DbSet<ptas_estimateHistory> ptas_estimateHistory { get; set; }
        public virtual DbSet<ptas_fileattachmentmetadata> ptas_fileattachmentmetadata { get; set; }
        public virtual DbSet<ptas_floatingHomeReplacementCostRate> ptas_floatingHomeReplacementCostRate { get; set; }
        public virtual DbSet<ptas_floatingHomeSlipValues> ptas_floatingHomeSlipValues { get; set; }
        public virtual DbSet<ptas_floatingHomeValuation> ptas_floatingHomeValuation { get; set; }
        public virtual DbSet<ptas_geoarea> ptas_geoarea { get; set; }
        public virtual DbSet<ptas_geoneighborhood> ptas_geoneighborhood { get; set; }
        public virtual DbSet<ptas_govtaxpayername> ptas_govtaxpayername { get; set; }
        public virtual DbSet<ptas_gradestratificationmapping> ptas_gradestratificationmapping { get; set; }
        public virtual DbSet<ptas_homeimprovement> ptas_homeimprovement { get; set; }
        public virtual DbSet<ptas_homeimprovementnotes> ptas_homeimprovementnotes { get; set; }
        public virtual DbSet<ptas_housingprogram> ptas_housingprogram { get; set; }
        public virtual DbSet<ptas_incomemodel> ptas_incomemodel { get; set; }
        public virtual DbSet<ptas_incomemodeldetail> ptas_incomemodeldetail { get; set; }
        public virtual DbSet<ptas_incomevaluation> ptas_incomevaluation { get; set; }
        public virtual DbSet<ptas_industry> ptas_industry { get; set; }
        public virtual DbSet<ptas_industry_ptas_personalpropertycategory> ptas_industry_ptas_personalpropertycategory { get; set; }
        public virtual DbSet<ptas_inspectionhistory> ptas_inspectionhistory { get; set; }
        public virtual DbSet<ptas_inspectionyear> ptas_inspectionyear { get; set; }
        public virtual DbSet<ptas_jurisdiction> ptas_jurisdiction { get; set; }
        public virtual DbSet<ptas_land> ptas_land { get; set; }
        public virtual DbSet<ptas_land_ptas_mediarepository> ptas_land_ptas_mediarepository { get; set; }
        public virtual DbSet<ptas_land_snapshot> ptas_land_snapshot { get; set; }
        public virtual DbSet<ptas_landuse> ptas_landuse { get; set; }
        public virtual DbSet<ptas_landvaluebreakdown> ptas_landvaluebreakdown { get; set; }
        public virtual DbSet<ptas_landvaluebreakdown_snapshot> ptas_landvaluebreakdown_snapshot { get; set; }
        public virtual DbSet<ptas_landvaluecalculation> ptas_landvaluecalculation { get; set; }
        public virtual DbSet<ptas_landvaluecalculation_snapshot> ptas_landvaluecalculation_snapshot { get; set; }
        public virtual DbSet<ptas_levycode> ptas_levycode { get; set; }
        public virtual DbSet<ptas_lowincomehousingprogram> ptas_lowincomehousingprogram { get; set; }
        public virtual DbSet<ptas_lowincomehousingprogram_snapshot> ptas_lowincomehousingprogram_snapshot { get; set; }
        public virtual DbSet<ptas_lowincomehousingunits> ptas_lowincomehousingunits { get; set; }
        public virtual DbSet<ptas_lowincomeparameters> ptas_lowincomeparameters { get; set; }
        public virtual DbSet<ptas_masspayaccumulator> ptas_masspayaccumulator { get; set; }
        public virtual DbSet<ptas_masspayaction> ptas_masspayaction { get; set; }
        public virtual DbSet<ptas_masspayer> ptas_masspayer { get; set; }
        public virtual DbSet<ptas_mediarepository> ptas_mediarepository { get; set; }
        public virtual DbSet<ptas_naicscode> ptas_naicscode { get; set; }
        public virtual DbSet<ptas_neighborhood> ptas_neighborhood { get; set; }
        public virtual DbSet<ptas_notificationconfiguration> ptas_notificationconfiguration { get; set; }
        public virtual DbSet<ptas_omit> ptas_omit { get; set; }
        public virtual DbSet<ptas_parceldetail> ptas_parceldetail { get; set; }
        public virtual DbSet<ptas_parceldetail_ptas_mediarepository> ptas_parceldetail_ptas_mediarepository { get; set; }
        public virtual DbSet<ptas_parceldetail_snapshot> ptas_parceldetail_snapshot { get; set; }
        public virtual DbSet<ptas_parceleconomicunit> ptas_parceleconomicunit { get; set; }
        public virtual DbSet<ptas_parkingdistrict> ptas_parkingdistrict { get; set; }
        public virtual DbSet<ptas_permit> ptas_permit { get; set; }
        public virtual DbSet<ptas_permit_ptas_mediarepository> ptas_permit_ptas_mediarepository { get; set; }
        public virtual DbSet<ptas_permitinspectionhistory> ptas_permitinspectionhistory { get; set; }
        public virtual DbSet<ptas_permitwebsiteconfig> ptas_permitwebsiteconfig { get; set; }
        public virtual DbSet<ptas_phonenumber> ptas_phonenumber { get; set; }
        public virtual DbSet<ptas_portaladdress> ptas_portaladdress { get; set; }
        public virtual DbSet<ptas_portalcontact> ptas_portalcontact { get; set; }
        public virtual DbSet<ptas_portalcontact_ptas_parceldetail> ptas_portalcontact_ptas_parceldetail { get; set; }
        public virtual DbSet<ptas_portalemail> ptas_portalemail { get; set; }
        public virtual DbSet<ptas_projectdock> ptas_projectdock { get; set; }
        public virtual DbSet<ptas_projectdock_snapshot> ptas_projectdock_snapshot { get; set; }
        public virtual DbSet<ptas_propertytype> ptas_propertytype { get; set; }
        public virtual DbSet<ptas_ptas_bookmark_ptas_bookmarktag> ptas_ptas_bookmark_ptas_bookmarktag { get; set; }
        public virtual DbSet<ptas_ptas_camanotes_ptas_fileattachmentmetad> ptas_ptas_camanotes_ptas_fileattachmentmetad { get; set; }
        public virtual DbSet<ptas_ptas_fileattachmentmetadata_ptas_addres> ptas_ptas_fileattachmentmetadata_ptas_addres { get; set; }
        public virtual DbSet<ptas_ptas_permit_ptas_fileattachmentmetadata> ptas_ptas_permit_ptas_fileattachmentmetadata { get; set; }
        public virtual DbSet<ptas_ptas_permit_ptas_mediarepository> ptas_ptas_permit_ptas_mediarepository { get; set; }
        public virtual DbSet<ptas_ptas_residentialappraiserteam_systemuse> ptas_ptas_residentialappraiserteam_systemuse { get; set; }
        public virtual DbSet<ptas_ptas_sales_ptas_fileattachmentmetadata> ptas_ptas_sales_ptas_fileattachmentmetadata { get; set; }
        public virtual DbSet<ptas_ptas_salesnote_ptas_fileattachmentmetad> ptas_ptas_salesnote_ptas_fileattachmentmetad { get; set; }
        public virtual DbSet<ptas_ptas_task_ptas_fileattachmentmetadata> ptas_ptas_task_ptas_fileattachmentmetadata { get; set; }
        public virtual DbSet<ptas_ptas_task_ptas_parceldetail> ptas_ptas_task_ptas_parceldetail { get; set; }
        public virtual DbSet<ptas_ptas_task_ptas_taxrollcorrection> ptas_ptas_task_ptas_taxrollcorrection { get; set; }
        public virtual DbSet<ptas_ptas_taxaccount_ptas_fileattachmentmeta> ptas_ptas_taxaccount_ptas_fileattachmentmeta { get; set; }
        public virtual DbSet<ptas_ptasconfiguration> ptas_ptasconfiguration { get; set; }
        public virtual DbSet<ptas_ptassetting> ptas_ptassetting { get; set; }
        public virtual DbSet<ptas_qstr> ptas_qstr { get; set; }
        public virtual DbSet<ptas_quickcollect> ptas_quickcollect { get; set; }
        public virtual DbSet<ptas_recentparcel> ptas_recentparcel { get; set; }
        public virtual DbSet<ptas_residentialappraiserteam> ptas_residentialappraiserteam { get; set; }
        public virtual DbSet<ptas_responsibility> ptas_responsibility { get; set; }
        public virtual DbSet<ptas_restrictedrent> ptas_restrictedrent { get; set; }
        public virtual DbSet<ptas_salepriceadjustment> ptas_salepriceadjustment { get; set; }
        public virtual DbSet<ptas_sales> ptas_sales { get; set; }
        public virtual DbSet<ptas_sales_parceldetail_parcelsinsale> ptas_sales_parceldetail_parcelsinsale { get; set; }
        public virtual DbSet<ptas_sales_ptas_mediarepository> ptas_sales_ptas_mediarepository { get; set; }
        public virtual DbSet<ptas_sales_ptas_saleswarningcode> ptas_sales_ptas_saleswarningcode { get; set; }
        public virtual DbSet<ptas_salesaggregate> ptas_salesaggregate { get; set; }
        public virtual DbSet<ptas_salesnote> ptas_salesnote { get; set; }
        public virtual DbSet<ptas_salesnote_ptas_mediarepository> ptas_salesnote_ptas_mediarepository { get; set; }
        public virtual DbSet<ptas_saleswarningcode> ptas_saleswarningcode { get; set; }
        public virtual DbSet<ptas_scheduledworkflow> ptas_scheduledworkflow { get; set; }
        public virtual DbSet<ptas_sectionusesqft> ptas_sectionusesqft { get; set; }
        public virtual DbSet<ptas_sectionusesqft_snapshot> ptas_sectionusesqft_snapshot { get; set; }
        public virtual DbSet<ptas_sketch> ptas_sketch { get; set; }
        public virtual DbSet<ptas_specialtyarea> ptas_specialtyarea { get; set; }
        public virtual DbSet<ptas_specialtyneighborhood> ptas_specialtyneighborhood { get; set; }
        public virtual DbSet<ptas_stateorprovince> ptas_stateorprovince { get; set; }
        public virtual DbSet<ptas_streetname> ptas_streetname { get; set; }
        public virtual DbSet<ptas_streettype> ptas_streettype { get; set; }
        public virtual DbSet<ptas_subarea> ptas_subarea { get; set; }
        public virtual DbSet<ptas_submarket> ptas_submarket { get; set; }
        public virtual DbSet<ptas_supergroup> ptas_supergroup { get; set; }
        public virtual DbSet<ptas_task> ptas_task { get; set; }
        public virtual DbSet<ptas_taxRollHistory> ptas_taxRollHistory { get; set; }
        public virtual DbSet<ptas_taxaccount> ptas_taxaccount { get; set; }
        public virtual DbSet<ptas_taxaccount_snapshot> ptas_taxaccount_snapshot { get; set; }
        public virtual DbSet<ptas_trendfactor> ptas_trendfactor { get; set; }
        public virtual DbSet<ptas_unitbreakdown> ptas_unitbreakdown { get; set; }
        public virtual DbSet<ptas_unitbreakdown_snapshot> ptas_unitbreakdown_snapshot { get; set; }
        public virtual DbSet<ptas_unitbreakdowntype> ptas_unitbreakdowntype { get; set; }
        public virtual DbSet<ptas_visitedsketch> ptas_visitedsketch { get; set; }
        public virtual DbSet<ptas_year> ptas_year { get; set; }
        public virtual DbSet<ptas_zipcode> ptas_zipcode { get; set; }
        public virtual DbSet<ptas_zipcode_stateorprovince> ptas_zipcode_stateorprovince { get; set; }
        public virtual DbSet<ptas_zoning> ptas_zoning { get; set; }
        public virtual DbSet<role> role { get; set; }
        public virtual DbSet<stringmap> stringmap { get; set; }
        public virtual DbSet<systemuser> systemuser { get; set; }
        public virtual DbSet<systemuserroles> systemuserroles { get; set; }
        public virtual DbSet<team> team { get; set; }
        public virtual DbSet<teammembership> teammembership { get; set; }
        public virtual DbSet<teamprofiles> teamprofiles { get; set; }
        public virtual DbSet<teamroles> teamroles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=tcp:Kcitazrsqldev01.database.windows.net,1433;Authentication=Active Directory Password;User ID=n-sderendoff@kingcounty.gov;Password=FPuMK4kNbZq@1mNecl3VFi%99;Initial Catalog=PTAS;Persist Security Info=False;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;", x => x.UseNetTopologySuite());
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ptas_accessorydetail>(entity =>
            {
                entity.ToTable("ptas_accessorydetail", "dynamics");

                entity.HasIndex(e => e.modifiedon)
                    .HasName("Idx_ptas_accessorydetail_modifiedon");

                entity.Property(e => e.ptas_accessorydetailid).ValueGeneratedNever();

                entity.Property(e => e.exchangerate).HasColumnType("money");

                entity.Property(e => e.ptas_accessoryvalue).HasColumnType("money");

                entity.Property(e => e.ptas_accessoryvalue_base).HasColumnType("money");

                entity.Property(e => e.ptas_netconditionvalue).HasColumnType("money");

                entity.Property(e => e.ptas_netconditionvalue_base).HasColumnType("money");

                entity.HasOne(d => d._createdby_valueNavigation)
                    .WithMany(p => p.ptas_accessorydetail_createdby_valueNavigation)
                    .HasForeignKey(d => d._createdby_value)
                    .HasConstraintName("FK_systemuser_ptas_accessorydetail_createdby");

                entity.HasOne(d => d._createdonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_accessorydetail_createdonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._createdonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_accessorydetail_createdonbehalfby");

                entity.HasOne(d => d._modifiedby_valueNavigation)
                    .WithMany(p => p.ptas_accessorydetail_modifiedby_valueNavigation)
                    .HasForeignKey(d => d._modifiedby_value)
                    .HasConstraintName("FK_systemuser_ptas_accessorydetail_modifiedby");

                entity.HasOne(d => d._modifiedonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_accessorydetail_modifiedonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._modifiedonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_accessorydetail_modifiedonbehalfby");

                entity.HasOne(d => d._owningteam_valueNavigation)
                    .WithMany(p => p.ptas_accessorydetail)
                    .HasForeignKey(d => d._owningteam_value)
                    .HasConstraintName("FK_team_ptas_accessorydetail_owningteam");

                entity.HasOne(d => d._owninguser_valueNavigation)
                    .WithMany(p => p.ptas_accessorydetail_owninguser_valueNavigation)
                    .HasForeignKey(d => d._owninguser_value)
                    .HasConstraintName("FK_systemuser_ptas_accessorydetail_owninguser");

                entity.HasOne(d => d._ptas_buildingdetailid_valueNavigation)
                    .WithMany(p => p.ptas_accessorydetail)
                    .HasForeignKey(d => d._ptas_buildingdetailid_value)
                    .HasConstraintName("FK_ptas_buildingdetail_ptas_accessorydetail_ptas_buildingdetailid");

                entity.HasOne(d => d._ptas_masteraccessoryid_valueNavigation)
                    .WithMany(p => p.Inverse_ptas_masteraccessoryid_valueNavigation)
                    .HasForeignKey(d => d._ptas_masteraccessoryid_value)
                    .HasConstraintName("FK_ptas_accessorydetail_ptas_accessorydetail_ptas_masteraccessoryid");

                entity.HasOne(d => d._ptas_parceldetailid_valueNavigation)
                    .WithMany(p => p.ptas_accessorydetail)
                    .HasForeignKey(d => d._ptas_parceldetailid_value)
                    .HasConstraintName("FK_ptas_parceldetail_ptas_accessorydetail_ptas_parceldetailid");

                entity.HasOne(d => d._ptas_projectid_valueNavigation)
                    .WithMany(p => p.ptas_accessorydetail)
                    .HasForeignKey(d => d._ptas_projectid_value)
                    .HasConstraintName("FK_ptas_condocomplex_ptas_accessorydetail_ptas_projectid");

                entity.HasOne(d => d._ptas_propertytypeid_valueNavigation)
                    .WithMany(p => p.ptas_accessorydetail)
                    .HasForeignKey(d => d._ptas_propertytypeid_value)
                    .HasConstraintName("FK_ptas_propertytype_ptas_accessorydetail_ptas_propertytypeid");

                entity.HasOne(d => d._ptas_sketchid_valueNavigation)
                    .WithMany(p => p.ptas_accessorydetail)
                    .HasForeignKey(d => d._ptas_sketchid_value)
                    .HasConstraintName("FK_ptas_sketch_ptas_accessorydetail_ptas_sketchid");
            });

            modelBuilder.Entity<ptas_accessorydetail_ptas_mediarepository>(entity =>
            {
                entity.ToTable("ptas_accessorydetail_ptas_mediarepository", "dynamics");

                entity.HasIndex(e => e.modifiedon)
                    .HasName("Idx_ptas_accessorydetail_ptas_mediarepository_modifiedon");

                entity.Property(e => e.ptas_accessorydetail_ptas_mediarepositoryid).ValueGeneratedNever();

                entity.Property(e => e.modifiedon)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");
            });

            modelBuilder.Entity<ptas_accessorydetail_snapshot>(entity =>
            {
                entity.HasKey(e => e.ptas_accessorydetailid);

                entity.ToTable("ptas_accessorydetail_snapshot", "dynamics");

                entity.Property(e => e.ptas_accessorydetailid).ValueGeneratedNever();

                entity.Property(e => e.exchangerate).HasColumnType("money");

                entity.Property(e => e.ptas_accessoryvalue).HasColumnType("money");

                entity.Property(e => e.ptas_accessoryvalue_base).HasColumnType("money");

                entity.Property(e => e.ptas_netconditionvalue).HasColumnType("money");

                entity.Property(e => e.ptas_netconditionvalue_base).HasColumnType("money");
            });

            modelBuilder.Entity<ptas_addresschangehistory>(entity =>
            {
                entity.ToTable("ptas_addresschangehistory", "dynamics");

                entity.Property(e => e.ptas_addresschangehistoryid).ValueGeneratedNever();

                entity.HasOne(d => d._createdby_valueNavigation)
                    .WithMany(p => p.ptas_addresschangehistory_createdby_valueNavigation)
                    .HasForeignKey(d => d._createdby_value)
                    .HasConstraintName("FK_systemuser_ptas_addresschangehistory_createdby");

                entity.HasOne(d => d._createdonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_addresschangehistory_createdonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._createdonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_addresschangehistory_createdonbehalfby");

                entity.HasOne(d => d._modifiedby_valueNavigation)
                    .WithMany(p => p.ptas_addresschangehistory_modifiedby_valueNavigation)
                    .HasForeignKey(d => d._modifiedby_value)
                    .HasConstraintName("FK_systemuser_ptas_addresschangehistory_modifiedby");

                entity.HasOne(d => d._modifiedonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_addresschangehistory_modifiedonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._modifiedonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_addresschangehistory_modifiedonbehalfby");

                entity.HasOne(d => d._owningteam_valueNavigation)
                    .WithMany(p => p.ptas_addresschangehistory)
                    .HasForeignKey(d => d._owningteam_value)
                    .HasConstraintName("FK_team_ptas_addresschangehistory_owningteam");

                entity.HasOne(d => d._owninguser_valueNavigation)
                    .WithMany(p => p.ptas_addresschangehistory_owninguser_valueNavigation)
                    .HasForeignKey(d => d._owninguser_value)
                    .HasConstraintName("FK_systemuser_ptas_addresschangehistory_owninguser");

                entity.HasOne(d => d._ptas_parcelid_valueNavigation)
                    .WithMany(p => p.ptas_addresschangehistory)
                    .HasForeignKey(d => d._ptas_parcelid_value)
                    .HasConstraintName("FK_ptas_parceldetail_ptas_addresschangehistory_ptas_parcelid");

                entity.HasOne(d => d._ptas_taxaccountid_valueNavigation)
                    .WithMany(p => p.ptas_addresschangehistory)
                    .HasForeignKey(d => d._ptas_taxaccountid_value)
                    .HasConstraintName("FK_ptas_taxaccount_ptas_addresschangehistory_ptas_taxaccountid");
            });

            modelBuilder.Entity<ptas_annexationparcelreview>(entity =>
            {
                entity.ToTable("ptas_annexationparcelreview", "dynamics");

                entity.Property(e => e.ptas_annexationparcelreviewid).ValueGeneratedNever();

                entity.Property(e => e.exchangerate).HasColumnType("money");

                entity.Property(e => e.ptas_assessedimprovements).HasColumnType("money");

                entity.Property(e => e.ptas_assessedimprovements_base).HasColumnType("money");

                entity.Property(e => e.ptas_assessedland).HasColumnType("money");

                entity.Property(e => e.ptas_assessedland_base).HasColumnType("money");

                entity.Property(e => e.ptas_totalassessedvalue).HasColumnType("money");

                entity.Property(e => e.ptas_totalassessedvalue_base).HasColumnType("money");

                entity.HasOne(d => d._createdby_valueNavigation)
                    .WithMany(p => p.ptas_annexationparcelreview_createdby_valueNavigation)
                    .HasForeignKey(d => d._createdby_value)
                    .HasConstraintName("FK_systemuser_ptas_annexationparcelreview_createdby");

                entity.HasOne(d => d._createdonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_annexationparcelreview_createdonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._createdonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_annexationparcelreview_createdonbehalfby");

                entity.HasOne(d => d._modifiedby_valueNavigation)
                    .WithMany(p => p.ptas_annexationparcelreview_modifiedby_valueNavigation)
                    .HasForeignKey(d => d._modifiedby_value)
                    .HasConstraintName("FK_systemuser_ptas_annexationparcelreview_modifiedby");

                entity.HasOne(d => d._modifiedonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_annexationparcelreview_modifiedonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._modifiedonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_annexationparcelreview_modifiedonbehalfby");

                entity.HasOne(d => d._owningteam_valueNavigation)
                    .WithMany(p => p.ptas_annexationparcelreview)
                    .HasForeignKey(d => d._owningteam_value)
                    .HasConstraintName("FK_team_ptas_annexationparcelreview_owningteam");

                entity.HasOne(d => d._owninguser_valueNavigation)
                    .WithMany(p => p.ptas_annexationparcelreview_owninguser_valueNavigation)
                    .HasForeignKey(d => d._owninguser_value)
                    .HasConstraintName("FK_systemuser_ptas_annexationparcelreview_owninguser");

                entity.HasOne(d => d._ptas_annexationtrackerid_valueNavigation)
                    .WithMany(p => p.ptas_annexationparcelreview)
                    .HasForeignKey(d => d._ptas_annexationtrackerid_value)
                    .HasConstraintName("FK_ptas_annexationtracker_ptas_annexationparcelreview_ptas_annexationtrackerid");

                entity.HasOne(d => d._ptas_levycodeid_valueNavigation)
                    .WithMany(p => p.ptas_annexationparcelreview)
                    .HasForeignKey(d => d._ptas_levycodeid_value)
                    .HasConstraintName("FK_ptas_levycode_ptas_annexationparcelreview_ptas_levycodeid");

                entity.HasOne(d => d._ptas_parcel_valueNavigation)
                    .WithMany(p => p.ptas_annexationparcelreview)
                    .HasForeignKey(d => d._ptas_parcel_value)
                    .HasConstraintName("FK_ptas_parceldetail_ptas_annexationparcelreview_ptas_parcel");

                entity.HasOne(d => d._ptas_taxrollyearforav_valueNavigation)
                    .WithMany(p => p.ptas_annexationparcelreview)
                    .HasForeignKey(d => d._ptas_taxrollyearforav_value)
                    .HasConstraintName("FK_ptas_year_ptas_annexationparcelreview_ptas_taxrollyearforav");
            });

            modelBuilder.Entity<ptas_annexationtracker>(entity =>
            {
                entity.ToTable("ptas_annexationtracker", "dynamics");

                entity.Property(e => e.ptas_annexationtrackerid).ValueGeneratedNever();

                entity.Property(e => e.exchangerate).HasColumnType("money");

                entity.Property(e => e.ptas_signedandverifiedav).HasColumnType("money");

                entity.Property(e => e.ptas_signedandverifiedav_base).HasColumnType("money");

                entity.Property(e => e.ptas_signedandverifiedpercentage).HasColumnType("money");

                entity.Property(e => e.ptas_totalassessedvalue).HasColumnType("money");

                entity.Property(e => e.ptas_totalassessedvalue_base).HasColumnType("money");

                entity.HasOne(d => d._createdby_valueNavigation)
                    .WithMany(p => p.ptas_annexationtracker_createdby_valueNavigation)
                    .HasForeignKey(d => d._createdby_value)
                    .HasConstraintName("FK_systemuser_ptas_annexationtracker_createdby");

                entity.HasOne(d => d._createdonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_annexationtracker_createdonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._createdonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_annexationtracker_createdonbehalfby");

                entity.HasOne(d => d._modifiedby_valueNavigation)
                    .WithMany(p => p.ptas_annexationtracker_modifiedby_valueNavigation)
                    .HasForeignKey(d => d._modifiedby_value)
                    .HasConstraintName("FK_systemuser_ptas_annexationtracker_modifiedby");

                entity.HasOne(d => d._modifiedonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_annexationtracker_modifiedonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._modifiedonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_annexationtracker_modifiedonbehalfby");

                entity.HasOne(d => d._owningteam_valueNavigation)
                    .WithMany(p => p.ptas_annexationtracker)
                    .HasForeignKey(d => d._owningteam_value)
                    .HasConstraintName("FK_team_ptas_annexationtracker_owningteam");

                entity.HasOne(d => d._owninguser_valueNavigation)
                    .WithMany(p => p.ptas_annexationtracker_owninguser_valueNavigation)
                    .HasForeignKey(d => d._owninguser_value)
                    .HasConstraintName("FK_systemuser_ptas_annexationtracker_owninguser");

                entity.HasOne(d => d._ptas_taxrollyeareffectiveid_valueNavigation)
                    .WithMany(p => p.ptas_annexationtracker)
                    .HasForeignKey(d => d._ptas_taxrollyeareffectiveid_value)
                    .HasConstraintName("FK_ptas_year_ptas_annexationtracker_ptas_taxrollyeareffectiveid");
            });

            modelBuilder.Entity<ptas_annualcostdistribution>(entity =>
            {
                entity.ToTable("ptas_annualcostdistribution", "dynamics");

                entity.Property(e => e.ptas_annualcostdistributionid).ValueGeneratedNever();

                entity.Property(e => e.exchangerate).HasColumnType("money");

                entity.Property(e => e.ptas_10yeartreasuryrate).HasColumnType("money");

                entity.Property(e => e.ptas_annualcost).HasColumnType("money");

                entity.Property(e => e.ptas_annualcost_base).HasColumnType("money");

                entity.Property(e => e.ptas_presentcost).HasColumnType("money");

                entity.Property(e => e.ptas_presentcost_base).HasColumnType("money");

                entity.Property(e => e.ptas_presentvaluefactor).HasColumnType("money");

                entity.HasOne(d => d._createdby_valueNavigation)
                    .WithMany(p => p.ptas_annualcostdistribution_createdby_valueNavigation)
                    .HasForeignKey(d => d._createdby_value)
                    .HasConstraintName("FK_systemuser_ptas_annualcostdistribution_createdby");

                entity.HasOne(d => d._createdonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_annualcostdistribution_createdonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._createdonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_annualcostdistribution_createdonbehalfby");

                entity.HasOne(d => d._modifiedby_valueNavigation)
                    .WithMany(p => p.ptas_annualcostdistribution_modifiedby_valueNavigation)
                    .HasForeignKey(d => d._modifiedby_value)
                    .HasConstraintName("FK_systemuser_ptas_annualcostdistribution_modifiedby");

                entity.HasOne(d => d._modifiedonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_annualcostdistribution_modifiedonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._modifiedonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_annualcostdistribution_modifiedonbehalfby");

                entity.HasOne(d => d._owningteam_valueNavigation)
                    .WithMany(p => p.ptas_annualcostdistribution)
                    .HasForeignKey(d => d._owningteam_value)
                    .HasConstraintName("FK_team_ptas_annualcostdistribution_owningteam");

                entity.HasOne(d => d._owninguser_valueNavigation)
                    .WithMany(p => p.ptas_annualcostdistribution_owninguser_valueNavigation)
                    .HasForeignKey(d => d._owninguser_value)
                    .HasConstraintName("FK_systemuser_ptas_annualcostdistribution_owninguser");

                entity.HasOne(d => d._ptas_contaminatedproject_valueNavigation)
                    .WithMany(p => p.ptas_annualcostdistribution)
                    .HasForeignKey(d => d._ptas_contaminatedproject_value)
                    .HasConstraintName("FK_ptas_contaminationproject_ptas_annualcostdistribution_ptas_contaminatedproject");
            });

            modelBuilder.Entity<ptas_apartmentregion>(entity =>
            {
                entity.ToTable("ptas_apartmentregion", "dynamics");

                entity.HasIndex(e => e.modifiedon)
                    .HasName("Idx_ptas_apartmentregion_modifiedon");

                entity.Property(e => e.ptas_apartmentregionid).ValueGeneratedNever();

                entity.HasOne(d => d._createdby_valueNavigation)
                    .WithMany(p => p.ptas_apartmentregion_createdby_valueNavigation)
                    .HasForeignKey(d => d._createdby_value)
                    .HasConstraintName("FK_systemuser_ptas_apartmentregion_createdby");

                entity.HasOne(d => d._createdonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_apartmentregion_createdonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._createdonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_apartmentregion_createdonbehalfby");

                entity.HasOne(d => d._modifiedby_valueNavigation)
                    .WithMany(p => p.ptas_apartmentregion_modifiedby_valueNavigation)
                    .HasForeignKey(d => d._modifiedby_value)
                    .HasConstraintName("FK_systemuser_ptas_apartmentregion_modifiedby");

                entity.HasOne(d => d._modifiedonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_apartmentregion_modifiedonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._modifiedonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_apartmentregion_modifiedonbehalfby");
            });

            modelBuilder.Entity<ptas_apartmentsupergroup>(entity =>
            {
                entity.ToTable("ptas_apartmentsupergroup", "dynamics");

                entity.HasIndex(e => e.modifiedon)
                    .HasName("Idx_ptas_apartmentsupergroup_modifiedon");

                entity.Property(e => e.ptas_apartmentsupergroupid).ValueGeneratedNever();

                entity.HasOne(d => d._createdby_valueNavigation)
                    .WithMany(p => p.ptas_apartmentsupergroup_createdby_valueNavigation)
                    .HasForeignKey(d => d._createdby_value)
                    .HasConstraintName("FK_systemuser_ptas_apartmentsupergroup_createdby");

                entity.HasOne(d => d._createdonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_apartmentsupergroup_createdonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._createdonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_apartmentsupergroup_createdonbehalfby");

                entity.HasOne(d => d._modifiedby_valueNavigation)
                    .WithMany(p => p.ptas_apartmentsupergroup_modifiedby_valueNavigation)
                    .HasForeignKey(d => d._modifiedby_value)
                    .HasConstraintName("FK_systemuser_ptas_apartmentsupergroup_modifiedby");

                entity.HasOne(d => d._modifiedonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_apartmentsupergroup_modifiedonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._modifiedonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_apartmentsupergroup_modifiedonbehalfby");

                entity.HasOne(d => d._ptas_apartmentregionid_valueNavigation)
                    .WithMany(p => p.ptas_apartmentsupergroup)
                    .HasForeignKey(d => d._ptas_apartmentregionid_value)
                    .HasConstraintName("FK_ptas_apartmentregion_ptas_apartmentsupergroup_ptas_apartmentregionid");
            });

            modelBuilder.Entity<ptas_appealaccess>(entity =>
            {
                entity.ToTable("ptas_appealaccess", "dynamics");

                entity.Property(e => e.ptas_appealaccessid).ValueGeneratedNever();
            });

            modelBuilder.Entity<ptas_appraisalHistory>(entity =>
            {
                entity.HasKey(e => e.appraisalHistoryGuid)
                    .HasName("PK_apprHistGuid2")
                    .IsClustered(false);

                entity.ToTable("ptas_appraisalHistory", "ptas");

                entity.HasIndex(e => e.taxYearIdName)
                    .HasName("IX_taxyearidname");

                entity.HasIndex(e => new { e.appraisedDate, e.parcelGuid })
                    .HasName("IX_parcelguid");

                entity.HasIndex(e => new { e.parcelIdName, e.RollYear })
                    .HasName("idx_ParcelNum_Year")
                    .IsClustered();

                entity.HasIndex(e => new { e.parcelGuid, e.realPropId, e.taxYearIdName })
                    .HasName("IX_appraisalhistory_realpropid_taxyearidname");

                entity.HasIndex(e => new { e.RollYear, e.taxYearIdName, e.transactionDate, e.parcelGuid })
                    .HasName("idx_parcelGuid");

                entity.HasIndex(e => new { e.parcelIdName, e.parcelGuid, e.RollYear, e.taxYearIdName, e.transactionDate, e.landId })
                    .HasName("idx_landId");

                entity.HasIndex(e => new { e.landValue, e.totalValue, e.ApprMethod, e.createdOn, e.newConstrValue, e.appraiserName, e.interfaceFlag, e.valuationReason, e.parcelGuid, e.taxYearIdName, e.revalOrMaint, e.impsValue })
                    .HasName("Ix_parcelGuid_Taxyear_revalormaint_impvalue");

                entity.Property(e => e.appraisalHistoryGuid).ValueGeneratedNever();

                entity.Property(e => e.ApprMethod).HasMaxLength(100);

                entity.Property(e => e.ImageCode).HasMaxLength(1);

                entity.Property(e => e.InterfaceFlagDesc).HasMaxLength(50);

                entity.Property(e => e.LandIdName).HasMaxLength(100);

                entity.Property(e => e.PercentChange).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.RollYear).HasMaxLength(100);

                entity.Property(e => e.appraisedDate).HasColumnType("datetime");

                entity.Property(e => e.appraiserName).HasMaxLength(100);

                entity.Property(e => e.createdByName).HasMaxLength(100);

                entity.Property(e => e.createdOn).HasColumnType("datetime");

                entity.Property(e => e.modifiedByName).HasMaxLength(100);

                entity.Property(e => e.modifiedOn).HasColumnType("datetime");

                entity.Property(e => e.note).HasMaxLength(500);

                entity.Property(e => e.parcelIdName).HasMaxLength(100);

                entity.Property(e => e.postDate).HasColumnType("datetime");

                entity.Property(e => e.recName).HasMaxLength(100);

                entity.Property(e => e.revalOrMaint).HasMaxLength(100);

                entity.Property(e => e.taxAccountIdName).HasMaxLength(100);

                entity.Property(e => e.taxYearIdName).HasMaxLength(100);

                entity.Property(e => e.transactionBy).HasMaxLength(100);

                entity.Property(e => e.transactionDate).HasColumnType("datetime");

                entity.Property(e => e.valuationReason).HasMaxLength(100);
            });

            modelBuilder.Entity<ptas_aptadjustedlevyrate>(entity =>
            {
                entity.ToTable("ptas_aptadjustedlevyrate", "dynamics");

                entity.Property(e => e.ptas_aptadjustedlevyrateid).ValueGeneratedNever();

                entity.Property(e => e.ptas_adjustedlevyrate).HasColumnType("money");

                entity.HasOne(d => d._createdby_valueNavigation)
                    .WithMany(p => p.ptas_aptadjustedlevyrate_createdby_valueNavigation)
                    .HasForeignKey(d => d._createdby_value)
                    .HasConstraintName("FK_systemuser_ptas_aptadjustedlevyrate_createdby");

                entity.HasOne(d => d._createdonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_aptadjustedlevyrate_createdonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._createdonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_aptadjustedlevyrate_createdonbehalfby");

                entity.HasOne(d => d._modifiedby_valueNavigation)
                    .WithMany(p => p.ptas_aptadjustedlevyrate_modifiedby_valueNavigation)
                    .HasForeignKey(d => d._modifiedby_value)
                    .HasConstraintName("FK_systemuser_ptas_aptadjustedlevyrate_modifiedby");

                entity.HasOne(d => d._modifiedonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_aptadjustedlevyrate_modifiedonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._modifiedonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_aptadjustedlevyrate_modifiedonbehalfby");

                entity.HasOne(d => d._owningteam_valueNavigation)
                    .WithMany(p => p.ptas_aptadjustedlevyrate)
                    .HasForeignKey(d => d._owningteam_value)
                    .HasConstraintName("FK_team_ptas_aptadjustedlevyrate_owningteam");

                entity.HasOne(d => d._owninguser_valueNavigation)
                    .WithMany(p => p.ptas_aptadjustedlevyrate_owninguser_valueNavigation)
                    .HasForeignKey(d => d._owninguser_value)
                    .HasConstraintName("FK_systemuser_ptas_aptadjustedlevyrate_owninguser");

                entity.HasOne(d => d._ptas_levycodeid_valueNavigation)
                    .WithMany(p => p.ptas_aptadjustedlevyrate)
                    .HasForeignKey(d => d._ptas_levycodeid_value)
                    .HasConstraintName("FK_ptas_levycode_ptas_aptadjustedlevyrate_ptas_levycodeid");
            });

            modelBuilder.Entity<ptas_aptavailablecomparablesale>(entity =>
            {
                entity.ToTable("ptas_aptavailablecomparablesale", "dynamics");

                entity.Property(e => e.ptas_aptavailablecomparablesaleid).ValueGeneratedNever();

                entity.Property(e => e.exchangerate).HasColumnType("money");

                entity.Property(e => e.ptas_airportnoiseadjustment).HasColumnType("money");

                entity.Property(e => e.ptas_caprate).HasColumnType("money");

                entity.Property(e => e.ptas_gim).HasColumnType("money");

                entity.Property(e => e.ptas_saleprice).HasColumnType("money");

                entity.Property(e => e.ptas_saleprice_base).HasColumnType("money");

                entity.Property(e => e.ptas_salepriceperunit).HasColumnType("money");

                entity.Property(e => e.ptas_salepriceperunit_base).HasColumnType("money");

                entity.Property(e => e.ptas_trendedsaleprice).HasColumnType("money");

                entity.Property(e => e.ptas_trendedsaleprice_base).HasColumnType("money");

                entity.Property(e => e.ptas_trendedsalepriceperunit).HasColumnType("money");

                entity.Property(e => e.ptas_trendedsalepriceperunit_base).HasColumnType("money");

                entity.Property(e => e.ptas_viewrank).HasColumnType("money");

                entity.HasOne(d => d._createdby_valueNavigation)
                    .WithMany(p => p.ptas_aptavailablecomparablesale_createdby_valueNavigation)
                    .HasForeignKey(d => d._createdby_value)
                    .HasConstraintName("FK_systemuser_ptas_aptavailablecomparablesale_createdby");

                entity.HasOne(d => d._createdonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_aptavailablecomparablesale_createdonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._createdonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_aptavailablecomparablesale_createdonbehalfby");

                entity.HasOne(d => d._modifiedby_valueNavigation)
                    .WithMany(p => p.ptas_aptavailablecomparablesale_modifiedby_valueNavigation)
                    .HasForeignKey(d => d._modifiedby_value)
                    .HasConstraintName("FK_systemuser_ptas_aptavailablecomparablesale_modifiedby");

                entity.HasOne(d => d._modifiedonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_aptavailablecomparablesale_modifiedonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._modifiedonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_aptavailablecomparablesale_modifiedonbehalfby");

                entity.HasOne(d => d._owningteam_valueNavigation)
                    .WithMany(p => p.ptas_aptavailablecomparablesale)
                    .HasForeignKey(d => d._owningteam_value)
                    .HasConstraintName("FK_team_ptas_aptavailablecomparablesale_owningteam");

                entity.HasOne(d => d._owninguser_valueNavigation)
                    .WithMany(p => p.ptas_aptavailablecomparablesale_owninguser_valueNavigation)
                    .HasForeignKey(d => d._owninguser_value)
                    .HasConstraintName("FK_systemuser_ptas_aptavailablecomparablesale_owninguser");

                entity.HasOne(d => d._ptas_parcelid_valueNavigation)
                    .WithMany(p => p.ptas_aptavailablecomparablesale)
                    .HasForeignKey(d => d._ptas_parcelid_value)
                    .HasConstraintName("FK_ptas_parceldetail_ptas_aptavailablecomparablesale_ptas_parcelid");

                entity.HasOne(d => d._ptas_propertytypeid_valueNavigation)
                    .WithMany(p => p.ptas_aptavailablecomparablesale)
                    .HasForeignKey(d => d._ptas_propertytypeid_value)
                    .HasConstraintName("FK_ptas_propertytype_ptas_aptavailablecomparablesale_ptas_propertytypeid");

                entity.HasOne(d => d._ptas_responsibilityid_valueNavigation)
                    .WithMany(p => p.ptas_aptavailablecomparablesale)
                    .HasForeignKey(d => d._ptas_responsibilityid_value)
                    .HasConstraintName("FK_ptas_responsibility_ptas_aptavailablecomparablesale_ptas_responsibilityid");

                entity.HasOne(d => d._ptas_saleid_valueNavigation)
                    .WithMany(p => p.ptas_aptavailablecomparablesale)
                    .HasForeignKey(d => d._ptas_saleid_value)
                    .HasConstraintName("FK_ptas_sales_ptas_aptavailablecomparablesale_ptas_saleid");

                entity.HasOne(d => d._ptas_specialtyareaid_valueNavigation)
                    .WithMany(p => p.ptas_aptavailablecomparablesale)
                    .HasForeignKey(d => d._ptas_specialtyareaid_value)
                    .HasConstraintName("FK_ptas_specialtyarea_ptas_aptavailablecomparablesale_ptas_specialtyareaid");
            });

            modelBuilder.Entity<ptas_aptbuildingqualityadjustment>(entity =>
            {
                entity.ToTable("ptas_aptbuildingqualityadjustment", "dynamics");

                entity.Property(e => e.ptas_aptbuildingqualityadjustmentid).ValueGeneratedNever();

                entity.Property(e => e.ptas_caprateadjustment).HasColumnType("money");

                entity.Property(e => e.ptas_commercialcapratemultiplier).HasColumnType("money");

                entity.Property(e => e.ptas_commercialgimmultiplier).HasColumnType("money");

                entity.Property(e => e.ptas_commercialrentmultiplier).HasColumnType("money");

                entity.Property(e => e.ptas_gimadjustment).HasColumnType("money");

                entity.Property(e => e.ptas_rentscoefficient).HasColumnType("money");

                entity.Property(e => e.ptas_salescoefficient).HasColumnType("money");

                entity.HasOne(d => d._createdby_valueNavigation)
                    .WithMany(p => p.ptas_aptbuildingqualityadjustment_createdby_valueNavigation)
                    .HasForeignKey(d => d._createdby_value)
                    .HasConstraintName("FK_systemuser_ptas_aptbuildingqualityadjustment_createdby");

                entity.HasOne(d => d._createdonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_aptbuildingqualityadjustment_createdonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._createdonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_aptbuildingqualityadjustment_createdonbehalfby");

                entity.HasOne(d => d._modifiedby_valueNavigation)
                    .WithMany(p => p.ptas_aptbuildingqualityadjustment_modifiedby_valueNavigation)
                    .HasForeignKey(d => d._modifiedby_value)
                    .HasConstraintName("FK_systemuser_ptas_aptbuildingqualityadjustment_modifiedby");

                entity.HasOne(d => d._modifiedonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_aptbuildingqualityadjustment_modifiedonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._modifiedonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_aptbuildingqualityadjustment_modifiedonbehalfby");

                entity.HasOne(d => d._owningteam_valueNavigation)
                    .WithMany(p => p.ptas_aptbuildingqualityadjustment)
                    .HasForeignKey(d => d._owningteam_value)
                    .HasConstraintName("FK_team_ptas_aptbuildingqualityadjustment_owningteam");

                entity.HasOne(d => d._owninguser_valueNavigation)
                    .WithMany(p => p.ptas_aptbuildingqualityadjustment_owninguser_valueNavigation)
                    .HasForeignKey(d => d._owninguser_value)
                    .HasConstraintName("FK_systemuser_ptas_aptbuildingqualityadjustment_owninguser");

                entity.HasOne(d => d._ptas_assessmentyearlookupid_valueNavigation)
                    .WithMany(p => p.ptas_aptbuildingqualityadjustment)
                    .HasForeignKey(d => d._ptas_assessmentyearlookupid_value)
                    .HasConstraintName("FK_ptas_year_ptas_aptbuildingqualityadjustment_ptas_assessmentyearlookupid");
            });

            modelBuilder.Entity<ptas_aptcloseproximity>(entity =>
            {
                entity.ToTable("ptas_aptcloseproximity", "dynamics");

                entity.Property(e => e.ptas_aptcloseproximityid).ValueGeneratedNever();

                entity.HasOne(d => d._createdby_valueNavigation)
                    .WithMany(p => p.ptas_aptcloseproximity_createdby_valueNavigation)
                    .HasForeignKey(d => d._createdby_value)
                    .HasConstraintName("FK_systemuser_ptas_aptcloseproximity_createdby");

                entity.HasOne(d => d._createdonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_aptcloseproximity_createdonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._createdonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_aptcloseproximity_createdonbehalfby");

                entity.HasOne(d => d._modifiedby_valueNavigation)
                    .WithMany(p => p.ptas_aptcloseproximity_modifiedby_valueNavigation)
                    .HasForeignKey(d => d._modifiedby_value)
                    .HasConstraintName("FK_systemuser_ptas_aptcloseproximity_modifiedby");

                entity.HasOne(d => d._modifiedonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_aptcloseproximity_modifiedonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._modifiedonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_aptcloseproximity_modifiedonbehalfby");

                entity.HasOne(d => d._ptas_assessmentyearlookupid_valueNavigation)
                    .WithMany(p => p.ptas_aptcloseproximity)
                    .HasForeignKey(d => d._ptas_assessmentyearlookupid_value)
                    .HasConstraintName("FK_ptas_year_ptas_aptcloseproximity_ptas_assessmentyearlookupid");

                entity.HasOne(d => d._ptas_salerentneighborhoodid_valueNavigation)
                    .WithMany(p => p.ptas_aptcloseproximity_ptas_salerentneighborhoodid_valueNavigation)
                    .HasForeignKey(d => d._ptas_salerentneighborhoodid_value)
                    .HasConstraintName("FK_ptas_aptneighborhood_ptas_aptcloseproximity_ptas_salerentneighborhoodid");

                entity.HasOne(d => d._ptas_subjectneighborhoodid_valueNavigation)
                    .WithMany(p => p.ptas_aptcloseproximity_ptas_subjectneighborhoodid_valueNavigation)
                    .HasForeignKey(d => d._ptas_subjectneighborhoodid_value)
                    .HasConstraintName("FK_ptas_aptneighborhood_ptas_aptcloseproximity_ptas_subjectneighborhoodid");
            });

            modelBuilder.Entity<ptas_aptcommercialincomeexpense>(entity =>
            {
                entity.ToTable("ptas_aptcommercialincomeexpense", "dynamics");

                entity.Property(e => e.ptas_aptcommercialincomeexpenseid).ValueGeneratedNever();

                entity.Property(e => e.exchangerate).HasColumnType("money");

                entity.Property(e => e.ptas_caprate).HasColumnType("money");

                entity.Property(e => e.ptas_effectivegrossincome).HasColumnType("money");

                entity.Property(e => e.ptas_effectivegrossincome_base).HasColumnType("money");

                entity.Property(e => e.ptas_incomevalue).HasColumnType("money");

                entity.Property(e => e.ptas_incomevalue_base).HasColumnType("money");

                entity.Property(e => e.ptas_netoperatingincome).HasColumnType("money");

                entity.Property(e => e.ptas_netoperatingincome_base).HasColumnType("money");

                entity.Property(e => e.ptas_operatingexpensefactor).HasColumnType("money");

                entity.Property(e => e.ptas_operatingexpenses).HasColumnType("money");

                entity.Property(e => e.ptas_operatingexpenses_base).HasColumnType("money");

                entity.Property(e => e.ptas_potentialgrossincome).HasColumnType("money");

                entity.Property(e => e.ptas_potentialgrossincome_base).HasColumnType("money");

                entity.Property(e => e.ptas_rentrate).HasColumnType("money");

                entity.Property(e => e.ptas_rentrate_base).HasColumnType("money");

                entity.Property(e => e.ptas_vacancyandcreditloss).HasColumnType("money");

                entity.HasOne(d => d._createdby_valueNavigation)
                    .WithMany(p => p.ptas_aptcommercialincomeexpense_createdby_valueNavigation)
                    .HasForeignKey(d => d._createdby_value)
                    .HasConstraintName("FK_systemuser_ptas_aptcommercialincomeexpense_createdby");

                entity.HasOne(d => d._createdonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_aptcommercialincomeexpense_createdonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._createdonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_aptcommercialincomeexpense_createdonbehalfby");

                entity.HasOne(d => d._modifiedby_valueNavigation)
                    .WithMany(p => p.ptas_aptcommercialincomeexpense_modifiedby_valueNavigation)
                    .HasForeignKey(d => d._modifiedby_value)
                    .HasConstraintName("FK_systemuser_ptas_aptcommercialincomeexpense_modifiedby");

                entity.HasOne(d => d._modifiedonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_aptcommercialincomeexpense_modifiedonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._modifiedonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_aptcommercialincomeexpense_modifiedonbehalfby");

                entity.HasOne(d => d._owningteam_valueNavigation)
                    .WithMany(p => p.ptas_aptcommercialincomeexpense)
                    .HasForeignKey(d => d._owningteam_value)
                    .HasConstraintName("FK_team_ptas_aptcommercialincomeexpense_owningteam");

                entity.HasOne(d => d._owninguser_valueNavigation)
                    .WithMany(p => p.ptas_aptcommercialincomeexpense_owninguser_valueNavigation)
                    .HasForeignKey(d => d._owninguser_value)
                    .HasConstraintName("FK_systemuser_ptas_aptcommercialincomeexpense_owninguser");

                entity.HasOne(d => d._ptas_aptvaluationid_valueNavigation)
                    .WithMany(p => p.ptas_aptcommercialincomeexpense)
                    .HasForeignKey(d => d._ptas_aptvaluationid_value)
                    .HasConstraintName("FK_ptas_aptvaluation_ptas_aptcommercialincomeexpense_ptas_aptvaluationid");

                entity.HasOne(d => d._ptas_sectionuseid_valueNavigation)
                    .WithMany(p => p.ptas_aptcommercialincomeexpense)
                    .HasForeignKey(d => d._ptas_sectionuseid_value)
                    .HasConstraintName("FK_ptas_buildingdetail_commercialuse_ptas_aptcommercialincomeexpense_ptas_sectionuseid");
            });

            modelBuilder.Entity<ptas_aptcomparablerent>(entity =>
            {
                entity.ToTable("ptas_aptcomparablerent", "dynamics");

                entity.Property(e => e.ptas_aptcomparablerentid).ValueGeneratedNever();

                entity.Property(e => e.exchangerate).HasColumnType("money");

                entity.Property(e => e.ptas_absoluteadjustsment).HasColumnType("money");

                entity.Property(e => e.ptas_adjustedrent).HasColumnType("money");

                entity.Property(e => e.ptas_adjustedrent_base).HasColumnType("money");

                entity.Property(e => e.ptas_ageadjustment).HasColumnType("money");

                entity.Property(e => e.ptas_airportnoiseadjustment).HasColumnType("money");

                entity.Property(e => e.ptas_conditionadjustment).HasColumnType("money");

                entity.Property(e => e.ptas_distancemetriccombinedadjustment).HasColumnType("money");

                entity.Property(e => e.ptas_locationadjustment).HasColumnType("money");

                entity.Property(e => e.ptas_netadjustment).HasColumnType("money");

                entity.Property(e => e.ptas_numberofbathroomsadjustment).HasColumnType("money");

                entity.Property(e => e.ptas_numberofbedroomsadjustment).HasColumnType("money");

                entity.Property(e => e.ptas_pooladjustment).HasColumnType("money");

                entity.Property(e => e.ptas_proximityadjustment).HasColumnType("money");

                entity.Property(e => e.ptas_qualityadjustment).HasColumnType("money");

                entity.Property(e => e.ptas_unitsizeadjustment1).HasColumnType("money");

                entity.Property(e => e.ptas_unitsizeadjustment2).HasColumnType("money");

                entity.Property(e => e.ptas_unittypeadjustment).HasColumnType("money");

                entity.Property(e => e.ptas_viewadjustment1).HasColumnType("money");

                entity.Property(e => e.ptas_viewadjustment2).HasColumnType("money");

                entity.Property(e => e.ptas_weightingdenominator).HasColumnType("money");

                entity.Property(e => e.ptas_yearbuiltadjustment).HasColumnType("money");

                entity.HasOne(d => d._createdby_valueNavigation)
                    .WithMany(p => p.ptas_aptcomparablerent_createdby_valueNavigation)
                    .HasForeignKey(d => d._createdby_value)
                    .HasConstraintName("FK_systemuser_ptas_aptcomparablerent_createdby");

                entity.HasOne(d => d._createdonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_aptcomparablerent_createdonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._createdonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_aptcomparablerent_createdonbehalfby");

                entity.HasOne(d => d._modifiedby_valueNavigation)
                    .WithMany(p => p.ptas_aptcomparablerent_modifiedby_valueNavigation)
                    .HasForeignKey(d => d._modifiedby_value)
                    .HasConstraintName("FK_systemuser_ptas_aptcomparablerent_modifiedby");

                entity.HasOne(d => d._modifiedonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_aptcomparablerent_modifiedonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._modifiedonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_aptcomparablerent_modifiedonbehalfby");

                entity.HasOne(d => d._owningteam_valueNavigation)
                    .WithMany(p => p.ptas_aptcomparablerent)
                    .HasForeignKey(d => d._owningteam_value)
                    .HasConstraintName("FK_team_ptas_aptcomparablerent_owningteam");

                entity.HasOne(d => d._owninguser_valueNavigation)
                    .WithMany(p => p.ptas_aptcomparablerent_owninguser_valueNavigation)
                    .HasForeignKey(d => d._owninguser_value)
                    .HasConstraintName("FK_systemuser_ptas_aptcomparablerent_owninguser");

                entity.HasOne(d => d._ptas_rentid_valueNavigation)
                    .WithMany(p => p.ptas_aptcomparablerent)
                    .HasForeignKey(d => d._ptas_rentid_value)
                    .HasConstraintName("FK_ptas_aptlistedrent_ptas_aptcomparablerent_ptas_rentid");

                entity.HasOne(d => d._ptas_rentsubjectid_valueNavigation)
                    .WithMany(p => p.ptas_aptcomparablerent)
                    .HasForeignKey(d => d._ptas_rentsubjectid_value)
                    .HasConstraintName("FK_ptas_aptvaluation_ptas_aptcomparablerent_ptas_rentsubjectid");
            });

            modelBuilder.Entity<ptas_aptcomparablesale>(entity =>
            {
                entity.ToTable("ptas_aptcomparablesale", "dynamics");

                entity.Property(e => e.ptas_aptcomparablesaleid).ValueGeneratedNever();

                entity.Property(e => e.exchangerate).HasColumnType("money");

                entity.Property(e => e.ptas_absoluteadjustmentwithoutlocation).HasColumnType("money");

                entity.Property(e => e.ptas_absoluteadjustsment).HasColumnType("money");

                entity.Property(e => e.ptas_adjustedsaleprice).HasColumnType("money");

                entity.Property(e => e.ptas_adjustedsaleprice_base).HasColumnType("money");

                entity.Property(e => e.ptas_adjustedsalepriceperunit).HasColumnType("money");

                entity.Property(e => e.ptas_adjustedsalepriceperunit_base).HasColumnType("money");

                entity.Property(e => e.ptas_ageadjustment).HasColumnType("money");

                entity.Property(e => e.ptas_aggregateabsoluteadjustment).HasColumnType("money");

                entity.Property(e => e.ptas_airportnoiseadjustment).HasColumnType("money");

                entity.Property(e => e.ptas_averageunitsizeadjustment).HasColumnType("money");

                entity.Property(e => e.ptas_buildingqualityadjustment).HasColumnType("money");

                entity.Property(e => e.ptas_complementofabsoluteadjustment).HasColumnType("money");

                entity.Property(e => e.ptas_compweight).HasColumnType("money");

                entity.Property(e => e.ptas_conditionadjustment).HasColumnType("money");

                entity.Property(e => e.ptas_distancemetriccombinedadjustment).HasColumnType("money");

                entity.Property(e => e.ptas_locationadjustment).HasColumnType("money");

                entity.Property(e => e.ptas_netadjustment).HasColumnType("money");

                entity.Property(e => e.ptas_netadjustmentwithoutlocation).HasColumnType("money");

                entity.Property(e => e.ptas_numberofunitsadjustment).HasColumnType("money");

                entity.Property(e => e.ptas_percentcommercialadjustment).HasColumnType("money");

                entity.Property(e => e.ptas_pooladjustment).HasColumnType("money");

                entity.Property(e => e.ptas_proximitycodeadjustment).HasColumnType("money");

                entity.Property(e => e.ptas_reconciledcomparablevalueperunit).HasColumnType("money");

                entity.Property(e => e.ptas_reconciledcomparablevalueperunit_base).HasColumnType("money");

                entity.Property(e => e.ptas_saledateadjustment).HasColumnType("money");

                entity.Property(e => e.ptas_trendedsalepriceperunit).HasColumnType("money");

                entity.Property(e => e.ptas_trendedsalepriceperunit_base).HasColumnType("money");

                entity.Property(e => e.ptas_viewadjustment).HasColumnType("money");

                entity.HasOne(d => d._createdby_valueNavigation)
                    .WithMany(p => p.ptas_aptcomparablesale_createdby_valueNavigation)
                    .HasForeignKey(d => d._createdby_value)
                    .HasConstraintName("FK_systemuser_ptas_aptcomparablesale_createdby");

                entity.HasOne(d => d._createdonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_aptcomparablesale_createdonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._createdonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_aptcomparablesale_createdonbehalfby");

                entity.HasOne(d => d._modifiedby_valueNavigation)
                    .WithMany(p => p.ptas_aptcomparablesale_modifiedby_valueNavigation)
                    .HasForeignKey(d => d._modifiedby_value)
                    .HasConstraintName("FK_systemuser_ptas_aptcomparablesale_modifiedby");

                entity.HasOne(d => d._modifiedonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_aptcomparablesale_modifiedonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._modifiedonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_aptcomparablesale_modifiedonbehalfby");

                entity.HasOne(d => d._owningteam_valueNavigation)
                    .WithMany(p => p.ptas_aptcomparablesale)
                    .HasForeignKey(d => d._owningteam_value)
                    .HasConstraintName("FK_team_ptas_aptcomparablesale_owningteam");

                entity.HasOne(d => d._owninguser_valueNavigation)
                    .WithMany(p => p.ptas_aptcomparablesale_owninguser_valueNavigation)
                    .HasForeignKey(d => d._owninguser_value)
                    .HasConstraintName("FK_systemuser_ptas_aptcomparablesale_owninguser");

                entity.HasOne(d => d._ptas_comparablesalesvaluationsubjectid_valueNavigation)
                    .WithMany(p => p.ptas_aptcomparablesale)
                    .HasForeignKey(d => d._ptas_comparablesalesvaluationsubjectid_value)
                    .HasConstraintName("FK_ptas_aptvaluation_ptas_aptcomparablesale_ptas_comparablesalesvaluationsubjectid");

                entity.HasOne(d => d._ptas_saleid_valueNavigation)
                    .WithMany(p => p.ptas_aptcomparablesale)
                    .HasForeignKey(d => d._ptas_saleid_value)
                    .HasConstraintName("FK_ptas_aptavailablecomparablesale_ptas_aptcomparablesale_ptas_saleid");
            });

            modelBuilder.Entity<ptas_aptconditionadjustment>(entity =>
            {
                entity.ToTable("ptas_aptconditionadjustment", "dynamics");

                entity.Property(e => e.ptas_aptconditionadjustmentid).ValueGeneratedNever();

                entity.Property(e => e.ptas_caprateadjustment).HasColumnType("money");

                entity.Property(e => e.ptas_gimadjustment).HasColumnType("money");

                entity.Property(e => e.ptas_rentscoefficient).HasColumnType("money");

                entity.Property(e => e.ptas_salescoefficient).HasColumnType("money");

                entity.HasOne(d => d._createdby_valueNavigation)
                    .WithMany(p => p.ptas_aptconditionadjustment_createdby_valueNavigation)
                    .HasForeignKey(d => d._createdby_value)
                    .HasConstraintName("FK_systemuser_ptas_aptconditionadjustment_createdby");

                entity.HasOne(d => d._createdonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_aptconditionadjustment_createdonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._createdonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_aptconditionadjustment_createdonbehalfby");

                entity.HasOne(d => d._modifiedby_valueNavigation)
                    .WithMany(p => p.ptas_aptconditionadjustment_modifiedby_valueNavigation)
                    .HasForeignKey(d => d._modifiedby_value)
                    .HasConstraintName("FK_systemuser_ptas_aptconditionadjustment_modifiedby");

                entity.HasOne(d => d._modifiedonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_aptconditionadjustment_modifiedonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._modifiedonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_aptconditionadjustment_modifiedonbehalfby");

                entity.HasOne(d => d._owningteam_valueNavigation)
                    .WithMany(p => p.ptas_aptconditionadjustment)
                    .HasForeignKey(d => d._owningteam_value)
                    .HasConstraintName("FK_team_ptas_aptconditionadjustment_owningteam");

                entity.HasOne(d => d._owninguser_valueNavigation)
                    .WithMany(p => p.ptas_aptconditionadjustment_owninguser_valueNavigation)
                    .HasForeignKey(d => d._owninguser_value)
                    .HasConstraintName("FK_systemuser_ptas_aptconditionadjustment_owninguser");

                entity.HasOne(d => d._ptas_assessmentyearlookupid_valueNavigation)
                    .WithMany(p => p.ptas_aptconditionadjustment)
                    .HasForeignKey(d => d._ptas_assessmentyearlookupid_value)
                    .HasConstraintName("FK_ptas_year_ptas_aptconditionadjustment_ptas_assessmentyearlookupid");
            });

            modelBuilder.Entity<ptas_aptestimatedunitsqft>(entity =>
            {
                entity.ToTable("ptas_aptestimatedunitsqft", "dynamics");

                entity.Property(e => e.ptas_aptestimatedunitsqftid).ValueGeneratedNever();

                entity.HasOne(d => d._createdby_valueNavigation)
                    .WithMany(p => p.ptas_aptestimatedunitsqft_createdby_valueNavigation)
                    .HasForeignKey(d => d._createdby_value)
                    .HasConstraintName("FK_systemuser_ptas_aptestimatedunitsqft_createdby");

                entity.HasOne(d => d._createdonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_aptestimatedunitsqft_createdonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._createdonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_aptestimatedunitsqft_createdonbehalfby");

                entity.HasOne(d => d._modifiedby_valueNavigation)
                    .WithMany(p => p.ptas_aptestimatedunitsqft_modifiedby_valueNavigation)
                    .HasForeignKey(d => d._modifiedby_value)
                    .HasConstraintName("FK_systemuser_ptas_aptestimatedunitsqft_modifiedby");

                entity.HasOne(d => d._modifiedonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_aptestimatedunitsqft_modifiedonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._modifiedonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_aptestimatedunitsqft_modifiedonbehalfby");

                entity.HasOne(d => d._owningteam_valueNavigation)
                    .WithMany(p => p.ptas_aptestimatedunitsqft)
                    .HasForeignKey(d => d._owningteam_value)
                    .HasConstraintName("FK_team_ptas_aptestimatedunitsqft_owningteam");

                entity.HasOne(d => d._owninguser_valueNavigation)
                    .WithMany(p => p.ptas_aptestimatedunitsqft_owninguser_valueNavigation)
                    .HasForeignKey(d => d._owninguser_value)
                    .HasConstraintName("FK_systemuser_ptas_aptestimatedunitsqft_owninguser");

                entity.HasOne(d => d._ptas_assessmentyearlookupid_valueNavigation)
                    .WithMany(p => p.ptas_aptestimatedunitsqft)
                    .HasForeignKey(d => d._ptas_assessmentyearlookupid_value)
                    .HasConstraintName("FK_ptas_year_ptas_aptestimatedunitsqft_ptas_assessmentyearlookupid");
            });

            modelBuilder.Entity<ptas_aptexpensehighend>(entity =>
            {
                entity.ToTable("ptas_aptexpensehighend", "dynamics");

                entity.Property(e => e.ptas_aptexpensehighendid).ValueGeneratedNever();

                entity.Property(e => e.ptas_highendexpense).HasColumnType("money");

                entity.HasOne(d => d._createdby_valueNavigation)
                    .WithMany(p => p.ptas_aptexpensehighend_createdby_valueNavigation)
                    .HasForeignKey(d => d._createdby_value)
                    .HasConstraintName("FK_systemuser_ptas_aptexpensehighend_createdby");

                entity.HasOne(d => d._createdonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_aptexpensehighend_createdonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._createdonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_aptexpensehighend_createdonbehalfby");

                entity.HasOne(d => d._modifiedby_valueNavigation)
                    .WithMany(p => p.ptas_aptexpensehighend_modifiedby_valueNavigation)
                    .HasForeignKey(d => d._modifiedby_value)
                    .HasConstraintName("FK_systemuser_ptas_aptexpensehighend_modifiedby");

                entity.HasOne(d => d._modifiedonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_aptexpensehighend_modifiedonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._modifiedonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_aptexpensehighend_modifiedonbehalfby");

                entity.HasOne(d => d._owningteam_valueNavigation)
                    .WithMany(p => p.ptas_aptexpensehighend)
                    .HasForeignKey(d => d._owningteam_value)
                    .HasConstraintName("FK_team_ptas_aptexpensehighend_owningteam");

                entity.HasOne(d => d._owninguser_valueNavigation)
                    .WithMany(p => p.ptas_aptexpensehighend_owninguser_valueNavigation)
                    .HasForeignKey(d => d._owninguser_value)
                    .HasConstraintName("FK_systemuser_ptas_aptexpensehighend_owninguser");

                entity.HasOne(d => d._ptas_assessmentyearlookupid_valueNavigation)
                    .WithMany(p => p.ptas_aptexpensehighend)
                    .HasForeignKey(d => d._ptas_assessmentyearlookupid_value)
                    .HasConstraintName("FK_ptas_year_ptas_aptexpensehighend_ptas_assessmentyearlookupid");
            });

            modelBuilder.Entity<ptas_aptexpenseunitsize>(entity =>
            {
                entity.ToTable("ptas_aptexpenseunitsize", "dynamics");

                entity.Property(e => e.ptas_aptexpenseunitsizeid).ValueGeneratedNever();

                entity.Property(e => e.ptas_unitsizeexpense).HasColumnType("money");

                entity.HasOne(d => d._createdby_valueNavigation)
                    .WithMany(p => p.ptas_aptexpenseunitsize_createdby_valueNavigation)
                    .HasForeignKey(d => d._createdby_value)
                    .HasConstraintName("FK_systemuser_ptas_aptexpenseunitsize_createdby");

                entity.HasOne(d => d._createdonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_aptexpenseunitsize_createdonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._createdonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_aptexpenseunitsize_createdonbehalfby");

                entity.HasOne(d => d._modifiedby_valueNavigation)
                    .WithMany(p => p.ptas_aptexpenseunitsize_modifiedby_valueNavigation)
                    .HasForeignKey(d => d._modifiedby_value)
                    .HasConstraintName("FK_systemuser_ptas_aptexpenseunitsize_modifiedby");

                entity.HasOne(d => d._modifiedonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_aptexpenseunitsize_modifiedonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._modifiedonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_aptexpenseunitsize_modifiedonbehalfby");

                entity.HasOne(d => d._owningteam_valueNavigation)
                    .WithMany(p => p.ptas_aptexpenseunitsize)
                    .HasForeignKey(d => d._owningteam_value)
                    .HasConstraintName("FK_team_ptas_aptexpenseunitsize_owningteam");

                entity.HasOne(d => d._owninguser_valueNavigation)
                    .WithMany(p => p.ptas_aptexpenseunitsize_owninguser_valueNavigation)
                    .HasForeignKey(d => d._owninguser_value)
                    .HasConstraintName("FK_systemuser_ptas_aptexpenseunitsize_owninguser");

                entity.HasOne(d => d._ptas_assessmentyearlookupid_valueNavigation)
                    .WithMany(p => p.ptas_aptexpenseunitsize)
                    .HasForeignKey(d => d._ptas_assessmentyearlookupid_value)
                    .HasConstraintName("FK_ptas_year_ptas_aptexpenseunitsize_ptas_assessmentyearlookupid");
            });

            modelBuilder.Entity<ptas_aptincomeexpense>(entity =>
            {
                entity.ToTable("ptas_aptincomeexpense", "dynamics");

                entity.Property(e => e.ptas_aptincomeexpenseid).ValueGeneratedNever();

                entity.Property(e => e.exchangerate).HasColumnType("money");

                entity.Property(e => e.ptas_basecaprate).HasColumnType("money");

                entity.Property(e => e.ptas_baseexpense).HasColumnType("money");

                entity.Property(e => e.ptas_baseexpense_base).HasColumnType("money");

                entity.Property(e => e.ptas_basegim).HasColumnType("money");

                entity.Property(e => e.ptas_basepercentexpense).HasColumnType("money");

                entity.Property(e => e.ptas_commercialgim).HasColumnType("money");

                entity.Property(e => e.ptas_otherincome).HasColumnType("money");

                entity.Property(e => e.ptas_otherincome_base).HasColumnType("money");

                entity.HasOne(d => d._createdby_valueNavigation)
                    .WithMany(p => p.ptas_aptincomeexpense_createdby_valueNavigation)
                    .HasForeignKey(d => d._createdby_value)
                    .HasConstraintName("FK_systemuser_ptas_aptincomeexpense_createdby");

                entity.HasOne(d => d._createdonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_aptincomeexpense_createdonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._createdonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_aptincomeexpense_createdonbehalfby");

                entity.HasOne(d => d._modifiedby_valueNavigation)
                    .WithMany(p => p.ptas_aptincomeexpense_modifiedby_valueNavigation)
                    .HasForeignKey(d => d._modifiedby_value)
                    .HasConstraintName("FK_systemuser_ptas_aptincomeexpense_modifiedby");

                entity.HasOne(d => d._modifiedonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_aptincomeexpense_modifiedonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._modifiedonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_aptincomeexpense_modifiedonbehalfby");

                entity.HasOne(d => d._ptas_apartmentneighborhoodid_valueNavigation)
                    .WithMany(p => p.ptas_aptincomeexpense)
                    .HasForeignKey(d => d._ptas_apartmentneighborhoodid_value)
                    .HasConstraintName("FK_ptas_aptneighborhood_ptas_aptincomeexpense_ptas_apartmentneighborhoodid");

                entity.HasOne(d => d._ptas_assessmentyearlookupid_valueNavigation)
                    .WithMany(p => p.ptas_aptincomeexpense)
                    .HasForeignKey(d => d._ptas_assessmentyearlookupid_value)
                    .HasConstraintName("FK_ptas_year_ptas_aptincomeexpense_ptas_assessmentyearlookupid");

                entity.HasOne(d => d._ptas_supergroupid_valueNavigation)
                    .WithMany(p => p.ptas_aptincomeexpense)
                    .HasForeignKey(d => d._ptas_supergroupid_value)
                    .HasConstraintName("FK_ptas_supergroup_ptas_aptincomeexpense_ptas_supergroupid");
            });

            modelBuilder.Entity<ptas_aptlistedrent>(entity =>
            {
                entity.ToTable("ptas_aptlistedrent", "dynamics");

                entity.Property(e => e.ptas_aptlistedrentid).ValueGeneratedNever();

                entity.Property(e => e.exchangerate).HasColumnType("money");

                entity.Property(e => e.ptas_trendedrent).HasColumnType("money");

                entity.Property(e => e.ptas_trendedrent_base).HasColumnType("money");

                entity.Property(e => e.ptas_typicalrent).HasColumnType("money");

                entity.Property(e => e.ptas_typicalrent_base).HasColumnType("money");

                entity.HasOne(d => d._createdby_valueNavigation)
                    .WithMany(p => p.ptas_aptlistedrent_createdby_valueNavigation)
                    .HasForeignKey(d => d._createdby_value)
                    .HasConstraintName("FK_systemuser_ptas_aptlistedrent_createdby");

                entity.HasOne(d => d._createdonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_aptlistedrent_createdonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._createdonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_aptlistedrent_createdonbehalfby");

                entity.HasOne(d => d._modifiedby_valueNavigation)
                    .WithMany(p => p.ptas_aptlistedrent_modifiedby_valueNavigation)
                    .HasForeignKey(d => d._modifiedby_value)
                    .HasConstraintName("FK_systemuser_ptas_aptlistedrent_modifiedby");

                entity.HasOne(d => d._modifiedonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_aptlistedrent_modifiedonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._modifiedonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_aptlistedrent_modifiedonbehalfby");

                entity.HasOne(d => d._owningteam_valueNavigation)
                    .WithMany(p => p.ptas_aptlistedrent)
                    .HasForeignKey(d => d._owningteam_value)
                    .HasConstraintName("FK_team_ptas_aptlistedrent_owningteam");

                entity.HasOne(d => d._owninguser_valueNavigation)
                    .WithMany(p => p.ptas_aptlistedrent_owninguser_valueNavigation)
                    .HasForeignKey(d => d._owninguser_value)
                    .HasConstraintName("FK_systemuser_ptas_aptlistedrent_owninguser");

                entity.HasOne(d => d._ptas_neighborhoodid_valueNavigation)
                    .WithMany(p => p.ptas_aptlistedrent)
                    .HasForeignKey(d => d._ptas_neighborhoodid_value)
                    .HasConstraintName("FK_ptas_neighborhood_ptas_aptlistedrent_ptas_neighborhoodid");

                entity.HasOne(d => d._ptas_parceld_valueNavigation)
                    .WithMany(p => p.ptas_aptlistedrent)
                    .HasForeignKey(d => d._ptas_parceld_value)
                    .HasConstraintName("FK_ptas_parceldetail_ptas_aptlistedrent_ptas_parceld");

                entity.HasOne(d => d._ptas_projectid_valueNavigation)
                    .WithMany(p => p.ptas_aptlistedrent)
                    .HasForeignKey(d => d._ptas_projectid_value)
                    .HasConstraintName("FK_ptas_condocomplex_ptas_aptlistedrent_ptas_projectid");
            });

            modelBuilder.Entity<ptas_aptneighborhood>(entity =>
            {
                entity.ToTable("ptas_aptneighborhood", "dynamics");

                entity.Property(e => e.ptas_aptneighborhoodid).ValueGeneratedNever();

                entity.Property(e => e.exchangerate).HasColumnType("money");

                entity.Property(e => e.ptas_parkingcoveredsecuredrent).HasColumnType("money");

                entity.Property(e => e.ptas_parkingcoveredsecuredrent_base).HasColumnType("money");

                entity.HasOne(d => d._createdby_valueNavigation)
                    .WithMany(p => p.ptas_aptneighborhood_createdby_valueNavigation)
                    .HasForeignKey(d => d._createdby_value)
                    .HasConstraintName("FK_systemuser_ptas_aptneighborhood_createdby");

                entity.HasOne(d => d._createdonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_aptneighborhood_createdonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._createdonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_aptneighborhood_createdonbehalfby");

                entity.HasOne(d => d._modifiedby_valueNavigation)
                    .WithMany(p => p.ptas_aptneighborhood_modifiedby_valueNavigation)
                    .HasForeignKey(d => d._modifiedby_value)
                    .HasConstraintName("FK_systemuser_ptas_aptneighborhood_modifiedby");

                entity.HasOne(d => d._modifiedonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_aptneighborhood_modifiedonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._modifiedonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_aptneighborhood_modifiedonbehalfby");

                entity.HasOne(d => d._ptas_assessmentyearlookupid_valueNavigation)
                    .WithMany(p => p.ptas_aptneighborhood)
                    .HasForeignKey(d => d._ptas_assessmentyearlookupid_value)
                    .HasConstraintName("FK_ptas_year_ptas_aptneighborhood_ptas_assessmentyearlookupid");
            });

            modelBuilder.Entity<ptas_aptnumberofunitsadjustment>(entity =>
            {
                entity.ToTable("ptas_aptnumberofunitsadjustment", "dynamics");

                entity.Property(e => e.ptas_aptnumberofunitsadjustmentid).ValueGeneratedNever();

                entity.Property(e => e.ptas_rentscoefficient).HasColumnType("money");

                entity.Property(e => e.ptas_salescoefficient).HasColumnType("money");

                entity.HasOne(d => d._createdby_valueNavigation)
                    .WithMany(p => p.ptas_aptnumberofunitsadjustment_createdby_valueNavigation)
                    .HasForeignKey(d => d._createdby_value)
                    .HasConstraintName("FK_systemuser_ptas_aptnumberofunitsadjustment_createdby");

                entity.HasOne(d => d._createdonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_aptnumberofunitsadjustment_createdonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._createdonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_aptnumberofunitsadjustment_createdonbehalfby");

                entity.HasOne(d => d._modifiedby_valueNavigation)
                    .WithMany(p => p.ptas_aptnumberofunitsadjustment_modifiedby_valueNavigation)
                    .HasForeignKey(d => d._modifiedby_value)
                    .HasConstraintName("FK_systemuser_ptas_aptnumberofunitsadjustment_modifiedby");

                entity.HasOne(d => d._modifiedonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_aptnumberofunitsadjustment_modifiedonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._modifiedonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_aptnumberofunitsadjustment_modifiedonbehalfby");

                entity.HasOne(d => d._owningteam_valueNavigation)
                    .WithMany(p => p.ptas_aptnumberofunitsadjustment)
                    .HasForeignKey(d => d._owningteam_value)
                    .HasConstraintName("FK_team_ptas_aptnumberofunitsadjustment_owningteam");

                entity.HasOne(d => d._owninguser_valueNavigation)
                    .WithMany(p => p.ptas_aptnumberofunitsadjustment_owninguser_valueNavigation)
                    .HasForeignKey(d => d._owninguser_value)
                    .HasConstraintName("FK_systemuser_ptas_aptnumberofunitsadjustment_owninguser");

                entity.HasOne(d => d._ptas_assessmentyearlookupid_valueNavigation)
                    .WithMany(p => p.ptas_aptnumberofunitsadjustment)
                    .HasForeignKey(d => d._ptas_assessmentyearlookupid_value)
                    .HasConstraintName("FK_ptas_year_ptas_aptnumberofunitsadjustment_ptas_assessmentyearlookupid");
            });

            modelBuilder.Entity<ptas_aptpoolandelevatorexpense>(entity =>
            {
                entity.ToTable("ptas_aptpoolandelevatorexpense", "dynamics");

                entity.Property(e => e.ptas_aptpoolandelevatorexpenseid).ValueGeneratedNever();

                entity.Property(e => e.ptas_elevatorpercentexpense).HasColumnType("money");

                entity.Property(e => e.ptas_poolpercentexpense).HasColumnType("money");

                entity.HasOne(d => d._createdby_valueNavigation)
                    .WithMany(p => p.ptas_aptpoolandelevatorexpense_createdby_valueNavigation)
                    .HasForeignKey(d => d._createdby_value)
                    .HasConstraintName("FK_systemuser_ptas_aptpoolandelevatorexpense_createdby");

                entity.HasOne(d => d._createdonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_aptpoolandelevatorexpense_createdonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._createdonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_aptpoolandelevatorexpense_createdonbehalfby");

                entity.HasOne(d => d._modifiedby_valueNavigation)
                    .WithMany(p => p.ptas_aptpoolandelevatorexpense_modifiedby_valueNavigation)
                    .HasForeignKey(d => d._modifiedby_value)
                    .HasConstraintName("FK_systemuser_ptas_aptpoolandelevatorexpense_modifiedby");

                entity.HasOne(d => d._modifiedonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_aptpoolandelevatorexpense_modifiedonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._modifiedonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_aptpoolandelevatorexpense_modifiedonbehalfby");

                entity.HasOne(d => d._owningteam_valueNavigation)
                    .WithMany(p => p.ptas_aptpoolandelevatorexpense)
                    .HasForeignKey(d => d._owningteam_value)
                    .HasConstraintName("FK_team_ptas_aptpoolandelevatorexpense_owningteam");

                entity.HasOne(d => d._owninguser_valueNavigation)
                    .WithMany(p => p.ptas_aptpoolandelevatorexpense_owninguser_valueNavigation)
                    .HasForeignKey(d => d._owninguser_value)
                    .HasConstraintName("FK_systemuser_ptas_aptpoolandelevatorexpense_owninguser");

                entity.HasOne(d => d._ptas_assessmentyearlookupid_valueNavigation)
                    .WithMany(p => p.ptas_aptpoolandelevatorexpense)
                    .HasForeignKey(d => d._ptas_assessmentyearlookupid_value)
                    .HasConstraintName("FK_ptas_year_ptas_aptpoolandelevatorexpense_ptas_assessmentyearlookupid");
            });

            modelBuilder.Entity<ptas_aptrentmodel>(entity =>
            {
                entity.ToTable("ptas_aptrentmodel", "dynamics");

                entity.Property(e => e.ptas_aptrentmodelid).ValueGeneratedNever();

                entity.Property(e => e.ptas_coefficient).HasColumnType("money");

                entity.HasOne(d => d._createdby_valueNavigation)
                    .WithMany(p => p.ptas_aptrentmodel_createdby_valueNavigation)
                    .HasForeignKey(d => d._createdby_value)
                    .HasConstraintName("FK_systemuser_ptas_aptrentmodel_createdby");

                entity.HasOne(d => d._createdonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_aptrentmodel_createdonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._createdonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_aptrentmodel_createdonbehalfby");

                entity.HasOne(d => d._modifiedby_valueNavigation)
                    .WithMany(p => p.ptas_aptrentmodel_modifiedby_valueNavigation)
                    .HasForeignKey(d => d._modifiedby_value)
                    .HasConstraintName("FK_systemuser_ptas_aptrentmodel_modifiedby");

                entity.HasOne(d => d._modifiedonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_aptrentmodel_modifiedonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._modifiedonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_aptrentmodel_modifiedonbehalfby");

                entity.HasOne(d => d._owningteam_valueNavigation)
                    .WithMany(p => p.ptas_aptrentmodel)
                    .HasForeignKey(d => d._owningteam_value)
                    .HasConstraintName("FK_team_ptas_aptrentmodel_owningteam");

                entity.HasOne(d => d._owninguser_valueNavigation)
                    .WithMany(p => p.ptas_aptrentmodel_owninguser_valueNavigation)
                    .HasForeignKey(d => d._owninguser_value)
                    .HasConstraintName("FK_systemuser_ptas_aptrentmodel_owninguser");

                entity.HasOne(d => d._ptas_assessmentyearlookupid_valueNavigation)
                    .WithMany(p => p.ptas_aptrentmodel)
                    .HasForeignKey(d => d._ptas_assessmentyearlookupid_value)
                    .HasConstraintName("FK_ptas_year_ptas_aptrentmodel_ptas_assessmentyearlookupid");
            });

            modelBuilder.Entity<ptas_aptsalesmodel>(entity =>
            {
                entity.ToTable("ptas_aptsalesmodel", "dynamics");

                entity.Property(e => e.ptas_aptsalesmodelid).ValueGeneratedNever();

                entity.Property(e => e.ptas_coefficient).HasColumnType("money");

                entity.HasOne(d => d._createdby_valueNavigation)
                    .WithMany(p => p.ptas_aptsalesmodel_createdby_valueNavigation)
                    .HasForeignKey(d => d._createdby_value)
                    .HasConstraintName("FK_systemuser_ptas_aptsalesmodel_createdby");

                entity.HasOne(d => d._createdonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_aptsalesmodel_createdonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._createdonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_aptsalesmodel_createdonbehalfby");

                entity.HasOne(d => d._modifiedby_valueNavigation)
                    .WithMany(p => p.ptas_aptsalesmodel_modifiedby_valueNavigation)
                    .HasForeignKey(d => d._modifiedby_value)
                    .HasConstraintName("FK_systemuser_ptas_aptsalesmodel_modifiedby");

                entity.HasOne(d => d._modifiedonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_aptsalesmodel_modifiedonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._modifiedonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_aptsalesmodel_modifiedonbehalfby");

                entity.HasOne(d => d._owningteam_valueNavigation)
                    .WithMany(p => p.ptas_aptsalesmodel)
                    .HasForeignKey(d => d._owningteam_value)
                    .HasConstraintName("FK_team_ptas_aptsalesmodel_owningteam");

                entity.HasOne(d => d._owninguser_valueNavigation)
                    .WithMany(p => p.ptas_aptsalesmodel_owninguser_valueNavigation)
                    .HasForeignKey(d => d._owninguser_value)
                    .HasConstraintName("FK_systemuser_ptas_aptsalesmodel_owninguser");
            });

            modelBuilder.Entity<ptas_apttrending>(entity =>
            {
                entity.ToTable("ptas_apttrending", "dynamics");

                entity.Property(e => e.ptas_apttrendingid).ValueGeneratedNever();

                entity.Property(e => e.ptas_rentcoefficient).HasColumnType("money");

                entity.Property(e => e.ptas_salecoefficient).HasColumnType("money");

                entity.HasOne(d => d._createdby_valueNavigation)
                    .WithMany(p => p.ptas_apttrending_createdby_valueNavigation)
                    .HasForeignKey(d => d._createdby_value)
                    .HasConstraintName("FK_systemuser_ptas_apttrending_createdby");

                entity.HasOne(d => d._createdonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_apttrending_createdonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._createdonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_apttrending_createdonbehalfby");

                entity.HasOne(d => d._modifiedby_valueNavigation)
                    .WithMany(p => p.ptas_apttrending_modifiedby_valueNavigation)
                    .HasForeignKey(d => d._modifiedby_value)
                    .HasConstraintName("FK_systemuser_ptas_apttrending_modifiedby");

                entity.HasOne(d => d._modifiedonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_apttrending_modifiedonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._modifiedonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_apttrending_modifiedonbehalfby");

                entity.HasOne(d => d._owningteam_valueNavigation)
                    .WithMany(p => p.ptas_apttrending)
                    .HasForeignKey(d => d._owningteam_value)
                    .HasConstraintName("FK_team_ptas_apttrending_owningteam");

                entity.HasOne(d => d._owninguser_valueNavigation)
                    .WithMany(p => p.ptas_apttrending_owninguser_valueNavigation)
                    .HasForeignKey(d => d._owninguser_value)
                    .HasConstraintName("FK_systemuser_ptas_apttrending_owninguser");

                entity.HasOne(d => d._ptas_assessmentyearlookupid_valueNavigation)
                    .WithMany(p => p.ptas_apttrending)
                    .HasForeignKey(d => d._ptas_assessmentyearlookupid_value)
                    .HasConstraintName("FK_ptas_year_ptas_apttrending_ptas_assessmentyearlookupid");
            });

            modelBuilder.Entity<ptas_aptunittypeadjustment>(entity =>
            {
                entity.ToTable("ptas_aptunittypeadjustment", "dynamics");

                entity.Property(e => e.ptas_aptunittypeadjustmentid).ValueGeneratedNever();

                entity.Property(e => e.ptas_apartmentunittyperentcoefficient).HasColumnType("money");

                entity.HasOne(d => d._createdby_valueNavigation)
                    .WithMany(p => p.ptas_aptunittypeadjustment_createdby_valueNavigation)
                    .HasForeignKey(d => d._createdby_value)
                    .HasConstraintName("FK_systemuser_ptas_aptunittypeadjustment_createdby");

                entity.HasOne(d => d._createdonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_aptunittypeadjustment_createdonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._createdonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_aptunittypeadjustment_createdonbehalfby");

                entity.HasOne(d => d._modifiedby_valueNavigation)
                    .WithMany(p => p.ptas_aptunittypeadjustment_modifiedby_valueNavigation)
                    .HasForeignKey(d => d._modifiedby_value)
                    .HasConstraintName("FK_systemuser_ptas_aptunittypeadjustment_modifiedby");

                entity.HasOne(d => d._modifiedonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_aptunittypeadjustment_modifiedonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._modifiedonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_aptunittypeadjustment_modifiedonbehalfby");

                entity.HasOne(d => d._owningteam_valueNavigation)
                    .WithMany(p => p.ptas_aptunittypeadjustment)
                    .HasForeignKey(d => d._owningteam_value)
                    .HasConstraintName("FK_team_ptas_aptunittypeadjustment_owningteam");

                entity.HasOne(d => d._owninguser_valueNavigation)
                    .WithMany(p => p.ptas_aptunittypeadjustment_owninguser_valueNavigation)
                    .HasForeignKey(d => d._owninguser_value)
                    .HasConstraintName("FK_systemuser_ptas_aptunittypeadjustment_owninguser");

                entity.HasOne(d => d._ptas_assessmentyearlookupid_valueNavigation)
                    .WithMany(p => p.ptas_aptunittypeadjustment)
                    .HasForeignKey(d => d._ptas_assessmentyearlookupid_value)
                    .HasConstraintName("FK_ptas_year_ptas_aptunittypeadjustment_ptas_assessmentyearlookupid");
            });

            modelBuilder.Entity<ptas_aptvaluation>(entity =>
            {
                entity.ToTable("ptas_aptvaluation", "dynamics");

                entity.Property(e => e.ptas_aptvaluationid).ValueGeneratedNever();

                entity.Property(e => e.exchangerate).HasColumnType("money");

                entity.Property(e => e.ptas_annuallaundryincome).HasColumnType("money");

                entity.Property(e => e.ptas_annuallaundryincome_base).HasColumnType("money");

                entity.Property(e => e.ptas_annualmiscellaneousincome).HasColumnType("money");

                entity.Property(e => e.ptas_annualmiscellaneousincome_base).HasColumnType("money");

                entity.Property(e => e.ptas_annualmoorageincome).HasColumnType("money");

                entity.Property(e => e.ptas_annualmoorageincome_base).HasColumnType("money");

                entity.Property(e => e.ptas_annualparkingincome).HasColumnType("money");

                entity.Property(e => e.ptas_annualparkingincome_base).HasColumnType("money");

                entity.Property(e => e.ptas_apartmentexpense).HasColumnType("money");

                entity.Property(e => e.ptas_apartmentexpense_base).HasColumnType("money");

                entity.Property(e => e.ptas_apartmentgim).HasColumnType("money");

                entity.Property(e => e.ptas_apartmentrentincomemonthly).HasColumnType("money");

                entity.Property(e => e.ptas_apartmentrentincomemonthly_base).HasColumnType("money");

                entity.Property(e => e.ptas_baselandvalue).HasColumnType("money");

                entity.Property(e => e.ptas_baselandvalue_base).HasColumnType("money");

                entity.Property(e => e.ptas_blendedcaprate).HasColumnType("money");

                entity.Property(e => e.ptas_commercialcaprate).HasColumnType("money");

                entity.Property(e => e.ptas_commercialegi).HasColumnType("money");

                entity.Property(e => e.ptas_commercialegi_base).HasColumnType("money");

                entity.Property(e => e.ptas_commercialgim).HasColumnType("money");

                entity.Property(e => e.ptas_commercialincomevalue).HasColumnType("money");

                entity.Property(e => e.ptas_commercialincomevalue_base).HasColumnType("money");

                entity.Property(e => e.ptas_commercialnoi).HasColumnType("money");

                entity.Property(e => e.ptas_commercialnoi_base).HasColumnType("money");

                entity.Property(e => e.ptas_commercialoexfactor).HasColumnType("money");

                entity.Property(e => e.ptas_commercialpgi).HasColumnType("money");

                entity.Property(e => e.ptas_commercialpgi_base).HasColumnType("money");

                entity.Property(e => e.ptas_commercialrentincomeannual).HasColumnType("money");

                entity.Property(e => e.ptas_commercialrentincomeannual_base).HasColumnType("money");

                entity.Property(e => e.ptas_commercialrentrate).HasColumnType("money");

                entity.Property(e => e.ptas_commercialrentrate_base).HasColumnType("money");

                entity.Property(e => e.ptas_commercialvcl).HasColumnType("money");

                entity.Property(e => e.ptas_comparablesalesvalue).HasColumnType("money");

                entity.Property(e => e.ptas_comparablesalesvalue_base).HasColumnType("money");

                entity.Property(e => e.ptas_comparablesalesvalueminusvacantland).HasColumnType("money");

                entity.Property(e => e.ptas_comparablesalesvalueminusvacantland_base).HasColumnType("money");

                entity.Property(e => e.ptas_comparablesalesvalueperunit).HasColumnType("money");

                entity.Property(e => e.ptas_comparablesalesvalueperunit_base).HasColumnType("money");

                entity.Property(e => e.ptas_comparablesalesweight).HasColumnType("money");

                entity.Property(e => e.ptas_costvaluercnldplusland).HasColumnType("money");

                entity.Property(e => e.ptas_costvaluercnldplusland_base).HasColumnType("money");

                entity.Property(e => e.ptas_effectivegrossincome).HasColumnType("money");

                entity.Property(e => e.ptas_effectivegrossincome_base).HasColumnType("money");

                entity.Property(e => e.ptas_emvminusvacantland).HasColumnType("money");

                entity.Property(e => e.ptas_emvminusvacantland_base).HasColumnType("money");

                entity.Property(e => e.ptas_emvperunit).HasColumnType("money");

                entity.Property(e => e.ptas_emvperunit_base).HasColumnType("money");

                entity.Property(e => e.ptas_emvweight).HasColumnType("money");

                entity.Property(e => e.ptas_estimatedmarketvalueemv).HasColumnType("money");

                entity.Property(e => e.ptas_estimatedmarketvalueemv_base).HasColumnType("money");

                entity.Property(e => e.ptas_gimblended).HasColumnType("money");

                entity.Property(e => e.ptas_gimminusvacantland).HasColumnType("money");

                entity.Property(e => e.ptas_gimminusvacantland_base).HasColumnType("money");

                entity.Property(e => e.ptas_grossincomemultiplervaluegim).HasColumnType("money");

                entity.Property(e => e.ptas_grossincomemultiplervaluegim_base).HasColumnType("money");

                entity.Property(e => e.ptas_incomevalue).HasColumnType("money");

                entity.Property(e => e.ptas_incomevalue_base).HasColumnType("money");

                entity.Property(e => e.ptas_incomevalueminusvacantland).HasColumnType("money");

                entity.Property(e => e.ptas_incomevalueminusvacantland_base).HasColumnType("money");

                entity.Property(e => e.ptas_incomevalueperunit).HasColumnType("money");

                entity.Property(e => e.ptas_incomevalueperunit_base).HasColumnType("money");

                entity.Property(e => e.ptas_incomevaluetopreviousvalue).HasColumnType("money");

                entity.Property(e => e.ptas_incomeweight).HasColumnType("money");

                entity.Property(e => e.ptas_manualvalue).HasColumnType("money");

                entity.Property(e => e.ptas_manualvalue_base).HasColumnType("money");

                entity.Property(e => e.ptas_neighborhoodrank).HasColumnType("money");

                entity.Property(e => e.ptas_netincome).HasColumnType("money");

                entity.Property(e => e.ptas_netincome_base).HasColumnType("money");

                entity.Property(e => e.ptas_ommercialoex).HasColumnType("money");

                entity.Property(e => e.ptas_ommercialoex_base).HasColumnType("money");

                entity.Property(e => e.ptas_otherincome).HasColumnType("money");

                entity.Property(e => e.ptas_otherincome_base).HasColumnType("money");

                entity.Property(e => e.ptas_percentcommercial).HasColumnType("money");

                entity.Property(e => e.ptas_percenttax).HasColumnType("money");

                entity.Property(e => e.ptas_potentialgrossincome).HasColumnType("money");

                entity.Property(e => e.ptas_potentialgrossincome_base).HasColumnType("money");

                entity.Property(e => e.ptas_previoustotalvalue).HasColumnType("money");

                entity.Property(e => e.ptas_previoustotalvalue_base).HasColumnType("money");

                entity.Property(e => e.ptas_recommendedvalue).HasColumnType("money");

                entity.Property(e => e.ptas_recommendedvalue_base).HasColumnType("money");

                entity.Property(e => e.ptas_recommendedvaluetopreviousvalue).HasColumnType("money");

                entity.Property(e => e.ptas_rent_roommarketrent).HasColumnType("money");

                entity.Property(e => e.ptas_rent_roommarketrent_base).HasColumnType("money");

                entity.Property(e => e.ptas_rents_1bedroom1bath_marketrent).HasColumnType("money");

                entity.Property(e => e.ptas_rents_1bedroom1bath_marketrent_base).HasColumnType("money");

                entity.Property(e => e.ptas_rents_1bedroom1bathcomprent).HasColumnType("money");

                entity.Property(e => e.ptas_rents_1bedroom1bathcomprent_base).HasColumnType("money");

                entity.Property(e => e.ptas_rents_2bedroom1bathcomprent).HasColumnType("money");

                entity.Property(e => e.ptas_rents_2bedroom1bathcomprent_base).HasColumnType("money");

                entity.Property(e => e.ptas_rents_2bedroom1bathmarketrent).HasColumnType("money");

                entity.Property(e => e.ptas_rents_2bedroom1bathmarketrent_base).HasColumnType("money");

                entity.Property(e => e.ptas_rents_2bedroom2bathcomprent).HasColumnType("money");

                entity.Property(e => e.ptas_rents_2bedroom2bathcomprent_base).HasColumnType("money");

                entity.Property(e => e.ptas_rents_2bedroom2bathmarketrent).HasColumnType("money");

                entity.Property(e => e.ptas_rents_2bedroom2bathmarketrent_base).HasColumnType("money");

                entity.Property(e => e.ptas_rents_3bedroom1bathcomprent).HasColumnType("money");

                entity.Property(e => e.ptas_rents_3bedroom1bathcomprent_base).HasColumnType("money");

                entity.Property(e => e.ptas_rents_3bedroom1bathmarketrent).HasColumnType("money");

                entity.Property(e => e.ptas_rents_3bedroom1bathmarketrent_base).HasColumnType("money");

                entity.Property(e => e.ptas_rents_3bedroom2bathcomprent).HasColumnType("money");

                entity.Property(e => e.ptas_rents_3bedroom2bathcomprent_base).HasColumnType("money");

                entity.Property(e => e.ptas_rents_3bedroom2bathmarketrent).HasColumnType("money");

                entity.Property(e => e.ptas_rents_3bedroom2bathmarketrent_base).HasColumnType("money");

                entity.Property(e => e.ptas_rents_3bedroom3bathcomprent).HasColumnType("money");

                entity.Property(e => e.ptas_rents_3bedroom3bathcomprent_base).HasColumnType("money");

                entity.Property(e => e.ptas_rents_3bedroom3bathmarketrent).HasColumnType("money");

                entity.Property(e => e.ptas_rents_3bedroom3bathmarketrent_base).HasColumnType("money");

                entity.Property(e => e.ptas_rents_4bedroomcomprent).HasColumnType("money");

                entity.Property(e => e.ptas_rents_4bedroomcomprent_base).HasColumnType("money");

                entity.Property(e => e.ptas_rents_4bedroommarketrent).HasColumnType("money");

                entity.Property(e => e.ptas_rents_4bedroommarketrent_base).HasColumnType("money");

                entity.Property(e => e.ptas_rents_5bedroompluscomprent).HasColumnType("money");

                entity.Property(e => e.ptas_rents_5bedroompluscomprent_base).HasColumnType("money");

                entity.Property(e => e.ptas_rents_5bedroomplusmarketrent).HasColumnType("money");

                entity.Property(e => e.ptas_rents_5bedroomplusmarketrent_base).HasColumnType("money");

                entity.Property(e => e.ptas_rents_marketrent).HasColumnType("money");

                entity.Property(e => e.ptas_rents_marketrent_base).HasColumnType("money");

                entity.Property(e => e.ptas_rents_open1bedroom_marketrent).HasColumnType("money");

                entity.Property(e => e.ptas_rents_open1bedroom_marketrent_base).HasColumnType("money");

                entity.Property(e => e.ptas_rents_open1bedroomcomprent).HasColumnType("money");

                entity.Property(e => e.ptas_rents_open1bedroomcomprent_base).HasColumnType("money");

                entity.Property(e => e.ptas_rents_roomcomprent).HasColumnType("money");

                entity.Property(e => e.ptas_rents_roomcomprent_base).HasColumnType("money");

                entity.Property(e => e.ptas_rents_studiocomprent).HasColumnType("money");

                entity.Property(e => e.ptas_rents_studiocomprent_base).HasColumnType("money");

                entity.Property(e => e.ptas_rents_studiomarketrent).HasColumnType("money");

                entity.Property(e => e.ptas_rents_studiomarketrent_base).HasColumnType("money");

                entity.Property(e => e.ptas_saleprice).HasColumnType("money");

                entity.Property(e => e.ptas_saleprice_base).HasColumnType("money");

                entity.Property(e => e.ptas_totalapartmentmarketrent).HasColumnType("money");

                entity.Property(e => e.ptas_totalapartmentmarketrent_base).HasColumnType("money");

                entity.Property(e => e.ptas_totalexpenses).HasColumnType("money");

                entity.Property(e => e.ptas_totalexpenses_base).HasColumnType("money");

                entity.Property(e => e.ptas_totalexpensespercent).HasColumnType("money");

                entity.Property(e => e.ptas_trendedprice).HasColumnType("money");

                entity.Property(e => e.ptas_trendedprice_base).HasColumnType("money");

                entity.Property(e => e.ptas_trendedpriceperunit).HasColumnType("money");

                entity.Property(e => e.ptas_trendedpriceperunit_base).HasColumnType("money");

                entity.Property(e => e.ptas_unitsizefactor).HasColumnType("money");

                entity.Property(e => e.ptas_vacancyandcreditloss).HasColumnType("money");

                entity.Property(e => e.ptas_viewrank).HasColumnType("money");

                entity.Property(e => e.ptas_weightedvalue).HasColumnType("money");

                entity.Property(e => e.ptas_weightedvalue_base).HasColumnType("money");

                entity.Property(e => e.ptas_weightedvalueminusvacantland).HasColumnType("money");

                entity.Property(e => e.ptas_weightedvalueminusvacantland_base).HasColumnType("money");

                entity.Property(e => e.ptas_weightedvalueperunit).HasColumnType("money");

                entity.Property(e => e.ptas_weightedvalueperunit_base).HasColumnType("money");

                entity.Property(e => e.ptas_weightedvaluetopreviousvalue).HasColumnType("money");

                entity.HasOne(d => d._createdby_valueNavigation)
                    .WithMany(p => p.ptas_aptvaluation_createdby_valueNavigation)
                    .HasForeignKey(d => d._createdby_value)
                    .HasConstraintName("FK_systemuser_ptas_aptvaluation_createdby");

                entity.HasOne(d => d._createdonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_aptvaluation_createdonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._createdonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_aptvaluation_createdonbehalfby");

                entity.HasOne(d => d._modifiedby_valueNavigation)
                    .WithMany(p => p.ptas_aptvaluation_modifiedby_valueNavigation)
                    .HasForeignKey(d => d._modifiedby_value)
                    .HasConstraintName("FK_systemuser_ptas_aptvaluation_modifiedby");

                entity.HasOne(d => d._modifiedonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_aptvaluation_modifiedonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._modifiedonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_aptvaluation_modifiedonbehalfby");

                entity.HasOne(d => d._owningteam_valueNavigation)
                    .WithMany(p => p.ptas_aptvaluation)
                    .HasForeignKey(d => d._owningteam_value)
                    .HasConstraintName("FK_team_ptas_aptvaluation_owningteam");

                entity.HasOne(d => d._owninguser_valueNavigation)
                    .WithMany(p => p.ptas_aptvaluation_owninguser_valueNavigation)
                    .HasForeignKey(d => d._owninguser_value)
                    .HasConstraintName("FK_systemuser_ptas_aptvaluation_owninguser");

                entity.HasOne(d => d._ptas_appraiserid_valueNavigation)
                    .WithMany(p => p.ptas_aptvaluation_ptas_appraiserid_valueNavigation)
                    .HasForeignKey(d => d._ptas_appraiserid_value)
                    .HasConstraintName("FK_systemuser_ptas_aptvaluation_ptas_appraiserid");

                entity.HasOne(d => d._ptas_aptneighborhoodid_valueNavigation)
                    .WithMany(p => p.ptas_aptvaluation)
                    .HasForeignKey(d => d._ptas_aptneighborhoodid_value)
                    .HasConstraintName("FK_ptas_aptneighborhood_ptas_aptvaluation_ptas_aptneighborhoodid");

                entity.HasOne(d => d._ptas_aptvaluationprojectid_valueNavigation)
                    .WithMany(p => p.ptas_aptvaluation)
                    .HasForeignKey(d => d._ptas_aptvaluationprojectid_value)
                    .HasConstraintName("FK_ptas_aptvaluationproject_ptas_aptvaluation_ptas_aptvaluationprojectid");

                entity.HasOne(d => d._ptas_economicunitid_valueNavigation)
                    .WithMany(p => p.ptas_aptvaluation)
                    .HasForeignKey(d => d._ptas_economicunitid_value)
                    .HasConstraintName("FK_ptas_economicunit_ptas_aptvaluation_ptas_economicunitid");

                entity.HasOne(d => d._ptas_geoareaid_valueNavigation)
                    .WithMany(p => p.ptas_aptvaluation)
                    .HasForeignKey(d => d._ptas_geoareaid_value)
                    .HasConstraintName("FK_ptas_geoarea_ptas_aptvaluation_ptas_geoareaid");

                entity.HasOne(d => d._ptas_geoneighborhoodid_valueNavigation)
                    .WithMany(p => p.ptas_aptvaluation)
                    .HasForeignKey(d => d._ptas_geoneighborhoodid_value)
                    .HasConstraintName("FK_ptas_geoneighborhood_ptas_aptvaluation_ptas_geoneighborhoodid");

                entity.HasOne(d => d._ptas_parcelid_valueNavigation)
                    .WithMany(p => p.ptas_aptvaluation)
                    .HasForeignKey(d => d._ptas_parcelid_value)
                    .HasConstraintName("FK_ptas_parceldetail_ptas_aptvaluation_ptas_parcelid");

                entity.HasOne(d => d._ptas_projectid_valueNavigation)
                    .WithMany(p => p.ptas_aptvaluation)
                    .HasForeignKey(d => d._ptas_projectid_value)
                    .HasConstraintName("FK_ptas_condocomplex_ptas_aptvaluation_ptas_projectid");

                entity.HasOne(d => d._ptas_propertytypeid_valueNavigation)
                    .WithMany(p => p.ptas_aptvaluation)
                    .HasForeignKey(d => d._ptas_propertytypeid_value)
                    .HasConstraintName("FK_ptas_propertytype_ptas_aptvaluation_ptas_propertytypeid");

                entity.HasOne(d => d._ptas_responsibilityapplgroup_valueNavigation)
                    .WithMany(p => p.ptas_aptvaluation)
                    .HasForeignKey(d => d._ptas_responsibilityapplgroup_value)
                    .HasConstraintName("FK_ptas_responsibility_ptas_aptvaluation_ptas_responsibilityapplgroup");

                entity.HasOne(d => d._ptas_specialtyarea_valueNavigation)
                    .WithMany(p => p.ptas_aptvaluation)
                    .HasForeignKey(d => d._ptas_specialtyarea_value)
                    .HasConstraintName("FK_ptas_specialtyarea_ptas_aptvaluation_ptas_specialtyarea");

                entity.HasOne(d => d._ptas_supergroup_valueNavigation)
                    .WithMany(p => p.ptas_aptvaluation)
                    .HasForeignKey(d => d._ptas_supergroup_value)
                    .HasConstraintName("FK_ptas_supergroup_ptas_aptvaluation_ptas_supergroup");

                entity.HasOne(d => d._ptas_updatedbyid_valueNavigation)
                    .WithMany(p => p.ptas_aptvaluation_ptas_updatedbyid_valueNavigation)
                    .HasForeignKey(d => d._ptas_updatedbyid_value)
                    .HasConstraintName("FK_systemuser_ptas_aptvaluation_ptas_updatedbyid");
            });

            modelBuilder.Entity<ptas_aptvaluationproject>(entity =>
            {
                entity.ToTable("ptas_aptvaluationproject", "dynamics");

                entity.Property(e => e.ptas_aptvaluationprojectid).ValueGeneratedNever();

                entity.Property(e => e.ptas_comparablesalesweight).HasColumnType("money");

                entity.Property(e => e.ptas_emvweight).HasColumnType("money");

                entity.Property(e => e.ptas_incomeweight).HasColumnType("money");

                entity.HasOne(d => d._createdby_valueNavigation)
                    .WithMany(p => p.ptas_aptvaluationproject_createdby_valueNavigation)
                    .HasForeignKey(d => d._createdby_value)
                    .HasConstraintName("FK_systemuser_ptas_aptvaluationproject_createdby");

                entity.HasOne(d => d._createdonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_aptvaluationproject_createdonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._createdonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_aptvaluationproject_createdonbehalfby");

                entity.HasOne(d => d._modifiedby_valueNavigation)
                    .WithMany(p => p.ptas_aptvaluationproject_modifiedby_valueNavigation)
                    .HasForeignKey(d => d._modifiedby_value)
                    .HasConstraintName("FK_systemuser_ptas_aptvaluationproject_modifiedby");

                entity.HasOne(d => d._modifiedonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_aptvaluationproject_modifiedonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._modifiedonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_aptvaluationproject_modifiedonbehalfby");

                entity.HasOne(d => d._owningteam_valueNavigation)
                    .WithMany(p => p.ptas_aptvaluationproject)
                    .HasForeignKey(d => d._owningteam_value)
                    .HasConstraintName("FK_team_ptas_aptvaluationproject_owningteam");

                entity.HasOne(d => d._owninguser_valueNavigation)
                    .WithMany(p => p.ptas_aptvaluationproject_owninguser_valueNavigation)
                    .HasForeignKey(d => d._owninguser_value)
                    .HasConstraintName("FK_systemuser_ptas_aptvaluationproject_owninguser");
            });

            modelBuilder.Entity<ptas_aptviewqualityadjustment>(entity =>
            {
                entity.ToTable("ptas_aptviewqualityadjustment", "dynamics");

                entity.Property(e => e.ptas_aptviewqualityadjustmentid).ValueGeneratedNever();

                entity.Property(e => e.ptas_viewqualityadjustment).HasColumnType("money");

                entity.HasOne(d => d._createdby_valueNavigation)
                    .WithMany(p => p.ptas_aptviewqualityadjustment_createdby_valueNavigation)
                    .HasForeignKey(d => d._createdby_value)
                    .HasConstraintName("FK_systemuser_ptas_aptviewqualityadjustment_createdby");

                entity.HasOne(d => d._createdonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_aptviewqualityadjustment_createdonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._createdonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_aptviewqualityadjustment_createdonbehalfby");

                entity.HasOne(d => d._modifiedby_valueNavigation)
                    .WithMany(p => p.ptas_aptviewqualityadjustment_modifiedby_valueNavigation)
                    .HasForeignKey(d => d._modifiedby_value)
                    .HasConstraintName("FK_systemuser_ptas_aptviewqualityadjustment_modifiedby");

                entity.HasOne(d => d._modifiedonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_aptviewqualityadjustment_modifiedonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._modifiedonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_aptviewqualityadjustment_modifiedonbehalfby");

                entity.HasOne(d => d._ptas_assessmentyearlookupid_valueNavigation)
                    .WithMany(p => p.ptas_aptviewqualityadjustment)
                    .HasForeignKey(d => d._ptas_assessmentyearlookupid_value)
                    .HasConstraintName("FK_ptas_year_ptas_aptviewqualityadjustment_ptas_assessmentyearlookupid");
            });

            modelBuilder.Entity<ptas_aptviewrankadjustment>(entity =>
            {
                entity.ToTable("ptas_aptviewrankadjustment", "dynamics");

                entity.Property(e => e.ptas_aptviewrankadjustmentid).ValueGeneratedNever();

                entity.Property(e => e.ptas_caprateviewadjustmentcoefficient).HasColumnType("money");

                entity.Property(e => e.ptas_caprateviewadjustmentintercept).HasColumnType("money");

                entity.Property(e => e.ptas_gimviewadjsutmentcoefficient).HasColumnType("money");

                entity.Property(e => e.ptas_gimviewadjustmentintercept).HasColumnType("money");

                entity.HasOne(d => d._createdby_valueNavigation)
                    .WithMany(p => p.ptas_aptviewrankadjustment_createdby_valueNavigation)
                    .HasForeignKey(d => d._createdby_value)
                    .HasConstraintName("FK_systemuser_ptas_aptviewrankadjustment_createdby");

                entity.HasOne(d => d._createdonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_aptviewrankadjustment_createdonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._createdonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_aptviewrankadjustment_createdonbehalfby");

                entity.HasOne(d => d._modifiedby_valueNavigation)
                    .WithMany(p => p.ptas_aptviewrankadjustment_modifiedby_valueNavigation)
                    .HasForeignKey(d => d._modifiedby_value)
                    .HasConstraintName("FK_systemuser_ptas_aptviewrankadjustment_modifiedby");

                entity.HasOne(d => d._modifiedonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_aptviewrankadjustment_modifiedonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._modifiedonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_aptviewrankadjustment_modifiedonbehalfby");

                entity.HasOne(d => d._owningteam_valueNavigation)
                    .WithMany(p => p.ptas_aptviewrankadjustment)
                    .HasForeignKey(d => d._owningteam_value)
                    .HasConstraintName("FK_team_ptas_aptviewrankadjustment_owningteam");

                entity.HasOne(d => d._owninguser_valueNavigation)
                    .WithMany(p => p.ptas_aptviewrankadjustment_owninguser_valueNavigation)
                    .HasForeignKey(d => d._owninguser_value)
                    .HasConstraintName("FK_systemuser_ptas_aptviewrankadjustment_owninguser");

                entity.HasOne(d => d._ptas_assessmentyearlookupid_valueNavigation)
                    .WithMany(p => p.ptas_aptviewrankadjustment)
                    .HasForeignKey(d => d._ptas_assessmentyearlookupid_value)
                    .HasConstraintName("FK_ptas_year_ptas_aptviewrankadjustment_ptas_assessmentyearlookupid");
            });

            modelBuilder.Entity<ptas_aptviewtypeadjustment>(entity =>
            {
                entity.ToTable("ptas_aptviewtypeadjustment", "dynamics");

                entity.Property(e => e.ptas_aptviewtypeadjustmentid).ValueGeneratedNever();

                entity.Property(e => e.ptas_viewtypeadjustment).HasColumnType("money");

                entity.HasOne(d => d._createdby_valueNavigation)
                    .WithMany(p => p.ptas_aptviewtypeadjustment_createdby_valueNavigation)
                    .HasForeignKey(d => d._createdby_value)
                    .HasConstraintName("FK_systemuser_ptas_aptviewtypeadjustment_createdby");

                entity.HasOne(d => d._createdonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_aptviewtypeadjustment_createdonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._createdonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_aptviewtypeadjustment_createdonbehalfby");

                entity.HasOne(d => d._modifiedby_valueNavigation)
                    .WithMany(p => p.ptas_aptviewtypeadjustment_modifiedby_valueNavigation)
                    .HasForeignKey(d => d._modifiedby_value)
                    .HasConstraintName("FK_systemuser_ptas_aptviewtypeadjustment_modifiedby");

                entity.HasOne(d => d._modifiedonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_aptviewtypeadjustment_modifiedonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._modifiedonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_aptviewtypeadjustment_modifiedonbehalfby");

                entity.HasOne(d => d._ptas_assessmentyearlookupid_valueNavigation)
                    .WithMany(p => p.ptas_aptviewtypeadjustment)
                    .HasForeignKey(d => d._ptas_assessmentyearlookupid_value)
                    .HasConstraintName("FK_ptas_year_ptas_aptviewtypeadjustment_ptas_assessmentyearlookupid");
            });

            modelBuilder.Entity<ptas_aptyearbuiltadjustment>(entity =>
            {
                entity.ToTable("ptas_aptyearbuiltadjustment", "dynamics");

                entity.Property(e => e.ptas_aptyearbuiltadjustmentid).ValueGeneratedNever();

                entity.Property(e => e.ptas_rentscoefficient).HasColumnType("money");

                entity.Property(e => e.ptas_salescoefficient).HasColumnType("money");

                entity.HasOne(d => d._createdby_valueNavigation)
                    .WithMany(p => p.ptas_aptyearbuiltadjustment_createdby_valueNavigation)
                    .HasForeignKey(d => d._createdby_value)
                    .HasConstraintName("FK_systemuser_ptas_aptyearbuiltadjustment_createdby");

                entity.HasOne(d => d._createdonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_aptyearbuiltadjustment_createdonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._createdonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_aptyearbuiltadjustment_createdonbehalfby");

                entity.HasOne(d => d._modifiedby_valueNavigation)
                    .WithMany(p => p.ptas_aptyearbuiltadjustment_modifiedby_valueNavigation)
                    .HasForeignKey(d => d._modifiedby_value)
                    .HasConstraintName("FK_systemuser_ptas_aptyearbuiltadjustment_modifiedby");

                entity.HasOne(d => d._modifiedonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_aptyearbuiltadjustment_modifiedonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._modifiedonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_aptyearbuiltadjustment_modifiedonbehalfby");

                entity.HasOne(d => d._owningteam_valueNavigation)
                    .WithMany(p => p.ptas_aptyearbuiltadjustment)
                    .HasForeignKey(d => d._owningteam_value)
                    .HasConstraintName("FK_team_ptas_aptyearbuiltadjustment_owningteam");

                entity.HasOne(d => d._owninguser_valueNavigation)
                    .WithMany(p => p.ptas_aptyearbuiltadjustment_owninguser_valueNavigation)
                    .HasForeignKey(d => d._owninguser_value)
                    .HasConstraintName("FK_systemuser_ptas_aptyearbuiltadjustment_owninguser");

                entity.HasOne(d => d._ptas_assessmentyearlookupid_valueNavigation)
                    .WithMany(p => p.ptas_aptyearbuiltadjustment)
                    .HasForeignKey(d => d._ptas_assessmentyearlookupid_value)
                    .HasConstraintName("FK_ptas_year_ptas_aptyearbuiltadjustment_ptas_assessmentyearlookupid");
            });

            modelBuilder.Entity<ptas_arcreasoncode>(entity =>
            {
                entity.ToTable("ptas_arcreasoncode", "dynamics");

                entity.Property(e => e.ptas_arcreasoncodeid).ValueGeneratedNever();

                entity.HasOne(d => d._createdby_valueNavigation)
                    .WithMany(p => p.ptas_arcreasoncode_createdby_valueNavigation)
                    .HasForeignKey(d => d._createdby_value)
                    .HasConstraintName("FK_systemuser_ptas_arcreasoncode_createdby");

                entity.HasOne(d => d._createdonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_arcreasoncode_createdonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._createdonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_arcreasoncode_createdonbehalfby");

                entity.HasOne(d => d._modifiedby_valueNavigation)
                    .WithMany(p => p.ptas_arcreasoncode_modifiedby_valueNavigation)
                    .HasForeignKey(d => d._modifiedby_value)
                    .HasConstraintName("FK_systemuser_ptas_arcreasoncode_modifiedby");

                entity.HasOne(d => d._modifiedonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_arcreasoncode_modifiedonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._modifiedonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_arcreasoncode_modifiedonbehalfby");

                entity.HasOne(d => d._owningteam_valueNavigation)
                    .WithMany(p => p.ptas_arcreasoncode)
                    .HasForeignKey(d => d._owningteam_value)
                    .HasConstraintName("FK_team_ptas_arcreasoncode_owningteam");

                entity.HasOne(d => d._owninguser_valueNavigation)
                    .WithMany(p => p.ptas_arcreasoncode_owninguser_valueNavigation)
                    .HasForeignKey(d => d._owninguser_value)
                    .HasConstraintName("FK_systemuser_ptas_arcreasoncode_owninguser");
            });

            modelBuilder.Entity<ptas_area>(entity =>
            {
                entity.ToTable("ptas_area", "dynamics");

                entity.HasIndex(e => e.modifiedon)
                    .HasName("Idx_ptas_area_modifiedon");

                entity.Property(e => e.ptas_areaid).ValueGeneratedNever();

                entity.HasOne(d => d._createdby_valueNavigation)
                    .WithMany(p => p.ptas_area_createdby_valueNavigation)
                    .HasForeignKey(d => d._createdby_value)
                    .HasConstraintName("FK_systemuser_ptas_area_createdby");

                entity.HasOne(d => d._createdonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_area_createdonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._createdonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_area_createdonbehalfby");

                entity.HasOne(d => d._modifiedby_valueNavigation)
                    .WithMany(p => p.ptas_area_modifiedby_valueNavigation)
                    .HasForeignKey(d => d._modifiedby_value)
                    .HasConstraintName("FK_systemuser_ptas_area_modifiedby");

                entity.HasOne(d => d._modifiedonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_area_modifiedonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._modifiedonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_area_modifiedonbehalfby");

                entity.HasOne(d => d._ptas_appraiserid_valueNavigation)
                    .WithMany(p => p.ptas_area_ptas_appraiserid_valueNavigation)
                    .HasForeignKey(d => d._ptas_appraiserid_value)
                    .HasConstraintName("FK_systemuser_ptas_area_ptas_appraiserid");

                entity.HasOne(d => d._ptas_residentialappraiserteam_valueNavigation)
                    .WithMany(p => p.ptas_area)
                    .HasForeignKey(d => d._ptas_residentialappraiserteam_value)
                    .HasConstraintName("FK_ptas_residentialappraiserteam_ptas_area_ptas_residentialappraiserteam");

                entity.HasOne(d => d._ptas_seniorappraiser_valueNavigation)
                    .WithMany(p => p.ptas_area_ptas_seniorappraiser_valueNavigation)
                    .HasForeignKey(d => d._ptas_seniorappraiser_value)
                    .HasConstraintName("FK_systemuser_ptas_area_ptas_seniorappraiser");
            });

            modelBuilder.Entity<ptas_assessmentrollcorrection>(entity =>
            {
                entity.ToTable("ptas_assessmentrollcorrection", "dynamics");

                entity.Property(e => e.ptas_assessmentrollcorrectionid).ValueGeneratedNever();

                entity.Property(e => e.exchangerate).HasColumnType("money");

                entity.Property(e => e.ptas_billdifference).HasColumnType("money");

                entity.Property(e => e.ptas_billdifference_base).HasColumnType("money");

                entity.Property(e => e.ptas_levyrate).HasColumnType("money");

                entity.Property(e => e.ptas_newbill).HasColumnType("money");

                entity.Property(e => e.ptas_newbill_base).HasColumnType("money");

                entity.Property(e => e.ptas_newvalue).HasColumnType("money");

                entity.Property(e => e.ptas_newvalue_base).HasColumnType("money");

                entity.Property(e => e.ptas_oldbill).HasColumnType("money");

                entity.Property(e => e.ptas_oldbill_base).HasColumnType("money");

                entity.Property(e => e.ptas_oldvalue).HasColumnType("money");

                entity.Property(e => e.ptas_oldvalue_base).HasColumnType("money");

                entity.Property(e => e.ptas_paidamount).HasColumnType("money");

                entity.Property(e => e.ptas_paidamount_base).HasColumnType("money");

                entity.Property(e => e.ptas_receipt1).HasColumnType("money");

                entity.Property(e => e.ptas_receipt1_base).HasColumnType("money");

                entity.Property(e => e.ptas_receipt2).HasColumnType("money");

                entity.Property(e => e.ptas_receipt2_base).HasColumnType("money");

                entity.HasOne(d => d._createdby_valueNavigation)
                    .WithMany(p => p.ptas_assessmentrollcorrection_createdby_valueNavigation)
                    .HasForeignKey(d => d._createdby_value)
                    .HasConstraintName("FK_systemuser_ptas_assessmentrollcorrection_createdby");

                entity.HasOne(d => d._createdonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_assessmentrollcorrection_createdonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._createdonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_assessmentrollcorrection_createdonbehalfby");

                entity.HasOne(d => d._modifiedby_valueNavigation)
                    .WithMany(p => p.ptas_assessmentrollcorrection_modifiedby_valueNavigation)
                    .HasForeignKey(d => d._modifiedby_value)
                    .HasConstraintName("FK_systemuser_ptas_assessmentrollcorrection_modifiedby");

                entity.HasOne(d => d._modifiedonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_assessmentrollcorrection_modifiedonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._modifiedonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_assessmentrollcorrection_modifiedonbehalfby");

                entity.HasOne(d => d._owningteam_valueNavigation)
                    .WithMany(p => p.ptas_assessmentrollcorrection)
                    .HasForeignKey(d => d._owningteam_value)
                    .HasConstraintName("FK_team_ptas_assessmentrollcorrection_owningteam");

                entity.HasOne(d => d._owninguser_valueNavigation)
                    .WithMany(p => p.ptas_assessmentrollcorrection_owninguser_valueNavigation)
                    .HasForeignKey(d => d._owninguser_value)
                    .HasConstraintName("FK_systemuser_ptas_assessmentrollcorrection_owninguser");

                entity.HasOne(d => d._ptas_approvalappraiserid_valueNavigation)
                    .WithMany(p => p.ptas_assessmentrollcorrection_ptas_approvalappraiserid_valueNavigation)
                    .HasForeignKey(d => d._ptas_approvalappraiserid_value)
                    .HasConstraintName("FK_systemuser_ptas_assessmentrollcorrection_ptas_approvalappraiserid");

                entity.HasOne(d => d._ptas_assessmentyearid_valueNavigation)
                    .WithMany(p => p.ptas_assessmentrollcorrection)
                    .HasForeignKey(d => d._ptas_assessmentyearid_value)
                    .HasConstraintName("FK_ptas_year_ptas_assessmentrollcorrection_ptas_assessmentyearid");

                entity.HasOne(d => d._ptas_levycodeid_valueNavigation)
                    .WithMany(p => p.ptas_assessmentrollcorrection)
                    .HasForeignKey(d => d._ptas_levycodeid_value)
                    .HasConstraintName("FK_ptas_levycode_ptas_assessmentrollcorrection_ptas_levycodeid");

                entity.HasOne(d => d._ptas_reasoncodeid_valueNavigation)
                    .WithMany(p => p.ptas_assessmentrollcorrection)
                    .HasForeignKey(d => d._ptas_reasoncodeid_value)
                    .HasConstraintName("FK_ptas_arcreasoncode_ptas_assessmentrollcorrection_ptas_reasoncodeid");

                entity.HasOne(d => d._ptas_responsibleappraiserid_valueNavigation)
                    .WithMany(p => p.ptas_assessmentrollcorrection_ptas_responsibleappraiserid_valueNavigation)
                    .HasForeignKey(d => d._ptas_responsibleappraiserid_value)
                    .HasConstraintName("FK_systemuser_ptas_assessmentrollcorrection_ptas_responsibleappraiserid");
            });

            modelBuilder.Entity<ptas_bookmark>(entity =>
            {
                entity.ToTable("ptas_bookmark", "dynamics");

                entity.HasIndex(e => e.modifiedon)
                    .HasName("Idx_ptas_bookmark_modifiedon");

                entity.Property(e => e.ptas_bookmarkid).ValueGeneratedNever();

                entity.HasOne(d => d._createdby_valueNavigation)
                    .WithMany(p => p.ptas_bookmark_createdby_valueNavigation)
                    .HasForeignKey(d => d._createdby_value)
                    .HasConstraintName("FK_systemuser_ptas_bookmark_createdby");

                entity.HasOne(d => d._createdonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_bookmark_createdonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._createdonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_bookmark_createdonbehalfby");

                entity.HasOne(d => d._modifiedby_valueNavigation)
                    .WithMany(p => p.ptas_bookmark_modifiedby_valueNavigation)
                    .HasForeignKey(d => d._modifiedby_value)
                    .HasConstraintName("FK_systemuser_ptas_bookmark_modifiedby");

                entity.HasOne(d => d._modifiedonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_bookmark_modifiedonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._modifiedonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_bookmark_modifiedonbehalfby");

                entity.HasOne(d => d._owningteam_valueNavigation)
                    .WithMany(p => p.ptas_bookmark)
                    .HasForeignKey(d => d._owningteam_value)
                    .HasConstraintName("FK_team_ptas_bookmark_owningteam");

                entity.HasOne(d => d._owninguser_valueNavigation)
                    .WithMany(p => p.ptas_bookmark_owninguser_valueNavigation)
                    .HasForeignKey(d => d._owninguser_value)
                    .HasConstraintName("FK_systemuser_ptas_bookmark_owninguser");

                entity.HasOne(d => d._ptas_parceldetailid_valueNavigation)
                    .WithMany(p => p.ptas_bookmark)
                    .HasForeignKey(d => d._ptas_parceldetailid_value)
                    .HasConstraintName("FK_ptas_parceldetail_ptas_bookmark_ptas_parceldetailid");

                entity.HasOne(d => d._ptas_tag1_valueNavigation)
                    .WithMany(p => p.ptas_bookmark_ptas_tag1_valueNavigation)
                    .HasForeignKey(d => d._ptas_tag1_value)
                    .HasConstraintName("FK_ptas_bookmarktag_ptas_bookmark_ptas_tag1");

                entity.HasOne(d => d._ptas_tag2_valueNavigation)
                    .WithMany(p => p.ptas_bookmark_ptas_tag2_valueNavigation)
                    .HasForeignKey(d => d._ptas_tag2_value)
                    .HasConstraintName("FK_ptas_bookmarktag_ptas_bookmark_ptas_tag2");

                entity.HasOne(d => d._ptas_tag3_valueNavigation)
                    .WithMany(p => p.ptas_bookmark_ptas_tag3_valueNavigation)
                    .HasForeignKey(d => d._ptas_tag3_value)
                    .HasConstraintName("FK_ptas_bookmarktag_ptas_bookmark_ptas_tag3");

                entity.HasOne(d => d._ptas_tag4_valueNavigation)
                    .WithMany(p => p.ptas_bookmark_ptas_tag4_valueNavigation)
                    .HasForeignKey(d => d._ptas_tag4_value)
                    .HasConstraintName("FK_ptas_bookmarktag_ptas_bookmark_ptas_tag4");

                entity.HasOne(d => d._ptas_tag5_valueNavigation)
                    .WithMany(p => p.ptas_bookmark_ptas_tag5_valueNavigation)
                    .HasForeignKey(d => d._ptas_tag5_value)
                    .HasConstraintName("FK_ptas_bookmarktag_ptas_bookmark_ptas_tag5");
            });

            modelBuilder.Entity<ptas_bookmarktag>(entity =>
            {
                entity.ToTable("ptas_bookmarktag", "dynamics");

                entity.HasIndex(e => e.modifiedon)
                    .HasName("Idx_ptas_bookmarktag_modifiedon");

                entity.Property(e => e.ptas_bookmarktagid).ValueGeneratedNever();

                entity.HasOne(d => d._createdby_valueNavigation)
                    .WithMany(p => p.ptas_bookmarktag_createdby_valueNavigation)
                    .HasForeignKey(d => d._createdby_value)
                    .HasConstraintName("FK_systemuser_ptas_bookmarktag_createdby");

                entity.HasOne(d => d._createdonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_bookmarktag_createdonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._createdonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_bookmarktag_createdonbehalfby");

                entity.HasOne(d => d._modifiedby_valueNavigation)
                    .WithMany(p => p.ptas_bookmarktag_modifiedby_valueNavigation)
                    .HasForeignKey(d => d._modifiedby_value)
                    .HasConstraintName("FK_systemuser_ptas_bookmarktag_modifiedby");

                entity.HasOne(d => d._modifiedonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_bookmarktag_modifiedonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._modifiedonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_bookmarktag_modifiedonbehalfby");

                entity.HasOne(d => d._owningteam_valueNavigation)
                    .WithMany(p => p.ptas_bookmarktag)
                    .HasForeignKey(d => d._owningteam_value)
                    .HasConstraintName("FK_team_ptas_bookmarktag_owningteam");

                entity.HasOne(d => d._owninguser_valueNavigation)
                    .WithMany(p => p.ptas_bookmarktag_owninguser_valueNavigation)
                    .HasForeignKey(d => d._owninguser_value)
                    .HasConstraintName("FK_systemuser_ptas_bookmarktag_owninguser");
            });

            modelBuilder.Entity<ptas_buildingdetail>(entity =>
            {
                entity.ToTable("ptas_buildingdetail", "dynamics");

                entity.HasIndex(e => e.modifiedon)
                    .HasName("Idx_ptas_buildingdetail_modifiedon");

                entity.Property(e => e.ptas_buildingdetailid).ValueGeneratedNever();

                entity.Property(e => e.exchangerate).HasColumnType("money");

                entity.Property(e => e.ptas_additionalcost).HasColumnType("money");

                entity.Property(e => e.ptas_additionalcost_base).HasColumnType("money");

                entity.Property(e => e.ptas_numberofstoriesdecimal).HasColumnType("money");

                entity.HasOne(d => d._createdby_valueNavigation)
                    .WithMany(p => p.ptas_buildingdetail_createdby_valueNavigation)
                    .HasForeignKey(d => d._createdby_value)
                    .HasConstraintName("FK_systemuser_ptas_buildingdetail_createdby");

                entity.HasOne(d => d._createdonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_buildingdetail_createdonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._createdonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_buildingdetail_createdonbehalfby");

                entity.HasOne(d => d._modifiedby_valueNavigation)
                    .WithMany(p => p.ptas_buildingdetail_modifiedby_valueNavigation)
                    .HasForeignKey(d => d._modifiedby_value)
                    .HasConstraintName("FK_systemuser_ptas_buildingdetail_modifiedby");

                entity.HasOne(d => d._modifiedonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_buildingdetail_modifiedonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._modifiedonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_buildingdetail_modifiedonbehalfby");

                entity.HasOne(d => d._owningteam_valueNavigation)
                    .WithMany(p => p.ptas_buildingdetail)
                    .HasForeignKey(d => d._owningteam_value)
                    .HasConstraintName("FK_team_ptas_buildingdetail_owningteam");

                entity.HasOne(d => d._owninguser_valueNavigation)
                    .WithMany(p => p.ptas_buildingdetail_owninguser_valueNavigation)
                    .HasForeignKey(d => d._owninguser_value)
                    .HasConstraintName("FK_systemuser_ptas_buildingdetail_owninguser");

                entity.HasOne(d => d._ptas_addr1_cityid_valueNavigation)
                    .WithMany(p => p.ptas_buildingdetail)
                    .HasForeignKey(d => d._ptas_addr1_cityid_value)
                    .HasConstraintName("FK_ptas_city_ptas_buildingdetail_ptas_addr1_cityid");

                entity.HasOne(d => d._ptas_addr1_countryid_valueNavigation)
                    .WithMany(p => p.ptas_buildingdetail)
                    .HasForeignKey(d => d._ptas_addr1_countryid_value)
                    .HasConstraintName("FK_ptas_country_ptas_buildingdetail_ptas_addr1_countryid");

                entity.HasOne(d => d._ptas_addr1_stateid_valueNavigation)
                    .WithMany(p => p.ptas_buildingdetail)
                    .HasForeignKey(d => d._ptas_addr1_stateid_value)
                    .HasConstraintName("FK_ptas_stateorprovince_ptas_buildingdetail_ptas_addr1_stateid");

                entity.HasOne(d => d._ptas_addr1_streetnameid_valueNavigation)
                    .WithMany(p => p.ptas_buildingdetail)
                    .HasForeignKey(d => d._ptas_addr1_streetnameid_value)
                    .HasConstraintName("FK_ptas_streetname_ptas_buildingdetail_ptas_addr1_streetnameid");

                entity.HasOne(d => d._ptas_addr1_streettypeid_valueNavigation)
                    .WithMany(p => p.ptas_buildingdetail)
                    .HasForeignKey(d => d._ptas_addr1_streettypeid_value)
                    .HasConstraintName("FK_ptas_streettype_ptas_buildingdetail_ptas_addr1_streettypeid");

                entity.HasOne(d => d._ptas_addr1_zipcodeid_valueNavigation)
                    .WithMany(p => p.ptas_buildingdetail)
                    .HasForeignKey(d => d._ptas_addr1_zipcodeid_value)
                    .HasConstraintName("FK_ptas_zipcode_ptas_buildingdetail_ptas_addr1_zipcodeid");

                entity.HasOne(d => d._ptas_buildingsectionuseid_valueNavigation)
                    .WithMany(p => p.ptas_buildingdetail)
                    .HasForeignKey(d => d._ptas_buildingsectionuseid_value)
                    .HasConstraintName("FK_ptas_buildingsectionuse_ptas_buildingdetail_ptas_buildingsectionuseid");

                entity.HasOne(d => d._ptas_effectiveyearid_valueNavigation)
                    .WithMany(p => p.ptas_buildingdetail_ptas_effectiveyearid_valueNavigation)
                    .HasForeignKey(d => d._ptas_effectiveyearid_value)
                    .HasConstraintName("FK_ptas_year_ptas_buildingdetail_ptas_effectiveyearid");

                entity.HasOne(d => d._ptas_masterbuildingid_valueNavigation)
                    .WithMany(p => p.Inverse_ptas_masterbuildingid_valueNavigation)
                    .HasForeignKey(d => d._ptas_masterbuildingid_value)
                    .HasConstraintName("FK_ptas_buildingdetail_ptas_buildingdetail_ptas_masterbuildingid");

                entity.HasOne(d => d._ptas_parceldetailid_valueNavigation)
                    .WithMany(p => p.ptas_buildingdetail)
                    .HasForeignKey(d => d._ptas_parceldetailid_value)
                    .HasConstraintName("FK_ptas_parceldetail_ptas_buildingdetail_ptas_parceldetailid");

                entity.HasOne(d => d._ptas_propertytypeid_valueNavigation)
                    .WithMany(p => p.ptas_buildingdetail)
                    .HasForeignKey(d => d._ptas_propertytypeid_value)
                    .HasConstraintName("FK_ptas_propertytype_ptas_buildingdetail_ptas_propertytypeid");

                entity.HasOne(d => d._ptas_sketchid_valueNavigation)
                    .WithMany(p => p.ptas_buildingdetail)
                    .HasForeignKey(d => d._ptas_sketchid_value)
                    .HasConstraintName("FK_ptas_sketch_ptas_buildingdetail_ptas_sketchid");

                entity.HasOne(d => d._ptas_taxaccount_valueNavigation)
                    .WithMany(p => p.ptas_buildingdetail)
                    .HasForeignKey(d => d._ptas_taxaccount_value)
                    .HasConstraintName("FK_ptas_taxaccount_ptas_buildingdetail_ptas_taxaccount");

                entity.HasOne(d => d._ptas_yearbuiltid_valueNavigation)
                    .WithMany(p => p.ptas_buildingdetail_ptas_yearbuiltid_valueNavigation)
                    .HasForeignKey(d => d._ptas_yearbuiltid_value)
                    .HasConstraintName("FK_ptas_year_ptas_buildingdetail_ptas_yearbuiltid");
            });

            modelBuilder.Entity<ptas_buildingdetail_commercialuse>(entity =>
            {
                entity.ToTable("ptas_buildingdetail_commercialuse", "dynamics");

                entity.HasIndex(e => e.modifiedon)
                    .HasName("Idx_ptas_buildingdetail_commercialuse_modifiedon");

                entity.Property(e => e.ptas_buildingdetail_commercialuseid).ValueGeneratedNever();

                entity.HasOne(d => d._createdby_valueNavigation)
                    .WithMany(p => p.ptas_buildingdetail_commercialuse_createdby_valueNavigation)
                    .HasForeignKey(d => d._createdby_value)
                    .HasConstraintName("FK_systemuser_ptas_buildingdetail_commercialuse_createdby");

                entity.HasOne(d => d._createdonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_buildingdetail_commercialuse_createdonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._createdonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_buildingdetail_commercialuse_createdonbehalfby");

                entity.HasOne(d => d._modifiedby_valueNavigation)
                    .WithMany(p => p.ptas_buildingdetail_commercialuse_modifiedby_valueNavigation)
                    .HasForeignKey(d => d._modifiedby_value)
                    .HasConstraintName("FK_systemuser_ptas_buildingdetail_commercialuse_modifiedby");

                entity.HasOne(d => d._modifiedonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_buildingdetail_commercialuse_modifiedonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._modifiedonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_buildingdetail_commercialuse_modifiedonbehalfby");

                entity.HasOne(d => d._owningteam_valueNavigation)
                    .WithMany(p => p.ptas_buildingdetail_commercialuse)
                    .HasForeignKey(d => d._owningteam_value)
                    .HasConstraintName("FK_team_ptas_buildingdetail_commercialuse_owningteam");

                entity.HasOne(d => d._owninguser_valueNavigation)
                    .WithMany(p => p.ptas_buildingdetail_commercialuse_owninguser_valueNavigation)
                    .HasForeignKey(d => d._owninguser_value)
                    .HasConstraintName("FK_systemuser_ptas_buildingdetail_commercialuse_owninguser");

                entity.HasOne(d => d._ptas_buildingdetailid_valueNavigation)
                    .WithMany(p => p.ptas_buildingdetail_commercialuse)
                    .HasForeignKey(d => d._ptas_buildingdetailid_value)
                    .HasConstraintName("FK_ptas_buildingdetail_ptas_buildingdetail_commercialuse_ptas_buildingdetailid");

                entity.HasOne(d => d._ptas_buildingsectionuseid_valueNavigation)
                    .WithMany(p => p.ptas_buildingdetail_commercialuse)
                    .HasForeignKey(d => d._ptas_buildingsectionuseid_value)
                    .HasConstraintName("FK_ptas_buildingsectionuse_ptas_buildingdetail_commercialuse_ptas_buildingsectionuseid");

                entity.HasOne(d => d._ptas_mastersectionuseid_valueNavigation)
                    .WithMany(p => p.Inverse_ptas_mastersectionuseid_valueNavigation)
                    .HasForeignKey(d => d._ptas_mastersectionuseid_value)
                    .HasConstraintName("FK_ptas_buildingdetail_commercialuse_ptas_buildingdetail_commercialuse_ptas_mastersectionuseid");

                entity.HasOne(d => d._ptas_projectid_valueNavigation)
                    .WithMany(p => p.ptas_buildingdetail_commercialuse)
                    .HasForeignKey(d => d._ptas_projectid_value)
                    .HasConstraintName("FK_ptas_condocomplex_ptas_buildingdetail_commercialuse_ptas_projectid");

                entity.HasOne(d => d._ptas_specialtyareaid_valueNavigation)
                    .WithMany(p => p.ptas_buildingdetail_commercialuse)
                    .HasForeignKey(d => d._ptas_specialtyareaid_value)
                    .HasConstraintName("FK_ptas_specialtyarea_ptas_buildingdetail_commercialuse_ptas_specialtyareaid");

                entity.HasOne(d => d._ptas_specialtynbhdid_valueNavigation)
                    .WithMany(p => p.ptas_buildingdetail_commercialuse)
                    .HasForeignKey(d => d._ptas_specialtynbhdid_value)
                    .HasConstraintName("FK_ptas_specialtyneighborhood_ptas_buildingdetail_commercialuse_ptas_specialtynbhdid");

                entity.HasOne(d => d._ptas_unitid_valueNavigation)
                    .WithMany(p => p.ptas_buildingdetail_commercialuse)
                    .HasForeignKey(d => d._ptas_unitid_value)
                    .HasConstraintName("FK_ptas_condounit_ptas_buildingdetail_commercialuse_ptas_unitid");
            });

            modelBuilder.Entity<ptas_buildingdetail_commercialuse_snapshot>(entity =>
            {
                entity.HasKey(e => e.ptas_buildingdetail_commercialuseid);

                entity.ToTable("ptas_buildingdetail_commercialuse_snapshot", "dynamics");

                entity.Property(e => e.ptas_buildingdetail_commercialuseid).ValueGeneratedNever();
            });

            modelBuilder.Entity<ptas_buildingdetail_ptas_mediarepository>(entity =>
            {
                entity.ToTable("ptas_buildingdetail_ptas_mediarepository", "dynamics");

                entity.HasIndex(e => e.modifiedon)
                    .HasName("Idx_ptas_buildingdetail_ptas_mediarepository_modifiedon");

                entity.Property(e => e.ptas_buildingdetail_ptas_mediarepositoryid).ValueGeneratedNever();

                entity.Property(e => e.modifiedon)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");
            });

            modelBuilder.Entity<ptas_buildingdetail_snapshot>(entity =>
            {
                entity.HasKey(e => e.ptas_buildingdetailid);

                entity.ToTable("ptas_buildingdetail_snapshot", "dynamics");

                entity.Property(e => e.ptas_buildingdetailid).ValueGeneratedNever();

                entity.Property(e => e.exchangerate).HasColumnType("money");

                entity.Property(e => e.ptas_additionalcost).HasColumnType("money");

                entity.Property(e => e.ptas_additionalcost_base).HasColumnType("money");

                entity.Property(e => e.ptas_numberofstoriesdecimal).HasColumnType("money");
            });

            modelBuilder.Entity<ptas_buildingsectionfeature>(entity =>
            {
                entity.ToTable("ptas_buildingsectionfeature", "dynamics");

                entity.HasIndex(e => e.modifiedon)
                    .HasName("Idx_ptas_buildingsectionfeature_modifiedon");

                entity.Property(e => e.ptas_buildingsectionfeatureid).ValueGeneratedNever();

                entity.HasOne(d => d._createdby_valueNavigation)
                    .WithMany(p => p.ptas_buildingsectionfeature_createdby_valueNavigation)
                    .HasForeignKey(d => d._createdby_value)
                    .HasConstraintName("FK_systemuser_ptas_buildingsectionfeature_createdby");

                entity.HasOne(d => d._createdonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_buildingsectionfeature_createdonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._createdonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_buildingsectionfeature_createdonbehalfby");

                entity.HasOne(d => d._modifiedby_valueNavigation)
                    .WithMany(p => p.ptas_buildingsectionfeature_modifiedby_valueNavigation)
                    .HasForeignKey(d => d._modifiedby_value)
                    .HasConstraintName("FK_systemuser_ptas_buildingsectionfeature_modifiedby");

                entity.HasOne(d => d._modifiedonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_buildingsectionfeature_modifiedonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._modifiedonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_buildingsectionfeature_modifiedonbehalfby");

                entity.HasOne(d => d._owningteam_valueNavigation)
                    .WithMany(p => p.ptas_buildingsectionfeature)
                    .HasForeignKey(d => d._owningteam_value)
                    .HasConstraintName("FK_team_ptas_buildingsectionfeature_owningteam");

                entity.HasOne(d => d._owninguser_valueNavigation)
                    .WithMany(p => p.ptas_buildingsectionfeature_owninguser_valueNavigation)
                    .HasForeignKey(d => d._owninguser_value)
                    .HasConstraintName("FK_systemuser_ptas_buildingsectionfeature_owninguser");

                entity.HasOne(d => d._ptas_building_valueNavigation)
                    .WithMany(p => p.ptas_buildingsectionfeature)
                    .HasForeignKey(d => d._ptas_building_value)
                    .HasConstraintName("FK_ptas_buildingdetail_ptas_buildingsectionfeature_ptas_building");

                entity.HasOne(d => d._ptas_buildingsectionuseid_valueNavigation)
                    .WithMany(p => p.ptas_buildingsectionfeature)
                    .HasForeignKey(d => d._ptas_buildingsectionuseid_value)
                    .HasConstraintName("FK_ptas_buildingdetail_commercialuse_ptas_buildingsectionfeature_ptas_buildingsectionuseid");

                entity.HasOne(d => d._ptas_masterfeatureid_valueNavigation)
                    .WithMany(p => p.Inverse_ptas_masterfeatureid_valueNavigation)
                    .HasForeignKey(d => d._ptas_masterfeatureid_value)
                    .HasConstraintName("FK_ptas_buildingsectionfeature_ptas_buildingsectionfeature_ptas_masterfeatureid");
            });

            modelBuilder.Entity<ptas_buildingsectionfeature_snapshot>(entity =>
            {
                entity.HasKey(e => e.ptas_buildingsectionfeatureid);

                entity.ToTable("ptas_buildingsectionfeature_snapshot", "dynamics");

                entity.Property(e => e.ptas_buildingsectionfeatureid).ValueGeneratedNever();
            });

            modelBuilder.Entity<ptas_buildingsectionuse>(entity =>
            {
                entity.ToTable("ptas_buildingsectionuse", "dynamics");

                entity.HasIndex(e => e.modifiedon)
                    .HasName("Idx_ptas_buildingsectionuse_modifiedon");

                entity.Property(e => e.ptas_buildingsectionuseid).ValueGeneratedNever();

                entity.HasOne(d => d._createdby_valueNavigation)
                    .WithMany(p => p.ptas_buildingsectionuse_createdby_valueNavigation)
                    .HasForeignKey(d => d._createdby_value)
                    .HasConstraintName("FK_systemuser_ptas_buildingsectionuse_createdby");

                entity.HasOne(d => d._createdonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_buildingsectionuse_createdonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._createdonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_buildingsectionuse_createdonbehalfby");

                entity.HasOne(d => d._modifiedby_valueNavigation)
                    .WithMany(p => p.ptas_buildingsectionuse_modifiedby_valueNavigation)
                    .HasForeignKey(d => d._modifiedby_value)
                    .HasConstraintName("FK_systemuser_ptas_buildingsectionuse_modifiedby");

                entity.HasOne(d => d._modifiedonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_buildingsectionuse_modifiedonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._modifiedonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_buildingsectionuse_modifiedonbehalfby");
            });

            modelBuilder.Entity<ptas_camanotes>(entity =>
            {
                entity.ToTable("ptas_camanotes", "dynamics");

                entity.HasIndex(e => e.modifiedon)
                    .HasName("Idx_ptas_camanotes_modifiedon");

                entity.Property(e => e.ptas_camanotesid).ValueGeneratedNever();
            });

            modelBuilder.Entity<ptas_camanotes_ptas_mediarepository>(entity =>
            {
                entity.ToTable("ptas_camanotes_ptas_mediarepository", "dynamics");

                entity.Property(e => e.ptas_camanotes_ptas_mediarepositoryid).ValueGeneratedNever();

                entity.Property(e => e.modifiedon)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");
            });

            modelBuilder.Entity<ptas_changehistory>(entity =>
            {
                entity.HasKey(e => e.ChngDtlGuid)
                    .HasName("PK_chngDtlGuid")
                    .IsClustered(false);

                entity.ToTable("ptas_changehistory", "ptas");

                entity.HasIndex(e => e.detailID)
                    .HasName("idx_DetailId");

                entity.HasIndex(e => new { e.Major, e.Minor })
                    .HasName("idx_MajMin");

                entity.HasIndex(e => new { e.UpdateDate, e.EventDate, e.parcelGuid })
                    .HasName("idx_parcelGuid");

                entity.HasIndex(e => new { e.parcelGuid, e.Major, e.Minor, e.ChngDtlGuid, e.ChngGuid, e.modifiedRecordName, e.modifiedRecordPK })
                    .HasName("idx_ModRecPK");

                entity.Property(e => e.ChngDtlGuid).ValueGeneratedNever();

                entity.Property(e => e.DocId)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.EventDate).HasColumnType("smalldatetime");

                entity.Property(e => e.IDValueNew)
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entity.Property(e => e.IDValueOriginal)
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

                entity.Property(e => e.PropStatus)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.UpdateDate).HasColumnType("smalldatetime");

                entity.Property(e => e.attribDispName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.attribSchemaName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.dataTypeDesc)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.detailID).ValueGeneratedOnAdd();

                entity.Property(e => e.displayValueNew)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.displayValueNewMulti).HasColumnType("ntext");

                entity.Property(e => e.displayValueOriginal)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.displayValueOriginalMulti).HasColumnType("ntext");

                entity.Property(e => e.entityDispName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.entitySchemaName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.modifiedRecordName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.modifiedRecordNewName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.parentRecordName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ptasName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.updatedBy).HasMaxLength(150);
            });

            modelBuilder.Entity<ptas_changereason>(entity =>
            {
                entity.ToTable("ptas_changereason", "dynamics");

                entity.Property(e => e.ptas_changereasonid).ValueGeneratedNever();

                entity.Property(e => e.ptas_changereason1).HasColumnName("ptas_changereason");

                entity.HasOne(d => d._createdby_valueNavigation)
                    .WithMany(p => p.ptas_changereason_createdby_valueNavigation)
                    .HasForeignKey(d => d._createdby_value)
                    .HasConstraintName("FK_systemuser_ptas_changereason_createdby");

                entity.HasOne(d => d._createdonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_changereason_createdonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._createdonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_changereason_createdonbehalfby");

                entity.HasOne(d => d._modifiedby_valueNavigation)
                    .WithMany(p => p.ptas_changereason_modifiedby_valueNavigation)
                    .HasForeignKey(d => d._modifiedby_value)
                    .HasConstraintName("FK_systemuser_ptas_changereason_modifiedby");

                entity.HasOne(d => d._modifiedonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_changereason_modifiedonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._modifiedonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_changereason_modifiedonbehalfby");

                entity.HasOne(d => d._owningteam_valueNavigation)
                    .WithMany(p => p.ptas_changereason)
                    .HasForeignKey(d => d._owningteam_value)
                    .HasConstraintName("FK_team_ptas_changereason_owningteam");

                entity.HasOne(d => d._owninguser_valueNavigation)
                    .WithMany(p => p.ptas_changereason_owninguser_valueNavigation)
                    .HasForeignKey(d => d._owninguser_value)
                    .HasConstraintName("FK_systemuser_ptas_changereason_owninguser");
            });

            modelBuilder.Entity<ptas_city>(entity =>
            {
                entity.ToTable("ptas_city", "dynamics");

                entity.HasIndex(e => e.modifiedon)
                    .HasName("Idx_ptas_city_modifiedon");

                entity.Property(e => e.ptas_cityid).ValueGeneratedNever();

                entity.HasOne(d => d._createdby_valueNavigation)
                    .WithMany(p => p.ptas_city_createdby_valueNavigation)
                    .HasForeignKey(d => d._createdby_value)
                    .HasConstraintName("FK_systemuser_ptas_city_createdby");

                entity.HasOne(d => d._createdonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_city_createdonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._createdonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_city_createdonbehalfby");

                entity.HasOne(d => d._modifiedby_valueNavigation)
                    .WithMany(p => p.ptas_city_modifiedby_valueNavigation)
                    .HasForeignKey(d => d._modifiedby_value)
                    .HasConstraintName("FK_systemuser_ptas_city_modifiedby");

                entity.HasOne(d => d._modifiedonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_city_modifiedonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._modifiedonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_city_modifiedonbehalfby");
            });

            modelBuilder.Entity<ptas_city_county>(entity =>
            {
                entity.ToTable("ptas_city_county", "dynamics");

                entity.HasIndex(e => e.modifiedon)
                    .HasName("Idx_ptas_city_county_modifiedon");

                entity.Property(e => e.ptas_city_countyid).ValueGeneratedNever();

                entity.Property(e => e.modifiedon)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");
            });

            modelBuilder.Entity<ptas_city_stateorprovince>(entity =>
            {
                entity.ToTable("ptas_city_stateorprovince", "dynamics");

                entity.HasIndex(e => e.modifiedon)
                    .HasName("Idx_ptas_city_stateorprovince_modifiedon");

                entity.Property(e => e.ptas_city_stateorprovinceid).ValueGeneratedNever();

                entity.Property(e => e.modifiedon)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");
            });

            modelBuilder.Entity<ptas_city_zipcode>(entity =>
            {
                entity.ToTable("ptas_city_zipcode", "dynamics");

                entity.HasIndex(e => e.modifiedon)
                    .HasName("Idx_ptas_city_zipcode_modifiedon");

                entity.Property(e => e.ptas_city_zipcodeid).ValueGeneratedNever();

                entity.Property(e => e.modifiedon)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");
            });

            modelBuilder.Entity<ptas_condocomplex>(entity =>
            {
                entity.ToTable("ptas_condocomplex", "dynamics");

                entity.HasIndex(e => e.modifiedon)
                    .HasName("Idx_ptas_condocomplex_modifiedon");

                entity.Property(e => e.ptas_condocomplexid).ValueGeneratedNever();

                entity.Property(e => e.exchangerate).HasColumnType("money");

                entity.Property(e => e.ptas_avnrasqft).HasColumnType("money");

                entity.Property(e => e.ptas_avnrasqft_base).HasColumnType("money");

                entity.Property(e => e.ptas_bathtosleepingroomratio).HasColumnType("money");

                entity.Property(e => e.ptas_kitchentosleepingroomratio).HasColumnType("money");

                entity.Property(e => e.ptas_lbratio).HasColumnType("money");

                entity.Property(e => e.ptas_parkingoperatingexpensepct).HasColumnType("money");

                entity.Property(e => e.ptas_parkingratio).HasColumnType("money");

                entity.Property(e => e.ptas_percentlandvaluedecimal).HasColumnType("money");

                entity.Property(e => e.ptas_percentownershipdecimal).HasColumnType("money");

                entity.Property(e => e.ptas_percenttotalvaluedecimal).HasColumnType("money");

                entity.Property(e => e.ptas_totalassessedvalue).HasColumnType("money");

                entity.Property(e => e.ptas_totalassessedvalue_base).HasColumnType("money");

                entity.Property(e => e.ptas_valaddcaprate).HasColumnType("money");

                entity.HasOne(d => d._createdby_valueNavigation)
                    .WithMany(p => p.ptas_condocomplex_createdby_valueNavigation)
                    .HasForeignKey(d => d._createdby_value)
                    .HasConstraintName("FK_systemuser_ptas_condocomplex_createdby");

                entity.HasOne(d => d._createdonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_condocomplex_createdonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._createdonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_condocomplex_createdonbehalfby");

                entity.HasOne(d => d._modifiedby_valueNavigation)
                    .WithMany(p => p.ptas_condocomplex_modifiedby_valueNavigation)
                    .HasForeignKey(d => d._modifiedby_value)
                    .HasConstraintName("FK_systemuser_ptas_condocomplex_modifiedby");

                entity.HasOne(d => d._modifiedonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_condocomplex_modifiedonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._modifiedonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_condocomplex_modifiedonbehalfby");

                entity.HasOne(d => d._owningteam_valueNavigation)
                    .WithMany(p => p.ptas_condocomplex)
                    .HasForeignKey(d => d._owningteam_value)
                    .HasConstraintName("FK_team_ptas_condocomplex_owningteam");

                entity.HasOne(d => d._owninguser_valueNavigation)
                    .WithMany(p => p.ptas_condocomplex_owninguser_valueNavigation)
                    .HasForeignKey(d => d._owninguser_value)
                    .HasConstraintName("FK_systemuser_ptas_condocomplex_owninguser");

                entity.HasOne(d => d._ptas_accessoryid_valueNavigation)
                    .WithMany(p => p.ptas_condocomplex)
                    .HasForeignKey(d => d._ptas_accessoryid_value)
                    .HasConstraintName("FK_ptas_accessorydetail_ptas_condocomplex_ptas_accessoryid");

                entity.HasOne(d => d._ptas_addr1_cityid_valueNavigation)
                    .WithMany(p => p.ptas_condocomplex)
                    .HasForeignKey(d => d._ptas_addr1_cityid_value)
                    .HasConstraintName("FK_ptas_city_ptas_condocomplex_ptas_addr1_cityid");

                entity.HasOne(d => d._ptas_addr1_countryid_valueNavigation)
                    .WithMany(p => p.ptas_condocomplex)
                    .HasForeignKey(d => d._ptas_addr1_countryid_value)
                    .HasConstraintName("FK_ptas_country_ptas_condocomplex_ptas_addr1_countryid");

                entity.HasOne(d => d._ptas_addr1_stateid_valueNavigation)
                    .WithMany(p => p.ptas_condocomplex)
                    .HasForeignKey(d => d._ptas_addr1_stateid_value)
                    .HasConstraintName("FK_ptas_stateorprovince_ptas_condocomplex_ptas_addr1_stateid");

                entity.HasOne(d => d._ptas_addr1_zipcode_valueNavigation)
                    .WithMany(p => p.ptas_condocomplex)
                    .HasForeignKey(d => d._ptas_addr1_zipcode_value)
                    .HasConstraintName("FK_ptas_zipcode_ptas_condocomplex_ptas_addr1_zipcode");

                entity.HasOne(d => d._ptas_associatedparcel2id_valueNavigation)
                    .WithMany(p => p.ptas_condocomplex_ptas_associatedparcel2id_valueNavigation)
                    .HasForeignKey(d => d._ptas_associatedparcel2id_value)
                    .HasConstraintName("FK_ptas_parceldetail_ptas_condocomplex_ptas_associatedparcel2id");

                entity.HasOne(d => d._ptas_associatedparcel3id_valueNavigation)
                    .WithMany(p => p.ptas_condocomplex_ptas_associatedparcel3id_valueNavigation)
                    .HasForeignKey(d => d._ptas_associatedparcel3id_value)
                    .HasConstraintName("FK_ptas_parceldetail_ptas_condocomplex_ptas_associatedparcel3id");

                entity.HasOne(d => d._ptas_associatedparcelid_valueNavigation)
                    .WithMany(p => p.ptas_condocomplex_ptas_associatedparcelid_valueNavigation)
                    .HasForeignKey(d => d._ptas_associatedparcelid_value)
                    .HasConstraintName("FK_ptas_parceldetail_ptas_condocomplex_ptas_associatedparcelid");

                entity.HasOne(d => d._ptas_contaminationproject_valueNavigation)
                    .WithMany(p => p.ptas_condocomplex)
                    .HasForeignKey(d => d._ptas_contaminationproject_value)
                    .HasConstraintName("FK_ptas_contaminationproject_ptas_condocomplex_ptas_contaminationproject");

                entity.HasOne(d => d._ptas_economicunitid_valueNavigation)
                    .WithMany(p => p.ptas_condocomplex)
                    .HasForeignKey(d => d._ptas_economicunitid_value)
                    .HasConstraintName("FK_ptas_economicunit_ptas_condocomplex_ptas_economicunitid");

                entity.HasOne(d => d._ptas_majorcondocomplexid_valueNavigation)
                    .WithMany(p => p.Inverse_ptas_majorcondocomplexid_valueNavigation)
                    .HasForeignKey(d => d._ptas_majorcondocomplexid_value)
                    .HasConstraintName("FK_ptas_condocomplex_ptas_condocomplex_ptas_majorcondocomplexid");

                entity.HasOne(d => d._ptas_masterprojectid_valueNavigation)
                    .WithMany(p => p.Inverse_ptas_masterprojectid_valueNavigation)
                    .HasForeignKey(d => d._ptas_masterprojectid_value)
                    .HasConstraintName("FK_ptas_condocomplex_ptas_condocomplex_ptas_masterprojectid");

                entity.HasOne(d => d._ptas_parcelid_valueNavigation)
                    .WithMany(p => p.ptas_condocomplex_ptas_parcelid_valueNavigation)
                    .HasForeignKey(d => d._ptas_parcelid_value)
                    .HasConstraintName("FK_ptas_parceldetail_ptas_condocomplex_ptas_parcelid");

                entity.HasOne(d => d._ptas_parkingdistrictid_valueNavigation)
                    .WithMany(p => p.ptas_condocomplex)
                    .HasForeignKey(d => d._ptas_parkingdistrictid_value)
                    .HasConstraintName("FK_ptas_parkingdistrict_ptas_condocomplex_ptas_parkingdistrictid");
            });

            modelBuilder.Entity<ptas_condocomplex_ptas_mediarepository>(entity =>
            {
                entity.ToTable("ptas_condocomplex_ptas_mediarepository", "dynamics");

                entity.HasIndex(e => e.modifiedon)
                    .HasName("Idx_ptas_condocomplex_ptas_mediarepository_modifiedon");

                entity.Property(e => e.ptas_condocomplex_ptas_mediarepositoryid).ValueGeneratedNever();

                entity.Property(e => e.modifiedon)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");
            });

            modelBuilder.Entity<ptas_condocomplex_snapshot>(entity =>
            {
                entity.HasKey(e => e.ptas_condocomplexid);

                entity.ToTable("ptas_condocomplex_snapshot", "dynamics");

                entity.Property(e => e.ptas_condocomplexid).ValueGeneratedNever();

                entity.Property(e => e.exchangerate).HasColumnType("money");

                entity.Property(e => e.ptas_avnrasqft).HasColumnType("money");

                entity.Property(e => e.ptas_avnrasqft_base).HasColumnType("money");

                entity.Property(e => e.ptas_bathtosleepingroomratio).HasColumnType("money");

                entity.Property(e => e.ptas_kitchentosleepingroomratio).HasColumnType("money");

                entity.Property(e => e.ptas_lbratio).HasColumnType("money");

                entity.Property(e => e.ptas_parkingoperatingexpensepct).HasColumnType("money");

                entity.Property(e => e.ptas_parkingratio).HasColumnType("money");

                entity.Property(e => e.ptas_percentlandvaluedecimal).HasColumnType("money");

                entity.Property(e => e.ptas_percentownershipdecimal).HasColumnType("money");

                entity.Property(e => e.ptas_percenttotalvaluedecimal).HasColumnType("money");

                entity.Property(e => e.ptas_totalassessedvalue).HasColumnType("money");

                entity.Property(e => e.ptas_totalassessedvalue_base).HasColumnType("money");

                entity.Property(e => e.ptas_valaddcaprate).HasColumnType("money");
            });

            modelBuilder.Entity<ptas_condounit>(entity =>
            {
                entity.ToTable("ptas_condounit", "dynamics");

                entity.HasIndex(e => e.modifiedon)
                    .HasName("Idx_ptas_condounit_modifiedon");

                entity.Property(e => e.ptas_condounitid).ValueGeneratedNever();

                entity.Property(e => e.exchangerate).HasColumnType("money");

                entity.Property(e => e.ptas_accessoryflatvalue).HasColumnType("money");

                entity.Property(e => e.ptas_accessoryflatvalue_base).HasColumnType("money");

                entity.Property(e => e.ptas_improvementsvalue).HasColumnType("money");

                entity.Property(e => e.ptas_improvementsvalue_base).HasColumnType("money");

                entity.Property(e => e.ptas_landvalue).HasColumnType("money");

                entity.Property(e => e.ptas_landvalue_base).HasColumnType("money");

                entity.Property(e => e.ptas_newconstructionvalue).HasColumnType("money");

                entity.Property(e => e.ptas_newconstructionvalue_base).HasColumnType("money");

                entity.Property(e => e.ptas_percentlandvaluedecimal).HasColumnType("money");

                entity.Property(e => e.ptas_percentownershipdecimal).HasColumnType("money");

                entity.Property(e => e.ptas_percenttotalvaluedecimal).HasColumnType("money");

                entity.Property(e => e.ptas_smallhomeadjustment).HasColumnType("money");

                entity.Property(e => e.ptas_totalvalue).HasColumnType("money");

                entity.Property(e => e.ptas_totalvalue_base).HasColumnType("money");

                entity.HasOne(d => d._createdby_valueNavigation)
                    .WithMany(p => p.ptas_condounit_createdby_valueNavigation)
                    .HasForeignKey(d => d._createdby_value)
                    .HasConstraintName("FK_systemuser_ptas_condounit_createdby");

                entity.HasOne(d => d._createdonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_condounit_createdonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._createdonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_condounit_createdonbehalfby");

                entity.HasOne(d => d._modifiedby_valueNavigation)
                    .WithMany(p => p.ptas_condounit_modifiedby_valueNavigation)
                    .HasForeignKey(d => d._modifiedby_value)
                    .HasConstraintName("FK_systemuser_ptas_condounit_modifiedby");

                entity.HasOne(d => d._modifiedonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_condounit_modifiedonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._modifiedonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_condounit_modifiedonbehalfby");

                entity.HasOne(d => d._owningteam_valueNavigation)
                    .WithMany(p => p.ptas_condounit)
                    .HasForeignKey(d => d._owningteam_value)
                    .HasConstraintName("FK_team_ptas_condounit_owningteam");

                entity.HasOne(d => d._owninguser_valueNavigation)
                    .WithMany(p => p.ptas_condounit_owninguser_valueNavigation)
                    .HasForeignKey(d => d._owninguser_value)
                    .HasConstraintName("FK_systemuser_ptas_condounit_owninguser");

                entity.HasOne(d => d._ptas_addr1_cityid_valueNavigation)
                    .WithMany(p => p.ptas_condounit)
                    .HasForeignKey(d => d._ptas_addr1_cityid_value)
                    .HasConstraintName("FK_ptas_city_ptas_condounit_ptas_addr1_cityid");

                entity.HasOne(d => d._ptas_addr1_countryid_valueNavigation)
                    .WithMany(p => p.ptas_condounit)
                    .HasForeignKey(d => d._ptas_addr1_countryid_value)
                    .HasConstraintName("FK_ptas_country_ptas_condounit_ptas_addr1_countryid");

                entity.HasOne(d => d._ptas_addr1_stateid_valueNavigation)
                    .WithMany(p => p.ptas_condounit)
                    .HasForeignKey(d => d._ptas_addr1_stateid_value)
                    .HasConstraintName("FK_ptas_stateorprovince_ptas_condounit_ptas_addr1_stateid");

                entity.HasOne(d => d._ptas_addr1_streetnameid_valueNavigation)
                    .WithMany(p => p.ptas_condounit)
                    .HasForeignKey(d => d._ptas_addr1_streetnameid_value)
                    .HasConstraintName("FK_ptas_streetname_ptas_condounit_ptas_addr1_streetnameid");

                entity.HasOne(d => d._ptas_addr1_streettypeid_valueNavigation)
                    .WithMany(p => p.ptas_condounit)
                    .HasForeignKey(d => d._ptas_addr1_streettypeid_value)
                    .HasConstraintName("FK_ptas_streettype_ptas_condounit_ptas_addr1_streettypeid");

                entity.HasOne(d => d._ptas_addr1_zipcodeid_valueNavigation)
                    .WithMany(p => p.ptas_condounit)
                    .HasForeignKey(d => d._ptas_addr1_zipcodeid_value)
                    .HasConstraintName("FK_ptas_zipcode_ptas_condounit_ptas_addr1_zipcodeid");

                entity.HasOne(d => d._ptas_buildingid_valueNavigation)
                    .WithMany(p => p.ptas_condounit)
                    .HasForeignKey(d => d._ptas_buildingid_value)
                    .HasConstraintName("FK_ptas_buildingdetail_ptas_condounit_ptas_buildingid");

                entity.HasOne(d => d._ptas_complexid_valueNavigation)
                    .WithMany(p => p.ptas_condounit)
                    .HasForeignKey(d => d._ptas_complexid_value)
                    .HasConstraintName("FK_ptas_condocomplex_ptas_condounit_ptas_complexid");

                entity.HasOne(d => d._ptas_dock_valueNavigation)
                    .WithMany(p => p.ptas_condounit)
                    .HasForeignKey(d => d._ptas_dock_value)
                    .HasConstraintName("FK_ptas_projectdock_ptas_condounit_ptas_dock");

                entity.HasOne(d => d._ptas_masterunitid_valueNavigation)
                    .WithMany(p => p.Inverse_ptas_masterunitid_valueNavigation)
                    .HasForeignKey(d => d._ptas_masterunitid_value)
                    .HasConstraintName("FK_ptas_condounit_ptas_condounit_ptas_masterunitid");

                entity.HasOne(d => d._ptas_parcelid_valueNavigation)
                    .WithMany(p => p.ptas_condounit)
                    .HasForeignKey(d => d._ptas_parcelid_value)
                    .HasConstraintName("FK_ptas_parceldetail_ptas_condounit_ptas_parcelid");

                entity.HasOne(d => d._ptas_propertytypeid_valueNavigation)
                    .WithMany(p => p.ptas_condounit)
                    .HasForeignKey(d => d._ptas_propertytypeid_value)
                    .HasConstraintName("FK_ptas_propertytype_ptas_condounit_ptas_propertytypeid");

                entity.HasOne(d => d._ptas_responsibilityid_valueNavigation)
                    .WithMany(p => p.ptas_condounit)
                    .HasForeignKey(d => d._ptas_responsibilityid_value)
                    .HasConstraintName("FK_ptas_responsibility_ptas_condounit_ptas_responsibilityid");

                entity.HasOne(d => d._ptas_selectbyid_valueNavigation)
                    .WithMany(p => p.ptas_condounit_ptas_selectbyid_valueNavigation)
                    .HasForeignKey(d => d._ptas_selectbyid_value)
                    .HasConstraintName("FK_systemuser_ptas_condounit_ptas_selectbyid");

                entity.HasOne(d => d._ptas_sketchid_valueNavigation)
                    .WithMany(p => p.ptas_condounit)
                    .HasForeignKey(d => d._ptas_sketchid_value)
                    .HasConstraintName("FK_ptas_sketch_ptas_condounit_ptas_sketchid");

                entity.HasOne(d => d._ptas_specialtyareaid_valueNavigation)
                    .WithMany(p => p.ptas_condounit)
                    .HasForeignKey(d => d._ptas_specialtyareaid_value)
                    .HasConstraintName("FK_ptas_specialtyarea_ptas_condounit_ptas_specialtyareaid");

                entity.HasOne(d => d._ptas_specialtynbhdid_valueNavigation)
                    .WithMany(p => p.ptas_condounit)
                    .HasForeignKey(d => d._ptas_specialtynbhdid_value)
                    .HasConstraintName("FK_ptas_specialtyneighborhood_ptas_condounit_ptas_specialtynbhdid");

                entity.HasOne(d => d._ptas_taxaccountid_valueNavigation)
                    .WithMany(p => p.ptas_condounit)
                    .HasForeignKey(d => d._ptas_taxaccountid_value)
                    .HasConstraintName("FK_ptas_taxaccount_ptas_condounit_ptas_taxaccountid");

                entity.HasOne(d => d._ptas_unitinspectedbyid_valueNavigation)
                    .WithMany(p => p.ptas_condounit_ptas_unitinspectedbyid_valueNavigation)
                    .HasForeignKey(d => d._ptas_unitinspectedbyid_value)
                    .HasConstraintName("FK_systemuser_ptas_condounit_ptas_unitinspectedbyid");
            });

            modelBuilder.Entity<ptas_condounit_ptas_mediarepository>(entity =>
            {
                entity.ToTable("ptas_condounit_ptas_mediarepository", "dynamics");

                entity.HasIndex(e => e.modifiedon)
                    .HasName("Idx_ptas_condounit_ptas_mediarepository_modifiedon");

                entity.Property(e => e.ptas_condounit_ptas_mediarepositoryid).ValueGeneratedNever();

                entity.Property(e => e.modifiedon)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");
            });

            modelBuilder.Entity<ptas_condounit_snapshot>(entity =>
            {
                entity.HasKey(e => e.ptas_condounitid);

                entity.ToTable("ptas_condounit_snapshot", "dynamics");

                entity.Property(e => e.ptas_condounitid).ValueGeneratedNever();

                entity.Property(e => e.exchangerate).HasColumnType("money");

                entity.Property(e => e.ptas_accessoryflatvalue).HasColumnType("money");

                entity.Property(e => e.ptas_accessoryflatvalue_base).HasColumnType("money");

                entity.Property(e => e.ptas_improvementsvalue).HasColumnType("money");

                entity.Property(e => e.ptas_improvementsvalue_base).HasColumnType("money");

                entity.Property(e => e.ptas_landvalue).HasColumnType("money");

                entity.Property(e => e.ptas_landvalue_base).HasColumnType("money");

                entity.Property(e => e.ptas_newconstructionvalue).HasColumnType("money");

                entity.Property(e => e.ptas_newconstructionvalue_base).HasColumnType("money");

                entity.Property(e => e.ptas_percentlandvaluedecimal).HasColumnType("money");

                entity.Property(e => e.ptas_percentownershipdecimal).HasColumnType("money");

                entity.Property(e => e.ptas_percenttotalvaluedecimal).HasColumnType("money");

                entity.Property(e => e.ptas_smallhomeadjustment).HasColumnType("money");

                entity.Property(e => e.ptas_totalvalue).HasColumnType("money");

                entity.Property(e => e.ptas_totalvalue_base).HasColumnType("money");
            });

            modelBuilder.Entity<ptas_contaminatedlandreduction>(entity =>
            {
                entity.ToTable("ptas_contaminatedlandreduction", "dynamics");

                entity.Property(e => e.ptas_contaminatedlandreductionid).ValueGeneratedNever();

                entity.Property(e => e.exchangerate).HasColumnType("money");

                entity.Property(e => e.ptas_assessedvaluereduction).HasColumnType("money");

                entity.Property(e => e.ptas_assessedvaluereduction_base).HasColumnType("money");

                entity.Property(e => e.ptas_baselandvalue).HasColumnType("money");

                entity.Property(e => e.ptas_baselandvalue_base).HasColumnType("money");

                entity.Property(e => e.ptas_improvementvalue).HasColumnType("money");

                entity.Property(e => e.ptas_improvementvalue_base).HasColumnType("money");

                entity.Property(e => e.ptas_landreducedvalue).HasColumnType("money");

                entity.Property(e => e.ptas_landreducedvalue_base).HasColumnType("money");

                entity.Property(e => e.ptas_landreducedvaluerounded).HasColumnType("money");

                entity.Property(e => e.ptas_landreducedvaluerounded_base).HasColumnType("money");

                entity.Property(e => e.ptas_percentremediationcost).HasColumnType("money");

                entity.Property(e => e.ptas_presentcost).HasColumnType("money");

                entity.Property(e => e.ptas_presentcost_base).HasColumnType("money");

                entity.Property(e => e.ptas_totalvalue).HasColumnType("money");

                entity.Property(e => e.ptas_totalvalue_base).HasColumnType("money");

                entity.HasOne(d => d._createdby_valueNavigation)
                    .WithMany(p => p.ptas_contaminatedlandreduction_createdby_valueNavigation)
                    .HasForeignKey(d => d._createdby_value)
                    .HasConstraintName("FK_systemuser_ptas_contaminatedlandreduction_createdby");

                entity.HasOne(d => d._createdonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_contaminatedlandreduction_createdonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._createdonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_contaminatedlandreduction_createdonbehalfby");

                entity.HasOne(d => d._modifiedby_valueNavigation)
                    .WithMany(p => p.ptas_contaminatedlandreduction_modifiedby_valueNavigation)
                    .HasForeignKey(d => d._modifiedby_value)
                    .HasConstraintName("FK_systemuser_ptas_contaminatedlandreduction_modifiedby");

                entity.HasOne(d => d._modifiedonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_contaminatedlandreduction_modifiedonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._modifiedonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_contaminatedlandreduction_modifiedonbehalfby");

                entity.HasOne(d => d._owningteam_valueNavigation)
                    .WithMany(p => p.ptas_contaminatedlandreduction)
                    .HasForeignKey(d => d._owningteam_value)
                    .HasConstraintName("FK_team_ptas_contaminatedlandreduction_owningteam");

                entity.HasOne(d => d._owninguser_valueNavigation)
                    .WithMany(p => p.ptas_contaminatedlandreduction_owninguser_valueNavigation)
                    .HasForeignKey(d => d._owninguser_value)
                    .HasConstraintName("FK_systemuser_ptas_contaminatedlandreduction_owninguser");

                entity.HasOne(d => d._ptas_assessmentyearid_valueNavigation)
                    .WithMany(p => p.ptas_contaminatedlandreduction)
                    .HasForeignKey(d => d._ptas_assessmentyearid_value)
                    .HasConstraintName("FK_ptas_year_ptas_contaminatedlandreduction_ptas_assessmentyearid");

                entity.HasOne(d => d._ptas_contaminatedprojectid_valueNavigation)
                    .WithMany(p => p.ptas_contaminatedlandreduction)
                    .HasForeignKey(d => d._ptas_contaminatedprojectid_value)
                    .HasConstraintName("FK_ptas_contaminationproject_ptas_contaminatedlandreduction_ptas_contaminatedprojectid");

                entity.HasOne(d => d._ptas_parcelid_valueNavigation)
                    .WithMany(p => p.ptas_contaminatedlandreduction)
                    .HasForeignKey(d => d._ptas_parcelid_value)
                    .HasConstraintName("FK_ptas_parceldetail_ptas_contaminatedlandreduction_ptas_parcelid");
            });

            modelBuilder.Entity<ptas_contaminationproject>(entity =>
            {
                entity.ToTable("ptas_contaminationproject", "dynamics");

                entity.Property(e => e.ptas_contaminationprojectid).ValueGeneratedNever();

                entity.Property(e => e.exchangerate).HasColumnType("money");

                entity.Property(e => e.ptas_10yeartreasuryrate).HasColumnType("money");

                entity.Property(e => e.ptas_presentcost).HasColumnType("money");

                entity.Property(e => e.ptas_presentcost_base).HasColumnType("money");

                entity.Property(e => e.ptas_totalremediationcost).HasColumnType("money");

                entity.Property(e => e.ptas_totalremediationcost_base).HasColumnType("money");

                entity.HasOne(d => d._createdby_valueNavigation)
                    .WithMany(p => p.ptas_contaminationproject_createdby_valueNavigation)
                    .HasForeignKey(d => d._createdby_value)
                    .HasConstraintName("FK_systemuser_ptas_contaminationproject_createdby");

                entity.HasOne(d => d._createdonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_contaminationproject_createdonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._createdonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_contaminationproject_createdonbehalfby");

                entity.HasOne(d => d._modifiedby_valueNavigation)
                    .WithMany(p => p.ptas_contaminationproject_modifiedby_valueNavigation)
                    .HasForeignKey(d => d._modifiedby_value)
                    .HasConstraintName("FK_systemuser_ptas_contaminationproject_modifiedby");

                entity.HasOne(d => d._modifiedonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_contaminationproject_modifiedonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._modifiedonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_contaminationproject_modifiedonbehalfby");

                entity.HasOne(d => d._owningteam_valueNavigation)
                    .WithMany(p => p.ptas_contaminationproject)
                    .HasForeignKey(d => d._owningteam_value)
                    .HasConstraintName("FK_team_ptas_contaminationproject_owningteam");

                entity.HasOne(d => d._owninguser_valueNavigation)
                    .WithMany(p => p.ptas_contaminationproject_owninguser_valueNavigation)
                    .HasForeignKey(d => d._owninguser_value)
                    .HasConstraintName("FK_systemuser_ptas_contaminationproject_owninguser");
            });

            modelBuilder.Entity<ptas_country>(entity =>
            {
                entity.ToTable("ptas_country", "dynamics");

                entity.HasIndex(e => e.modifiedon)
                    .HasName("Idx_ptas_country_modifiedon");

                entity.Property(e => e.ptas_countryid).ValueGeneratedNever();

                entity.HasOne(d => d._createdby_valueNavigation)
                    .WithMany(p => p.ptas_country_createdby_valueNavigation)
                    .HasForeignKey(d => d._createdby_value)
                    .HasConstraintName("FK_systemuser_ptas_country_createdby");

                entity.HasOne(d => d._createdonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_country_createdonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._createdonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_country_createdonbehalfby");

                entity.HasOne(d => d._modifiedby_valueNavigation)
                    .WithMany(p => p.ptas_country_modifiedby_valueNavigation)
                    .HasForeignKey(d => d._modifiedby_value)
                    .HasConstraintName("FK_systemuser_ptas_country_modifiedby");

                entity.HasOne(d => d._modifiedonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_country_modifiedonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._modifiedonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_country_modifiedonbehalfby");
            });

            modelBuilder.Entity<ptas_county>(entity =>
            {
                entity.ToTable("ptas_county", "dynamics");

                entity.HasIndex(e => e.modifiedon)
                    .HasName("Idx_ptas_county_modifiedon");

                entity.Property(e => e.ptas_countyid).ValueGeneratedNever();

                entity.HasOne(d => d._createdby_valueNavigation)
                    .WithMany(p => p.ptas_county_createdby_valueNavigation)
                    .HasForeignKey(d => d._createdby_value)
                    .HasConstraintName("FK_systemuser_ptas_county_createdby");

                entity.HasOne(d => d._createdonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_county_createdonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._createdonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_county_createdonbehalfby");

                entity.HasOne(d => d._modifiedby_valueNavigation)
                    .WithMany(p => p.ptas_county_modifiedby_valueNavigation)
                    .HasForeignKey(d => d._modifiedby_value)
                    .HasConstraintName("FK_systemuser_ptas_county_modifiedby");

                entity.HasOne(d => d._modifiedonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_county_modifiedonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._modifiedonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_county_modifiedonbehalfby");
            });

            modelBuilder.Entity<ptas_county_stateorprovince>(entity =>
            {
                entity.ToTable("ptas_county_stateorprovince", "dynamics");

                entity.HasIndex(e => e.modifiedon)
                    .HasName("Idx_ptas_county_stateorprovince_modifiedon");

                entity.Property(e => e.ptas_county_stateorprovinceid).ValueGeneratedNever();

                entity.Property(e => e.modifiedon)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");
            });

            modelBuilder.Entity<ptas_county_zipcode>(entity =>
            {
                entity.ToTable("ptas_county_zipcode", "dynamics");

                entity.HasIndex(e => e.modifiedon)
                    .HasName("Idx_ptas_county_zipcode_modifiedon");

                entity.Property(e => e.ptas_county_zipcodeid).ValueGeneratedNever();

                entity.Property(e => e.modifiedon)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");
            });

            modelBuilder.Entity<ptas_depreciationtable>(entity =>
            {
                entity.ToTable("ptas_depreciationtable", "dynamics");

                entity.Property(e => e.ptas_depreciationtableid).ValueGeneratedNever();

                entity.Property(e => e.ptas_interestrate).HasColumnType("money");

                entity.Property(e => e.ptas_minpercentgoodfactor).HasColumnType("money");

                entity.HasOne(d => d._createdby_valueNavigation)
                    .WithMany(p => p.ptas_depreciationtable_createdby_valueNavigation)
                    .HasForeignKey(d => d._createdby_value)
                    .HasConstraintName("FK_systemuser_ptas_depreciationtable_createdby");

                entity.HasOne(d => d._createdonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_depreciationtable_createdonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._createdonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_depreciationtable_createdonbehalfby");

                entity.HasOne(d => d._modifiedby_valueNavigation)
                    .WithMany(p => p.ptas_depreciationtable_modifiedby_valueNavigation)
                    .HasForeignKey(d => d._modifiedby_value)
                    .HasConstraintName("FK_systemuser_ptas_depreciationtable_modifiedby");

                entity.HasOne(d => d._modifiedonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_depreciationtable_modifiedonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._modifiedonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_depreciationtable_modifiedonbehalfby");

                entity.HasOne(d => d._owningteam_valueNavigation)
                    .WithMany(p => p.ptas_depreciationtable)
                    .HasForeignKey(d => d._owningteam_value)
                    .HasConstraintName("FK_team_ptas_depreciationtable_owningteam");

                entity.HasOne(d => d._owninguser_valueNavigation)
                    .WithMany(p => p.ptas_depreciationtable_owninguser_valueNavigation)
                    .HasForeignKey(d => d._owninguser_value)
                    .HasConstraintName("FK_systemuser_ptas_depreciationtable_owninguser");
            });

            modelBuilder.Entity<ptas_district>(entity =>
            {
                entity.ToTable("ptas_district", "dynamics");

                entity.HasIndex(e => e.modifiedon)
                    .HasName("Idx_ptas_district_modifiedon");

                entity.Property(e => e.ptas_districtid).ValueGeneratedNever();

                entity.HasOne(d => d._createdby_valueNavigation)
                    .WithMany(p => p.ptas_district_createdby_valueNavigation)
                    .HasForeignKey(d => d._createdby_value)
                    .HasConstraintName("FK_systemuser_ptas_district_createdby");

                entity.HasOne(d => d._createdonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_district_createdonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._createdonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_district_createdonbehalfby");

                entity.HasOne(d => d._modifiedby_valueNavigation)
                    .WithMany(p => p.ptas_district_modifiedby_valueNavigation)
                    .HasForeignKey(d => d._modifiedby_value)
                    .HasConstraintName("FK_systemuser_ptas_district_modifiedby");

                entity.HasOne(d => d._modifiedonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_district_modifiedonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._modifiedonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_district_modifiedonbehalfby");
            });

            modelBuilder.Entity<ptas_ebscostcenter>(entity =>
            {
                entity.ToTable("ptas_ebscostcenter", "dynamics");

                entity.Property(e => e.ptas_ebscostcenterid).ValueGeneratedNever();

                entity.HasOne(d => d._createdby_valueNavigation)
                    .WithMany(p => p.ptas_ebscostcenter_createdby_valueNavigation)
                    .HasForeignKey(d => d._createdby_value)
                    .HasConstraintName("FK_systemuser_ptas_ebscostcenter_createdby");

                entity.HasOne(d => d._createdonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_ebscostcenter_createdonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._createdonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_ebscostcenter_createdonbehalfby");

                entity.HasOne(d => d._modifiedby_valueNavigation)
                    .WithMany(p => p.ptas_ebscostcenter_modifiedby_valueNavigation)
                    .HasForeignKey(d => d._modifiedby_value)
                    .HasConstraintName("FK_systemuser_ptas_ebscostcenter_modifiedby");

                entity.HasOne(d => d._modifiedonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_ebscostcenter_modifiedonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._modifiedonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_ebscostcenter_modifiedonbehalfby");
            });

            modelBuilder.Entity<ptas_ebsfundnumber>(entity =>
            {
                entity.ToTable("ptas_ebsfundnumber", "dynamics");

                entity.Property(e => e.ptas_ebsfundnumberid).ValueGeneratedNever();

                entity.HasOne(d => d._createdby_valueNavigation)
                    .WithMany(p => p.ptas_ebsfundnumber_createdby_valueNavigation)
                    .HasForeignKey(d => d._createdby_value)
                    .HasConstraintName("FK_systemuser_ptas_ebsfundnumber_createdby");

                entity.HasOne(d => d._createdonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_ebsfundnumber_createdonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._createdonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_ebsfundnumber_createdonbehalfby");

                entity.HasOne(d => d._modifiedby_valueNavigation)
                    .WithMany(p => p.ptas_ebsfundnumber_modifiedby_valueNavigation)
                    .HasForeignKey(d => d._modifiedby_value)
                    .HasConstraintName("FK_systemuser_ptas_ebsfundnumber_modifiedby");

                entity.HasOne(d => d._modifiedonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_ebsfundnumber_modifiedonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._modifiedonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_ebsfundnumber_modifiedonbehalfby");
            });

            modelBuilder.Entity<ptas_ebsmainaccount>(entity =>
            {
                entity.ToTable("ptas_ebsmainaccount", "dynamics");

                entity.Property(e => e.ptas_ebsmainaccountid).ValueGeneratedNever();

                entity.HasOne(d => d._createdby_valueNavigation)
                    .WithMany(p => p.ptas_ebsmainaccount_createdby_valueNavigation)
                    .HasForeignKey(d => d._createdby_value)
                    .HasConstraintName("FK_systemuser_ptas_ebsmainaccount_createdby");

                entity.HasOne(d => d._createdonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_ebsmainaccount_createdonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._createdonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_ebsmainaccount_createdonbehalfby");

                entity.HasOne(d => d._modifiedby_valueNavigation)
                    .WithMany(p => p.ptas_ebsmainaccount_modifiedby_valueNavigation)
                    .HasForeignKey(d => d._modifiedby_value)
                    .HasConstraintName("FK_systemuser_ptas_ebsmainaccount_modifiedby");

                entity.HasOne(d => d._modifiedonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_ebsmainaccount_modifiedonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._modifiedonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_ebsmainaccount_modifiedonbehalfby");
            });

            modelBuilder.Entity<ptas_ebsproject>(entity =>
            {
                entity.ToTable("ptas_ebsproject", "dynamics");

                entity.Property(e => e.ptas_ebsprojectid).ValueGeneratedNever();

                entity.HasOne(d => d._createdby_valueNavigation)
                    .WithMany(p => p.ptas_ebsproject_createdby_valueNavigation)
                    .HasForeignKey(d => d._createdby_value)
                    .HasConstraintName("FK_systemuser_ptas_ebsproject_createdby");

                entity.HasOne(d => d._createdonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_ebsproject_createdonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._createdonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_ebsproject_createdonbehalfby");

                entity.HasOne(d => d._modifiedby_valueNavigation)
                    .WithMany(p => p.ptas_ebsproject_modifiedby_valueNavigation)
                    .HasForeignKey(d => d._modifiedby_value)
                    .HasConstraintName("FK_systemuser_ptas_ebsproject_modifiedby");

                entity.HasOne(d => d._modifiedonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_ebsproject_modifiedonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._modifiedonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_ebsproject_modifiedonbehalfby");
            });

            modelBuilder.Entity<ptas_economicunit>(entity =>
            {
                entity.ToTable("ptas_economicunit", "dynamics");

                entity.HasIndex(e => e.modifiedon)
                    .HasName("Idx_ptas_economicunit_modifiedon");

                entity.Property(e => e.ptas_economicunitid).ValueGeneratedNever();

                entity.HasOne(d => d._createdby_valueNavigation)
                    .WithMany(p => p.ptas_economicunit_createdby_valueNavigation)
                    .HasForeignKey(d => d._createdby_value)
                    .HasConstraintName("FK_systemuser_ptas_economicunit_createdby");

                entity.HasOne(d => d._createdonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_economicunit_createdonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._createdonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_economicunit_createdonbehalfby");

                entity.HasOne(d => d._modifiedby_valueNavigation)
                    .WithMany(p => p.ptas_economicunit_modifiedby_valueNavigation)
                    .HasForeignKey(d => d._modifiedby_value)
                    .HasConstraintName("FK_systemuser_ptas_economicunit_modifiedby");

                entity.HasOne(d => d._modifiedonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_economicunit_modifiedonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._modifiedonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_economicunit_modifiedonbehalfby");

                entity.HasOne(d => d._owningteam_valueNavigation)
                    .WithMany(p => p.ptas_economicunit)
                    .HasForeignKey(d => d._owningteam_value)
                    .HasConstraintName("FK_team_ptas_economicunit_owningteam");

                entity.HasOne(d => d._owninguser_valueNavigation)
                    .WithMany(p => p.ptas_economicunit_owninguser_valueNavigation)
                    .HasForeignKey(d => d._owninguser_value)
                    .HasConstraintName("FK_systemuser_ptas_economicunit_owninguser");
            });

            modelBuilder.Entity<ptas_economicunit_accessorydetail>(entity =>
            {
                entity.ToTable("ptas_economicunit_accessorydetail", "dynamics");

                entity.HasIndex(e => e.modifiedon)
                    .HasName("Idx_ptas_economicunit_accessorydetail_modifiedon");

                entity.Property(e => e.ptas_economicunit_accessorydetailid).ValueGeneratedNever();

                entity.Property(e => e.modifiedon)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");
            });

            modelBuilder.Entity<ptas_environmentalrestriction>(entity =>
            {
                entity.ToTable("ptas_environmentalrestriction", "dynamics");

                entity.Property(e => e.ptas_environmentalrestrictionid).ValueGeneratedNever();

                entity.Property(e => e.exchangerate).HasColumnType("money");

                entity.Property(e => e.ptas_dollaradjustment).HasColumnType("money");

                entity.Property(e => e.ptas_dollaradjustment_base).HasColumnType("money");

                entity.Property(e => e.ptas_dollarpersqft).HasColumnType("money");

                entity.Property(e => e.ptas_dollarpersqft_base).HasColumnType("money");

                entity.Property(e => e.ptas_sqft).HasColumnType("money");

                entity.HasOne(d => d._createdby_valueNavigation)
                    .WithMany(p => p.ptas_environmentalrestriction_createdby_valueNavigation)
                    .HasForeignKey(d => d._createdby_value)
                    .HasConstraintName("FK_systemuser_ptas_environmentalrestriction_createdby");

                entity.HasOne(d => d._createdonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_environmentalrestriction_createdonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._createdonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_environmentalrestriction_createdonbehalfby");

                entity.HasOne(d => d._modifiedby_valueNavigation)
                    .WithMany(p => p.ptas_environmentalrestriction_modifiedby_valueNavigation)
                    .HasForeignKey(d => d._modifiedby_value)
                    .HasConstraintName("FK_systemuser_ptas_environmentalrestriction_modifiedby");

                entity.HasOne(d => d._modifiedonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_environmentalrestriction_modifiedonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._modifiedonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_environmentalrestriction_modifiedonbehalfby");

                entity.HasOne(d => d._owningteam_valueNavigation)
                    .WithMany(p => p.ptas_environmentalrestriction)
                    .HasForeignKey(d => d._owningteam_value)
                    .HasConstraintName("FK_team_ptas_environmentalrestriction_owningteam");

                entity.HasOne(d => d._owninguser_valueNavigation)
                    .WithMany(p => p.ptas_environmentalrestriction_owninguser_valueNavigation)
                    .HasForeignKey(d => d._owninguser_value)
                    .HasConstraintName("FK_systemuser_ptas_environmentalrestriction_owninguser");

                entity.HasOne(d => d._ptas_landid_valueNavigation)
                    .WithMany(p => p.ptas_environmentalrestriction)
                    .HasForeignKey(d => d._ptas_landid_value)
                    .HasConstraintName("FK_ptas_land_ptas_environmentalrestriction_ptas_landid");
            });

            modelBuilder.Entity<ptas_estimateHistory>(entity =>
            {
                entity.HasKey(e => e.estimateHistoryGuid)
                    .HasName("PK_estHistGuid")
                    .IsClustered(false);

                entity.ToTable("ptas_estimateHistory", "ptas");

                entity.HasIndex(e => new { e.parcelIdName, e.rollYearName })
                    .HasName("idx_ParcelNum_Year")
                    .IsClustered();

                entity.HasIndex(e => new { e.rollYearName, e.parcelGuid })
                    .HasName("idx_parcelGuid");

                entity.HasIndex(e => new { e.buildingIdName, e.landIdName, e.parcelGuid, e.parcelIdName, e.rollYearName, e.landId, e.buildingGuid })
                    .HasName("idx_landId_bldgGuid");

                entity.HasIndex(e => new { e.landId, e.buildingIdName, e.landIdName, e.parcelGuid, e.parcelIdName, e.rollYearName, e.buildingGuid })
                    .HasName("idx_buildingGuid");

                entity.Property(e => e.estimateHistoryGuid).ValueGeneratedNever();

                entity.Property(e => e.assessmentYearIdName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.buildingIdName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.calculationDate).HasColumnType("datetime");

                entity.Property(e => e.createdByName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.createdOn).HasColumnType("datetime");

                entity.Property(e => e.estimateSrc).HasMaxLength(25);

                entity.Property(e => e.estimateType)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.landIdName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.modifiedByName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.modifiedOn).HasColumnType("datetime");

                entity.Property(e => e.parcelIdName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ppTaxAccountIdName).HasMaxLength(100);

                entity.Property(e => e.recName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.rollYearName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.taxAccountIdName)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ptas_fileattachmentmetadata>(entity =>
            {
                entity.ToTable("ptas_fileattachmentmetadata", "dynamics");

                entity.HasIndex(e => e.modifiedon)
                    .HasName("Idx_ptas_fileattachmentmetadata_modifiedon");

                entity.Property(e => e.ptas_fileattachmentmetadataid).ValueGeneratedNever();

                entity.HasOne(d => d._createdby_valueNavigation)
                    .WithMany(p => p.ptas_fileattachmentmetadata_createdby_valueNavigation)
                    .HasForeignKey(d => d._createdby_value)
                    .HasConstraintName("FK_systemuser_ptas_fileattachmentmetadata_createdby");

                entity.HasOne(d => d._createdonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_fileattachmentmetadata_createdonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._createdonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_fileattachmentmetadata_createdonbehalfby");

                entity.HasOne(d => d._modifiedby_valueNavigation)
                    .WithMany(p => p.ptas_fileattachmentmetadata_modifiedby_valueNavigation)
                    .HasForeignKey(d => d._modifiedby_value)
                    .HasConstraintName("FK_systemuser_ptas_fileattachmentmetadata_modifiedby");

                entity.HasOne(d => d._modifiedonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_fileattachmentmetadata_modifiedonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._modifiedonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_fileattachmentmetadata_modifiedonbehalfby");

                entity.HasOne(d => d._owningteam_valueNavigation)
                    .WithMany(p => p.ptas_fileattachmentmetadata)
                    .HasForeignKey(d => d._owningteam_value)
                    .HasConstraintName("FK_team_ptas_fileattachmentmetadata_owningteam");

                entity.HasOne(d => d._owninguser_valueNavigation)
                    .WithMany(p => p.ptas_fileattachmentmetadata_owninguser_valueNavigation)
                    .HasForeignKey(d => d._owninguser_value)
                    .HasConstraintName("FK_systemuser_ptas_fileattachmentmetadata_owninguser");

                entity.HasOne(d => d._ptas_arcid_valueNavigation)
                    .WithMany(p => p.ptas_fileattachmentmetadata)
                    .HasForeignKey(d => d._ptas_arcid_value)
                    .HasConstraintName("FK_ptas_assessmentrollcorrection_ptas_fileattachmentmetadata_ptas_arcid");

                entity.HasOne(d => d._ptas_attachmentid_valueNavigation)
                    .WithMany(p => p.ptas_fileattachmentmetadata)
                    .HasForeignKey(d => d._ptas_attachmentid_value)
                    .HasConstraintName("FK_ptas_homeimprovement_ptas_fileattachmentmetadata_ptas_attachmentid");

                entity.HasOne(d => d._ptas_loadbyid_valueNavigation)
                    .WithMany(p => p.ptas_fileattachmentmetadata_ptas_loadbyid_valueNavigation)
                    .HasForeignKey(d => d._ptas_loadbyid_value)
                    .HasConstraintName("FK_systemuser_ptas_fileattachmentmetadata_ptas_loadbyid");

                entity.HasOne(d => d._ptas_parcelid_valueNavigation)
                    .WithMany(p => p.ptas_fileattachmentmetadata)
                    .HasForeignKey(d => d._ptas_parcelid_value)
                    .HasConstraintName("FK_ptas_parceldetail_ptas_fileattachmentmetadata_ptas_parcelid");

                entity.HasOne(d => d._ptas_rollyearid_valueNavigation)
                    .WithMany(p => p.ptas_fileattachmentmetadata_ptas_rollyearid_valueNavigation)
                    .HasForeignKey(d => d._ptas_rollyearid_value)
                    .HasConstraintName("FK_ptas_year_ptas_fileattachmentmetadata_ptas_rollyearid");

                entity.HasOne(d => d._ptas_taxaccountid_valueNavigation)
                    .WithMany(p => p.ptas_fileattachmentmetadata_ptas_taxaccountid_valueNavigation)
                    .HasForeignKey(d => d._ptas_taxaccountid_value)
                    .HasConstraintName("FK_ptas_taxaccount_ptas_fileattachmentmetadata_ptas_taxaccountid");

                entity.HasOne(d => d._ptas_taxpayerid_valueNavigation)
                    .WithMany(p => p.ptas_fileattachmentmetadata_ptas_taxpayerid_valueNavigation)
                    .HasForeignKey(d => d._ptas_taxpayerid_value)
                    .HasConstraintName("FK_ptas_taxaccount_ptas_fileattachmentmetadata_ptas_taxpayerid");

                entity.HasOne(d => d._ptas_taxyearid_valueNavigation)
                    .WithMany(p => p.ptas_fileattachmentmetadata_ptas_taxyearid_valueNavigation)
                    .HasForeignKey(d => d._ptas_taxyearid_value)
                    .HasConstraintName("FK_ptas_year_ptas_fileattachmentmetadata_ptas_taxyearid");

                entity.HasOne(d => d._ptas_yearid_valueNavigation)
                    .WithMany(p => p.ptas_fileattachmentmetadata_ptas_yearid_valueNavigation)
                    .HasForeignKey(d => d._ptas_yearid_value)
                    .HasConstraintName("FK_ptas_year_ptas_fileattachmentmetadata_ptas_yearid");
            });

            modelBuilder.Entity<ptas_floatingHomeReplacementCostRate>(entity =>
            {
                entity.HasKey(e => e.floatingHomeReplacementCostRateId)
                    .HasName("PK__ptas_flo__CBA2DB126A4C792B");

                entity.ToTable("ptas_floatingHomeReplacementCostRate", "ptas");

                entity.Property(e => e.floatingHomeReplacementCostRateId).ValueGeneratedNever();

                entity.Property(e => e.assessmentYearIdName).HasMaxLength(50);

                entity.Property(e => e.gradeAverage).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.gradeAverageMinus).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.gradeAveragePlus).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.gradeExcellent).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.gradeExcellentMinus).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.gradeExcellentPlus).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.gradeGood).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.gradeGoodMinus).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.gradeGoodPlus).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.specialityAreaIdName).HasMaxLength(50);

                entity.Property(e => e.specialityNeighborhoodIdName).HasMaxLength(50);
            });

            modelBuilder.Entity<ptas_floatingHomeSlipValues>(entity =>
            {
                entity.HasKey(e => e.floatingHomeSlipValuesId)
                    .HasName("PK__ptas_flo__54393D3F55E2D2C7");

                entity.ToTable("ptas_floatingHomeSlipValues", "ptas");

                entity.Property(e => e.floatingHomeSlipValuesId).ValueGeneratedNever();

                entity.Property(e => e.assessmentYearIdName).HasMaxLength(50);

                entity.Property(e => e.grade1).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.grade2).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.grade3).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.grade4).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.grade5).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.grade6).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.grade7).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.specialityAreaIdName).HasMaxLength(50);
            });

            modelBuilder.Entity<ptas_floatingHomeValuation>(entity =>
            {
                entity.HasKey(e => e.floatingHomeValuationId)
                    .HasName("PK__ptas_flo__800798B9955F3C39");

                entity.ToTable("ptas_floatingHomeValuation", "ptas");

                entity.Property(e => e.floatingHomeValuationId).ValueGeneratedNever();

                entity.Property(e => e.RCNLD).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.RCNLDperSqft).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.RCNperSqft).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.assessmentYearIdName).HasMaxLength(50);

                entity.Property(e => e.basementValue).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.citySlipValue).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.dnrSlipValue).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.floatingHomeProjectIdName).HasMaxLength(50);

                entity.Property(e => e.floatingHomeUnitIdName).HasMaxLength(50);

                entity.Property(e => e.livingValue).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.parcelIdName).HasMaxLength(50);

                entity.Property(e => e.pcntNetConditionValue).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.recordName).HasMaxLength(50);

                entity.Property(e => e.slipGradeValue).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.smallHomeAdjustmentValue).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.subjectParcelSlipValue).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.totalHomeValue).HasColumnType("decimal(18, 0)");
            });

            modelBuilder.Entity<ptas_geoarea>(entity =>
            {
                entity.ToTable("ptas_geoarea", "dynamics");

                entity.HasIndex(e => e.modifiedon)
                    .HasName("Idx_ptas_geoarea_modifiedon");

                entity.Property(e => e.ptas_geoareaid).ValueGeneratedNever();

                entity.HasOne(d => d._createdby_valueNavigation)
                    .WithMany(p => p.ptas_geoarea_createdby_valueNavigation)
                    .HasForeignKey(d => d._createdby_value)
                    .HasConstraintName("FK_systemuser_ptas_geoarea_createdby");

                entity.HasOne(d => d._createdonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_geoarea_createdonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._createdonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_geoarea_createdonbehalfby");

                entity.HasOne(d => d._modifiedby_valueNavigation)
                    .WithMany(p => p.ptas_geoarea_modifiedby_valueNavigation)
                    .HasForeignKey(d => d._modifiedby_value)
                    .HasConstraintName("FK_systemuser_ptas_geoarea_modifiedby");

                entity.HasOne(d => d._modifiedonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_geoarea_modifiedonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._modifiedonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_geoarea_modifiedonbehalfby");

                entity.HasOne(d => d._ptas_appraiserid_valueNavigation)
                    .WithMany(p => p.ptas_geoarea_ptas_appraiserid_valueNavigation)
                    .HasForeignKey(d => d._ptas_appraiserid_value)
                    .HasConstraintName("FK_systemuser_ptas_geoarea_ptas_appraiserid");

                entity.HasOne(d => d._ptas_seniorappraiserid_valueNavigation)
                    .WithMany(p => p.ptas_geoarea_ptas_seniorappraiserid_valueNavigation)
                    .HasForeignKey(d => d._ptas_seniorappraiserid_value)
                    .HasConstraintName("FK_systemuser_ptas_geoarea_ptas_seniorappraiserid");
            });

            modelBuilder.Entity<ptas_geoneighborhood>(entity =>
            {
                entity.ToTable("ptas_geoneighborhood", "dynamics");

                entity.HasIndex(e => e.modifiedon)
                    .HasName("Idx_ptas_geoneighborhood_modifiedon");

                entity.Property(e => e.ptas_geoneighborhoodid).ValueGeneratedNever();

                entity.HasOne(d => d._createdby_valueNavigation)
                    .WithMany(p => p.ptas_geoneighborhood_createdby_valueNavigation)
                    .HasForeignKey(d => d._createdby_value)
                    .HasConstraintName("FK_systemuser_ptas_geoneighborhood_createdby");

                entity.HasOne(d => d._createdonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_geoneighborhood_createdonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._createdonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_geoneighborhood_createdonbehalfby");

                entity.HasOne(d => d._modifiedby_valueNavigation)
                    .WithMany(p => p.ptas_geoneighborhood_modifiedby_valueNavigation)
                    .HasForeignKey(d => d._modifiedby_value)
                    .HasConstraintName("FK_systemuser_ptas_geoneighborhood_modifiedby");

                entity.HasOne(d => d._modifiedonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_geoneighborhood_modifiedonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._modifiedonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_geoneighborhood_modifiedonbehalfby");

                entity.HasOne(d => d._ptas_geoareaid_valueNavigation)
                    .WithMany(p => p.ptas_geoneighborhood)
                    .HasForeignKey(d => d._ptas_geoareaid_value)
                    .HasConstraintName("FK_ptas_geoarea_ptas_geoneighborhood_ptas_geoareaid");

                entity.HasOne(d => d._ptas_submarket_valueNavigation)
                    .WithMany(p => p.ptas_geoneighborhood)
                    .HasForeignKey(d => d._ptas_submarket_value)
                    .HasConstraintName("FK_ptas_submarket_ptas_geoneighborhood_ptas_submarket");

                entity.HasOne(d => d._ptas_supergroup_valueNavigation)
                    .WithMany(p => p.ptas_geoneighborhood)
                    .HasForeignKey(d => d._ptas_supergroup_value)
                    .HasConstraintName("FK_ptas_supergroup_ptas_geoneighborhood_ptas_supergroup");
            });

            modelBuilder.Entity<ptas_govtaxpayername>(entity =>
            {
                entity.ToTable("ptas_govtaxpayername", "dynamics");

                entity.Property(e => e.ptas_govtaxpayernameid).ValueGeneratedNever();

                entity.HasOne(d => d._createdby_valueNavigation)
                    .WithMany(p => p.ptas_govtaxpayername_createdby_valueNavigation)
                    .HasForeignKey(d => d._createdby_value)
                    .HasConstraintName("FK_systemuser_ptas_govtaxpayername_createdby");

                entity.HasOne(d => d._createdonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_govtaxpayername_createdonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._createdonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_govtaxpayername_createdonbehalfby");

                entity.HasOne(d => d._modifiedby_valueNavigation)
                    .WithMany(p => p.ptas_govtaxpayername_modifiedby_valueNavigation)
                    .HasForeignKey(d => d._modifiedby_value)
                    .HasConstraintName("FK_systemuser_ptas_govtaxpayername_modifiedby");

                entity.HasOne(d => d._modifiedonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_govtaxpayername_modifiedonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._modifiedonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_govtaxpayername_modifiedonbehalfby");
            });

            modelBuilder.Entity<ptas_gradestratificationmapping>(entity =>
            {
                entity.ToTable("ptas_gradestratificationmapping", "dynamics");

                entity.Property(e => e.ptas_gradestratificationmappingid).ValueGeneratedNever();

                entity.HasOne(d => d._createdby_valueNavigation)
                    .WithMany(p => p.ptas_gradestratificationmapping_createdby_valueNavigation)
                    .HasForeignKey(d => d._createdby_value)
                    .HasConstraintName("FK_systemuser_ptas_gradestratificationmapping_createdby");

                entity.HasOne(d => d._createdonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_gradestratificationmapping_createdonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._createdonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_gradestratificationmapping_createdonbehalfby");

                entity.HasOne(d => d._modifiedby_valueNavigation)
                    .WithMany(p => p.ptas_gradestratificationmapping_modifiedby_valueNavigation)
                    .HasForeignKey(d => d._modifiedby_value)
                    .HasConstraintName("FK_systemuser_ptas_gradestratificationmapping_modifiedby");

                entity.HasOne(d => d._modifiedonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_gradestratificationmapping_modifiedonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._modifiedonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_gradestratificationmapping_modifiedonbehalfby");

                entity.HasOne(d => d._ptas_specialtyneighborhoodid_valueNavigation)
                    .WithMany(p => p.ptas_gradestratificationmapping)
                    .HasForeignKey(d => d._ptas_specialtyneighborhoodid_value)
                    .HasConstraintName("FK_ptas_specialtyneighborhood_ptas_gradestratificationmapping_ptas_specialtyneighborhoodid");
            });

            modelBuilder.Entity<ptas_homeimprovement>(entity =>
            {
                entity.ToTable("ptas_homeimprovement", "dynamics");

                entity.Property(e => e.ptas_homeimprovementid).ValueGeneratedNever();

                entity.Property(e => e.exchangerate).HasColumnType("money");

                entity.Property(e => e.ptas_estimatedconstructioncost).HasColumnType("money");

                entity.Property(e => e.ptas_estimatedconstructioncost_base).HasColumnType("money");

                entity.Property(e => e.ptas_exemptionamount).HasColumnType("money");

                entity.Property(e => e.ptas_exemptionamount_base).HasColumnType("money");

                entity.HasOne(d => d._createdby_valueNavigation)
                    .WithMany(p => p.ptas_homeimprovement_createdby_valueNavigation)
                    .HasForeignKey(d => d._createdby_value)
                    .HasConstraintName("FK_systemuser_ptas_homeimprovement_createdby");

                entity.HasOne(d => d._createdonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_homeimprovement_createdonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._createdonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_homeimprovement_createdonbehalfby");

                entity.HasOne(d => d._modifiedby_valueNavigation)
                    .WithMany(p => p.ptas_homeimprovement_modifiedby_valueNavigation)
                    .HasForeignKey(d => d._modifiedby_value)
                    .HasConstraintName("FK_systemuser_ptas_homeimprovement_modifiedby");

                entity.HasOne(d => d._modifiedonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_homeimprovement_modifiedonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._modifiedonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_homeimprovement_modifiedonbehalfby");

                entity.HasOne(d => d._owningteam_valueNavigation)
                    .WithMany(p => p.ptas_homeimprovement)
                    .HasForeignKey(d => d._owningteam_value)
                    .HasConstraintName("FK_team_ptas_homeimprovement_owningteam");

                entity.HasOne(d => d._owninguser_valueNavigation)
                    .WithMany(p => p.ptas_homeimprovement_owninguser_valueNavigation)
                    .HasForeignKey(d => d._owninguser_value)
                    .HasConstraintName("FK_systemuser_ptas_homeimprovement_owninguser");

                entity.HasOne(d => d._ptas_buildingid_valueNavigation)
                    .WithMany(p => p.ptas_homeimprovement)
                    .HasForeignKey(d => d._ptas_buildingid_value)
                    .HasConstraintName("FK_ptas_buildingdetail_ptas_homeimprovement_ptas_buildingid");

                entity.HasOne(d => d._ptas_exemptionbeginyearid_valueNavigation)
                    .WithMany(p => p.ptas_homeimprovement_ptas_exemptionbeginyearid_valueNavigation)
                    .HasForeignKey(d => d._ptas_exemptionbeginyearid_value)
                    .HasConstraintName("FK_ptas_year_ptas_homeimprovement_ptas_exemptionbeginyearid");

                entity.HasOne(d => d._ptas_exemptionendyearid_valueNavigation)
                    .WithMany(p => p.ptas_homeimprovement_ptas_exemptionendyearid_valueNavigation)
                    .HasForeignKey(d => d._ptas_exemptionendyearid_value)
                    .HasConstraintName("FK_ptas_year_ptas_homeimprovement_ptas_exemptionendyearid");

                entity.HasOne(d => d._ptas_parcelid_valueNavigation)
                    .WithMany(p => p.ptas_homeimprovement)
                    .HasForeignKey(d => d._ptas_parcelid_value)
                    .HasConstraintName("FK_ptas_parceldetail_ptas_homeimprovement_ptas_parcelid");

                entity.HasOne(d => d._ptas_permitid_valueNavigation)
                    .WithMany(p => p.ptas_homeimprovement)
                    .HasForeignKey(d => d._ptas_permitid_value)
                    .HasConstraintName("FK_ptas_permit_ptas_homeimprovement_ptas_permitid");

                entity.HasOne(d => d._ptas_permitjurisdictionid_valueNavigation)
                    .WithMany(p => p.ptas_homeimprovement)
                    .HasForeignKey(d => d._ptas_permitjurisdictionid_value)
                    .HasConstraintName("FK_ptas_jurisdiction_ptas_homeimprovement_ptas_permitjurisdictionid");

                entity.HasOne(d => d._ptas_portalcontactid_valueNavigation)
                    .WithMany(p => p.ptas_homeimprovement)
                    .HasForeignKey(d => d._ptas_portalcontactid_value)
                    .HasConstraintName("FK_ptas_portalcontact_ptas_homeimprovement_ptas_portalcontactid");

                entity.HasOne(d => d._ptas_taxaccountid_valueNavigation)
                    .WithMany(p => p.ptas_homeimprovement)
                    .HasForeignKey(d => d._ptas_taxaccountid_value)
                    .HasConstraintName("FK_ptas_taxaccount_ptas_homeimprovement_ptas_taxaccountid");
            });

            modelBuilder.Entity<ptas_homeimprovementnotes>(entity =>
            {
                entity.ToTable("ptas_homeimprovementnotes", "dynamics");

                entity.Property(e => e.ptas_homeimprovementnotesid).ValueGeneratedNever();
            });

            modelBuilder.Entity<ptas_housingprogram>(entity =>
            {
                entity.ToTable("ptas_housingprogram", "dynamics");

                entity.HasIndex(e => e.modifiedon)
                    .HasName("Idx_ptas_housingprogram_modifiedon");

                entity.Property(e => e.ptas_housingprogramid).ValueGeneratedNever();

                entity.HasOne(d => d._createdby_valueNavigation)
                    .WithMany(p => p.ptas_housingprogram_createdby_valueNavigation)
                    .HasForeignKey(d => d._createdby_value)
                    .HasConstraintName("FK_systemuser_ptas_housingprogram_createdby");

                entity.HasOne(d => d._createdonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_housingprogram_createdonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._createdonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_housingprogram_createdonbehalfby");

                entity.HasOne(d => d._modifiedby_valueNavigation)
                    .WithMany(p => p.ptas_housingprogram_modifiedby_valueNavigation)
                    .HasForeignKey(d => d._modifiedby_value)
                    .HasConstraintName("FK_systemuser_ptas_housingprogram_modifiedby");

                entity.HasOne(d => d._modifiedonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_housingprogram_modifiedonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._modifiedonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_housingprogram_modifiedonbehalfby");
            });

            modelBuilder.Entity<ptas_incomemodel>(entity =>
            {
                entity.HasKey(e => e.incomemodelId);

                entity.ToTable("ptas_incomemodel", "ptas");

                entity.Property(e => e.incomemodelId).ValueGeneratedNever();

                entity.Property(e => e.name).HasMaxLength(100);
            });

            modelBuilder.Entity<ptas_incomemodeldetail>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("ptas_incomemodeldetail", "ptas");

                entity.Property(e => e.RowID).ValueGeneratedOnAdd();

                entity.Property(e => e.currentSectionUseCodes)
                    .HasMaxLength(100)
                    .IsFixedLength();

                entity.Property(e => e.grade1).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.grade2).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.grade3).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.grade4).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.grade5).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.grade6).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.grade7).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.rateType).HasMaxLength(50);
            });

            modelBuilder.Entity<ptas_incomevaluation>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("ptas_incomevaluation", "ptas");

                entity.Property(e => e.RowID).ValueGeneratedOnAdd();

                entity.Property(e => e.capRate).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.capRateParking).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.dailyRate).HasColumnType("money");

                entity.Property(e => e.dollarPerSqFt).HasColumnType("money");

                entity.Property(e => e.effectiveGrossIncome).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.effectiveGrossIncomeParking).HasColumnType("money");

                entity.Property(e => e.exceptionCode)
                    .HasMaxLength(100)
                    .IsFixedLength();

                entity.Property(e => e.indicatedValue).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.monthlyRate).HasColumnType("money");

                entity.Property(e => e.name).HasMaxLength(100);

                entity.Property(e => e.netOperatingIncome).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.netOperatingIncomeParking).HasColumnType("money");

                entity.Property(e => e.operatingExpensePercent).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.operatingExpensesParking).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.parkingOccupancy).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.potentialGrossIncome).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.rent).HasColumnType("money");

                entity.Property(e => e.vacancyAndLossCollection).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.weightedCapAmt).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.weightedCapAmtParking).HasColumnType("decimal(18, 2)");
            });

            modelBuilder.Entity<ptas_industry>(entity =>
            {
                entity.ToTable("ptas_industry", "dynamics");

                entity.Property(e => e.ptas_industryid).ValueGeneratedNever();

                entity.HasOne(d => d._createdby_valueNavigation)
                    .WithMany(p => p.ptas_industry_createdby_valueNavigation)
                    .HasForeignKey(d => d._createdby_value)
                    .HasConstraintName("FK_systemuser_ptas_industry_createdby");

                entity.HasOne(d => d._createdonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_industry_createdonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._createdonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_industry_createdonbehalfby");

                entity.HasOne(d => d._modifiedby_valueNavigation)
                    .WithMany(p => p.ptas_industry_modifiedby_valueNavigation)
                    .HasForeignKey(d => d._modifiedby_value)
                    .HasConstraintName("FK_systemuser_ptas_industry_modifiedby");

                entity.HasOne(d => d._modifiedonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_industry_modifiedonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._modifiedonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_industry_modifiedonbehalfby");

                entity.HasOne(d => d._owningteam_valueNavigation)
                    .WithMany(p => p.ptas_industry)
                    .HasForeignKey(d => d._owningteam_value)
                    .HasConstraintName("FK_team_ptas_industry_owningteam");

                entity.HasOne(d => d._owninguser_valueNavigation)
                    .WithMany(p => p.ptas_industry_owninguser_valueNavigation)
                    .HasForeignKey(d => d._owninguser_value)
                    .HasConstraintName("FK_systemuser_ptas_industry_owninguser");
            });

            modelBuilder.Entity<ptas_industry_ptas_personalpropertycategory>(entity =>
            {
                entity.ToTable("ptas_industry_ptas_personalpropertycategory", "dynamics");

                entity.Property(e => e.ptas_industry_ptas_personalpropertycategoryid).ValueGeneratedNever();
            });

            modelBuilder.Entity<ptas_inspectionhistory>(entity =>
            {
                entity.ToTable("ptas_inspectionhistory", "dynamics");

                entity.HasIndex(e => e.modifiedon)
                    .HasName("Idx_ptas_inspectionhistory_modifiedon");

                entity.Property(e => e.ptas_inspectionhistoryid).ValueGeneratedNever();

                entity.HasOne(d => d._createdby_valueNavigation)
                    .WithMany(p => p.ptas_inspectionhistory_createdby_valueNavigation)
                    .HasForeignKey(d => d._createdby_value)
                    .HasConstraintName("FK_systemuser_ptas_inspectionhistory_createdby");

                entity.HasOne(d => d._createdonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_inspectionhistory_createdonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._createdonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_inspectionhistory_createdonbehalfby");

                entity.HasOne(d => d._modifiedby_valueNavigation)
                    .WithMany(p => p.ptas_inspectionhistory_modifiedby_valueNavigation)
                    .HasForeignKey(d => d._modifiedby_value)
                    .HasConstraintName("FK_systemuser_ptas_inspectionhistory_modifiedby");

                entity.HasOne(d => d._modifiedonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_inspectionhistory_modifiedonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._modifiedonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_inspectionhistory_modifiedonbehalfby");

                entity.HasOne(d => d._owningteam_valueNavigation)
                    .WithMany(p => p.ptas_inspectionhistory)
                    .HasForeignKey(d => d._owningteam_value)
                    .HasConstraintName("FK_team_ptas_inspectionhistory_owningteam");

                entity.HasOne(d => d._owninguser_valueNavigation)
                    .WithMany(p => p.ptas_inspectionhistory_owninguser_valueNavigation)
                    .HasForeignKey(d => d._owninguser_value)
                    .HasConstraintName("FK_systemuser_ptas_inspectionhistory_owninguser");

                entity.HasOne(d => d._ptas_inspectedbyid_valueNavigation)
                    .WithMany(p => p.ptas_inspectionhistory_ptas_inspectedbyid_valueNavigation)
                    .HasForeignKey(d => d._ptas_inspectedbyid_value)
                    .HasConstraintName("FK_systemuser_ptas_inspectionhistory_ptas_inspectedbyid");

                entity.HasOne(d => d._ptas_parcelid_valueNavigation)
                    .WithMany(p => p.ptas_inspectionhistory)
                    .HasForeignKey(d => d._ptas_parcelid_value)
                    .HasConstraintName("FK_ptas_parceldetail_ptas_inspectionhistory_ptas_parcelid");

                entity.HasOne(d => d._ptas_unit_valueNavigation)
                    .WithMany(p => p.ptas_inspectionhistory)
                    .HasForeignKey(d => d._ptas_unit_value)
                    .HasConstraintName("FK_ptas_condounit_ptas_inspectionhistory_ptas_unit");
            });

            modelBuilder.Entity<ptas_inspectionyear>(entity =>
            {
                entity.ToTable("ptas_inspectionyear", "dynamics");

                entity.Property(e => e.ptas_inspectionyearid).ValueGeneratedNever();

                entity.HasOne(d => d._createdby_valueNavigation)
                    .WithMany(p => p.ptas_inspectionyear_createdby_valueNavigation)
                    .HasForeignKey(d => d._createdby_value)
                    .HasConstraintName("FK_systemuser_ptas_inspectionyear_createdby");

                entity.HasOne(d => d._createdonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_inspectionyear_createdonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._createdonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_inspectionyear_createdonbehalfby");

                entity.HasOne(d => d._modifiedby_valueNavigation)
                    .WithMany(p => p.ptas_inspectionyear_modifiedby_valueNavigation)
                    .HasForeignKey(d => d._modifiedby_value)
                    .HasConstraintName("FK_systemuser_ptas_inspectionyear_modifiedby");

                entity.HasOne(d => d._modifiedonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_inspectionyear_modifiedonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._modifiedonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_inspectionyear_modifiedonbehalfby");

                entity.HasOne(d => d._owningteam_valueNavigation)
                    .WithMany(p => p.ptas_inspectionyear)
                    .HasForeignKey(d => d._owningteam_value)
                    .HasConstraintName("FK_team_ptas_inspectionyear_owningteam");

                entity.HasOne(d => d._owninguser_valueNavigation)
                    .WithMany(p => p.ptas_inspectionyear_owninguser_valueNavigation)
                    .HasForeignKey(d => d._owninguser_value)
                    .HasConstraintName("FK_systemuser_ptas_inspectionyear_owninguser");

                entity.HasOne(d => d._ptas_area_valueNavigation)
                    .WithMany(p => p.ptas_inspectionyear)
                    .HasForeignKey(d => d._ptas_area_value)
                    .HasConstraintName("FK_ptas_area_ptas_inspectionyear_ptas_area");

                entity.HasOne(d => d._ptas_geoneighborhood_valueNavigation)
                    .WithMany(p => p.ptas_inspectionyear)
                    .HasForeignKey(d => d._ptas_geoneighborhood_value)
                    .HasConstraintName("FK_ptas_geoneighborhood_ptas_inspectionyear_ptas_geoneighborhood");

                entity.HasOne(d => d._ptas_specialtyneighborhood_valueNavigation)
                    .WithMany(p => p.ptas_inspectionyear)
                    .HasForeignKey(d => d._ptas_specialtyneighborhood_value)
                    .HasConstraintName("FK_ptas_specialtyneighborhood_ptas_inspectionyear_ptas_specialtyneighborhood");
            });

            modelBuilder.Entity<ptas_jurisdiction>(entity =>
            {
                entity.ToTable("ptas_jurisdiction", "dynamics");

                entity.HasIndex(e => e.modifiedon)
                    .HasName("Idx_ptas_jurisdiction_modifiedon");

                entity.Property(e => e.ptas_jurisdictionid).ValueGeneratedNever();

                entity.HasOne(d => d._createdby_valueNavigation)
                    .WithMany(p => p.ptas_jurisdiction_createdby_valueNavigation)
                    .HasForeignKey(d => d._createdby_value)
                    .HasConstraintName("FK_systemuser_ptas_jurisdiction_createdby");

                entity.HasOne(d => d._createdonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_jurisdiction_createdonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._createdonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_jurisdiction_createdonbehalfby");

                entity.HasOne(d => d._modifiedby_valueNavigation)
                    .WithMany(p => p.ptas_jurisdiction_modifiedby_valueNavigation)
                    .HasForeignKey(d => d._modifiedby_value)
                    .HasConstraintName("FK_systemuser_ptas_jurisdiction_modifiedby");

                entity.HasOne(d => d._modifiedonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_jurisdiction_modifiedonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._modifiedonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_jurisdiction_modifiedonbehalfby");

                entity.HasOne(d => d._ptas_permitwebsiteconfigid_valueNavigation)
                    .WithMany(p => p.ptas_jurisdiction)
                    .HasForeignKey(d => d._ptas_permitwebsiteconfigid_value)
                    .HasConstraintName("FK_ptas_permitwebsiteconfig_ptas_jurisdiction_ptas_permitwebsiteconfigid");
            });

            modelBuilder.Entity<ptas_land>(entity =>
            {
                entity.ToTable("ptas_land", "dynamics");

                entity.HasIndex(e => e.modifiedon)
                    .HasName("Idx_ptas_land_modifiedon");

                entity.Property(e => e.ptas_landid).ValueGeneratedNever();

                entity.Property(e => e.exchangerate).HasColumnType("money");

                entity.Property(e => e.ptas_baselandvalue).HasColumnType("money");

                entity.Property(e => e.ptas_baselandvalue_base).HasColumnType("money");

                entity.Property(e => e.ptas_commerciallandvalue).HasColumnType("money");

                entity.Property(e => e.ptas_commerciallandvalue_base).HasColumnType("money");

                entity.Property(e => e.ptas_dollarspersquarefoot).HasColumnType("money");

                entity.Property(e => e.ptas_dollarspersquarefoot_base).HasColumnType("money");

                entity.Property(e => e.ptas_economicunitlandvalue).HasColumnType("money");

                entity.Property(e => e.ptas_economicunitlandvalue_base).HasColumnType("money");

                entity.Property(e => e.ptas_grosslandvalue).HasColumnType("money");

                entity.Property(e => e.ptas_grosslandvalue_base).HasColumnType("money");

                entity.Property(e => e.ptas_landacres).HasColumnType("money");

                entity.HasOne(d => d._createdby_valueNavigation)
                    .WithMany(p => p.ptas_land_createdby_valueNavigation)
                    .HasForeignKey(d => d._createdby_value)
                    .HasConstraintName("FK_systemuser_ptas_land_createdby");

                entity.HasOne(d => d._createdonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_land_createdonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._createdonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_land_createdonbehalfby");

                entity.HasOne(d => d._modifiedby_valueNavigation)
                    .WithMany(p => p.ptas_land_modifiedby_valueNavigation)
                    .HasForeignKey(d => d._modifiedby_value)
                    .HasConstraintName("FK_systemuser_ptas_land_modifiedby");

                entity.HasOne(d => d._modifiedonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_land_modifiedonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._modifiedonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_land_modifiedonbehalfby");

                entity.HasOne(d => d._owningteam_valueNavigation)
                    .WithMany(p => p.ptas_land)
                    .HasForeignKey(d => d._owningteam_value)
                    .HasConstraintName("FK_team_ptas_land_owningteam");

                entity.HasOne(d => d._owninguser_valueNavigation)
                    .WithMany(p => p.ptas_land_owninguser_valueNavigation)
                    .HasForeignKey(d => d._owninguser_value)
                    .HasConstraintName("FK_systemuser_ptas_land_owninguser");

                entity.HasOne(d => d._ptas_masterlandid_valueNavigation)
                    .WithMany(p => p.Inverse_ptas_masterlandid_valueNavigation)
                    .HasForeignKey(d => d._ptas_masterlandid_value)
                    .HasConstraintName("FK_ptas_land_ptas_land_ptas_masterlandid");
            });

            modelBuilder.Entity<ptas_land_ptas_mediarepository>(entity =>
            {
                entity.ToTable("ptas_land_ptas_mediarepository", "dynamics");

                entity.HasIndex(e => e.modifiedon)
                    .HasName("Idx_ptas_land_ptas_mediarepository_modifiedon");

                entity.Property(e => e.ptas_land_ptas_mediarepositoryid).ValueGeneratedNever();

                entity.Property(e => e.modifiedon)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");
            });

            modelBuilder.Entity<ptas_land_snapshot>(entity =>
            {
                entity.HasKey(e => e.ptas_landid);

                entity.ToTable("ptas_land_snapshot", "dynamics");

                entity.Property(e => e.ptas_landid).ValueGeneratedNever();

                entity.Property(e => e.exchangerate).HasColumnType("money");

                entity.Property(e => e.ptas_baselandvalue).HasColumnType("money");

                entity.Property(e => e.ptas_baselandvalue_base).HasColumnType("money");

                entity.Property(e => e.ptas_commerciallandvalue).HasColumnType("money");

                entity.Property(e => e.ptas_commerciallandvalue_base).HasColumnType("money");

                entity.Property(e => e.ptas_dollarspersquarefoot).HasColumnType("money");

                entity.Property(e => e.ptas_dollarspersquarefoot_base).HasColumnType("money");

                entity.Property(e => e.ptas_economicunitlandvalue).HasColumnType("money");

                entity.Property(e => e.ptas_economicunitlandvalue_base).HasColumnType("money");

                entity.Property(e => e.ptas_grosslandvalue).HasColumnType("money");

                entity.Property(e => e.ptas_grosslandvalue_base).HasColumnType("money");

                entity.Property(e => e.ptas_landacres).HasColumnType("money");
            });

            modelBuilder.Entity<ptas_landuse>(entity =>
            {
                entity.ToTable("ptas_landuse", "dynamics");

                entity.HasIndex(e => e.modifiedon)
                    .HasName("Idx_ptas_landuse_modifiedon");

                entity.Property(e => e.ptas_landuseid).ValueGeneratedNever();

                entity.HasOne(d => d._createdby_valueNavigation)
                    .WithMany(p => p.ptas_landuse_createdby_valueNavigation)
                    .HasForeignKey(d => d._createdby_value)
                    .HasConstraintName("FK_systemuser_ptas_landuse_createdby");

                entity.HasOne(d => d._createdonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_landuse_createdonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._createdonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_landuse_createdonbehalfby");

                entity.HasOne(d => d._modifiedby_valueNavigation)
                    .WithMany(p => p.ptas_landuse_modifiedby_valueNavigation)
                    .HasForeignKey(d => d._modifiedby_value)
                    .HasConstraintName("FK_systemuser_ptas_landuse_modifiedby");

                entity.HasOne(d => d._modifiedonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_landuse_modifiedonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._modifiedonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_landuse_modifiedonbehalfby");
            });

            modelBuilder.Entity<ptas_landvaluebreakdown>(entity =>
            {
                entity.ToTable("ptas_landvaluebreakdown", "dynamics");

                entity.HasIndex(e => e.modifiedon)
                    .HasName("Idx_ptas_landvaluebreakdown_modifiedon");

                entity.Property(e => e.ptas_landvaluebreakdownid).ValueGeneratedNever();

                entity.Property(e => e.ptas_percent).HasColumnType("money");

                entity.HasOne(d => d._createdby_valueNavigation)
                    .WithMany(p => p.ptas_landvaluebreakdown_createdby_valueNavigation)
                    .HasForeignKey(d => d._createdby_value)
                    .HasConstraintName("FK_systemuser_ptas_landvaluebreakdown_createdby");

                entity.HasOne(d => d._createdonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_landvaluebreakdown_createdonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._createdonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_landvaluebreakdown_createdonbehalfby");

                entity.HasOne(d => d._modifiedby_valueNavigation)
                    .WithMany(p => p.ptas_landvaluebreakdown_modifiedby_valueNavigation)
                    .HasForeignKey(d => d._modifiedby_value)
                    .HasConstraintName("FK_systemuser_ptas_landvaluebreakdown_modifiedby");

                entity.HasOne(d => d._modifiedonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_landvaluebreakdown_modifiedonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._modifiedonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_landvaluebreakdown_modifiedonbehalfby");

                entity.HasOne(d => d._owningteam_valueNavigation)
                    .WithMany(p => p.ptas_landvaluebreakdown)
                    .HasForeignKey(d => d._owningteam_value)
                    .HasConstraintName("FK_team_ptas_landvaluebreakdown_owningteam");

                entity.HasOne(d => d._owninguser_valueNavigation)
                    .WithMany(p => p.ptas_landvaluebreakdown_owninguser_valueNavigation)
                    .HasForeignKey(d => d._owninguser_value)
                    .HasConstraintName("FK_systemuser_ptas_landvaluebreakdown_owninguser");

                entity.HasOne(d => d._ptas_landid_valueNavigation)
                    .WithMany(p => p.ptas_landvaluebreakdown)
                    .HasForeignKey(d => d._ptas_landid_value)
                    .HasConstraintName("FK_ptas_land_ptas_landvaluebreakdown_ptas_landid");

                entity.HasOne(d => d._ptas_masterlandvaluebreakdownid_valueNavigation)
                    .WithMany(p => p.Inverse_ptas_masterlandvaluebreakdownid_valueNavigation)
                    .HasForeignKey(d => d._ptas_masterlandvaluebreakdownid_value)
                    .HasConstraintName("FK_ptas_landvaluebreakdown_ptas_landvaluebreakdown_ptas_masterlandvaluebreakdownid");

                entity.HasOne(d => d._ptas_parceldetailid_valueNavigation)
                    .WithMany(p => p.ptas_landvaluebreakdown)
                    .HasForeignKey(d => d._ptas_parceldetailid_value)
                    .HasConstraintName("FK_ptas_parceldetail_ptas_landvaluebreakdown_ptas_parceldetailid");
            });

            modelBuilder.Entity<ptas_landvaluebreakdown_snapshot>(entity =>
            {
                entity.HasKey(e => e.ptas_landvaluebreakdownid);

                entity.ToTable("ptas_landvaluebreakdown_snapshot", "dynamics");

                entity.Property(e => e.ptas_landvaluebreakdownid).ValueGeneratedNever();

                entity.Property(e => e.ptas_percent).HasColumnType("money");
            });

            modelBuilder.Entity<ptas_landvaluecalculation>(entity =>
            {
                entity.ToTable("ptas_landvaluecalculation", "dynamics");

                entity.HasIndex(e => e.modifiedon)
                    .HasName("Idx_ptas_landvaluecalculation_modifiedon");

                entity.Property(e => e.ptas_landvaluecalculationid).ValueGeneratedNever();

                entity.Property(e => e.exchangerate).HasColumnType("money");

                entity.Property(e => e.ptas_adjustedvalue).HasColumnType("money");

                entity.Property(e => e.ptas_adjustedvalue_base).HasColumnType("money");

                entity.Property(e => e.ptas_dollaradjustment).HasColumnType("money");

                entity.Property(e => e.ptas_dollaradjustment_base).HasColumnType("money");

                entity.Property(e => e.ptas_dollarperlinearft).HasColumnType("money");

                entity.Property(e => e.ptas_dollarperlinearft_base).HasColumnType("money");

                entity.Property(e => e.ptas_dollarpersqft).HasColumnType("money");

                entity.Property(e => e.ptas_dollarpersqft_base).HasColumnType("money");

                entity.Property(e => e.ptas_dollarperunit).HasColumnType("money");

                entity.Property(e => e.ptas_dollarperunit_base).HasColumnType("money");

                entity.Property(e => e.ptas_grosslandvalue).HasColumnType("money");

                entity.Property(e => e.ptas_grosslandvalue_base).HasColumnType("money");

                entity.Property(e => e.ptas_sitevalue).HasColumnType("money");

                entity.Property(e => e.ptas_sitevalue_base).HasColumnType("money");

                entity.HasOne(d => d._createdby_valueNavigation)
                    .WithMany(p => p.ptas_landvaluecalculation_createdby_valueNavigation)
                    .HasForeignKey(d => d._createdby_value)
                    .HasConstraintName("FK_systemuser_ptas_landvaluecalculation_createdby");

                entity.HasOne(d => d._createdonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_landvaluecalculation_createdonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._createdonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_landvaluecalculation_createdonbehalfby");

                entity.HasOne(d => d._modifiedby_valueNavigation)
                    .WithMany(p => p.ptas_landvaluecalculation_modifiedby_valueNavigation)
                    .HasForeignKey(d => d._modifiedby_value)
                    .HasConstraintName("FK_systemuser_ptas_landvaluecalculation_modifiedby");

                entity.HasOne(d => d._modifiedonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_landvaluecalculation_modifiedonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._modifiedonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_landvaluecalculation_modifiedonbehalfby");

                entity.HasOne(d => d._owningteam_valueNavigation)
                    .WithMany(p => p.ptas_landvaluecalculation)
                    .HasForeignKey(d => d._owningteam_value)
                    .HasConstraintName("FK_team_ptas_landvaluecalculation_owningteam");

                entity.HasOne(d => d._owninguser_valueNavigation)
                    .WithMany(p => p.ptas_landvaluecalculation_owninguser_valueNavigation)
                    .HasForeignKey(d => d._owninguser_value)
                    .HasConstraintName("FK_systemuser_ptas_landvaluecalculation_owninguser");

                entity.HasOne(d => d._ptas_landid_valueNavigation)
                    .WithMany(p => p.ptas_landvaluecalculation)
                    .HasForeignKey(d => d._ptas_landid_value)
                    .HasConstraintName("FK_ptas_land_ptas_landvaluecalculation_ptas_landid");

                entity.HasOne(d => d._ptas_masterlandcharacteristicid_valueNavigation)
                    .WithMany(p => p.Inverse_ptas_masterlandcharacteristicid_valueNavigation)
                    .HasForeignKey(d => d._ptas_masterlandcharacteristicid_value)
                    .HasConstraintName("FK_ptas_landvaluecalculation_ptas_landvaluecalculation_ptas_masterlandcharacteristicid");

                entity.HasOne(d => d._ptas_zoningtypeid_valueNavigation)
                    .WithMany(p => p.ptas_landvaluecalculation)
                    .HasForeignKey(d => d._ptas_zoningtypeid_value)
                    .HasConstraintName("FK_ptas_zoning_ptas_landvaluecalculation_ptas_zoningtypeid");
            });

            modelBuilder.Entity<ptas_landvaluecalculation_snapshot>(entity =>
            {
                entity.HasKey(e => e.ptas_landvaluecalculationid);

                entity.ToTable("ptas_landvaluecalculation_snapshot", "dynamics");

                entity.Property(e => e.ptas_landvaluecalculationid).ValueGeneratedNever();

                entity.Property(e => e.exchangerate).HasColumnType("money");

                entity.Property(e => e.ptas_adjustedvalue).HasColumnType("money");

                entity.Property(e => e.ptas_adjustedvalue_base).HasColumnType("money");

                entity.Property(e => e.ptas_dollaradjustment).HasColumnType("money");

                entity.Property(e => e.ptas_dollaradjustment_base).HasColumnType("money");

                entity.Property(e => e.ptas_dollarperlinearft).HasColumnType("money");

                entity.Property(e => e.ptas_dollarperlinearft_base).HasColumnType("money");

                entity.Property(e => e.ptas_dollarpersqft).HasColumnType("money");

                entity.Property(e => e.ptas_dollarpersqft_base).HasColumnType("money");

                entity.Property(e => e.ptas_dollarperunit).HasColumnType("money");

                entity.Property(e => e.ptas_dollarperunit_base).HasColumnType("money");

                entity.Property(e => e.ptas_grosslandvalue).HasColumnType("money");

                entity.Property(e => e.ptas_grosslandvalue_base).HasColumnType("money");

                entity.Property(e => e.ptas_sitevalue).HasColumnType("money");

                entity.Property(e => e.ptas_sitevalue_base).HasColumnType("money");
            });

            modelBuilder.Entity<ptas_levycode>(entity =>
            {
                entity.ToTable("ptas_levycode", "dynamics");

                entity.HasIndex(e => e.modifiedon)
                    .HasName("Idx_ptas_levycode_modifiedon");

                entity.Property(e => e.ptas_levycodeid).ValueGeneratedNever();

                entity.Property(e => e.exchangerate).HasColumnType("money");

                entity.Property(e => e.ptas_9998590checktotal).HasColumnType("money");

                entity.Property(e => e.ptas_current1constitutionallimit).HasColumnType("money");

                entity.Property(e => e.ptas_locallevytotallimit).HasColumnType("money");

                entity.Property(e => e.ptas_overunderconstitutional).HasColumnType("money");

                entity.Property(e => e.ptas_totallevyrateconstitutional).HasColumnType("money");

                entity.HasOne(d => d._createdby_valueNavigation)
                    .WithMany(p => p.ptas_levycode_createdby_valueNavigation)
                    .HasForeignKey(d => d._createdby_value)
                    .HasConstraintName("FK_systemuser_ptas_levycode_createdby");

                entity.HasOne(d => d._createdonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_levycode_createdonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._createdonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_levycode_createdonbehalfby");

                entity.HasOne(d => d._modifiedby_valueNavigation)
                    .WithMany(p => p.ptas_levycode_modifiedby_valueNavigation)
                    .HasForeignKey(d => d._modifiedby_value)
                    .HasConstraintName("FK_systemuser_ptas_levycode_modifiedby");

                entity.HasOne(d => d._modifiedonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_levycode_modifiedonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._modifiedonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_levycode_modifiedonbehalfby");
            });

            modelBuilder.Entity<ptas_lowincomehousingprogram>(entity =>
            {
                entity.ToTable("ptas_lowincomehousingprogram", "dynamics");

                entity.HasIndex(e => e.modifiedon)
                    .HasName("Idx_ptas_lowincomehousingprogram_modifiedon");

                entity.Property(e => e.ptas_lowincomehousingprogramid).ValueGeneratedNever();

                entity.HasOne(d => d._createdby_valueNavigation)
                    .WithMany(p => p.ptas_lowincomehousingprogram_createdby_valueNavigation)
                    .HasForeignKey(d => d._createdby_value)
                    .HasConstraintName("FK_systemuser_ptas_lowincomehousingprogram_createdby");

                entity.HasOne(d => d._createdonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_lowincomehousingprogram_createdonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._createdonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_lowincomehousingprogram_createdonbehalfby");

                entity.HasOne(d => d._modifiedby_valueNavigation)
                    .WithMany(p => p.ptas_lowincomehousingprogram_modifiedby_valueNavigation)
                    .HasForeignKey(d => d._modifiedby_value)
                    .HasConstraintName("FK_systemuser_ptas_lowincomehousingprogram_modifiedby");

                entity.HasOne(d => d._modifiedonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_lowincomehousingprogram_modifiedonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._modifiedonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_lowincomehousingprogram_modifiedonbehalfby");

                entity.HasOne(d => d._owningteam_valueNavigation)
                    .WithMany(p => p.ptas_lowincomehousingprogram)
                    .HasForeignKey(d => d._owningteam_value)
                    .HasConstraintName("FK_team_ptas_lowincomehousingprogram_owningteam");

                entity.HasOne(d => d._owninguser_valueNavigation)
                    .WithMany(p => p.ptas_lowincomehousingprogram_owninguser_valueNavigation)
                    .HasForeignKey(d => d._owninguser_value)
                    .HasConstraintName("FK_systemuser_ptas_lowincomehousingprogram_owninguser");

                entity.HasOne(d => d._ptas_condocomplexid_valueNavigation)
                    .WithMany(p => p.ptas_lowincomehousingprogram)
                    .HasForeignKey(d => d._ptas_condocomplexid_value)
                    .HasConstraintName("FK_ptas_condocomplex_ptas_lowincomehousingprogram_ptas_condocomplexid");

                entity.HasOne(d => d._ptas_housingprogramid_valueNavigation)
                    .WithMany(p => p.ptas_lowincomehousingprogram)
                    .HasForeignKey(d => d._ptas_housingprogramid_value)
                    .HasConstraintName("FK_ptas_housingprogram_ptas_lowincomehousingprogram_ptas_housingprogramid");

                entity.HasOne(d => d._ptas_masterlowincomehousingid_valueNavigation)
                    .WithMany(p => p.Inverse_ptas_masterlowincomehousingid_valueNavigation)
                    .HasForeignKey(d => d._ptas_masterlowincomehousingid_value)
                    .HasConstraintName("FK_ptas_lowincomehousingprogram_ptas_lowincomehousingprogram_ptas_masterlowincomehousingid");
            });

            modelBuilder.Entity<ptas_lowincomehousingprogram_snapshot>(entity =>
            {
                entity.HasKey(e => e.ptas_lowincomehousingprogramid);

                entity.ToTable("ptas_lowincomehousingprogram_snapshot", "dynamics");

                entity.Property(e => e.ptas_lowincomehousingprogramid).ValueGeneratedNever();
            });

            modelBuilder.Entity<ptas_lowincomehousingunits>(entity =>
            {
                entity.ToTable("ptas_lowincomehousingunits", "dynamics");

                entity.Property(e => e.ptas_lowincomehousingunitsid).ValueGeneratedNever();

                entity.Property(e => e.exchangerate).HasColumnType("money");

                entity.Property(e => e.ptas_iac_capitalization_rate).HasColumnType("money");

                entity.Property(e => e.ptas_iac_effectivegrossincome).HasColumnType("money");

                entity.Property(e => e.ptas_iac_effectivegrossincome_base).HasColumnType("money");

                entity.Property(e => e.ptas_iac_expenseamount).HasColumnType("money");

                entity.Property(e => e.ptas_iac_expenseamount_base).HasColumnType("money");

                entity.Property(e => e.ptas_iac_expensespct).HasColumnType("money");

                entity.Property(e => e.ptas_iac_expensespct_base).HasColumnType("money");

                entity.Property(e => e.ptas_iac_indicatedvalue).HasColumnType("money");

                entity.Property(e => e.ptas_iac_indicatedvalue_base).HasColumnType("money");

                entity.Property(e => e.ptas_iac_netoperatingincome).HasColumnType("money");

                entity.Property(e => e.ptas_iac_netoperatingincome_base).HasColumnType("money");

                entity.Property(e => e.ptas_iac_otherincome).HasColumnType("money");

                entity.Property(e => e.ptas_iac_otherincome_base).HasColumnType("money");

                entity.Property(e => e.ptas_iac_potentialgrossincome).HasColumnType("money");

                entity.Property(e => e.ptas_iac_potentialgrossincome_base).HasColumnType("money");

                entity.Property(e => e.ptas_iac_totalgrossincome).HasColumnType("money");

                entity.Property(e => e.ptas_iac_totalgrossincome_base).HasColumnType("money");

                entity.Property(e => e.ptas_iac_vacancyandcreditlossamount).HasColumnType("money");

                entity.Property(e => e.ptas_iac_vacancyandcreditlossamount_base).HasColumnType("money");

                entity.Property(e => e.ptas_iac_vacancyandcreditlosspercent).HasColumnType("money");

                entity.Property(e => e.ptas_indicatedrent_1bedroom).HasColumnType("money");

                entity.Property(e => e.ptas_indicatedrent_1bedroom_base).HasColumnType("money");

                entity.Property(e => e.ptas_indicatedrent_2bedroom).HasColumnType("money");

                entity.Property(e => e.ptas_indicatedrent_2bedroom2bath).HasColumnType("money");

                entity.Property(e => e.ptas_indicatedrent_2bedroom2bath_base).HasColumnType("money");

                entity.Property(e => e.ptas_indicatedrent_2bedroom_base).HasColumnType("money");

                entity.Property(e => e.ptas_indicatedrent_3bedroom).HasColumnType("money");

                entity.Property(e => e.ptas_indicatedrent_3bedroom2bath).HasColumnType("money");

                entity.Property(e => e.ptas_indicatedrent_3bedroom2bath_base).HasColumnType("money");

                entity.Property(e => e.ptas_indicatedrent_3bedroom3bath).HasColumnType("money");

                entity.Property(e => e.ptas_indicatedrent_3bedroom3bath_base).HasColumnType("money");

                entity.Property(e => e.ptas_indicatedrent_3bedroom_base).HasColumnType("money");

                entity.Property(e => e.ptas_indicatedrent_4bedroom).HasColumnType("money");

                entity.Property(e => e.ptas_indicatedrent_4bedroom_base).HasColumnType("money");

                entity.Property(e => e.ptas_indicatedrent_5plusbedroom).HasColumnType("money");

                entity.Property(e => e.ptas_indicatedrent_5plusbedroom_base).HasColumnType("money");

                entity.Property(e => e.ptas_indicatedrent_sleepingroom).HasColumnType("money");

                entity.Property(e => e.ptas_indicatedrent_sleepingroom_base).HasColumnType("money");

                entity.Property(e => e.ptas_indicatedrent_studio).HasColumnType("money");

                entity.Property(e => e.ptas_indicatedrent_studio_base).HasColumnType("money");

                entity.Property(e => e.ptas_percentofunitsat100).HasColumnType("money");

                entity.Property(e => e.ptas_percentofunitsat120).HasColumnType("money");

                entity.Property(e => e.ptas_percentofunitsat20).HasColumnType("money");

                entity.Property(e => e.ptas_percentofunitsat30).HasColumnType("money");

                entity.Property(e => e.ptas_percentofunitsat35).HasColumnType("money");

                entity.Property(e => e.ptas_percentofunitsat40).HasColumnType("money");

                entity.Property(e => e.ptas_percentofunitsat45).HasColumnType("money");

                entity.Property(e => e.ptas_percentofunitsat50).HasColumnType("money");

                entity.Property(e => e.ptas_percentofunitsat60).HasColumnType("money");

                entity.Property(e => e.ptas_percentofunitsat70).HasColumnType("money");

                entity.Property(e => e.ptas_percentofunitsat80).HasColumnType("money");

                entity.Property(e => e.ptas_percentofunitsatmarket).HasColumnType("money");

                entity.Property(e => e.ptas_percentofunitsatrestrictedrent).HasColumnType("money");

                entity.Property(e => e.ptas_restrictedrent_1bed).HasColumnType("money");

                entity.Property(e => e.ptas_restrictedrent_1bed_base).HasColumnType("money");

                entity.Property(e => e.ptas_restrictedrent_2bed).HasColumnType("money");

                entity.Property(e => e.ptas_restrictedrent_2bed_base).HasColumnType("money");

                entity.Property(e => e.ptas_restrictedrent_3bed).HasColumnType("money");

                entity.Property(e => e.ptas_restrictedrent_3bed_base).HasColumnType("money");

                entity.Property(e => e.ptas_restrictedrent_4bed).HasColumnType("money");

                entity.Property(e => e.ptas_restrictedrent_4bed_base).HasColumnType("money");

                entity.Property(e => e.ptas_restrictedrent_5bedplus).HasColumnType("money");

                entity.Property(e => e.ptas_restrictedrent_5bedplus_base).HasColumnType("money");

                entity.Property(e => e.ptas_restrictedrent_sleepingroom).HasColumnType("money");

                entity.Property(e => e.ptas_restrictedrent_sleepingroom_base).HasColumnType("money");

                entity.Property(e => e.ptas_restrictedrent_studio).HasColumnType("money");

                entity.Property(e => e.ptas_restrictedrent_studio_base).HasColumnType("money");

                entity.Property(e => e.ptas_rra_1bedroom1bathrent).HasColumnType("money");

                entity.Property(e => e.ptas_rra_1bedroom1bathrent_base).HasColumnType("money");

                entity.Property(e => e.ptas_rra_2bedroom1bathrent).HasColumnType("money");

                entity.Property(e => e.ptas_rra_2bedroom1bathrent_base).HasColumnType("money");

                entity.Property(e => e.ptas_rra_2bedroom2bathrent).HasColumnType("money");

                entity.Property(e => e.ptas_rra_2bedroom2bathrent_base).HasColumnType("money");

                entity.Property(e => e.ptas_rra_3bedroom1bathrent).HasColumnType("money");

                entity.Property(e => e.ptas_rra_3bedroom1bathrent_base).HasColumnType("money");

                entity.Property(e => e.ptas_rra_3bedroom2bathrent).HasColumnType("money");

                entity.Property(e => e.ptas_rra_3bedroom2bathrent_base).HasColumnType("money");

                entity.Property(e => e.ptas_rra_3bedroom3bathrent).HasColumnType("money");

                entity.Property(e => e.ptas_rra_3bedroom3bathrent_base).HasColumnType("money");

                entity.Property(e => e.ptas_rra_4bedroomrent).HasColumnType("money");

                entity.Property(e => e.ptas_rra_4bedroomrent_base).HasColumnType("money");

                entity.Property(e => e.ptas_rra_5bedroomplusrent).HasColumnType("money");

                entity.Property(e => e.ptas_rra_5bedroomplusrent_base).HasColumnType("money");

                entity.Property(e => e.ptas_rra_commercialrent).HasColumnType("money");

                entity.Property(e => e.ptas_rra_commercialrent_base).HasColumnType("money");

                entity.Property(e => e.ptas_rra_open1bedroomrent).HasColumnType("money");

                entity.Property(e => e.ptas_rra_open1bedroomrent_base).HasColumnType("money");

                entity.Property(e => e.ptas_rra_open2bedroomrent).HasColumnType("money");

                entity.Property(e => e.ptas_rra_open2bedroomrent_base).HasColumnType("money");

                entity.Property(e => e.ptas_rra_otherincomeperunit).HasColumnType("money");

                entity.Property(e => e.ptas_rra_otherincomeperunit_base).HasColumnType("money");

                entity.Property(e => e.ptas_rra_sleepingroomrent).HasColumnType("money");

                entity.Property(e => e.ptas_rra_sleepingroomrent_base).HasColumnType("money");

                entity.Property(e => e.ptas_rra_studiorent).HasColumnType("money");

                entity.Property(e => e.ptas_rra_studiorent_base).HasColumnType("money");

                entity.Property(e => e.ptas_ruc_discountrate).HasColumnType("money");

                entity.Property(e => e.ptas_ruc_discountrate_base).HasColumnType("money");

                entity.Property(e => e.ptas_ruc_leaseholdreversionvalue).HasColumnType("money");

                entity.Property(e => e.ptas_ruc_leaseholdreversionvalue_base).HasColumnType("money");

                entity.Property(e => e.ptas_ruc_presentvalueofleasholdreversion).HasColumnType("money");

                entity.Property(e => e.ptas_ruc_presentvalueofleasholdreversion_base).HasColumnType("money");

                entity.Property(e => e.ptas_ruc_restrictedleasedfeevalue).HasColumnType("money");

                entity.Property(e => e.ptas_ruc_restrictedleasedfeevalue_base).HasColumnType("money");

                entity.Property(e => e.ptas_ruc_totalrestrictedusevalue).HasColumnType("money");

                entity.Property(e => e.ptas_ruc_totalrestrictedusevalue_base).HasColumnType("money");

                entity.Property(e => e.ptas_ruc_unrestrictedmarketvalue).HasColumnType("money");

                entity.Property(e => e.ptas_ruc_unrestrictedmarketvalue_base).HasColumnType("money");
            });

            modelBuilder.Entity<ptas_lowincomeparameters>(entity =>
            {
                entity.ToTable("ptas_lowincomeparameters", "dynamics");

                entity.Property(e => e.ptas_lowincomeparametersid).ValueGeneratedNever();

                entity.Property(e => e.ptas_caprateloadrate).HasColumnType("money");

                entity.Property(e => e.ptas_discountrate).HasColumnType("money");

                entity.Property(e => e.ptas_expensemultiplier).HasColumnType("money");

                entity.Property(e => e.ptas_rentmultiplier).HasColumnType("money");

                entity.Property(e => e.ptas_vclmultiplier).HasColumnType("money");
            });

            modelBuilder.Entity<ptas_masspayaccumulator>(entity =>
            {
                entity.ToTable("ptas_masspayaccumulator", "dynamics");

                entity.Property(e => e.ptas_masspayaccumulatorid).ValueGeneratedNever();

                entity.Property(e => e.ptas_loanoriginationdate).HasColumnType("datetime");

                entity.Property(e => e.ptas_rundate).HasColumnType("datetime");

                entity.HasOne(d => d._createdby_valueNavigation)
                    .WithMany(p => p.ptas_masspayaccumulator_createdby_valueNavigation)
                    .HasForeignKey(d => d._createdby_value)
                    .HasConstraintName("FK_systemuser_ptas_masspayaccumulator_createdby");

                entity.HasOne(d => d._createdonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_masspayaccumulator_createdonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._createdonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_masspayaccumulator_createdonbehalfby");

                entity.HasOne(d => d._modifiedby_valueNavigation)
                    .WithMany(p => p.ptas_masspayaccumulator_modifiedby_valueNavigation)
                    .HasForeignKey(d => d._modifiedby_value)
                    .HasConstraintName("FK_systemuser_ptas_masspayaccumulator_modifiedby");

                entity.HasOne(d => d._modifiedonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_masspayaccumulator_modifiedonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._modifiedonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_masspayaccumulator_modifiedonbehalfby");

                entity.HasOne(d => d._owningteam_valueNavigation)
                    .WithMany(p => p.ptas_masspayaccumulator)
                    .HasForeignKey(d => d._owningteam_value)
                    .HasConstraintName("FK_team_ptas_masspayaccumulator_owningteam");

                entity.HasOne(d => d._owninguser_valueNavigation)
                    .WithMany(p => p.ptas_masspayaccumulator_owninguser_valueNavigation)
                    .HasForeignKey(d => d._owninguser_value)
                    .HasConstraintName("FK_systemuser_ptas_masspayaccumulator_owninguser");

                entity.HasOne(d => d._ptas_agentcodeid_valueNavigation)
                    .WithMany(p => p.ptas_masspayaccumulator)
                    .HasForeignKey(d => d._ptas_agentcodeid_value)
                    .HasConstraintName("FK_ptas_masspayer_ptas_masspayaccumulator_ptas_agentcodeid");

                entity.HasOne(d => d._ptas_taxaccountnumberid_valueNavigation)
                    .WithMany(p => p.ptas_masspayaccumulator)
                    .HasForeignKey(d => d._ptas_taxaccountnumberid_value)
                    .HasConstraintName("FK_ptas_taxaccount_ptas_masspayaccumulator_ptas_taxaccountnumberid");
            });

            modelBuilder.Entity<ptas_masspayaction>(entity =>
            {
                entity.ToTable("ptas_masspayaction", "dynamics");

                entity.Property(e => e.ptas_masspayactionid).ValueGeneratedNever();

                entity.HasOne(d => d._createdby_valueNavigation)
                    .WithMany(p => p.ptas_masspayaction_createdby_valueNavigation)
                    .HasForeignKey(d => d._createdby_value)
                    .HasConstraintName("FK_systemuser_ptas_masspayaction_createdby");

                entity.HasOne(d => d._createdonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_masspayaction_createdonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._createdonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_masspayaction_createdonbehalfby");

                entity.HasOne(d => d._modifiedby_valueNavigation)
                    .WithMany(p => p.ptas_masspayaction_modifiedby_valueNavigation)
                    .HasForeignKey(d => d._modifiedby_value)
                    .HasConstraintName("FK_systemuser_ptas_masspayaction_modifiedby");

                entity.HasOne(d => d._modifiedonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_masspayaction_modifiedonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._modifiedonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_masspayaction_modifiedonbehalfby");

                entity.HasOne(d => d._owningteam_valueNavigation)
                    .WithMany(p => p.ptas_masspayaction)
                    .HasForeignKey(d => d._owningteam_value)
                    .HasConstraintName("FK_team_ptas_masspayaction_owningteam");

                entity.HasOne(d => d._owninguser_valueNavigation)
                    .WithMany(p => p.ptas_masspayaction_owninguser_valueNavigation)
                    .HasForeignKey(d => d._owninguser_value)
                    .HasConstraintName("FK_systemuser_ptas_masspayaction_owninguser");

                entity.HasOne(d => d._ptas_agentcodeid_valueNavigation)
                    .WithMany(p => p.ptas_masspayaction)
                    .HasForeignKey(d => d._ptas_agentcodeid_value)
                    .HasConstraintName("FK_ptas_masspayer_ptas_masspayaction_ptas_agentcodeid");
            });

            modelBuilder.Entity<ptas_masspayer>(entity =>
            {
                entity.ToTable("ptas_masspayer", "dynamics");

                entity.HasIndex(e => e.modifiedon)
                    .HasName("Idx_ptas_masspayer_modifiedon");

                entity.Property(e => e.ptas_masspayerid).ValueGeneratedNever();

                entity.HasOne(d => d._createdby_valueNavigation)
                    .WithMany(p => p.ptas_masspayer_createdby_valueNavigation)
                    .HasForeignKey(d => d._createdby_value)
                    .HasConstraintName("FK_systemuser_ptas_masspayer_createdby");

                entity.HasOne(d => d._createdonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_masspayer_createdonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._createdonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_masspayer_createdonbehalfby");

                entity.HasOne(d => d._modifiedby_valueNavigation)
                    .WithMany(p => p.ptas_masspayer_modifiedby_valueNavigation)
                    .HasForeignKey(d => d._modifiedby_value)
                    .HasConstraintName("FK_systemuser_ptas_masspayer_modifiedby");

                entity.HasOne(d => d._modifiedonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_masspayer_modifiedonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._modifiedonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_masspayer_modifiedonbehalfby");

                entity.HasOne(d => d._owningteam_valueNavigation)
                    .WithMany(p => p.ptas_masspayer)
                    .HasForeignKey(d => d._owningteam_value)
                    .HasConstraintName("FK_team_ptas_masspayer_owningteam");

                entity.HasOne(d => d._owninguser_valueNavigation)
                    .WithMany(p => p.ptas_masspayer_owninguser_valueNavigation)
                    .HasForeignKey(d => d._owninguser_value)
                    .HasConstraintName("FK_systemuser_ptas_masspayer_owninguser");
            });

            modelBuilder.Entity<ptas_mediarepository>(entity =>
            {
                entity.ToTable("ptas_mediarepository", "dynamics");

                entity.HasIndex(e => e.modifiedon)
                    .HasName("Idx_ptas_mediarepository_modifiedon");

                entity.Property(e => e.ptas_mediarepositoryid).ValueGeneratedNever();

                entity.HasOne(d => d._createdby_valueNavigation)
                    .WithMany(p => p.ptas_mediarepository_createdby_valueNavigation)
                    .HasForeignKey(d => d._createdby_value)
                    .HasConstraintName("FK_systemuser_ptas_mediarepository_createdby");

                entity.HasOne(d => d._createdonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_mediarepository_createdonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._createdonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_mediarepository_createdonbehalfby");

                entity.HasOne(d => d._modifiedby_valueNavigation)
                    .WithMany(p => p.ptas_mediarepository_modifiedby_valueNavigation)
                    .HasForeignKey(d => d._modifiedby_value)
                    .HasConstraintName("FK_systemuser_ptas_mediarepository_modifiedby");

                entity.HasOne(d => d._modifiedonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_mediarepository_modifiedonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._modifiedonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_mediarepository_modifiedonbehalfby");

                entity.HasOne(d => d._owningteam_valueNavigation)
                    .WithMany(p => p.ptas_mediarepository)
                    .HasForeignKey(d => d._owningteam_value)
                    .HasConstraintName("FK_team_ptas_mediarepository_owningteam");

                entity.HasOne(d => d._owninguser_valueNavigation)
                    .WithMany(p => p.ptas_mediarepository_owninguser_valueNavigation)
                    .HasForeignKey(d => d._owninguser_value)
                    .HasConstraintName("FK_systemuser_ptas_mediarepository_owninguser");

                entity.HasOne(d => d._ptas_saleid_valueNavigation)
                    .WithMany(p => p.ptas_mediarepository)
                    .HasForeignKey(d => d._ptas_saleid_value)
                    .HasConstraintName("FK_ptas_sales_ptas_mediarepository_ptas_saleid");

                entity.HasOne(d => d._ptas_yearid_valueNavigation)
                    .WithMany(p => p.ptas_mediarepository)
                    .HasForeignKey(d => d._ptas_yearid_value)
                    .HasConstraintName("FK_ptas_year_ptas_mediarepository_ptas_yearid");
            });

            modelBuilder.Entity<ptas_naicscode>(entity =>
            {
                entity.ToTable("ptas_naicscode", "dynamics");

                entity.HasIndex(e => e.modifiedon)
                    .HasName("Idx_ptas_naicscode_modifiedon");

                entity.Property(e => e.ptas_naicscodeid).ValueGeneratedNever();

                entity.HasOne(d => d._createdby_valueNavigation)
                    .WithMany(p => p.ptas_naicscode_createdby_valueNavigation)
                    .HasForeignKey(d => d._createdby_value)
                    .HasConstraintName("FK_systemuser_ptas_naicscode_createdby");

                entity.HasOne(d => d._createdonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_naicscode_createdonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._createdonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_naicscode_createdonbehalfby");

                entity.HasOne(d => d._modifiedby_valueNavigation)
                    .WithMany(p => p.ptas_naicscode_modifiedby_valueNavigation)
                    .HasForeignKey(d => d._modifiedby_value)
                    .HasConstraintName("FK_systemuser_ptas_naicscode_modifiedby");

                entity.HasOne(d => d._modifiedonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_naicscode_modifiedonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._modifiedonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_naicscode_modifiedonbehalfby");
            });

            modelBuilder.Entity<ptas_neighborhood>(entity =>
            {
                entity.ToTable("ptas_neighborhood", "dynamics");

                entity.HasIndex(e => e.modifiedon)
                    .HasName("Idx_ptas_neighborhood_modifiedon");

                entity.Property(e => e.ptas_neighborhoodid).ValueGeneratedNever();

                entity.HasOne(d => d._createdby_valueNavigation)
                    .WithMany(p => p.ptas_neighborhood_createdby_valueNavigation)
                    .HasForeignKey(d => d._createdby_value)
                    .HasConstraintName("FK_systemuser_ptas_neighborhood_createdby");

                entity.HasOne(d => d._createdonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_neighborhood_createdonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._createdonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_neighborhood_createdonbehalfby");

                entity.HasOne(d => d._modifiedby_valueNavigation)
                    .WithMany(p => p.ptas_neighborhood_modifiedby_valueNavigation)
                    .HasForeignKey(d => d._modifiedby_value)
                    .HasConstraintName("FK_systemuser_ptas_neighborhood_modifiedby");

                entity.HasOne(d => d._modifiedonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_neighborhood_modifiedonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._modifiedonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_neighborhood_modifiedonbehalfby");

                entity.HasOne(d => d._ptas_areaid_valueNavigation)
                    .WithMany(p => p.ptas_neighborhood)
                    .HasForeignKey(d => d._ptas_areaid_value)
                    .HasConstraintName("FK_ptas_area_ptas_neighborhood_ptas_areaid");
            });

            modelBuilder.Entity<ptas_notificationconfiguration>(entity =>
            {
                entity.ToTable("ptas_notificationconfiguration", "dynamics");

                entity.Property(e => e.ptas_notificationconfigurationid).ValueGeneratedNever();

                entity.HasOne(d => d._createdby_valueNavigation)
                    .WithMany(p => p.ptas_notificationconfiguration_createdby_valueNavigation)
                    .HasForeignKey(d => d._createdby_value)
                    .HasConstraintName("FK_systemuser_ptas_notificationconfiguration_createdby");

                entity.HasOne(d => d._createdonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_notificationconfiguration_createdonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._createdonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_notificationconfiguration_createdonbehalfby");

                entity.HasOne(d => d._modifiedby_valueNavigation)
                    .WithMany(p => p.ptas_notificationconfiguration_modifiedby_valueNavigation)
                    .HasForeignKey(d => d._modifiedby_value)
                    .HasConstraintName("FK_systemuser_ptas_notificationconfiguration_modifiedby");

                entity.HasOne(d => d._modifiedonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_notificationconfiguration_modifiedonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._modifiedonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_notificationconfiguration_modifiedonbehalfby");

                entity.HasOne(d => d._owningteam_valueNavigation)
                    .WithMany(p => p.ptas_notificationconfiguration)
                    .HasForeignKey(d => d._owningteam_value)
                    .HasConstraintName("FK_team_ptas_notificationconfiguration_owningteam");

                entity.HasOne(d => d._owninguser_valueNavigation)
                    .WithMany(p => p.ptas_notificationconfiguration_owninguser_valueNavigation)
                    .HasForeignKey(d => d._owninguser_value)
                    .HasConstraintName("FK_systemuser_ptas_notificationconfiguration_owninguser");
            });

            modelBuilder.Entity<ptas_omit>(entity =>
            {
                entity.ToTable("ptas_omit", "dynamics");

                entity.Property(e => e.ptas_omitid).ValueGeneratedNever();

                entity.Property(e => e.exchangerate).HasColumnType("money");

                entity.Property(e => e.ptas_agriculturevalue).HasColumnType("money");

                entity.Property(e => e.ptas_agriculturevalue_base).HasColumnType("money");

                entity.Property(e => e.ptas_machineequipmentvalue).HasColumnType("money");

                entity.Property(e => e.ptas_machineequipmentvalue_base).HasColumnType("money");

                entity.Property(e => e.ptas_manufacturingvalue).HasColumnType("money");

                entity.Property(e => e.ptas_manufacturingvalue_base).HasColumnType("money");

                entity.Property(e => e.ptas_othervalue).HasColumnType("money");

                entity.Property(e => e.ptas_othervalue_base).HasColumnType("money");

                entity.Property(e => e.ptas_suppliesvalue).HasColumnType("money");

                entity.Property(e => e.ptas_suppliesvalue_base).HasColumnType("money");

                entity.HasOne(d => d._createdby_valueNavigation)
                    .WithMany(p => p.ptas_omit_createdby_valueNavigation)
                    .HasForeignKey(d => d._createdby_value)
                    .HasConstraintName("FK_systemuser_ptas_omit_createdby");

                entity.HasOne(d => d._createdonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_omit_createdonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._createdonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_omit_createdonbehalfby");

                entity.HasOne(d => d._modifiedby_valueNavigation)
                    .WithMany(p => p.ptas_omit_modifiedby_valueNavigation)
                    .HasForeignKey(d => d._modifiedby_value)
                    .HasConstraintName("FK_systemuser_ptas_omit_modifiedby");

                entity.HasOne(d => d._modifiedonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_omit_modifiedonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._modifiedonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_omit_modifiedonbehalfby");

                entity.HasOne(d => d._owningteam_valueNavigation)
                    .WithMany(p => p.ptas_omit)
                    .HasForeignKey(d => d._owningteam_value)
                    .HasConstraintName("FK_team_ptas_omit_owningteam");

                entity.HasOne(d => d._owninguser_valueNavigation)
                    .WithMany(p => p.ptas_omit_owninguser_valueNavigation)
                    .HasForeignKey(d => d._owninguser_value)
                    .HasConstraintName("FK_systemuser_ptas_omit_owninguser");

                entity.HasOne(d => d._ptas_assessmentyearid_valueNavigation)
                    .WithMany(p => p.ptas_omit_ptas_assessmentyearid_valueNavigation)
                    .HasForeignKey(d => d._ptas_assessmentyearid_value)
                    .HasConstraintName("FK_ptas_year_ptas_omit_ptas_assessmentyearid");

                entity.HasOne(d => d._ptas_levycodeid_valueNavigation)
                    .WithMany(p => p.ptas_omit_ptas_levycodeid_valueNavigation)
                    .HasForeignKey(d => d._ptas_levycodeid_value)
                    .HasConstraintName("FK_ptas_levycode_ptas_omit_ptas_levycodeid");

                entity.HasOne(d => d._ptas_omitlevycodeid_valueNavigation)
                    .WithMany(p => p.ptas_omit_ptas_omitlevycodeid_valueNavigation)
                    .HasForeignKey(d => d._ptas_omitlevycodeid_value)
                    .HasConstraintName("FK_ptas_levycode_ptas_omit_ptas_omitlevycodeid");

                entity.HasOne(d => d._ptas_omittedassessmentyearid_valueNavigation)
                    .WithMany(p => p.ptas_omit_ptas_omittedassessmentyearid_valueNavigation)
                    .HasForeignKey(d => d._ptas_omittedassessmentyearid_value)
                    .HasConstraintName("FK_ptas_year_ptas_omit_ptas_omittedassessmentyearid");
            });

            modelBuilder.Entity<ptas_parceldetail>(entity =>
            {
                entity.ToTable("ptas_parceldetail", "dynamics");

                entity.HasIndex(e => e.modifiedon)
                    .HasName("Idx_ptas_parceldetail_modifiedon");

                entity.HasIndex(e => e.ptas_name);

                entity.HasIndex(e => new { e.ptas_major, e.ptas_minor })
                    .HasName("Idx_ptas_parceldetail_ptas_major_ptas_minor");

                entity.Property(e => e.ptas_parceldetailid).ValueGeneratedNever();

                entity.Property(e => e.exchangerate).HasColumnType("money");

                entity.Property(e => e.ptas_benefitacres).HasColumnType("money");

                entity.Property(e => e.ptas_delinquenttaxesowed).HasColumnType("money");

                entity.Property(e => e.ptas_delinquenttaxesowed_base).HasColumnType("money");

                entity.Property(e => e.ptas_forestfireacres).HasColumnType("money");

                entity.Property(e => e.ptas_lotacreage).HasColumnType("money");

                entity.Property(e => e.ptas_major).HasMaxLength(6);

                entity.Property(e => e.ptas_minor).HasMaxLength(4);

                entity.Property(e => e.ptas_name).HasMaxLength(100);

                entity.Property(e => e.ptas_totalaccessoryvalue).HasColumnType("money");

                entity.Property(e => e.ptas_totalaccessoryvalue_base).HasColumnType("money");

                entity.HasOne(d => d._createdby_valueNavigation)
                    .WithMany(p => p.ptas_parceldetail_createdby_valueNavigation)
                    .HasForeignKey(d => d._createdby_value)
                    .HasConstraintName("FK_systemuser_ptas_parceldetail_createdby");

                entity.HasOne(d => d._createdonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_parceldetail_createdonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._createdonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_parceldetail_createdonbehalfby");

                entity.HasOne(d => d._modifiedby_valueNavigation)
                    .WithMany(p => p.ptas_parceldetail_modifiedby_valueNavigation)
                    .HasForeignKey(d => d._modifiedby_value)
                    .HasConstraintName("FK_systemuser_ptas_parceldetail_modifiedby");

                entity.HasOne(d => d._modifiedonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_parceldetail_modifiedonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._modifiedonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_parceldetail_modifiedonbehalfby");

                entity.HasOne(d => d._owningteam_valueNavigation)
                    .WithMany(p => p.ptas_parceldetail)
                    .HasForeignKey(d => d._owningteam_value)
                    .HasConstraintName("FK_team_ptas_parceldetail_owningteam");

                entity.HasOne(d => d._owninguser_valueNavigation)
                    .WithMany(p => p.ptas_parceldetail_owninguser_valueNavigation)
                    .HasForeignKey(d => d._owninguser_value)
                    .HasConstraintName("FK_systemuser_ptas_parceldetail_owninguser");

                entity.HasOne(d => d._ptas_addr1_cityid_valueNavigation)
                    .WithMany(p => p.ptas_parceldetail)
                    .HasForeignKey(d => d._ptas_addr1_cityid_value)
                    .HasConstraintName("FK_ptas_city_ptas_parceldetail_ptas_addr1_cityid");

                entity.HasOne(d => d._ptas_addr1_countryid_valueNavigation)
                    .WithMany(p => p.ptas_parceldetail)
                    .HasForeignKey(d => d._ptas_addr1_countryid_value)
                    .HasConstraintName("FK_ptas_country_ptas_parceldetail_ptas_addr1_countryid");

                entity.HasOne(d => d._ptas_addr1_stateid_valueNavigation)
                    .WithMany(p => p.ptas_parceldetail)
                    .HasForeignKey(d => d._ptas_addr1_stateid_value)
                    .HasConstraintName("FK_ptas_stateorprovince_ptas_parceldetail_ptas_addr1_stateid");

                entity.HasOne(d => d._ptas_addr1_streetnameid_valueNavigation)
                    .WithMany(p => p.ptas_parceldetail)
                    .HasForeignKey(d => d._ptas_addr1_streetnameid_value)
                    .HasConstraintName("FK_ptas_streetname_ptas_parceldetail_ptas_addr1_streetnameid");

                entity.HasOne(d => d._ptas_addr1_streettypeid_valueNavigation)
                    .WithMany(p => p.ptas_parceldetail)
                    .HasForeignKey(d => d._ptas_addr1_streettypeid_value)
                    .HasConstraintName("FK_ptas_streettype_ptas_parceldetail_ptas_addr1_streettypeid");

                entity.HasOne(d => d._ptas_addr1_zipcodeid_valueNavigation)
                    .WithMany(p => p.ptas_parceldetail)
                    .HasForeignKey(d => d._ptas_addr1_zipcodeid_value)
                    .HasConstraintName("FK_ptas_zipcode_ptas_parceldetail_ptas_addr1_zipcodeid");

                entity.HasOne(d => d._ptas_areaid_valueNavigation)
                    .WithMany(p => p.ptas_parceldetail)
                    .HasForeignKey(d => d._ptas_areaid_value)
                    .HasConstraintName("FK_ptas_area_ptas_parceldetail_ptas_areaid");

                entity.HasOne(d => d._ptas_assignedappraiserid_valueNavigation)
                    .WithMany(p => p.ptas_parceldetail_ptas_assignedappraiserid_valueNavigation)
                    .HasForeignKey(d => d._ptas_assignedappraiserid_value)
                    .HasConstraintName("FK_systemuser_ptas_parceldetail_ptas_assignedappraiserid");

                entity.HasOne(d => d._ptas_districtid_valueNavigation)
                    .WithMany(p => p.ptas_parceldetail)
                    .HasForeignKey(d => d._ptas_districtid_value)
                    .HasConstraintName("FK_ptas_district_ptas_parceldetail_ptas_districtid");

                entity.HasOne(d => d._ptas_economicunit_valueNavigation)
                    .WithMany(p => p.ptas_parceldetail)
                    .HasForeignKey(d => d._ptas_economicunit_value)
                    .HasConstraintName("FK_ptas_economicunit_ptas_parceldetail_ptas_economicunit");

                entity.HasOne(d => d._ptas_geoareaid_valueNavigation)
                    .WithMany(p => p.ptas_parceldetail)
                    .HasForeignKey(d => d._ptas_geoareaid_value)
                    .HasConstraintName("FK_ptas_geoarea_ptas_parceldetail_ptas_geoareaid");

                entity.HasOne(d => d._ptas_geonbhdid_valueNavigation)
                    .WithMany(p => p.ptas_parceldetail)
                    .HasForeignKey(d => d._ptas_geonbhdid_value)
                    .HasConstraintName("FK_ptas_geoneighborhood_ptas_parceldetail_ptas_geonbhdid");

                entity.HasOne(d => d._ptas_jurisdiction_valueNavigation)
                    .WithMany(p => p.ptas_parceldetail)
                    .HasForeignKey(d => d._ptas_jurisdiction_value)
                    .HasConstraintName("FK_ptas_jurisdiction_ptas_parceldetail_ptas_jurisdiction");

                entity.HasOne(d => d._ptas_landid_valueNavigation)
                    .WithMany(p => p.ptas_parceldetail)
                    .HasForeignKey(d => d._ptas_landid_value)
                    .HasConstraintName("FK_ptas_land_ptas_parceldetail_ptas_landid");

                entity.HasOne(d => d._ptas_landinspectedbyid_valueNavigation)
                    .WithMany(p => p.ptas_parceldetail_ptas_landinspectedbyid_valueNavigation)
                    .HasForeignKey(d => d._ptas_landinspectedbyid_value)
                    .HasConstraintName("FK_systemuser_ptas_parceldetail_ptas_landinspectedbyid");

                entity.HasOne(d => d._ptas_levycodeid_valueNavigation)
                    .WithMany(p => p.ptas_parceldetail)
                    .HasForeignKey(d => d._ptas_levycodeid_value)
                    .HasConstraintName("FK_ptas_levycode_ptas_parceldetail_ptas_levycodeid");

                entity.HasOne(d => d._ptas_masterparcelid_valueNavigation)
                    .WithMany(p => p.Inverse_ptas_masterparcelid_valueNavigation)
                    .HasForeignKey(d => d._ptas_masterparcelid_value)
                    .HasConstraintName("FK_ptas_parceldetail_ptas_parceldetail_ptas_masterparcelid");

                entity.HasOne(d => d._ptas_neighborhoodid_valueNavigation)
                    .WithMany(p => p.ptas_parceldetail)
                    .HasForeignKey(d => d._ptas_neighborhoodid_value)
                    .HasConstraintName("FK_ptas_neighborhood_ptas_parceldetail_ptas_neighborhoodid");

                entity.HasOne(d => d._ptas_parcelinspectedbyid_valueNavigation)
                    .WithMany(p => p.ptas_parceldetail_ptas_parcelinspectedbyid_valueNavigation)
                    .HasForeignKey(d => d._ptas_parcelinspectedbyid_value)
                    .HasConstraintName("FK_systemuser_ptas_parceldetail_ptas_parcelinspectedbyid");

                entity.HasOne(d => d._ptas_propertytypeid_valueNavigation)
                    .WithMany(p => p.ptas_parceldetail)
                    .HasForeignKey(d => d._ptas_propertytypeid_value)
                    .HasConstraintName("FK_ptas_propertytype_ptas_parceldetail_ptas_propertytypeid");

                entity.HasOne(d => d._ptas_qstrid_valueNavigation)
                    .WithMany(p => p.ptas_parceldetail)
                    .HasForeignKey(d => d._ptas_qstrid_value)
                    .HasConstraintName("FK_ptas_qstr_ptas_parceldetail_ptas_qstrid");

                entity.HasOne(d => d._ptas_responsibilityid_valueNavigation)
                    .WithMany(p => p.ptas_parceldetail)
                    .HasForeignKey(d => d._ptas_responsibilityid_value)
                    .HasConstraintName("FK_ptas_responsibility_ptas_parceldetail_ptas_responsibilityid");

                entity.HasOne(d => d._ptas_saleid_valueNavigation)
                    .WithMany(p => p.ptas_parceldetail)
                    .HasForeignKey(d => d._ptas_saleid_value)
                    .HasConstraintName("FK_ptas_sales_ptas_parceldetail_ptas_saleid");

                entity.HasOne(d => d._ptas_specialtyappraiserid_valueNavigation)
                    .WithMany(p => p.ptas_parceldetail_ptas_specialtyappraiserid_valueNavigation)
                    .HasForeignKey(d => d._ptas_specialtyappraiserid_value)
                    .HasConstraintName("FK_systemuser_ptas_parceldetail_ptas_specialtyappraiserid");

                entity.HasOne(d => d._ptas_specialtyareaid_valueNavigation)
                    .WithMany(p => p.ptas_parceldetail)
                    .HasForeignKey(d => d._ptas_specialtyareaid_value)
                    .HasConstraintName("FK_ptas_specialtyarea_ptas_parceldetail_ptas_specialtyareaid");

                entity.HasOne(d => d._ptas_specialtynbhdid_valueNavigation)
                    .WithMany(p => p.ptas_parceldetail)
                    .HasForeignKey(d => d._ptas_specialtynbhdid_value)
                    .HasConstraintName("FK_ptas_specialtyneighborhood_ptas_parceldetail_ptas_specialtynbhdid");

                entity.HasOne(d => d._ptas_splitaccount1id_valueNavigation)
                    .WithMany(p => p.ptas_parceldetail_ptas_splitaccount1id_valueNavigation)
                    .HasForeignKey(d => d._ptas_splitaccount1id_value)
                    .HasConstraintName("FK_ptas_taxaccount_ptas_parceldetail_ptas_splitaccount1id");

                entity.HasOne(d => d._ptas_splitaccount2id_valueNavigation)
                    .WithMany(p => p.ptas_parceldetail_ptas_splitaccount2id_valueNavigation)
                    .HasForeignKey(d => d._ptas_splitaccount2id_value)
                    .HasConstraintName("FK_ptas_taxaccount_ptas_parceldetail_ptas_splitaccount2id");

                entity.HasOne(d => d._ptas_subareaid_valueNavigation)
                    .WithMany(p => p.ptas_parceldetail)
                    .HasForeignKey(d => d._ptas_subareaid_value)
                    .HasConstraintName("FK_ptas_subarea_ptas_parceldetail_ptas_subareaid");

                entity.HasOne(d => d._ptas_submarketid_valueNavigation)
                    .WithMany(p => p.ptas_parceldetail)
                    .HasForeignKey(d => d._ptas_submarketid_value)
                    .HasConstraintName("FK_ptas_submarket_ptas_parceldetail_ptas_submarketid");

                entity.HasOne(d => d._ptas_supergroupdid_valueNavigation)
                    .WithMany(p => p.ptas_parceldetail)
                    .HasForeignKey(d => d._ptas_supergroupdid_value)
                    .HasConstraintName("FK_ptas_supergroup_ptas_parceldetail_ptas_supergroupdid");

                entity.HasOne(d => d._ptas_taxaccountid_valueNavigation)
                    .WithMany(p => p.ptas_parceldetail_ptas_taxaccountid_valueNavigation)
                    .HasForeignKey(d => d._ptas_taxaccountid_value)
                    .HasConstraintName("FK_ptas_taxaccount_ptas_parceldetail_ptas_taxaccountid");
            });

            modelBuilder.Entity<ptas_parceldetail_ptas_mediarepository>(entity =>
            {
                entity.ToTable("ptas_parceldetail_ptas_mediarepository", "dynamics");

                entity.HasIndex(e => e.modifiedon)
                    .HasName("Idx_ptas_parceldetail_ptas_mediarepository_modifiedon");

                entity.Property(e => e.ptas_parceldetail_ptas_mediarepositoryid).ValueGeneratedNever();

                entity.Property(e => e.modifiedon)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");
            });

            modelBuilder.Entity<ptas_parceldetail_snapshot>(entity =>
            {
                entity.HasKey(e => e.ptas_parceldetailid);

                entity.ToTable("ptas_parceldetail_snapshot", "dynamics");

                entity.Property(e => e.ptas_parceldetailid).ValueGeneratedNever();

                entity.Property(e => e.exchangerate).HasColumnType("money");

                entity.Property(e => e.ptas_benefitacres).HasColumnType("money");

                entity.Property(e => e.ptas_delinquenttaxesowed).HasColumnType("money");

                entity.Property(e => e.ptas_delinquenttaxesowed_base).HasColumnType("money");

                entity.Property(e => e.ptas_forestfireacres).HasColumnType("money");

                entity.Property(e => e.ptas_lotacreage).HasColumnType("money");

                entity.Property(e => e.ptas_totalaccessoryvalue).HasColumnType("money");

                entity.Property(e => e.ptas_totalaccessoryvalue_base).HasColumnType("money");
            });

            modelBuilder.Entity<ptas_parceleconomicunit>(entity =>
            {
                entity.ToTable("ptas_parceleconomicunit", "dynamics");

                entity.HasIndex(e => e.modifiedon)
                    .HasName("Idx_ptas_parceleconomicunit_modifiedon");

                entity.Property(e => e.ptas_parceleconomicunitid).ValueGeneratedNever();

                entity.HasOne(d => d._createdby_valueNavigation)
                    .WithMany(p => p.ptas_parceleconomicunit_createdby_valueNavigation)
                    .HasForeignKey(d => d._createdby_value)
                    .HasConstraintName("FK_systemuser_ptas_parceleconomicunit_createdby");

                entity.HasOne(d => d._createdonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_parceleconomicunit_createdonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._createdonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_parceleconomicunit_createdonbehalfby");

                entity.HasOne(d => d._modifiedby_valueNavigation)
                    .WithMany(p => p.ptas_parceleconomicunit_modifiedby_valueNavigation)
                    .HasForeignKey(d => d._modifiedby_value)
                    .HasConstraintName("FK_systemuser_ptas_parceleconomicunit_modifiedby");

                entity.HasOne(d => d._modifiedonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_parceleconomicunit_modifiedonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._modifiedonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_parceleconomicunit_modifiedonbehalfby");

                entity.HasOne(d => d._owningteam_valueNavigation)
                    .WithMany(p => p.ptas_parceleconomicunit)
                    .HasForeignKey(d => d._owningteam_value)
                    .HasConstraintName("FK_team_ptas_parceleconomicunit_owningteam");

                entity.HasOne(d => d._owninguser_valueNavigation)
                    .WithMany(p => p.ptas_parceleconomicunit_owninguser_valueNavigation)
                    .HasForeignKey(d => d._owninguser_value)
                    .HasConstraintName("FK_systemuser_ptas_parceleconomicunit_owninguser");

                entity.HasOne(d => d._ptas_economicunitid_valueNavigation)
                    .WithMany(p => p.ptas_parceleconomicunit)
                    .HasForeignKey(d => d._ptas_economicunitid_value)
                    .HasConstraintName("FK_ptas_economicunit_ptas_parceleconomicunit_ptas_economicunitid");

                entity.HasOne(d => d._ptas_effectiveyearid_valueNavigation)
                    .WithMany(p => p.ptas_parceleconomicunit_ptas_effectiveyearid_valueNavigation)
                    .HasForeignKey(d => d._ptas_effectiveyearid_value)
                    .HasConstraintName("FK_ptas_year_ptas_parceleconomicunit_ptas_effectiveyearid");

                entity.HasOne(d => d._ptas_parcelid_valueNavigation)
                    .WithMany(p => p.ptas_parceleconomicunit)
                    .HasForeignKey(d => d._ptas_parcelid_value)
                    .HasConstraintName("FK_ptas_parceldetail_ptas_parceleconomicunit_ptas_parcelid");

                entity.HasOne(d => d._ptas_yearbuiltid_valueNavigation)
                    .WithMany(p => p.ptas_parceleconomicunit_ptas_yearbuiltid_valueNavigation)
                    .HasForeignKey(d => d._ptas_yearbuiltid_value)
                    .HasConstraintName("FK_ptas_year_ptas_parceleconomicunit_ptas_yearbuiltid");
            });

            modelBuilder.Entity<ptas_parkingdistrict>(entity =>
            {
                entity.ToTable("ptas_parkingdistrict", "dynamics");

                entity.HasIndex(e => e.modifiedon)
                    .HasName("Idx_ptas_parkingdistrict_modifiedon");

                entity.Property(e => e.ptas_parkingdistrictid).ValueGeneratedNever();

                entity.Property(e => e.exchangerate).HasColumnType("money");

                entity.Property(e => e.ptas_caprate).HasColumnType("money");

                entity.Property(e => e.ptas_dailyrate).HasColumnType("money");

                entity.Property(e => e.ptas_dailyrate_base).HasColumnType("money");

                entity.Property(e => e.ptas_monthlyrate).HasColumnType("money");

                entity.Property(e => e.ptas_monthlyrate_base).HasColumnType("money");

                entity.Property(e => e.ptas_operatingexpenses).HasColumnType("money");

                entity.Property(e => e.ptas_operatingexpenses_base).HasColumnType("money");

                entity.HasOne(d => d._createdby_valueNavigation)
                    .WithMany(p => p.ptas_parkingdistrict_createdby_valueNavigation)
                    .HasForeignKey(d => d._createdby_value)
                    .HasConstraintName("FK_systemuser_ptas_parkingdistrict_createdby");

                entity.HasOne(d => d._createdonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_parkingdistrict_createdonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._createdonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_parkingdistrict_createdonbehalfby");

                entity.HasOne(d => d._modifiedby_valueNavigation)
                    .WithMany(p => p.ptas_parkingdistrict_modifiedby_valueNavigation)
                    .HasForeignKey(d => d._modifiedby_value)
                    .HasConstraintName("FK_systemuser_ptas_parkingdistrict_modifiedby");

                entity.HasOne(d => d._modifiedonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_parkingdistrict_modifiedonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._modifiedonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_parkingdistrict_modifiedonbehalfby");

                entity.HasOne(d => d._owningteam_valueNavigation)
                    .WithMany(p => p.ptas_parkingdistrict)
                    .HasForeignKey(d => d._owningteam_value)
                    .HasConstraintName("FK_team_ptas_parkingdistrict_owningteam");

                entity.HasOne(d => d._owninguser_valueNavigation)
                    .WithMany(p => p.ptas_parkingdistrict_owninguser_valueNavigation)
                    .HasForeignKey(d => d._owninguser_value)
                    .HasConstraintName("FK_systemuser_ptas_parkingdistrict_owninguser");
            });

            modelBuilder.Entity<ptas_permit>(entity =>
            {
                entity.ToTable("ptas_permit", "dynamics");

                entity.HasIndex(e => e.modifiedon)
                    .HasName("Idx_ptas_permit_modifiedon");

                entity.Property(e => e.ptas_permitid).ValueGeneratedNever();

                entity.Property(e => e.exchangerate).HasColumnType("money");

                entity.Property(e => e.ptas_permitvalue).HasColumnType("money");

                entity.Property(e => e.ptas_permitvalue_base).HasColumnType("money");

                entity.HasOne(d => d._createdby_valueNavigation)
                    .WithMany(p => p.ptas_permit_createdby_valueNavigation)
                    .HasForeignKey(d => d._createdby_value)
                    .HasConstraintName("FK_systemuser_ptas_permit_createdby");

                entity.HasOne(d => d._createdonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_permit_createdonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._createdonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_permit_createdonbehalfby");

                entity.HasOne(d => d._modifiedby_valueNavigation)
                    .WithMany(p => p.ptas_permit_modifiedby_valueNavigation)
                    .HasForeignKey(d => d._modifiedby_value)
                    .HasConstraintName("FK_systemuser_ptas_permit_modifiedby");

                entity.HasOne(d => d._modifiedonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_permit_modifiedonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._modifiedonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_permit_modifiedonbehalfby");

                entity.HasOne(d => d._owningteam_valueNavigation)
                    .WithMany(p => p.ptas_permit)
                    .HasForeignKey(d => d._owningteam_value)
                    .HasConstraintName("FK_team_ptas_permit_owningteam");

                entity.HasOne(d => d._owninguser_valueNavigation)
                    .WithMany(p => p.ptas_permit_owninguser_valueNavigation)
                    .HasForeignKey(d => d._owninguser_value)
                    .HasConstraintName("FK_systemuser_ptas_permit_owninguser");

                entity.HasOne(d => d._ptas_condounitid_valueNavigation)
                    .WithMany(p => p.ptas_permit_ptas_condounitid_valueNavigation)
                    .HasForeignKey(d => d._ptas_condounitid_value)
                    .HasConstraintName("FK_ptas_parceldetail_ptas_permit_ptas_condounitid");

                entity.HasOne(d => d._ptas_currentjurisdiction_valueNavigation)
                    .WithMany(p => p.ptas_permit_ptas_currentjurisdiction_valueNavigation)
                    .HasForeignKey(d => d._ptas_currentjurisdiction_value)
                    .HasConstraintName("FK_ptas_jurisdiction_ptas_permit_ptas_currentjurisdiction");

                entity.HasOne(d => d._ptas_issuedbyid_valueNavigation)
                    .WithMany(p => p.ptas_permit_ptas_issuedbyid_valueNavigation)
                    .HasForeignKey(d => d._ptas_issuedbyid_value)
                    .HasConstraintName("FK_ptas_jurisdiction_ptas_permit_ptas_issuedbyid");

                entity.HasOne(d => d._ptas_parcelid_valueNavigation)
                    .WithMany(p => p.ptas_permit_ptas_parcelid_valueNavigation)
                    .HasForeignKey(d => d._ptas_parcelid_value)
                    .HasConstraintName("FK_ptas_parceldetail_ptas_permit_ptas_parcelid");

                entity.HasOne(d => d._ptas_reviewedbyid_valueNavigation)
                    .WithMany(p => p.ptas_permit_ptas_reviewedbyid_valueNavigation)
                    .HasForeignKey(d => d._ptas_reviewedbyid_value)
                    .HasConstraintName("FK_systemuser_ptas_permit_ptas_reviewedbyid");

                entity.HasOne(d => d._ptas_statusupdatedbyid_valueNavigation)
                    .WithMany(p => p.ptas_permit_ptas_statusupdatedbyid_valueNavigation)
                    .HasForeignKey(d => d._ptas_statusupdatedbyid_value)
                    .HasConstraintName("FK_systemuser_ptas_permit_ptas_statusupdatedbyid");
            });

            modelBuilder.Entity<ptas_permit_ptas_mediarepository>(entity =>
            {
                entity.ToTable("ptas_permit_ptas_mediarepository", "dynamics");

                entity.Property(e => e.ptas_permit_ptas_mediarepositoryid).ValueGeneratedNever();
            });

            modelBuilder.Entity<ptas_permitinspectionhistory>(entity =>
            {
                entity.ToTable("ptas_permitinspectionhistory", "dynamics");

                entity.HasIndex(e => e.modifiedon)
                    .HasName("Idx_ptas_permitinspectionhistory_modifiedon");

                entity.Property(e => e.ptas_permitinspectionhistoryid).ValueGeneratedNever();

                entity.HasOne(d => d._createdby_valueNavigation)
                    .WithMany(p => p.ptas_permitinspectionhistory_createdby_valueNavigation)
                    .HasForeignKey(d => d._createdby_value)
                    .HasConstraintName("FK_systemuser_ptas_permitinspectionhistory_createdby");

                entity.HasOne(d => d._createdonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_permitinspectionhistory_createdonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._createdonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_permitinspectionhistory_createdonbehalfby");

                entity.HasOne(d => d._modifiedby_valueNavigation)
                    .WithMany(p => p.ptas_permitinspectionhistory_modifiedby_valueNavigation)
                    .HasForeignKey(d => d._modifiedby_value)
                    .HasConstraintName("FK_systemuser_ptas_permitinspectionhistory_modifiedby");

                entity.HasOne(d => d._modifiedonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_permitinspectionhistory_modifiedonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._modifiedonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_permitinspectionhistory_modifiedonbehalfby");

                entity.HasOne(d => d._owningteam_valueNavigation)
                    .WithMany(p => p.ptas_permitinspectionhistory)
                    .HasForeignKey(d => d._owningteam_value)
                    .HasConstraintName("FK_team_ptas_permitinspectionhistory_owningteam");

                entity.HasOne(d => d._owninguser_valueNavigation)
                    .WithMany(p => p.ptas_permitinspectionhistory_owninguser_valueNavigation)
                    .HasForeignKey(d => d._owninguser_value)
                    .HasConstraintName("FK_systemuser_ptas_permitinspectionhistory_owninguser");

                entity.HasOne(d => d._ptas_parcelid_valueNavigation)
                    .WithMany(p => p.ptas_permitinspectionhistory)
                    .HasForeignKey(d => d._ptas_parcelid_value)
                    .HasConstraintName("FK_ptas_parceldetail_ptas_permitinspectionhistory_ptas_parcelid");

                entity.HasOne(d => d._ptas_permitid_valueNavigation)
                    .WithMany(p => p.ptas_permitinspectionhistory)
                    .HasForeignKey(d => d._ptas_permitid_value)
                    .HasConstraintName("FK_ptas_permit_ptas_permitinspectionhistory_ptas_permitid");
            });

            modelBuilder.Entity<ptas_permitwebsiteconfig>(entity =>
            {
                entity.ToTable("ptas_permitwebsiteconfig", "dynamics");

                entity.HasIndex(e => e.modifiedon)
                    .HasName("Idx_ptas_permitwebsiteconfig_modifiedon");

                entity.Property(e => e.ptas_permitwebsiteconfigid).ValueGeneratedNever();

                entity.HasOne(d => d._createdby_valueNavigation)
                    .WithMany(p => p.ptas_permitwebsiteconfig_createdby_valueNavigation)
                    .HasForeignKey(d => d._createdby_value)
                    .HasConstraintName("FK_systemuser_ptas_permitwebsiteconfig_createdby");

                entity.HasOne(d => d._createdonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_permitwebsiteconfig_createdonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._createdonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_permitwebsiteconfig_createdonbehalfby");

                entity.HasOne(d => d._modifiedby_valueNavigation)
                    .WithMany(p => p.ptas_permitwebsiteconfig_modifiedby_valueNavigation)
                    .HasForeignKey(d => d._modifiedby_value)
                    .HasConstraintName("FK_systemuser_ptas_permitwebsiteconfig_modifiedby");

                entity.HasOne(d => d._modifiedonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_permitwebsiteconfig_modifiedonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._modifiedonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_permitwebsiteconfig_modifiedonbehalfby");

                entity.HasOne(d => d._owningteam_valueNavigation)
                    .WithMany(p => p.ptas_permitwebsiteconfig)
                    .HasForeignKey(d => d._owningteam_value)
                    .HasConstraintName("FK_team_ptas_permitwebsiteconfig_owningteam");

                entity.HasOne(d => d._owninguser_valueNavigation)
                    .WithMany(p => p.ptas_permitwebsiteconfig_owninguser_valueNavigation)
                    .HasForeignKey(d => d._owninguser_value)
                    .HasConstraintName("FK_systemuser_ptas_permitwebsiteconfig_owninguser");
            });

            modelBuilder.Entity<ptas_phonenumber>(entity =>
            {
                entity.ToTable("ptas_phonenumber", "dynamics");

                entity.Property(e => e.ptas_phonenumberid).ValueGeneratedNever();

                entity.Property(e => e.ptas_phonenumber1).HasColumnName("ptas_phonenumber");

                entity.HasOne(d => d._createdby_valueNavigation)
                    .WithMany(p => p.ptas_phonenumber_createdby_valueNavigation)
                    .HasForeignKey(d => d._createdby_value)
                    .HasConstraintName("FK_systemuser_ptas_phonenumber_createdby");

                entity.HasOne(d => d._createdonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_phonenumber_createdonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._createdonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_phonenumber_createdonbehalfby");

                entity.HasOne(d => d._modifiedby_valueNavigation)
                    .WithMany(p => p.ptas_phonenumber_modifiedby_valueNavigation)
                    .HasForeignKey(d => d._modifiedby_value)
                    .HasConstraintName("FK_systemuser_ptas_phonenumber_modifiedby");

                entity.HasOne(d => d._modifiedonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_phonenumber_modifiedonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._modifiedonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_phonenumber_modifiedonbehalfby");

                entity.HasOne(d => d._owningteam_valueNavigation)
                    .WithMany(p => p.ptas_phonenumber)
                    .HasForeignKey(d => d._owningteam_value)
                    .HasConstraintName("FK_team_ptas_phonenumber_owningteam");

                entity.HasOne(d => d._owninguser_valueNavigation)
                    .WithMany(p => p.ptas_phonenumber_owninguser_valueNavigation)
                    .HasForeignKey(d => d._owninguser_value)
                    .HasConstraintName("FK_systemuser_ptas_phonenumber_owninguser");

                entity.HasOne(d => d._ptas_portalcontact_valueNavigation)
                    .WithMany(p => p.ptas_phonenumber)
                    .HasForeignKey(d => d._ptas_portalcontact_value)
                    .HasConstraintName("FK_ptas_portalcontact_ptas_phonenumber_ptas_portalcontact");
            });

            modelBuilder.Entity<ptas_portaladdress>(entity =>
            {
                entity.ToTable("ptas_portaladdress", "dynamics");

                entity.Property(e => e.ptas_portaladdressid).ValueGeneratedNever();
            });

            modelBuilder.Entity<ptas_portalcontact>(entity =>
            {
                entity.ToTable("ptas_portalcontact", "dynamics");

                entity.Property(e => e.ptas_portalcontactid).ValueGeneratedNever();

                entity.HasOne(d => d._createdby_valueNavigation)
                    .WithMany(p => p.ptas_portalcontact_createdby_valueNavigation)
                    .HasForeignKey(d => d._createdby_value)
                    .HasConstraintName("FK_systemuser_ptas_portalcontact_createdby");

                entity.HasOne(d => d._createdonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_portalcontact_createdonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._createdonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_portalcontact_createdonbehalfby");

                entity.HasOne(d => d._modifiedby_valueNavigation)
                    .WithMany(p => p.ptas_portalcontact_modifiedby_valueNavigation)
                    .HasForeignKey(d => d._modifiedby_value)
                    .HasConstraintName("FK_systemuser_ptas_portalcontact_modifiedby");

                entity.HasOne(d => d._modifiedonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_portalcontact_modifiedonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._modifiedonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_portalcontact_modifiedonbehalfby");

                entity.HasOne(d => d._owningteam_valueNavigation)
                    .WithMany(p => p.ptas_portalcontact)
                    .HasForeignKey(d => d._owningteam_value)
                    .HasConstraintName("FK_team_ptas_portalcontact_owningteam");

                entity.HasOne(d => d._owninguser_valueNavigation)
                    .WithMany(p => p.ptas_portalcontact_owninguser_valueNavigation)
                    .HasForeignKey(d => d._owninguser_value)
                    .HasConstraintName("FK_systemuser_ptas_portalcontact_owninguser");
            });

            modelBuilder.Entity<ptas_portalcontact_ptas_parceldetail>(entity =>
            {
                entity.ToTable("ptas_portalcontact_ptas_parceldetail", "dynamics");

                entity.Property(e => e.ptas_portalcontact_ptas_parceldetailid).ValueGeneratedNever();
            });

            modelBuilder.Entity<ptas_portalemail>(entity =>
            {
                entity.ToTable("ptas_portalemail", "dynamics");

                entity.Property(e => e.ptas_portalemailid).ValueGeneratedNever();

                entity.HasOne(d => d._createdby_valueNavigation)
                    .WithMany(p => p.ptas_portalemail_createdby_valueNavigation)
                    .HasForeignKey(d => d._createdby_value)
                    .HasConstraintName("FK_systemuser_ptas_portalemail_createdby");

                entity.HasOne(d => d._createdonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_portalemail_createdonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._createdonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_portalemail_createdonbehalfby");

                entity.HasOne(d => d._modifiedby_valueNavigation)
                    .WithMany(p => p.ptas_portalemail_modifiedby_valueNavigation)
                    .HasForeignKey(d => d._modifiedby_value)
                    .HasConstraintName("FK_systemuser_ptas_portalemail_modifiedby");

                entity.HasOne(d => d._modifiedonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_portalemail_modifiedonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._modifiedonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_portalemail_modifiedonbehalfby");

                entity.HasOne(d => d._owningteam_valueNavigation)
                    .WithMany(p => p.ptas_portalemail)
                    .HasForeignKey(d => d._owningteam_value)
                    .HasConstraintName("FK_team_ptas_portalemail_owningteam");

                entity.HasOne(d => d._owninguser_valueNavigation)
                    .WithMany(p => p.ptas_portalemail_owninguser_valueNavigation)
                    .HasForeignKey(d => d._owninguser_value)
                    .HasConstraintName("FK_systemuser_ptas_portalemail_owninguser");

                entity.HasOne(d => d._ptas_portalcontact_valueNavigation)
                    .WithMany(p => p.ptas_portalemail)
                    .HasForeignKey(d => d._ptas_portalcontact_value)
                    .HasConstraintName("FK_ptas_portalcontact_ptas_portalemail_ptas_portalcontact");
            });

            modelBuilder.Entity<ptas_projectdock>(entity =>
            {
                entity.ToTable("ptas_projectdock", "dynamics");

                entity.HasIndex(e => e.modifiedon)
                    .HasName("Idx_ptas_projectdock_modifiedon");

                entity.Property(e => e.ptas_projectdockid).ValueGeneratedNever();

                entity.HasOne(d => d._createdby_valueNavigation)
                    .WithMany(p => p.ptas_projectdock_createdby_valueNavigation)
                    .HasForeignKey(d => d._createdby_value)
                    .HasConstraintName("FK_systemuser_ptas_projectdock_createdby");

                entity.HasOne(d => d._createdonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_projectdock_createdonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._createdonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_projectdock_createdonbehalfby");

                entity.HasOne(d => d._modifiedby_valueNavigation)
                    .WithMany(p => p.ptas_projectdock_modifiedby_valueNavigation)
                    .HasForeignKey(d => d._modifiedby_value)
                    .HasConstraintName("FK_systemuser_ptas_projectdock_modifiedby");

                entity.HasOne(d => d._modifiedonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_projectdock_modifiedonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._modifiedonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_projectdock_modifiedonbehalfby");

                entity.HasOne(d => d._ptas_condocomplexid_valueNavigation)
                    .WithMany(p => p.ptas_projectdock)
                    .HasForeignKey(d => d._ptas_condocomplexid_value)
                    .HasConstraintName("FK_ptas_condocomplex_ptas_projectdock_ptas_condocomplexid");

                entity.HasOne(d => d._ptas_masterdockid_valueNavigation)
                    .WithMany(p => p.Inverse_ptas_masterdockid_valueNavigation)
                    .HasForeignKey(d => d._ptas_masterdockid_value)
                    .HasConstraintName("FK_ptas_projectdock_ptas_projectdock_ptas_masterdockid");
            });

            modelBuilder.Entity<ptas_projectdock_snapshot>(entity =>
            {
                entity.HasKey(e => e.ptas_projectdockid);

                entity.ToTable("ptas_projectdock_snapshot", "dynamics");

                entity.Property(e => e.ptas_projectdockid).ValueGeneratedNever();
            });

            modelBuilder.Entity<ptas_propertytype>(entity =>
            {
                entity.ToTable("ptas_propertytype", "dynamics");

                entity.HasIndex(e => e.modifiedon)
                    .HasName("Idx_ptas_propertytype_modifiedon");

                entity.Property(e => e.ptas_propertytypeid).ValueGeneratedNever();

                entity.HasOne(d => d._createdby_valueNavigation)
                    .WithMany(p => p.ptas_propertytype_createdby_valueNavigation)
                    .HasForeignKey(d => d._createdby_value)
                    .HasConstraintName("FK_systemuser_ptas_propertytype_createdby");

                entity.HasOne(d => d._createdonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_propertytype_createdonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._createdonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_propertytype_createdonbehalfby");

                entity.HasOne(d => d._modifiedby_valueNavigation)
                    .WithMany(p => p.ptas_propertytype_modifiedby_valueNavigation)
                    .HasForeignKey(d => d._modifiedby_value)
                    .HasConstraintName("FK_systemuser_ptas_propertytype_modifiedby");

                entity.HasOne(d => d._modifiedonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_propertytype_modifiedonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._modifiedonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_propertytype_modifiedonbehalfby");
            });

            modelBuilder.Entity<ptas_ptas_bookmark_ptas_bookmarktag>(entity =>
            {
                entity.ToTable("ptas_ptas_bookmark_ptas_bookmarktag", "dynamics");

                entity.HasIndex(e => e.modifiedon)
                    .HasName("Idx_ptas_ptas_bookmark_ptas_bookmarktag_modifiedon");

                entity.Property(e => e.ptas_ptas_bookmark_ptas_bookmarktagid).ValueGeneratedNever();

                entity.Property(e => e.modifiedon)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");
            });

            modelBuilder.Entity<ptas_ptas_camanotes_ptas_fileattachmentmetad>(entity =>
            {
                entity.ToTable("ptas_ptas_camanotes_ptas_fileattachmentmetad", "dynamics");

                entity.HasIndex(e => e.modifiedon)
                    .HasName("Idx_ptas_ptas_camanotes_ptas_fileattachmentmetad_modifiedon");

                entity.Property(e => e.ptas_ptas_camanotes_ptas_fileattachmentmetadid).ValueGeneratedNever();

                entity.Property(e => e.modifiedon)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");
            });

            modelBuilder.Entity<ptas_ptas_fileattachmentmetadata_ptas_addres>(entity =>
            {
                entity.ToTable("ptas_ptas_fileattachmentmetadata_ptas_addres", "dynamics");

                entity.Property(e => e.ptas_ptas_fileattachmentmetadata_ptas_addresid).ValueGeneratedNever();
            });

            modelBuilder.Entity<ptas_ptas_permit_ptas_fileattachmentmetadata>(entity =>
            {
                entity.ToTable("ptas_ptas_permit_ptas_fileattachmentmetadata", "dynamics");

                entity.Property(e => e.ptas_ptas_permit_ptas_fileattachmentmetadataid).ValueGeneratedNever();
            });

            modelBuilder.Entity<ptas_ptas_permit_ptas_mediarepository>(entity =>
            {
                entity.ToTable("ptas_ptas_permit_ptas_mediarepository", "dynamics");

                entity.Property(e => e.ptas_ptas_permit_ptas_mediarepositoryid).ValueGeneratedNever();

                entity.Property(e => e.modifiedon)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");
            });

            modelBuilder.Entity<ptas_ptas_residentialappraiserteam_systemuse>(entity =>
            {
                entity.ToTable("ptas_ptas_residentialappraiserteam_systemuse", "dynamics");

                entity.Property(e => e.ptas_ptas_residentialappraiserteam_systemuseid).ValueGeneratedNever();
            });

            modelBuilder.Entity<ptas_ptas_sales_ptas_fileattachmentmetadata>(entity =>
            {
                entity.ToTable("ptas_ptas_sales_ptas_fileattachmentmetadata", "dynamics");

                entity.HasIndex(e => e.modifiedon)
                    .HasName("Idx_ptas_ptas_sales_ptas_fileattachmentmetadata_modifiedon");

                entity.Property(e => e.ptas_ptas_sales_ptas_fileattachmentmetadataid).ValueGeneratedNever();

                entity.Property(e => e.modifiedon)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");
            });

            modelBuilder.Entity<ptas_ptas_salesnote_ptas_fileattachmentmetad>(entity =>
            {
                entity.ToTable("ptas_ptas_salesnote_ptas_fileattachmentmetad", "dynamics");

                entity.Property(e => e.ptas_ptas_salesnote_ptas_fileattachmentmetadid).ValueGeneratedNever();
            });

            modelBuilder.Entity<ptas_ptas_task_ptas_fileattachmentmetadata>(entity =>
            {
                entity.ToTable("ptas_ptas_task_ptas_fileattachmentmetadata", "dynamics");

                entity.Property(e => e.ptas_ptas_task_ptas_fileattachmentmetadataid).ValueGeneratedNever();

                entity.Property(e => e.modifiedon)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");
            });

            modelBuilder.Entity<ptas_ptas_task_ptas_parceldetail>(entity =>
            {
                entity.ToTable("ptas_ptas_task_ptas_parceldetail", "dynamics");

                entity.Property(e => e.ptas_ptas_task_ptas_parceldetailid).ValueGeneratedNever();

                entity.Property(e => e.modifiedon)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");
            });

            modelBuilder.Entity<ptas_ptas_task_ptas_taxrollcorrection>(entity =>
            {
                entity.ToTable("ptas_ptas_task_ptas_taxrollcorrection", "dynamics");

                entity.Property(e => e.ptas_ptas_task_ptas_taxrollcorrectionid).ValueGeneratedNever();
            });

            modelBuilder.Entity<ptas_ptas_taxaccount_ptas_fileattachmentmeta>(entity =>
            {
                entity.ToTable("ptas_ptas_taxaccount_ptas_fileattachmentmeta", "dynamics");

                entity.Property(e => e.ptas_ptas_taxaccount_ptas_fileattachmentmetaid).ValueGeneratedNever();
            });

            modelBuilder.Entity<ptas_ptasconfiguration>(entity =>
            {
                entity.ToTable("ptas_ptasconfiguration", "dynamics");

                entity.HasIndex(e => e.modifiedon)
                    .HasName("Idx_ptas_ptasconfiguration_modifiedon");

                entity.Property(e => e.ptas_ptasconfigurationid).ValueGeneratedNever();

                entity.HasOne(d => d._createdby_valueNavigation)
                    .WithMany(p => p.ptas_ptasconfiguration_createdby_valueNavigation)
                    .HasForeignKey(d => d._createdby_value)
                    .HasConstraintName("FK_systemuser_ptas_ptasconfiguration_createdby");

                entity.HasOne(d => d._createdonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_ptasconfiguration_createdonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._createdonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_ptasconfiguration_createdonbehalfby");

                entity.HasOne(d => d._modifiedby_valueNavigation)
                    .WithMany(p => p.ptas_ptasconfiguration_modifiedby_valueNavigation)
                    .HasForeignKey(d => d._modifiedby_value)
                    .HasConstraintName("FK_systemuser_ptas_ptasconfiguration_modifiedby");

                entity.HasOne(d => d._modifiedonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_ptasconfiguration_modifiedonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._modifiedonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_ptasconfiguration_modifiedonbehalfby");

                entity.HasOne(d => d._ptas_defaultsendfromid_valueNavigation)
                    .WithMany(p => p.ptas_ptasconfiguration_ptas_defaultsendfromid_valueNavigation)
                    .HasForeignKey(d => d._ptas_defaultsendfromid_value)
                    .HasConstraintName("FK_systemuser_ptas_ptasconfiguration_ptas_defaultsendfromid");

                entity.HasOne(d => d._ptas_sendsrexemptsyncemailto_valueNavigation)
                    .WithMany(p => p.ptas_ptasconfiguration_ptas_sendsrexemptsyncemailto_valueNavigation)
                    .HasForeignKey(d => d._ptas_sendsrexemptsyncemailto_value)
                    .HasConstraintName("FK_systemuser_ptas_ptasconfiguration_ptas_sendsrexemptsyncemailto");
            });

            modelBuilder.Entity<ptas_ptassetting>(entity =>
            {
                entity.ToTable("ptas_ptassetting", "dynamics");

                entity.HasIndex(e => e.modifiedon)
                    .HasName("Idx_ptas_ptassetting_modifiedon");

                entity.Property(e => e.ptas_ptassettingid).ValueGeneratedNever();

                entity.HasOne(d => d._createdby_valueNavigation)
                    .WithMany(p => p.ptas_ptassetting_createdby_valueNavigation)
                    .HasForeignKey(d => d._createdby_value)
                    .HasConstraintName("FK_systemuser_ptas_ptassetting_createdby");

                entity.HasOne(d => d._createdonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_ptassetting_createdonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._createdonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_ptassetting_createdonbehalfby");

                entity.HasOne(d => d._modifiedby_valueNavigation)
                    .WithMany(p => p.ptas_ptassetting_modifiedby_valueNavigation)
                    .HasForeignKey(d => d._modifiedby_value)
                    .HasConstraintName("FK_systemuser_ptas_ptassetting_modifiedby");

                entity.HasOne(d => d._modifiedonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_ptassetting_modifiedonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._modifiedonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_ptassetting_modifiedonbehalfby");

                entity.HasOne(d => d._owningteam_valueNavigation)
                    .WithMany(p => p.ptas_ptassetting)
                    .HasForeignKey(d => d._owningteam_value)
                    .HasConstraintName("FK_team_ptas_ptassetting_owningteam");

                entity.HasOne(d => d._owninguser_valueNavigation)
                    .WithMany(p => p.ptas_ptassetting_owninguser_valueNavigation)
                    .HasForeignKey(d => d._owninguser_value)
                    .HasConstraintName("FK_systemuser_ptas_ptassetting_owninguser");
            });

            modelBuilder.Entity<ptas_qstr>(entity =>
            {
                entity.ToTable("ptas_qstr", "dynamics");

                entity.HasIndex(e => e.modifiedon)
                    .HasName("Idx_ptas_qstr_modifiedon");

                entity.Property(e => e.ptas_qstrid).ValueGeneratedNever();

                entity.HasOne(d => d._createdby_valueNavigation)
                    .WithMany(p => p.ptas_qstr_createdby_valueNavigation)
                    .HasForeignKey(d => d._createdby_value)
                    .HasConstraintName("FK_systemuser_ptas_qstr_createdby");

                entity.HasOne(d => d._createdonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_qstr_createdonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._createdonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_qstr_createdonbehalfby");

                entity.HasOne(d => d._modifiedby_valueNavigation)
                    .WithMany(p => p.ptas_qstr_modifiedby_valueNavigation)
                    .HasForeignKey(d => d._modifiedby_value)
                    .HasConstraintName("FK_systemuser_ptas_qstr_modifiedby");

                entity.HasOne(d => d._modifiedonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_qstr_modifiedonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._modifiedonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_qstr_modifiedonbehalfby");
            });

            modelBuilder.Entity<ptas_quickcollect>(entity =>
            {
                entity.ToTable("ptas_quickcollect", "dynamics");

                entity.Property(e => e.ptas_quickcollectid).ValueGeneratedNever();

                entity.Property(e => e.exchangerate).HasColumnType("money");

                entity.Property(e => e.ptas_assessmentvalue).HasColumnType("money");

                entity.Property(e => e.ptas_assessmentvalue_base).HasColumnType("money");

                entity.Property(e => e.ptas_equipment).HasColumnType("money");

                entity.Property(e => e.ptas_equipment_base).HasColumnType("money");

                entity.Property(e => e.ptas_intangibles).HasColumnType("money");

                entity.Property(e => e.ptas_intangibles_base).HasColumnType("money");

                entity.Property(e => e.ptas_leaseholdimprovements).HasColumnType("money");

                entity.Property(e => e.ptas_leaseholdimprovements_base).HasColumnType("money");

                entity.Property(e => e.ptas_other).HasColumnType("money");

                entity.Property(e => e.ptas_other_base).HasColumnType("money");

                entity.Property(e => e.ptas_totalsalesprice).HasColumnType("money");

                entity.Property(e => e.ptas_totalsalesprice_base).HasColumnType("money");

                entity.HasOne(d => d._createdby_valueNavigation)
                    .WithMany(p => p.ptas_quickcollect_createdby_valueNavigation)
                    .HasForeignKey(d => d._createdby_value)
                    .HasConstraintName("FK_systemuser_ptas_quickcollect_createdby");

                entity.HasOne(d => d._createdonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_quickcollect_createdonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._createdonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_quickcollect_createdonbehalfby");

                entity.HasOne(d => d._modifiedby_valueNavigation)
                    .WithMany(p => p.ptas_quickcollect_modifiedby_valueNavigation)
                    .HasForeignKey(d => d._modifiedby_value)
                    .HasConstraintName("FK_systemuser_ptas_quickcollect_modifiedby");

                entity.HasOne(d => d._modifiedonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_quickcollect_modifiedonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._modifiedonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_quickcollect_modifiedonbehalfby");

                entity.HasOne(d => d._owningteam_valueNavigation)
                    .WithMany(p => p.ptas_quickcollect)
                    .HasForeignKey(d => d._owningteam_value)
                    .HasConstraintName("FK_team_ptas_quickcollect_owningteam");

                entity.HasOne(d => d._owninguser_valueNavigation)
                    .WithMany(p => p.ptas_quickcollect_owninguser_valueNavigation)
                    .HasForeignKey(d => d._owninguser_value)
                    .HasConstraintName("FK_systemuser_ptas_quickcollect_owninguser");

                entity.HasOne(d => d._ptas_billofsale_fileattachementmetadataid_valueNavigation)
                    .WithMany(p => p.ptas_quickcollect)
                    .HasForeignKey(d => d._ptas_billofsale_fileattachementmetadataid_value)
                    .HasConstraintName("FK_ptas_fileattachmentmetadata_ptas_quickcollect_ptas_billofsale_fileattachementmetadataid");

                entity.HasOne(d => d._ptas_newinformation_addr_locationstateid_valueNavigation)
                    .WithMany(p => p.ptas_quickcollect_ptas_newinformation_addr_locationstateid_valueNavigation)
                    .HasForeignKey(d => d._ptas_newinformation_addr_locationstateid_value)
                    .HasConstraintName("FK_ptas_stateorprovince_ptas_quickcollect_ptas_newinformation_addr_locationstateid");

                entity.HasOne(d => d._ptas_newinformation_addr_stateid_valueNavigation)
                    .WithMany(p => p.ptas_quickcollect_ptas_newinformation_addr_stateid_valueNavigation)
                    .HasForeignKey(d => d._ptas_newinformation_addr_stateid_value)
                    .HasConstraintName("FK_ptas_stateorprovince_ptas_quickcollect_ptas_newinformation_addr_stateid");

                entity.HasOne(d => d._ptas_personalpropinfo_addr_locationstateid_valueNavigation)
                    .WithMany(p => p.ptas_quickcollect_ptas_personalpropinfo_addr_locationstateid_valueNavigation)
                    .HasForeignKey(d => d._ptas_personalpropinfo_addr_locationstateid_value)
                    .HasConstraintName("FK_ptas_stateorprovince_ptas_quickcollect_ptas_personalpropinfo_addr_locationstateid");

                entity.HasOne(d => d._ptas_personalpropinfo_addr_stateid_valueNavigation)
                    .WithMany(p => p.ptas_quickcollect_ptas_personalpropinfo_addr_stateid_valueNavigation)
                    .HasForeignKey(d => d._ptas_personalpropinfo_addr_stateid_value)
                    .HasConstraintName("FK_ptas_stateorprovince_ptas_quickcollect_ptas_personalpropinfo_addr_stateid");

                entity.HasOne(d => d._ptas_processeduserid_valueNavigation)
                    .WithMany(p => p.ptas_quickcollect_ptas_processeduserid_valueNavigation)
                    .HasForeignKey(d => d._ptas_processeduserid_value)
                    .HasConstraintName("FK_systemuser_ptas_quickcollect_ptas_processeduserid");

                entity.HasOne(d => d._ptas_requestorinfo_addr_stateid_valueNavigation)
                    .WithMany(p => p.ptas_quickcollect_ptas_requestorinfo_addr_stateid_valueNavigation)
                    .HasForeignKey(d => d._ptas_requestorinfo_addr_stateid_value)
                    .HasConstraintName("FK_ptas_stateorprovince_ptas_quickcollect_ptas_requestorinfo_addr_stateid");

                entity.HasOne(d => d._ptas_yearid_valueNavigation)
                    .WithMany(p => p.ptas_quickcollect)
                    .HasForeignKey(d => d._ptas_yearid_value)
                    .HasConstraintName("FK_ptas_year_ptas_quickcollect_ptas_yearid");
            });

            modelBuilder.Entity<ptas_recentparcel>(entity =>
            {
                entity.ToTable("ptas_recentparcel", "dynamics");

                entity.HasIndex(e => e.modifiedon)
                    .HasName("Idx_ptas_recentparcel_modifiedon");

                entity.Property(e => e.ptas_recentparcelid).ValueGeneratedNever();

                entity.HasOne(d => d._createdby_valueNavigation)
                    .WithMany(p => p.ptas_recentparcel_createdby_valueNavigation)
                    .HasForeignKey(d => d._createdby_value)
                    .HasConstraintName("FK_systemuser_ptas_recentparcel_createdby");

                entity.HasOne(d => d._createdonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_recentparcel_createdonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._createdonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_recentparcel_createdonbehalfby");

                entity.HasOne(d => d._modifiedby_valueNavigation)
                    .WithMany(p => p.ptas_recentparcel_modifiedby_valueNavigation)
                    .HasForeignKey(d => d._modifiedby_value)
                    .HasConstraintName("FK_systemuser_ptas_recentparcel_modifiedby");

                entity.HasOne(d => d._modifiedonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_recentparcel_modifiedonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._modifiedonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_recentparcel_modifiedonbehalfby");

                entity.HasOne(d => d._owningteam_valueNavigation)
                    .WithMany(p => p.ptas_recentparcel)
                    .HasForeignKey(d => d._owningteam_value)
                    .HasConstraintName("FK_team_ptas_recentparcel_owningteam");

                entity.HasOne(d => d._owninguser_valueNavigation)
                    .WithMany(p => p.ptas_recentparcel_owninguser_valueNavigation)
                    .HasForeignKey(d => d._owninguser_value)
                    .HasConstraintName("FK_systemuser_ptas_recentparcel_owninguser");

                entity.HasOne(d => d._ptas_parcelid_valueNavigation)
                    .WithMany(p => p.ptas_recentparcel)
                    .HasForeignKey(d => d._ptas_parcelid_value)
                    .HasConstraintName("FK_ptas_parceldetail_ptas_recentparcel_ptas_parcelid");
            });

            modelBuilder.Entity<ptas_residentialappraiserteam>(entity =>
            {
                entity.ToTable("ptas_residentialappraiserteam", "dynamics");

                entity.Property(e => e.ptas_residentialappraiserteamid).ValueGeneratedNever();

                entity.HasOne(d => d._createdby_valueNavigation)
                    .WithMany(p => p.ptas_residentialappraiserteam_createdby_valueNavigation)
                    .HasForeignKey(d => d._createdby_value)
                    .HasConstraintName("FK_systemuser_ptas_residentialappraiserteam_createdby");

                entity.HasOne(d => d._createdonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_residentialappraiserteam_createdonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._createdonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_residentialappraiserteam_createdonbehalfby");

                entity.HasOne(d => d._modifiedby_valueNavigation)
                    .WithMany(p => p.ptas_residentialappraiserteam_modifiedby_valueNavigation)
                    .HasForeignKey(d => d._modifiedby_value)
                    .HasConstraintName("FK_systemuser_ptas_residentialappraiserteam_modifiedby");

                entity.HasOne(d => d._modifiedonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_residentialappraiserteam_modifiedonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._modifiedonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_residentialappraiserteam_modifiedonbehalfby");

                entity.HasOne(d => d._owningteam_valueNavigation)
                    .WithMany(p => p.ptas_residentialappraiserteam)
                    .HasForeignKey(d => d._owningteam_value)
                    .HasConstraintName("FK_team_ptas_residentialappraiserteam_owningteam");

                entity.HasOne(d => d._owninguser_valueNavigation)
                    .WithMany(p => p.ptas_residentialappraiserteam_owninguser_valueNavigation)
                    .HasForeignKey(d => d._owninguser_value)
                    .HasConstraintName("FK_systemuser_ptas_residentialappraiserteam_owninguser");
            });

            modelBuilder.Entity<ptas_responsibility>(entity =>
            {
                entity.ToTable("ptas_responsibility", "dynamics");

                entity.HasIndex(e => e.modifiedon)
                    .HasName("Idx_ptas_responsibility_modifiedon");

                entity.Property(e => e.ptas_responsibilityid).ValueGeneratedNever();

                entity.HasOne(d => d._createdby_valueNavigation)
                    .WithMany(p => p.ptas_responsibility_createdby_valueNavigation)
                    .HasForeignKey(d => d._createdby_value)
                    .HasConstraintName("FK_systemuser_ptas_responsibility_createdby");

                entity.HasOne(d => d._createdonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_responsibility_createdonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._createdonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_responsibility_createdonbehalfby");

                entity.HasOne(d => d._modifiedby_valueNavigation)
                    .WithMany(p => p.ptas_responsibility_modifiedby_valueNavigation)
                    .HasForeignKey(d => d._modifiedby_value)
                    .HasConstraintName("FK_systemuser_ptas_responsibility_modifiedby");

                entity.HasOne(d => d._modifiedonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_responsibility_modifiedonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._modifiedonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_responsibility_modifiedonbehalfby");
            });

            modelBuilder.Entity<ptas_restrictedrent>(entity =>
            {
                entity.ToTable("ptas_restrictedrent", "dynamics");

                entity.Property(e => e.ptas_restrictedrentid).ValueGeneratedNever();

                entity.Property(e => e.exchangerate).HasColumnType("money");

                entity.Property(e => e.ptas_setaside_100pct).HasColumnType("money");

                entity.Property(e => e.ptas_setaside_100pct_base).HasColumnType("money");

                entity.Property(e => e.ptas_setaside_120pct).HasColumnType("money");

                entity.Property(e => e.ptas_setaside_120pct_base).HasColumnType("money");

                entity.Property(e => e.ptas_setaside_20pct).HasColumnType("money");

                entity.Property(e => e.ptas_setaside_20pct_base).HasColumnType("money");

                entity.Property(e => e.ptas_setaside_30pct).HasColumnType("money");

                entity.Property(e => e.ptas_setaside_30pct_base).HasColumnType("money");

                entity.Property(e => e.ptas_setaside_35pct).HasColumnType("money");

                entity.Property(e => e.ptas_setaside_35pct_base).HasColumnType("money");

                entity.Property(e => e.ptas_setaside_40pct).HasColumnType("money");

                entity.Property(e => e.ptas_setaside_40pct_base).HasColumnType("money");

                entity.Property(e => e.ptas_setaside_45pct).HasColumnType("money");

                entity.Property(e => e.ptas_setaside_45pct_base).HasColumnType("money");

                entity.Property(e => e.ptas_setaside_50pct).HasColumnType("money");

                entity.Property(e => e.ptas_setaside_50pct_base).HasColumnType("money");

                entity.Property(e => e.ptas_setaside_60pct).HasColumnType("money");

                entity.Property(e => e.ptas_setaside_60pct_base).HasColumnType("money");

                entity.Property(e => e.ptas_setaside_70pct).HasColumnType("money");

                entity.Property(e => e.ptas_setaside_70pct_base).HasColumnType("money");

                entity.Property(e => e.ptas_setaside_80pct).HasColumnType("money");

                entity.Property(e => e.ptas_setaside_80pct_base).HasColumnType("money");

                entity.HasOne(d => d._createdby_valueNavigation)
                    .WithMany(p => p.ptas_restrictedrent_createdby_valueNavigation)
                    .HasForeignKey(d => d._createdby_value)
                    .HasConstraintName("FK_systemuser_ptas_restrictedrent_createdby");

                entity.HasOne(d => d._createdonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_restrictedrent_createdonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._createdonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_restrictedrent_createdonbehalfby");

                entity.HasOne(d => d._modifiedby_valueNavigation)
                    .WithMany(p => p.ptas_restrictedrent_modifiedby_valueNavigation)
                    .HasForeignKey(d => d._modifiedby_value)
                    .HasConstraintName("FK_systemuser_ptas_restrictedrent_modifiedby");

                entity.HasOne(d => d._modifiedonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_restrictedrent_modifiedonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._modifiedonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_restrictedrent_modifiedonbehalfby");

                entity.HasOne(d => d._owningteam_valueNavigation)
                    .WithMany(p => p.ptas_restrictedrent)
                    .HasForeignKey(d => d._owningteam_value)
                    .HasConstraintName("FK_team_ptas_restrictedrent_owningteam");

                entity.HasOne(d => d._owninguser_valueNavigation)
                    .WithMany(p => p.ptas_restrictedrent_owninguser_valueNavigation)
                    .HasForeignKey(d => d._owninguser_value)
                    .HasConstraintName("FK_systemuser_ptas_restrictedrent_owninguser");

                entity.HasOne(d => d._ptas_assessmentyear_valueNavigation)
                    .WithMany(p => p.ptas_restrictedrent)
                    .HasForeignKey(d => d._ptas_assessmentyear_value)
                    .HasConstraintName("FK_ptas_year_ptas_restrictedrent_ptas_assessmentyear");

                entity.HasOne(d => d._ptas_lowincomehousingprogram_valueNavigation)
                    .WithMany(p => p.ptas_restrictedrent)
                    .HasForeignKey(d => d._ptas_lowincomehousingprogram_value)
                    .HasConstraintName("FK_ptas_housingprogram_ptas_restrictedrent_ptas_lowincomehousingprogram");
            });

            modelBuilder.Entity<ptas_salepriceadjustment>(entity =>
            {
                entity.ToTable("ptas_salepriceadjustment", "dynamics");

                entity.HasIndex(e => e.modifiedon)
                    .HasName("Idx_ptas_salepriceadjustment_modifiedon");

                entity.Property(e => e.ptas_salepriceadjustmentid).ValueGeneratedNever();

                entity.Property(e => e.exchangerate).HasColumnType("money");

                entity.Property(e => e.ptas_price).HasColumnType("money");

                entity.Property(e => e.ptas_price_base).HasColumnType("money");

                entity.HasOne(d => d._createdby_valueNavigation)
                    .WithMany(p => p.ptas_salepriceadjustment_createdby_valueNavigation)
                    .HasForeignKey(d => d._createdby_value)
                    .HasConstraintName("FK_systemuser_ptas_salepriceadjustment_createdby");

                entity.HasOne(d => d._createdonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_salepriceadjustment_createdonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._createdonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_salepriceadjustment_createdonbehalfby");

                entity.HasOne(d => d._modifiedby_valueNavigation)
                    .WithMany(p => p.ptas_salepriceadjustment_modifiedby_valueNavigation)
                    .HasForeignKey(d => d._modifiedby_value)
                    .HasConstraintName("FK_systemuser_ptas_salepriceadjustment_modifiedby");

                entity.HasOne(d => d._modifiedonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_salepriceadjustment_modifiedonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._modifiedonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_salepriceadjustment_modifiedonbehalfby");

                entity.HasOne(d => d._owningteam_valueNavigation)
                    .WithMany(p => p.ptas_salepriceadjustment)
                    .HasForeignKey(d => d._owningteam_value)
                    .HasConstraintName("FK_team_ptas_salepriceadjustment_owningteam");

                entity.HasOne(d => d._owninguser_valueNavigation)
                    .WithMany(p => p.ptas_salepriceadjustment_owninguser_valueNavigation)
                    .HasForeignKey(d => d._owninguser_value)
                    .HasConstraintName("FK_systemuser_ptas_salepriceadjustment_owninguser");

                entity.HasOne(d => d._ptas_saleid_valueNavigation)
                    .WithMany(p => p.ptas_salepriceadjustment)
                    .HasForeignKey(d => d._ptas_saleid_value)
                    .HasConstraintName("FK_ptas_sales_ptas_salepriceadjustment_ptas_saleid");
            });

            modelBuilder.Entity<ptas_sales>(entity =>
            {
                entity.ToTable("ptas_sales", "dynamics");

                entity.HasIndex(e => e.modifiedon)
                    .HasName("Idx_ptas_sales_modifiedon");

                entity.Property(e => e.ptas_salesid).ValueGeneratedNever();

                entity.Property(e => e.exchangerate).HasColumnType("money");

                entity.Property(e => e.ptas_adjustedsaleprice).HasColumnType("money");

                entity.Property(e => e.ptas_adjustedsaleprice_base).HasColumnType("money");

                entity.Property(e => e.ptas_agglandacres).HasColumnType("money");

                entity.Property(e => e.ptas_appraiseradjustment).HasColumnType("money");

                entity.Property(e => e.ptas_appraiseradjustment_base).HasColumnType("money");

                entity.Property(e => e.ptas_saleprice).HasColumnType("money");

                entity.Property(e => e.ptas_saleprice_base).HasColumnType("money");

                entity.Property(e => e.ptas_sqftlotgra).HasColumnType("money");

                entity.Property(e => e.ptas_sqftlotnra).HasColumnType("money");

                entity.Property(e => e.ptas_sqftlotunit).HasColumnType("money");

                entity.Property(e => e.ptas_taxablesellingprice).HasColumnType("money");

                entity.Property(e => e.ptas_taxablesellingprice_base).HasColumnType("money");

                entity.Property(e => e.ptas_vspgra).HasColumnType("money");

                entity.Property(e => e.ptas_vspgra_base).HasColumnType("money");

                entity.Property(e => e.ptas_vspnra).HasColumnType("money");

                entity.Property(e => e.ptas_vspnra_base).HasColumnType("money");

                entity.Property(e => e.ptas_vspsqftlot).HasColumnType("money");

                entity.Property(e => e.ptas_vspsqftlot_base).HasColumnType("money");

                entity.Property(e => e.ptas_vspunit).HasColumnType("money");

                entity.Property(e => e.ptas_vspunit_base).HasColumnType("money");
            });

            modelBuilder.Entity<ptas_sales_parceldetail_parcelsinsale>(entity =>
            {
                entity.ToTable("ptas_sales_parceldetail_parcelsinsale", "dynamics");

                entity.HasIndex(e => e.modifiedon)
                    .HasName("Idx_ptas_sales_parceldetail_parcelsinsale_modifiedon");

                entity.Property(e => e.ptas_sales_parceldetail_parcelsinsaleid).ValueGeneratedNever();

                entity.Property(e => e.modifiedon)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");
            });

            modelBuilder.Entity<ptas_sales_ptas_mediarepository>(entity =>
            {
                entity.ToTable("ptas_sales_ptas_mediarepository", "dynamics");

                entity.Property(e => e.ptas_sales_ptas_mediarepositoryid).ValueGeneratedNever();

                entity.Property(e => e.modifiedon)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");
            });

            modelBuilder.Entity<ptas_sales_ptas_saleswarningcode>(entity =>
            {
                entity.ToTable("ptas_sales_ptas_saleswarningcode", "dynamics");

                entity.HasIndex(e => e.modifiedon)
                    .HasName("Idx_ptas_sales_ptas_saleswarningcode_modifiedon");

                entity.Property(e => e.ptas_sales_ptas_saleswarningcodeid).ValueGeneratedNever();

                entity.Property(e => e.modifiedon)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");
            });

            modelBuilder.Entity<ptas_salesaggregate>(entity =>
            {
                entity.ToTable("ptas_salesaggregate", "dynamics");

                entity.Property(e => e.ptas_salesaggregateid).ValueGeneratedNever();

                entity.Property(e => e.ptas_acres).HasColumnType("money");

                entity.Property(e => e.ptas_percentview).HasColumnType("money");

                entity.Property(e => e.ptas_sqftlotgrossbuildingarea).HasColumnType("money");

                entity.Property(e => e.ptas_sqftlotnetbuildingarea).HasColumnType("money");

                entity.Property(e => e.ptas_sqftlotunit).HasColumnType("money");

                entity.Property(e => e.ptas_verifiedsalepricesqftlot).HasColumnType("money");

                entity.Property(e => e.ptas_vsppergra).HasColumnType("money");

                entity.Property(e => e.ptas_vsppernra).HasColumnType("money");

                entity.Property(e => e.ptas_vspperunit).HasColumnType("money");

                entity.HasOne(d => d._createdby_valueNavigation)
                    .WithMany(p => p.ptas_salesaggregate_createdby_valueNavigation)
                    .HasForeignKey(d => d._createdby_value)
                    .HasConstraintName("FK_systemuser_ptas_salesaggregate_createdby");

                entity.HasOne(d => d._createdonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_salesaggregate_createdonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._createdonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_salesaggregate_createdonbehalfby");

                entity.HasOne(d => d._modifiedby_valueNavigation)
                    .WithMany(p => p.ptas_salesaggregate_modifiedby_valueNavigation)
                    .HasForeignKey(d => d._modifiedby_value)
                    .HasConstraintName("FK_systemuser_ptas_salesaggregate_modifiedby");

                entity.HasOne(d => d._modifiedonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_salesaggregate_modifiedonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._modifiedonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_salesaggregate_modifiedonbehalfby");

                entity.HasOne(d => d._owningteam_valueNavigation)
                    .WithMany(p => p.ptas_salesaggregate)
                    .HasForeignKey(d => d._owningteam_value)
                    .HasConstraintName("FK_team_ptas_salesaggregate_owningteam");

                entity.HasOne(d => d._owninguser_valueNavigation)
                    .WithMany(p => p.ptas_salesaggregate_owninguser_valueNavigation)
                    .HasForeignKey(d => d._owninguser_value)
                    .HasConstraintName("FK_systemuser_ptas_salesaggregate_owninguser");

                entity.HasOne(d => d._ptas_buildingsectionuseid_valueNavigation)
                    .WithMany(p => p.ptas_salesaggregate)
                    .HasForeignKey(d => d._ptas_buildingsectionuseid_value)
                    .HasConstraintName("FK_ptas_buildingsectionuse_ptas_salesaggregate_ptas_buildingsectionuseid");

                entity.HasOne(d => d._ptas_districtid_valueNavigation)
                    .WithMany(p => p.ptas_salesaggregate)
                    .HasForeignKey(d => d._ptas_districtid_value)
                    .HasConstraintName("FK_ptas_district_ptas_salesaggregate_ptas_districtid");

                entity.HasOne(d => d._ptas_geoareaid_valueNavigation)
                    .WithMany(p => p.ptas_salesaggregate)
                    .HasForeignKey(d => d._ptas_geoareaid_value)
                    .HasConstraintName("FK_ptas_geoarea_ptas_salesaggregate_ptas_geoareaid");

                entity.HasOne(d => d._ptas_geoneighborhoodid_valueNavigation)
                    .WithMany(p => p.ptas_salesaggregate)
                    .HasForeignKey(d => d._ptas_geoneighborhoodid_value)
                    .HasConstraintName("FK_ptas_geoneighborhood_ptas_salesaggregate_ptas_geoneighborhoodid");

                entity.HasOne(d => d._ptas_presentuseid_valueNavigation)
                    .WithMany(p => p.ptas_salesaggregate)
                    .HasForeignKey(d => d._ptas_presentuseid_value)
                    .HasConstraintName("FK_ptas_landuse_ptas_salesaggregate_ptas_presentuseid");

                entity.HasOne(d => d._ptas_primarybuildingid_valueNavigation)
                    .WithMany(p => p.ptas_salesaggregate)
                    .HasForeignKey(d => d._ptas_primarybuildingid_value)
                    .HasConstraintName("FK_ptas_buildingdetail_ptas_salesaggregate_ptas_primarybuildingid");

                entity.HasOne(d => d._ptas_propertytypeid_valueNavigation)
                    .WithMany(p => p.ptas_salesaggregate)
                    .HasForeignKey(d => d._ptas_propertytypeid_value)
                    .HasConstraintName("FK_ptas_propertytype_ptas_salesaggregate_ptas_propertytypeid");

                entity.HasOne(d => d._ptas_qstrid_valueNavigation)
                    .WithMany(p => p.ptas_salesaggregate)
                    .HasForeignKey(d => d._ptas_qstrid_value)
                    .HasConstraintName("FK_ptas_qstr_ptas_salesaggregate_ptas_qstrid");

                entity.HasOne(d => d._ptas_responsibilityid_valueNavigation)
                    .WithMany(p => p.ptas_salesaggregate)
                    .HasForeignKey(d => d._ptas_responsibilityid_value)
                    .HasConstraintName("FK_ptas_responsibility_ptas_salesaggregate_ptas_responsibilityid");

                entity.HasOne(d => d._ptas_saleid_valueNavigation)
                    .WithMany(p => p.ptas_salesaggregate)
                    .HasForeignKey(d => d._ptas_saleid_value)
                    .HasConstraintName("FK_ptas_sales_ptas_salesaggregate_ptas_saleid");

                entity.HasOne(d => d._ptas_specialtyareaid_valueNavigation)
                    .WithMany(p => p.ptas_salesaggregate)
                    .HasForeignKey(d => d._ptas_specialtyareaid_value)
                    .HasConstraintName("FK_ptas_specialtyarea_ptas_salesaggregate_ptas_specialtyareaid");

                entity.HasOne(d => d._ptas_specialtyneighborhoodid_valueNavigation)
                    .WithMany(p => p.ptas_salesaggregate)
                    .HasForeignKey(d => d._ptas_specialtyneighborhoodid_value)
                    .HasConstraintName("FK_ptas_specialtyneighborhood_ptas_salesaggregate_ptas_specialtyneighborhoodid");

                entity.HasOne(d => d._ptas_yearbuiltid_valueNavigation)
                    .WithMany(p => p.ptas_salesaggregate_ptas_yearbuiltid_valueNavigation)
                    .HasForeignKey(d => d._ptas_yearbuiltid_value)
                    .HasConstraintName("FK_ptas_year_ptas_salesaggregate_ptas_yearbuiltid");

                entity.HasOne(d => d._ptas_yeareffectiveid_valueNavigation)
                    .WithMany(p => p.ptas_salesaggregate_ptas_yeareffectiveid_valueNavigation)
                    .HasForeignKey(d => d._ptas_yeareffectiveid_value)
                    .HasConstraintName("FK_ptas_year_ptas_salesaggregate_ptas_yeareffectiveid");

                entity.HasOne(d => d._ptas_zoningid_valueNavigation)
                    .WithMany(p => p.ptas_salesaggregate)
                    .HasForeignKey(d => d._ptas_zoningid_value)
                    .HasConstraintName("FK_ptas_zoning_ptas_salesaggregate_ptas_zoningid");
            });

            modelBuilder.Entity<ptas_salesnote>(entity =>
            {
                entity.ToTable("ptas_salesnote", "dynamics");

                entity.HasIndex(e => e.modifiedon)
                    .HasName("Idx_ptas_salesnote_modifiedon");

                entity.Property(e => e.ptas_salesnoteid).ValueGeneratedNever();

                entity.HasOne(d => d._createdby_valueNavigation)
                    .WithMany(p => p.ptas_salesnote_createdby_valueNavigation)
                    .HasForeignKey(d => d._createdby_value)
                    .HasConstraintName("FK_systemuser_ptas_salesnote_createdby");

                entity.HasOne(d => d._createdonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_salesnote_createdonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._createdonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_salesnote_createdonbehalfby");

                entity.HasOne(d => d._modifiedby_valueNavigation)
                    .WithMany(p => p.ptas_salesnote_modifiedby_valueNavigation)
                    .HasForeignKey(d => d._modifiedby_value)
                    .HasConstraintName("FK_systemuser_ptas_salesnote_modifiedby");

                entity.HasOne(d => d._modifiedonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_salesnote_modifiedonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._modifiedonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_salesnote_modifiedonbehalfby");

                entity.HasOne(d => d._owningteam_valueNavigation)
                    .WithMany(p => p.ptas_salesnote)
                    .HasForeignKey(d => d._owningteam_value)
                    .HasConstraintName("FK_team_ptas_salesnote_owningteam");

                entity.HasOne(d => d._owninguser_valueNavigation)
                    .WithMany(p => p.ptas_salesnote_owninguser_valueNavigation)
                    .HasForeignKey(d => d._owninguser_value)
                    .HasConstraintName("FK_systemuser_ptas_salesnote_owninguser");

                entity.HasOne(d => d._ptas_saleid_valueNavigation)
                    .WithMany(p => p.ptas_salesnote)
                    .HasForeignKey(d => d._ptas_saleid_value)
                    .HasConstraintName("FK_ptas_sales_ptas_salesnote_ptas_saleid");
            });

            modelBuilder.Entity<ptas_salesnote_ptas_mediarepository>(entity =>
            {
                entity.ToTable("ptas_salesnote_ptas_mediarepository", "dynamics");

                entity.Property(e => e.ptas_salesnote_ptas_mediarepositoryid).ValueGeneratedNever();
            });

            modelBuilder.Entity<ptas_saleswarningcode>(entity =>
            {
                entity.ToTable("ptas_saleswarningcode", "dynamics");

                entity.HasIndex(e => e.modifiedon)
                    .HasName("Idx_ptas_saleswarningcode_modifiedon");

                entity.Property(e => e.ptas_saleswarningcodeid).ValueGeneratedNever();

                entity.HasOne(d => d._createdby_valueNavigation)
                    .WithMany(p => p.ptas_saleswarningcode_createdby_valueNavigation)
                    .HasForeignKey(d => d._createdby_value)
                    .HasConstraintName("FK_systemuser_ptas_saleswarningcode_createdby");

                entity.HasOne(d => d._createdonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_saleswarningcode_createdonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._createdonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_saleswarningcode_createdonbehalfby");

                entity.HasOne(d => d._modifiedby_valueNavigation)
                    .WithMany(p => p.ptas_saleswarningcode_modifiedby_valueNavigation)
                    .HasForeignKey(d => d._modifiedby_value)
                    .HasConstraintName("FK_systemuser_ptas_saleswarningcode_modifiedby");

                entity.HasOne(d => d._modifiedonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_saleswarningcode_modifiedonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._modifiedonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_saleswarningcode_modifiedonbehalfby");
            });

            modelBuilder.Entity<ptas_scheduledworkflow>(entity =>
            {
                entity.ToTable("ptas_scheduledworkflow", "dynamics");

                entity.Property(e => e.ptas_scheduledworkflowid).ValueGeneratedNever();

                entity.HasOne(d => d._createdby_valueNavigation)
                    .WithMany(p => p.ptas_scheduledworkflow_createdby_valueNavigation)
                    .HasForeignKey(d => d._createdby_value)
                    .HasConstraintName("FK_systemuser_ptas_scheduledworkflow_createdby");

                entity.HasOne(d => d._createdonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_scheduledworkflow_createdonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._createdonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_scheduledworkflow_createdonbehalfby");

                entity.HasOne(d => d._modifiedby_valueNavigation)
                    .WithMany(p => p.ptas_scheduledworkflow_modifiedby_valueNavigation)
                    .HasForeignKey(d => d._modifiedby_value)
                    .HasConstraintName("FK_systemuser_ptas_scheduledworkflow_modifiedby");

                entity.HasOne(d => d._modifiedonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_scheduledworkflow_modifiedonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._modifiedonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_scheduledworkflow_modifiedonbehalfby");

                entity.HasOne(d => d._owningteam_valueNavigation)
                    .WithMany(p => p.ptas_scheduledworkflow)
                    .HasForeignKey(d => d._owningteam_value)
                    .HasConstraintName("FK_team_ptas_scheduledworkflow_owningteam");

                entity.HasOne(d => d._owninguser_valueNavigation)
                    .WithMany(p => p.ptas_scheduledworkflow_owninguser_valueNavigation)
                    .HasForeignKey(d => d._owninguser_value)
                    .HasConstraintName("FK_systemuser_ptas_scheduledworkflow_owninguser");
            });

            modelBuilder.Entity<ptas_sectionusesqft>(entity =>
            {
                entity.ToTable("ptas_sectionusesqft", "dynamics");

                entity.HasIndex(e => e.modifiedon)
                    .HasName("Idx_ptas_sectionusesqft_modifiedon");

                entity.Property(e => e.ptas_sectionusesqftid).ValueGeneratedNever();

                entity.HasOne(d => d._createdby_valueNavigation)
                    .WithMany(p => p.ptas_sectionusesqft_createdby_valueNavigation)
                    .HasForeignKey(d => d._createdby_value)
                    .HasConstraintName("FK_systemuser_ptas_sectionusesqft_createdby");

                entity.HasOne(d => d._createdonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_sectionusesqft_createdonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._createdonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_sectionusesqft_createdonbehalfby");

                entity.HasOne(d => d._modifiedby_valueNavigation)
                    .WithMany(p => p.ptas_sectionusesqft_modifiedby_valueNavigation)
                    .HasForeignKey(d => d._modifiedby_value)
                    .HasConstraintName("FK_systemuser_ptas_sectionusesqft_modifiedby");

                entity.HasOne(d => d._modifiedonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_sectionusesqft_modifiedonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._modifiedonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_sectionusesqft_modifiedonbehalfby");

                entity.HasOne(d => d._owningteam_valueNavigation)
                    .WithMany(p => p.ptas_sectionusesqft)
                    .HasForeignKey(d => d._owningteam_value)
                    .HasConstraintName("FK_team_ptas_sectionusesqft_owningteam");

                entity.HasOne(d => d._owninguser_valueNavigation)
                    .WithMany(p => p.ptas_sectionusesqft_owninguser_valueNavigation)
                    .HasForeignKey(d => d._owninguser_value)
                    .HasConstraintName("FK_systemuser_ptas_sectionusesqft_owninguser");

                entity.HasOne(d => d._ptas_mastersectionusesqftid_valueNavigation)
                    .WithMany(p => p.Inverse_ptas_mastersectionusesqftid_valueNavigation)
                    .HasForeignKey(d => d._ptas_mastersectionusesqftid_value)
                    .HasConstraintName("FK_ptas_sectionusesqft_ptas_sectionusesqft_ptas_mastersectionusesqftid");

                entity.HasOne(d => d._ptas_projectid_valueNavigation)
                    .WithMany(p => p.ptas_sectionusesqft)
                    .HasForeignKey(d => d._ptas_projectid_value)
                    .HasConstraintName("FK_ptas_condocomplex_ptas_sectionusesqft_ptas_projectid");

                entity.HasOne(d => d._ptas_sectionuse_valueNavigation)
                    .WithMany(p => p.ptas_sectionusesqft)
                    .HasForeignKey(d => d._ptas_sectionuse_value)
                    .HasConstraintName("FK_ptas_buildingsectionuse_ptas_sectionusesqft_ptas_sectionuse");
            });

            modelBuilder.Entity<ptas_sectionusesqft_snapshot>(entity =>
            {
                entity.HasKey(e => e.ptas_sectionusesqftid);

                entity.ToTable("ptas_sectionusesqft_snapshot", "dynamics");

                entity.Property(e => e.ptas_sectionusesqftid).ValueGeneratedNever();
            });

            modelBuilder.Entity<ptas_sketch>(entity =>
            {
                entity.ToTable("ptas_sketch", "dynamics");

                entity.HasIndex(e => e.modifiedon)
                    .HasName("Idx_ptas_sketch_modifiedon");

                entity.Property(e => e.ptas_sketchid).ValueGeneratedNever();

                entity.HasOne(d => d._createdby_valueNavigation)
                    .WithMany(p => p.ptas_sketch_createdby_valueNavigation)
                    .HasForeignKey(d => d._createdby_value)
                    .HasConstraintName("FK_systemuser_ptas_sketch_createdby");

                entity.HasOne(d => d._createdonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_sketch_createdonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._createdonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_sketch_createdonbehalfby");

                entity.HasOne(d => d._modifiedby_valueNavigation)
                    .WithMany(p => p.ptas_sketch_modifiedby_valueNavigation)
                    .HasForeignKey(d => d._modifiedby_value)
                    .HasConstraintName("FK_systemuser_ptas_sketch_modifiedby");

                entity.HasOne(d => d._modifiedonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_sketch_modifiedonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._modifiedonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_sketch_modifiedonbehalfby");

                entity.HasOne(d => d._owningteam_valueNavigation)
                    .WithMany(p => p.ptas_sketch)
                    .HasForeignKey(d => d._owningteam_value)
                    .HasConstraintName("FK_team_ptas_sketch_owningteam");

                entity.HasOne(d => d._owninguser_valueNavigation)
                    .WithMany(p => p.ptas_sketch_owninguser_valueNavigation)
                    .HasForeignKey(d => d._owninguser_value)
                    .HasConstraintName("FK_systemuser_ptas_sketch_owninguser");

                entity.HasOne(d => d._ptas_accessoryid_valueNavigation)
                    .WithMany(p => p.ptas_sketch)
                    .HasForeignKey(d => d._ptas_accessoryid_value)
                    .HasConstraintName("FK_ptas_accessorydetail_ptas_sketch_ptas_accessoryid");

                entity.HasOne(d => d._ptas_buildingid_valueNavigation)
                    .WithMany(p => p.ptas_sketch)
                    .HasForeignKey(d => d._ptas_buildingid_value)
                    .HasConstraintName("FK_ptas_buildingdetail_ptas_sketch_ptas_buildingid");

                entity.HasOne(d => d._ptas_drawauthorid_valueNavigation)
                    .WithMany(p => p.ptas_sketch_ptas_drawauthorid_valueNavigation)
                    .HasForeignKey(d => d._ptas_drawauthorid_value)
                    .HasConstraintName("FK_systemuser_ptas_sketch_ptas_drawauthorid");

                entity.HasOne(d => d._ptas_lockedbyid_valueNavigation)
                    .WithMany(p => p.ptas_sketch_ptas_lockedbyid_valueNavigation)
                    .HasForeignKey(d => d._ptas_lockedbyid_value)
                    .HasConstraintName("FK_systemuser_ptas_sketch_ptas_lockedbyid");

                entity.HasOne(d => d._ptas_parcelid_valueNavigation)
                    .WithMany(p => p.ptas_sketch)
                    .HasForeignKey(d => d._ptas_parcelid_value)
                    .HasConstraintName("FK_ptas_parceldetail_ptas_sketch_ptas_parcelid");

                entity.HasOne(d => d._ptas_templateid_valueNavigation)
                    .WithMany(p => p.Inverse_ptas_templateid_valueNavigation)
                    .HasForeignKey(d => d._ptas_templateid_value)
                    .HasConstraintName("FK_ptas_sketch_ptas_sketch_ptas_templateid");

                entity.HasOne(d => d._ptas_unitid_valueNavigation)
                    .WithMany(p => p.ptas_sketch)
                    .HasForeignKey(d => d._ptas_unitid_value)
                    .HasConstraintName("FK_ptas_condounit_ptas_sketch_ptas_unitid");
            });

            modelBuilder.Entity<ptas_specialtyarea>(entity =>
            {
                entity.ToTable("ptas_specialtyarea", "dynamics");

                entity.HasIndex(e => e.modifiedon)
                    .HasName("Idx_ptas_specialtyarea_modifiedon");

                entity.Property(e => e.ptas_specialtyareaid).ValueGeneratedNever();

                entity.HasOne(d => d._createdby_valueNavigation)
                    .WithMany(p => p.ptas_specialtyarea_createdby_valueNavigation)
                    .HasForeignKey(d => d._createdby_value)
                    .HasConstraintName("FK_systemuser_ptas_specialtyarea_createdby");

                entity.HasOne(d => d._createdonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_specialtyarea_createdonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._createdonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_specialtyarea_createdonbehalfby");

                entity.HasOne(d => d._modifiedby_valueNavigation)
                    .WithMany(p => p.ptas_specialtyarea_modifiedby_valueNavigation)
                    .HasForeignKey(d => d._modifiedby_value)
                    .HasConstraintName("FK_systemuser_ptas_specialtyarea_modifiedby");

                entity.HasOne(d => d._modifiedonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_specialtyarea_modifiedonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._modifiedonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_specialtyarea_modifiedonbehalfby");

                entity.HasOne(d => d._ptas_seniorappraiserid_valueNavigation)
                    .WithMany(p => p.ptas_specialtyarea_ptas_seniorappraiserid_valueNavigation)
                    .HasForeignKey(d => d._ptas_seniorappraiserid_value)
                    .HasConstraintName("FK_systemuser_ptas_specialtyarea_ptas_seniorappraiserid");
            });

            modelBuilder.Entity<ptas_specialtyneighborhood>(entity =>
            {
                entity.ToTable("ptas_specialtyneighborhood", "dynamics");

                entity.HasIndex(e => e.modifiedon)
                    .HasName("Idx_ptas_specialtyneighborhood_modifiedon");

                entity.Property(e => e.ptas_specialtyneighborhoodid).ValueGeneratedNever();

                entity.HasOne(d => d._createdby_valueNavigation)
                    .WithMany(p => p.ptas_specialtyneighborhood_createdby_valueNavigation)
                    .HasForeignKey(d => d._createdby_value)
                    .HasConstraintName("FK_systemuser_ptas_specialtyneighborhood_createdby");

                entity.HasOne(d => d._createdonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_specialtyneighborhood_createdonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._createdonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_specialtyneighborhood_createdonbehalfby");

                entity.HasOne(d => d._modifiedby_valueNavigation)
                    .WithMany(p => p.ptas_specialtyneighborhood_modifiedby_valueNavigation)
                    .HasForeignKey(d => d._modifiedby_value)
                    .HasConstraintName("FK_systemuser_ptas_specialtyneighborhood_modifiedby");

                entity.HasOne(d => d._modifiedonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_specialtyneighborhood_modifiedonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._modifiedonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_specialtyneighborhood_modifiedonbehalfby");

                entity.HasOne(d => d._ptas_appraiserid_valueNavigation)
                    .WithMany(p => p.ptas_specialtyneighborhood_ptas_appraiserid_valueNavigation)
                    .HasForeignKey(d => d._ptas_appraiserid_value)
                    .HasConstraintName("FK_systemuser_ptas_specialtyneighborhood_ptas_appraiserid");

                entity.HasOne(d => d._ptas_specialtyareaid_valueNavigation)
                    .WithMany(p => p.ptas_specialtyneighborhood)
                    .HasForeignKey(d => d._ptas_specialtyareaid_value)
                    .HasConstraintName("FK_ptas_specialtyarea_ptas_specialtyneighborhood_ptas_specialtyareaid");

                entity.HasOne(d => d._ptas_supergroupid_valueNavigation)
                    .WithMany(p => p.ptas_specialtyneighborhood)
                    .HasForeignKey(d => d._ptas_supergroupid_value)
                    .HasConstraintName("FK_ptas_supergroup_ptas_specialtyneighborhood_ptas_supergroupid");
            });

            modelBuilder.Entity<ptas_stateorprovince>(entity =>
            {
                entity.ToTable("ptas_stateorprovince", "dynamics");

                entity.HasIndex(e => e.modifiedon)
                    .HasName("Idx_ptas_stateorprovince_modifiedon");

                entity.Property(e => e.ptas_stateorprovinceid).ValueGeneratedNever();

                entity.HasOne(d => d._createdby_valueNavigation)
                    .WithMany(p => p.ptas_stateorprovince_createdby_valueNavigation)
                    .HasForeignKey(d => d._createdby_value)
                    .HasConstraintName("FK_systemuser_ptas_stateorprovince_createdby");

                entity.HasOne(d => d._createdonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_stateorprovince_createdonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._createdonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_stateorprovince_createdonbehalfby");

                entity.HasOne(d => d._modifiedby_valueNavigation)
                    .WithMany(p => p.ptas_stateorprovince_modifiedby_valueNavigation)
                    .HasForeignKey(d => d._modifiedby_value)
                    .HasConstraintName("FK_systemuser_ptas_stateorprovince_modifiedby");

                entity.HasOne(d => d._modifiedonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_stateorprovince_modifiedonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._modifiedonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_stateorprovince_modifiedonbehalfby");

                entity.HasOne(d => d._ptas_countryid_valueNavigation)
                    .WithMany(p => p.ptas_stateorprovince)
                    .HasForeignKey(d => d._ptas_countryid_value)
                    .HasConstraintName("FK_ptas_country_ptas_stateorprovince_ptas_countryid");
            });

            modelBuilder.Entity<ptas_streetname>(entity =>
            {
                entity.ToTable("ptas_streetname", "dynamics");

                entity.HasIndex(e => e.modifiedon)
                    .HasName("Idx_ptas_streetname_modifiedon");

                entity.Property(e => e.ptas_streetnameid).ValueGeneratedNever();

                entity.HasOne(d => d._createdby_valueNavigation)
                    .WithMany(p => p.ptas_streetname_createdby_valueNavigation)
                    .HasForeignKey(d => d._createdby_value)
                    .HasConstraintName("FK_systemuser_ptas_streetname_createdby");

                entity.HasOne(d => d._createdonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_streetname_createdonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._createdonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_streetname_createdonbehalfby");

                entity.HasOne(d => d._modifiedby_valueNavigation)
                    .WithMany(p => p.ptas_streetname_modifiedby_valueNavigation)
                    .HasForeignKey(d => d._modifiedby_value)
                    .HasConstraintName("FK_systemuser_ptas_streetname_modifiedby");

                entity.HasOne(d => d._modifiedonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_streetname_modifiedonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._modifiedonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_streetname_modifiedonbehalfby");
            });

            modelBuilder.Entity<ptas_streettype>(entity =>
            {
                entity.ToTable("ptas_streettype", "dynamics");

                entity.HasIndex(e => e.modifiedon)
                    .HasName("Idx_ptas_streettype_modifiedon");

                entity.Property(e => e.ptas_streettypeid).ValueGeneratedNever();

                entity.HasOne(d => d._createdby_valueNavigation)
                    .WithMany(p => p.ptas_streettype_createdby_valueNavigation)
                    .HasForeignKey(d => d._createdby_value)
                    .HasConstraintName("FK_systemuser_ptas_streettype_createdby");

                entity.HasOne(d => d._createdonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_streettype_createdonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._createdonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_streettype_createdonbehalfby");

                entity.HasOne(d => d._modifiedby_valueNavigation)
                    .WithMany(p => p.ptas_streettype_modifiedby_valueNavigation)
                    .HasForeignKey(d => d._modifiedby_value)
                    .HasConstraintName("FK_systemuser_ptas_streettype_modifiedby");

                entity.HasOne(d => d._modifiedonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_streettype_modifiedonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._modifiedonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_streettype_modifiedonbehalfby");
            });

            modelBuilder.Entity<ptas_subarea>(entity =>
            {
                entity.ToTable("ptas_subarea", "dynamics");

                entity.HasIndex(e => e.modifiedon)
                    .HasName("Idx_ptas_subarea_modifiedon");

                entity.Property(e => e.ptas_subareaid).ValueGeneratedNever();

                entity.HasOne(d => d._createdby_valueNavigation)
                    .WithMany(p => p.ptas_subarea_createdby_valueNavigation)
                    .HasForeignKey(d => d._createdby_value)
                    .HasConstraintName("FK_systemuser_ptas_subarea_createdby");

                entity.HasOne(d => d._createdonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_subarea_createdonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._createdonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_subarea_createdonbehalfby");

                entity.HasOne(d => d._modifiedby_valueNavigation)
                    .WithMany(p => p.ptas_subarea_modifiedby_valueNavigation)
                    .HasForeignKey(d => d._modifiedby_value)
                    .HasConstraintName("FK_systemuser_ptas_subarea_modifiedby");

                entity.HasOne(d => d._modifiedonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_subarea_modifiedonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._modifiedonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_subarea_modifiedonbehalfby");

                entity.HasOne(d => d._ptas_areaid_valueNavigation)
                    .WithMany(p => p.ptas_subarea)
                    .HasForeignKey(d => d._ptas_areaid_value)
                    .HasConstraintName("FK_ptas_area_ptas_subarea_ptas_areaid");
            });

            modelBuilder.Entity<ptas_submarket>(entity =>
            {
                entity.ToTable("ptas_submarket", "dynamics");

                entity.HasIndex(e => e.modifiedon)
                    .HasName("Idx_ptas_submarket_modifiedon");

                entity.Property(e => e.ptas_submarketid).ValueGeneratedNever();

                entity.HasOne(d => d._createdby_valueNavigation)
                    .WithMany(p => p.ptas_submarket_createdby_valueNavigation)
                    .HasForeignKey(d => d._createdby_value)
                    .HasConstraintName("FK_systemuser_ptas_submarket_createdby");

                entity.HasOne(d => d._createdonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_submarket_createdonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._createdonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_submarket_createdonbehalfby");

                entity.HasOne(d => d._modifiedby_valueNavigation)
                    .WithMany(p => p.ptas_submarket_modifiedby_valueNavigation)
                    .HasForeignKey(d => d._modifiedby_value)
                    .HasConstraintName("FK_systemuser_ptas_submarket_modifiedby");

                entity.HasOne(d => d._modifiedonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_submarket_modifiedonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._modifiedonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_submarket_modifiedonbehalfby");
            });

            modelBuilder.Entity<ptas_supergroup>(entity =>
            {
                entity.ToTable("ptas_supergroup", "dynamics");

                entity.HasIndex(e => e.modifiedon)
                    .HasName("Idx_ptas_supergroup_modifiedon");

                entity.Property(e => e.ptas_supergroupid).ValueGeneratedNever();

                entity.HasOne(d => d._createdby_valueNavigation)
                    .WithMany(p => p.ptas_supergroup_createdby_valueNavigation)
                    .HasForeignKey(d => d._createdby_value)
                    .HasConstraintName("FK_systemuser_ptas_supergroup_createdby");

                entity.HasOne(d => d._createdonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_supergroup_createdonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._createdonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_supergroup_createdonbehalfby");

                entity.HasOne(d => d._modifiedby_valueNavigation)
                    .WithMany(p => p.ptas_supergroup_modifiedby_valueNavigation)
                    .HasForeignKey(d => d._modifiedby_value)
                    .HasConstraintName("FK_systemuser_ptas_supergroup_modifiedby");

                entity.HasOne(d => d._modifiedonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_supergroup_modifiedonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._modifiedonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_supergroup_modifiedonbehalfby");
            });

            modelBuilder.Entity<ptas_task>(entity =>
            {
                entity.ToTable("ptas_task", "dynamics");

                entity.Property(e => e.ptas_taskid).ValueGeneratedNever();

                entity.Property(e => e.exchangerate).HasColumnType("money");

                entity.Property(e => e.ptas_appdeterredinval_assessedval_imps).HasColumnType("money");

                entity.Property(e => e.ptas_appdeterredinval_assessedval_imps_base).HasColumnType("money");

                entity.Property(e => e.ptas_appdeterredinval_assessedval_land).HasColumnType("money");

                entity.Property(e => e.ptas_appdeterredinval_assessedval_land_base).HasColumnType("money");

                entity.Property(e => e.ptas_appdeterredinval_fullmarketvalue_imps).HasColumnType("money");

                entity.Property(e => e.ptas_appdeterredinval_fullmarketvalue_imps_base).HasColumnType("money");

                entity.Property(e => e.ptas_appdeterredinval_fullmarketvalue_land).HasColumnType("money");

                entity.Property(e => e.ptas_appdeterredinval_fullmarketvalue_land_base).HasColumnType("money");

                entity.Property(e => e.ptas_appdeterredinval_totalamount_imps).HasColumnType("money");

                entity.Property(e => e.ptas_appdeterredinval_totalamount_imps_base).HasColumnType("money");

                entity.Property(e => e.ptas_appdeterredinval_totalamount_land).HasColumnType("money");

                entity.Property(e => e.ptas_appdeterredinval_totalamount_land_base).HasColumnType("money");

                entity.Property(e => e.ptas_appraisedimprovementincrease).HasColumnType("money");

                entity.Property(e => e.ptas_appraisedimprovementincrease_base).HasColumnType("money");

                entity.Property(e => e.ptas_appraisedimprovementvalue).HasColumnType("money");

                entity.Property(e => e.ptas_appraisedimprovementvalue_base).HasColumnType("money");

                entity.Property(e => e.ptas_appraisedlandvalue).HasColumnType("money");

                entity.Property(e => e.ptas_appraisedlandvalue_base).HasColumnType("money");

                entity.Property(e => e.ptas_appraisedtotalvalue).HasColumnType("money");

                entity.Property(e => e.ptas_appraisedtotalvalue_base).HasColumnType("money");

                entity.Property(e => e.ptas_taxableimprovementvalue).HasColumnType("money");

                entity.Property(e => e.ptas_taxableimprovementvalue_base).HasColumnType("money");

                entity.Property(e => e.ptas_taxablelandvalue).HasColumnType("money");

                entity.Property(e => e.ptas_taxablelandvalue_base).HasColumnType("money");

                entity.Property(e => e.ptas_taxabletotalvalue).HasColumnType("money");

                entity.Property(e => e.ptas_taxabletotalvalue_base).HasColumnType("money");

                entity.HasOne(d => d._createdby_valueNavigation)
                    .WithMany(p => p.ptas_task_createdby_valueNavigation)
                    .HasForeignKey(d => d._createdby_value)
                    .HasConstraintName("FK_systemuser_ptas_task_createdby");

                entity.HasOne(d => d._createdonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_task_createdonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._createdonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_task_createdonbehalfby");

                entity.HasOne(d => d._modifiedby_valueNavigation)
                    .WithMany(p => p.ptas_task_modifiedby_valueNavigation)
                    .HasForeignKey(d => d._modifiedby_value)
                    .HasConstraintName("FK_systemuser_ptas_task_modifiedby");

                entity.HasOne(d => d._modifiedonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_task_modifiedonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._modifiedonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_task_modifiedonbehalfby");

                entity.HasOne(d => d._owningteam_valueNavigation)
                    .WithMany(p => p.ptas_task)
                    .HasForeignKey(d => d._owningteam_value)
                    .HasConstraintName("FK_team_ptas_task_owningteam");

                entity.HasOne(d => d._owninguser_valueNavigation)
                    .WithMany(p => p.ptas_task_owninguser_valueNavigation)
                    .HasForeignKey(d => d._owninguser_value)
                    .HasConstraintName("FK_systemuser_ptas_task_owninguser");

                entity.HasOne(d => d._ptas_accountingsectionsupervisor_valueNavigation)
                    .WithMany(p => p.ptas_task_ptas_accountingsectionsupervisor_valueNavigation)
                    .HasForeignKey(d => d._ptas_accountingsectionsupervisor_value)
                    .HasConstraintName("FK_systemuser_ptas_task_ptas_accountingsectionsupervisor");

                entity.HasOne(d => d._ptas_appraiser_valueNavigation)
                    .WithMany(p => p.ptas_task_ptas_appraiser_valueNavigation)
                    .HasForeignKey(d => d._ptas_appraiser_value)
                    .HasConstraintName("FK_systemuser_ptas_task_ptas_appraiser");

                entity.HasOne(d => d._ptas_commercialsrappraiser_valueNavigation)
                    .WithMany(p => p.ptas_task_ptas_commercialsrappraiser_valueNavigation)
                    .HasForeignKey(d => d._ptas_commercialsrappraiser_value)
                    .HasConstraintName("FK_systemuser_ptas_task_ptas_commercialsrappraiser");

                entity.HasOne(d => d._ptas_convertpropertytypefromid_valueNavigation)
                    .WithMany(p => p.ptas_task_ptas_convertpropertytypefromid_valueNavigation)
                    .HasForeignKey(d => d._ptas_convertpropertytypefromid_value)
                    .HasConstraintName("FK_ptas_propertytype_ptas_task_ptas_convertpropertytypefromid");

                entity.HasOne(d => d._ptas_convertpropertytypetoid_valueNavigation)
                    .WithMany(p => p.ptas_task_ptas_convertpropertytypetoid_valueNavigation)
                    .HasForeignKey(d => d._ptas_convertpropertytypetoid_value)
                    .HasConstraintName("FK_ptas_propertytype_ptas_task_ptas_convertpropertytypetoid");

                entity.HasOne(d => d._ptas_homeimprovementid_valueNavigation)
                    .WithMany(p => p.ptas_task)
                    .HasForeignKey(d => d._ptas_homeimprovementid_value)
                    .HasConstraintName("FK_ptas_homeimprovement_ptas_task_ptas_homeimprovementid");

                entity.HasOne(d => d._ptas_parcelid_valueNavigation)
                    .WithMany(p => p.ptas_task)
                    .HasForeignKey(d => d._ptas_parcelid_value)
                    .HasConstraintName("FK_ptas_parceldetail_ptas_task_ptas_parcelid");

                entity.HasOne(d => d._ptas_portalcontact_valueNavigation)
                    .WithMany(p => p.ptas_task)
                    .HasForeignKey(d => d._ptas_portalcontact_value)
                    .HasConstraintName("FK_ptas_portalcontact_ptas_task_ptas_portalcontact");

                entity.HasOne(d => d._ptas_residentialsrappraiser_valueNavigation)
                    .WithMany(p => p.ptas_task_ptas_residentialsrappraiser_valueNavigation)
                    .HasForeignKey(d => d._ptas_residentialsrappraiser_value)
                    .HasConstraintName("FK_systemuser_ptas_task_ptas_residentialsrappraiser");

                entity.HasOne(d => d._ptas_responsibilityfrom_valueNavigation)
                    .WithMany(p => p.ptas_task_ptas_responsibilityfrom_valueNavigation)
                    .HasForeignKey(d => d._ptas_responsibilityfrom_value)
                    .HasConstraintName("FK_ptas_responsibility_ptas_task_ptas_responsibilityfrom");

                entity.HasOne(d => d._ptas_responsibilityto_valueNavigation)
                    .WithMany(p => p.ptas_task_ptas_responsibilityto_valueNavigation)
                    .HasForeignKey(d => d._ptas_responsibilityto_value)
                    .HasConstraintName("FK_ptas_responsibility_ptas_task_ptas_responsibilityto");

                entity.HasOne(d => d._ptas_salesid_valueNavigation)
                    .WithMany(p => p.ptas_task)
                    .HasForeignKey(d => d._ptas_salesid_value)
                    .HasConstraintName("FK_ptas_sales_ptas_task_ptas_salesid");

                entity.HasOne(d => d._ptas_taxaccountnumber_valueNavigation)
                    .WithMany(p => p.ptas_task)
                    .HasForeignKey(d => d._ptas_taxaccountnumber_value)
                    .HasConstraintName("FK_ptas_taxaccount_ptas_task_ptas_taxaccountnumber");
            });

            modelBuilder.Entity<ptas_taxRollHistory>(entity =>
            {
                entity.HasKey(e => e.taxRollHistoryGuid)
                    .HasName("PK_taxRollHistGuid")
                    .IsClustered(false);

                entity.ToTable("ptas_taxRollHistory", "ptas");

                entity.HasIndex(e => new { e.parcelIdName, e.taxYearIdName })
                    .HasName("idx_ParcelNum_Year")
                    .IsClustered();

                entity.HasIndex(e => new { e.taxYearIdName, e.parcelGuid })
                    .HasName("idx_parcelGuid");

                entity.Property(e => e.taxRollHistoryGuid).ValueGeneratedNever();

                entity.Property(e => e.acctStat)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.levyCodeIdName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.modifiedOn).HasColumnType("datetime");

                entity.Property(e => e.omitYearIdName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.parcelIdName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.recName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.receivableType)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.taxAccountIdName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.taxStat)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.taxYearIdName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.taxableValueReason)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ptas_taxaccount>(entity =>
            {
                entity.ToTable("ptas_taxaccount", "dynamics");

                entity.HasIndex(e => e.modifiedon)
                    .HasName("Idx_ptas_taxaccount_modifiedon");

                entity.Property(e => e.ptas_taxaccountid).ValueGeneratedNever();

                entity.Property(e => e.ptas_lotacreage_calc).HasColumnType("money");

                entity.HasOne(d => d._createdby_valueNavigation)
                    .WithMany(p => p.ptas_taxaccount_createdby_valueNavigation)
                    .HasForeignKey(d => d._createdby_value)
                    .HasConstraintName("FK_systemuser_ptas_taxaccount_createdby");

                entity.HasOne(d => d._createdonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_taxaccount_createdonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._createdonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_taxaccount_createdonbehalfby");

                entity.HasOne(d => d._modifiedby_valueNavigation)
                    .WithMany(p => p.ptas_taxaccount_modifiedby_valueNavigation)
                    .HasForeignKey(d => d._modifiedby_value)
                    .HasConstraintName("FK_systemuser_ptas_taxaccount_modifiedby");

                entity.HasOne(d => d._modifiedonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_taxaccount_modifiedonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._modifiedonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_taxaccount_modifiedonbehalfby");

                entity.HasOne(d => d._owningteam_valueNavigation)
                    .WithMany(p => p.ptas_taxaccount)
                    .HasForeignKey(d => d._owningteam_value)
                    .HasConstraintName("FK_team_ptas_taxaccount_owningteam");

                entity.HasOne(d => d._owninguser_valueNavigation)
                    .WithMany(p => p.ptas_taxaccount_owninguser_valueNavigation)
                    .HasForeignKey(d => d._owninguser_value)
                    .HasConstraintName("FK_systemuser_ptas_taxaccount_owninguser");

                entity.HasOne(d => d._ptas_addr1_cityid_valueNavigation)
                    .WithMany(p => p.ptas_taxaccount)
                    .HasForeignKey(d => d._ptas_addr1_cityid_value)
                    .HasConstraintName("FK_ptas_city_ptas_taxaccount_ptas_addr1_cityid");

                entity.HasOne(d => d._ptas_addr1_countryid_valueNavigation)
                    .WithMany(p => p.ptas_taxaccount)
                    .HasForeignKey(d => d._ptas_addr1_countryid_value)
                    .HasConstraintName("FK_ptas_country_ptas_taxaccount_ptas_addr1_countryid");

                entity.HasOne(d => d._ptas_addr1_stateid_valueNavigation)
                    .WithMany(p => p.ptas_taxaccount)
                    .HasForeignKey(d => d._ptas_addr1_stateid_value)
                    .HasConstraintName("FK_ptas_stateorprovince_ptas_taxaccount_ptas_addr1_stateid");

                entity.HasOne(d => d._ptas_addr1_zipcodeid_valueNavigation)
                    .WithMany(p => p.ptas_taxaccount)
                    .HasForeignKey(d => d._ptas_addr1_zipcodeid_value)
                    .HasConstraintName("FK_ptas_zipcode_ptas_taxaccount_ptas_addr1_zipcodeid");

                entity.HasOne(d => d._ptas_condounitid_valueNavigation)
                    .WithMany(p => p.ptas_taxaccount)
                    .HasForeignKey(d => d._ptas_condounitid_value)
                    .HasConstraintName("FK_ptas_condounit_ptas_taxaccount_ptas_condounitid");

                entity.HasOne(d => d._ptas_levycodeid_valueNavigation)
                    .WithMany(p => p.ptas_taxaccount)
                    .HasForeignKey(d => d._ptas_levycodeid_value)
                    .HasConstraintName("FK_ptas_levycode_ptas_taxaccount_ptas_levycodeid");

                entity.HasOne(d => d._ptas_masspayerid_valueNavigation)
                    .WithMany(p => p.ptas_taxaccount)
                    .HasForeignKey(d => d._ptas_masspayerid_value)
                    .HasConstraintName("FK_ptas_masspayer_ptas_taxaccount_ptas_masspayerid");

                entity.HasOne(d => d._ptas_mastertaxaccountid_valueNavigation)
                    .WithMany(p => p.Inverse_ptas_mastertaxaccountid_valueNavigation)
                    .HasForeignKey(d => d._ptas_mastertaxaccountid_value)
                    .HasConstraintName("FK_ptas_taxaccount_ptas_taxaccount_ptas_mastertaxaccountid");

                entity.HasOne(d => d._ptas_parcelid_valueNavigation)
                    .WithMany(p => p.ptas_taxaccount)
                    .HasForeignKey(d => d._ptas_parcelid_value)
                    .HasConstraintName("FK_ptas_parceldetail_ptas_taxaccount_ptas_parcelid");
            });

            modelBuilder.Entity<ptas_taxaccount_snapshot>(entity =>
            {
                entity.HasKey(e => e.ptas_taxaccountid);

                entity.ToTable("ptas_taxaccount_snapshot", "dynamics");

                entity.Property(e => e.ptas_taxaccountid).ValueGeneratedNever();

                entity.Property(e => e.ptas_lotacreage_calc).HasColumnType("money");
            });

            modelBuilder.Entity<ptas_trendfactor>(entity =>
            {
                entity.ToTable("ptas_trendfactor", "dynamics");

                entity.Property(e => e.ptas_trendfactorid).ValueGeneratedNever();

                entity.Property(e => e.ptas_interestrate).HasColumnType("money");

                entity.Property(e => e.ptas_percentgoodfactor).HasColumnType("money");

                entity.HasOne(d => d._createdby_valueNavigation)
                    .WithMany(p => p.ptas_trendfactor_createdby_valueNavigation)
                    .HasForeignKey(d => d._createdby_value)
                    .HasConstraintName("FK_systemuser_ptas_trendfactor_createdby");

                entity.HasOne(d => d._createdonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_trendfactor_createdonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._createdonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_trendfactor_createdonbehalfby");

                entity.HasOne(d => d._modifiedby_valueNavigation)
                    .WithMany(p => p.ptas_trendfactor_modifiedby_valueNavigation)
                    .HasForeignKey(d => d._modifiedby_value)
                    .HasConstraintName("FK_systemuser_ptas_trendfactor_modifiedby");

                entity.HasOne(d => d._modifiedonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_trendfactor_modifiedonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._modifiedonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_trendfactor_modifiedonbehalfby");

                entity.HasOne(d => d._owningteam_valueNavigation)
                    .WithMany(p => p.ptas_trendfactor)
                    .HasForeignKey(d => d._owningteam_value)
                    .HasConstraintName("FK_team_ptas_trendfactor_owningteam");

                entity.HasOne(d => d._owninguser_valueNavigation)
                    .WithMany(p => p.ptas_trendfactor_owninguser_valueNavigation)
                    .HasForeignKey(d => d._owninguser_value)
                    .HasConstraintName("FK_systemuser_ptas_trendfactor_owninguser");

                entity.HasOne(d => d._ptas_assessmentyearid_valueNavigation)
                    .WithMany(p => p.ptas_trendfactor)
                    .HasForeignKey(d => d._ptas_assessmentyearid_value)
                    .HasConstraintName("FK_ptas_year_ptas_trendfactor_ptas_assessmentyearid");
            });

            modelBuilder.Entity<ptas_unitbreakdown>(entity =>
            {
                entity.ToTable("ptas_unitbreakdown", "dynamics");

                entity.HasIndex(e => e.modifiedon)
                    .HasName("Idx_ptas_unitbreakdown_modifiedon");

                entity.Property(e => e.ptas_unitbreakdownid).ValueGeneratedNever();

                entity.Property(e => e.ptas_cityparcel).HasColumnType("money");

                entity.Property(e => e.ptas_dnrparcel).HasColumnType("money");

                entity.Property(e => e.ptas_nbrofbedrooms).HasColumnType("money");

                entity.Property(e => e.ptas_numberofbathrooms).HasColumnType("money");

                entity.Property(e => e.ptas_subjectparcel).HasColumnType("money");

                entity.HasOne(d => d._createdby_valueNavigation)
                    .WithMany(p => p.ptas_unitbreakdown_createdby_valueNavigation)
                    .HasForeignKey(d => d._createdby_value)
                    .HasConstraintName("FK_systemuser_ptas_unitbreakdown_createdby");

                entity.HasOne(d => d._createdonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_unitbreakdown_createdonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._createdonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_unitbreakdown_createdonbehalfby");

                entity.HasOne(d => d._modifiedby_valueNavigation)
                    .WithMany(p => p.ptas_unitbreakdown_modifiedby_valueNavigation)
                    .HasForeignKey(d => d._modifiedby_value)
                    .HasConstraintName("FK_systemuser_ptas_unitbreakdown_modifiedby");

                entity.HasOne(d => d._modifiedonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_unitbreakdown_modifiedonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._modifiedonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_unitbreakdown_modifiedonbehalfby");

                entity.HasOne(d => d._owningteam_valueNavigation)
                    .WithMany(p => p.ptas_unitbreakdown)
                    .HasForeignKey(d => d._owningteam_value)
                    .HasConstraintName("FK_team_ptas_unitbreakdown_owningteam");

                entity.HasOne(d => d._owninguser_valueNavigation)
                    .WithMany(p => p.ptas_unitbreakdown_owninguser_valueNavigation)
                    .HasForeignKey(d => d._owninguser_value)
                    .HasConstraintName("FK_systemuser_ptas_unitbreakdown_owninguser");

                entity.HasOne(d => d._ptas_buildingid_valueNavigation)
                    .WithMany(p => p.ptas_unitbreakdown)
                    .HasForeignKey(d => d._ptas_buildingid_value)
                    .HasConstraintName("FK_ptas_buildingdetail_ptas_unitbreakdown_ptas_buildingid");

                entity.HasOne(d => d._ptas_condocomplexid_valueNavigation)
                    .WithMany(p => p.ptas_unitbreakdown)
                    .HasForeignKey(d => d._ptas_condocomplexid_value)
                    .HasConstraintName("FK_ptas_condocomplex_ptas_unitbreakdown_ptas_condocomplexid");

                entity.HasOne(d => d._ptas_floatinghome_valueNavigation)
                    .WithMany(p => p.ptas_unitbreakdown)
                    .HasForeignKey(d => d._ptas_floatinghome_value)
                    .HasConstraintName("FK_ptas_condounit_ptas_unitbreakdown_ptas_floatinghome");

                entity.HasOne(d => d._ptas_masterunitbreakdownid_valueNavigation)
                    .WithMany(p => p.Inverse_ptas_masterunitbreakdownid_valueNavigation)
                    .HasForeignKey(d => d._ptas_masterunitbreakdownid_value)
                    .HasConstraintName("FK_ptas_unitbreakdown_ptas_unitbreakdown_ptas_masterunitbreakdownid");

                entity.HasOne(d => d._ptas_unitbreakdowntypeid_valueNavigation)
                    .WithMany(p => p.ptas_unitbreakdown)
                    .HasForeignKey(d => d._ptas_unitbreakdowntypeid_value)
                    .HasConstraintName("FK_ptas_unitbreakdowntype_ptas_unitbreakdown_ptas_unitbreakdowntypeid");
            });

            modelBuilder.Entity<ptas_unitbreakdown_snapshot>(entity =>
            {
                entity.HasKey(e => e.ptas_unitbreakdownid);

                entity.ToTable("ptas_unitbreakdown_snapshot", "dynamics");

                entity.Property(e => e.ptas_unitbreakdownid).ValueGeneratedNever();

                entity.Property(e => e.ptas_cityparcel).HasColumnType("money");

                entity.Property(e => e.ptas_dnrparcel).HasColumnType("money");

                entity.Property(e => e.ptas_nbrofbedrooms).HasColumnType("money");

                entity.Property(e => e.ptas_numberofbathrooms).HasColumnType("money");

                entity.Property(e => e.ptas_subjectparcel).HasColumnType("money");
            });

            modelBuilder.Entity<ptas_unitbreakdowntype>(entity =>
            {
                entity.ToTable("ptas_unitbreakdowntype", "dynamics");

                entity.HasIndex(e => e.modifiedon)
                    .HasName("Idx_ptas_unitbreakdowntype_modifiedon");

                entity.Property(e => e.ptas_unitbreakdowntypeid).ValueGeneratedNever();

                entity.HasOne(d => d._createdby_valueNavigation)
                    .WithMany(p => p.ptas_unitbreakdowntype_createdby_valueNavigation)
                    .HasForeignKey(d => d._createdby_value)
                    .HasConstraintName("FK_systemuser_ptas_unitbreakdowntype_createdby");

                entity.HasOne(d => d._createdonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_unitbreakdowntype_createdonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._createdonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_unitbreakdowntype_createdonbehalfby");

                entity.HasOne(d => d._modifiedby_valueNavigation)
                    .WithMany(p => p.ptas_unitbreakdowntype_modifiedby_valueNavigation)
                    .HasForeignKey(d => d._modifiedby_value)
                    .HasConstraintName("FK_systemuser_ptas_unitbreakdowntype_modifiedby");

                entity.HasOne(d => d._modifiedonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_unitbreakdowntype_modifiedonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._modifiedonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_unitbreakdowntype_modifiedonbehalfby");
            });

            modelBuilder.Entity<ptas_visitedsketch>(entity =>
            {
                entity.ToTable("ptas_visitedsketch", "dynamics");

                entity.HasIndex(e => e.modifiedon)
                    .HasName("Idx_ptas_visitedsketch_modifiedon");

                entity.Property(e => e.ptas_visitedsketchid).ValueGeneratedNever();

                entity.HasOne(d => d._createdby_valueNavigation)
                    .WithMany(p => p.ptas_visitedsketch_createdby_valueNavigation)
                    .HasForeignKey(d => d._createdby_value)
                    .HasConstraintName("FK_systemuser_ptas_visitedsketch_createdby");

                entity.HasOne(d => d._createdonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_visitedsketch_createdonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._createdonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_visitedsketch_createdonbehalfby");

                entity.HasOne(d => d._modifiedby_valueNavigation)
                    .WithMany(p => p.ptas_visitedsketch_modifiedby_valueNavigation)
                    .HasForeignKey(d => d._modifiedby_value)
                    .HasConstraintName("FK_systemuser_ptas_visitedsketch_modifiedby");

                entity.HasOne(d => d._modifiedonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_visitedsketch_modifiedonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._modifiedonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_visitedsketch_modifiedonbehalfby");

                entity.HasOne(d => d._owningteam_valueNavigation)
                    .WithMany(p => p.ptas_visitedsketch)
                    .HasForeignKey(d => d._owningteam_value)
                    .HasConstraintName("FK_team_ptas_visitedsketch_owningteam");

                entity.HasOne(d => d._owninguser_valueNavigation)
                    .WithMany(p => p.ptas_visitedsketch_owninguser_valueNavigation)
                    .HasForeignKey(d => d._owninguser_value)
                    .HasConstraintName("FK_systemuser_ptas_visitedsketch_owninguser");

                entity.HasOne(d => d._ptas_sketchid_valueNavigation)
                    .WithMany(p => p.ptas_visitedsketch)
                    .HasForeignKey(d => d._ptas_sketchid_value)
                    .HasConstraintName("FK_ptas_sketch_ptas_visitedsketch_ptas_sketchid");

                entity.HasOne(d => d._ptas_visitedbyid_valueNavigation)
                    .WithMany(p => p.ptas_visitedsketch_ptas_visitedbyid_valueNavigation)
                    .HasForeignKey(d => d._ptas_visitedbyid_value)
                    .HasConstraintName("FK_systemuser_ptas_visitedsketch_ptas_visitedbyid");
            });

            modelBuilder.Entity<ptas_year>(entity =>
            {
                entity.ToTable("ptas_year", "dynamics");

                entity.HasIndex(e => e.modifiedon)
                    .HasName("Idx_ptas_year_modifiedon");

                entity.Property(e => e.ptas_yearid).ValueGeneratedNever();

                entity.Property(e => e.exchangerate).HasColumnType("money");

                entity.Property(e => e.ptas_1constitutionalcheck).HasColumnType("money");

                entity.Property(e => e.ptas_constitutionalcheck).HasColumnType("money");

                entity.Property(e => e.ptas_constitutionalcheck_base).HasColumnType("money");

                entity.Property(e => e.ptas_costindexadjustmentvalue).HasColumnType("money");

                entity.Property(e => e.ptas_implicitpricedeflator).HasColumnType("money");

                entity.Property(e => e.ptas_integralhomesiteimprovementsvalue).HasColumnType("money");

                entity.Property(e => e.ptas_integralhomesiteimprovementsvalue_base).HasColumnType("money");

                entity.Property(e => e.ptas_integralhomesiteperacrevalue).HasColumnType("money");

                entity.Property(e => e.ptas_integralhomesiteperacrevalue_base).HasColumnType("money");

                entity.Property(e => e.ptas_limitfactor).HasColumnType("money");

                entity.Property(e => e.ptas_personalpropertyratio).HasColumnType("money");

                entity.Property(e => e.ptas_realpropertyratio).HasColumnType("money");

                entity.Property(e => e.ptas_totalfarmandagriculturalacres).HasColumnType("money");

                entity.Property(e => e.ptas_totalfarmandagriculturallandvalue).HasColumnType("money");

                entity.Property(e => e.ptas_totalfarmandagriculturallandvalue_base).HasColumnType("money");

                entity.HasOne(d => d._createdby_valueNavigation)
                    .WithMany(p => p.ptas_year_createdby_valueNavigation)
                    .HasForeignKey(d => d._createdby_value)
                    .HasConstraintName("FK_systemuser_ptas_year_createdby");

                entity.HasOne(d => d._createdonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_year_createdonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._createdonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_year_createdonbehalfby");

                entity.HasOne(d => d._modifiedby_valueNavigation)
                    .WithMany(p => p.ptas_year_modifiedby_valueNavigation)
                    .HasForeignKey(d => d._modifiedby_value)
                    .HasConstraintName("FK_systemuser_ptas_year_modifiedby");

                entity.HasOne(d => d._modifiedonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_year_modifiedonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._modifiedonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_year_modifiedonbehalfby");

                entity.HasOne(d => d._ptas_rollovernotificationid_valueNavigation)
                    .WithMany(p => p.ptas_year_ptas_rollovernotificationid_valueNavigation)
                    .HasForeignKey(d => d._ptas_rollovernotificationid_value)
                    .HasConstraintName("FK_systemuser_ptas_year_ptas_rollovernotificationid");
            });

            modelBuilder.Entity<ptas_zipcode>(entity =>
            {
                entity.ToTable("ptas_zipcode", "dynamics");

                entity.HasIndex(e => e.modifiedon)
                    .HasName("Idx_ptas_zipcode_modifiedon");

                entity.Property(e => e.ptas_zipcodeid).ValueGeneratedNever();

                entity.HasOne(d => d._createdby_valueNavigation)
                    .WithMany(p => p.ptas_zipcode_createdby_valueNavigation)
                    .HasForeignKey(d => d._createdby_value)
                    .HasConstraintName("FK_systemuser_ptas_zipcode_createdby");

                entity.HasOne(d => d._createdonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_zipcode_createdonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._createdonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_zipcode_createdonbehalfby");

                entity.HasOne(d => d._modifiedby_valueNavigation)
                    .WithMany(p => p.ptas_zipcode_modifiedby_valueNavigation)
                    .HasForeignKey(d => d._modifiedby_value)
                    .HasConstraintName("FK_systemuser_ptas_zipcode_modifiedby");

                entity.HasOne(d => d._modifiedonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_zipcode_modifiedonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._modifiedonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_zipcode_modifiedonbehalfby");
            });

            modelBuilder.Entity<ptas_zipcode_stateorprovince>(entity =>
            {
                entity.ToTable("ptas_zipcode_stateorprovince", "dynamics");

                entity.HasIndex(e => e.modifiedon)
                    .HasName("Idx_ptas_zipcode_stateorprovince_modifiedon");

                entity.Property(e => e.ptas_zipcode_stateorprovinceid).ValueGeneratedNever();

                entity.Property(e => e.modifiedon)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");
            });

            modelBuilder.Entity<ptas_zoning>(entity =>
            {
                entity.ToTable("ptas_zoning", "dynamics");

                entity.HasIndex(e => e.modifiedon)
                    .HasName("Idx_ptas_zoning_modifiedon");

                entity.Property(e => e.ptas_zoningid).ValueGeneratedNever();

                entity.HasOne(d => d._createdby_valueNavigation)
                    .WithMany(p => p.ptas_zoning_createdby_valueNavigation)
                    .HasForeignKey(d => d._createdby_value)
                    .HasConstraintName("FK_systemuser_ptas_zoning_createdby");

                entity.HasOne(d => d._createdonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_zoning_createdonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._createdonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_zoning_createdonbehalfby");

                entity.HasOne(d => d._modifiedby_valueNavigation)
                    .WithMany(p => p.ptas_zoning_modifiedby_valueNavigation)
                    .HasForeignKey(d => d._modifiedby_value)
                    .HasConstraintName("FK_systemuser_ptas_zoning_modifiedby");

                entity.HasOne(d => d._modifiedonbehalfby_valueNavigation)
                    .WithMany(p => p.ptas_zoning_modifiedonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._modifiedonbehalfby_value)
                    .HasConstraintName("FK_systemuser_ptas_zoning_modifiedonbehalfby");
            });

            modelBuilder.Entity<role>(entity =>
            {
                entity.ToTable("role", "dynamics");

                entity.HasIndex(e => e.modifiedon)
                    .HasName("Idx_role_modifiedon");

                entity.Property(e => e.roleid).ValueGeneratedNever();

                entity.HasOne(d => d._createdby_valueNavigation)
                    .WithMany(p => p.role_createdby_valueNavigation)
                    .HasForeignKey(d => d._createdby_value)
                    .HasConstraintName("FK_systemuser_role_createdby");

                entity.HasOne(d => d._createdonbehalfby_valueNavigation)
                    .WithMany(p => p.role_createdonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._createdonbehalfby_value)
                    .HasConstraintName("FK_systemuser_role_createdonbehalfby");

                entity.HasOne(d => d._modifiedby_valueNavigation)
                    .WithMany(p => p.role_modifiedby_valueNavigation)
                    .HasForeignKey(d => d._modifiedby_value)
                    .HasConstraintName("FK_systemuser_role_modifiedby");

                entity.HasOne(d => d._modifiedonbehalfby_valueNavigation)
                    .WithMany(p => p.role_modifiedonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._modifiedonbehalfby_value)
                    .HasConstraintName("FK_systemuser_role_modifiedonbehalfby");

                entity.HasOne(d => d._parentroleid_valueNavigation)
                    .WithMany(p => p.Inverse_parentroleid_valueNavigation)
                    .HasForeignKey(d => d._parentroleid_value)
                    .HasConstraintName("FK_role_role_parentroleid");

                entity.HasOne(d => d._parentrootroleid_valueNavigation)
                    .WithMany(p => p.Inverse_parentrootroleid_valueNavigation)
                    .HasForeignKey(d => d._parentrootroleid_value)
                    .HasConstraintName("FK_role_role_parentrootroleid");
            });

            modelBuilder.Entity<stringmap>(entity =>
            {
                entity.ToTable("stringmap", "dynamics");

                entity.HasIndex(e => e.displayorder)
                    .HasName("Idx_displayorder");

                entity.HasIndex(e => e.modifiedon)
                    .HasName("Idx_stringmap_modifiedon");

                entity.HasIndex(e => new { e.attributename, e.objecttypecode })
                    .HasName("Idx_attributename_objecttypecode");

                entity.Property(e => e.stringmapid).ValueGeneratedNever();

                entity.Property(e => e.attributename).HasMaxLength(1000);

                entity.Property(e => e.modifiedon)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.objecttypecode).HasMaxLength(1000);

                entity.Property(e => e.value).HasMaxLength(4000);
            });

            modelBuilder.Entity<systemuser>(entity =>
            {
                entity.ToTable("systemuser", "dynamics");

                entity.HasIndex(e => e.modifiedon)
                    .HasName("Idx_systemuser_modifiedon");

                entity.Property(e => e.systemuserid).ValueGeneratedNever();

                entity.Property(e => e.exchangerate).HasColumnType("money");

                entity.HasOne(d => d._createdby_valueNavigation)
                    .WithMany(p => p.Inverse_createdby_valueNavigation)
                    .HasForeignKey(d => d._createdby_value)
                    .HasConstraintName("FK_systemuser_systemuser_createdby");

                entity.HasOne(d => d._createdonbehalfby_valueNavigation)
                    .WithMany(p => p.Inverse_createdonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._createdonbehalfby_value)
                    .HasConstraintName("FK_systemuser_systemuser_createdonbehalfby");

                entity.HasOne(d => d._modifiedby_valueNavigation)
                    .WithMany(p => p.Inverse_modifiedby_valueNavigation)
                    .HasForeignKey(d => d._modifiedby_value)
                    .HasConstraintName("FK_systemuser_systemuser_modifiedby");

                entity.HasOne(d => d._modifiedonbehalfby_valueNavigation)
                    .WithMany(p => p.Inverse_modifiedonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._modifiedonbehalfby_value)
                    .HasConstraintName("FK_systemuser_systemuser_modifiedonbehalfby");

                entity.HasOne(d => d._parentsystemuserid_valueNavigation)
                    .WithMany(p => p.Inverse_parentsystemuserid_valueNavigation)
                    .HasForeignKey(d => d._parentsystemuserid_value)
                    .HasConstraintName("FK_systemuser_systemuser_parentsystemuserid");
            });

            modelBuilder.Entity<systemuserroles>(entity =>
            {
                entity.HasKey(e => e.systemuserroleid);

                entity.ToTable("systemuserroles", "dynamics");

                entity.HasIndex(e => e.modifiedon)
                    .HasName("Idx_systemuserroles_modifiedon");

                entity.Property(e => e.systemuserroleid).ValueGeneratedNever();

                entity.Property(e => e.modifiedon)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");
            });

            modelBuilder.Entity<team>(entity =>
            {
                entity.ToTable("team", "dynamics");

                entity.HasIndex(e => e.modifiedon)
                    .HasName("Idx_team_modifiedon");

                entity.Property(e => e.teamid).ValueGeneratedNever();

                entity.Property(e => e.exchangerate).HasColumnType("money");

                entity.HasOne(d => d._administratorid_valueNavigation)
                    .WithMany(p => p.team_administratorid_valueNavigation)
                    .HasForeignKey(d => d._administratorid_value)
                    .HasConstraintName("FK_systemuser_team_administratorid");

                entity.HasOne(d => d._createdby_valueNavigation)
                    .WithMany(p => p.team_createdby_valueNavigation)
                    .HasForeignKey(d => d._createdby_value)
                    .HasConstraintName("FK_systemuser_team_createdby");

                entity.HasOne(d => d._createdonbehalfby_valueNavigation)
                    .WithMany(p => p.team_createdonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._createdonbehalfby_value)
                    .HasConstraintName("FK_systemuser_team_createdonbehalfby");

                entity.HasOne(d => d._modifiedby_valueNavigation)
                    .WithMany(p => p.team_modifiedby_valueNavigation)
                    .HasForeignKey(d => d._modifiedby_value)
                    .HasConstraintName("FK_systemuser_team_modifiedby");

                entity.HasOne(d => d._modifiedonbehalfby_valueNavigation)
                    .WithMany(p => p.team_modifiedonbehalfby_valueNavigation)
                    .HasForeignKey(d => d._modifiedonbehalfby_value)
                    .HasConstraintName("FK_systemuser_team_modifiedonbehalfby");
            });

            modelBuilder.Entity<teammembership>(entity =>
            {
                entity.ToTable("teammembership", "dynamics");

                entity.HasIndex(e => e.modifiedon)
                    .HasName("Idx_teammembership_modifiedon");

                entity.Property(e => e.teammembershipid).ValueGeneratedNever();

                entity.Property(e => e.modifiedon)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");
            });

            modelBuilder.Entity<teamprofiles>(entity =>
            {
                entity.HasKey(e => e.teamprofileid);

                entity.ToTable("teamprofiles", "dynamics");

                entity.HasIndex(e => e.modifiedon)
                    .HasName("Idx_teamprofiles_modifiedon");

                entity.Property(e => e.teamprofileid).ValueGeneratedNever();

                entity.Property(e => e.modifiedon)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");
            });

            modelBuilder.Entity<teamroles>(entity =>
            {
                entity.HasKey(e => e.teamroleid);

                entity.ToTable("teamroles", "dynamics");

                entity.HasIndex(e => e.modifiedon)
                    .HasName("Idx_teamroles_modifiedon");

                entity.Property(e => e.teamroleid).ValueGeneratedNever();

                entity.Property(e => e.modifiedon)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
