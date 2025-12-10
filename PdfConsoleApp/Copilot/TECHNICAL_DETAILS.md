# 中文乱码修复 - 技术说明

## 问题根源

PdfSharp 库在处理字体时的默认行为：

```csharp
// 默认构造函数
public XFont(string familyName, double emSize)
{
    // 内部使用默认的 PdfFontEncoding.WinAnsi
    // WinAnsi 编码只支持西欧字符（0x00-0xFF）
    // 不支持中文（Unicode 范围：0x4E00-0x9FFF）
}
```

## 解决方案详解

### 完整的字体创建流程

```csharp
// 步骤 1: 创建字体选项对象
var options = new XPdfFontOptions(PdfFontEncoding.Unicode);

// 步骤 2: 使用选项创建字体
var font = new XFont(
    fontName,            // 字体名称，如 "宋体"
    fontSize,            // 字体大小，如 12
    XFontStyle.Regular,  // 字体样式
    options              // 编码选项（关键！）
);
```

### PdfFontEncoding 枚举值

```csharp
public enum PdfFontEncoding
{
    WinAnsi,          // Windows ANSI (CP1252) - 仅西欧字符
    Unicode,          // Unicode - 支持所有字符 ?
    WinAnsiEncoding,  // 同 WinAnsi
}
```

## 修复前后对比

### 修复前（有问题）
```csharp
var font = new XFont(fontName, fontSize);
// 结果: 中文显示为 ?? 或 □□
```

### 修复后（已修复）?
```csharp
var options = new XPdfFontOptions(PdfFontEncoding.Unicode);
var font = new XFont(fontName, fontSize, XFontStyle.Regular, options);
// 结果: 中文完美显示 你好世界
```

## 最佳实践

### ? 推荐做法
```csharp
// 1. 始终指定 Unicode 编码
var options = new XPdfFontOptions(PdfFontEncoding.Unicode);
var font = new XFont("宋体", 12, XFontStyle.Regular, options);

// 2. 使用系统内置字体
// Windows: 宋体、微软雅黑、黑体
```

### ? 避免的做法
```csharp
// 1. 不要使用默认构造函数（对中文内容）
var font = new XFont("宋体", 12);  // 会乱码！

// 2. 不要使用不存在的字体
var font = new XFont("不存在的字体", 12, ...);  // 会失败
```

---

**文档版本:** 1.0  
**创建日期:** 2025-12-10  
**作者:** GitHub Copilot
