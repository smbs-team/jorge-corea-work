// <copyright file="Utility.cs" company="King County">
// Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASketchFileMigratorConsoleApp
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.IO.Abstractions;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using MoreLinq;
    using Newtonsoft.Json;
    using PTASExportConnector.SDK;
    using PTASketchFileMigratorConsoleApp.Constants;
    using PTASketchFileMigratorConsoleApp.Types;
    using Serilog;

    /// <summary>Utility methods.</summary>
    public class Utility : IUtility
    {
        private readonly IFileSystem fileSystem;
        private readonly IOdata odata;
        private readonly Dictionary<string, string> typeNames;
        private readonly FileType identifiers;
        private readonly string[] accessoryTypes;
        private List<string> invalidFiles;

        /// <summary>Initializes a new instance of the <see cref="Utility"/> class.</summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="odata">Odata methods.</param>
        public Utility(IFileSystem fileSystem, IOdata odata)
        {
            this.fileSystem = fileSystem;
            this.odata = odata;
            this.OfficialFiles = this.LoadOfficialList();
            this.invalidFiles = this.LoadInvalidFilesList();
            try
            {
                this.identifiers = JsonConvert.DeserializeObject<FileType>(this.fileSystem.File.ReadAllText(@"./fileData.json"));
            }
            catch (Exception)
            {
                throw;
            }

            this.typeNames = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            this.identifiers.DrawingIds.ForEach(s => this.typeNames.Add(s.Key.Trim(), s.Value.Trim()));
            this.accessoryTypes = this.identifiers.AccessoryDrawingIDs.Select(s => s.ToLower().Trim()).ToArray();
        }

        /// <summary>Gets or sets the official files.</summary>
        /// <value>The official files.</value>
        public List<OfficialFile> OfficialFiles { get; set; }

        /// <inheritdoc/>
        public async Task RenameFiles(IEnumerable<IFileInfo> fileArray)
        {
            if (fileArray == null)
            {
                return;
            }

            try
            {
                foreach (var file in fileArray)
                {
                    var fileData = this.ChopFileName(file);

                    if (string.IsNullOrEmpty(fileData.EntityName))
                    {
                        Log.Information($"[{fileData.EntityName} not implemented] Skipped {fileData.FullName}");
                        continue;
                    }

                    var isOfficial = this.OfficialFiles.FindIndex(f => f.FileName == fileData.FullName && f.Identifier == fileData.Identifier);

                    if (isOfficial != -1)
                    {
                        continue;
                    }

                    var result = await this.GetSketchId(fileData);

                    if (!result.IsValid)
                    {
                        continue;
                    }

                    this.fileSystem.File.Move(file.FullName, $"{file.DirectoryName}/{result.SketchId}.xml");
                    this.OfficialFiles.Add(new OfficialFile { FileName = fileData.FullName, SketchName = result.SketchId, Identifier = fileData.Identifier, EntityResult = result });
                }
            }
            finally
            {
                this.ExportToJson(this.OfficialFiles, $"./Logs/officialFiles.json");
            }
        }

        /// <inheritdoc/>
        public IEnumerable<IFileInfo> FilterByValidFiles(IEnumerable<IFileInfo> arrayToFilter)
        {
            Log.Information("Filtering by valid files...");
            var types = string.Join(string.Empty, this.typeNames.Keys.Select(k => k + @"\d*|").ToArray());
            var types2 = string.Join(string.Empty, this.accessoryTypes.Select(a => a + @"\d*|").ToArray());
            var regex = new Regex(@"\b\d{6}-\d{4}(|" + types.Remove(types.Length - 1) + "|" + types2.Remove(types2.Length - 1) + @").vcd\b", RegexOptions.IgnoreCase);

            var validFiles = new List<IFileInfo>();

            foreach (var file in arrayToFilter)
            {
                if (regex.IsMatch(file.Name))
                {
                    validFiles.Add(file);
                }
                else
                {
                    this.invalidFiles.Add(file.Name);
                }
            }

            this.ExportToJson(this.invalidFiles, $"./Logs/invalidFiles.json");
            return this.FilterBuildings(validFiles);
        }

        private IEnumerable<IFileInfo> FilterBuildings(IEnumerable<IFileInfo> arrayToFilter)
        {
            var regex = new Regex(@"\b\d{6}-\d{4}(|b|b1).vcd\b", RegexOptions.IgnoreCase);

            var newList = arrayToFilter.Where(f => !regex.IsMatch(f.Name)).ToList();
            var filtered = arrayToFilter.Where(f => regex.IsMatch(f.Name));

            foreach (var file in filtered)
            {
                var choppedFile = this.ChopFileName(file);
                var regex2 = new Regex(@"\b" + $"{choppedFile.Mayor}-{choppedFile.Minor}" + @"(|b|b1).vcd\b", RegexOptions.IgnoreCase);
                var result = newList.Any(f => regex2.IsMatch(f.Name));
                if (result)
                {
                    continue;
                }

                var newestFile = filtered.Where(f => f.Name.Contains($"{choppedFile.Mayor}-{choppedFile.Minor}")).OrderByDescending(f => f.LastWriteTime).Take(1).FirstOrDefault();
                newList.Add(newestFile);
            }

            Log.Information($"Filter: {newList.Count()} files.");
            return newList;
        }

        private FileNameMetadata ChopFileName(IFileInfo file)
        {
            var modifiedDate = this.fileSystem.File.GetLastWriteTime(file.FullName);
            var fileName = this.fileSystem.Path.GetFileNameWithoutExtension(file.Name);

            if (fileName.Length < 11)
            {
                return null;
            }

            var mayor = fileName.Substring(0, 6);
            var minor = fileName.Substring(7, 4);
            var type = fileName.Length > 11 ? fileName.Substring(11) : string.Empty;
            var number = string.Empty;
            if (!string.IsNullOrEmpty(type))
            {
                number = new string(type.Where(char.IsDigit).ToArray());
                type = new string(type.Where(char.IsLetter).ToArray());
            }

            if (string.IsNullOrEmpty(number))
            {
                number = "1";
            }

            return new FileNameMetadata(this.fileSystem) { Mayor = mayor, Minor = minor, EntityName = type, Number = number, FullName = fileName, ModifiedDate = modifiedDate, Name = $"{mayor}-{minor}" };
        }

        private async Task<EntityResult> GetSketchId(FileNameMetadata fileData)
        {
            switch (fileData.EntityName)
            {
                case EntityNames.Building:
                    var buildingQuery = $"$select=ptas_sketchid&$expand=ptas_buildingid($select=ptas_buildingdetailid)&$filter=ptas_parcelid/ptas_major eq '{fileData.Mayor}' and ptas_parcelid/ptas_minor eq '{fileData.Minor}' and (ptas_buildingid/ptas_buildingnbr eq {fileData.Number} or ptas_buildingid/ptas_name eq '{fileData.Mayor}-{fileData.Minor} - {fileData.Number}') and ptas_buildingid/statecode eq 0 and ptas_buildingid/statuscode eq 1 and contains(ptas_tags, 'Migrated') and ptas_iscomplete eq true and ptas_isofficial eq false and statecode eq 0 and statuscode eq 1&$orderby=ptas_version desc&$top=1";
                    var buildingRes = await this.odata.Get(EntityNames.Sketch, buildingQuery);

                    if (buildingRes.Value == null || buildingRes.Value.Length == 0)
                    {
                        this.LogToFile($"[{EntityNames.Building}] {fileData.FullName} not found in database.");
                        return new EntityResult { IsValid = false };
                    }

                    return new EntityResult { SketchId = buildingRes.Value.FirstOrDefault().SketchId, Id = buildingRes.Value.FirstOrDefault().Building.Id, EntityName = EntityNames.Building, IsValid = true };
                case EntityNames.Unit:
                    var unitQuery = $"$select=ptas_sketchid, ptas_version&$expand=ptas_unitid($select=ptas_condounitid, ptas_accounttype)&$filter=contains(ptas_tags, 'Migrated') and ptas_iscomplete eq true and ptas_isofficial eq false and statecode eq 0 and statuscode eq 1 and ptas_unitid/ptas_parcelid/ptas_major eq '{fileData.Mayor}' and ptas_unitid/ptas_parcelid/ptas_minor eq '{fileData.Minor}' and ptas_unitid/statecode eq 0 and ptas_unitid/statuscode eq 1 and ptas_unitid/ptas_unittype eq 668020001 and (ptas_unitid/ptas_unitnumbertext eq '%23{fileData.Number}' or ptas_unitid/ptas_unitnumbertext eq '%230{fileData.Number}' or ptas_unitid/ptas_unitnumbertext eq '%2300{fileData.Number}' or ptas_unitid/ptas_unitnumbertext eq '{fileData.Number}')";

                    var unitResponse = await this.odata.Get(EntityNames.Sketch, unitQuery);

                    if (unitResponse.Value == null || unitResponse.Value.Length == 0)
                    {
                        this.LogToFile($"[{EntityNames.Unit}] {fileData.FullName} not found in database.");
                        return new EntityResult { IsValid = false };
                    }

                    var results = unitResponse.Value.Length;

                    if (results == 1)
                    {
                        return new EntityResult { SketchId = unitResponse.Value.FirstOrDefault().SketchId, Id = unitResponse.Value.FirstOrDefault().CondoUnit.Id, EntityName = EntityNames.Unit, IsValid = true };
                    }

                    // check if all objects have same unit id
                    if (unitResponse.Value.All(s => s.CondoUnit.Id == unitResponse.Value[0].CondoUnit.Id))
                    {
                        // return the highest version
                        var item = unitResponse.Value.MaxBy(s => int.Parse(s.Version));
                        return new EntityResult { SketchId = item.FirstOrDefault().SketchId, Id = item.FirstOrDefault().CondoUnit.Id, EntityName = EntityNames.Unit, IsValid = true };
                    }
                    else
                    {
                        var fileId = fileData.Identifier == "mr" ? AccountTypes.RealProperty : AccountTypes.PersonalProperty;
                        var filteredUnits = unitResponse.Value.Where(s => s.CondoUnit.AccountType == fileId);
                        var totalFilteredUnits = filteredUnits.Count();
                        if (totalFilteredUnits == 1)
                        {
                            return new EntityResult { SketchId = filteredUnits.FirstOrDefault().SketchId, Id = filteredUnits.FirstOrDefault().CondoUnit.Id, EntityName = EntityNames.Unit, IsValid = true };
                        }
                        else if (totalFilteredUnits > 1)
                        {
                            var firstUnit = filteredUnits.FirstOrDefault();
                            var sketchToUse = this.GetUnitSketch(unitResponse, firstUnit);
                            if (!sketchToUse.IsValid)
                            {
                                this.LogToFile($"[{EntityNames.Unit}] {fileData.FullName} multiple units of type {fileId} found, sketch id used: {sketchToUse}");
                            }

                            return sketchToUse;
                        }
                        else if (totalFilteredUnits == 0)
                        {
                            var firstUnit = unitResponse.Value.FirstOrDefault();
                            var sketchToUse = this.GetUnitSketch(unitResponse, firstUnit);
                            if (!sketchToUse.IsValid)
                            {
                                this.LogToFile($"[{EntityNames.Unit}] {fileData.FullName} no account types found, sketch id used: {sketchToUse}");
                            }

                            return sketchToUse;
                        }
                    }

                    return new EntityResult { IsValid = false };
                case EntityNames.Accessory:
                    var accQuery = $"$select=ptas_sketchid, ptas_version&$expand=ptas_accessoryid($select=ptas_resaccessorytype, ptas_commaccessorytype, ptas_name, ptas_accessorydetailid)&$filter=contains(ptas_tags, 'Migrated') and ptas_iscomplete eq true and ptas_isofficial eq false and statecode eq 0 and statuscode eq 1 and ptas_accessoryid/ptas_parceldetailid/ptas_major eq '{fileData.Mayor}' and ptas_accessoryid/ptas_parceldetailid/ptas_minor eq '{fileData.Minor}' and ptas_accessoryid/statecode eq 0 and ptas_accessoryid/statuscode eq 1";
                    var accRes = await this.odata.Get(EntityNames.Sketch, accQuery);

                    if (fileData.AccType == AccessoryTypes.Residential)
                    {
                        var filteredAccessories = accRes.Value.Where(a => a.Accessory.ResidentialType == fileData.AccAttributeValue);
                        return this.GetAccSketch(filteredAccessories, fileData);
                    }
                    else if (fileData.AccType == AccessoryTypes.Commercial)
                    {
                        var filteredAccessories = accRes.Value.Where(a => a.Accessory.CommercialType == fileData.AccAttributeValue);
                        return this.GetAccSketch(filteredAccessories, fileData);
                    }

                    this.LogToFile($"[{EntityNames.Accessory}] {fileData.FullName} No associated account type found.");
                    return new EntityResult { IsValid = false };
                default:
                    return new EntityResult { IsValid = false };
            }
        }

        private EntityResult GetAccSketch(IEnumerable<Sketch> filteredAccessories, FileNameMetadata fileData)
        {
            var filteredAccCount = filteredAccessories.Count();
            if (filteredAccCount == 0)
            {
                return new EntityResult { IsValid = false };
            }
            else if (filteredAccCount == 1)
            {
                return new EntityResult { SketchId = filteredAccessories.FirstOrDefault().SketchId, Id = filteredAccessories.FirstOrDefault().Accessory.Id, EntityName = EntityNames.Accessory, IsValid = true };
            }
            else if (filteredAccCount > 1)
            {
                var filteredByName = filteredAccessories.Where(f => f.Accessory.Name.ToLower().Trim() == $"accessory {fileData.Number}").FirstOrDefault();
                if (filteredByName == null)
                {
                    var chosenAcc = filteredAccessories.ElementAtOrDefault(int.Parse(fileData.Number) - 1);
                    if (chosenAcc != null)
                    {
                        this.LogToFile($"[{EntityNames.Accessory}] {fileData.FullName} ptas_name not found, sketch id used: {chosenAcc.SketchId}");
                        return new EntityResult { SketchId = chosenAcc.SketchId, Id = chosenAcc.Accessory.Id, EntityName = EntityNames.Accessory, IsValid = true };
                    }

                    return new EntityResult { IsValid = false };
                }

                return new EntityResult { SketchId = filteredByName.SketchId, Id = filteredByName.Accessory.Id, EntityName = EntityNames.Accessory, IsValid = true };
            }

            return new EntityResult { IsValid = false };
        }

        private EntityResult GetUnitSketch(OdataResponse array, Sketch unitToFilter)
        {
            var unitSketches = array.Value.Where(u => u.CondoUnit.Id == unitToFilter.CondoUnit.Id);
            var totalSketches = unitSketches.Count();
            if (totalSketches == 1)
            {
                return new EntityResult { SketchId = unitSketches.FirstOrDefault().SketchId, Id = unitSketches.FirstOrDefault().CondoUnit.Id, EntityName = EntityNames.Unit, IsValid = true };
            }
            else if (totalSketches > 1)
            {
                var item = unitSketches.MaxBy(s => int.Parse(s.Version));
                return new EntityResult { SketchId = item.FirstOrDefault().SketchId, Id = item.FirstOrDefault().CondoUnit.Id, EntityName = EntityNames.Unit, IsValid = true };
            }

            return new EntityResult { IsValid = false };
        }

        private void LogToFile(string message)
        {
            this.fileSystem.File.AppendAllText($"./Logs/nameChangeErrors.txt", message + Environment.NewLine);
        }

        private void ExportToJson(object content, string route)
        {
            string json = JsonConvert.SerializeObject(content, Formatting.Indented);
            var trimmedRoute = route.Substring(0, route.LastIndexOf('/'));

            if (!this.fileSystem.Directory.Exists(trimmedRoute))
            {
                this.fileSystem.Directory.CreateDirectory(trimmedRoute);
            }

            this.fileSystem.File.WriteAllText(route, json);
        }

        private List<OfficialFile> LoadOfficialList()
        {
            try
            {
                return JsonConvert.DeserializeObject<List<OfficialFile>>(this.fileSystem.File.ReadAllText(@"./Logs/officialFiles.json"));
            }
            catch (Exception ex)
            {
                if (ex is FileNotFoundException || ex is DirectoryNotFoundException)
                {
                    return new List<OfficialFile>();
                }

                throw;
            }
        }

        private List<string> LoadInvalidFilesList()
        {
            try
            {
                return JsonConvert.DeserializeObject<List<string>>(this.fileSystem.File.ReadAllText(@"./Logs/invalidFiles.json"));
            }
            catch (Exception ex)
            {
                if (ex is FileNotFoundException || ex is DirectoryNotFoundException)
                {
                    return new List<string>();
                }

                throw;
            }
        }
    }
}
