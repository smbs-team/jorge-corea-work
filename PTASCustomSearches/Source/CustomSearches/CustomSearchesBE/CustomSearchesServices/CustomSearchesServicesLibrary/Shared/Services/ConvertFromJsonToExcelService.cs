namespace CustomSearchesServicesLibrary.Shared.Services
{
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using ClosedXML.Excel;
    using CustomSearchesServicesLibrary.Exception;
    using CustomSearchesServicesLibrary.Misc;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Service that converts from json object to excel file.
    /// </summary>
    public class ConvertFromJsonToExcelService : BaseService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConvertFromJsonToExcelService"/> class.
        /// </summary>
        /// <param name="serviceContext">The service context.</param>
        public ConvertFromJsonToExcelService(IServiceContext serviceContext)
            : base(serviceContext)
        {
        }

        /// <summary>
        /// Converts from json object to excel file.
        /// </summary>
        /// <param name="jsonObject">The json object to convert.</param>
        /// <param name="fileName">The file name as an output parameter.  Created from the dataset.</param>
        /// <returns>
        /// The bytes of the excel file.
        /// </returns>
        /// <exception cref="CustomSearchesEntityNotFoundException">Dataset was not found.</exception>
        public async Task<byte[]> ConvertFromJsonToExcelAsync(
            JObject jsonObject,
            Ref<string> fileName)
        {
            fileName.Value = $"Excel.xlsx";

            // Creating a new workbook
            var workbook = new XLWorkbook();

            foreach (JProperty sheetProperty in (JToken)jsonObject)
            {
                string sheetName = sheetProperty.Name;

                // Adding a worksheet
                var worksheet = workbook.Worksheets.Add(sheetName);

                int rowIndex = 1;
                var headers = sheetProperty.Value.ToArray().Where(t => ((JProperty)t).Name.Trim().ToLower() == "headers").FirstOrDefault();
                if (headers != null)
                {
                    var headerValues = headers.Values();
                    int columnIndex = 1;
                    foreach (var cellValue in headerValues)
                    {
                        // Adding cell to worksheet
                        worksheet.Cell(rowIndex, columnIndex).SetValue(((JValue)cellValue.Value<object>()).Value);
                        if (cellValue.Type == JTokenType.Float)
                        {
                            worksheet.Cell(rowIndex, columnIndex).Style.NumberFormat.SetNumberFormatId((int)XLPredefinedFormat.Number.Precision2);
                        }
                        else if (cellValue.Type == JTokenType.Integer)
                        {
                            worksheet.Cell(rowIndex, columnIndex).Style.NumberFormat.SetNumberFormatId((int)XLPredefinedFormat.Number.Integer);
                        }

                        columnIndex++;
                    }

                    rowIndex++;
                }

                var rows = sheetProperty.Value.ToArray().Where(t => ((JProperty)t).Name.Trim().ToLower() == "rows").FirstOrDefault();
                if (rows != null)
                {
                    var rowValues = rows.Values();
                    foreach (var row in rowValues)
                    {
                        int columnIndex = 1;
                        foreach (var cellValue in row)
                        {
                            // Adding cell to worksheet
                            worksheet.Cell(rowIndex, columnIndex).SetValue(((JValue)cellValue.Value<object>()).Value);
                            if (cellValue.Type == JTokenType.Float)
                            {
                                worksheet.Cell(rowIndex, columnIndex).Style.NumberFormat.SetNumberFormatId((int)XLPredefinedFormat.Number.Precision2);
                            }
                            else if (cellValue.Type == JTokenType.Integer)
                            {
                                worksheet.Cell(rowIndex, columnIndex).Style.NumberFormat.SetNumberFormatId((int)XLPredefinedFormat.Number.Integer);
                            }

                            columnIndex++;
                        }

                        rowIndex++;
                    }
                }

                // Adjust column widths to their content
                worksheet.Columns().AdjustToContents();
            }

            using (var stream = new MemoryStream())
            {
                workbook.SaveAs(stream);
                return stream.ToArray();
            }
        }
    }
}
