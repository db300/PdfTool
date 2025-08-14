using InvoiceExtractor.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InvoiceExtractor.Helpers
{
    /// <summary>
    /// 解析帮助类
    /// </summary>
    internal static class ParseHelper
    {
        internal static (bool, string, InvoiceItem) Extract(string fileName)
        {
            try
            {
                var tables = PdfHelperLibrary3.TableHelper.Pdf2Table(fileName, new List<int>(), new List<int>());
                var ss = PdfHelperLibrary3.TextHelper.Pdf2String(fileName);

                var invoiceItem = new InvoiceItem();
                //读取发票号码和开票日期
                ExtractInvoiceBaseInfo(ss, invoiceItem);
                //读取购买方信息和销售方信息
                ExtractPartyInfo(tables.FirstOrDefault(), invoiceItem);
                //读取发票内容项
                ExtractContentItems(ss, invoiceItem);

#if DEBUG
                System.Diagnostics.Debug.WriteLine(invoiceItem);
#endif
                return (true, "", invoiceItem);
            }
            catch (Exception ex)
            {
                return (false, ex.Message, null);
            }
        }

        private static void ExtractInvoiceBaseInfo(List<string> ss, InvoiceItem invoiceItem)
        {
            int found = 0;
            foreach (var s in ss)
            {
                if (s.Contains("发票号码"))
                {
                    invoiceItem.InvoiceNumber = GetValueAfterColon(s);
                    found++;
                }
                else if (s.Contains("开票日期"))
                {
                    invoiceItem.InvoiceDate = GetValueAfterColon(s);
                    found++;
                }
                if (found == 2) break;
            }
        }

        private static void ExtractPartyInfo(List<string> table, InvoiceItem invoiceItem)
        {
            if (table == null) return;
            table.RemoveAll(string.IsNullOrWhiteSpace);

            for (int i = 0; i < table.Count; i++)
            {
                var cell = table[i].Replace("\n", "");
                if (cell == "购买方信息")
                {
                    ExtractSinglePartyInfo(table, i + 1, invoiceItem, true);
                    i++;
                }
                else if (cell == "销售方信息")
                {
                    ExtractSinglePartyInfo(table, i + 1, invoiceItem, false);
                    i++;
                }
            }
        }

        private static void ExtractSinglePartyInfo(List<string> table, int index, InvoiceItem invoiceItem, bool isBuyer)
        {
            if (index >= table.Count) return;
            var lines = table[index].Split('\n');
            foreach (var line in lines)
            {
                //if (line.Contains("名称"))
                if (System.Text.RegularExpressions.Regex.IsMatch(line, @"名\s*称"))//兼容"名称"和"名 称"
                {
                    if (isBuyer) invoiceItem.BuyerName = GetValueAfterColon(line);
                    else invoiceItem.SellerName = GetValueAfterColon(line);
                }
                else if (line.Contains("统一社会信用代码/纳税人识别号"))
                {
                    if (isBuyer) invoiceItem.BuyerTaxNumber = GetValueAfterColon(line);
                    else invoiceItem.SellerTaxNumber = GetValueAfterColon(line);
                }
            }
        }

        private static void ExtractContentItems(List<string> ss, InvoiceItem invoiceItem)
        {
            var infos = new List<string>();
            bool read = false;
            foreach (var s in ss)
            {
                if (s.Contains("项目名称")) { read = true; continue; }
                //if (s.Contains("价税合计（大写）")) { break; }
                if (System.Text.RegularExpressions.Regex.IsMatch(s, @"价税合计[（(]大写[）)]")) { break; }//兼容"价税合计（大写）"和"价税合计(大写)"
                if (read) infos.Add(s);
            }

            var items = new List<InvoiceContentItem>();
            foreach (var infoStr in infos)
            {
                var info = infoStr.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                if (info.Count == 1 && items.Count > 0)//补充折行的项目名称
                {
                    items.Last().ItemName += info[0];
                }
                else if (info.Count == 7)
                {
                    items.Add(new InvoiceContentItem
                    {
                        ItemName = info[0],
                        Unit = info[1],
                        Quantity = info[2],
                        UnitPrice = info[3],
                        Amount = info[4],
                        TaxRate = info[5],
                        TaxAmount = info[6]
                    });
                }
                else if (info.Count == 8)
                {
                    items.Add(new InvoiceContentItem
                    {
                        ItemName = info[0],
                        Specifications = info[1],
                        Unit = info[2],
                        Quantity = info[3],
                        UnitPrice = info[4],
                        Amount = info[5],
                        TaxRate = info[6],
                        TaxAmount = info[7]
                    });
                }
                else if (info.Count == 4)
                {
                    invoiceItem.TotalAmount = info[2];
                    invoiceItem.TotalTax = info[3];
                }
            }
            invoiceItem.InvoiceContentItems = items;
        }

        private static string GetValueAfterColon(string s)
        {
            return s.Split(new[] { '：', ':' }).LastOrDefault()?.Trim();
        }
    }
}
