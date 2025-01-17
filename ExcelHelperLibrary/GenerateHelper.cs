using NPOI.XSSF.UserModel;
using System.Collections.Generic;
using System.IO;

namespace ExcelHelperLibrary
{
    /// <summary>
    /// 生成帮助类
    /// </summary>
    public static class GenerateHelper
    {
        public static void GenerateExcel(string fileName, List<CommonTable> tables)
        {
            var workbook = new XSSFWorkbook();

            foreach (var table in tables)
            {
                var sheet = workbook.CreateSheet("Sheet" + (workbook.NumberOfSheets + 1));
                for (var rowIndex = 0; rowIndex < table.Rows.Count; rowIndex++)
                {
                    var row = sheet.CreateRow(rowIndex);
                    var commonRow = table.Rows[rowIndex];
                    for (var colIndex = 0; colIndex < commonRow.Cells.Count; colIndex++)
                    {
                        var cell = row.CreateCell(colIndex);
                        cell.SetCellValue(commonRow.Cells[colIndex]);
                    }
                }
            }

            using (var fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write))
            {
                workbook.Write(fileStream);
            }
        }
    }

    public class CommonTable
    {
        public List<CommonRow> Rows;
    }

    public class CommonRow
    {
        public List<string> Cells;
    }
}
