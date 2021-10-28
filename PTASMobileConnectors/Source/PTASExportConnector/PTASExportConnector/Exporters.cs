// <copyright file="Exporters.cs" company="King County">
// Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASExportConnector
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.IO.Abstractions;
    using System.Linq;
    using System.Security;
    using ConnectorService;
    using Microsoft.AspNetCore.Http;
    using Newtonsoft.Json.Linq;
    using PTASExportConnector.Exceptions;
    using PTASExportConnector.SDK;

    /// <summary>It has the code for exporting from the device to the back-end.</summary>
    /// <seealso cref="PTASExportConnector.IExporters" />
    public class Exporters : IExporters
    {
        private readonly IDbService dbService;
        private readonly Dictionary<string, string> names;
        private readonly Dictionary<string, string> entityNames;
        private readonly IFileSystem fileSystem;
        private readonly IOdata oData;

        /// <summary>Initializes a new instance of the <see cref="Exporters"/> class.</summary>
        /// <param name="dbService">  It has back-end related code.</param>
        /// <param name="fileSystem">  Abstraction of the file system for easier testing.</param>
        /// <param name="oData">  It has the OData related code.</param>
        public Exporters(IDbService dbService, IFileSystem fileSystem, IOdata oData)
        {
            if (dbService == null)
            {
                throw new ExportConnectorException("Error while initiating the export process.", StatusCodes.Status500InternalServerError, new ArgumentNullException("dbService"));
            }

            if (fileSystem == null)
            {
                throw new ExportConnectorException("Error while initiating the export process.", StatusCodes.Status500InternalServerError, new ArgumentNullException("fileSystem"));
            }

            if (oData == null)
            {
                throw new ExportConnectorException("Error while initiating the export process.", StatusCodes.Status500InternalServerError, new ArgumentNullException("oData"));
            }

            this.dbService = dbService;
            this.fileSystem = fileSystem;
            this.oData = oData;
            this.names = new Dictionary<string, string>()
            {
                { "_ptas_landid_value", "ptas_land" },
                { "_ptas_apartmentregionid_value", "ptas_apartmentregion" },
                { "_ptas_zoningid_value",  "ptas_zoning" },
                { "_ptas_presentuseid_value", "ptas_landuse" },
                { "_ptas_highestandbestuseifvacantid_value", "ptas_landuse" },
                { "_ptas_highestandbestuseasimprovedid_value", "ptas_landuse" },
                { "_ptas_watersystemid_value", "ptas_watersewersystem" },
                { "_ptas_sewersystemid_value", "ptas_watersewersystem" },
                { "_ptas_condocomplexid_value", "ptas_condocomplex" },
                { "_ptas_housingprogramid_value", "ptas_housingprogram" },
                { "_ptas_projectid_value", "ptas_condocomplex" },
                { "_ptas_sectionuse_value", "ptas_buildingsectionuse" },
                { "_ptas_taxdistrictid_value", "ptas_taxdistrict" },
                { "_ptas_parceldetailid_value", "ptas_parceldetail" },
                { "_ptas_propertytypeid_value", "ptas_propertytype" },
                { "_ptas_yearbuiltid_value", "ptas_year" },
                { "_ptas_heatingsystemid_value", "ptas_heatingsystem" },
                { "_ptas_addr1_streetnameid_value", "ptas_streetname" },
                { "_ptas_addr1_streettypeid_value", "ptas_streettype" },
                { "_ptas_addr1_cityid_value", "ptas_city" },
                { "_ptas_addr1_stateid_value", "ptas_stateorprovince" },
                { "_ptas_addr1_zipcodeid_value", "ptas_zipcode" },
                { "_ptas_addr1_countryid_value", "ptas_country" },
                { "_ptas_yearrenovatedid_value", "ptas_year" },
                { "_ptas_buildingsectionuseid_value", "ptas_buildingsectionuse" },
                { "_ptas_effectiveyearid_value", "ptas_year" },
                { "_ptas_designationtypeid_value", "ptas_designationtype" },
                { "_ptas_environmentalrestrictiontypeid_value", "ptas_environmentalrestrictiontype" },
                { "_ptas_nuisancetypeid_value", "ptas_nuisancetype" },
                { "_ptas_inspectionyearid_value", "ptas_year" },
                { "_ptas_areaid_value", "ptas_area" },
                { "_ptas_neighborhoodid_value", "ptas_neighborhood" },
                { "_ptas_viewtypeid_value", "ptas_viewtype" },
                { "_ptas_zoningtypeid_value", "ptas_zoning" },
                { "_ptas_economicunitid_value", "ptas_economicunit" },
                { "_ptas_personalpropertyid_value", "ptas_personalproperty" },
                { "_ptas_condounitid_value", "ptas_condounit" },
                { "_ptas_buildingdetailid_value", "ptas_building" },
                { "_ptas_valuationunitid_value", "ptas_valuationunit" },
                { "_ptas_res_accessorytypeid_value", "ptas_residentialaccessorytype" },
                { "_ptas_com_accessorytypeid_value", "ptas_commercialaccessorytype" },
                { "_ptas_responsibilityid_value", "ptas_responsibility" },
                { "_ptas_subareaid_value", "ptas_subarea" },
                { "_ptas_qstrid_value", "ptas_qstr" },
                { "_ptas_levycodeid_value", "ptas_levycode" },
                { "_ptas_taxaccountid_value", "ptas_taxaccount" },
                { "_ptas_unitid_value", "ptas_condounit" },
            };
            this.entityNames = new Dictionary<string, string>()
            {
                { "Apartment_Region", "ptas_apartmentregions" },
                { "Dock", "ptas_projectdocks" },
                { "Economic_Unit", "ptas_economicunits" },
                { "Housing_Program", "ptas_housingprograms" },
                { "Land_Waterfront", "ptas_landwaterfronts" },
                { "Parking_District", "ptas_parkingdistricts" },
                { "Condo_Complex", "ptas_condocomplexs" },
                { "Section_Use_Type", "ptas_buildingsectionuses" },
                { "Unit", "ptas_condounits" },
                { "Apartment_Super_Group", "ptas_apartmentsupergroups" },
                { "Land", "ptas_lands" },
                { "Low_Income_Housing_Program", "ptas_lowincomehousingprograms" },
                { "Section_Use_Square_Foot", "ptas_sectionusesqfts" },
                { "Zoning", "ptas_zonings" },
                { "Building", "ptas_buildingdetails" },
                { "Land_Designation", "ptas_landdesignations" },
                { "Land_Environmental", "ptas_environmentalrestrictions" },
                { "Land_Nuisance", "ptas_landnuisances" },
                { "Land_Schedule", "ptas_landschedules" },
                { "Land_Submerged", "ptas_landsubmergeds" },
                { "Land_Value_Calculation", "ptas_landvaluecalculations" },
                { "Land_View", "ptas_landviews" },
                { "Land_Zoning", "ptas_landzonings" },
                { "Parcel_Economic_Unit", "ptas_parceleconomicunits" },
                { "Tax_account", "ptas_taxaccounts" },
                { "Valuation_Unit", "ptas_valuationunits" },
                { "Accessory", "ptas_accessorydetails" },
                { "Building_Section_Use", "ptas_buildingdetail_commercialuses" },
                { "Parcel", "ptas_parceldetails" },
                { "Valuation_Unit_Detail", "ptas_valuationunitdetails" },
                { "Building_Section_Feature", "ptas_buildingsectionfeatures" },
            };

            var crmUri = Environment.GetEnvironmentVariable("crmUri");
            var authUri = Environment.GetEnvironmentVariable("authUri");
            var clientId = Environment.GetEnvironmentVariable("clientId");
            var clientSecret = Environment.GetEnvironmentVariable("clientSecret");

            this.oData.Init(crmUri, authUri, clientId, clientSecret);
        }

        /// <summary>Exports the retrieved data from the related device GUID and entity name.</summary>
        /// <param name="connector">Used to get the entity data.</param>
        /// <param name="deviceGuid">The device GUID to filter.</param>
        /// <param name="entityName">The name of the entity to retrieve the data.</param>
        /// <returns>A list of change-sets ids.</returns>
        public List<string> Export(IConnector connector, Guid deviceGuid, string entityName)
        {
            if (connector == null)
            {
                throw new ExportConnectorException("Error during the export process.", StatusCodes.Status400BadRequest, new ArgumentNullException("connector"));
            }

            if (deviceGuid == Guid.Empty)
            {
                throw new ExportConnectorException("Error during the export process.", StatusCodes.Status400BadRequest, new ArgumentException("Device GUID is empty."));
            }

            if (string.IsNullOrEmpty(entityName))
            {
                throw new ExportConnectorException("Error during the export process.", StatusCodes.Status400BadRequest, new ArgumentException("Entity name is null or empty."));
            }

            var webapiurl = Environment.GetEnvironmentVariable("webapiurl");

            if (string.IsNullOrEmpty(webapiurl))
            {
                throw new ExportConnectorException("Error during the export process.", StatusCodes.Status400BadRequest, new ArgumentException("webapiurl is null or empty."));
            }

            var changesetIds = new List<string>();
            var model = this.dbService.GetDatabaseModel();
            var entityDefinition = model.Entities.Where(e => e.Name == entityName).FirstOrDefault();

            var entityList = connector.GetModifiedEntityData(null, entityName, deviceGuid);
            var fieldName = entityDefinition.Attributes.Where(a => a.IsClientKey == true).Select(a => a.Name).FirstOrDefault().ToString();

            foreach (var entity in entityList)
            {
                var json = this.CreateJson(entity, entityDefinition);

                if ((string)entity["rowStatus_mb"] == "I")
                {
                    var response = this.oData.Create(webapiurl + this.entityNames[entityName], json, string.Empty);
                    var r = response.Result.Content.ReadAsStringAsync().Result;
                    var guidIncomplete = r.Split("(")[1];
                    var guid = guidIncomplete.Replace(")", string.Empty);
                    this.dbService.UpdateEntityKeys((Guid)entity["guid_mb"], entityName, fieldName, Guid.Parse(guid));
                }
                else if ((string)entity["rowStatus_mb"] == "U")
                {
                    var primaryKey = this.GetGuid((Guid)entity["guid_mb"], entityName, fieldName);
                    this.oData.Update(webapiurl + this.entityNames[entityName], json, primaryKey);
                }
                else
                {
                    var primaryKey = this.GetGuid((Guid)entity["guid_mb"], entityName, fieldName);
                    this.oData.Delete(webapiurl + this.entityNames[entityName] + "(" + primaryKey + ")");
                }

                if (!changesetIds.Contains(entity["changesetId_mb"].ToString()))
                {
                    changesetIds.Add(entity["changesetId_mb"].ToString());
                }
            }

            return changesetIds;
        }

        /// <summary>Gets the list of entities to process.</summary>
        /// <param name="route">Location of the .JSON containing the entities data.</param>
        /// <returns>A IList of Entity objects.</returns>
        public IList<Entity> GetEntityList(string route)
        {
            if (string.IsNullOrEmpty(route))
            {
                throw new ExportConnectorException("Error while trying to get the entity list, the path to the file is null or empty.", StatusCodes.Status400BadRequest, new ArgumentException("route is null or empty."));
            }

            try
            {
                var fileManager = this.fileSystem.File;
                JObject o = JObject.Parse(fileManager.ReadAllText(Path.Combine(route, @"\EntityList.json")));
                JArray a = (JArray)o["Entities"];
                return a.ToObject<IList<Entity>>();
            }
            catch (DirectoryNotFoundException ex)
            {
                throw new ExportConnectorException("Error while getting the entity list, the provided directory was not found.", StatusCodes.Status400BadRequest, ex.InnerException);
            }
            catch (FileNotFoundException ex)
            {
                throw new ExportConnectorException("Error while getting the entity list, the JSON was not found.", StatusCodes.Status400BadRequest, ex.InnerException);
            }
            catch (NotSupportedException ex)
            {
                throw new ExportConnectorException("Error while getting the entity list, the provided path is in an invalid format.", StatusCodes.Status400BadRequest, ex.InnerException);
            }
            catch (SecurityException ex)
            {
                throw new ExportConnectorException("Error while getting the entity list, the caller does not have the required permission.", StatusCodes.Status400BadRequest, ex.InnerException);
            }
            catch (Exception ex)
            {
                throw new ExportConnectorException("Error while getting the entity list.", StatusCodes.Status400BadRequest, ex.InnerException);
            }
        }

        private string CreateJson(Dictionary<string, object> rows, EntityModel entityDefinition)
        {
            JObject o = new JObject();

            foreach (var pair in rows)
            {
                if (pair.Key.Contains("_value"))
                {
                    continue;
                }

                if (pair.Key.Contains("fk_r_"))
                {
                    var relationship = entityDefinition.Relationships.Where(r => r.Name == pair.Key.Replace("fk_", string.Empty)).FirstOrDefault();
                    var keyName = this.GetGuid((Guid)pair.Value, relationship.DestinationEntity, relationship.DestinationKey);

                    o.Add(relationship.SourceKey + "@odata.bind", "/" + this.entityNames[relationship.DestinationEntity] + "(" + keyName + ")");
                }
                else
                {
                    var attribute = entityDefinition.Attributes.Where(a => a.Name == pair.Key).FirstOrDefault();
                    switch (attribute.AttributeType)
                    {
                        case "Integer 16":
                        case "Integer 32":
                            o.Add(pair.Key, (int)pair.Value);
                            break;
                        case "Integer 64":
                            o.Add(pair.Key, (long)pair.Value);
                            break;
                        case "Double":
                            o.Add(pair.Key, (float)pair.Value);
                            break;
                        case "Boolean":
                            o.Add(pair.Key, (bool)pair.Value);
                            break;
                        case "GUID":
                            o.Add(pair.Key, pair.Value.ToString());
                            break;
                        default:
                            o.Add(pair.Key, (string)pair.Value);
                            break;
                    }
                }
            }

            return o.ToString();
        }

        private string GetGuid(Guid entityGuid, string entityKind, string keyName)
        {
            var guid = this.dbService.GetEntityKeys(entityGuid, entityKind);
            return guid[keyName].ToString();
        }
    }
}
