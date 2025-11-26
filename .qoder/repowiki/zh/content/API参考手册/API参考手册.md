# API参考手册

<cite>
**本文档中引用的文件**  
- [SplitHelper.cs](file://PdfHelperLibrary/SplitHelper.cs)
- [MergeHelper.cs](file://PdfHelperLibrary/MergeHelper.cs)
- [ExtractHelper.cs](file://PdfHelperLibrary/ExtractHelper.cs)
- [ImageExtractHelper.cs](file://PdfHelperLibrary/ImageExtractHelper.cs)
- [TextExtractHelper.cs](file://PdfHelperLibrary/TextExtractHelper.cs)
- [TableExtractHelper.cs](file://PdfHelperLibrary/TableExtractHelper.cs)
- [WatermarkHelper.cs](file://PdfHelperLibrary/WatermarkHelper.cs)
- [ProtectHelper.cs](file://PdfHelperLibrary/ProtectHelper.cs)
- [CompressHelper.cs](file://PdfHelperLibrary/CompressHelper.cs)
- [PageRotateHelper.cs](file://PdfHelperLibrary/PageRotateHelper.cs)
- [PrintHelper.cs](file://PdfHelperLibrary/PrintHelper.cs)
- [SealHelper.cs](file://PdfHelperLibrary/SealHelper.cs)
- [CommonHelper.cs](file://PdfHelperLibrary/CommonHelper.cs)
- [Builder.cs](file://PdfHelperLibrary/Builder.cs)
- [PdfHelperLibrary.csproj](file://PdfHelperLibrary/PdfHelperLibrary.csproj)
</cite>

## 目录
1. [简介](#简介)
2. [项目结构](#项目结构)
3. [核心组件](#核心组件)
4. [SplitHelper类](#splithelper类)
5. [MergeHelper类](#mergehelper类)
6. [ExtractHelper类](#extracthelper类)
7. [ImageExtractHelper类](#imageextracthelper类)
8. [TextExtractHelper类](#textextracthelper类)
9. [TableExtractHelper类](#tableextracthelper类)
10. [WatermarkHelper类](#watermarkhelper类)
11. [ProtectHelper类](#protecthelper类)
12. [CompressHelper类](#compresshelper类)
13. [PageRotateHelper类](#pagerotatehelper类)
14. [PrintHelper类](#printhelper类)
15. [SealHelper类](#sealhelper类)
16. [CommonHelper类](#commonhelper类)
17. [Builder类](#builder类)
18. [依赖项与外部库](#依赖项与外部库)
19. [使用示例](#使用示例)
20. [性能与线程安全](#性能与线程安全)
21. [已废弃方法](#已废弃方法)
22. [结论](#结论)

## 简介
本API参考手册详细介绍了PdfHelperLibrary系列库中公开的静态方法接口。文档为每个Helper类创建了独立条目，记录了其公共方法的签名、参数含义、返回值类型及可能抛出的异常。同时说明了方法的线程安全性、内存使用特征及性能瓶颈，并提供了C#调用示例，便于开发者在外部项目中集成PDF功能。

## 项目结构
PdfHelperLibrary是一个专注于PDF处理的.NET库，包含多个Helper类，每个类负责特定的PDF操作功能。项目基于.NET Framework 4.7.2构建，使用PDFsharp作为核心PDF处理引擎，并集成Tabula和PdfPig等第三方库用于表格和文本提取。

**文档来源**
- [PdfHelperLibrary.csproj](file://PdfHelperLibrary/PdfHelperLibrary.csproj)

## 核心组件
PdfHelperLibrary提供了全面的PDF处理功能，包括拆分、合并、提取、压缩、保护、水印、旋转等操作。所有Helper类均为静态类，提供简单易用的静态方法接口。

**文档来源**
- [PdfHelperLibrary.csproj](file://PdfHelperLibrary/PdfHelperLibrary.csproj)

## SplitHelper类
SplitHelper类提供PDF文档拆分功能。

### SplitPdf方法（单文件拆分）
```csharp
public static void SplitPdf(string inputPdfFileName)
```
- **参数**: 
  - `inputPdfFileName`: 输入PDF文件路径
- **功能**: 将PDF文档的每一页拆分为独立的PDF文件
- **输出**: 在原文件目录下生成"原文件名 - Page 页码.pdf"格式的文件
- **异常**: 拆分失败时抛出异常
- **线程安全**: 是
- **内存使用**: 处理大文件时内存消耗与文件大小成正比

**代码来源**
- [SplitHelper.cs](file://PdfHelperLibrary/SplitHelper.cs#L13-L30)

### SplitPdf方法（按页数拆分）
```csharp
public static string SplitPdf(string inputPdfFileName, int pageCountPerDoc)
```
- **参数**: 
  - `inputPdfFileName`: 输入PDF文件路径
  - `pageCountPerDoc`: 每个拆分后文档的页数
- **返回值**: 成功时返回空字符串，失败时返回错误信息
- **功能**: 按指定页数将PDF文档拆分为多个文件
- **输出**: 在原文件目录下生成"原文件名 - Page 页码范围.pdf"格式的文件
- **线程安全**: 是
- **内存使用**: 处理大文件时内存消耗与文件大小成正比

**代码来源**
- [SplitHelper.cs](file://PdfHelperLibrary/SplitHelper.cs#L37-L67)

## MergeHelper类
MergeHelper类提供PDF文档合并功能。

### MergePdf方法（带输出参数）
```csharp
public static string MergePdf(List<string> inputPdfFilenameList, bool autoOpen, bool addBookmarks, out string outputPdfFilename)
```
- **参数**: 
  - `inputPdfFilenameList`: 要合并的PDF文件路径列表
  - `autoOpen`: 是否自动打开合并后的文件
  - `addBookmarks`: 是否为每个源文件添加书签
  - `outputPdfFilename`: 输出文件路径（out参数）
- **返回值**: 成功时返回空字符串，失败时返回错误信息
- **功能**: 将多个PDF文件合并为一个文件
- **输出文件名**: "MergedFile - 时间戳.pdf"
- **线程安全**: 是
- **内存使用**: 内存消耗与所有输入文件总大小相关

**代码来源**
- [MergeHelper.cs](file://PdfHelperLibrary/MergeHelper.cs#L16-L41)

### MergePdf方法（元组返回）
```csharp
public static (string, string) MergePdf(List<string> inputPdfFilenameList, bool autoOpen, bool addBookmarks, string outputPdfFilename)
```
- **参数**: 
  - `inputPdfFilenameList`: 要合并的PDF文件路径列表
  - `autoOpen`: 是否自动打开合并后的文件
  - `addBookmarks`: 是否为每个源文件添加书签
  - `outputPdfFilename`: 输出文件路径，可为空
- **返回值**: (错误信息, 输出文件路径)元组
- **功能**: 将多个PDF文件合并为一个文件，支持自定义输出路径
- **线程安全**: 是
- **性能**: 当输出路径为空时，自动生成时间戳文件名

**代码来源**
- [MergeHelper.cs](file://PdfHelperLibrary/MergeHelper.cs#L44-L71)

## ExtractHelper类
ExtractHelper类提供PDF内容提取和页面删除功能。

### ExtractPdf方法
```csharp
public static string ExtractPdf(string inputPdfFileName, int pageFrom, int pageTo, out string outputPdfFileName)
```
- **参数**: 
  - `inputPdfFileName`: 输入PDF文件路径
  - `pageFrom`: 起始页码（从1开始）
  - `pageTo`: 结束页码
  - `outputPdfFileName`: 输出文件路径（out参数）
- **返回值**: 成功时返回空字符串，失败时返回错误信息
- **功能**: 提取指定页码范围的PDF页面
- **输出文件名**: "原文件名 - Page 起始页 to 结束页.pdf"
- **线程安全**: 是
- **内存使用**: 内存消耗与提取的页面数量成正比

**代码来源**
- [ExtractHelper.cs](file://PdfHelperLibrary/ExtractHelper.cs#L20-L39)

### DeletePdfPage方法
```csharp
public static string DeletePdfPage(string inputPdfFileName, List<int> pageNums, out string outputPdfFileName)
```
- **参数**: 
  - `inputPdfFileName`: 输入PDF文件路径
  - `pageNums`: 要删除的页码列表（从1开始）
  - `outputPdfFileName`: 输出文件路径（out参数）
- **返回值**: 成功时返回空字符串，失败时返回错误信息
- **功能**: 删除指定页码的PDF页面
- **输出文件名**: "原文件名 - DeletePageFile - 时间戳.pdf"
- **线程安全**: 是
- **性能**: 遍历所有页面进行过滤，时间复杂度O(n)

**代码来源**
- [ExtractHelper.cs](file://PdfHelperLibrary/ExtractHelper.cs#L47-L68)

## ImageExtractHelper类
ImageExtractHelper类提供PDF图片提取功能。

### ExportImage方法（单文件）
```csharp
public static string ExportImage(string inputPdfFileName)
```
- **参数**: 
  - `inputPdfFileName`: 输入PDF文件路径
- **返回值**: 成功时返回"提取 X 张图片."，失败时返回错误信息
- **功能**: 提取PDF文档中的所有图片
- **输出格式**: 根据图片编码格式输出为JPEG、PNG或TIFF文件
- **支持的编码**: DCTDecode (JPEG), FlateDecode (PNG), CCITTFaxDecode (TIFF)
- **线程安全**: 是
- **内存使用**: 处理大文件时内存消耗较高，建议分批处理

**代码来源**
- [ImageExtractHelper.cs](file://PdfHelperLibrary/ImageExtractHelper.cs#L17-L45)

### ExportImages方法（多文件）
```csharp
public static List<string> ExportImages(List<string> fileNames)
```
- **参数**: 
  - `fileNames`: PDF文件路径列表
- **返回值**: 提取的图片文件路径列表
- **功能**: 批量提取多个PDF文档中的图片
- **异常处理**: 单个文件处理失败不会影响其他文件
- **调试信息**: DEBUG模式下输出处理日志
- **线程安全**: 是
- **性能**: 顺序处理文件列表，建议在多线程环境中使用

**代码来源**
- [ImageExtractHelper.cs](file://PdfHelperLibrary/ImageExtractHelper.cs#L48-L87)

## TextExtractHelper类
TextExtractHelper类提供PDF文本提取功能。

### ExtractText方法
```csharp
public static List<string> ExtractText(string inputPdfFileName)
```
- **参数**: 
  - `inputPdfFileName`: 输入PDF文件路径
- **返回值**: 每页文本内容的字符串列表
- **功能**: 提取PDF文档中每一页的文本内容
- **底层库**: 使用UglyToad.PdfPig进行文本提取
- **线程安全**: 是
- **内存使用**: 内存消耗与文档总文本量相关
- **性能**: 对于复杂布局的PDF，文本提取可能不完整

**代码来源**
- [TextExtractHelper.cs](file://PdfHelperLibrary/TextExtractHelper.cs#L11-L32)

## TableExtractHelper类
TableExtractHelper类提供PDF表格提取功能。

### ExtractTable方法（Stream重载）
```csharp
public static (bool, string, List<PdfExtractTable>) ExtractTable(Stream inputPdfStrean)
```
- **参数**: 
  - `inputPdfStrean`: PDF文件流
- **返回值**: (是否成功, 错误信息, 表格数据列表)元组
- **功能**: 从PDF流中提取表格数据
- **底层库**: 使用Tabula进行表格识别
- **线程安全**: 是
- **内存使用**: 内存消耗与表格复杂度相关

**代码来源**
- [TableExtractHelper.cs](file://PdfHelperLibrary/TableExtractHelper.cs#L15-L21)

### ExtractTable方法（文件路径重载）
```csharp
public static (bool, string, List<PdfExtractTable>) ExtractTable(string inputPdfFileName)
```
- **参数**: 
  - `inputPdfFileName`: 输入PDF文件路径
- **返回值**: (是否成功, 错误信息, 表格数据列表)元组
- **功能**: 从PDF文件中提取表格数据
- **数据结构**: 返回PdfExtractTable对象列表，每个包含行和单元格数据
- **线程安全**: 是
- **性能**: 对于复杂表格，处理时间可能较长

**代码来源**
- [TableExtractHelper.cs](file://PdfHelperLibrary/TableExtractHelper.cs#L24-L31)

### ExtractTableRows方法
```csharp
public static (bool, string, List<PdfExtractRow>) ExtractTableRows(string inputPdfFileName)
```
- **参数**: 
  - `inputPdfFileName`: 输入PDF文件路径
- **返回值**: (是否成功, 错误信息, 表格行数据列表)元组
- **功能**: 提取PDF文档中的表格行数据，不保留表格结构
- **适用场景**: 当只需要表格内容而不需要结构信息时
- **线程安全**: 是
- **内存使用**: 比ExtractTable方法内存消耗略低

**代码来源**
- [TableExtractHelper.cs](file://PdfHelperLibrary/TableExtractHelper.cs#L42-L49)

## WatermarkHelper类
WatermarkHelper类提供PDF水印添加功能。

### WatermarkPdf方法
```csharp
public static string WatermarkPdf(string inputPdfFileName, string watermark)
```
- **参数**: 
  - `inputPdfFileName`: 输入PDF文件路径
  - `watermark`: 水印文本内容
- **返回值**: 成功时返回空字符串，失败时返回错误信息
- **功能**: 为PDF文档的每一页添加文本水印
- **水印样式**: 黑体，30pt，粗体斜体，红色半透明
- **位置**: 页面中心，倾斜角度与页面长宽比相关
- **输出文件**: "原文件名-WatermarkFile - 时间戳.pdf"
- **线程安全**: 是
- **内存使用**: 内存消耗与文档页数成正比

**代码来源**
- [WatermarkHelper.cs](file://PdfHelperLibrary/WatermarkHelper.cs#L25-L46)

## ProtectHelper类
ProtectHelper类提供PDF文档保护功能。

### Protect方法
```csharp
public static string Protect(string inputPdfFileName, string outputPdfFileName, string password)
```
- **参数**: 
  - `inputPdfFileName`: 输入PDF文件路径
  - `outputPdfFileName`: 输出PDF文件路径
  - `password`: 保护密码
- **返回值**: 成功时返回空字符串，失败时返回错误信息
- **功能**: 为PDF文档添加密码保护和权限限制
- **权限设置**: 
  - 禁止提取内容
  - 禁止添加注释
  - 禁止拼接和拆分文档
  - 禁止高质量打印
  - 禁止打印
  - 允许填写表单
  - 允许修改文档
- **线程安全**: 是
- **性能**: 处理速度与文档大小相关

**代码来源**
- [ProtectHelper.cs](file://PdfHelperLibrary/ProtectHelper.cs#L12-L38)

### Unprotect方法
```csharp
public static string Unprotect(string inputPdfFileName, string outputPdfFileName, string password)
```
- **参数**: 
  - `inputPdfFileName`: 输入受保护的PDF文件路径
  - `outputPdfFileName`: 输出PDF文件路径
  - `password`: 所有者密码
- **返回值**: 成功时返回空字符串，失败时返回错误信息
- **功能**: 移除PDF文档的密码保护
- **线程安全**: 是
- **注意事项**: 需要所有者密码才能解除保护

**代码来源**
- [ProtectHelper.cs](file://PdfHelperLibrary/ProtectHelper.cs#L45-L65)

## CompressHelper类
CompressHelper类提供PDF文档压缩功能。

### Compress方法
```csharp
public static string Compress(string inputFileName, string outputFileName)
```
- **参数**: 
  - `inputFileName`: 输入PDF文件路径
  - `outputFileName`: 输出PDF文件路径
- **返回值**: 成功时返回空字符串，失败时返回错误信息
- **功能**: 压缩PDF文档的内容流
- **压缩方式**: 启用内容流压缩
- **线程安全**: 是
- **性能**: 压缩效果取决于原始文档的内容和结构

**代码来源**
- [CompressHelper.cs](file://PdfHelperLibrary/CompressHelper.cs#L15-L30)

## PageRotateHelper类
PageRotateHelper类提供PDF页面旋转功能。

### RotatePdf方法（全部页面）
```csharp
public static string RotatePdf(string inputFilePath, out string outputFilePath, int rotationAngle)
```
- **参数**: 
  - `inputFilePath`: 输入PDF文件路径
  - `outputFilePath`: 输出文件路径（out参数）
  - `rotationAngle`: 旋转角度（90的倍数）
- **返回值**: 成功时返回空字符串，失败时返回错误信息
- **功能**: 将PDF文档的所有页面旋转指定角度
- **输出文件名**: "原文件名 - 旋转角度.pdf"
- **线程安全**: 是

**代码来源**
- [PageRotateHelper.cs](file://PdfHelperLibrary/PageRotateHelper.cs#L14-L36)

### RotatePdf方法（指定页面）
```csharp
public static string RotatePdf(string inputFilePath, out string outputFilePath, int rotationAngle, List<int> pageNums)
```
- **参数**: 
  - `inputFilePath`: 输入PDF文件路径
  - `outputFilePath`: 输出文件路径（out参数）
  - `rotationAngle`: 旋转角度（90的倍数）
  - `pageNums`: 要旋转的页码列表（从1开始）
- **返回值**: 成功时返回空字符串，失败时返回错误信息
- **功能**: 将PDF文档的指定页面旋转指定角度
- **页码验证**: 自动过滤无效页码
- **线程安全**: 是

**代码来源**
- [PageRotateHelper.cs](file://PdfHelperLibrary/PageRotateHelper.cs#L39-L64)

### RotatePdf方法（字典映射）
```csharp
public static string RotatePdf(string inputFilePath, string outputFilePath, Dictionary<int, int> pageRotateDict)
```
- **参数**: 
  - `inputFilePath`: 输入PDF文件路径
  - `outputFilePath`: 输出文件路径
  - `pageRotateDict`: 页面索引到旋转次数的字典（索引从0开始，旋转次数乘以90度）
- **返回值**: 成功时返回空字符串，失败时返回错误信息
- **功能**: 根据字典映射为PDF文档的页面设置不同的旋转角度
- **线程安全**: 是
- **性能**: 适用于需要复杂旋转配置的场景

**代码来源**
- [PageRotateHelper.cs](file://PdfHelperLibrary/PageRotateHelper.cs#L67-L92)

## PrintHelper类
PrintHelper类提供PDF文档打印功能。

### Print方法
```csharp
public static string Print(string inputPdfFileName)
```
- **参数**: 
  - `inputPdfFileName`: 要打印的PDF文件路径
- **返回值**: 成功时返回空字符串，失败时返回错误信息
- **功能**: 使用系统默认PDF阅读器打印文档
- **实现方式**: 通过Process.Start的"print"动词触发打印
- **线程安全**: 是
- **注意事项**: 依赖系统已安装的PDF阅读器

**代码来源**
- [PrintHelper.cs](file://PdfHelperLibrary/PrintHelper.cs#L11-L30)

## SealHelper类
SealHelper类提供PDF骑缝章功能。

### CrossPageSeal方法
```csharp
public static void CrossPageSeal(string sealImgFileName, string intputPdfFileName, double scale = 1.0)
```
- **参数**: 
  - `sealImgFileName`: 印章图片文件路径
  - `intputPdfFileName`: 输入PDF文件路径
  - `scale`: 印章图片缩放比例（默认1.0）
- **功能**: 在PDF文档的每一页上添加骑缝章效果
- **实现原理**: 将印章图片平均分割，每页显示一部分
- **输出文件名**: "原文件名_sealed.pdf"
- **位置**: 页面右下角
- **线程安全**: 是
- **性能**: 处理速度与文档页数和印章图片大小相关

**代码来源**
- [SealHelper.cs](file://PdfHelperLibrary/SealHelper.cs#L20-L47)

## CommonHelper类
CommonHelper类提供通用PDF辅助功能。

### GetPageCount方法
```csharp
public static int GetPageCount(string inputPdfFileName)
```
- **参数**: 
  - `inputPdfFileName`: PDF文件路径
- **返回值**: PDF文档的页数
- **功能**: 获取PDF文档的总页数
- **异常处理**: 
  - PdfReaderException: PDF读取异常
  - 其他异常: 文件访问异常
- **线程安全**: 是
- **性能**: 快速获取页数，无需加载整个文档

**代码来源**
- [CommonHelper.cs](file://PdfHelperLibrary/CommonHelper.cs#L11-L27)

## Builder类
Builder类提供PDF文档生成功能。

### Image2Pdf方法
```csharp
public static void Image2Pdf(List<string> imgFileList, string outFileName)
```
- **参数**: 
  - `imgFileList`: 图片文件路径列表
  - `outFileName`: 输出PDF文件路径
- **功能**: 将多个图片文件合并为一个PDF文档
- **页面方向**: 根据图片宽高比自动设置横向或纵向
- **填充方式**: 图片拉伸填充整个页面
- **线程安全**: 是
- **内存使用**: 内存消耗与图片总大小相关

**代码来源**
- [Builder.cs](file://PdfHelperLibrary/Builder.cs#L17-L33)

### InsertImage2Pdf方法
```csharp
public static void InsertImage2Pdf(string inFileName, List<string> imgFileList, string outFileName)
```
- **参数**: 
  - `inFileName`: 输入PDF文件路径
  - `imgFileList`: 要插入的图片文件路径列表
  - `outFileName`: 输出PDF文件路径
- **功能**: 将图片插入到现有PDF文档的末尾
- **页面方向**: 根据图片宽高比自动设置
- **线程安全**: 是
- **性能**: 适用于向现有文档追加图片内容

**代码来源**
- [Builder.cs](file://PdfHelperLibrary/Builder.cs#L36-L53)

## 依赖项与外部库
PdfHelperLibrary依赖多个外部库来实现完整的PDF处理功能：

- **PDFsharp**: 核心PDF处理引擎，用于PDF创建、修改、渲染
- **UglyToad.PdfPig**: 文本提取引擎，提供准确的文本内容提取
- **Tabula**: 表格提取引擎，专门用于识别和提取PDF中的表格数据
- **Microsoft.Bcl.HashCode**: .NET扩展库，提供哈希码功能
- **System.Memory**: .NET扩展库，提供内存和跨度操作支持

这些依赖项通过NuGet包管理器进行管理，确保版本兼容性和稳定性。

**代码来源**
- [PdfHelperLibrary.csproj](file://PdfHelperLibrary/PdfHelperLibrary.csproj)

## 使用示例
以下是在外部项目中使用PdfHelperLibrary的C#调用示例：

```csharp
// 添加引用
// using PdfHelperLibrary;

// 合并PDF文件
var filesToMerge = new List<string> { "file1.pdf", "file2.pdf" };
string outputFilePath;
var result = MergeHelper.MergePdf(filesToMerge, false, true, out outputFilePath);
if (string.IsNullOrEmpty(result))
{
    Console.WriteLine($"合并成功，输出文件：{outputFilePath}");
}
else
{
    Console.WriteLine($"合并失败：{result}");
}

// 提取PDF文本
var textList = TextExtractHelper.ExtractText("document.pdf");
foreach (var text in textList)
{
    Console.WriteLine(text);
}

// 为PDF添加水印
var watermarkResult = WatermarkHelper.WatermarkPdf("document.pdf", "机密");
if (string.IsNullOrEmpty(watermarkResult))
{
    Console.WriteLine("水印添加成功");
}
else
{
    Console.WriteLine($"水印添加失败：{watermarkResult}");
}

// 获取PDF页数
try
{
    var pageCount = CommonHelper.GetPageCount("document.pdf");
    Console.WriteLine($"文档共有 {pageCount} 页");
}
catch (Exception ex)
{
    Console.WriteLine($"获取页数失败：{ex.Message}");
}
```

## 性能与线程安全
### 性能特征
- **内存使用**: 大多数操作的内存消耗与处理的PDF文件大小成正比
- **大文件处理**: 建议对大文件进行分批处理，避免内存溢出
- **性能瓶颈**: 表格提取和图片提取是相对耗时的操作
- **并发处理**: 可以在多线程环境中并行处理多个独立的PDF文件

### 线程安全
- **静态方法**: 所有公共静态方法都是线程安全的
- **并发访问**: 可以在多线程环境中安全调用
- **文件锁定**: 操作期间会锁定输入文件，避免并发写入冲突
- **最佳实践**: 对于大量文件处理，建议使用线程池或任务并行库

## 已废弃方法
经分析，当前版本的PdfHelperLibrary中没有标记为已废弃的方法。所有公共方法均为有效且推荐使用的接口。

## 结论
PdfHelperLibrary提供了一套完整、易用的PDF处理API，涵盖了文档拆分、合并、提取、保护、水印等常见功能。所有方法设计简洁，返回值明确，便于集成到各种.NET应用程序中。通过合理使用这些API，开发者可以快速实现复杂的PDF处理需求。