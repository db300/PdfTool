using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System;

namespace PdfHelperLibrary
{
    /// <summary>
    /// 保护帮助类
    /// </summary>
    public static class ProtectHelper
    {
        public static string Protect(string inputPdfFileName, string outputPdfFileName, string password)
        {
            try
            {
                // 打开输入PDF文档
                var inputDocument = PdfReader.Open(inputPdfFileName, PdfDocumentOpenMode.Import);
                // 创建一个新的PDF文档
                var outputDocument = new PdfDocument();
                // 复制页面到新的PDF文档
                foreach (var page in inputDocument.Pages) outputDocument.AddPage(page);
                // 设置安全设置
                var securitySettings = outputDocument.SecuritySettings;
                securitySettings.UserPassword = password;
                securitySettings.OwnerPassword = password;
                // 限制某些权限
                securitySettings.PermitAccessibilityExtractContent = false;//禁止提取内容
                securitySettings.PermitAnnotations = false;//禁止添加注释
                securitySettings.PermitAssembleDocument = false;//禁止拼接和拆分文档
                securitySettings.PermitExtractContent = false;//禁止提取内容
                securitySettings.PermitFormsFill = true;//允许填写表单
                securitySettings.PermitFullQualityPrint = false;//禁止高质量打印
                securitySettings.PermitModifyDocument = true;//允许修改文档
                securitySettings.PermitPrint = false;//禁止打印
                // 保存受保护的PDF文档
                outputDocument.Save(outputPdfFileName);
                return "";
            }
            catch (Exception ex)
            {
                return $"{inputPdfFileName} 保护失败，原因：{ex.Message}";
            }
        }

        public static string Unprotect(string inputPdfFileName, string outputPdfFileName, string password)
        {
            try
            {
                // 使用所有者密码打开受保护的PDF文档
                var inputDocument = PdfReader.Open(inputPdfFileName, password, PdfDocumentOpenMode.Import);
                // 创建一个新的PDF文档
                var outputDocument = new PdfDocument();
                // 复制页面到新的PDF文档
                foreach (var page in inputDocument.Pages) outputDocument.AddPage(page);
                // 清除安全设置
                var securitySettings = outputDocument.SecuritySettings;
                securitySettings.DocumentSecurityLevel = PdfSharp.Pdf.Security.PdfDocumentSecurityLevel.None;
                // 保存未受保护的PDF文档
                outputDocument.Save(outputPdfFileName);
                return "";
            }
            catch (Exception ex)
            {
                return $"{inputPdfFileName} 解除保护失败，原因：{ex.Message}";
            }
        }
    }
}
