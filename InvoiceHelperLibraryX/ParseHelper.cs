using InvoiceHelperLibraryX.Entities;

namespace InvoiceHelperLibraryX
{
    /// <summary>
    /// 解析帮助类
    /// </summary>
    public static class ParseHelper
    {
        private const string InvoiceTypeTag1 = "电子发票（普通发票）";
        private const string InvoiceTypeTag11 = "电⼦发票（普通发票）";
        private const string InvoiceTypeTag12 = "电子发票(普通发票)";
        private const string InvoiceTypeTag2 = "电子发票（增值税专用发票）";
        private const string InvoiceTypeTag3 = "北京市社会团体会费统一票据（电子）";
        private const string InvoiceTypeTag4 = "北京市人民法院诉讼收费专用票据（电子）";
        private const string InvoiceTypeTag5 = "电子发票（铁路电子客票）";
        private const string InvoiceTypeTag6 = "电子发票（航空运输电子客票行程单）";

        /// <summary>
        /// 提取发票信息(读取流)
        /// 返回值: (是否成功, 错误信息, 发票实体)
        /// </summary>
        public static (bool, string, InvoiceItem?) Extract(Stream stream)
        {
            try
            {
                var tables = PdfHelperLibraryX3.TableHelper.Pdf2Table(stream, new List<int>(), new List<int>());
                var ss = PdfHelperLibraryX3.TextHelper.Pdf2String(stream);
                return Extract(tables, ss);
            }
            catch (Exception ex)
            {
                return (false, ex.Message, null);
            }
        }

        /// <summary>
        /// 提取发票信息(读取文件)
        /// 返回值: (是否成功, 错误信息, 发票实体)
        /// </summary>
        public static (bool, string, InvoiceItem?) Extract(string fileName)
        {
            try
            {
                var tables = PdfHelperLibraryX3.TableHelper.Pdf2Table(fileName, new List<int>(), new List<int>());
                var ss = PdfHelperLibraryX3.TextHelper.Pdf2String(fileName);
                return Extract(tables, ss);
            }
            catch (Exception ex)
            {
                return (false, ex.Message, null);
            }
        }

        private static (bool, string, InvoiceItem?) Extract(List<List<string>> tables, List<string> ss)
        {
            try
            {
                var invoiceType = GetInvoiceType(ss);
                var invoiceItem = new InvoiceItem { InvoiceType = invoiceType };
                switch (invoiceType)
                {
                    case InvoiceTypeTag5:
                        Extract4Railway(invoiceItem, ss);
                        return (true, "", invoiceItem);
                    case InvoiceTypeTag6:
                        Extract4AirPlane(invoiceItem, ss, tables);
                        return (true, "", invoiceItem);
                    case InvoiceTypeTag3:
                        Extract4SocialGroup(invoiceItem, ss);
                        return (true, "", invoiceItem);
                    case InvoiceTypeTag4:
                        Extract4Court(invoiceItem, tables);
                        return (true, "", invoiceItem);
                    case InvoiceTypeTag1:
                    case InvoiceTypeTag2:
                        Extract4Common(invoiceItem, ss, tables);
                        return (true, "", invoiceItem);
                    default:
                        Extract4Common(invoiceItem, ss, tables);
                        return (true, "", invoiceItem);
                }
            }
            catch (Exception ex)
            {
                return (false, ex.Message, null);
            }
        }

        private static string GetInvoiceType(List<string> ss)
        {
            foreach (var s in ss)
            {
                if (s.Contains("铁路电子客票")) return InvoiceTypeTag5;
                else if (s.Contains("航空运输电子客票行程单")) return InvoiceTypeTag6;
                else if (s.Contains(InvoiceTypeTag3)) return InvoiceTypeTag3;
                else if (s.Contains("诉讼费")) return InvoiceTypeTag4;
                else if (s.Contains(InvoiceTypeTag1)) return InvoiceTypeTag1;
                else if (s.Contains(InvoiceTypeTag2)) return InvoiceTypeTag2;
                else if (s.Contains(InvoiceTypeTag11)) return InvoiceTypeTag1;
                else if (s.Contains(InvoiceTypeTag12)) return InvoiceTypeTag1;
            }
            return "";
        }

        private static void Extract4Common(InvoiceItem invoiceItem, List<string> ss, List<List<string>> tables)
        {
            //读取发票号码和开票日期
            ExtractInvoiceBaseInfo(ss, invoiceItem);
            //读取购买方信息和销售方信息
            ExtractPartyInfo(tables.FirstOrDefault(), invoiceItem);
            //读取发票内容项
            ExtractContentItems(ss, invoiceItem);
            //检查补充金额信息
            if (string.IsNullOrWhiteSpace(invoiceItem.TotalAmount) || string.IsNullOrWhiteSpace(invoiceItem.TotalTax))
            {
                ExtractAmount(tables, invoiceItem);
            }
        }

        /// <summary>
        /// 提取铁路电子客票
        /// </summary>
        private static void Extract4Railway(InvoiceItem invoiceItem, List<string> ss)
        {
            foreach (var s in ss)
            {
                var infos = s.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                foreach (var info in infos)
                {
                    if (info.Contains("发票号码")) invoiceItem.InvoiceNumber = GetValueAfterColon(info);
                    else if (info.Contains("开票日期")) invoiceItem.InvoiceDate = GetValueAfterColon(info);
                    else if (info.Contains("购买方名称")) invoiceItem.BuyerName = GetValueAfterColon(info);
                    else if (info.Contains("统一社会信用代码")) invoiceItem.BuyerTaxNumber = GetValueAfterColon(info);
                    else if (info.Contains("票价")) invoiceItem.TotalAmount = GetValueAfterColon(info);
                }
            }
        }

        private static void Extract4AirPlane(InvoiceItem invoiceItem, List<string> ss, List<List<string>> tables)
        {
            foreach (var s in ss)
            {
                var infos = s.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                foreach (var info in infos)
                {
                    if (info.Contains("发票号码")) invoiceItem.InvoiceNumber = GetValueAfterColon(info);
                    else if (info.Contains("填开日期")) invoiceItem.InvoiceDate = GetValueAfterColon(info);
                    else if (info.Contains("购买方名称")) invoiceItem.BuyerName = GetValueAfterColon(info);
                    else if (info.Contains("统一社会信用代码/纳税人识别号")) invoiceItem.BuyerTaxNumber = GetValueAfterColon(info);
                }
            }
            double amount1 = 0, amount2 = 0, amount3 = 0;
            foreach (var table in tables)
            {
                table.RemoveAll(a => string.IsNullOrWhiteSpace(a));
                foreach (var row in table)
                {
                    var cells = row.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    if (cells[0] == "票价") amount1 = GetValueAmountAfterCNY(cells[1]);
                    else if (cells[0] == "燃油附加费") amount2 = GetValueAmountAfterCNY(cells[1]);
                    else if (cells[0] == "增值税税额") amount3 = GetValueAmountAfterCNY(cells[1]);
                }
            }
            invoiceItem.TotalAmount = (amount1 + amount2).ToString("F2");
            invoiceItem.TotalTax = amount3.ToString("F2");
        }

        /// <summary>
        /// 提取北京市社会团体会费统一票据 
        /// </summary>
        private static void Extract4SocialGroup(InvoiceItem invoiceItem, List<string> ss)
        {
            foreach (var s in ss)
            {
                var s1 = s.Replace(": ", ":");
                var infos = s1.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                foreach (var info in infos)
                {
                    if (info.Contains("票据号码")) invoiceItem.InvoiceNumber = GetValueAfterColon(info);
                    else if (info.Contains("开票日期")) invoiceItem.InvoiceDate = GetValueAfterColon(info);
                    else if (info.Contains("交款人统一社会信用代码")) invoiceItem.BuyerTaxNumber = GetValueAfterColon(info);
                    else if (info.Contains("交款人")) invoiceItem.BuyerName = GetValueAfterColon(info);
                    else if (info.Contains("小写")) invoiceItem.TotalAmount = GetValueAfterColon4Amount(info);
                }
            }
        }

        /// <summary>
        /// 提取北京市人民法院诉讼收费专用票据
        /// </summary>
        private static void Extract4Court(InvoiceItem invoiceItem, List<List<string>> tables)
        {
            foreach (var table in tables)
            {
                table.RemoveAll(a => string.IsNullOrWhiteSpace(a));
                foreach (var item in table)
                {
                    var ss = item.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    foreach (var s in ss)
                    {
                        var s1 = s.Replace("） ", "）");
                        var infos = s1.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                        foreach (var info in infos)
                        {
                            if (info.Contains("票据号码")) invoiceItem.InvoiceNumber = GetValueAfterColon(info);
                            else if (info.Contains("开票日期")) invoiceItem.InvoiceDate = GetValueAfterColon(info);
                            else if (info.Contains("交款人统一社会信用代码")) invoiceItem.BuyerTaxNumber = GetValueAfterColon(info);
                            else if (info.Contains("代交款人")) continue;
                            else if (info.Contains("交款人")) invoiceItem.BuyerName = GetValueAfterColon(info);
                            else if (info.Contains("小写")) invoiceItem.TotalAmount = GetValueAfterColon4Amount(info);
                        }
                    }
                }
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

        private static void ExtractAmount(List<List<string>> tables, InvoiceItem invoiceItem)
        {
            foreach (var table in tables)
            {
                table.RemoveAll(a => string.IsNullOrWhiteSpace(a));
                var rows = new List<string>();
                foreach (var item in table)
                {
                    rows.AddRange(item.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries));
                }
                foreach (var row in rows)
                {
                    if (row.Replace(" ", "").Contains("合计"))
                    {
                        var s = row.Replace("¥ ", "¥");
                        var amounts = s.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                        invoiceItem.TotalAmount = amounts[amounts.Count - 2];
                        invoiceItem.TotalTax = amounts[amounts.Count - 1];
                        return;
                    }
                }
            }
        }

        private static string GetValueAfterColon(string s)
        {
            return s.Split(new[] { '：', ':' }).LastOrDefault()?.Trim();
        }

        private static string GetValueAfterColon4Amount(string s)
        {
            return s.Split(new[] { ')', '）' }).LastOrDefault()?.Trim();
        }

        private static double GetValueAmountAfterCNY(string s)
        {
            return double.Parse(s.Replace("CNY", "").Trim());
        }
    }
}
