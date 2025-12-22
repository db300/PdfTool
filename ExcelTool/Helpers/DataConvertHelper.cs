using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ExcelTool.Helpers
{
    /// <summary>
    /// 数据转换帮助类
    /// </summary>
    internal static class DataConvertHelper
    {
        #region Convert Excel file to JSON
        internal static void ConvertExcelToJson(string excelFilePath, string outputJsonPath)
        {
            ConvertExcelToJson(excelFilePath, outputJsonPath, true, null);
        }

        /// <summary>
        /// 将Excel文件转换为JSON格式
        /// </summary>
        /// <param name="excelFilePath">Excel文件路径</param>
        /// <param name="outputJsonPath">输出JSON文件路径</param>
        /// <param name="skipEmptyRows">是否跳过空行，默认为true</param>
        /// <param name="columnMapping">列名映射字典，将Excel列名映射为JSON的key，为null时使用原列名</param>
        internal static void ConvertExcelToJson(string excelFilePath, string outputJsonPath, bool skipEmptyRows = true, Dictionary<string, string> columnMapping = null)
        {
            ExcelPackage.License.SetNonCommercialPersonal("ColdDaddy");

            try
            {
                var jsonList = new List<Dictionary<string, object>>();

                using (var package = new ExcelPackage(new FileInfo(excelFilePath)))
                {
                    var worksheet = package.Workbook.Worksheets[0];
                    var rowCount = worksheet.Dimension?.Rows ?? 0;
                    var colCount = worksheet.Dimension?.Columns ?? 0;

                    // Get headers from first row
                    var headers = new List<string>();
                    for (int col = 1; col <= colCount; col++)
                    {
                        var headerName = worksheet.Cells[1, col].Value?.ToString() ?? $"Column{col}";
                        headers.Add(headerName);
                    }

                    // Convert data rows to JSON objects
                    for (int row = 2; row <= rowCount; row++)
                    {
                        // Check if row is empty
                        if (skipEmptyRows && IsRowEmpty(worksheet, row, colCount))
                        {
                            continue;
                        }

                        var rowData = new Dictionary<string, object>();
                        for (int col = 1; col <= colCount; col++)
                        {
                            var originalKey = headers[col - 1];
                            var mappedKey = ApplyColumnMapping(originalKey, columnMapping);
                            rowData[mappedKey] = worksheet.Cells[row, col].Value ?? string.Empty;
                        }
                        jsonList.Add(rowData);
                    }
                }

                // Build JSON manually
                var jsonContent = BuildJsonFromDictionaries(jsonList);
                File.WriteAllText(outputJsonPath, jsonContent, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"转换失败: {ex.Message}", "错误");
            }
        }

        /// <summary>
        /// 检查行是否为空
        /// </summary>
        private static bool IsRowEmpty(ExcelWorksheet worksheet, int row, int colCount)
        {
            for (int col = 1; col <= colCount; col++)
            {
                var cellValue = worksheet.Cells[row, col].Value;
                if (cellValue != null && !string.IsNullOrWhiteSpace(cellValue.ToString()))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 应用列名映射
        /// </summary>
        private static string ApplyColumnMapping(string originalColumnName, Dictionary<string, string> columnMapping)
        {
            if (columnMapping != null && columnMapping.ContainsKey(originalColumnName))
            {
                return columnMapping[originalColumnName];
            }
            return originalColumnName;
        }

        private static string BuildJsonFromDictionaries(List<Dictionary<string, object>> dataList)
        {
            var sb = new StringBuilder();
            sb.AppendLine("[");

            for (int i = 0; i < dataList.Count; i++)
            {
                sb.AppendLine("  {");
                var dict = dataList[i];
                var items = dict.ToList();

                for (int j = 0; j < items.Count; j++)
                {
                    var key = items[j].Key;
                    var value = items[j].Value;
                    var jsonValue = value == null ? "null" : $"\"{EscapeJsonString(value.ToString())}\"";

                    sb.Append($"    \"{EscapeJsonString(key)}\": {jsonValue}");
                    if (j < items.Count - 1)
                        sb.Append(",");
                    sb.AppendLine();
                }

                sb.Append("  }");
                if (i < dataList.Count - 1)
                    sb.Append(",");
                sb.AppendLine();
            }

            sb.AppendLine("]");
            return sb.ToString();
        }

        private static string EscapeJsonString(string str)
        {
            if (string.IsNullOrEmpty(str))
                return string.Empty;

            return str.Replace("\\", "\\\\")
                      .Replace("\"", "\\\"")
                      .Replace("\n", "\\n")
                      .Replace("\r", "\\r")
                      .Replace("\t", "\\t");
        }
        #endregion
    }
}
