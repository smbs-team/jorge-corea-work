namespace CustomSearchesServicesLibrary.CustomSearches
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using CustomSearchesServicesLibrary.CustomSearches.Services.Datasets;
    using Microsoft.VisualBasic.FileIO;

    /// <summary>
    /// Class that contains helper methods to read from csv files.
    /// </summary>
    public static class CsvReader
    {
        /// <summary>
        /// Processes the CSV file.
        /// </summary>
        /// <param name="onReadHeaders">The on read headers function.  Receives the header contents in a string array and locates
        /// potential custom search keys (CustomSearchResultId or Major/Minor).  If keys are found, indices to the keys are returned
        /// in the int array.  If this function returns false, the csv rows are not processed.</param>
        /// <param name="onReadRow">The on read row action.  Receives the row contents in a string array.</param>
        /// <param name="csvData">The CSV data.</param>
        /// <param name="processOnlyIfKeyFound">if set to true, the rows are processed only if a key was found during header processing.</param>
        /// <returns>The task.</returns>
        public static async Task ProcessCsvAsync(
            Func<string[], int[], Task<bool>> onReadHeaders,
            Func<string[], Task> onReadRow,
            string csvData,
            bool processOnlyIfKeyFound = true)
        {
            var strReader = new StringReader(csvData);
            var parser = new TextFieldParser(strReader);
            parser.TextFieldType = FieldType.Delimited;
            parser.SetDelimiters(ExportDatasetDataToFileService.CsvSeparator);
            int i = 0;
            bool continueProcessing = true;

            while (!parser.EndOfData && continueProcessing)
            {
                string[] fields = parser.ReadFields();

                if (i == 0)
                {
                    int fieldIndex = 0;
                    int customSearchResultIdIndex = -1;
                    int majorFieldIndex = -1;
                    int minorFieldIndex = -1;
                    int[] keyIndices = null;

                    foreach (var header in fields)
                    {
                        if (header.ToLower() == "customsearchresultid")
                        {
                            customSearchResultIdIndex = fieldIndex;
                        }
                        else if (header.ToLower() == "major")
                        {
                            majorFieldIndex = fieldIndex;
                        }
                        else if (header.ToLower() == "minor")
                        {
                            minorFieldIndex = fieldIndex;
                        }

                        fieldIndex++;
                    }

                    if ((majorFieldIndex != -1) && (minorFieldIndex != -1))
                    {
                        keyIndices = new int[] { majorFieldIndex, minorFieldIndex };
                    }
                    else if (customSearchResultIdIndex != -1)
                    {
                        keyIndices = new int[] { customSearchResultIdIndex };
                    }

                    continueProcessing = keyIndices != null || !processOnlyIfKeyFound;

                    if (continueProcessing)
                    {
                        continueProcessing = await onReadHeaders(fields, keyIndices);
                    }
                }
                else
                {
                    await onReadRow(fields);
                }

                i++;
            }
        }
    }
}