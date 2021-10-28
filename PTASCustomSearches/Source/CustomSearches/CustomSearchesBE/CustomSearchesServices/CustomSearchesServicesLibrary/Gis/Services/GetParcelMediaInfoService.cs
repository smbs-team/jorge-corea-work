namespace CustomSearchesServicesLibrary.Gis.Services
{
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Dynamic;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.Gis;
    using CustomSearchesServicesLibrary.CustomSearches;
    using CustomSearchesServicesLibrary.ServiceFramework;

    /// <summary>
    /// Gets the parcel media info.
    /// </summary>
    public class GetParcelMediaInfoService : BaseService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetParcelMediaInfoService"/> class.
        /// </summary>
        /// <param name="serviceContext">The service context.</param>
        public GetParcelMediaInfoService(IServiceContext serviceContext)
            : base(serviceContext)
        {
        }

        /// <summary>
        /// Gets the parcel media info.
        /// </summary>
        /// <param name="dbContext">Database context.</param>
        /// <param name="parcelId">The parcel identifier.</param>
        /// <returns>
        /// The media info for a given parcel.
        /// </returns>
        public async Task<object[]> GetParcelMediaInfoAsync(GisDbContext dbContext, string parcelId)
        {
            List<object> results = new List<object>();

            string unionQuery =
                $"SELECT distinct M.[ptas_mediarepositoryid] as MediaId, M.[ptas_rootfolder] + M.[ptas_blobpath] + '.' + M.[ptas_fileextension] as MediaPath , M.[ptas_primary] as IsPrimary, M.[ptas_order] as ImageOrder, P.ptas_parceldetailid as ParcelId" +
                $"	FROM[dynamics].[ptas_mediarepository] AS M  " +
                $"    INNER JOIN[dynamics].[ptas_parceldetail_ptas_mediarepository] AS PM ON M.ptas_mediarepositoryid = PM.ptas_mediarepositoryid  " +
                $"    INNER JOIN[dynamics].[ptas_parceldetail] AS P ON PM.ptas_parceldetailid = P.ptas_parceldetailid AND P.ptas_name = '{parcelId}'  " +
                $"  WHERE M.ptas_mediatype = 1 AND M.ptas_imagetype = 1 " +
                $"UNION " +
                $"SELECT distinct M.[ptas_mediarepositoryid] as MediaId, M.[ptas_rootfolder] + M.[ptas_blobpath] + '.' + M.[ptas_fileextension] as MediaPath , M.[ptas_primary] as IsPrimary, M.[ptas_order] as ImageOrder, P.ptas_parceldetailid as ParcelId " +
                $"	FROM[dynamics].[ptas_mediarepository] AS M  " +
                $"    INNER JOIN[dynamics].[ptas_land_ptas_mediarepository] AS L ON M.ptas_mediarepositoryid = L.ptas_mediarepositoryid  " +
                $"    INNER JOIN[dynamics].[ptas_parceldetail] AS P ON L.ptas_landid = P._ptas_landid_value AND P.ptas_name = '{parcelId}'  " +
                $"  WHERE M.ptas_mediatype = 1 AND M.ptas_imagetype = 1 " +
                $"UNION " +
                $"SELECT distinct M.[ptas_mediarepositoryid] as MediaId, M.[ptas_rootfolder] + M.[ptas_blobpath] + '.' + M.[ptas_fileextension] as MediaPath , M.[ptas_primary] as IsPrimary, M.[ptas_order] as ImageOrder, P.ptas_parceldetailid as ParcelId " +
                $"	FROM[dynamics].[ptas_mediarepository] AS M  " +
                $"    INNER JOIN[dynamics].[ptas_buildingdetail_ptas_mediarepository] AS B ON M.ptas_mediarepositoryid = B.ptas_mediarepositoryid  " +
                $"	  INNER JOIN[dynamics].[ptas_buildingdetail] AS BD ON B.ptas_buildingdetailid = BD.ptas_buildingdetailid " +
                $"    INNER JOIN[dynamics].[ptas_parceldetail] AS P ON BD._ptas_parceldetailid_value = P.ptas_parceldetailid AND P.ptas_name = '{parcelId}'  " +
                $"  WHERE M.ptas_mediatype = 1 AND M.ptas_imagetype = 1 " +
                $"UNION " +
                $"SELECT distinct M.[ptas_mediarepositoryid] as MediaId, M.[ptas_rootfolder] + M.[ptas_blobpath] + '.' + M.[ptas_fileextension] as MediaPath , M.[ptas_primary] as IsPrimary, M.[ptas_order] as ImageOrder, P.ptas_parceldetailid as ParcelId " +
                $"	FROM[dynamics].[ptas_mediarepository] AS M  " +
                $"    INNER JOIN[dynamics].[ptas_accessorydetail_ptas_mediarepository] AS A ON M.ptas_mediarepositoryid = A.ptas_mediarepositoryid  " +
                $"	  INNER JOIN[dynamics].[ptas_accessorydetail] AS AD ON A.ptas_accessorydetailid = AD.ptas_accessorydetailid " +
                $"    INNER JOIN[dynamics].[ptas_parceldetail] AS P ON AD._ptas_parceldetailid_value = P.ptas_parceldetailid AND P.ptas_name = '{parcelId}'  " +
                $"  WHERE M.ptas_mediatype = 1 AND M.ptas_imagetype = 1 " +
                $"UNION " +
                $"SELECT distinct M.[ptas_mediarepositoryid] as MediaId, M.[ptas_rootfolder] + M.[ptas_blobpath] + '.' + M.[ptas_fileextension] as MediaPath , M.[ptas_primary] as IsPrimary, M.[ptas_order] as ImageOrder, P.ptas_parceldetailid as ParcelId " +
                $"	FROM[dynamics].[ptas_mediarepository] AS M  " +
                $"    INNER JOIN[dynamics].[ptas_condocomplex_ptas_mediarepository] AS C ON M.ptas_mediarepositoryid = C.ptas_mediarepositoryid  " +
                $"	  INNER JOIN[dynamics].[ptas_condocomplex] AS CD ON C.ptas_condocomplexid = CD.ptas_condocomplexid " +
                $"    INNER JOIN[dynamics].[ptas_parceldetail] AS P ON CD._ptas_parcelid_value = P.ptas_parceldetailid AND P.ptas_name = '{parcelId}'  " +
                $"  WHERE M.ptas_mediatype = 1 AND M.ptas_imagetype = 1 " +
                $"UNION " +
                $"SELECT distinct M.[ptas_mediarepositoryid] as MediaId, M.[ptas_rootfolder] + M.[ptas_blobpath] + '.' + M.[ptas_fileextension] as MediaPath , M.[ptas_primary] as IsPrimary, M.[ptas_order] as ImageOrder, P.ptas_parceldetailid as ParcelId " +
                $"	FROM[dynamics].[ptas_mediarepository] AS M  " +
                $"    INNER JOIN[dynamics].[ptas_condounit_ptas_mediarepository] AS C ON M.ptas_mediarepositoryid = C.ptas_mediarepositoryid  " +
                $"	  INNER JOIN[dynamics].[ptas_condounit] AS CD ON C.ptas_condounitid = CD.ptas_condounitid " +
                $"    INNER JOIN[dynamics].[ptas_parceldetail] AS P ON CD._ptas_parcelid_value = P.ptas_parceldetailid AND P.ptas_name = '{parcelId}' " +
                $"  WHERE M.ptas_mediatype = 1 AND M.ptas_imagetype = 1 ";

            string filterQuery = $"SELECT top 1 * from ({unionQuery}) IM Order by isprimary desc";

            DbConnection connection = dbContext.GetOpenConnection();
            await DynamicSqlStatementHelper.ExecuteReaderWithRetriesAsync(
                this.ServiceContext,
                this.ServiceContext.DataDbContextFactory,
                filterQuery,
                async (command, dataReader) =>
                {
                    results.Clear();
                    while (await dataReader.ReadAsync())
                    {
                        // Create the dynamic result for each row
                        var result = new ExpandoObject() as IDictionary<string, object>;
                        result.Add("MediaId", dataReader["MediaId"]);
                        result.Add("MediaPath", dataReader["MediaPath"]);
                        result.Add("IsPrimary", dataReader["IsPrimary"]);
                        result.Add("ImageOrder", dataReader["ImageOrder"]);
                        results.Add(result);
                    }
                },
                $"Cannot get media info for parcel: '{parcelId}'.");

            return results.ToArray();
        }
    }
}
