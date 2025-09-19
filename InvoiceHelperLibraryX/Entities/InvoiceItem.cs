namespace InvoiceHelperLibraryX.Entities
{
    /// <summary>
    /// 发票实体
    /// </summary>
    public class InvoiceItem
    {
        /// <summary>
        /// 发票类型
        /// </summary>
        public string InvoiceType { get; set; }
        /// <summary>
        /// 发票号码
        /// </summary>
        public string InvoiceNumber { get; set; }
        /// <summary>
        /// 开票日期
        /// </summary>
        public string InvoiceDate { get; set; }
        /// <summary>
        /// 购买方名称
        /// </summary>
        public string BuyerName { get; set; }
        /// <summary>
        /// 购买方税号
        /// </summary>
        public string BuyerTaxNumber { get; set; }
        /// <summary>
        /// 销售方名称
        /// </summary>
        public string SellerName { get; set; }
        /// <summary>
        /// 销售方税号
        /// </summary>
        public string SellerTaxNumber { get; set; }
        /// <summary>
        /// 开票内容列表
        /// </summary>
        public List<InvoiceContentItem> InvoiceContentItems { get; set; }
        /// <summary>
        /// 金额合计
        /// </summary>
        public string TotalAmount { get; set; }
        /// <summary>
        /// 税额合计
        /// </summary>
        public string TotalTax { get; set; }
        /// <summary>
        /// 价税合计（大写）
        /// </summary>
        public string TotalUpper { get; set; }
        /// <summary>
        /// 价税合计（小写）
        /// </summary>
        public string TotalLower { get; set; }
    }

    /// <summary>
    /// 开票内容实体
    /// </summary>
    public class InvoiceContentItem
    {
        /// <summary>
        /// 项目名称
        /// </summary>
        public string ItemName { get; set; }
        /// <summary>
        /// 规格型号
        /// </summary>
        public string Specifications { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
        public string Unit { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public string Quantity { get; set; }
        /// <summary>
        /// 单价
        /// </summary>
        public string UnitPrice { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public string Amount { get; set; }
        /// <summary>
        /// 税率/征收率
        /// </summary>
        public string TaxRate { get; set; }
        /// <summary>
        /// 税额
        /// </summary>
        public string TaxAmount { get; set; }
    }
}
