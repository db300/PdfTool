# ? 调试配置完成

## ?? 现在您可以直接按 F5 调试了！

### ? 已配置的功能

#### 1. 自动默认参数（最简单）
```csharp
#if DEBUG
    if (args.Length == 0)
        args = new[] { @"..\..\test_chinese.json" };
#endif
```

**使用方式：**
- 直接按 `F5`
- 无需任何配置
- 自动使用 test_chinese.json

#### 2. 多个调试配置
已创建 `Properties\launchSettings.json`，包含 6 个配置：

| 配置名称 | JSON 文件 | 说明 |
|---------|----------|------|
| PdfConsoleApp | test_chinese.json | 中文字符测试 ? |
| PdfConsoleApp (Sample) | sample.json | 标准示例 |
| PdfConsoleApp (Simple) | test_simple.json | 最简配置 |
| PdfConsoleApp (Multiline) | test_multiline.json | 多行文本 |
| PdfConsoleApp (Custom Page) | test_custom_page.json | 自定义页面 |
| PdfConsoleApp (No Args) | (无) | 测试自动调试模式 |

**使用方式：**
1. 点击 Visual Studio 工具栏的启动按钮旁的下拉箭头
2. 选择配置
3. 按 `F5`

---

## ?? 快速调试步骤

### 方式 1: 最快速（推荐）
```
1. 打开 PdfConsoleApp\Program.cs
2. 按 F5
3. 完成！
```

自动使用 test_chinese.json 进行调试。

### 方式 2: 选择配置
```
1. 点击启动按钮旁的下拉箭头 ▼
2. 选择 "PdfConsoleApp"
3. 按 F5
4. 完成！
```

### 方式 3: 切换配置
想测试其他 JSON 文件：
```
1. 下拉箭头 ▼
2. 选择其他配置，如 "PdfConsoleApp (Sample)"
3. 按 F5
```

---

## ?? 推荐的断点位置

```csharp
// Program.cs - 主要流程
var jsonFilePath = args[0];                    // 行 27 - 检查参数
var param = ParseJson(jsonContent);            // 行 44 - 检查解析
PdfBuilder.GeneratePdf(param);                 // 行 69 - 生成 PDF

// 深入调试
private static PdfBuildParam ParseJson(...)    // 行 81 - JSON 解析
private static List<string> SplitJsonPairs(...)// 行 153 - 分割键值对
```

---

## ? 验证清单

调试前：
- ? test_chinese.json 文件存在于项目根目录
- ? 项目已编译成功
- ? 已设置 PdfConsoleApp 为启动项目

调试时：
- ? 控制台输出："调试模式：使用默认参数 ..\..\test_chinese.json"
- ? 断点被命中
- ? 变量值正确

调试后：
- ? 控制台输出："PDF 生成成功: test_chinese.pdf"
- ? bin\Debug\test_chinese.pdf 文件存在
- ? PDF 内容正确，中文无乱码

---

## ?? 文件结构

```
PdfConsoleApp\
├── Program.cs                      ← 主程序（已添加调试支持）
├── PdfBuilder.cs                   ← PDF 生成器
├── PdfBuildParam.cs               ← 参数实体
├── Properties\
│   └── launchSettings.json        ← 调试配置 ?
├── test_chinese.json              ← 中文测试 ?
├── sample.json                    ← 标准示例
├── test_simple.json               ← 最简配置
├── test_multiline.json            ← 多行文本
├── test_custom_page.json          ← 自定义页面
├── DEBUG_GUIDE.md                 ← 详细调试指南 ?
├── QUICK_DEBUG.md                 ← 快速调试卡片 ?
└── bin\Debug\
    └── *.pdf                      ← 生成的 PDF 文件
```

---

## ?? 调试技巧

### 查看变量
调试时按 `Alt + 1` 打开监视窗口，添加：
- `args`
- `param`
- `param.TextContent`
- `param.OutputFileName`

### 立即窗口
按 `Ctrl + Alt + I` 打开立即窗口，执行：
```csharp
? Directory.GetCurrentDirectory()
? File.Exists(jsonFilePath)
? param.TextContent
```

### 条件断点
右键断点 → 条件 → 输入条件：
```csharp
param.FontSize > 12
```

---

## ?? 测试命令

### 命令行测试
```bash
cd bin\Debug

# 测试默认参数（自动使用 test_chinese.json）
.\PdfConsoleApp.exe

# 测试指定文件
.\PdfConsoleApp.exe ..\..\sample.json
.\PdfConsoleApp.exe ..\..\test_simple.json
```

### PowerShell 批量测试
```powershell
cd bin\Debug
@("sample", "test_simple", "test_multiline", "test_custom_page", "test_chinese") | ForEach-Object {
    Write-Host "测试 $_.json..." -ForegroundColor Yellow
    .\PdfConsoleApp.exe "..\..\$_.json"
}
```

---

## ?? 总结

### ? 已完成
1. ? 添加 DEBUG 模式自动参数
2. ? 创建 launchSettings.json（6 个配置）
3. ? 使用正确的相对路径
4. ? 添加调试信息输出
5. ? 创建调试指南文档
6. ? 测试验证通过

### ?? 立即开始
**最简单的方式：直接按 F5！**

程序会自动：
- 使用 test_chinese.json
- 生成 test_chinese.pdf
- 显示调试信息

**无需任何手动配置！**

---

**配置完成时间:** 2025-12-10  
**配置人员:** GitHub Copilot  
**状态:** ? 可以开始调试了！
