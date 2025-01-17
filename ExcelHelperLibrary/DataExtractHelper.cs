using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;

namespace ExcelHelperLibrary
{
    /// <summary>
    /// 数据提取帮助类
    /// </summary>
    public static class DataExtractHelper
    {
        public static List<List<string>> Extract2Table(string inputFileName)
        {
            try
            {
                var list = new List<List<string>>();
                using (var fileStream = new FileStream(inputFileName, FileMode.Open, FileAccess.Read))
                using (var workbook = new XSSFWorkbook(fileStream))
                {
                    var sheet = workbook.GetSheetAt(0);
                    for (var i = 0; i <= sheet.LastRowNum; i++)
                    {
                        var row = sheet.GetRow(i);
                        if (row == null) continue;
                        var rowData = new List<string>();
                        for (var j = 0; j < row.LastCellNum; j++)
                        {
                            var cell = row.GetCell(j);
                            rowData.Add(cell?.ToString() ?? string.Empty);
                        }
                        list.Add(rowData);
                    }
                }
                return list;
            }
            catch (Exception ex)
            {
                // 记录异常信息
                System.Diagnostics.Debug.WriteLine($"Error extracting data from Excel file: {ex.Message}");
                return null;
            }
        }
    }
}
