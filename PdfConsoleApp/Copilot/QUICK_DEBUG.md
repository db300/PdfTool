# ?? 快速调试 - test_chinese.json

## 最简单的方法

### ? 推荐：直接按 F5
```
无需任何配置！
程序会自动在 DEBUG 模式下使用 test_chinese.json
```

**原理：** 
代码中已添加自动默认参数：
```csharp
#if DEBUG
    if (args.Length == 0)
        args = new[] { "test_chinese.json" };
#endif
```

---

## 其他方法

### 方法 1: 使用 launchSettings.json
在 Visual Studio 工具栏：
1. 点击启动按钮旁的下拉箭头 ▼
2. 选择 **"PdfConsoleApp"**
3. 按 F5

### 方法 2: 项目属性
1. 右键 PdfConsoleApp 项目 → 属性
2. 调试 → 命令行参数 → 输入：`test_chinese.json`
3. 保存 → F5

---

## 常用断点位置

```csharp
// Program.cs
var jsonFilePath = args[0];                    // 行 27
var param = ParseJson(jsonContent);            // 行 39
PdfBuilder.GeneratePdf(param);                 // 行 60

// PdfBuilder.cs
PdfHelperLibrary.Builder.Text2Pdf(...);       // 行 15

// Builder.cs (PdfHelperLibrary)
var options = new XPdfFontOptions(...);        // 行 72
```

---

## 验证步骤

1. ? 按 F5 启动调试
2. ? 控制台显示："调试模式：使用默认参数 test_chinese.json"
3. ? 控制台显示："PDF 生成成功: test_chinese.pdf"
4. ? 打开 `bin\Debug\test_chinese.pdf` 查看效果

---

**提示：** 在 DEBUG 模式下，无需任何配置即可调试！
