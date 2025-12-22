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
            // Temporary code: Excel to JSON conversion
            //ConvertExcelToJson("C:\\Users\\db300\\Downloads\\H5页面素材\\中文学生组.xlsx", "C:\\Users\\db300\\Downloads\\H5页面素材\\中文学生组.json");
            //ConvertExcelToJson("C:\\Users\\db300\\Downloads\\H5页面素材\\多文种.xlsx", "C:\\Users\\db300\\Downloads\\H5页面素材\\多文种.json");

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
                        headers.Add(worksheet.Cells[1, col].Value?.ToString() ?? $"Column{col}");
                    }

                    // Convert data rows to JSON objects
                    for (int row = 2; row <= rowCount; row++)
                    {
                        var rowData = new Dictionary<string, object>();
                        for (int col = 1; col <= colCount; col++)
                        {
                            rowData[headers[col - 1]] = worksheet.Cells[row, col].Value ?? string.Empty;
                        }
                        jsonList.Add(rowData);
                    }
                }

                // Build JSON manually
                var jsonContent = BuildJsonFromDictionaries(jsonList);
                File.WriteAllText(outputJsonPath, jsonContent, Encoding.UTF8);

                MessageBox.Show($"Excel文件已转换为JSON格式，保存到: {outputJsonPath}", "转换成功");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"转换失败: {ex.Message}", "错误");
            }
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
