using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;

namespace ExcelTool.Helpers
{
    /// <summary>
    /// 表格拆分帮助类(EXCEL)
    /// </summary>
    internal static class TableSplitHelper4Excel
    {
        internal static IEnumerable<string> Split(string inputFileName, int headerRows, int maxRowsPerFile)
        {
            if (string.IsNullOrEmpty(inputFileName) || !File.Exists(inputFileName))
                throw new FileNotFoundException("输入文件不存在", inputFileName);

            if (headerRows < 0) throw new ArgumentOutOfRangeException(nameof(headerRows));
            if (maxRowsPerFile <= 0) throw new ArgumentOutOfRangeException(nameof(maxRowsPerFile));

            var fileDir = Path.GetDirectoryName(inputFileName);
            var fileNameWithoutExt = Path.GetFileNameWithoutExtension(inputFileName);
            var fileExt = Path.GetExtension(inputFileName);

            IWorkbook workbook;
            using (var fs = new FileStream(inputFileName, FileMode.Open, FileAccess.Read))
            {
                if (fileExt.Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
                    workbook = new XSSFWorkbook(fs);
                else
                    workbook = new HSSFWorkbook(fs);
            }

            var sheet = workbook.GetSheetAt(0);
            var lastRow = sheet.LastRowNum;
            if (lastRow + 1 <= headerRows)
                throw new InvalidOperationException("文件数据行数不足，无法拆分。");

            // 获取表头
            var headerRowList = new List<IRow>();
            for (var i = 0; i < headerRows; i++)
            {
                var row = sheet.GetRow(i);
                headerRowList.Add(row);
            }

            // 计算需要拆分的文件数量
            var totalDataRows = lastRow + 1 - headerRows;
            var fileCount = (int)Math.Ceiling((double)totalDataRows / maxRowsPerFile);
            var numberLength = fileCount.ToString().Length;

            for (var i = 0; i < fileCount; i++)
            {
                IWorkbook newWb = fileExt.Equals(".xlsx", StringComparison.OrdinalIgnoreCase)
                    ? (IWorkbook)new XSSFWorkbook()
                    : new HSSFWorkbook();
                var newSheet = newWb.CreateSheet(sheet.SheetName);

                // 写表头
                for (var h = 0; h < headerRows; h++)
                {
                    var srcRow = headerRowList[h];
                    var destRow = newSheet.CreateRow(h);
                    if (srcRow != null)
                    {
                        for (var c = 0; c < srcRow.LastCellNum; c++)
                        {
                            var srcCell = srcRow.GetCell(c);
                            var destCell = destRow.CreateCell(c);
                            if (srcCell != null)
                                destCell.SetCellValue(srcCell.ToString());
                        }
                    }
                }

                // 写数据
                int startDataRow = headerRows + i * maxRowsPerFile;
                int endDataRow = Math.Min(headerRows + (i + 1) * maxRowsPerFile - 1, lastRow);
                int destRowIdx = headerRows;
                for (int r = startDataRow; r <= endDataRow; r++, destRowIdx++)
                {
                    var srcRow = sheet.GetRow(r);
                    var destRow = newSheet.CreateRow(destRowIdx);
                    if (srcRow != null)
                    {
                        for (int c = 0; c < srcRow.LastCellNum; c++)
                        {
                            var srcCell = srcRow.GetCell(c);
                            var destCell = destRow.CreateCell(c);
                            if (srcCell != null)
                                destCell.SetCellValue(srcCell.ToString());
                        }
                    }
                }

                var index = (i + 1).ToString().PadLeft(numberLength, '0');
                var outputFileName = Path.Combine(fileDir, $"{fileNameWithoutExt}_part{index}{fileExt}");
                using (var outFs = new FileStream(outputFileName, FileMode.Create, FileAccess.Write))
                {
                    newWb.Write(outFs);
                }
                yield return outputFileName;
            }
        }
    }
}
