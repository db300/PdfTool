using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;

namespace ExcelTool.Helpers
{
    /// <summary>
    /// 数据提取帮助类
    /// </summary>
    internal static class DataExtractHelper
    {
        internal static List<string> ExtractHeader(string inputFile, CommonHelper.InfoHandler infoHandler)
        {
            var ext = Path.GetExtension(inputFile).ToLower();
            switch (ext)
            {
                case ".csv":
                    return ExtractHeaderWithNPOI(inputFile, infoHandler);
                case ".xls":
                case ".xlsx":
                    break;
            }
            return null;
        }

        private static List<string> ExtractHeader4Csv(string inputFile, CommonHelper.InfoHandler infoHandler)
        {
            try
            {
                /*
                //var s = File.ReadAllText(inputFile, Encoding.GetEncoding(54936));
                var encoding = GetTxtEncoding(inputFile);
                var s = File.ReadAllText(inputFile, encoding);
                var csvLines = CsvReader.ReadFromText(s).ToList();
                if (csvLines.Count == 0)
                {
                    infoHandler($"{inputFile} 文件内容为空");
                    return null;
                }
                var headers = csvLines.First().Headers.ToList();
                return headers;
                */
                return null;
            }
            catch (Exception ex)
            {
                infoHandler($"{inputFile} 文件读取失败，原因：{ex.Message}");
                return null;
            }
        }

        internal static List<string> ExtractHeaderWithNPOI(string inputFile, CommonHelper.InfoHandler infoHandler)
        {
            try
            {
                using (var fileStream = new FileStream(inputFile, FileMode.Open, FileAccess.Read))
                {
                    IWorkbook workbook = new XSSFWorkbook(fileStream);
                    ISheet sheet = workbook.GetSheetAt(0);
                    IRow headerRow = sheet.GetRow(0);

                    List<string> headers = new List<string>();
                    for (int i = 0; i < headerRow.LastCellNum; i++)
                    {
                        headers.Add(headerRow.GetCell(i).ToString());
                    }

                    return headers;
                }
            }
            catch (Exception ex)
            {
                infoHandler($"{inputFile} 文件读取失败，原因：{ex.Message}");
                return null;
            }
        }

        internal static void Extract(string inputFile, List<string> fields, CommonHelper.InfoHandler infoHandler)
        {
            var ext = Path.GetExtension(inputFile).ToLower();
            switch (ext)
            {
                case ".csv":
                    ExtractCsv(inputFile, fields, infoHandler);
                    break;
                case ".xls":
                case ".xlsx":
                    break;
            }
            // Extract data from the input file
            infoHandler($"Extracting data from {inputFile}");
            // Do the extraction
            infoHandler("Data extraction completed");
        }

        private static void ExtractCsv(string inputFile, List<string> fields, CommonHelper.InfoHandler infoHandler)
        {
            try
            {
                /*
                //读取CSV文件内容，使用GB18030编码
                var s = File.ReadAllText(inputFile, Encoding.GetEncoding(54936));
                var csvLines = CsvReader.ReadFromText(s).ToList();
                if (csvLines.Count == 0)
                {
                    infoHandler($"{inputFile} 文件内容为空");
                    return;
                }
                //提取符合fields列的数据
                var headers = csvLines[0].Headers.ToList();
                var actFields = headers.Intersect(fields).ToList();
                //将csvLines里符合actFields列的数据提取出来
                var extractedData = csvLines
                    .Select(line => string.Join(",", actFields.Select(field => line[field])))
                    .ToList();
                */
            }
            catch (Exception ex)
            {
                infoHandler($"{inputFile} 文件读取失败，原因：{ex.Message}");
            }
        }
    }
}
