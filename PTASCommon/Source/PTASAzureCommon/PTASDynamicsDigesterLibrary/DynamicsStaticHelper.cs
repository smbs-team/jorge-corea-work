// <copyright file="DynamicsStaticHelper.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>
namespace PTASDynamicsDigesterLibrary
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using D2SSyncHelpers.Services;

    using Microsoft.Azure.Services.AppAuthentication;
    using Microsoft.IdentityModel.Clients.ActiveDirectory;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    using PTASDynamicsDigesterLibrary.Classes;

    using PTASServicesCommon.TokenProvider;

    /// <summary>
    /// Helper methods for dynamics processing.
    /// </summary>
    public static class DynamicsStaticHelper
    {
        private const string ConnectionStringPasswordSection = "password";

        private const string NoValue = "NOVALUE";

        /// <summary>
        /// Attempt to create access to the database.
        /// </summary>
        /// <param name="conStr">Connections String.</param>
        /// <param name="principalCredentials">Principal credentials.</param>
        /// <returns>Access if possible.</returns>
        public static DataAccessLibrary CreateDBAccess(string conStr, ClientCredential principalCredentials)
        {
            string token = null;
            if (!IsLocalStr(conStr))
            {
                var tokenProvider = new AzureTokenProvider();
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
            }

            return new DataAccessLibrary(conStr, token);
        }

        /// <summary>
        /// Attempts to retrieve an entity from received message.
        /// </summary>
        /// <param name="json">JSON string.</param>
        /// <returns>Mapped entity.</returns>
        public static ReceivedEntityInfo GetDynamicsMessage(string json) =>
            JsonConvert.DeserializeObject<ReceivedEntityInfo>(
            JsonConvert.DeserializeObject(json).ToString());

        /// <summary>
        /// Returns the name for this field. If it is a related table it will rename the field
        /// appropiately.
        /// </summary>
        /// <param name="keyVal">Key value to examine.</param>
        /// <returns>The field name.</returns>
        public static string GetFieldNameFrom(KeyValLower keyVal) =>
                            keyVal.Value is JObject extractedObject && extractedObject.HasValues && extractedObject["Id"] != null
                                ? $"_{keyVal.Key}_value"
                                : $"{keyVal.Key}";

        /// <summary>
        /// Gets the sql insert script.
        /// </summary>
        /// <param name="receivedInfo">Into to process.</param>
        /// <param name="validFields">List of valid fields.</param>
        /// <returns>SQL insert script.</returns>
        public static string GetInsertScript(ReceivedInfoSimplfier receivedInfo, IEnumerable<string> validFields)
        {
            var allFields = receivedInfo.AllValues
                .Select(field => (Key: GetFieldNameFrom(field), Value: $"'{GetValueFrom(field.Value)}'"))
                .Where(keyVal => !keyVal.Value.Contains(NoValue))
                .Join(
                    inner: validFields,
                    outerKeySelector: keyVal => keyVal.Key,
                    innerKeySelector: validField => validField,
                    resultSelector: (keyVal, validField) => keyVal);
            if (!allFields.Any())
            {
                throw new Exception("There were no fields to update! Check SQL table.");
            }

            var insertFields = string.Join(",", allFields.Select(f => f.Key));
            var insertValues = string.Join(",", allFields.Select(f => f.Value));
            return $"insert into dynamics.{receivedInfo.PrimaryEntityName} ({insertFields}) values ({insertValues})";
        }

        /// <summary>
        /// Gets sql update script.
        /// </summary>
        /// <param name="receivedInfo">Info to analyze.</param>
        /// <param name="validFields">Valid field names.</param>
        /// <returns>SQL Script.</returns>
        public static string GetUpdateScript(ReceivedInfoSimplfier receivedInfo, IEnumerable<string> validFields)
        {
            IEnumerable<KeyValLower> values = receivedInfo.AllValues
                .Where(f => f.Key != receivedInfo.GetPrimaryKeyName()).ToList();
            if (!values.Any())
            {
                throw new Exception("There were no fields to update! Check SQL table.");
            }

            var fields = values
                .Select(field => (Key: GetFieldNameFrom(field), Value: $"'{GetValueFrom(field.Value)}'"))
                .Join(
                    inner: validFields,
                    outerKeySelector: keyVal => keyVal.Key,
                    innerKeySelector: validField => validField,
                    resultSelector: (keyVal, validField) => keyVal)
                .Select(keyVal => $"{keyVal.Key}={keyVal.Value}")
                .Where(resultStr => !resultStr.Contains(NoValue));
            var allFields = string.Join(", ", fields);
            string primaryKeyName = receivedInfo.GetPrimaryKeyName();
            var q = $@"update dynamics.{receivedInfo.PrimaryEntityName} set {allFields} where {primaryKeyName}='{receivedInfo.Id}'";
            return q;
        }

        /// <summary>
        /// Retrieves the value from a dynamics mapped json field.
        /// </summary>
        /// <param name="value">Dynamic value to parse.</param>
        /// <returns>Extracted value.</returns>
        public static string GetValueFrom(dynamic value) =>
            GetValue(value).Replace("'", "''");

        /// <summary>
        /// Attempts tp save record.
        /// </summary>
        /// <param name="dbAccess">DB Access.</param>
        /// <param name="receivedInfo">Info.</param>
        /// <returns>Result on string.</returns>
        public static async Task<string> InsertRecord(DataAccessLibrary dbAccess, ReceivedInfoSimplfier receivedInfo)
        {
            var validFields = await GetValidFields(dbAccess, receivedInfo);
            var insertCmd = GetInsertScript(receivedInfo, validFields);
            try
            {
                await dbAccess.SaveData<object>(insertCmd, null);
                return $"Inserted record.";
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed executing {insertCmd}\n{ex.Message}", ex);
            }
        }

        /// <summary>
        /// Updates a record.
        /// </summary>
        /// <param name="dbAccess">Database access.</param>
        /// <param name="infoToSave">Info.</param>
        /// <returns>Number of records and result.</returns>
        public static async Task<(int records, string result)> UpdateRecord(DataAccessLibrary dbAccess, ReceivedInfoSimplfier infoToSave)
        {
            var validFields = await GetValidFields(dbAccess, infoToSave);
            var toExec = GetUpdateScript(infoToSave, validFields);
            var records = await dbAccess.SaveData<object>(toExec, null);
            return (records, $"Update record script: {toExec}");
        }

        private static async Task<IEnumerable<string>> GetValidFields(DataAccessLibrary dbAccess, ReceivedInfoSimplfier wrapper) =>
            (await new DBTablesService(dbAccess).GetTable(wrapper.PrimaryEntityName))?.Fields.Select(f => f.Name).ToArray();

        private static string GetValue(dynamic value)
        {
            switch (value)
            {
                case JValue v:
                    return v.Value?.ToString() ?? NoValue;

                case JObject obj:
                    {
                        return obj.HasValues
                            ? (obj["Value"] ?? obj["Id"] ?? "NA").ToString()
                            : string.Empty;
                    }

                default:
                    return value.ToString();
            }
        }

        private static bool IsLocalStr(string conStr)
        {
            return conStr.Contains(ConnectionStringPasswordSection, StringComparison.OrdinalIgnoreCase) || conStr.Contains("(local)", StringComparison.OrdinalIgnoreCase);
        }
    }
}