using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ExcelTool.Helpers
{
    /// <summary>
    /// 表格拆分帮助类(CSV)
    /// </summary>
    internal static class TableSplitHelper4Csv
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

            // 读取所有行
            var allLines = File.ReadAllLines(inputFileName, Encoding.Default);
            if (allLines.Length <= headerRows)
                throw new InvalidOperationException("文件数据行数不足，无法拆分。");

            // 获取表头和数据行
            var headers = allLines.Take(headerRows);
            var dataLines = allLines.Skip(headerRows).ToArray();

            // 计算需要拆分的文件数量
            var totalDataLines = dataLines.Length;
            var fileCount = (int)Math.Ceiling((double)totalDataLines / maxRowsPerFile);
            var numberLength = fileCount.ToString().Length;

            for (var i = 0; i < fileCount; i++)
            {
                var start = i * maxRowsPerFile;
                var count = Math.Min(maxRowsPerFile, totalDataLines - start);
                var linesToTake = dataLines.Skip(start).Take(count);

                var splitLines = headers.Concat(linesToTake);
                var index = (i + 1).ToString().PadLeft(numberLength, '0');
                var outputFileName = Path.Combine(fileDir, $"{fileNameWithoutExt}_part{index}{fileExt}");
                File.WriteAllLines(outputFileName, splitLines, Encoding.Default);
                yield return outputFileName;
            }
        }
    }
}
