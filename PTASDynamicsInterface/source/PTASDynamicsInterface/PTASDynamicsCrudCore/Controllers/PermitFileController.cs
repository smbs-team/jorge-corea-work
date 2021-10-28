// <copyright file="PermitFileController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace PTASDynamicsCrudCore.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web;

    using Azure.Storage.Blobs;

    using ClosedXML.Excel;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;

    using PTASCRMHelpers;

    using PTASDynamicsCrudHelperClasses.Classes;
    using PTASDynamicsCrudHelperClasses.Interfaces;

    /// <summary>
    /// Upload a permit file to storage.
    /// </summary>
    [Route("v1/api/[controller]")]
    [ApiController]
    public class PermitFileController : GenericDynamicsControllerBase
    {
        private const string BlobContainerName = "permits";
        private const string ErrorCol = "error_col";
        private const string ErrorColTitle = "Errors Found";
        private const int GuidLength = 36;
        private const int InvalidParcel = 591500001;
        private const string NoErrorLabel = "";
        private const string ParcelNotFoundError = "Error: Invalid parcel";
        private const string ParcelRelation = "_ptas_parcelid_value";
        private const string Permitstatus = "ptas_permitstatus";
        private const string PermitType = "ptas_permittype";
        private const int SourceJurisdiction = 591500000;
        private const string TableId = "ptas_permit";

        private static readonly Dictionary<string, string> ColumnMapping = new Dictionary<string, string>()
        {
            { "% complete", "ptas_percentcomplete" },
            { "Assigned to", "ownerid" },
            { "Condo unit", "ptas_condounitid" },
            { "Current jurisdiction", "ptas_currentjurisdiction" },
            { "Description", "ptas_description" },
            { "Error reason", "ptas_errorreason" },
            { "Issue date", "ptas_issueddate" },
            { "Issuing jurisdiction", "ptas_issuedbyid" },
            { "Jurisdiction permit status", Permitstatus },
            { "Latest permit inspection date", "ptas_latestpermitinspectiondate" },
            { "Latest permit inspection type", "ptas_latestpermitinspectiontype" },
            { "Link to permit", "ptas_linktopermit" },
            { "Parcel", ParcelRelation },
            { "Permit number", "ptas_name" },
            { "Permit source", "ptas_permitsource" },
            { "Permit status", "statuscode" },
            { "Permit type", "ptas_permittype" },
            { "Permit value", "ptas_permitvalue" },
            { "Plan ready date", "ptas_planreadydate" },
            { "Plan request", "ptas_planrequest" },
            { "Plan request date", "ptas_planrequestdate" },
            { "Project address", "ptas_projectaddress" },
            { "Project description shortcut", "ptas_projectdescriptionshortcut" },
            { "Project name", "ptas_projectname" },
            { "Reviewed by", "ptas_reviewedbyid" },
            { "Reviewed date", "ptas_revieweddate" },
            { "Send HI exemption postcard", "ptas_qualifiesforhiex" },
            { "Show Inspection History", "ptas_showinspectionhistory" },
        };

        private readonly CRMWrapper crmWrapper;
        private readonly string storageConnectionString;

        /// <summary>
        /// Initializes a new instance of the <see cref="PermitFileController"/> class.
        /// </summary>
        /// <param name="config">Sys config.</param>
        /// <param name="memoryCache">Memory cache.</param>
        /// <param name="tokenManager">Token.</param>
        /// <param name="crmWrapper">Ze wrapper.</param>
        public PermitFileController(IConfigurationParams config, IMemoryCache memoryCache, ITokenManager tokenManager, CRMWrapper crmWrapper)
            : base(config, memoryCache, tokenManager)
        {
            this.storageConnectionString = config.StorageConnectionString;
            this.crmWrapper = crmWrapper;
        }

        private Dictionary<string, int> PermitStatusMapping { get; set; }

        private Dictionary<string, int> PermitTypeMapping { get; set; }

        /// <summary>
        /// Gets a list of items.
        /// </summary>
        /// <param name="id">id of the contact id to store the permit file under.</param>
        /// <param name="form">Form file payload.</param>
        /// <returns>Got items if found.</returns>
        [HttpPost("SaveFile/{id}")]
        public async Task<ActionResult<dynamic>> SaveFile(string id, [FromForm] Payload form)
        {
            try
            {
                GetXLFileFromForm(form, out string fileName, out IXLWorksheet fstSheet);

                // Get field names and titles.
                this.ExtractSheetInfo(fstSheet, out List<string> fieldTitles, out List<string> fieldNames, out List<object[]> rows);

                IEnumerable<string> parcelIdsToCheck = this.ExtractParcelNames(fieldNames, rows);
                var checkMessages = await this.CheckRows(parcelIdsToCheck.ToArray());

                var errorCount = checkMessages.Count(cm => cm.StartsWith(ParcelNotFoundError));

                // Add names and titles for error column.
                fieldNames.Insert(0, ErrorCol);
                fieldTitles.Insert(0, $"{ErrorColTitle} ({errorCount})");

                // Add error column to all rows.
                var newRows = rows.Select((row, rowIndex) => row.Prepend(checkMessages[rowIndex]).ToArray()).ToList();

                XLWorkbook resultingBook = this.CreateNewWorkbook(fieldTitles, newRows);

                var memoryStream = new MemoryStream();
                resultingBook.SaveAs(memoryStream);
                memoryStream.Seek(0, SeekOrigin.Begin);

                string file = await this.SaveToContainer(id, fileName, memoryStream);

                return new OkObjectResult(await Task.FromResult(new
                {
                    file,
                    hasErrors = errorCount > 0,
                    errorCount,
                }));
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    ex.Message,
                    hasErrors = true,
                });
            }
        }

        /// <summary>
        /// Takes info in excel file and saves it to dynamics.
        /// </summary>
        /// <param name="form">Form data.</param>
        /// <returns>Got items if found.</returns>
        [HttpPut("UpdatePermitInfo/")]
        public async Task<ActionResult> UpdatePermitInfo([FromForm] PostPermitInfo form)
        {
            try
            {
                if (form is null || form.JurisdictionId is null || form.FileUrl is null)
                {
                    throw new ArgumentNullException(nameof(form));
                }

                // step 1: Load from blob
                string fileUrl = HttpUtility.UrlDecode(form.FileUrl);
                var lookFor = $"/{BlobContainerName}/";
                var p = fileUrl.IndexOf(lookFor);
                if (p < 0)
                {
                    throw new Exception($"Could not find the blob container name {BlobContainerName} in {fileUrl}");
                }

                var filePath = fileUrl.Substring(p + lookFor.Length);

                BlobClient b = new BlobClient(
                    connectionString: this.storageConnectionString,
                    blobContainerName: BlobContainerName,
                    blobName: filePath);

                var s = await b.OpenReadAsync();

                // step 2: convert stream so excel workbook.
                var wb = new XLWorkbook(s);
                var fstSheet = wb.Worksheets.First();

                // step 3: get info from the sheet.
                this.ExtractSheetInfo(fstSheet, out List<string> fieldTitles, out List<string> fieldNames, out List<object[]> rows);

                // step 4: get parcel id's from the parcel names.
                var parcelNames = this.ExtractParcelNames(fieldNames, rows);
                var parcelIds = await this.GetParcelIds(parcelNames);
                var parcelIndex = fieldNames.IndexOf(ParcelRelation);

                // step 5: add missing fields and check for errors.
                rows = rows.Select((oldRow, i) =>
                {
                    var row = oldRow.ToList();
                    row.Add(form.JurisdictionId);
                    row.Add(SourceJurisdiction); // source: jurisdiction.
                    row.Add(row[parcelIndex]);   // parcel number.
                    string currParcelId = parcelIds[i]; // parcel id to replace with
                    row[parcelIndex] = currParcelId;    // replace parcel name with id.
                    if (string.IsNullOrEmpty(currParcelId))
                    {
                        row.Add(InvalidParcel); // Invalid Parcel error.
                    }

                    return row.ToArray();
                }).ToList();

                // step 6. Add field names for extra fields.
                fieldNames.Add("_ptas_jurisdictionid_value");
                fieldNames.Add("ptas_permitsource");
                fieldNames.Add("ptas_parcelheadername");
                fieldNames.Add("ptas_errorreason"); // must be last as it is optional when no error.

                var result = await this.SaveData(fieldNames.ToArray(), rows.ToArray());

                // step 7. Check for errors.
                int nullParcels = parcelIds.Count(pid => pid is null);
                if (nullParcels > 0)
                {
                    return new OkObjectResult(await Task.FromResult(new
                    {
                        message = $"{nullParcels} parcel(s) not found.",
                        hasErrors = true,
                        result,
                    }));
                }

                // step 8. Delete the original blob.
                await b.DeleteIfExistsAsync();
                return new OkObjectResult(await Task.FromResult(new
                {
                    hasErrors = false,
                    message = $"Updated data and deleted file {form.FileUrl}",
                }));
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    ex.Message,
                    hasErrors = true,
                });
            }
        }

        private static void GetXLFileFromForm(Payload form, out string fileName, out IXLWorksheet fstSheet)
        {
            var formFile = form.File;
            fileName = formFile.FileName.ToLower();

            // Filenames downloaded from the storage have this format:
            // d999a3f1-e8a0-432a-b1fe-73ae2c7289c0_permits.xlsx
            // the original slash is replaced for an underscore, so the
            // following lines prevent the filename to continue to grow.
            int idx = fileName.IndexOf("_");
            if (idx == GuidLength)
            {
                fileName = fileName.Substring(GuidLength + 1);
            }

            string extension = Path.GetExtension(fileName);
            if (extension != ".xlsx")
            {
                throw new Exception($"Must be an excel file (.xlsx). Input file name: {fileName}");
            }

            var fileStream = formFile.OpenReadStream();
            var wb = new XLWorkbook(fileStream);
            fstSheet = wb.Worksheets.First();
        }

        private void AddRowToSheet(IXLWorksheet targetSheet, IEnumerable<object> values, int rowNumber, Action<IXLCell> onCreateCell)
        {
            char col = 'A';
            foreach (var item in values)
            {
                IXLCell cell = targetSheet.Cell($"{col++}{rowNumber}");
                cell.Value = item;
                onCreateCell?.Invoke(cell);
            }
        }

        private async Task<string[]> CheckRows(string[] parcels)
        {
            string[] foundParcels = await this.GetParcelIds(parcels);

            // find task result for this parcel.
            return foundParcels.Select(ptas_parceldetailid =>
                string.IsNullOrEmpty(ptas_parceldetailid) ? ParcelNotFoundError : NoErrorLabel).ToArray();
        }

        private EntityChanges CreateChanges(string permitId, string[] fieldNames, object[] cells)
        {
            var x = cells.Select((cell, index) =>
            (cell, index)).ToDictionary(
                t =>
                {
                    return fieldNames[t.index];
                },
                (t) =>
                {
                    var fieldName = fieldNames[t.index];
                    switch (fieldName)
                    {
                        case "ptas_permitstatus":
                            return this.GetPermitStatus(t.cell);

                        case "ptas_permittype":
                            return this.GetPermitType(t.cell);

                        default:
                            return t.cell;
                    }
                });
            return new EntityChanges
            {
                Changes = x,
                EntityId = permitId,
                EntityName = "ptas_permit",
            };
        }

        private XLWorkbook CreateNewWorkbook(List<string> fieldTitles, List<object[]> newRows)
        {
            var resultingBook = new XLWorkbook();
            var sheet = resultingBook.AddWorksheet();
            this.AddRowToSheet(sheet, fieldTitles, 1, (IXLCell cell) =>
            {
                var s = $"{cell.Value}";
                cell.Style.Font.Bold = true;
                cell.Style.Border.BottomBorder = XLBorderStyleValues.Thick;
                if (s.StartsWith(ErrorColTitle))
                {
                    cell.Style.Fill.BackgroundColor = XLColor.FromArgb(185, 10, 0);
                    cell.Style.Font.FontColor = XLColor.White;
                }
                else
                {
                    cell.Style.Fill.BackgroundColor = XLColor.FromArgb(240, 240, 240);
                }
            });
            int idx = 2;
            for (int i = 0; i < newRows.Count; i++)
            {
                var row = newRows[i];

                this.AddRowToSheet(sheet, row, idx++, (IXLCell cell) =>
                {
                    var s = $"{cell.Value}";
                    if (s.StartsWith("Error:"))
                    {
                        cell.Style.Fill.BackgroundColor = XLColor.FromArgb(255, 112, 99);
                        cell.Value = s.Replace("Error:", string.Empty).Trim();
                    }
                });
            }

            sheet.Columns().AdjustToContents();
            sheet.SheetView.FreezeColumns(1);
            return resultingBook;
        }

        private IEnumerable<string> ExtractParcelNames(List<string> fieldNames, List<object[]> rows)
        {
            var parcelIndex = fieldNames.IndexOf(ParcelRelation);

            rows.ForEach(r => r[parcelIndex] = this.InsertDash(r[parcelIndex].ToString()));
            var parcelIdsToCheck = rows.Select(r => r[parcelIndex].ToString());
            return parcelIdsToCheck;
        }

        private void ExtractSheetInfo(IXLWorksheet fstSheet, out List<string> fieldTitles, out List<string> fieldNames, out List<object[]> rows)
        {
            fieldTitles = this.GetFields(fstSheet, 1).Select(s => s.ToString()).ToList();
            fieldNames = fieldTitles.Select(fn =>
        fn.StartsWith(ErrorColTitle) ? ErrorCol : ColumnMapping.TryGetValue(fn, out string v) ? v : string.Empty).ToList();
            var errorIndex = fieldNames.IndexOf(ErrorCol);

            rows = this.GetSheetData(fstSheet).ToList();

            // remove error column if present.
            if (errorIndex > -1)
            {
                // slice error out of all arrays.
                IEnumerable<object> RemoveErrorColumn(IEnumerable<object> fn) => fn.Where((object r, int i) => !i.Equals(errorIndex));

                rows = rows.Select(r => RemoveErrorColumn(r).ToArray()).ToList();
                fieldNames = RemoveErrorColumn(fieldNames).Select(f => f.ToString()).ToList();
                fieldTitles = RemoveErrorColumn(fieldTitles).Select(f => f.ToString()).ToList();
            }
        }

        private IEnumerable<object> GetFields(IXLWorksheet wb, int rowId, char lastCol = 'I')
        {
            for (char i = 'A'; i <= lastCol; i++)
            {
                string namedCell = $"{i}{rowId}";
                yield return wb.Cell(namedCell).Value;
            }
        }

        private async Task<string[]> GetParcelIds(IEnumerable<string> parcels)
        {
            var uniqueParcels = parcels
                .Distinct().ToList();

            // just need to run it once per parcel.
            var tasks = uniqueParcels.Select(parcelId =>
                this.crmWrapper.ExecuteGet<dynamic>(
                    "ptas_parceldetails",
                    $"$filter=ptas_name eq '{parcelId}'&$select=ptas_parceldetailid"));
            var taskResults = await Task.WhenAll(tasks);

            var foundParcels = parcels.Select(parcelName =>
            {
                string ptas_parceldetailid = taskResults[uniqueParcels.IndexOf(parcelName)].FirstOrDefault()?.ptas_parceldetailid;
                return ptas_parceldetailid;
            }).ToArray();
            return foundParcels;
        }

        private int GetPermitStatus(object cell) =>
            this.PermitStatusMapping.TryGetValue(cell.ToString(), out int value) ? value : 0;

        private object GetPermitType(object cell) =>
            this.PermitTypeMapping.TryGetValue(cell.ToString(), out int value) ? value : 0;

        private IEnumerable<object[]> GetSheetData(IXLWorksheet fstSheet)
        {
            for (int i = 2; i < 100; i++)
            {
                var thisRow = this.GetFields(fstSheet, i).ToArray();
                if (thisRow.All(r => string.IsNullOrEmpty(r?.ToString())))
                {
                    break;
                }

                yield return thisRow;
            }
        }

        private string InsertDash(string s)
        {
            if (s.Contains("-"))
            {
                return s;
            }

            var leftPart = s.Substring(0, s.Length - 4);
            var rightPart = s.Substring(s.Length - 4);
            return leftPart + "-" + rightPart;
        }

        private async Task LoadRelatedData()
        {
            if (this.PermitStatusMapping == null)
            {
                var task1 = this.crmWrapper.GetOptionsets(TableId, Permitstatus);
                var task2 = this.crmWrapper.GetOptionsets(TableId, PermitType);

                await Task.WhenAll(task1, task2);

                this.PermitStatusMapping = task1.Result
                    .ToDictionary(os => os.Value, os => os.AttributeValue ?? 0);
                this.PermitTypeMapping = task2.Result
                    .ToDictionary(os => os.Value, os => os.AttributeValue ?? 0);
            }
        }

        private async Task<object> SaveData(string[] fieldNames, object[][] rows)
        {
            await this.LoadRelatedData();
            EntityChanges[] result = rows.Select(row => this.CreateChanges(Guid.NewGuid().ToString(), fieldNames, row)).Skip(1).Take(1).ToArray();
            Console.WriteLine(new { result });
            var batchResults = await this.TransactionHelper.PrepareAndSendBatch(result, false);
            var resultContentTasks = batchResults.Select(httpResult =>
            {
                Task<string> task = httpResult.Content?.ReadAsStringAsync() ?? Task.FromResult("OK");
                return task;
            });
            var resultContents = await Task.WhenAll(resultContentTasks);
            return resultContents;
        }

        private async Task<string> SaveToContainer(string id, string fileName, MemoryStream memoryStream)
        {
            BlobContainerClient c = new BlobContainerClient(this.storageConnectionString, BlobContainerName);
            await c.CreateIfNotExistsAsync();

            BlobClient b = new BlobClient(
                connectionString: this.storageConnectionString,
                blobContainerName: BlobContainerName,
                blobName: $"{id.ToLower()}/{fileName}");
            _ = await b.DeleteIfExistsAsync();
            await b.UploadAsync(memoryStream);

            //// NOTE: uncomment following line if need to add SAS in the future.
            //// Reason for removal: the xl viewer does not understand it.
            ////DateTimeOffset expiresOn = DateTimeOffset.UtcNow.AddHours(this.permitSASExpireHours);
            ////var file = b.CanGenerateSasUri
            ////    ? b.GenerateSasUri(BlobSasPermissions.Read, expiresOn)?.ToString()
            ////    : null;
            ////if (string.IsNullOrEmpty(file))
            ////{
            ////    throw new Exception("Could not calculate SAS URI.");
            ////}

            var file = b.Uri.ToString();
            return file;
        }

        /// <summary>
        /// Payload input for the PermitFile api.
        /// </summary>
        public class Payload
        {
            /// <summary>
            /// Gets or sets file.
            /// </summary>
            public IFormFile File { get; set; }
        }

        /// <summary>
        /// Payload for saving the permit.
        /// </summary>
        public class PostPermitInfo
        {
            /// <summary>
            /// Gets or sets the url.
            /// </summary>
            public string FileUrl { get; set; }

            /// <summary>
            /// Gets or sets jurisdiction Id.
            /// </summary>
            public string JurisdictionId { get; set; }
        }
    }
}