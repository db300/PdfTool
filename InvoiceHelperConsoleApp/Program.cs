using CsvHelper;
using CsvHelper.Configuration;
using InvoiceHelperLibrary.Entities;
using System.Globalization;
using System.Text;

namespace InvoiceHelperConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            var list = new List<InvoiceItem>();
            foreach (var fileName in InputPdfFileList)
            {
                var (success, msg, invoiceItem) = InvoiceHelperLibrary.ParseHelper.Extract(fileName);
                if (!success)
                {
                    Console.Error.WriteLine($"{fileName} 提取失败: {msg}");
                    continue;
                }
                list.Add(invoiceItem);
                Console.Out.WriteLine($"{fileName} 提取成功");
            }

            ExportCsv(list, out var outputFileName);
            Console.Out.WriteLine($"{outputFileName} 生成完成");
        }

        private static void ExportCsv(List<InvoiceItem> list, out string outputFileName)
        {
            outputFileName = $"发票信息提取结果_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Encoding = Encoding.UTF8,
                HasHeaderRecord = true
            };

            using (var writer = new StreamWriter(outputFileName, false, Encoding.UTF8))
            using (var csv = new CsvWriter(writer, config))
            {
                // 写表头
                foreach (var column in Columns)
                {
                    csv.WriteField(column.Item1);
                }
                csv.NextRecord();

                // 写数据
                foreach (var item in list)
                {
                    csv.WriteField(item.InvoiceType);
                    csv.WriteField(item.InvoiceNumber);
                    csv.WriteField(item.InvoiceDate);
                    csv.WriteField(item.BuyerName);
                    csv.WriteField(item.BuyerTaxNumber);
                    csv.WriteField(item.SellerName);
                    csv.WriteField(item.SellerTaxNumber);
                    csv.WriteField(item.TotalAmount);
                    csv.WriteField(item.TotalTax);
                    csv.NextRecord();
                }
            }
        }

        private static readonly List<(string, string)> Columns = new List<(string, string)>
        {
            ("发票类型", "InvoiceType"),
            ("发票号码", "InvoiceNumber"),
            ("开票日期", "InvoiceDate"),
            ("购买方名称", "BuyerName"),
            ("购买方税号", "BuyerTaxNumber"),
            ("销售方名称", "SellerName"),
            ("销售方税号", "SellerTaxNumber"),
            ("金额合计", "TotalAmount"),
            ("税额合计", "TotalTax")
        };

        private static readonly List<string> InputPdfFileList = new List<string>
        {
            "C:\\Users\\冷怀晶\\xwechat_files\\HawkLeng_adc4\\msg\\file\\2025-08\\发票测试\\兼容性测试通过\\车票.pdf",
            "C:\\Users\\冷怀晶\\xwechat_files\\HawkLeng_adc4\\msg\\file\\2025-08\\发票测试\\兼容性测试通过\\法院票据.PDF",
            "C:\\Users\\冷怀晶\\xwechat_files\\HawkLeng_adc4\\msg\\file\\2025-08\\发票测试\\兼容性测试通过\\会费.pdf",
            "C:\\Users\\冷怀晶\\xwechat_files\\HawkLeng_adc4\\msg\\file\\2025-08\\发票测试\\兼容性测试通过\\机票行程单.pdf",
            "C:\\Users\\冷怀晶\\xwechat_files\\HawkLeng_adc4\\msg\\file\\2025-08\\发票测试\\兼容性测试通过\\普票1.pdf",
            "C:\\Users\\冷怀晶\\xwechat_files\\HawkLeng_adc4\\msg\\file\\2025-08\\发票测试\\兼容性测试通过\\普票2.pdf",
            "C:\\Users\\冷怀晶\\xwechat_files\\HawkLeng_adc4\\msg\\file\\2025-08\\发票测试\\兼容性测试通过\\普票2-税务局导出.pdf",
            "C:\\Users\\冷怀晶\\xwechat_files\\HawkLeng_adc4\\msg\\file\\2025-08\\发票测试\\兼容性测试通过\\铁路电子客票.pdf",
            "C:\\Users\\冷怀晶\\xwechat_files\\HawkLeng_adc4\\msg\\file\\2025-08\\发票测试\\兼容性测试通过\\专票多行.pdf",
            "C:\\Users\\冷怀晶\\xwechat_files\\HawkLeng_adc4\\msg\\file\\2025-08\\发票测试\\兼容性测试通过\\专票两行.pdf",
            "C:\\Users\\冷怀晶\\xwechat_files\\HawkLeng_adc4\\msg\\file\\2025-08\\发票测试\\兼容性测试通过\\专票一行.pdf",
        };
    }
}
