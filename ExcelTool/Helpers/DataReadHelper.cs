using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace ExcelTool.Helpers
{
    /// <summary>
    /// 数据读取帮助类
    /// </summary>
    internal static class DataReadHelper
    {
        internal static Dictionary<string, DataTable> ReadExcelToDataTable(string filePath)
        {
            var result = new Dictionary<string, DataTable>();

            IWorkbook workbook;
            using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                if (Path.GetExtension(filePath).ToLower() == ".xlsx")
                    workbook = new XSSFWorkbook(fs);
                else
                    workbook = new HSSFWorkbook(fs);
            }

            for (var i = 0; i < workbook.NumberOfSheets; i++)
            {
                var sheet = workbook.GetSheetAt(i);
                var table = new DataTable();

                // 查找第一个非空行作为表头
                IRow headerRow = null;
                int headerRowIndex = -1;
                for (int r = 0; r <= sheet.LastRowNum; r++)
                {
                    var row = sheet.GetRow(r);
                    if (row != null && row.Cells.Any(cell => cell != null && !string.IsNullOrWhiteSpace(cell.ToString())))
                    {
                        headerRow = row;
                        headerRowIndex = r;
                        break;
                    }
                }

                if (headerRow == null)
                {
                    // 整个Sheet为空，添加空表
                    result[sheet.SheetName] = table;
                    continue;
                }

                int cellCount = headerRow.LastCellNum;
                // 添加列
                for (var c = 0; c < cellCount; c++)
                {
                    var columnName = headerRow.GetCell(c)?.ToString() ?? $"列{c + 1}";
                    if (table.Columns.Contains(columnName))
                        columnName += $"_{c}"; // Append index to make it unique
                    table.Columns.Add(columnName);
                }
                // 添加行
                for (int r = headerRowIndex + 1; r <= sheet.LastRowNum; r++)
                {
                    var row = sheet.GetRow(r);
                    if (row == null) continue;
                    var dataRow = table.NewRow();
                    for (int c = 0; c < cellCount; c++)
                        dataRow[c] = row.GetCell(c)?.ToString();
                    table.Rows.Add(dataRow);
                }
                result[sheet.SheetName] = table;
            }

            return result;
        }
    }
}
