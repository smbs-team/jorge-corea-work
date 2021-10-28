namespace CustomSearchesEFLibrary.Gis
{
    using System.Data.Common;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.Gis.Model;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.IdentityModel.Clients.ActiveDirectory;
    using PTASServicesCommon.TokenProvider;

    public partial class GisDbContext : DbContext
    {
        /// <summary>
        /// The connection string password section.
        /// </summary>
        private const string ConnectionStringPasswordSection = "password";

        /// <summary>
        /// Initializes a new instance of the <see cref="GisDbContext" /> class.
        /// </summary>
        /// <param name="options">The options for this context.</param>
        /// <param name="tokenProvider">The token provider.</param>
        /// <param name="principalCredentials">The principal credentials.</param>
        /// <exception cref="System.ArgumentNullException">tokenProvider.</exception>
        public GisDbContext(DbContextOptions<GisDbContext> options, IServiceTokenProvider tokenProvider, ClientCredential principalCredentials)
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
                if (!conn.ConnectionString.Contains(GisDbContext.ConnectionStringPasswordSection, System.StringComparison.OrdinalIgnoreCase))
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

        public virtual DbSet<Folder> Folder { get; set; }
        public virtual DbSet<FolderItemType> FolderItemType { get; set; }
        public virtual DbSet<FolderType> FolderType { get; set; }
        public virtual DbSet<LayerSource> LayerSource { get; set; }
        public virtual DbSet<MapRenderer> MapRenderer { get; set; }
        public virtual DbSet<MobileLayerRenderer> MobileLayerRenderer { get; set; }
        public virtual DbSet<MapRendererCategory> MapRendererCategory { get; set; }
        public virtual DbSet<MapRendererCategoryMapRenderer> MapRendererCategoryMapRenderer { get; set; }
        public virtual DbSet<MapRendererLogicType> MapRendererLogicType { get; set; }
        public virtual DbSet<MapRendererType> MapRendererType { get; set; }
        public virtual DbSet<MapRendererUserSelection> MapRendererUserSelection { get; set; }
        public virtual DbSet<Systemuser> Systemuser { get; set; }
        public virtual DbSet<UserMap> UserMap { get; set; }
        public virtual DbSet<UserMapCategory> UserMapCategory { get; set; }
        public virtual DbSet<UserMapCategoryUserMap> UserMapCategoryUserMap { get; set; }
        public virtual DbSet<UserMapSelection> UserMapSelection { get; set; }

        /// <summary>
        /// Switches staging and final tables.
        /// </summary>
        /// <param name="tableName">Name of the table to switch.</param>
        public virtual void SwitchStagingTable(string tableName)
        {
            this.Database.ExecuteSqlInterpolated($"exec [gis].[SwitchStagingTable] {tableName}");
        }

        public virtual void DeleteTable(string tableName)
        {
            string dropStatement = $"drop table if exists gis.{tableName}";
            this.Database.ExecuteSqlRaw(dropStatement);
        }

        /// <summary>
        /// Gets the database connection and open the connection if isn't open.
        /// </summary>
        /// <returns>The database connection.</returns>
        public DbConnection GetOpenConnection()
        {
            var connection = this.Database.GetDbConnection();

            if (connection.State != System.Data.ConnectionState.Open)
            {
                connection.Open();
            }

            return connection;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Folder>(entity =>
            {
                entity.ToTable("Folder", "gis");

                entity.HasIndex(e => e.FolderName);

                entity.HasIndex(e => e.FolderType);

                entity.HasIndex(e => e.ParentFolderId);

                entity.HasIndex(e => e.UserId);

                entity.Property(e => e.FolderItemType)
                    .IsRequired()
                    .HasMaxLength(256)
                    .HasDefaultValueSql("('UserMap')");

                entity.Property(e => e.FolderName)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.FolderType)
                    .IsRequired()
                    .HasMaxLength(256)
                    .HasDefaultValueSql("('User')");

                entity.HasOne(d => d.FolderItemTypeNavigation)
                    .WithMany(p => p.Folder)
                    .HasForeignKey(d => d.FolderItemType)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Folder_ToFolderItemType");

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

            modelBuilder.Entity<FolderItemType>(entity =>
            {
                entity.HasKey(e => e.FolderItemType1)
                    .HasName("PK_FolderItemType_FolderItemType");

                entity.ToTable("FolderItemType", "gis");

                entity.Property(e => e.FolderItemType1)
                    .HasColumnName("FolderItemType")
                    .HasMaxLength(256);
            });

            modelBuilder.Entity<FolderType>(entity =>
            {
                entity.HasKey(e => e.FolderType1)
                    .HasName("PK_FolderType_FolderType");

                entity.ToTable("FolderType", "gis");

                entity.Property(e => e.FolderType1)
                    .HasColumnName("FolderType")
                    .HasMaxLength(256);
            });

            modelBuilder.Entity<LayerSource>(entity =>
            {
                entity.ToTable("LayerSource", "gis");

                entity.HasIndex(e => e.HasOfflineSupport);

                entity.HasIndex(e => e.IsParcelSource);

                entity.HasIndex(e => e.IsVectorLayer);

                entity.HasIndex(e => e.LayerSourceAlias);

                entity.HasIndex(e => e.LayerSourceName);

                entity.Property(e => e.DataSourceUrl)
                    .IsRequired()
                    .HasMaxLength(2048);

                entity.Property(e => e.DefaultLabelMapboxLayer);

                entity.Property(e => e.DefaultLabelMaxZoom).HasDefaultValueSql("((16))");

                entity.Property(e => e.DefaultLabelMinZoom).HasDefaultValueSql("((10))");

                entity.Property(e => e.DefaultMapboxLayer).IsRequired();

                entity.Property(e => e.DefaultMaxZoom).HasDefaultValueSql("((16))");

                entity.Property(e => e.DefaultMinZoom).HasDefaultValueSql("((10))");

                entity.Property(e => e.Jurisdiction).HasMaxLength(256);

                entity.Property(e => e.LayerSourceAlias)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.LayerSourceName)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.Organization).HasMaxLength(256);

                entity.Property(e => e.TileSize).HasDefaultValueSql("((256))");
            });

            modelBuilder.Entity<MobileLayerRenderer>(entity =>
            {
                entity.ToTable("MobileLayerRenderer", "gis");

                entity.HasIndex(e => e.MobileLayerRendererId);

                entity.Property(e => e.CreatedTimestamp)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.LastModifiedTimestamp)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.Query).IsRequired();

                entity.Property(e => e.RendererRules).IsRequired();

                entity.Property(e => e.IsSelected);
                entity.Property(e => e.Role)
                    .HasMaxLength(100);
                entity.Property(e => e.Categories)
                    .HasMaxLength(4000);

                entity.HasOne(d => d.LayerSource)
                    .WithMany(p => p.MobileLayerRenderers)
                    .HasForeignKey(d => d.LayerSourceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MobileLayerRenderer_LayerSource");
            });

            modelBuilder.Entity<MapRenderer>(entity =>
            {
                entity.ToTable("MapRenderer", "gis");

                entity.HasIndex(e => e.LayerSourceId);

                entity.HasIndex(e => e.MapRendererLogicType);

                entity.HasIndex(e => e.MapRendererType);

                entity.HasIndex(e => e.UserMapId);

                entity.Property(e => e.CreatedTimestamp)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.LastModifiedTimestamp)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.MapRendererLogicType)
                    .IsRequired()
                    .HasMaxLength(256)
                    .HasDefaultValueSql("('Simple')");

                entity.Property(e => e.MapRendererName)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.MapRendererType)
                    .IsRequired()
                    .HasMaxLength(256)
                    .HasDefaultValueSql("('Parcel')");

                entity.Property(e => e.RendererRules).IsRequired();

                entity.HasOne(d => d.LayerSource)
                    .WithMany(p => p.MapRenderer)
                    .HasForeignKey(d => d.LayerSourceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MapRenderer_ToLayerSource");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.MapRendererCreatedByNavigation)
                    .HasForeignKey(d => d.CreatedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MapRenderer_ToSystemUser_CreatedBy")
                    .IsRequired(false);

                entity.HasOne(d => d.LastModifiedByNavigation)
                    .WithMany(p => p.MapRendererLastModifiedByNavigation)
                    .HasForeignKey(d => d.LastModifiedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MapRenderer_ToSystemUser_LastModifiedBy")
                    .IsRequired(false);

                entity.HasOne(d => d.MapRendererLogicTypeNavigation)
                    .WithMany(p => p.MapRenderer)
                    .HasForeignKey(d => d.MapRendererLogicType)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MapRenderer_ToMapRendererLogicType");

                entity.HasOne(d => d.MapRendererTypeNavigation)
                    .WithMany(p => p.MapRenderer)
                    .HasForeignKey(d => d.MapRendererType)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MapRenderer_ToMapRendererType");

                entity.HasOne(d => d.UserMap)
                    .WithMany(p => p.MapRenderer)
                    .HasForeignKey(d => d.UserMapId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MapRenderer_ToUserMap");
            });

            modelBuilder.Entity<MapRendererCategory>(entity =>
            {
                entity.ToTable("MapRendererCategory", "gis");

                entity.HasIndex(e => e.CategoryName);

                entity.Property(e => e.CategoryDescription)
                    .IsRequired()
                    .HasMaxLength(2048);

                entity.Property(e => e.CategoryName)
                    .IsRequired()
                    .HasMaxLength(256);
            });

            modelBuilder.Entity<MapRendererCategoryMapRenderer>(entity =>
            {
                entity.HasKey(e => new { e.MapRendererCategoryId, e.MapRendererId })
                    .HasName("PK_MapRendererCategory_MapRenderer_MapRendererCategoryIdMapRendererId");

                entity.ToTable("MapRendererCategory_MapRenderer", "gis");

                entity.HasOne(d => d.MapRendererCategory)
                    .WithMany(p => p.MapRendererCategoryMapRenderer)
                    .HasForeignKey(d => d.MapRendererCategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MapRendererCategory_MapRenderer_ToMapRendererCategory");

                entity.HasOne(d => d.MapRenderer)
                    .WithMany(p => p.MapRendererCategoryMapRenderer)
                    .HasForeignKey(d => d.MapRendererId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MapRendererCategory_MapRenderer_ToMapRenderer");
            });

            modelBuilder.Entity<MapRendererLogicType>(entity =>
            {
                entity.HasKey(e => e.MapRendererLogicType1)
                    .HasName("PK_MapRendererLogicType_MapRendererLogicType");

                entity.ToTable("MapRendererLogicType", "gis");

                entity.Property(e => e.MapRendererLogicType1)
                    .HasColumnName("MapRendererLogicType")
                    .HasMaxLength(256);
            });

            modelBuilder.Entity<MapRendererType>(entity =>
            {
                entity.HasKey(e => e.MapRendererType1)
                    .HasName("PK_MapRendererType_MapRendererType");

                entity.ToTable("MapRendererType", "gis");

                entity.Property(e => e.MapRendererType1)
                    .HasColumnName("MapRendererType")
                    .HasMaxLength(256);
            });

            modelBuilder.Entity<MapRendererUserSelection>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("PK_MapRendererUserSelection_UserId");

                entity.ToTable("MapRendererUserSelection", "gis");

                entity.Property(e => e.UserId).ValueGeneratedNever();

                entity.HasOne(d => d.MapRenderer)
                    .WithMany(p => p.MapRendererUserSelection)
                    .HasForeignKey(d => d.MapRendererId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MapRendererUserSelection_ToMapRenderer");
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

            modelBuilder.Entity<UserMap>(entity =>
            {
                entity.ToTable("UserMap", "gis");

                entity.HasIndex(e => e.CreatedBy)
                    .HasName("IX_Map_CreatedBy");

                entity.HasIndex(e => e.ParentFolderId);

                entity.HasIndex(e => e.UserMapName);

                entity.Property(e => e.CreatedTimestamp)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.LastModifiedTimestamp)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UserMapName)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.UserMapCreatedByNavigation)
                    .HasForeignKey(d => d.CreatedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserMap_ToSystemUser_CreatedBy")
                    .IsRequired(false);

                entity.HasOne(d => d.LastModifiedByNavigation)
                    .WithMany(p => p.UserMapLastModifiedByNavigation)
                    .HasForeignKey(d => d.LastModifiedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserMap_ToSystemUser_LastModifiedBy")
                    .IsRequired(false);

                entity.HasOne(d => d.ParentFolder)
                    .WithMany(p => p.UserMap)
                    .HasForeignKey(d => d.ParentFolderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserMap_ToFolder");
            });

            modelBuilder.Entity<UserMapCategory>(entity =>
            {
                entity.ToTable("UserMapCategory", "gis");

                entity.HasIndex(e => e.CategoryName);

                entity.Property(e => e.CategoryDescription)
                    .IsRequired()
                    .HasMaxLength(2048);

                entity.Property(e => e.CategoryName)
                    .IsRequired()
                    .HasMaxLength(256);
            });

            modelBuilder.Entity<UserMapCategoryUserMap>(entity =>
            {
                entity.HasKey(e => new { e.UserMapCategoryId, e.UserMapId })
                    .HasName("PK_UserMapCategory_UserMap_UserMapCategoryIdUserMapId");

                entity.ToTable("UserMapCategory_UserMap", "gis");

                entity.HasOne(d => d.UserMapCategory)
                    .WithMany(p => p.UserMapCategoryUserMap)
                    .HasForeignKey(d => d.UserMapCategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserMapCategory_UserMap_ToUserMapCategory");

                entity.HasOne(d => d.UserMap)
                    .WithMany(p => p.UserMapCategoryUserMap)
                    .HasForeignKey(d => d.UserMapId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserMapCategory_UserMap_ToUserMap");
            });

            modelBuilder.Entity<UserMapSelection>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("PK_UserMapSelection_UserId");

                entity.ToTable("UserMapSelection", "gis");

                entity.Property(e => e.UserId).ValueGeneratedNever();

                entity.HasOne(d => d.UserMap)
                    .WithMany(p => p.UserMapSelection)
                    .HasForeignKey(d => d.UserMapId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserMapSelection_ToUserMap");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
