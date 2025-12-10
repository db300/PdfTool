# PdfConsoleApp - PDF 生成控制台应用

## 功能说明

该控制台应用支持通过 JSON 配置文件生成 PDF 文档，**完全支持中文字符**。

## 使用方法

```bash
PdfConsoleApp.exe <json文件路径>
```

### 示例

```bash
PdfConsoleApp.exe sample.json
```

## JSON 配置文件格式

```json
{
  "OutputFileName": "output.pdf",
  "TextContent": "这是 PDF 的文本内容。\n支持换行。",
  "FontName": "宋体",
  "FontSize": 12,
  "PageWidth": 595,
  "PageHeight": 842,
  "MarginLeft": 40,
  "MarginRight": 40,
  "MarginTop": 40,
  "MarginBottom": 40
}
```

## 参数说明

| 参数名 | 类型 | 必填 | 默认值 | 说明 |
|--------|------|------|--------|------|
| OutputFileName | string | 是 | - | 输出的 PDF 文件名 |
| TextContent | string | 是 | - | PDF 的文本内容，支持 \n 换行 |
| FontName | string | 否 | 宋体 | 字体名称 |
| FontSize | number | 否 | 12 | 字体大小（磅） |
| PageWidth | number | 否 | 595 | 页面宽度（点，A4 宽度） |
| PageHeight | number | 否 | 842 | 页面高度（点，A4 高度） |
| MarginLeft | number | 否 | 40 | 左边距（点） |
| MarginRight | number | 否 | 40 | 右边距（点） |
| MarginTop | number | 否 | 40 | 上边距（点） |
| MarginBottom | number | 否 | 40 | 下边距（点） |

## 中文支持

? **完全支持中文字符**

- 使用 Unicode 编码，确保中文正确显示
- 支持中文标点符号
- 支持中英文混合
- 推荐字体：宋体、微软雅黑、黑体、楷体

### 示例：中文内容

```json
{
  "OutputFileName": "chinese_test.pdf",
  "TextContent": "你好，世界！\n这是中文测试。",
  "FontName": "宋体",
  "FontSize": 14
}
```

## 常见纸张尺寸

| 纸张 | 宽度 | 高度 |
|------|------|------|
| A4 | 595 | 842 |
| A3 | 842 | 1191 |
| Letter | 612 | 792 |

注：尺寸单位为点（1英寸 = 72点）

## 推荐字体

### 中文字体

- **宋体** (SimSun) - 默认，兼容性最好 ?
- **微软雅黑** (Microsoft YaHei) - 现代化，适合标题
- **黑体** (SimHei) - 加粗效果
- **楷体** (KaiTi) - 艺术效果

### 英文字体

- **Arial** - 无衬线字体
- **Times New Roman** - 有衬线字体
- **Courier New** - 等宽字体

## 示例文件

项目中包含以下示例文件：

1. **sample.json** - 标准示例（中文多行文本）
2. **test_simple.json** - 最简配置
3. **test_multiline.json** - 多行文本（微软雅黑）
4. **test_custom_page.json** - 自定义页面尺寸
5. **test_chinese.json** - 中文字符测试

### 运行示例

```bash
# 基本示例
PdfConsoleApp.exe sample.json

# 中文测试
PdfConsoleApp.exe test_chinese.json

# 自定义页面
PdfConsoleApp.exe test_custom_page.json
```

## 技术说明

### 字体编码

- 使用 Unicode 编码（PdfFontEncoding.Unicode）
- 自动嵌入字体子集
- 确保跨平台兼容性

### 文本处理

- 支持 `\n` 换行符
- 支持 `\r\n` Windows 换行符
- 自动文本换行

## 故障排除

### 中文显示为乱码

? **已解决** - 已使用 Unicode 编码

### 字体不存在

确保系统中安装了指定的字体，或使用默认字体"宋体"

### 文件生成失败

检查输出路径是否有写入权限

## 更新日志

### 2025-12-10

- ? 修复中文乱码问题
- ? 添加 Unicode 编码支持
- ? 添加中文测试用例
- ? 更新文档
