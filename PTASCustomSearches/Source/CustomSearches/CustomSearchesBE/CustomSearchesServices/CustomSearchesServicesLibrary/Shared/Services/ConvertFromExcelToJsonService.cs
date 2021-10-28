namespace CustomSearchesServicesLibrary.Shared.Services
{
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using ClosedXML.Excel;
    using CustomSearchesServicesLibrary.Exception;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using DocumentFormat.OpenXml.Office2010.ExcelAc;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Service that converts from excel file to json object.
    /// </summary>
    public class ConvertFromExcelToJsonService : BaseService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConvertFromExcelToJsonService"/> class.
        /// </summary>
        /// <param name="serviceContext">The service context.</param>
        public ConvertFromExcelToJsonService(IServiceContext serviceContext)
            : base(serviceContext)
        {
        }

        /// <summary>
        /// Converts from excel file to json object.
        /// </summary>
        /// <param name="data">The excel file to convert.</param>
        /// <param name="hasHeader">Value indicating whether the results should include the post process.</param>
        /// <returns>
        /// The json object.
        /// </returns>
        /// <exception cref="CustomSearchesEntityNotFoundException">Custom search definition or parameter was not found.</exception>
        /// <exception cref="AuthorizationException">Not authorized request.</exception>
        /// <exception cref="CustomSearchesRequestBodyException">Required parameter was not added or parameter has an invalid value.</exception>
        public async Task<object> ConvertFromExcelToJsonAsync(Stream data, bool hasHeader)
        {
            XLWorkbook workbook = new XLWorkbook(data);

            JObject jsonObject = new JObject();

            int worksheetIndex = 0;
            foreach (var worksheet in workbook.Worksheets)
            {
                bool requiresHeader = hasHeader;
                var rows = worksheet.Rows();
                JObject jsonWorksheet = new JObject();
                List<IEnumerable<object>> rowList = new List<IEnumerable<object>>();
                foreach (var row in rows)
                {
                    var cellValues = row.Cells().Select(c => c.Value);
                    if (requiresHeader)
                    {
                        JToken jsonCells = JToken.FromObject(cellValues);
                        jsonWorksheet.Add("headers", jsonCells);
                        requiresHeader = false;
                    }
                    else
                    {
                        rowList.Add(cellValues);
                    }
                }

                JToken jsonRowList = JToken.FromObject(rowList);
                jsonWorksheet.Add("rows", jsonRowList);
                jsonObject.Add(worksheet.Name, (JToken)jsonWorksheet);
                worksheetIndex++;
            }

            return jsonObject.ToObject<object>();
        }
    }
}
