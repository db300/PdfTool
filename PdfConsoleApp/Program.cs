using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PdfConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // 检查命令行参数
                if (args.Length == 0)
                {
                    Console.WriteLine("用法: PdfConsoleApp.exe <json文件路径>");
                    Console.WriteLine("示例: PdfConsoleApp.exe config.json");
                    return;
                }

                var jsonFilePath = args[0];

                // 检查 JSON 文件是否存在
                if (!File.Exists(jsonFilePath))
                {
                    Console.Error.WriteLine($"错误: JSON 文件不存在 - {jsonFilePath}");
                    Console.Error.WriteLine($"当前目录: {Directory.GetCurrentDirectory()}");
                    return;
                }

                // 读取 JSON 文件
                var jsonContent = File.ReadAllText(jsonFilePath, Encoding.UTF8);

                // 解析 JSON 为 PdfBuildParam 对象
                var param = ParseJson(jsonContent);

                // 验证参数
                if (param == null)
                {
                    Console.Error.WriteLine("错误: JSON 解析失败");
                    return;
                }

                if (string.IsNullOrWhiteSpace(param.OutputFileName))
                {
                    Console.Error.WriteLine("错误: 输出文件名不能为空");
                    return;
                }

                if (string.IsNullOrWhiteSpace(param.TextContent))
                {
                    Console.Error.WriteLine("错误: 文本内容不能为空");
                    return;
                }

                // 生成 PDF
                PdfBuilder.GeneratePdf(param);

                Console.WriteLine($"PDF 生成成功: {param.OutputFileName}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"错误: {ex.Message}");
                Console.Error.WriteLine(ex.StackTrace);
            }
        }

        /// <summary>
        /// 简单的 JSON 解析（仅支持基本的键值对）
        /// </summary>
        private static PdfBuildParam ParseJson(string json)
        {
            var param = new PdfBuildParam();

            // 移除空白字符和花括号
            json = json.Trim().TrimStart('{').TrimEnd('}');

            // 按逗号分割，处理引号内的逗号
            var pairs = SplitJsonPairs(json);

            foreach (var pair in pairs)
            {
                var colonIndex = pair.IndexOf(':');
                if (colonIndex < 0) continue;

                var key = pair.Substring(0, colonIndex).Trim().Trim('"');
                var value = pair.Substring(colonIndex + 1).Trim().Trim('"', ',');

                switch (key)
                {
                    case "OutputFileName":
                        param.OutputFileName = value;
                        break;
                    case "TextContent":
                        // 处理转义的换行符
                        param.TextContent = value.Replace("\\r\\n", "\r\n").Replace("\\n", "\n");
                        break;
                    case "FontName":
                        param.FontName = value;
                        break;
                    case "FontSize":
                        if (double.TryParse(value, out var fontSize))
                            param.FontSize = fontSize;
                        break;
                    case "PageWidth":
                        if (double.TryParse(value, out var pageWidth))
                            param.PageWidth = pageWidth;
                        break;
                    case "PageHeight":
                        if (double.TryParse(value, out var pageHeight))
                            param.PageHeight = pageHeight;
                        break;
                    case "MarginLeft":
                        if (double.TryParse(value, out var marginLeft))
                            param.MarginLeft = marginLeft;
                        break;
                    case "MarginRight":
                        if (double.TryParse(value, out var marginRight))
                            param.MarginRight = marginRight;
                        break;
                    case "MarginTop":
                        if (double.TryParse(value, out var marginTop))
                            param.MarginTop = marginTop;
                        break;
                    case "MarginBottom":
                        if (double.TryParse(value, out var marginBottom))
                            param.MarginBottom = marginBottom;
                        break;
                }
            }

            return param;
        }

        /// <summary>
        /// 分割 JSON 键值对（简化版本）
        /// </summary>
        private static List<string> SplitJsonPairs(string json)
        {
            var pairs = new List<string>();
            var current = new StringBuilder();
            var inString = false;
            var escapeNext = false;

            foreach (var ch in json)
            {
                if (escapeNext)
                {
                    current.Append(ch);
                    escapeNext = false;
                    continue;
                }

                if (ch == '\\')
                {
                    current.Append(ch);
                    escapeNext = true;
                    continue;
                }

                if (ch == '"')
                {
                    inString = !inString;
                    current.Append(ch);
                    continue;
                }

                if (ch == ',' && !inString)
                {
                    pairs.Add(current.ToString().Trim());
                    current.Clear();
                    continue;
                }

                current.Append(ch);
            }

            if (current.Length > 0)
            {
                pairs.Add(current.ToString().Trim());
            }

            return pairs;
        }
    }
}
