# PdfConsoleApp 调试指南

## 快速开始

### 方法 1: 使用调试配置（推荐）?

已为您创建了 `Properties\launchSettings.json`，包含多个调试配置。

**使用步骤：**

1. 在 Visual Studio 中，找到工具栏上的启动按钮
2. 点击启动按钮旁边的下拉箭头
3. 选择以下配置之一：
   - **PdfConsoleApp** - 使用 test_chinese.json（中文测试）
   - **PdfConsoleApp (Sample)** - 使用 sample.json
   - **PdfConsoleApp (Simple)** - 使用 test_simple.json
   - **PdfConsoleApp (Multiline)** - 使用 test_multiline.json
   - **PdfConsoleApp (Custom Page)** - 使用 test_custom_page.json
4. 按 `F5` 开始调试

### 方法 2: 使用自动默认参数 ?

已在代码中添加了 DEBUG 模式支持：

```csharp
#if DEBUG
    // 调试时使用默认参数
    if (args.Length == 0)
    {
        args = new[] { "test_chinese.json" };
        Console.WriteLine($"调试模式：使用默认参数 {args[0]}");
    }
#endif
```

**使用步骤：**
1. 直接按 `F5` 开始调试
2. 程序会自动使用 `test_chinese.json` 作为参数
3. 无需任何配置！

### 方法 3: 手动设置命令行参数

1. 右键点击 **PdfConsoleApp** 项目
2. 选择 **"属性"**
3. 在左侧选择 **"调试"** 选项卡
4. 在 **"命令行参数"** 框中输入：
   ```
   test_chinese.json
   ```
5. 保存并按 `F5` 开始调试

---

## 调试技巧

### 1. 设置断点

在以下位置设置断点进行调试：

```csharp
// 行 27: 检查 JSON 文件路径
var jsonFilePath = args[0];  // ← 设置断点

// 行 36: 检查 JSON 内容
var jsonContent = File.ReadAllText(jsonFilePath, Encoding.UTF8);  // ← 设置断点

// 行 39: 检查解析结果
var param = ParseJson(jsonContent);  // ← 设置断点

// 行 60: 检查生成 PDF
PdfBuilder.GeneratePdf(param);  // ← 设置断点
```

### 2. 监视变量

在调试时添加以下监视表达式：

- `args` - 命令行参数
- `jsonFilePath` - JSON 文件路径
- `jsonContent` - JSON 文件内容
- `param.OutputFileName` - 输出文件名
- `param.TextContent` - PDF 文本内容
- `param.FontName` - 字体名称

### 3. 立即窗口

在调试时可以在立即窗口中执行：

```csharp
// 查看参数对象
? param

// 查看 JSON 内容
? jsonContent

// 查看输出文件名
? param.OutputFileName

// 查看文本内容
? param.TextContent
```

---

## 调试不同的测试用例

### 调试中文测试
```
命令行参数: test_chinese.json
```
测试内容：完整的中文字符测试

### 调试标准示例
```
命令行参数: sample.json
```
测试内容：多行中文文本

### 调试最简配置
```
命令行参数: test_simple.json
```
测试内容：最少参数配置

### 调试自定义页面
```
命令行参数: test_custom_page.json
```
测试内容：自定义页面尺寸和字体

---

## 调试 JSON 解析

如果想调试 JSON 解析过程：

1. 在 `ParseJson` 方法中设置断点：
   ```csharp
   private static PdfBuildParam ParseJson(string json)  // ← 行 73
   {
       var param = new PdfBuildParam();  // ← 设置断点
   ```

2. 逐步执行（F10）查看每个键值对的解析过程

3. 监视以下变量：
   - `pairs` - 分割后的键值对列表
   - `key` - 当前键
   - `value` - 当前值
   - `param` - 参数对象

---

## 调试 PDF 生成

如果想调试 PDF 生成过程：

1. 在 `PdfBuilder.cs` 中设置断点：
   ```csharp
   public static void GeneratePdf(PdfBuildParam param)
   {
       PdfHelperLibrary.Builder.Text2Pdf(  // ← 设置断点
   ```

2. 按 `F11` 单步进入 `PdfHelperLibrary.Builder.Text2Pdf` 方法

3. 在 `PdfHelperLibrary\Builder.cs` 中继续调试

---

## 输出文件位置

调试生成的 PDF 文件位置：
```
D:\GitHub\MyTools\PdfConsoleApp\bin\Debug\
```

文件名：
- `test_chinese.pdf`
- `output.pdf`
- `test_simple.pdf`
- 等等

---

## 常见调试场景

### 场景 1: JSON 文件找不到

**现象：** 提示 "错误: JSON 文件不存在"

**调试步骤：**
1. 在 `File.Exists(jsonFilePath)` 处设置断点
2. 检查 `jsonFilePath` 的值
3. 确认工作目录：在立即窗口输入 `? Directory.GetCurrentDirectory()`
4. 确认 JSON 文件是否在正确位置

**解决方案：**
- 使用绝对路径
- 或使用相对于 bin\Debug 的路径：`..\..\test_chinese.json`

### 场景 2: JSON 解析失败

**现象：** 提示 "错误: JSON 解析失败"

**调试步骤：**
1. 在 `ParseJson` 方法中设置断点
2. 检查 `json` 字符串的内容
3. 逐步执行，查看哪个键值对解析失败

### 场景 3: 中文显示乱码

**现象：** PDF 中中文显示为方框或乱码

**调试步骤：**
1. 在 `PdfHelperLibrary\Builder.cs` 的 `Text2Pdf` 方法中设置断点
2. 检查字体创建代码：
   ```csharp
   var options = new XPdfFontOptions(PdfFontEncoding.Unicode);
   var font = new XFont(fontName, fontSize, XFontStyle.Regular, options);
   ```
3. 确认使用了 Unicode 编码

---

## 快捷键

| 快捷键 | 功能 |
|--------|------|
| F5 | 开始调试 |
| F9 | 设置/取消断点 |
| F10 | 逐过程（不进入函数） |
| F11 | 逐语句（进入函数） |
| Shift+F11 | 跳出当前函数 |
| Ctrl+Shift+F9 | 删除所有断点 |
| F5 | 继续执行 |
| Shift+F5 | 停止调试 |

---

## 调试检查清单

调试前确认：
- ? test_chinese.json 文件存在
- ? 项目已编译（Build → Rebuild Solution）
- ? 设置了正确的启动项目（PdfConsoleApp）
- ? 选择了正确的调试配置

调试时检查：
- ? 断点被命中
- ? 变量值正确
- ? JSON 解析成功
- ? 参数验证通过
- ? PDF 生成成功

调试后验证：
- ? 控制台输出 "PDF 生成成功"
- ? bin\Debug 目录下有生成的 PDF 文件
- ? 打开 PDF 文件查看内容正确
- ? 中文字符正常显示

---

**更新日期:** 2025-12-10  
**适用版本:** PdfConsoleApp 1.0
